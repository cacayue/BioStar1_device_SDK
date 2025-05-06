// UserManagement.cpp : implementation file
//

#include "stdafx.h"
#include "BioStarCPP.h"
#include "UserManagement.h"
#include "Util.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define TEMPLATE_SIZE		    384
#define FACETEMPLATE_SIZE	    2284
#define FACETEMPLATE_FST_SIZE	2000

/////////////////////////////////////////////////////////////////////////////
// CUserManagement dialog

CUserManagement::CUserManagement(CWnd* pParent /*=NULL*/)
	: CDialog(CUserManagement::IDD, pParent)
{
	//{{AFX_DATA_INIT(CUserManagement)
	m_AdminLevel = -1;
	m_AuthMode = -1;
	m_CardID = 0;
	m_CardType = -1;
	m_Checksum1 = 0;
	m_Checksum2 = 0;
	m_FaceChecksum = 0;
    m_FST_FaceNum = 0;
	m_CustomID = 0;
	m_Device = _T("");
	m_Duress1 = FALSE;
	m_Duress2 = FALSE;
	m_Finger1 = FALSE;
	m_Finger2 = FALSE;
	m_UserID = 0;
	m_Name = _T("");
    m_Password = _T("");
	m_NumOfFaceTemplate = 0;
	m_NumOfTemplate = 0;
	m_NumOfUser = 0;
	m_SecurityLevel = -1;
    m_AccessGroup = _T("");
	m_ExtRF = FALSE;
	//}}AFX_DATA_INIT

	m_TemplateData      = (unsigned char*)malloc( TEMPLATE_SIZE * 2 * BS_MAX_TEMPLATE_PER_USER );    // for finger template
	m_FaceTemplate_DST  = (unsigned char*)malloc( FACETEMPLATE_SIZE * BS_MAX_FACE_PER_USER );    // for D-Station face template
    m_FaceTemplate_FST  = (unsigned char*)malloc( FACETEMPLATE_FST_SIZE * BS_MAX_FACE_TEMPLATE ); // for FaceStation face template


    memset(&m_userTemplateHdr, 0x00, sizeof(FSUserTemplateHdr));
}

CUserManagement::~CUserManagement()
{
}

void CUserManagement::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CUserManagement)
	DDX_Control(pDX, IDC_USER_LIST, m_UserList);
	DDX_CBIndex(pDX, IDC_ADMIN_LEVEL, m_AdminLevel);
	DDX_CBIndex(pDX, IDC_AUTH_MODE, m_AuthMode);
	DDX_Text(pDX, IDC_CARD_ID, m_CardID);
	DDX_CBIndex(pDX, IDC_CARD_TYPE, m_CardType);
	DDX_Text(pDX, IDC_CHECKSUM1, m_Checksum1);
	DDX_Text(pDX, IDC_CHECKSUM2, m_Checksum2);
	DDX_Text(pDX, IDC_FACE_CHECKSUM, m_FaceChecksum);
    DDX_Text(pDX, IDC_FST_FACENUM, m_FST_FaceNum);
	DDX_Text(pDX, IDC_CUSTOM_ID, m_CustomID);
	DDX_Text(pDX, IDC_DEVICE, m_Device);
	DDX_Check(pDX, IDC_DURESS1, m_Duress1);
	DDX_Check(pDX, IDC_DURESS2, m_Duress2);
	DDX_Check(pDX, IDC_FINGER1, m_Finger1);
	DDX_Check(pDX, IDC_FINGER2, m_Finger2);
	DDX_Check(pDX, IDC_FACE_DST, m_Face_DStation);
    DDX_Check(pDX, IDC_FACE_FST, m_Face_FaceStation);
	DDX_Text(pDX, IDC_ID, m_UserID);
	DDX_Text(pDX, IDC_NAME, m_Name);
    DDX_Text(pDX, IDC_PASSWORD, m_Password);
	DDX_Text(pDX, IDC_NUM_OF_FACE_TEMPLATE, m_NumOfFaceTemplate);
	DDX_Text(pDX, IDC_NUM_OF_TEMPLATE, m_NumOfTemplate);
	DDX_Text(pDX, IDC_NUM_OF_USER, m_NumOfUser);
	DDX_CBIndex(pDX, IDC_SECURITY_LEVEL, m_SecurityLevel);
	DDX_DateTimeCtrl(pDX, IDC_START_DATE, m_StartDate);
	DDX_DateTimeCtrl(pDX, IDC_EXPIRY_DATE, m_ExpiryDate);
	DDX_Text(pDX, IDC_ACCESS_GROUP, m_AccessGroup);
	DDX_Check(pDX, IDC_CHECK_RF, m_ExtRF);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CUserManagement, CDialog)
	//{{AFX_MSG_MAP(CUserManagement)
	ON_NOTIFY(NM_CLICK, IDC_USER_LIST, OnClickUserList)
	ON_BN_CLICKED(IDC_DELETE, OnDelete)
	ON_BN_CLICKED(IDC_DELETE_ALL, OnDeleteAll)
	ON_BN_CLICKED(IDC_REFRESH, OnRefresh)
	ON_BN_CLICKED(IDC_UPDATE, OnUpdate)
	ON_BN_CLICKED(IDC_ADD, OnAdd)
	ON_BN_CLICKED(IDC_READ_CARD, OnReadCard)
	ON_WM_DESTROY()
	//}}AFX_MSG_MAP
    ON_BN_CLICKED(IDC_FST_SCAN_FACE, &CUserManagement::OnBnClickedFstScanFace)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CUserManagement message handlers

BOOL CUserManagement::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	// TODO: Add extra initialization here
	DWORD style = LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES;
	m_UserList.SendMessage(LVM_SETEXTENDEDLISTVIEWSTYLE, 0, LPARAM(style));

	m_UserList.InsertColumn( 1, "ID", LVCFMT_LEFT, 70, 0 );
	m_UserList.InsertColumn( 2, "Finger", LVCFMT_CENTER, 60, 1  );
	m_UserList.InsertColumn( 3, "Face", LVCFMT_CENTER, 60, 1  );
	m_UserList.InsertColumn( 4, "Card ID", LVCFMT_CENTER, 80, 2  );
	
	// TODO: Add extra initialization here
	char buf[32];

	sprintf( buf, "%d.%d.%d.%d(%u)", m_DeviceAddr & 0xff, (m_DeviceAddr & 0xff00) >> 8, 
		(m_DeviceAddr & 0xff0000) >> 16, (m_DeviceAddr & 0xff000000) >> 24, m_DeviceID );

	m_Device = buf;

	if( !getUserInfo() )
	{
		MessageBox( "Cannot get user info" );
	}

    FillAuthmodeData();
    SetControlState();

	return TRUE;  
}

void CUserManagement::SetControlState()
{
    GetDlgItem(IDC_FINGER1)->EnableWindow(FALSE);
    GetDlgItem(IDC_FINGER2)->EnableWindow(FALSE);
    GetDlgItem(IDC_DURESS1)->EnableWindow(FALSE);
    GetDlgItem(IDC_DURESS2)->EnableWindow(FALSE);
    GetDlgItem(IDC_FACE_DST)->EnableWindow(FALSE);
    GetDlgItem(IDC_FACE_FST)->EnableWindow(FALSE);
    GetDlgItem(IDC_STATIC_FACENUM)->EnableWindow(FALSE);
    GetDlgItem(IDC_FST_SCAN_FACE)->EnableWindow(FALSE);

	switch (m_DeviceType)
    {
        case BS_DEVICE_BIOSTATION2:
        case BS_DEVICE_BIOSTATION:
        case BS_DEVICE_BIOENTRY_PLUS:
        case BS_DEVICE_BIOENTRY_W:
        case BS_DEVICE_BIOLITE:
            GetDlgItem(IDC_FINGER1)->EnableWindow(TRUE);
            GetDlgItem(IDC_FINGER2)->EnableWindow(TRUE);
            GetDlgItem(IDC_DURESS1)->EnableWindow(TRUE);
            GetDlgItem(IDC_DURESS2)->EnableWindow(TRUE);
            break;
        case BS_DEVICE_DSTATION:
            GetDlgItem(IDC_FINGER1)->EnableWindow(TRUE);
            GetDlgItem(IDC_FINGER2)->EnableWindow(TRUE);
            GetDlgItem(IDC_DURESS1)->EnableWindow(TRUE);
            GetDlgItem(IDC_DURESS2)->EnableWindow(TRUE);
            GetDlgItem(IDC_FACE_DST)->EnableWindow(TRUE);
            break;
        case BS_DEVICE_FSTATION:
            GetDlgItem(IDC_FACE_FST)->EnableWindow(TRUE);
            GetDlgItem(IDC_STATIC_FACENUM)->EnableWindow(TRUE);
            GetDlgItem(IDC_FST_SCAN_FACE)->EnableWindow(TRUE);
            break;
        case BS_DEVICE_XPASS:
        case BS_DEVICE_XPASS_SLIM:
        case BS_DEVICE_XPASS_SLIM2:
        case BS_DEVICE_XSTATION:
        default:
	        break;
    }
}

void CUserManagement::FillAuthmodeData()
{
    CComboBox *pCombo = (CComboBox*)GetDlgItem(IDC_AUTH_MODE);
    if (!pCombo) return;

    pCombo->ResetContent();

	switch (m_DeviceType)
    {
        case BS_DEVICE_FSTATION:
            pCombo->AddString("Face Only");
            pCombo->AddString("Face and Password");
            pCombo->AddString("Card Only");
            pCombo->AddString("Card and Password");
            pCombo->AddString("Card and Face");
            pCombo->AddString("Card and Face/Password");
            pCombo->SetCurSel(0);
            break;

        case BS_DEVICE_BIOSTATION2:
        case BS_DEVICE_BIOSTATION:
        case BS_DEVICE_BIOENTRY_PLUS:
        case BS_DEVICE_BIOENTRY_W:
        case BS_DEVICE_BIOLITE:
        case BS_DEVICE_DSTATION:
        case BS_DEVICE_XPASS:
        case BS_DEVICE_XPASS_SLIM:
        case BS_DEVICE_XPASS_SLIM2:
        case BS_DEVICE_XSTATION:
        default:
            pCombo->AddString("Disabled");
            pCombo->AddString("Finger Only");
            pCombo->AddString("Finger and Password");
            pCombo->AddString("Finger or Password");
            pCombo->AddString("Password Only");
            pCombo->AddString("Card Only");
            pCombo->SetCurSel(1);
            break;
    }
}

void CUserManagement::setDevice( int handle, unsigned deviceID, unsigned deviceAddr, int deviceType )
{
	m_Handle = handle;
	m_DeviceID = deviceID;
	m_DeviceAddr = deviceAddr;
	m_DeviceType = deviceType;
}

bool CUserManagement::getUserInfo()
{
	m_NumOfUser = 0;
	m_NumOfTemplate = 0;
	m_NumOfFaceTemplate = 0;

	m_UserList.DeleteAllItems();

	BeginWaitCursor();

	if( m_DeviceType == BS_DEVICE_BIOENTRY_PLUS || 
		m_DeviceType == BS_DEVICE_BIOENTRY_W	|| 
        m_DeviceType == BS_DEVICE_BIOLITE       || 
        m_DeviceType == BS_DEVICE_XPASS         || 
        m_DeviceType == BS_DEVICE_XPASS_SLIM    || 
        m_DeviceType == BS_DEVICE_XPASS_SLIM2)
	{
		BEUserHdr* userHdr = NULL;

		BS_RET_CODE result = BS_GetUserDBInfo( m_Handle, (int*)&m_NumOfUser, (int*)&m_NumOfTemplate );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			return false;
		}

		userHdr = (BEUserHdr*)malloc( sizeof(BEUserHdr) * m_NumOfUser );

		result = BS_GetAllUserInfoBEPlus( m_Handle, userHdr, (int*)&m_NumOfUser );

		if( result != BS_SUCCESS && result != BS_ERR_NOT_FOUND )
		{
			EndWaitCursor();
			free( userHdr );
			return false;
		}

		for( int i = 0; i < m_NumOfUser; i++ )
		{
			CString value;
			value.Format( "%10u", userHdr[i].userID );
			int listIndex = m_UserList.InsertItem( LVIF_TEXT | LVIF_PARAM, i, value, 0, 0, 0, userHdr[i].userID );

			if( listIndex != -1 )
			{
				value.Format( "%d", userHdr[i].numOfFinger );
				m_UserList.SetItem( listIndex, 1, LVIF_TEXT, value, 0, 0, 0, 0 );

				value.Format( "%d", 0 );
				m_UserList.SetItem( listIndex, 2, LVIF_TEXT, value, 0, 0, 0, 0 );

				value.Format( "%#x", userHdr[i].cardID );
				m_UserList.SetItem( listIndex, 3, LVIF_TEXT, value, 0, 0, 0, 0 );
			}
		}

		UpdateData( FALSE );

		EndWaitCursor();

		if( m_NumOfUser > 0 )
		{
			getUserInfo( userHdr[0].userID );
		}

		free( userHdr );
	}
	else if( m_DeviceType == BS_DEVICE_BIOSTATION )
	{
		BSUserHdrEx* userHdr = NULL;

		BS_RET_CODE result = BS_GetUserDBInfo( m_Handle, (int*)&m_NumOfUser, (int*)&m_NumOfTemplate );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			return false;
		}

		userHdr = (BSUserHdrEx*)malloc( sizeof(BSUserHdrEx) * m_NumOfUser );

		result = BS_GetAllUserInfoEx( m_Handle, userHdr, (int*)&m_NumOfUser );

		if( result != BS_SUCCESS && result != BS_ERR_NOT_FOUND )
		{
			EndWaitCursor();
			free( userHdr );
			return false;
		}

		for( int i = 0; i < m_NumOfUser; i++ )
		{
			CString value;
			value.Format( "%10u", userHdr[i].ID );
			int listIndex = m_UserList.InsertItem( LVIF_TEXT | LVIF_PARAM, i, value, 0, 0, 0, userHdr[i].ID );

			if( listIndex != -1 )
			{
				value.Format( "%d", userHdr[i].numOfFinger );
				m_UserList.SetItem( listIndex, 1, LVIF_TEXT, value, 0, 0, 0, 0 );

				value.Format( "%d", 0 );
				m_UserList.SetItem( listIndex, 2, LVIF_TEXT, value, 0, 0, 0, 0 );

				value.Format( "%#x", userHdr[i].cardID );
				m_UserList.SetItem( listIndex, 3, LVIF_TEXT, value, 0, 0, 0, 0 );
			}
		}

		UpdateData( FALSE );

		EndWaitCursor();

		if( m_NumOfUser > 0 )
		{
			getUserInfo( userHdr[0].ID );
		}

		free( userHdr );
	}
	else if( m_DeviceType == BS_DEVICE_DSTATION )
	{
		DSUserHdr* userHdr = NULL;

		// Get count of finger template
		BS_RET_CODE result = BS_GetUserDBInfo( m_Handle, (int*)&m_NumOfUser, (int*)&m_NumOfTemplate );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			return false;
		}

		// Get count of face template
		result = BS_GetUserFaceInfo( m_Handle, (int*)&m_NumOfUser, (int*)&m_NumOfFaceTemplate );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			return false;
		}

		userHdr = (DSUserHdr*)malloc( sizeof(DSUserHdr) * m_NumOfUser );

		result = BS_GetAllUserInfoDStation( m_Handle, userHdr, (int*)&m_NumOfUser );

		if( result != BS_SUCCESS && result != BS_ERR_NOT_FOUND )
		{
			EndWaitCursor();
			free( userHdr );
			return false;
		}

		for( int i = 0; i < m_NumOfUser; i++ )
		{
			CString value;
			value.Format( "%10u", userHdr[i].ID );
			int listIndex = m_UserList.InsertItem( LVIF_TEXT | LVIF_PARAM, i, value, 0, 0, 0, userHdr[i].ID );

			if( listIndex != -1 )
			{
				value.Format( "%d", userHdr[i].numOfFinger );
				m_UserList.SetItem( listIndex, 1, LVIF_TEXT, value, 0, 0, 0, 0 );

				value.Format( "%d", userHdr[i].numOfFace );
				m_UserList.SetItem( listIndex, 2, LVIF_TEXT, value, 0, 0, 0, 0 );

				value.Format( "%#x", userHdr[i].cardID );
				m_UserList.SetItem( listIndex, 3, LVIF_TEXT, value, 0, 0, 0, 0 );
			}
		}

		UpdateData( FALSE );

		EndWaitCursor();

		if( m_NumOfUser > 0 )
		{
			getUserInfo( userHdr[0].ID );
		}

		free( userHdr );
	}
    else if( m_DeviceType == BS_DEVICE_XSTATION )
	{
		XSUserHdr* userHdr = NULL;

		// Get count of finger template
		BS_RET_CODE result = BS_GetUserDBInfo( m_Handle, (int*)&m_NumOfUser, (int*)&m_NumOfTemplate );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			return false;
		}

		userHdr = (XSUserHdr*)malloc( sizeof(XSUserHdr) * m_NumOfUser );

		result = BS_GetAllUserInfoXStation( m_Handle, userHdr, (int*)&m_NumOfUser );

		if( result != BS_SUCCESS && result != BS_ERR_NOT_FOUND )
		{
			EndWaitCursor();
			free( userHdr );
			return false;
		}

		for( int i = 0; i < m_NumOfUser; i++ )
		{
			CString value;
			value.Format( "%10u", userHdr[i].ID );
			int listIndex = m_UserList.InsertItem( LVIF_TEXT | LVIF_PARAM, i, value, 0, 0, 0, userHdr[i].ID );

			if( listIndex != -1 )
			{
				value.Format( "%d", userHdr[i].numOfFinger );
				m_UserList.SetItem( listIndex, 1, LVIF_TEXT, value, 0, 0, 0, 0 );

				value.Format( "%d", userHdr[i].numOfFace );
				m_UserList.SetItem( listIndex, 2, LVIF_TEXT, value, 0, 0, 0, 0 );

				value.Format( "%#x", userHdr[i].cardID );
				m_UserList.SetItem( listIndex, 3, LVIF_TEXT, value, 0, 0, 0, 0 );
			}
		}

		UpdateData( FALSE );

		EndWaitCursor();

		if( m_NumOfUser > 0 )
		{
			getUserInfo( userHdr[0].ID );
		}

		free( userHdr );
	}
	else if( m_DeviceType == BS_DEVICE_BIOSTATION2 )
	{
		BS2UserHdr* userHdr = NULL;

		// Get count of finger template
		BS_RET_CODE result = BS_GetUserDBInfo( m_Handle, (int*)&m_NumOfUser, (int*)&m_NumOfTemplate );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			return false;
		}

		userHdr = (BS2UserHdr*)malloc( sizeof(BS2UserHdr) * m_NumOfUser );

		result = BS_GetAllUserInfoBioStation2( m_Handle, userHdr, (int*)&m_NumOfUser );

		if( result != BS_SUCCESS && result != BS_ERR_NOT_FOUND )
		{
			EndWaitCursor();
			free( userHdr );
			return false;
		}

		for( int i = 0; i < m_NumOfUser; i++ )
		{
			CString value;
			value.Format( "%10u", userHdr[i].ID );
			int listIndex = m_UserList.InsertItem( LVIF_TEXT | LVIF_PARAM, i, value, 0, 0, 0, userHdr[i].ID );

			if( listIndex != -1 )
			{
				value.Format( "%d", userHdr[i].numOfFinger );
				m_UserList.SetItem( listIndex, 1, LVIF_TEXT, value, 0, 0, 0, 0 );

				value.Format( "%d", userHdr[i].numOfFace );
				m_UserList.SetItem( listIndex, 2, LVIF_TEXT, value, 0, 0, 0, 0 );

				value.Format( "%#x", userHdr[i].cardID );
				m_UserList.SetItem( listIndex, 3, LVIF_TEXT, value, 0, 0, 0, 0 );
			}
		}

		UpdateData( FALSE );

		EndWaitCursor();

		if( m_NumOfUser > 0 )
		{
			getUserInfo( userHdr[0].ID );
		}

		free( userHdr );
	}
	else if( m_DeviceType == BS_DEVICE_FSTATION )
	{
        int nUserID = 0;

	    FSUserHdr* userHdr = NULL;

	    // Get count of finger template
	    BS_RET_CODE result = BS_GetUserDBInfo( m_Handle, (int*)&m_NumOfUser, (int*)&m_NumOfTemplate );

	    if( result != BS_SUCCESS )
	    {
		    EndWaitCursor();
		    return false;
	    }

	    // Get count of face template
	    result = BS_GetUserFaceInfo( m_Handle, (int*)&m_NumOfUser, (int*)&m_NumOfFaceTemplate );

	    if( result != BS_SUCCESS )
	    {
		    EndWaitCursor();
		    return false;
	    }

	    userHdr = (FSUserHdr*)malloc( sizeof(FSUserHdr) * m_NumOfUser );

	    result = BS_GetAllUserInfoFStation( m_Handle, userHdr, (int*)&m_NumOfUser );

	    if( result != BS_SUCCESS && result != BS_ERR_NOT_FOUND )
	    {
		    EndWaitCursor();
		    free( userHdr );
		    return false;
	    }

	    for( int i = 0; i < m_NumOfUser; i++ )
	    {
		    CString value;
		    value.Format( "%10u", userHdr[i].ID );
		    int listIndex = m_UserList.InsertItem( LVIF_TEXT | LVIF_PARAM, i, value, 0, 0, 0, userHdr[i].ID );

		    if( listIndex != -1 )
		    {
			    value.Format( "%d", 0 );    // FaceStation support no Fingerprint Template
			    m_UserList.SetItem( listIndex, 1, LVIF_TEXT, value, 0, 0, 0, 0 );

			    value.Format( "%d", userHdr[i].numOfFace );
			    m_UserList.SetItem( listIndex, 2, LVIF_TEXT, value, 0, 0, 0, 0 );

			    value.Format( "%#x", userHdr[i].cardID );
			    m_UserList.SetItem( listIndex, 3, LVIF_TEXT, value, 0, 0, 0, 0 );
		    }
	    }

        nUserID = userHdr[0].ID;

        free( userHdr );

		UpdateData( FALSE );

		EndWaitCursor();

		if( m_NumOfUser > 0 )
		{
			getUserInfo( nUserID );
		}
	}

	return true;	
}


bool CUserManagement::getUserInfo( unsigned userID )
{
	m_CardID = 0;
	m_Checksum1 = 0;
	m_Checksum2 = 0;
	m_CustomID = 0;
	m_Finger1 = FALSE;
	m_Finger2 = FALSE;
	m_Duress1 = FALSE;
	m_Duress2 = FALSE;
	m_Face_DStation = FALSE;
    m_Face_FaceStation = FALSE;

    m_UserID = 0;
	m_Name = "";
    m_Password = "";
	m_AccessGroup = "";

    m_StartDate = COleDateTime::GetCurrentTime();
    m_ExpiryDate = COleDateTime::GetCurrentTime();

	m_AdminLevel = 0;
	m_SecurityLevel = 0;
	m_CardType = 0;
	m_AuthMode = 0;

	memset(m_TemplateData, 0x00, TEMPLATE_SIZE * 2 * BS_MAX_TEMPLATE_PER_USER);
	memset(m_FaceTemplate_DST, 0x00, FACETEMPLATE_SIZE * BS_MAX_FACE_PER_USER);
    memset(m_FaceTemplate_FST, 0x00, FACETEMPLATE_FST_SIZE * BS_MAX_FACE_TEMPLATE);

	BeginWaitCursor();

	if( m_DeviceType == BS_DEVICE_BIOENTRY_PLUS || 
		m_DeviceType == BS_DEVICE_BIOENTRY_W	|| 
        m_DeviceType == BS_DEVICE_BIOLITE       || 
        m_DeviceType == BS_DEVICE_XPASS         || 
        m_DeviceType == BS_DEVICE_XPASS_SLIM    || 
        m_DeviceType == BS_DEVICE_XPASS_SLIM2)
	{
		BEUserHdr userHdr;
		BS_RET_CODE result = BS_GetUserBEPlus( m_Handle, userID, &userHdr, m_TemplateData );

		EndWaitCursor();

		if( result != BS_SUCCESS )
		{
			UpdateData( FALSE );
			return false;
		}

		m_CardID = userHdr.cardID;
		m_Checksum1 = userHdr.fingerChecksum[0];
		m_Checksum2 = userHdr.fingerChecksum[1];
		m_CustomID = userHdr.cardCustomID;
		m_Finger1 = (userHdr.numOfFinger >= 1)? TRUE : FALSE;
		m_Finger2 = (userHdr.numOfFinger >= 2)? TRUE : FALSE;
		m_Duress1 = userHdr.isDuress[0];
		m_Duress2 = userHdr.isDuress[1];
		m_UserID = userHdr.userID;
		
        char szPassword[32] = {0};
        BS_UTF16ToString(userHdr.password, szPassword);
        m_Password = szPassword;

		TIME_ZONE_INFORMATION timeInfo;
		GetTimeZoneInformation( &timeInfo );


        time_t nStart = ConvertToUTCTime( userHdr.startTime );
        ConvertTimeToDate(nStart, m_StartDate);

        time_t nEnd = ConvertToUTCTime( userHdr.expiryTime );
        ConvertTimeToDate(nEnd, m_ExpiryDate);

		m_AdminLevel = userHdr.adminLevel;
		m_SecurityLevel = userHdr.securityLevel;
		m_CardType = userHdr.cardFlag;

		if( userHdr.opMode == BS_AUTH_MODE_DISABLED )
		{
			m_AuthMode = 0;
		}
		else
		{
			m_AuthMode -= BS_AUTH_FINGER_ONLY + 1;
		}

		char buf[16];
		sprintf( buf, "%#0x", userHdr.accessGroupMask );
		m_AccessGroup = buf;
		
	}
	else if( m_DeviceType == BS_DEVICE_BIOSTATION )
	{
		BSUserHdrEx userHdr;
		BS_RET_CODE result = BS_GetUserEx( m_Handle, userID, &userHdr, m_TemplateData );

		EndWaitCursor();

		if( result != BS_SUCCESS )
		{
			UpdateData( FALSE );
			return false;
		}

		m_CardID = userHdr.cardID;
		m_Checksum1 = userHdr.checksum[0];
		m_Checksum2 = userHdr.checksum[1];
		m_CustomID = userHdr.customID;
		m_Finger1 = (userHdr.numOfFinger >= 1)? TRUE : FALSE;
		m_Finger2 = (userHdr.numOfFinger >= 2)? TRUE : FALSE;
		m_Duress1 = userHdr.duressMask & 0x01;
		m_Duress2 = userHdr.duressMask & 0x02;
		m_UserID = userHdr.ID;

        char szName[64] = {0};
		BS_UTF8ToString(userHdr.name, szName);
        m_Name = szName;

        char szPassword[32] = {0};
        BS_UTF8ToString(userHdr.password, szPassword);
        m_Password = szPassword;
		
		TIME_ZONE_INFORMATION timeInfo;
		GetTimeZoneInformation( &timeInfo );

        time_t nStart = ConvertToUTCTime( userHdr.startDateTime );
        ConvertTimeToDate(nStart, m_StartDate);

        time_t nEnd = ConvertToUTCTime( userHdr.expireDateTime );
        ConvertTimeToDate(nEnd, m_ExpiryDate);

		m_AdminLevel = (userHdr.adminLevel == BS_USER_ADMIN)? 1 : 0;
		m_SecurityLevel = (userHdr.securityLevel >= BS_USER_SECURITY_DEFAULT)? userHdr.securityLevel - BS_USER_SECURITY_DEFAULT : 0;
		m_CardType = userHdr.bypassCard;

		if( userHdr.authMode == BS_AUTH_MODE_DISABLED )
		{
			m_AuthMode = 0;
		}
		else
		{
			m_AuthMode -= BS_AUTH_FINGER_ONLY + 1;
		}

		char buf[16];
		sprintf( buf, "%#0x", userHdr.accessGroupMask );
		m_AccessGroup = buf;
		
	}
	else if( m_DeviceType == BS_DEVICE_DSTATION )
	{
		DSUserHdr userHdr;

		BS_RET_CODE result = BS_GetUserDStation( m_Handle, userID, &userHdr, m_TemplateData, m_FaceTemplate_DST );

		EndWaitCursor();

		if( result != BS_SUCCESS )
		{
			UpdateData( FALSE );
			return false;
		}

		m_CardID = userHdr.cardID;
		m_Checksum1 = userHdr.fingerChecksum[0];
		m_Checksum2 = userHdr.fingerChecksum[1];
		m_FaceChecksum = userHdr.faceChecksum[0];
		m_CustomID = userHdr.customID;
		m_Finger1 = (userHdr.numOfFinger >= 1)? TRUE : FALSE;
		m_Finger2 = (userHdr.numOfFinger >= 2)? TRUE : FALSE;
		m_Duress1 = userHdr.duress[0];
		m_Duress2 = userHdr.duress[1];
		m_Face_DStation = (userHdr.numOfFace >= 1)? TRUE : FALSE;
        m_Face_FaceStation = FALSE;
		m_UserID = userHdr.ID;

        char szName[64] = {0};
		BS_UTF16ToString((const char*)userHdr.name, szName);
        m_Name = szName;

        char szPassword[32] = {0};
        BS_UTF16ToString((const char*)userHdr.password, szPassword);
        m_Password = szPassword;
		
		TIME_ZONE_INFORMATION timeInfo;
		GetTimeZoneInformation( &timeInfo );

        time_t nStart = ConvertToUTCTime( userHdr.startDateTime );
        ConvertTimeToDate(nStart, m_StartDate);

        time_t nEnd = ConvertToUTCTime( userHdr.expireDateTime );
        ConvertTimeToDate(nEnd, m_ExpiryDate);

        m_AdminLevel = (userHdr.adminLevel == DSUserHdr::USER_ADMIN)? 1 : 0;
		m_SecurityLevel = (userHdr.securityLevel >= BS_USER_SECURITY_DEFAULT)? userHdr.securityLevel - BS_USER_SECURITY_DEFAULT : 0;
		m_CardType = userHdr.bypassCard;

		if( userHdr.authMode == BS_AUTH_MODE_DISABLED )
		{
			m_AuthMode = 0;
		}
		else
		{
			m_AuthMode -= BS_AUTH_FINGER_ONLY + 1;
		}

		char buf[16];
		sprintf( buf, "%#0x", userHdr.accessGroupMask );
		m_AccessGroup = buf;
		
	}
    else if( m_DeviceType == BS_DEVICE_XSTATION )
	{
		XSUserHdr userHdr;

		BS_RET_CODE result = BS_GetUserXStation( m_Handle, userID, &userHdr);

		EndWaitCursor();

		if( result != BS_SUCCESS )
		{
			UpdateData( FALSE );
			return false;
		}

		m_CardID = userHdr.cardID;
		m_CustomID = userHdr.customID;
		m_UserID = userHdr.ID;

		char szName[64] = {0};
		BS_UTF16ToString((const char*)userHdr.name, szName);
        m_Name = szName;

        char szPassword[32] = {0};
        BS_UTF16ToString((const char*)userHdr.password, szPassword);
        m_Password = szPassword;
		
		TIME_ZONE_INFORMATION timeInfo;
		GetTimeZoneInformation( &timeInfo );


        time_t nStart = ConvertToUTCTime( userHdr.startDateTime );
        ConvertTimeToDate(nStart, m_StartDate);

        time_t nEnd = ConvertToUTCTime( userHdr.expireDateTime );
        ConvertTimeToDate(nEnd, m_ExpiryDate);

        m_AdminLevel = (userHdr.adminLevel == XSUserHdr::USER_ADMIN)? 1 : 0;
		m_SecurityLevel = (userHdr.securityLevel >= BS_USER_SECURITY_DEFAULT)? userHdr.securityLevel - BS_USER_SECURITY_DEFAULT : 0;
		m_CardType = userHdr.bypassCard;

		if( userHdr.authMode == BS_AUTH_MODE_DISABLED )
		{
			m_AuthMode = 0;
		}
		else
		{
			m_AuthMode -= BS_AUTH_FINGER_ONLY + 1;
		}

		char buf[16];
		sprintf( buf, "%#0x", userHdr.accessGroupMask );
		m_AccessGroup = buf;
		
	}
	else if( m_DeviceType == BS_DEVICE_BIOSTATION2 )
	{
		BS2UserHdr userHdr;

		memset(m_TemplateData, 0x00, TEMPLATE_SIZE * 2 * BS_MAX_TEMPLATE_PER_USER);

		BS_RET_CODE result = BS_GetUserBioStation2( m_Handle, userID, &userHdr, m_TemplateData );

		EndWaitCursor();

		if( result != BS_SUCCESS )
		{
			UpdateData( FALSE );
			return false;
		}

		m_CardID = userHdr.cardID;
		m_Checksum1 = userHdr.fingerChecksum[0];
		m_Checksum2 = userHdr.fingerChecksum[1];
		m_FaceChecksum = userHdr.faceChecksum[0];
		m_CustomID = userHdr.customID;
		m_Finger1 = (userHdr.numOfFinger >= 1)? TRUE : FALSE;
		m_Finger2 = (userHdr.numOfFinger >= 2)? TRUE : FALSE;
		m_Duress1 = userHdr.duress[0];
		m_Duress2 = userHdr.duress[1];
		m_UserID = userHdr.ID;

        char szName[64] = {0};
		BS_UTF16ToString((const char*)userHdr.name, szName);
        m_Name = szName;
		
        char szPassword[32] = {0};
        BS_UTF16ToString((const char*)userHdr.password, szPassword);
        m_Password = szPassword;

		TIME_ZONE_INFORMATION timeInfo;
		GetTimeZoneInformation( &timeInfo );

        time_t nStart = ConvertToUTCTime( userHdr.startDateTime );
        ConvertTimeToDate(nStart, m_StartDate);

        time_t nEnd = ConvertToUTCTime( userHdr.expireDateTime );
        ConvertTimeToDate(nEnd, m_ExpiryDate);

        m_AdminLevel = (userHdr.adminLevel == BS2UserHdr::USER_ADMIN)? 1 : 0;
		m_SecurityLevel = (userHdr.securityLevel >= BS_USER_SECURITY_DEFAULT)? userHdr.securityLevel - BS_USER_SECURITY_DEFAULT : 0;
		m_CardType = userHdr.bypassCard;

		if( userHdr.authMode == BS_AUTH_MODE_DISABLED )
		{
			m_AuthMode = 0;
		}
		else
		{
			m_AuthMode -= BS_AUTH_FINGER_ONLY + 1;
		}

		char buf[16];
		sprintf( buf, "%#0x", userHdr.accessGroupMask );
		m_AccessGroup = buf;
	}
	else if( m_DeviceType == BS_DEVICE_FSTATION )
	{
	    FSUserHdr userHdr;

	    BS_RET_CODE result = BS_GetUserFStation( m_Handle, userID, &userHdr, m_FaceTemplate_FST );

	    EndWaitCursor();

	    if( result != BS_SUCCESS )
	    {
		    UpdateData( FALSE );
		    return false;
	    }

        if (userHdr.numOfFace < 1 && userHdr.numOfUpdatedFace < 1)
            m_Face_FaceStation = FALSE;
        else
            m_Face_FaceStation = TRUE;

        m_FST_FaceNum = userHdr.numOfFace;

	    m_CardID = userHdr.cardID;
	    m_CustomID = userHdr.customID;
        m_UserID = userHdr.ID;
	    
        char szName[64] = {0};
		BS_UTF16ToString((const char*)userHdr.name, szName);
        m_Name = szName;
		
        char szPassword[32] = {0};
        BS_UTF16ToString((const char*)userHdr.password, szPassword);
        m_Password = szPassword;

	    TIME_ZONE_INFORMATION timeInfo;
	    GetTimeZoneInformation( &timeInfo );

        time_t nStart = ConvertToUTCTime( userHdr.startDateTime );
        ConvertTimeToDate(nStart, m_StartDate);

        time_t nEnd = ConvertToUTCTime( userHdr.expireDateTime );
        ConvertTimeToDate(nEnd, m_ExpiryDate);

        m_AdminLevel = (userHdr.adminLevel == FSUserHdr::USER_ADMIN)? 1 : 0;
	    m_SecurityLevel = (userHdr.securityLevel >= BS_USER_SECURITY_DEFAULT)? userHdr.securityLevel - BS_USER_SECURITY_DEFAULT : 0;
	    m_CardType = userHdr.bypassCard;

	    m_AuthMode = userHdr.authMode;

	    char buf[16];
	    sprintf( buf, "%#0x", userHdr.accessGroupMask );
	    m_AccessGroup = buf;
	}

	UpdateData( FALSE );

	return true;
}

void CUserManagement::OnClickUserList(NMHDR* pNMHDR, LRESULT* pResult) 
{
	// TODO: Add your control notification handler code here
	
	*pResult = 0;

	POSITION pos = m_UserList.GetFirstSelectedItemPosition();

	if( pos )
	{
		int userIndex = m_UserList.GetNextSelectedItem( pos );

		unsigned userID = m_UserList.GetItemData( userIndex );

		if( !getUserInfo( userID ) )
		{
			MessageBox( "Cannot get user info" );
		}
	}
}

void CUserManagement::OnDelete() 
{
	// TODO: Add your control notification handler code here
	UpdateData();

	BeginWaitCursor();

	BS_RET_CODE result = BS_DeleteUser( m_Handle, m_UserID );

	EndWaitCursor();

	if( result != BS_SUCCESS )
	{
		MessageBox( "Cannot delete user" );
		return;
	}

	getUserInfo();	
}


void CUserManagement::OnDeleteAll() 
{
	// TODO: Add your control notification handler code here
	UpdateData();

	BeginWaitCursor();

	BS_RET_CODE result = BS_DeleteAllUser( m_Handle );

	EndWaitCursor();

	if( result != BS_SUCCESS )
	{
		MessageBox( "Cannot delete all user" );
		return;
	}

	getUserInfo();

}

void CUserManagement::OnRefresh() 
{
	// TODO: Add your control notification handler code here
	if( !getUserInfo() )
	{
		MessageBox( "Cannot get user info" );
	}	
}

void CUserManagement::OnUpdate() 
{
	// TODO: Add your control notification handler code here

    UpdateData();

    if (m_StartDate.GetYear() < 1970)
    {
        AfxMessageBox("Start Date can not be less than 1970.");
        return;
    }

    if (m_StartDate.GetYear() > 2030)
    {
        AfxMessageBox("Start Date can not be more than 2030.");
        return;
    }

    if (m_ExpiryDate.GetYear() < 1970)
    {
        AfxMessageBox("Expire Date can not be less than 1970.");
        return;
    }

    if (m_ExpiryDate.GetYear() > 2030)
    {
        AfxMessageBox("Expire Date can not be more than 2030.");
        return;
    }

	BeginWaitCursor();

	if( m_DeviceType == BS_DEVICE_BIOENTRY_PLUS || 
		m_DeviceType == BS_DEVICE_BIOENTRY_W	|| 
        m_DeviceType == BS_DEVICE_BIOLITE       ||
        m_DeviceType == BS_DEVICE_XPASS         || 
        m_DeviceType == BS_DEVICE_XPASS_SLIM    || 
        m_DeviceType == BS_DEVICE_XPASS_SLIM2)

	{
		BEUserHdr userHdr;

		BS_RET_CODE result = BS_GetUserBEPlus( m_Handle, m_UserID, &userHdr, m_TemplateData );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			MessageBox( "Cannot get the user info" );
			return;
		}

		if( m_Finger1 )
		{
			userHdr.numOfFinger = 1;
			userHdr.isDuress[0] = m_Duress1;

			if( m_Finger2 )
			{
				userHdr.numOfFinger = 2;
				userHdr.isDuress[1] = m_Duress2;
			}
		}
		else
		{
			userHdr.numOfFinger = 0;
		}

        wsprintf((char*)userHdr.password, "%s", m_Password);
        BS_ConvertToUTF16((const char*)userHdr.password, (char*)userHdr.password, sizeof(userHdr.password)-1);


        m_StartDate.SetDateTime(m_StartDate.GetYear(), m_StartDate.GetMonth(), m_StartDate.GetDay(), 0, 0, 0);
        userHdr.startTime = ConvertToLocalTime(ConvertDateToTime(m_StartDate));

        m_ExpiryDate.SetDateTime(m_ExpiryDate.GetYear(), m_ExpiryDate.GetMonth(), m_ExpiryDate.GetDay(), 23, 59, 59);
        userHdr.expiryTime = ConvertToLocalTime(ConvertDateToTime(m_ExpiryDate));

		userHdr.adminLevel = m_AdminLevel;
		userHdr.securityLevel = m_SecurityLevel;
		userHdr.cardFlag = m_CardType;
		userHdr.cardID   = m_CardID;
		userHdr.opMode = (m_AuthMode == BS_AUTH_MODE_DISABLED)? BS_AUTH_MODE_DISABLED : m_AuthMode + BS_AUTH_FINGER_ONLY - 1;
		sscanf( m_AccessGroup, "&%x", &userHdr.accessGroupMask );

		result = BS_EnrollUserBEPlus( m_Handle, &userHdr, m_TemplateData );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			MessageBox( "Cannot update the user info" );
			return;
		}
	}
	else if( m_DeviceType == BS_DEVICE_BIOSTATION )
	{
		BSUserHdrEx userHdr;

		BS_RET_CODE result = BS_GetUserEx( m_Handle, m_UserID, &userHdr, m_TemplateData );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			MessageBox( "Cannot get the user info" );
			return;
		}

		userHdr.duressMask = 0x00;

		if( m_Finger1 )
		{
			userHdr.numOfFinger = 1;
			userHdr.duressMask |= m_Duress1;

			if( m_Finger2 )
			{
				userHdr.numOfFinger = 2;
				userHdr.duressMask |= m_Duress2 << 1;
			}
		}
		else
		{
			userHdr.numOfFinger = 0;
		}

        wsprintf((char*)userHdr.name, "%s", m_Name);
        BS_ConvertToUTF8((const char*)userHdr.name, (char*)userHdr.name, sizeof(userHdr.name)-1);

        wsprintf((char*)userHdr.password, "%s", m_Password);
        BS_ConvertToUTF8((const char*)userHdr.password, (char*)userHdr.password, sizeof(userHdr.password)-1);

        m_StartDate.SetDateTime(m_StartDate.GetYear(), m_StartDate.GetMonth(), m_StartDate.GetDay(), 0, 0, 0);
        userHdr.startDateTime = ConvertToLocalTime(ConvertDateToTime(m_StartDate));

        m_ExpiryDate.SetDateTime(m_ExpiryDate.GetYear(), m_ExpiryDate.GetMonth(), m_ExpiryDate.GetDay(), 23, 59, 59);
        userHdr.expireDateTime = ConvertToLocalTime(ConvertDateToTime(m_ExpiryDate));

		userHdr.adminLevel = (m_AdminLevel == 1)? BS_USER_ADMIN : BS_USER_NORMAL;
		userHdr.securityLevel = m_SecurityLevel + BS_USER_SECURITY_DEFAULT;
		userHdr.bypassCard = m_CardType;
		userHdr.authMode = (m_AuthMode == BS_AUTH_MODE_DISABLED)? BS_AUTH_MODE_DISABLED : m_AuthMode + BS_AUTH_FINGER_ONLY - 1;
		sscanf( m_AccessGroup, "&%x", &userHdr.accessGroupMask );

		result = BS_EnrollUserEx( m_Handle, &userHdr, m_TemplateData );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			MessageBox( "Cannot update the user info" );
			return;
		}
	}
	else if( m_DeviceType == BS_DEVICE_DSTATION )
	{
		DSUserHdr userHdr;

		memset(m_TemplateData, 0x00, TEMPLATE_SIZE * 2 * BS_MAX_TEMPLATE_PER_USER);
		memset(m_FaceTemplate_DST, 0x00, FACETEMPLATE_SIZE * BS_MAX_FACE_PER_USER);

		BS_RET_CODE result = BS_GetUserDStation( m_Handle, m_UserID, &userHdr, m_TemplateData, m_FaceTemplate_DST );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			MessageBox( "Cannot get the user info" );
			return;
		}

		if( m_Finger1 )
		{
			userHdr.numOfFinger = 1;
			userHdr.duress[0] = m_Duress1;

			if( m_Finger2 )
			{
				userHdr.numOfFinger = 2;
				userHdr.duress[1] = m_Duress2;
			}
		}
		else
		{
			userHdr.numOfFinger = 0;
		}


		if( m_Face_DStation > 0)
			userHdr.numOfFace = 1;
		else
			userHdr.numOfFace = 0;

        wsprintf((char*)userHdr.name, "%s", m_Name);
        BS_ConvertToUTF16((const char*)userHdr.name, (char*)userHdr.name, sizeof(userHdr.name)-1);

        wsprintf((char*)userHdr.password, "%s", m_Password);
        BS_ConvertToUTF16((const char*)userHdr.password, (char*)userHdr.password, sizeof(userHdr.password)-1);

        m_StartDate.SetDateTime(m_StartDate.GetYear(), m_StartDate.GetMonth(), m_StartDate.GetDay(), 0, 0, 0);
        userHdr.startDateTime = ConvertToLocalTime(ConvertDateToTime(m_StartDate));

        m_ExpiryDate.SetDateTime(m_ExpiryDate.GetYear(), m_ExpiryDate.GetMonth(), m_ExpiryDate.GetDay(), 23, 59, 59);
        userHdr.expireDateTime = ConvertToLocalTime(ConvertDateToTime(m_ExpiryDate));

        userHdr.adminLevel		= (m_AdminLevel == 1)? DSUserHdr::USER_ADMIN : DSUserHdr::USER_NORMAL;
		userHdr.securityLevel	= m_SecurityLevel + BS_USER_SECURITY_DEFAULT;
		userHdr.bypassCard		= m_CardType;
		userHdr.authMode		= (m_AuthMode == BS_AUTH_MODE_DISABLED)? BS_AUTH_MODE_DISABLED : m_AuthMode + BS_AUTH_FINGER_ONLY - 1;
		sscanf( m_AccessGroup, "&%x", &userHdr.accessGroupMask );

		result = BS_EnrollUserDStation( m_Handle, &userHdr, m_TemplateData, m_FaceTemplate_DST );
		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			MessageBox( "Cannot update the user info" );
			return;
		}
	}
    else if( m_DeviceType == BS_DEVICE_XSTATION )
	{
		XSUserHdr userHdr;
        memset(&userHdr, 0x00, sizeof(XSUserHdr));

		BS_RET_CODE result = BS_GetUserXStation( m_Handle, m_UserID, &userHdr);

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			MessageBox( "Cannot get the user info" );
			return;
		}

        userHdr.numOfFinger = 0;
		userHdr.numOfFace = 0;

        wsprintf((char*)userHdr.name, "%s", m_Name);
        BS_ConvertToUTF16((const char*)userHdr.name, (char*)userHdr.name, sizeof(userHdr.name)-1);

        wsprintf((char*)userHdr.password, "%s", m_Password);
        BS_ConvertToUTF16((const char*)userHdr.password, (char*)userHdr.password, sizeof(userHdr.password)-1);


        m_StartDate.SetDateTime(m_StartDate.GetYear(), m_StartDate.GetMonth(), m_StartDate.GetDay(), 0, 0, 0);
        userHdr.startDateTime = ConvertToLocalTime(ConvertDateToTime(m_StartDate));

        m_ExpiryDate.SetDateTime(m_ExpiryDate.GetYear(), m_ExpiryDate.GetMonth(), m_ExpiryDate.GetDay(), 23, 59, 59);
        userHdr.expireDateTime = ConvertToLocalTime(ConvertDateToTime(m_ExpiryDate));

        userHdr.adminLevel		= (m_AdminLevel == 1)? XSUserHdr::USER_ADMIN : XSUserHdr::USER_NORMAL;
		userHdr.securityLevel	= m_SecurityLevel + BS_USER_SECURITY_DEFAULT;
		userHdr.bypassCard		= m_CardType;
		userHdr.authMode		= (m_AuthMode == BS_AUTH_MODE_DISABLED)? BS_AUTH_MODE_DISABLED : m_AuthMode + BS_AUTH_FINGER_ONLY - 1;
		sscanf( m_AccessGroup, "&%x", &userHdr.accessGroupMask );

		result = BS_EnrollUserXStation( m_Handle, &userHdr );
		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			MessageBox( "Cannot update the user info" );
			return;
		}
	}
	else if( m_DeviceType == BS_DEVICE_BIOSTATION2 )
	{
		BS2UserHdr userHdr;
		memset(m_TemplateData, 0x00, TEMPLATE_SIZE * 2 * BS_MAX_TEMPLATE_PER_USER);

		BS_RET_CODE result = BS_GetUserBioStation2( m_Handle, m_UserID, &userHdr, m_TemplateData );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			MessageBox( "Cannot get the user info" );
			return;
		}

		if( m_Finger1 )
		{
			userHdr.numOfFinger = 1;
			userHdr.duress[0] = m_Duress1;

			if( m_Finger2 )
			{
				userHdr.numOfFinger = 2;
				userHdr.duress[1] = m_Duress2;
			}
		}
		else
		{
			userHdr.numOfFinger = 0;
		}


		userHdr.numOfFace = 0;
        
        wsprintf((char*)userHdr.name, "%s", m_Name);
        BS_ConvertToUTF16((const char*)userHdr.name, (char*)userHdr.name, sizeof(userHdr.name)-1);

        wsprintf((char*)userHdr.password, "%s", m_Password);
        BS_ConvertToUTF16((const char*)userHdr.password, (char*)userHdr.password, sizeof(userHdr.password)-1);

        m_StartDate.SetDateTime(m_StartDate.GetYear(), m_StartDate.GetMonth(), m_StartDate.GetDay(), 0, 0, 0);
        userHdr.startDateTime = ConvertToLocalTime(ConvertDateToTime(m_StartDate));

        m_ExpiryDate.SetDateTime(m_ExpiryDate.GetYear(), m_ExpiryDate.GetMonth(), m_ExpiryDate.GetDay(), 23, 59, 59);
        userHdr.expireDateTime = ConvertToLocalTime(ConvertDateToTime(m_ExpiryDate));

        userHdr.adminLevel		= (m_AdminLevel == 1)? BS2UserHdr::USER_ADMIN : BS2UserHdr::USER_NORMAL;
		userHdr.securityLevel	= m_SecurityLevel + BS_USER_SECURITY_DEFAULT;
		userHdr.bypassCard		= m_CardType;
		userHdr.authMode		= (m_AuthMode == BS_AUTH_MODE_DISABLED)? BS_AUTH_MODE_DISABLED : m_AuthMode + BS_AUTH_FINGER_ONLY - 1;
        userHdr.cardID          = m_CardID;
        userHdr.customID        = m_CustomID;
		sscanf( m_AccessGroup, "&%x", &userHdr.accessGroupMask );

		result = BS_EnrollUserBioStation2( m_Handle, &userHdr, m_TemplateData );
		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			MessageBox( "Cannot update the user info" );
			return;
		}
	}
	else if( m_DeviceType == BS_DEVICE_FSTATION )
	{
	    FSUserHdr userHdr;

        unsigned char * faceTemplate  = (unsigned char*)malloc( FACETEMPLATE_FST_SIZE * BS_MAX_FACE_TEMPLATE ); // for FaceStation face template
	    memset(faceTemplate, 0x00, FACETEMPLATE_FST_SIZE * BS_MAX_FACE_TEMPLATE);

	    BS_RET_CODE result = BS_GetUserFStation( m_Handle, m_UserID, &userHdr, faceTemplate );

	    if( result != BS_SUCCESS )
	    {
            free(faceTemplate);
		    EndWaitCursor();
		    MessageBox( "Cannot get the user info" );
		    return;
	    }


        int bScannedFace = ((CButton*)GetDlgItem(IDC_CHECK_SCANFACE))->GetCheck();

        if (bScannedFace)
        {
            userHdr.numOfFace        = m_userTemplateHdr.numOfFace;
            userHdr.numOfUpdatedFace = m_userTemplateHdr.numOfUpdatedFace;

            //face len
            for (int i=0; i < FSUserHdr::MAX_FACE; i++)
            {
                userHdr.faceLen[i] = m_userTemplateHdr.faceLen[i];
            }

            //checksum
            int offset = 0;
            unsigned checksum = 0;
            for (int i=0; i < userHdr.numOfFace; i++)
            {
                unsigned char *templateBuf = m_FaceTemplate_FST + offset;
                for (int j=0; j < userHdr.faceLen[i]; j++)
                {
                    checksum += templateBuf[j];
                }

                userHdr.faceChecksum[i] = checksum;
                offset += userHdr.faceLen[i];
            }
        }
        else
        {
            int bufPos = 0;
            int templatePos = 0;

            for (int i=0; i < FSUserHdr::MAX_FACE; i++)
            {
                memcpy(m_FaceTemplate_FST + bufPos, faceTemplate + templatePos, userHdr.faceLen[i]);

                bufPos      += userHdr.faceLen[i];
                templatePos += userHdr.faceLen[i];
            }
        }

        wsprintf((char*)userHdr.name, "%s", m_Name);
        BS_ConvertToUTF16((const char*)userHdr.name, (char*)userHdr.name, sizeof(userHdr.name)-1);

        wsprintf((char*)userHdr.password, "%s", m_Password);
        BS_ConvertToUTF16((const char*)userHdr.password, (char*)userHdr.password, sizeof(userHdr.password)-1);

        m_StartDate.SetDateTime(m_StartDate.GetYear(), m_StartDate.GetMonth(), m_StartDate.GetDay(), 0, 0, 0);
        userHdr.startDateTime = ConvertToLocalTime(ConvertDateToTime(m_StartDate));

        m_ExpiryDate.SetDateTime(m_ExpiryDate.GetYear(), m_ExpiryDate.GetMonth(), m_ExpiryDate.GetDay(), 23, 59, 59);
        userHdr.expireDateTime = ConvertToLocalTime(ConvertDateToTime(m_ExpiryDate));

        userHdr.adminLevel		= (m_AdminLevel == 1)? FSUserHdr::USER_ADMIN : FSUserHdr::USER_NORMAL;
	    userHdr.securityLevel	= m_SecurityLevel + BS_USER_SECURITY_DEFAULT;
	    userHdr.bypassCard		= m_CardType;
	    userHdr.authMode		= m_AuthMode;
        userHdr.cardID          = m_CardID;
        userHdr.customID        = m_CustomID;
	    sscanf( m_AccessGroup, "&%x", &userHdr.accessGroupMask );


	    result = BS_EnrollUserFStation( m_Handle, &userHdr, m_FaceTemplate_FST );
	    if( result != BS_SUCCESS )
	    {
		    EndWaitCursor();
		    MessageBox( "Cannot update the user info" );
		    return;
	    }
	}

	EndWaitCursor();	
}

void CUserManagement::OnAdd() 
{
	// TODO: Add your control notification handler code here
	UpdateData();
	
    if (m_StartDate.GetYear() < 1970)
    {
        AfxMessageBox("Start Date can not be less than 1970.");
        return;
    }

    if (m_StartDate.GetYear() > 2030)
    {
        AfxMessageBox("Start Date can not be more than 2030.");
        return;
    }

    if (m_ExpiryDate.GetYear() < 1970)
    {
        AfxMessageBox("Expire Date can not be less than 1970.");
        return;
    }

    if (m_ExpiryDate.GetYear() > 2030)
    {
        AfxMessageBox("Expire Date can not be more than 2030.");
        return;
    }

	BeginWaitCursor();

	BS_RET_CODE result;

	if( m_Finger1 )
	{
		result = BS_ScanTemplate( m_Handle, m_TemplateData );
		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			MessageBox( "Cannot scan the template" );
			return;
		}

		result = BS_ScanTemplate( m_Handle, m_TemplateData + TEMPLATE_SIZE );
		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			MessageBox( "Cannot scan the template" );
			return;
		}

		if( m_Finger2 )
		{
			BS_RET_CODE result = BS_ScanTemplate( m_Handle, m_TemplateData + TEMPLATE_SIZE * 2 );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();
				MessageBox( "Cannot scan the template" );
				return;
			}

			result = BS_ScanTemplate( m_Handle, m_TemplateData + TEMPLATE_SIZE * 3 );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();
				MessageBox( "Cannot scan the template" );
				return;
			}
		}
	}

	if( m_Face_DStation )
	{
		if (m_DeviceType == BS_DEVICE_DSTATION)
		{

			int imageLen = 0;
			int pos_1 = 0, pos_2 = 0;
			unsigned char *imageData = (unsigned char*)malloc(BS_MAX_IMAGE_SIZE);

            int nRet = 0;
            do 
            {
                nRet = 0;
                result = BS_ReadFaceData( m_Handle, &imageLen, imageData, m_FaceTemplate_DST);
                if (result != BS_SUCCESS)
                { 
                    CString  errMsg = "Error Capture User Face!!!\r\nTry capture again?";
                    nRet = AfxMessageBox(errMsg, MB_RETRYCANCEL|MB_ICONSTOP);
                    if (nRet == IDCANCEL) 
                    {
					    free(imageData);
					    EndWaitCursor();
                        return;
                    }
                }
            } while (nRet == IDRETRY);

			free(imageData);

            Sleep(500);     // The delay is required that is more than five miliseconds.
		}
	}


    if( m_Face_FaceStation )
	{
		if (m_DeviceType == BS_DEVICE_FSTATION)
		{
            FSUserTemplateHdr userTemplateHdr = {0};

            unsigned char *imageData = (unsigned char*)malloc(BS_MAX_IMAGE_SIZE);
            unsigned char *faceTemplate = (unsigned char*)malloc(FACETEMPLATE_FST_SIZE * BS_MAX_FACE_TEMPLATE);

            int nRet = 0;
            do 
            {
                nRet = 0;
                result = BS_ScanFaceTemplate( m_Handle, &userTemplateHdr, imageData, faceTemplate );
                if (result != BS_SUCCESS)
                { 
                    CString  errMsg = "Error Capture User Face from FaceStation!!!\r\nTry capture again?";
                    nRet = AfxMessageBox(errMsg, MB_RETRYCANCEL|MB_ICONSTOP);
                    if (nRet == IDCANCEL) 
                    {
					    free(imageData);
					    EndWaitCursor();
                        return;
                    }
                }
            } while (nRet == IDRETRY);

            memcpy(&m_userTemplateHdr, &userTemplateHdr, sizeof(FSUserTemplateHdr));

            // face template data (max 25)
            int bufPos = 0;
            int templatePos = 0;

            memset(m_FaceTemplate_FST, 0x00, FACETEMPLATE_FST_SIZE * BS_MAX_FACE_TEMPLATE);

            for (int i=0; i < FSUserHdr::MAX_FACE; i++)
            {
                memcpy(m_FaceTemplate_FST + bufPos, faceTemplate + templatePos, userTemplateHdr.faceLen[i]);

                bufPos      += userTemplateHdr.faceLen[i];
                templatePos += userTemplateHdr.faceLen[i];
            }

			free(imageData);
            free(faceTemplate);

            Sleep(500);     // The delay is required that is more than five miliseconds.
		}
	}


	if( m_DeviceType == BS_DEVICE_BIOENTRY_PLUS || 
		m_DeviceType == BS_DEVICE_BIOENTRY_W	|| 
        m_DeviceType == BS_DEVICE_BIOLITE       || 
        m_DeviceType == BS_DEVICE_XPASS         || 
        m_DeviceType == BS_DEVICE_XPASS_SLIM    || 
        m_DeviceType == BS_DEVICE_XPASS_SLIM2)
	{
		BEUserHdr userHdr;

		memset( &userHdr, 0, sizeof( BEUserHdr ) );

		if( m_Finger1 )
		{
			userHdr.numOfFinger = 1;
			userHdr.isDuress[0] = m_Duress1;

			userHdr.fingerChecksum[0] = 0;

			for( int i = 0; i < TEMPLATE_SIZE * 2; i++ )
			{
				userHdr.fingerChecksum[0] += m_TemplateData[i];
			}

			if( m_Finger2 )
			{
				userHdr.numOfFinger = 2;
				userHdr.isDuress[1] = m_Duress2;

				userHdr.fingerChecksum[1] = 0;

				for( int i = 0; i < TEMPLATE_SIZE * 2; i++ )
				{
					userHdr.fingerChecksum[1] += m_TemplateData[TEMPLATE_SIZE * 2 + i];
				}
			}
		}
		else
		{
			userHdr.numOfFinger = 0;
		}

        wsprintf((char*)userHdr.password, "%s", m_Password);
        BS_ConvertToUTF16((const char*)userHdr.password, (char*)userHdr.password, sizeof(userHdr.password)-1);

		userHdr.userID = m_UserID;
		userHdr.cardID = m_CardID;
		userHdr.cardCustomID = m_CustomID;
		userHdr.cardVersion = BEUserHdr::CARD_VERSION_1;

        m_StartDate.SetDateTime(m_StartDate.GetYear(), m_StartDate.GetMonth(), m_StartDate.GetDay(), 0, 0, 0);
        userHdr.startTime = ConvertToLocalTime(ConvertDateToTime(m_StartDate));

        m_ExpiryDate.SetDateTime(m_ExpiryDate.GetYear(), m_ExpiryDate.GetMonth(), m_ExpiryDate.GetDay(), 23, 59, 59);
        userHdr.expiryTime = ConvertToLocalTime(ConvertDateToTime(m_ExpiryDate));

		userHdr.adminLevel = m_AdminLevel;
		userHdr.securityLevel = m_SecurityLevel;
		userHdr.cardFlag = m_CardType;
		
		userHdr.opMode = (m_AuthMode == BS_AUTH_MODE_DISABLED)? BS_AUTH_MODE_DISABLED : m_AuthMode + BS_AUTH_FINGER_ONLY - 1;

		sscanf( m_AccessGroup, "&%x", &userHdr.accessGroupMask );

		result = BS_EnrollUserBEPlus( m_Handle, &userHdr, m_TemplateData );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();

			MessageBox( "Cannot enroll the user" );
			return;
		}
		this->getUserInfo(); 
	}
	else if( m_DeviceType == BS_DEVICE_BIOSTATION )
	{
		BSUserHdrEx userHdr;

		memset( &userHdr, 0, sizeof( BSUserHdrEx ) );

		userHdr.duressMask = 0x00;

		if( m_Finger1 )
		{
			userHdr.numOfFinger = 1;
			userHdr.duressMask |= m_Duress1;

			userHdr.checksum[0] = 0;

			for( int i = 0; i < TEMPLATE_SIZE * 2; i++ )
			{
				userHdr.checksum[0] += m_TemplateData[i];
			}

			if( m_Finger2 )
			{
				userHdr.numOfFinger = 2;
				userHdr.duressMask |= m_Duress2 << 1;

				userHdr.checksum[1] = 0;

				for( int i = 0; i < TEMPLATE_SIZE * 2; i++ )
				{
					userHdr.checksum[1] += m_TemplateData[TEMPLATE_SIZE * 2 + i];
				}
			}
		}
		else
		{
			userHdr.numOfFinger = 0;
		}

        wsprintf((char*)userHdr.name, "%s", m_Name);
        BS_ConvertToUTF8((const char*)userHdr.name, (char*)userHdr.name, sizeof(userHdr.name)-1);

        wsprintf((char*)userHdr.password, "%s", m_Password);
        BS_ConvertToUTF8((const char*)userHdr.password, (char*)userHdr.password, sizeof(userHdr.password)-1);

		userHdr.ID = m_UserID;
		userHdr.cardID = m_CardID;
		userHdr.customID = m_CustomID;
		userHdr.version = CARD_INFO_VERSION;


        m_StartDate.SetDateTime(m_StartDate.GetYear(), m_StartDate.GetMonth(), m_StartDate.GetDay(), 0, 0, 0);
        userHdr.startDateTime = ConvertToLocalTime(ConvertDateToTime(m_StartDate));

        m_ExpiryDate.SetDateTime(m_ExpiryDate.GetYear(), m_ExpiryDate.GetMonth(), m_ExpiryDate.GetDay(), 23, 59, 59);
        userHdr.expireDateTime = ConvertToLocalTime(ConvertDateToTime(m_ExpiryDate));

		userHdr.adminLevel = (m_AdminLevel == 1)? BS_USER_ADMIN : BS_USER_NORMAL;
		userHdr.securityLevel = m_SecurityLevel + BS_USER_SECURITY_DEFAULT;
		userHdr.bypassCard = m_CardType;
		
		userHdr.authMode = (m_AuthMode == BS_AUTH_MODE_DISABLED)? BS_AUTH_MODE_DISABLED : m_AuthMode + BS_AUTH_FINGER_ONLY - 1;

		sscanf( m_AccessGroup, "&%x", &userHdr.accessGroupMask );

		result = BS_EnrollUserEx( m_Handle, &userHdr, m_TemplateData );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();

			MessageBox( "Cannot enroll the user" );
			return;
		}
		this->getUserInfo(); 
	}
	else if( m_DeviceType == BS_DEVICE_DSTATION )
	{
		DSUserHdr userHdr;

		memset( &userHdr, 0, sizeof( DSUserHdr ) );

		if( m_Finger1 )
		{
			userHdr.numOfFinger = 1;
			userHdr.duress[0] = m_Duress1;

			userHdr.fingerChecksum[0] = 0;

			for( int i = 0; i < TEMPLATE_SIZE * 2; i++ )
			{
				userHdr.fingerChecksum[0] += m_TemplateData[i];
			}

			if( m_Finger2 )
			{
				userHdr.numOfFinger = 2;
				userHdr.duress[1] = m_Duress2;

				userHdr.fingerChecksum[1] = 0;

				for( int i = 0; i < TEMPLATE_SIZE * 2; i++ )
				{
					userHdr.fingerChecksum[1] += m_TemplateData[TEMPLATE_SIZE * 2 + i];
				}
			}
		}
		else
		{
			userHdr.numOfFinger = 0;
		}

		if( m_Face_DStation )
		{
			userHdr.numOfFace = 1;
			userHdr.faceChecksum[0] = 0;

			for( int i = 0; i < FACETEMPLATE_SIZE; i++ )
			{
				userHdr.faceChecksum[0] += m_FaceTemplate_DST[i];
			}
		}
		else
		{
			userHdr.numOfFace = 0;
		}

        wsprintf((char*)userHdr.name, "%s", m_Name);
        BS_ConvertToUTF16((const char*)userHdr.name, (char*)userHdr.name, sizeof(userHdr.name)-1);

        wsprintf((char*)userHdr.password, "%s", m_Password);
        BS_ConvertToUTF16((const char*)userHdr.password, (char*)userHdr.password, sizeof(userHdr.password)-1);

		userHdr.ID				= m_UserID;
		userHdr.cardID			= m_CardID;
		userHdr.customID		= m_CustomID;


        m_StartDate.SetDateTime(m_StartDate.GetYear(), m_StartDate.GetMonth(), m_StartDate.GetDay(), 0, 0, 0);
        userHdr.startDateTime = ConvertToLocalTime(ConvertDateToTime(m_StartDate));

        m_ExpiryDate.SetDateTime(m_ExpiryDate.GetYear(), m_ExpiryDate.GetMonth(), m_ExpiryDate.GetDay(), 23, 59, 59);
        userHdr.expireDateTime = ConvertToLocalTime(ConvertDateToTime(m_ExpiryDate));

        userHdr.adminLevel		= (m_AdminLevel == 1)? DSUserHdr::USER_ADMIN : DSUserHdr::USER_NORMAL;
		userHdr.securityLevel	= m_SecurityLevel + BS_USER_SECURITY_DEFAULT;
		userHdr.bypassCard		= m_CardType;

		userHdr.authMode		= (m_AuthMode == BS_AUTH_MODE_DISABLED)? BS_AUTH_MODE_DISABLED : m_AuthMode + BS_AUTH_FINGER_ONLY - 1;

		sscanf( m_AccessGroup, "&%x", &userHdr.accessGroupMask );

		result = BS_EnrollUserDStation( m_Handle, &userHdr, m_TemplateData, m_FaceTemplate_DST );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			MessageBox( "Cannot enroll the user" );

			return;
		}
		this->getUserInfo(); 
	}
    else if( m_DeviceType == BS_DEVICE_XSTATION )
	{
		XSUserHdr userHdr;
		memset( &userHdr, 0, sizeof( XSUserHdr ) );

        wsprintf((char*)userHdr.name, "%s", m_Name);
        BS_ConvertToUTF16((const char*)userHdr.name, (char*)userHdr.name, sizeof(userHdr.name)-1);

        wsprintf((char*)userHdr.password, "%s", m_Password);
        BS_ConvertToUTF16((const char*)userHdr.password, (char*)userHdr.password, sizeof(userHdr.password)-1);

		userHdr.ID				= m_UserID;
		userHdr.cardID			= m_CardID;
		userHdr.customID		= m_CustomID;

        m_StartDate.SetDateTime(m_StartDate.GetYear(), m_StartDate.GetMonth(), m_StartDate.GetDay(), 0, 0, 0);
        userHdr.startDateTime = ConvertToLocalTime(ConvertDateToTime(m_StartDate));

        m_ExpiryDate.SetDateTime(m_ExpiryDate.GetYear(), m_ExpiryDate.GetMonth(), m_ExpiryDate.GetDay(), 23, 59, 59);
        userHdr.expireDateTime = ConvertToLocalTime(ConvertDateToTime(m_ExpiryDate));

        userHdr.adminLevel		= (m_AdminLevel == 1)? XSUserHdr::USER_ADMIN : XSUserHdr::USER_NORMAL;
		userHdr.securityLevel	= m_SecurityLevel + BS_USER_SECURITY_DEFAULT;
		userHdr.bypassCard		= m_CardType;

		userHdr.authMode		= (m_AuthMode == BS_AUTH_MODE_DISABLED)? BS_AUTH_MODE_DISABLED : m_AuthMode + BS_AUTH_FINGER_ONLY - 1;

		sscanf( m_AccessGroup, "&%x", &userHdr.accessGroupMask );

		result = BS_EnrollUserXStation( m_Handle, &userHdr );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			MessageBox( "Cannot enroll the user" );

			return;
		}
		this->getUserInfo(); 
	}
	else if( m_DeviceType == BS_DEVICE_BIOSTATION2 )
	{
		BS2UserHdr userHdr;

		memset( &userHdr, 0, sizeof( BS2UserHdr ) );

		if( m_Finger1 )
		{
			userHdr.numOfFinger = 1;
			userHdr.duress[0] = m_Duress1;

			userHdr.fingerChecksum[0] = 0;

			for( int i = 0; i < TEMPLATE_SIZE * 2; i++ )
			{
				userHdr.fingerChecksum[0] += m_TemplateData[i];
			}

			if( m_Finger2 )
			{
				userHdr.numOfFinger = 2;
				userHdr.duress[1] = m_Duress2;

				userHdr.fingerChecksum[1] = 0;

				for( int i = 0; i < TEMPLATE_SIZE * 2; i++ )
				{
					userHdr.fingerChecksum[1] += m_TemplateData[TEMPLATE_SIZE * 2 + i];
				}
			}
		}
		else
		{
			userHdr.numOfFinger = 0;
		}

		userHdr.numOfFace = 0;

        wsprintf((char*)userHdr.name, "%s", m_Name);
        BS_ConvertToUTF16((const char*)userHdr.name, (char*)userHdr.name, sizeof(userHdr.name)-1);

        wsprintf((char*)userHdr.password, "%s", m_Password);
        BS_ConvertToUTF16((const char*)userHdr.password, (char*)userHdr.password, sizeof(userHdr.password)-1);

		userHdr.ID				= m_UserID;
		userHdr.cardID			= m_CardID;
		userHdr.customID		= m_CustomID;

        m_StartDate.SetDateTime(m_StartDate.GetYear(), m_StartDate.GetMonth(), m_StartDate.GetDay(), 0, 0, 0);
        userHdr.startDateTime = ConvertToLocalTime(ConvertDateToTime(m_StartDate));

        m_ExpiryDate.SetDateTime(m_ExpiryDate.GetYear(), m_ExpiryDate.GetMonth(), m_ExpiryDate.GetDay(), 23, 59, 59);
        userHdr.expireDateTime = ConvertToLocalTime(ConvertDateToTime(m_ExpiryDate));

        userHdr.adminLevel		= (m_AdminLevel == 1)? BS2UserHdr::USER_ADMIN : BS2UserHdr::USER_NORMAL;
		userHdr.securityLevel	= m_SecurityLevel + BS_USER_SECURITY_DEFAULT;
		userHdr.bypassCard		= m_CardType;

		userHdr.authMode		= (m_AuthMode == BS_AUTH_MODE_DISABLED)? BS_AUTH_MODE_DISABLED : m_AuthMode + BS_AUTH_FINGER_ONLY - 1;

		sscanf( m_AccessGroup, "&%x", &userHdr.accessGroupMask );

		result = BS_EnrollUserBioStation2( m_Handle, &userHdr, m_TemplateData );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();
			MessageBox( "Cannot enroll the user" );

			return;
		}
		this->getUserInfo(); 
	}
	else if( m_DeviceType == BS_DEVICE_FSTATION )
	{
	    FSUserHdr userHdr;

        memset( &userHdr, 0x00, sizeof( FSUserHdr ) );

        userHdr.numOfFace        = m_userTemplateHdr.numOfFace;
        userHdr.numOfUpdatedFace = m_userTemplateHdr.numOfUpdatedFace;
        
        // face template's length
        for (int i=0; i < FSUserHdr::MAX_FACE; i++)
        {
            userHdr.faceLen[i] = m_userTemplateHdr.faceLen[i];
        }
        
        // face template's checksum
        int offset = 0;
        unsigned checksum = 0;
        for (int i=0; i < userHdr.numOfFace; i++)
        {
            unsigned char *templateData = m_FaceTemplate_FST + offset;

            for (int j=0; j < m_userTemplateHdr.faceLen[i]; j++)
            {
                checksum += templateData[j];
            }
            
            userHdr.faceChecksum[i] = checksum;
            offset += m_userTemplateHdr.faceLen[i];
        }
        //

        // face temp data
        memcpy(userHdr.faceTemp, m_userTemplateHdr.faceTemp, 256);


        wsprintf((char*)userHdr.name, "%s", m_Name);
        BS_ConvertToUTF16((const char*)userHdr.name, (char*)userHdr.name, sizeof(userHdr.name)-1);

        wsprintf((char*)userHdr.password, "%s", m_Password);
        BS_ConvertToUTF16((const char*)userHdr.password, (char*)userHdr.password, sizeof(userHdr.password)-1);

	    userHdr.ID				= m_UserID;
	    userHdr.cardID			= m_CardID;
	    userHdr.customID		= m_CustomID;

        m_StartDate.SetDateTime(m_StartDate.GetYear(), m_StartDate.GetMonth(), m_StartDate.GetDay(), 0, 0, 0);
        userHdr.startDateTime = ConvertToLocalTime(ConvertDateToTime(m_StartDate));

        m_ExpiryDate.SetDateTime(m_ExpiryDate.GetYear(), m_ExpiryDate.GetMonth(), m_ExpiryDate.GetDay(), 23, 59, 59);
        userHdr.expireDateTime = ConvertToLocalTime(ConvertDateToTime(m_ExpiryDate));

        userHdr.adminLevel		= (m_AdminLevel == 1)? FSUserHdr::USER_ADMIN : FSUserHdr::USER_NORMAL;
	    userHdr.securityLevel	= m_SecurityLevel + BS_USER_SECURITY_DEFAULT;
	    userHdr.bypassCard		= m_CardType;

	    userHdr.authMode		= m_AuthMode;

	    sscanf( m_AccessGroup, "&%x", &userHdr.accessGroupMask );

	    result = BS_EnrollUserFStation( m_Handle, &userHdr, m_FaceTemplate_FST );

	    if( result != BS_SUCCESS )
	    {
		    EndWaitCursor();
		    MessageBox( "Cannot enroll the user" );

		    return;
	    }
		this->getUserInfo(); 
	}

	EndWaitCursor();	
	
}

void CUserManagement::OnBnClickedFstScanFace()
{
    FSUserTemplateHdr userTemplateHdr = {0};

    unsigned char *imageData = (unsigned char*)malloc(BS_MAX_IMAGE_SIZE);
    unsigned char *faceTemplate = (unsigned char*)malloc(FACETEMPLATE_FST_SIZE * BS_MAX_FACE_TEMPLATE);

    int result = BS_ScanFaceTemplate( m_Handle, &userTemplateHdr, imageData, faceTemplate );
    if (result != BS_SUCCESS)
    {
        free(imageData);
        free(faceTemplate);
        ((CButton*)GetDlgItem(IDC_CHECK_SCANFACE))->SetCheck(0);

        return;
    }

    memcpy(&m_userTemplateHdr, &userTemplateHdr, sizeof(FSUserTemplateHdr));

    // user's face template count
    int numOfFace        = userTemplateHdr.numOfFace;
    int numOfUpdatedFace = userTemplateHdr.numOfUpdatedFace;

    m_FST_FaceNum = numOfFace;


    // face template data (max 25)
    int bufPos = 0;
    int templatePos = 0;

    memset(m_FaceTemplate_FST, 0x00, FACETEMPLATE_FST_SIZE * BS_MAX_FACE_TEMPLATE);

    for (int i=0; i < FSUserHdr::MAX_FACE; i++)
    {
        memcpy(m_FaceTemplate_FST + bufPos, faceTemplate + templatePos, userTemplateHdr.faceLen[i]);

        bufPos      += userTemplateHdr.faceLen[i];
        templatePos += userTemplateHdr.faceLen[i];
    }

    // save profile image 
    FILE *fp = NULL;
    if ((fp = fopen("C:\\Temp\\Profile.jpg", "wb")))
    {
        fwrite(imageData, sizeof(char), userTemplateHdr.imageSize, fp);    
        fclose(fp);
    }
    //

    free(imageData);
    free(faceTemplate);

    ((CButton*)GetDlgItem(IDC_CHECK_SCANFACE))->SetCheck(1);
}

void CUserManagement::OnReadCard() 
{
	// TODO: Add your control notification handler code here
	UpdateData();

	BeginWaitCursor();

	if( m_ExtRF )
	{
		unsigned cardID;
		int customID;
		int result;

		if( m_DeviceType == BS_DEVICE_BIOSTATION   || 		
            m_DeviceType == BS_DEVICE_DSTATION     ||		
            m_DeviceType == BS_DEVICE_XSTATION     ||
            m_DeviceType == BS_DEVICE_BIOSTATION2  ||
            m_DeviceType == BS_DEVICE_FSTATION)
		{
			BSIOConfig ioConfig;
			result = BS_ReadIOConfig( m_Handle, &ioConfig );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();

				MessageBox( "Cannot read IO Config" );
				return;
			}

			ioConfig.wiegandMode = BS_IO_WIEGAND_MODE_EXTENDED;
			ioConfig.input[0] = BS_IO_INPUT_WIEGAND_CARD;
			ioConfig.input[1] = BS_IO_INPUT_WIEGAND_CARD;
			ioConfig.cardReaderID = (m_DeviceID * 16 + 14);

			result = BS_WriteIOConfig( m_Handle, &ioConfig );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();

				MessageBox( "Cannot write IO Config" );
				return;
			}
		}
		else if( m_DeviceType == BS_DEVICE_BIOENTRY_PLUS || m_DeviceType == BS_DEVICE_BIOENTRY_W)
		{
			BEConfigData config;
			int size;
			result = BS_ReadConfig( m_Handle, BEPLUS_CONFIG, &size, &config );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();

				MessageBox( "Cannot read config" );
				return;
			}

			config.wiegandMode = BEConfigData::WIEGAND_MODE_EXTENDED;
			config.useWiegandInput = true;
			config.useWiegandOutput = false;
			config.wiegandIdType = BEConfigData::WIEGAND_CARD;
			config.wiegandReaderID = (m_DeviceID * 16 + 14);

			result = BS_WriteConfig( m_Handle, BEPLUS_CONFIG, size, &config );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();

				MessageBox( "Cannot write config" );
				return;
			}
		}
		else if( m_DeviceType == BS_DEVICE_BIOLITE )
		{
			BEConfigDataBLN config;
			int size;
			result = BS_ReadConfig( m_Handle, BIOLITE_CONFIG, &size, &config );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();

				MessageBox( "Cannot read config" );
				return;
			}

			config.wiegandMode = BEConfigDataBLN::WIEGAND_MODE_EXTENDED;
			config.useWiegandInput = true;
			config.useWiegandOutput = false;
			config.wiegandIdType = BEConfigDataBLN::WIEGAND_CARD;
			config.wiegandReaderID = (m_DeviceID * 16 + 14);

			result = BS_WriteConfig( m_Handle, BIOLITE_CONFIG, size, &config );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();

				MessageBox( "Cannot write config" );
				return;
			}
		}


		result = BS_ReadRFCardIDEx( m_Handle, (m_DeviceID * 16 + 14), &cardID, &customID );
		if( result != BS_SUCCESS )
		{
			EndWaitCursor();

			MessageBox( "Cannot read the card" );
			return;
		}

		m_CardID = cardID;
		m_CustomID = customID;

		if( m_DeviceType == BS_DEVICE_BIOSTATION )		
		{
			BSIOConfig ioConfig;
			result = BS_ReadIOConfig( m_Handle, &ioConfig );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();

				MessageBox( "Cannot read IO Config" );
				return;
			}

			ioConfig.wiegandMode = BS_IO_WIEGAND_MODE_LEGACY;
			ioConfig.input[0] = BS_IO_INPUT_WIEGAND_CARD;
			ioConfig.input[1] = BS_IO_INPUT_WIEGAND_CARD;
			ioConfig.cardReaderID = 0;

			result = BS_WriteIOConfig( m_Handle, &ioConfig );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();

				MessageBox( "Cannot write IO Config" );
				return;
			}
		}
		if( m_DeviceType == BS_DEVICE_DSTATION      || 
            m_DeviceType == BS_DEVICE_XSTATION      || 
            m_DeviceType == BS_DEVICE_BIOSTATION2   ||
            m_DeviceType == BS_DEVICE_FSTATION)		
		{
			BSIOConfig ioConfig;
			result = BS_ReadIOConfig( m_Handle, &ioConfig );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();

				MessageBox( "Cannot read IO Config" );
				return;
			}

			ioConfig.wiegandMode = BS_IO_WIEGAND_MODE_LEGACY;
			ioConfig.input[0] = BS_IO_INPUT_WIEGAND_CARD;
			ioConfig.input[1] = BS_IO_INPUT_WIEGAND_CARD;
			ioConfig.cardReaderID = 0;

			result = BS_WriteIOConfig( m_Handle, &ioConfig );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();

				MessageBox( "Cannot write IO Config" );
				return;
			}
		}
		else if( m_DeviceType == BS_DEVICE_BIOENTRY_PLUS || m_DeviceType == BS_DEVICE_BIOENTRY_W)
		{
			BEConfigData config;
			int size;
			result = BS_ReadConfig( m_Handle, BEPLUS_CONFIG, &size, &config );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();

				MessageBox( "Cannot read config" );
				return;
			}

			config.wiegandMode = BEConfigData::WIEGAND_MODE_NORMAL;
			config.useWiegandInput = true;
			config.useWiegandOutput = false;
			config.wiegandIdType = BEConfigData::WIEGAND_CARD;
			config.wiegandReaderID = 0;

			result = BS_WriteConfig( m_Handle, BEPLUS_CONFIG, size, &config );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();

				MessageBox( "Cannot write config" );
				return;
			}
		}
		else if( m_DeviceType == BS_DEVICE_BIOLITE )
		{
			BEConfigDataBLN config;
			int size;
			result = BS_ReadConfig( m_Handle, BIOLITE_CONFIG, &size, &config );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();

				MessageBox( "Cannot read config" );
				return;
			}

			config.wiegandMode = BEConfigDataBLN::WIEGAND_MODE_NORMAL;
			config.useWiegandInput = true;
			config.useWiegandOutput = false;
			config.wiegandIdType = BEConfigDataBLN::WIEGAND_CARD;
			config.wiegandReaderID = 0;

			result = BS_WriteConfig( m_Handle, BIOLITE_CONFIG, size, &config );
			if( result != BS_SUCCESS )
			{
				EndWaitCursor();

				MessageBox( "Cannot write config" );
				return;
			}
		}
	}
	else
	{
		unsigned cardID;
		int customID;
		
		int result = BS_ReadCardIDEx( m_Handle, &cardID, &customID );

		if( result != BS_SUCCESS )
		{
			EndWaitCursor();

			MessageBox( "Cannot read the card" );
			return;
		}

		m_CardID = cardID;
		m_CustomID = customID;
	}
	UpdateData( FALSE );

	EndWaitCursor();
}

void CUserManagement::OnDestroy() 
{
	CDialog::OnDestroy();
	
	// TODO: Add your message handler code here
	if (m_TemplateData)	
	{
		free( m_TemplateData );
		m_TemplateData = NULL;
	}

	if (m_FaceTemplate_DST)
	{
		free( m_FaceTemplate_DST );
		m_FaceTemplate_DST = NULL;
	}

	if (m_FaceTemplate_FST)
	{
		free( m_FaceTemplate_FST );
		m_FaceTemplate_FST = NULL;
	}
}

