// RS485Management.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "BioStarCPP.h"
#include "RS485Management.h"
#include "BioStarCPPDlg.h"

#include <winsock2.h>


#define TEMPLATE_SIZE		384


// RS485Management 대화 상자입니다.

IMPLEMENT_DYNAMIC(RS485Management, CDialog)

RS485Management::RS485Management(CWnd* pParent /*=NULL*/)
	: CDialog(RS485Management::IDD, pParent)
{
	//{{AFX_DATA_INIT(CLogManagement)
	m_Device = _T("");
	m_NumOfSlave = 0;
	//}}AFX_DATA_INIT
}

RS485Management::~RS485Management()
{
}

void RS485Management::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CLogManagement)
	DDX_Control(pDX, IDC_SLAVE_LIST, m_SlaveList);
	DDX_Text(pDX, IDC_DEVICE, m_Device);
	DDX_Text(pDX, IDC_NUM_OF_SLAVE, m_NumOfSlave);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(RS485Management, CDialog)
    ON_BN_CLICKED(IDC_SEARCH, OnBnClickedSearch)
    ON_BN_CLICKED(IDC_ADD_USER, OnBnClickedAddUser)
    ON_BN_CLICKED(IDC_DELETE_USER, OnBnClickedDeleteUser)
END_MESSAGE_MAP()


// RS485Management 메시지 처리기입니다.

BOOL RS485Management::OnInitDialog()
{
    CDialog::OnInitDialog();

 	DWORD style = LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES;
	m_SlaveList.SendMessage(LVM_SETEXTENDEDLISTVIEWSTYLE, 0, LPARAM(style));

	m_SlaveList.InsertColumn( 1, "Device ID", LVCFMT_LEFT, 300, 0 );
	m_SlaveList.InsertColumn( 2, "Device Type", LVCFMT_LEFT, 300, 1 );


	char buf[32];

	sprintf( buf, "%d.%d.%d.%d(%u)", m_DeviceAddr & 0xff, (m_DeviceAddr & 0xff00) >> 8, 
		(m_DeviceAddr & 0xff0000) >> 16, (m_DeviceAddr & 0xff000000) >> 24, m_DeviceID );

	m_Device = buf;

    UpdateData(FALSE);

    return TRUE;  
}

CString RS485Management::getDeviceString(int nType)
{
    switch (nType)
    {
        case BS_DEVICE_BIOSTATION:
            return CString("BioStation");
        case BS_DEVICE_BIOENTRY_PLUS:
            return CString("BioEntry Plus");
        case BS_DEVICE_BIOENTRY_W:
            return CString("BioEntry W");
        case BS_DEVICE_BIOLITE:
            return CString("BioLite Net");
        case BS_DEVICE_XPASS:
            return CString("Xpass");
        case BS_DEVICE_XPASS_SLIM:
            return CString("Xpass Slim");
        case BS_DEVICE_DSTATION:
            return CString("D-Station");
        case BS_DEVICE_XSTATION:
            return CString("X-Station");
        case BS_DEVICE_BIOSTATION2:
            return CString("BioStation T2");
        case BS_DEVICE_FSTATION:
            return CString("FaceStation");
        case BS_DEVICE_XPASS_SLIM2:
            return CString("Xpass S2");

    }
    return CString("Unknown");
}


void RS485Management::setDevice( int handle, unsigned deviceID, unsigned deviceAddr, int deviceType )
{
	m_Handle = handle;
	m_DeviceID = deviceID;
	m_DeviceAddr = deviceAddr;
	m_DeviceType = deviceType;
}

void RS485Management::OnBnClickedSearch()
{
   
    BS485SlaveInfo slaveInfo[8];    

    int numOfSlave = 0; 

    CString  strID, strType;

    BS_RET_CODE result = BS_Search485Slaves( m_Handle, slaveInfo, &numOfSlave ); 
    
    for( int i = 0; i < numOfSlave; i++ ) 
    { 
        strID.Format( "%u", slaveInfo[i].slaveID );
        strType.Format( "%s", getDeviceString(slaveInfo[i].slaveType));
		
        int listIndex = m_SlaveList.InsertItem( LVIF_TEXT | LVIF_PARAM, i, strID, 0, 0, 0, slaveInfo[i].slaveID ); 
		
		if( listIndex != -1 )
		{
			m_SlaveList.SetItem( listIndex, 1, LVIF_TEXT, strType, 0, 0, 0, 0 );
		}        
    }
}

void RS485Management::OnBnClickedAddUser()
{
    // TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.

	BEUserHdr userHdr;

	memset( &userHdr, 0, sizeof( BEUserHdr ) );


	unsigned char *m_TemplateData = (unsigned char*)malloc( TEMPLATE_SIZE * 2 * BS_MAX_TEMPLATE_PER_USER );
    

	userHdr.numOfFinger = 0;

	userHdr.userID = 55556;
	userHdr.cardID = 0;
	userHdr.cardCustomID = 0;
	userHdr.cardVersion = BEUserHdr::CARD_VERSION_1;

	userHdr.startTime = 0;
	userHdr.expiryTime = 0;

	userHdr.adminLevel = BS_USER_NORMAL;
	userHdr.securityLevel = BS_USER_SECURITY_DEFAULT;
	userHdr.cardFlag = 1;
	
	userHdr.opMode = BS_AUTH_FINGER_ONLY - 1;
	userHdr.accessGroupMask = 0xffffffff;

    // set Slave-Device to Target-Device
	POSITION pos = m_SlaveList.GetFirstSelectedItemPosition();
	if( pos )
	{
		int nIndex = m_SlaveList.GetNextSelectedItem( pos );
		unsigned deviceID = (unsigned)m_SlaveList.GetItemData( nIndex );

	    BS_RET_CODE result = BS_SetDeviceID( m_Handle, deviceID, 1);

	    result = BS_EnrollUserBEPlus( m_Handle, &userHdr, m_TemplateData );

	    if( result != BS_SUCCESS )
	    {
		    EndWaitCursor();
		    MessageBox( "Cannot enroll the user" );
		    return;
	    }            
	}

	EndWaitCursor();
}

void RS485Management::OnBnClickedDeleteUser()
{
	UpdateData();

	BeginWaitCursor();

    unsigned UserID = 55556;

	POSITION pos = m_SlaveList.GetFirstSelectedItemPosition();

	if( pos )
	{
		int nIndex = m_SlaveList.GetNextSelectedItem( pos );

		unsigned deviceID = (unsigned)m_SlaveList.GetItemData( nIndex );


	    BS_RET_CODE result = BS_SetDeviceID( m_Handle, deviceID, 1);
            
        result = BS_DeleteUser( m_Handle, UserID );

	    if( result != BS_SUCCESS )
	    {
            EndWaitCursor();
		    
            MessageBox( "Cannot delete user" );
		    return;
	    }
	}

	EndWaitCursor();
}


