// UserManagement_FST.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "BioStarCPP.h"
#include "UserManagement_FST.h"
#include "Util.h"

#include <GdiPlus.h>
using namespace Gdiplus;
#pragma comment(lib, "GdiPlus")

ULONG_PTR m_gpToken;

// CUserManagement_FST dialog

IMPLEMENT_DYNAMIC(CUserManagement_FST, CDialog)

CUserManagement_FST::CUserManagement_FST(CWnd* pParent /*=NULL*/)
	: CDialog(CUserManagement_FST::IDD, pParent)
    , m_nFaceNum(0)
{
	//{{AFX_DATA_INIT(CUserManagement_FST)
	m_AdminLevel = -1;
	m_AuthMode = -1;
	m_CardID = 0;
	m_CardType = -1;
	m_Checksum1 = 0;
	m_Checksum2 = 0;
	m_CustomID = 0;
	m_Device = _T("");
	m_UserID = 0;
    m_Password = _T("");
	m_Name = _T("");
	m_NumOfFaceTemplate = 0;
	m_NumOfTemplate = 0;
	m_NumOfUser = 0;
	m_SecurityLevel = -1;
	m_AccessGroup = _T("");
	m_ExtRF = FALSE;
	//}}AFX_DATA_INIT

    memset(&m_userHdr, 0x00, sizeof(FSUserHdrEx));
    memset(m_Image, 0x00, sizeof(m_Image));
}

CUserManagement_FST::~CUserManagement_FST()
{
    GdiplusShutdown(m_gpToken);
}

void CUserManagement_FST::DoDataExchange(CDataExchange* pDX)
{
    CDialog::DoDataExchange(pDX);
    //{{AFX_DATA_MAP(CUserManagement_FST)
    DDX_Control(pDX, IDC_USER_LIST, m_UserList);
    DDX_CBIndex(pDX, IDC_ADMIN_LEVEL, m_AdminLevel);
    DDX_CBIndex(pDX, IDC_AUTH_MODE, m_AuthMode);
    DDX_Text(pDX, IDC_CARD_ID, m_CardID);
    DDX_Text(pDX, IDC_CUSTOM_ID, m_CustomID);
    DDX_CBIndex(pDX, IDC_CARD_TYPE, m_CardType);
    DDX_Text(pDX, IDC_DEVICE, m_Device);
    DDX_DateTimeCtrl(pDX, IDC_START_DATE, m_StartDate);
    DDX_DateTimeCtrl(pDX, IDC_EXPIRY_DATE, m_ExpiryDate);
    DDX_Text(pDX, IDC_ID, m_UserID);
    DDX_Text(pDX, IDC_PASSWORD, m_Password);
    DDX_Text(pDX, IDC_NAME, m_Name);
    DDX_Text(pDX, IDC_NUM_OF_FACE_TEMPLATE, m_NumOfFaceTemplate);
    DDX_Text(pDX, IDC_NUM_OF_USER, m_NumOfUser);
    DDX_CBIndex(pDX, IDC_SECURITY_LEVEL, m_SecurityLevel);
    DDX_Text(pDX, IDC_ACCESS_GROUP, m_AccessGroup);
    DDX_Check(pDX, IDC_CHECK_RF, m_ExtRF);
    DDX_Text(pDX, IDC_FACE_NUM, m_nFaceNum);
    DDX_Control(pDX, IDC_LIST_FACE, m_faceList);
    //}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CUserManagement_FST, CDialog)
	//{{AFX_MSG_MAP(CUserManagement_FST)
    ON_WM_PAINT()
	ON_NOTIFY(NM_CLICK, IDC_USER_LIST, OnClickUserList)
	ON_BN_CLICKED(IDC_DELETE, OnDelete)
	ON_BN_CLICKED(IDC_DELETE_ALL, OnDeleteAll)
	ON_BN_CLICKED(IDC_REFRESH, OnRefresh)
	ON_BN_CLICKED(IDC_UPDATE, OnUpdate)
	ON_BN_CLICKED(IDC_ADD, OnAdd)
	ON_BN_CLICKED(IDC_READ_CARD, OnReadCard)
	ON_WM_DESTROY()
    ON_BN_CLICKED(IDC_FST_SCAN_FACE, &CUserManagement_FST::OnBnClickedFstScanFace)
    ON_LBN_SELCHANGE(IDC_LIST_FACE, &CUserManagement_FST::OnLbnSelchangeListFace)
    ON_BN_CLICKED(IDC_BTN_ADD, &CUserManagement_FST::OnBnClickedBtnAdd)
    ON_BN_CLICKED(IDC_BTN_DELETE, &CUserManagement_FST::OnBnClickedBtnDelete)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


// CUserManagement_FST message handlers

BOOL CUserManagement_FST::OnInitDialog() 
{
	CDialog::OnInitDialog();

    GdiplusStartupInput gpsi;
    if (GdiplusStartup(&m_gpToken,&gpsi,NULL) != Ok) 
    {
    	AfxMessageBox(_T("Can't Initialize GDI+ Library."));
    	return FALSE;
    }
	
	// TODO: Add extra initialization here
	DWORD style = LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES;
	m_UserList.SendMessage(LVM_SETEXTENDEDLISTVIEWSTYLE, 0, LPARAM(style));

	m_UserList.InsertColumn( 1, "ID", LVCFMT_LEFT, 70, 0 );
	m_UserList.InsertColumn( 2, "Finger", LVCFMT_CENTER, 60, 1  );
	m_UserList.InsertColumn( 3, "Face", LVCFMT_CENTER, 60, 1  );
	m_UserList.InsertColumn( 4, "Card ID", LVCFMT_CENTER, 80, 2  );
	
	// TODO: Add extra initialization here
	char buf[32];

	sprintf( buf, "%d.%d.%d.%d(%u)", m_DeviceAddr & 0xff, (m_DeviceAddr & 0xff00) >> 8, (m_DeviceAddr & 0xff0000) >> 16, 
                                    (m_DeviceAddr & 0xff000000) >> 24, m_DeviceID );
	m_Device = buf;

	if( !getUserInfo() )
	{
		MessageBox( "Cannot get user info" );
	}

    FillAuthmodeData();

	return TRUE;  
}

void CUserManagement_FST::FillAuthmodeData()
{
    CComboBox *pCombo = (CComboBox*)GetDlgItem(IDC_AUTH_MODE);
    if (!pCombo) return;

    pCombo->ResetContent();

    pCombo->AddString("Face Only");
    pCombo->AddString("Face and Password");
    pCombo->AddString("Card Only");
    pCombo->AddString("Card and Password");
    pCombo->AddString("Card and Face");
    pCombo->AddString("Card and Face/Password");
    pCombo->SetCurSel(0);
}

void CUserManagement_FST::setDevice( int handle, unsigned deviceID, unsigned deviceAddr, int deviceType )
{
	m_Handle     = handle;
	m_DeviceID   = deviceID;
	m_DeviceAddr = deviceAddr;
	m_DeviceType = deviceType;
}

bool CUserManagement_FST::getUserInfo()
{
    BeginWaitCursor();

    int nUserID = 0;

	m_NumOfUser = 0;
	m_NumOfTemplate = 0;
	m_NumOfFaceTemplate = 0;

	m_UserList.DeleteAllItems();

	if( m_DeviceType == BS_DEVICE_FSTATION )
	{
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

	    FSUserHdrEx* userHdr = (FSUserHdrEx*)malloc( sizeof(FSUserHdrEx) * m_NumOfUser );
	    result = BS_GetAllUserInfoFStationEx( m_Handle, userHdr, (int*)&m_NumOfUser );
	    if( result != BS_SUCCESS && result != BS_ERR_NOT_FOUND )
	    {
		    EndWaitCursor();
		    free( userHdr );
		    return false;
	    }

        m_NumOfFaceTemplate = 0;

	    for( int i = 0; i < m_NumOfUser; i++ )
	    {
		    CString value;
		    value.Format( "%10u", userHdr[i].ID );
		    int listIndex = m_UserList.InsertItem( LVIF_TEXT | LVIF_PARAM, i, value, 0, 0, 0, userHdr[i].ID );

		    if( listIndex != -1 )
		    {
			    value = "0";    // FaceStation support no Fingerprint Template
			    m_UserList.SetItem( listIndex, 1, LVIF_TEXT, value, 0, 0, 0, 0 );

                value.Format( "%d", userHdr[i].numOfFaceType );
			    m_UserList.SetItem( listIndex, 2, LVIF_TEXT, value, 0, 0, 0, 0 );

			    value.Format( "%#x", userHdr[i].cardID );
			    m_UserList.SetItem( listIndex, 3, LVIF_TEXT, value, 0, 0, 0, 0 );
		    }

            m_NumOfFaceTemplate += userHdr[i].numOfFaceType;
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


bool CUserManagement_FST::getUserInfo( unsigned userID )
{
	m_CardID = 0;
	m_CustomID = 0;
    m_Face_FaceStation = FALSE;

    m_UserID = 0;
    m_Password = "";
	m_Name = "";
	m_AccessGroup = "";

    m_StartDate = COleDateTime::GetCurrentTime();
    m_ExpiryDate = COleDateTime::GetCurrentTime();

	m_AdminLevel = 0;
	m_SecurityLevel = 0;
	m_CardType = 0;
	m_AuthMode = 0;

    memset(m_Image, 0x00, sizeof(m_Image));
 
	BeginWaitCursor();

    if( m_DeviceType == BS_DEVICE_FSTATION )
	{
	    FSUserHdrEx userHdr;
        memset(&userHdr, 0x00, sizeof(FSUserHdrEx));

        unsigned char *imageData = (unsigned char*)malloc(BS_MAX_IMAGE_SIZE * BS_MAX_FACE_TYPE);
        unsigned char *faceTemplate = (unsigned char*)malloc(FACETEMPLATE_FST_SIZE * BS_MAX_FACE_TEMPLATE * BS_MAX_FACE_TYPE);

        BS_RET_CODE result = BS_GetUserFStationEx( m_Handle, userID, &userHdr, imageData, faceTemplate );

	    EndWaitCursor();

	    if( result != BS_SUCCESS )
	    {
            InvalidateRect( m_ImageRect );
		    UpdateData( FALSE );
		    return false;
	    }

        m_nFaceNum = userHdr.numOfFaceType;

        if (userHdr.numOfFaceType > 0)
            m_Face_FaceStation = TRUE;
        else
            m_Face_FaceStation = FALSE;

	    m_CardID    = userHdr.cardID;
	    m_CustomID  = userHdr.customID;
        m_UserID    = userHdr.ID;

        char szName[64] = {0};
		BS_UTF16ToString((const char*)userHdr.name, szName);
        m_Name = szName;
		
	    TIME_ZONE_INFORMATION timeInfo;
	    GetTimeZoneInformation( &timeInfo );

        time_t nStart = ConvertToUTCTime( userHdr.startDateTime );
        if(nStart < 0) nStart = 0;
        ConvertTimeToDate(nStart, m_StartDate);

        time_t nEnd = ConvertToUTCTime( userHdr.expireDateTime );
        if(nEnd < 0) nEnd = 0;
        ConvertTimeToDate(nEnd, m_ExpiryDate);

        m_AdminLevel    = (userHdr.adminLevel == FSUserHdr::USER_ADMIN)? 1 : 0;
	    m_SecurityLevel = (userHdr.securityLevel >= BS_USER_SECURITY_DEFAULT)? userHdr.securityLevel - BS_USER_SECURITY_DEFAULT : 0;
	    m_CardType      = userHdr.bypassCard;
	    m_AuthMode      = userHdr.authMode;

	    char buf[16];
	    sprintf( buf, "%#0x", userHdr.accessGroupMask );
	    m_AccessGroup = buf;


        int nIndex = 0;
        int imagePos = 0, templatePos = 0;
        int nTemplateLen = 0;
        CString strText;
        m_faceList.ResetContent();

        for (int i=0; i < userHdr.numOfFaceType; i++)
        {
            strText.Format("%d : template Num= %02d updated= %d", i+1, userHdr.numOfFace[i], userHdr.numOfUpdatedFace[i]);
            m_faceList.AddString(strText);

            //stillcut
            memcpy( &m_StillcutData[i], imageData + imagePos, userHdr.faceStillcutLen[i] );
            imagePos += userHdr.faceStillcutLen[i];

            //facetemplate
            nTemplateLen = 0;
            for (int k=0; k < userHdr.numOfFace[i]; k++)
                nTemplateLen += userHdr.faceLen[i][k];

            memcpy( &m_FaceTemplate[i], faceTemplate + templatePos, nTemplateLen );
            templatePos += nTemplateLen;
        }

        memcpy(&m_userHdr, &userHdr, sizeof(FSUserHdrEx));
	}

    InvalidateRect( m_ImageRect );
	
    UpdateData( FALSE );

	return true;
}

void CUserManagement_FST::OnClickUserList(NMHDR* pNMHDR, LRESULT* pResult) 
{
	*pResult = 0;

	POSITION pos = m_UserList.GetFirstSelectedItemPosition();

	if( pos )
	{
		int userIndex   = m_UserList.GetNextSelectedItem( pos );
		unsigned userID = m_UserList.GetItemData( userIndex );

		if( !getUserInfo( userID ) )
		{
			MessageBox( "Cannot get user info" );
		}
	}
}

void CUserManagement_FST::OnDelete() 
{
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

void CUserManagement_FST::OnDeleteAll() 
{
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

void CUserManagement_FST::OnRefresh() 
{
	if( !getUserInfo() )
	{
		MessageBox( "Cannot get user info" );
	}	
}

void CUserManagement_FST::OnBnClickedFstScanFace()
{
    int nIndex = m_faceList.GetCurSel();
    if (nIndex < 0) 
    {
        AfxMessageBox("There is no selected item");
        return;
    }

	BeginWaitCursor();

    FSUserTemplateHdr userTemplateHdr = {0};

    unsigned char *imageData = (unsigned char*)malloc(BS_MAX_IMAGE_SIZE);
    unsigned char *faceTemplate = (unsigned char*)malloc(FACETEMPLATE_FST_SIZE * BS_MAX_FACE_TEMPLATE);

    int nRet = 0;
    do
    {
        int result = BS_ScanFaceTemplate( m_Handle, &userTemplateHdr, imageData, faceTemplate );
        if (result != BS_SUCCESS)
        {
            CString  errMsg = "Error Capture User Face from FaceStation!!!\r\nTry capture again?";
            nRet = AfxMessageBox(errMsg, MB_RETRYCANCEL|MB_ICONSTOP);
            if (nRet == IDCANCEL) 
            {
                free(imageData);
                free(faceTemplate);
                EndWaitCursor();

                return;
            }
        }
        else
        {
            int faceTemplateLen = 0;
            int nOffset = 0;
            unsigned nChecksum = 0;

            m_userHdr.numOfFace[nIndex]        = userTemplateHdr.numOfFace;
            m_userHdr.numOfUpdatedFace[nIndex] = userTemplateHdr.numOfUpdatedFace;
            m_userHdr.faceStillcutLen[nIndex]  = userTemplateHdr.imageSize;

			 
            for (int i=0; i < BS_MAX_FACE_TEMPLATE; i++)
            {
                m_userHdr.faceLen[nIndex][i] = userTemplateHdr.faceLen[i];
                faceTemplateLen += userTemplateHdr.faceLen[i];

                // face template's checksum
                int nLen = userTemplateHdr.faceLen[i];
                if (nLen > 0)
                {
                    unsigned char* templateBuf = faceTemplate + nOffset;
                    for( int j=0; j < userTemplateHdr.faceLen[i]; j++ )
                    {
                        nChecksum += templateBuf[j];
                    }
                    m_userHdr.faceChecksum[nIndex][i] = nChecksum;
                    nOffset += userTemplateHdr.faceLen[i];
 
                }
            }

            memcpy(&m_StillcutData[nIndex], imageData, userTemplateHdr.imageSize);
            memcpy(&m_FaceTemplate[nIndex], faceTemplate, faceTemplateLen);
			memcpy(&m_Image, imageData, userTemplateHdr.imageSize );		
			SaveFaceImage(m_Image, userTemplateHdr.imageSize ); 
			InvalidateRect( m_ImageRect );  
			m_userHdr.numOfFaceType++; 

            break;
        }

    } while (nRet == IDRETRY);
 
    free(imageData);
    free(faceTemplate);

    EndWaitCursor();
}

void CUserManagement_FST::OnUpdate() 
{
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

    for(int i=0; i < m_Password.GetLength(); i++)
    {
        if( !::isdigit( m_Password.GetAt(i) ) )
        {
            AfxMessageBox("Password permit digit only.");
            return;
        }
    }

	BeginWaitCursor();

    FSUserHdrEx userHdr = {0};
    memcpy(&userHdr, &m_userHdr, sizeof(FSUserHdrEx));

    unsigned char *imageData = (unsigned char*)malloc( BS_MAX_IMAGE_SIZE * BS_MAX_FACE_TYPE );   
    unsigned char *faceTemplate = (unsigned char*)malloc( FACETEMPLATE_FST_SIZE * BS_MAX_FACE_TEMPLATE * BS_MAX_FACE_TYPE ); 


    //face Stillcut
    int nCount = 0;
    int stillcutPos = 0; 
    for (int i=0; i < FSUserHdrEx::MAX_FACE_TYPE; i++)
    {
        if (userHdr.faceStillcutLen[i] > 0)
        {
            memcpy(imageData + stillcutPos, &m_StillcutData[i], m_userHdr.faceStillcutLen[i]); 
            stillcutPos += m_userHdr.faceStillcutLen[i];

            userHdr.faceStillcutLen[nCount++] = m_userHdr.faceStillcutLen[i];
        }
    } 

    //face template
    nCount = 0;
    int templatePos = 0;

    userHdr.numOfFaceType = 0;

    for (int i=0; i < FSUserHdrEx::MAX_FACE_TYPE; i++)
    {
        int len = 0;
        for (int k=0; k < FSUserHdrEx::MAX_FACE; k++)
        {
            len += userHdr.faceLen[i][k];
        }

        if (len > 0)
        { 

            memcpy(faceTemplate + templatePos, &m_FaceTemplate[i], len);
            templatePos += len;  
            userHdr.numOfFaceType++;
			
			userHdr.numOfFace[nCount]        = m_userHdr.numOfFace[i]; 
			userHdr.numOfUpdatedFace[nCount] = m_userHdr.numOfUpdatedFace[i];  
			userHdr.faceUpdatedIndex[nCount] = m_userHdr.faceUpdatedIndex[i]; 
			userHdr.faceStillcutLen[nCount]  = m_userHdr.faceStillcutLen[i];  

			TRACE( "numofFace : %d\n", userHdr.numOfFace[nCount]  ); 
			TRACE( "numOfUpdatedFace: %d\n", userHdr.numOfUpdatedFace[nCount]  ); 
			TRACE( "faceUpdatedIndex: %d\n", userHdr.faceUpdatedIndex[nCount]  ); 
			TRACE( "faceStillcutLen: %d\n", userHdr.faceStillcutLen[nCount]  ); 

            for (int k=0; k < FSUserHdrEx::MAX_FACE; k++)
            {
                userHdr.faceLen[nCount][k]      = m_userHdr.faceLen[i][k]; //faceLen
                userHdr.faceChecksum[nCount][k] = m_userHdr.faceChecksum[i][k];    //face Checksum

				if( k >= 20 &&  userHdr.faceLen[nCount][k] > 0 ) 
					TRACE( "%d : len :%d  checksum:%d \n",k , userHdr.faceLen[nCount][k], userHdr.faceChecksum[nCount][k] ); 
            }
            nCount++;
        }
    }

	//initilize dummy facetemplete data 
	for(int i=nCount; i<FSUserHdrEx::MAX_FACE_TYPE; i++)
	{
		userHdr.numOfFace[i]        = 0; 
		userHdr.numOfUpdatedFace[i] = 0;  
		userHdr.faceUpdatedIndex[i] = 0; 
		userHdr.faceStillcutLen[i] = 0; 
		 for (int k=0; k < FSUserHdrEx::MAX_FACE; k++)
         {
             userHdr.faceLen[i][k]      = 0; //faceLen
             userHdr.faceChecksum[i][k] = 0; //face Checksum
		 }
	}  

    //name
    wsprintf((char*)userHdr.name, "%s", m_Name);
    BS_ConvertToUTF16((const char*)userHdr.name, (char*)userHdr.name, sizeof(userHdr.name)-1);

    //pwd
    BS_EncryptSHA256((unsigned char*)m_Password.GetBuffer(0), m_Password.GetLength(), (unsigned char*)userHdr.password);


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

    BS_RET_CODE result = BS_EnrollUserFStationEx( m_Handle, &userHdr, imageData, faceTemplate );

    if( result != BS_SUCCESS )
    {
        free(imageData);
        free(faceTemplate);
	    EndWaitCursor();
	    MessageBox( "Cannot enroll the user" );
	    return;
    }

    free(imageData);
    free(faceTemplate);

	EndWaitCursor();	
}

void CUserManagement_FST::OnAdd() 
{
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

    for(int i=0; i < m_Password.GetLength(); i++)
    {
        if( !::isdigit( m_Password.GetAt(i) ) )
        {
            AfxMessageBox("Password permit digit only.");
            return;
        }
    }


	BeginWaitCursor();

	BS_RET_CODE result;

    unsigned char *StillcutData = (unsigned char*)malloc( BS_MAX_IMAGE_SIZE * BS_MAX_FACE_TYPE );   
    unsigned char *FaceTemplateData = (unsigned char*)malloc( FACETEMPLATE_FST_SIZE * BS_MAX_FACE_TEMPLATE * BS_MAX_FACE_TYPE ); 

    FSUserHdrEx userHdr;
    memset( &userHdr, 0, sizeof( FSUserHdrEx ) );

    BOOL bNeedScan = FALSE;
    if (IDYES == AfxMessageBox("Do you want to scan a new face ?", MB_YESNO))
    { 
        bNeedScan = TRUE;
    }

    if (bNeedScan == TRUE)
    {
        unsigned char *imageData = (unsigned char*)malloc(BS_MAX_IMAGE_SIZE);
        unsigned char *faceTemplate = (unsigned char*)malloc(FACETEMPLATE_FST_SIZE * BS_MAX_FACE_TEMPLATE);

        FSUserTemplateHdr userTemplateHdr = {0};

        int nRet = 0;
        int nStillcutBufPos = 0;
        int nTemplateBufPos = 0;
        int nOffset = 0;

        for (int i=0; i < BS_MAX_FACE_TYPE; i++)
        {
            do 
            {
                nRet = 0;
                memset(&userTemplateHdr, 0x00, sizeof(FSUserTemplateHdr));

                result = BS_ScanFaceTemplate( m_Handle, &userTemplateHdr, imageData, faceTemplate );
                if (result != BS_SUCCESS)
                { 
                    CString  errMsg = "Error Capture User Face from FaceStation!!!\r\nTry capture again?";
                    nRet = AfxMessageBox(errMsg, MB_RETRYCANCEL|MB_ICONSTOP);
                    if (nRet == IDCANCEL) 
                    {
                        break;
                    }
                }
            } while (nRet == IDRETRY);

            if (nRet == 0 && result == BS_SUCCESS && userTemplateHdr.numOfFace > 0)
            {
                //scan success
                userHdr.numOfFaceType++;
                userHdr.numOfFace[i]        = userTemplateHdr.numOfFace;
                userHdr.numOfUpdatedFace[i] = userTemplateHdr.numOfUpdatedFace;

                //face len
                for (int k=0; k < FSUserHdrEx::MAX_FACE; k++)
                {
                    userHdr.faceLen[i][k] = userTemplateHdr.faceLen[k];
                }

                //face checksum
                nOffset = 0;
                unsigned nChecksum = 0;
                for( int k=0; k < FSUserHdrEx::MAX_FACE; k++ )
                {
	                unsigned char* templateBuf = faceTemplate + nOffset;
	                for( int j=0; j < userTemplateHdr.faceLen[k]; j++ )
	                {
                        nChecksum += templateBuf[j];
	                }
                    userHdr.faceChecksum[i][k] = nChecksum;
	                nOffset += userTemplateHdr.faceLen[k];
                }

                //stillcut length
                userHdr.faceStillcutLen[i] = userTemplateHdr.imageSize;

                //fill the Stillcut image data
                memcpy( StillcutData + nStillcutBufPos, imageData, userTemplateHdr.imageSize);
                nStillcutBufPos     += userTemplateHdr.imageSize;

                //fill the facetemplate data
                nOffset = 0;
                for (int k=0; k < FSUserHdrEx::MAX_FACE; k++)
                {
	                memcpy( FaceTemplateData + nTemplateBufPos, faceTemplate + nOffset, userTemplateHdr.faceLen[k]);

	                nTemplateBufPos += userTemplateHdr.faceLen[k];
	                nOffset         += userTemplateHdr.faceLen[k];
                }
            }

            Sleep(500);     // The delay is required that is more than five miliseconds.

            if (IDYES != AfxMessageBox("Do you want to continue scanning ?", MB_YESNO))
            {
                break;
            }
        }

		free(imageData);
        free(faceTemplate);
	}
       
    wsprintf((char*)userHdr.name, "%s", m_Name);
    BS_ConvertToUTF16((const char*)userHdr.name, (char*)userHdr.name, sizeof(userHdr.name)-1);

    userHdr.ID				= m_UserID;

    //pwd
    BS_EncryptSHA256((unsigned char*)m_Password.GetBuffer(0), m_Password.GetLength(), (unsigned char*)userHdr.password);


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

    result = BS_EnrollUserFStationEx( m_Handle, &userHdr, StillcutData, FaceTemplateData );

    if( result != BS_SUCCESS )
    {
        free (StillcutData);
        free (FaceTemplateData);

        EndWaitCursor();
	    MessageBox( "Cannot enroll the user" );
	    return;
    }

    free (StillcutData);
    free (FaceTemplateData);

	EndWaitCursor();	
}

void CUserManagement_FST::OnReadCard() 
{
	// TODO: Add your control notification handler code here
	UpdateData();

	BeginWaitCursor();

	if( m_ExtRF )
	{
		unsigned cardID;
		int customID;
		int result;

		if( m_DeviceType == BS_DEVICE_FSTATION )
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

		result = BS_ReadRFCardIDEx( m_Handle, (m_DeviceID * 16 + 14), &cardID, &customID );
		if( result != BS_SUCCESS )
		{
			EndWaitCursor();

			MessageBox( "Cannot read the card" );
			return;
		}

		m_CardID = cardID;
		m_CustomID = customID;


		if( m_DeviceType == BS_DEVICE_FSTATION )		
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

void CUserManagement_FST::OnDestroy() 
{
	CDialog::OnDestroy();
}

void CUserManagement_FST::OnPaint() 
{
    CPaintDC dc(this);

    DrawFaceImage(&dc);
}

void CUserManagement_FST::DrawFaceImage(CDC* pDC) 
{
    GetDlgItem( IDC_IMG_FACE1 )->GetWindowRect( m_ImageRect ); 
    ScreenToClient( m_ImageRect );

    TCHAR tmpPath[MAX_PATH] = {0,};
    TCHAR NewPath[MAX_PATH] = {0,};
    TCHAR pImageName[MAX_PATH] = {0,};

    if ( m_Image[0] )
    {
        GetTempPath(MAX_PATH, tmpPath);
        wsprintf(NewPath, "%sBioStar_Sample\\", tmpPath);
        memcpy(tmpPath, NewPath, MAX_PATH);
        wsprintf(pImageName, "%sResampleImage.jpg", tmpPath);
        CStringW wstr(pImageName);

        Graphics G(*pDC);
        Image *pI;

        pI = Image::FromFile(wstr);
        G.DrawImage(pI, m_ImageRect.left, m_ImageRect.top, m_ImageRect.Width(), m_ImageRect.Height());
        delete pI;
    }
    else
    {
        int	 nOption = 1;  // 1 for center, 2 for Stretch, And 3 for title
        DrawBitmap(pDC->GetSafeHdc(), IDB_NOIMG, m_ImageRect, nOption);  
    }
}

// 1 for center, 2 for Stretch, And 3 for title
BOOL CUserManagement_FST::DrawBitmap(HDC hDC, UINT nIDResource, CRect rect, int nOption) 
{
    CBitmap bitmap;
    bitmap.LoadBitmap(nIDResource);
    if(!bitmap.m_hObject) return FALSE;

    HDC memDC = ::CreateCompatibleDC(hDC);    
    ::SelectObject(memDC, (HGDIOBJ)bitmap.GetSafeHandle());

    BITMAP bmap;
    bitmap.GetBitmap(&bmap);

    int bmW = bmap.bmWidth;
    int bmH = bmap.bmHeight;

    int x=rect.left, y=rect.top;

    switch(nOption) 
    {
        case 1:
            ::StretchBlt(hDC, x, y, rect.Width(), rect.Height(), memDC, 0, 0, bmW, bmH, SRCCOPY);
            break;
        case 2:
            (bmW < rect.Width())  ? x = (rect.Width() - bmW)/2  : x = rect.left;
            (bmH < rect.Height()) ? y = (rect.Height() - bmH)/2 : y = rect.top;
            ::BitBlt(hDC, x, y, rect.Width(), rect.Height(), memDC, 0, 0, SRCCOPY);
            break;
        case 3:
            for(y = rect.top; y < rect.top + rect.Height(); y += bmH) 
                for(x = rect.left; x < rect.left + rect.Width(); x += bmW) 
                    ::BitBlt(hDC, x, y, rect.Width(), rect.Height(), memDC, 0, 0, SRCCOPY);
            break;
    }

    bitmap.DeleteObject(); 

    return TRUE;
}

void CUserManagement_FST::SaveFaceImage(unsigned char *pImage, int nImageLen)
{
    // save original image
    TCHAR tmpPath[MAX_PATH] = {0,};
    TCHAR NewPath[MAX_PATH] = {0,};
    TCHAR pImageName[MAX_PATH] = {0,};
    TCHAR pThumbImage[MAX_PATH] = {0,};

    GetTempPath(MAX_PATH, tmpPath);
    wsprintf(NewPath, "%sBioStar_Sample\\", tmpPath);
    ::CreateDirectory(NewPath, NULL);
    memcpy(tmpPath, NewPath, MAX_PATH);
    wsprintf(pThumbImage, "%sFaceImage.jpg", tmpPath);

    FILE *fp=NULL;
    if((fp=fopen(pThumbImage, "wb")) != NULL)
    {
        fwrite(pImage, sizeof(char), nImageLen, fp);
        fclose(fp);
    }

    // resample small image
    float width  = 240;
    float height = 320;

    Bitmap TargetImg((INT)width, (INT)height, PixelFormat24bppRGB);
    CLSID Clsid;

    Image *pI;
    CStringW ImagePath(pThumbImage);
    pI=Image::FromFile(ImagePath);

    // Rotate right 90'
    pI->RotateFlip(Rotate90FlipNone);

    GetEncCLSID(L"image/jpeg",&Clsid);

    RectF rtTarget;
    rtTarget.X		= 0.0;
    rtTarget.Y		= 0.0;
    rtTarget.Width  = (REAL)width;
    rtTarget.Height = (REAL)height;

    Graphics g(&TargetImg);
    g.Clear(Color(255, 255, 255, 255));
    g.SetInterpolationMode( InterpolationModeHighQualityBicubic );
    g.DrawImage(pI, rtTarget, 0, 0, (REAL)pI->GetWidth(), (REAL)pI->GetHeight(), UnitPixel);

    GetTempPath(MAX_PATH, tmpPath);
    wsprintf(NewPath, "%sBioStar_Sample\\", tmpPath);
    ::CreateDirectory(NewPath, NULL);
    memcpy(tmpPath, NewPath, MAX_PATH);
    wsprintf(pImageName, "%sResampleImage.jpg", tmpPath);

    CStringW resampleImage(pImageName);
    TargetImg.Save(resampleImage, &Clsid, NULL);

    delete pI;
}

BOOL CUserManagement_FST::GetEncCLSID(WCHAR *mime, CLSID *pClsid)
{
    UINT num,size,i;
    ImageCodecInfo *arCod;
    BOOL bFound=FALSE;

    GetImageEncodersSize(&num,&size);
    arCod=(ImageCodecInfo *)malloc(size);
    GetImageEncoders(num,size,arCod);

    for (i=0;i<num;i++) 
    {
        if(wcscmp(arCod[i].MimeType,mime)==0) 
        {
            *pClsid=arCod[i].Clsid;
            bFound=TRUE;
            break;
        }    
    }
    free(arCod);
    return bFound;
}

void CUserManagement_FST::OnLbnSelchangeListFace()
{
    UpdateData(TRUE);

    memset(m_Image, 0x00, sizeof(m_Image));
    InvalidateRect( m_ImageRect );

    int nIndex = m_faceList.GetCurSel();
    if (nIndex < 0) return;

	if( m_userHdr.faceStillcutLen[nIndex]>0 ) 
	{
        memcpy( m_Image, &m_StillcutData[nIndex], m_userHdr.faceStillcutLen[nIndex]);
        SaveFaceImage(m_Image, m_userHdr.faceStillcutLen[nIndex]); 
	} 

    InvalidateRect( m_ImageRect );
}

void CUserManagement_FST::OnBnClickedBtnAdd()
{
    int nCount = m_faceList.GetCount();
    if (nCount >= BS_MAX_FACE_TYPE)
    {
        AfxMessageBox("Cannot add more face. Maximum face is up to 5.");
        return;
    }

    CString strText;
    strText.Format("%d : template Num= %02d updated= %d", nCount+1, 0, 0);

    m_faceList.AddString(strText); 
}

void CUserManagement_FST::OnBnClickedBtnDelete()
{
    UpdateData(TRUE);

    int nDeleteIndex = m_faceList.GetCurSel();
    if (nDeleteIndex < 0) return;

    m_faceList.DeleteString(nDeleteIndex); 

	int i=0; 
	for( i=nDeleteIndex ; i < m_faceList.GetCount() ; i++ ) 
	{ 
		int nTemplateLen = 0;
        for (int k=0; k < BS_MAX_FACE_TEMPLATE; k++) 
        {
            nTemplateLen += m_userHdr.faceLen[i+1][k];
        }

        memcpy((void*)&m_FaceTemplate[i], m_FaceTemplate[i+1], nTemplateLen);    
		memcpy((void*)&m_StillcutData[i], m_StillcutData[i+1], m_userHdr.faceStillcutLen[i+1]);   
		m_userHdr.numOfFace[i]        = m_userHdr.numOfFace[i+1]; 
		m_userHdr.numOfUpdatedFace[i] = m_userHdr.numOfUpdatedFace[i+1];  
		m_userHdr.faceUpdatedIndex[i] = m_userHdr.faceUpdatedIndex[i+1]; 
		m_userHdr.faceStillcutLen[i]  = m_userHdr.faceStillcutLen[i+1];  
		for (int k=0; k < FSUserHdrEx::MAX_FACE; k++)
        {
            m_userHdr.faceLen[i][k]      = m_userHdr.faceLen[i+1][k];			 
            m_userHdr.faceChecksum[i][k] = m_userHdr.faceChecksum[i+1][k];     
        }
	}

	//initilize dummy facetemplate data 
	for(  ;i < FSUserHdrEx::MAX_FACE_TYPE; i++ )  
	{  
		m_userHdr.numOfFace[i]        = 0; 
		m_userHdr.numOfUpdatedFace[i] = 0;  
		m_userHdr.faceUpdatedIndex[i] = 0; 
		m_userHdr.faceStillcutLen[i]  = 0; 
		for (int k=0; k < FSUserHdrEx::MAX_FACE; k++)
        {
            m_userHdr.faceLen[i][k]      = 0;			 
            m_userHdr.faceChecksum[i][k] = 0;     
        }
	}

	m_Image[0] = 0;  
    m_userHdr.numOfFaceType = i - 1;  
	
    InvalidateRect(m_ImageRect); 
}
