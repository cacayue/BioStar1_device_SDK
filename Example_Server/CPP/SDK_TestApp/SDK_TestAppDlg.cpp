// SDK_TestAppDlg.cpp : implementation of the CSDK_TestAppDlg class
//

#include "stdafx.h"
#include "SDK_TestApp.h"
#include "SDK_TestAppDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


CRITICAL_SECTION g_cs;


// CAboutDlg dialog used for App About

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// Dialog Data
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

// Implementation
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
END_MESSAGE_MAP()


// CSDK_TestAppDlg dialog

CSDK_TestAppDlg::CSDK_TestAppDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CSDK_TestAppDlg::IDD, pParent)
	, m_Port(0)
	, m_Connections(0)
	, m_UseAutoResponse(FALSE)
	, m_UseFunctionLock(FALSE)
	, m_UseLock(FALSE)
	, m_MatchingFail(FALSE)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);

    InitializeCriticalSection(&g_cs);
}

void CSDK_TestAppDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, ID_DEVICE_LIST, m_DeviceList);
	DDX_Text(pDX, ID_EDIT_PORT, m_Port);
	DDX_Text(pDX, ID_EDIT_CONNECTION, m_Connections);
	DDX_Check(pDX, ID_CHECK_AUTO_RESPONSE, m_UseAutoResponse);
	DDX_Check(pDX, ID_CHECK_FUNCTION_LOCK, m_UseFunctionLock);
	DDX_Check(pDX, ID_CHECK_LOCK, m_UseLock);
	DDX_Check(pDX, ID_CHK_MATCH, m_MatchingFail);
}

BEGIN_MESSAGE_MAP(CSDK_TestAppDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_BN_CLICKED(ID_BTN_START, &CSDK_TestAppDlg::OnBnClickedBtnStart)
	ON_BN_CLICKED(ID_BTN_STOP, &CSDK_TestAppDlg::OnBnClickedBtnStop)
	ON_BN_CLICKED(ID_BTN_REQUEST, &CSDK_TestAppDlg::OnBnClickedBtnRequest)
	ON_BN_CLICKED(ID_BTN_ISSUE, &CSDK_TestAppDlg::OnBnClickedBtnIssue)
	ON_BN_CLICKED(ID_BTN_DELETE, &CSDK_TestAppDlg::OnBnClickedBtnDelete)
    ON_MESSAGE(UM_UPDATE, &CSDK_TestAppDlg::OnUmUpdate)
	ON_WM_DESTROY()
END_MESSAGE_MAP()


// CSDK_TestAppDlg message handlers

BOOL CSDK_TestAppDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	SetIcon(m_hIcon, TRUE);			
	SetIcon(m_hIcon, FALSE);		


    // Your source code inserted here !!

	BS_InitSDK();

	DWORD style = LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES;
	m_DeviceList.SendMessage(LVM_SETEXTENDEDLISTVIEWSTYLE, 0, LPARAM(style));

	m_DeviceList.InsertColumn(1, _T("ID"), LVCFMT_CENTER, 60, 0);
	m_DeviceList.InsertColumn(2, _T("Handle"), LVCFMT_CENTER, 80, 1);
	m_DeviceList.InsertColumn(3, _T("IP"), LVCFMT_CENTER, 120, 2);
	m_DeviceList.InsertColumn(4, _T("Type"), LVCFMT_CENTER, 95, 3);
	m_DeviceList.InsertColumn(5, _T("Conn"), LVCFMT_CENTER, 90, 4);
	m_DeviceList.InsertColumn(6, _T("Status"), LVCFMT_CENTER, 200, 5);

	m_Port = 5001;
	m_Connections = 32;

	UpdateData(FALSE);


	DWORD threadID;
	HANDLE hLogThread = CreateThread( NULL, NULL, CSDK_TestAppDlg::ThreadProc, this, NULL, &threadID );

	return TRUE;  
}

void CSDK_TestAppDlg::OnDestroy()
{
	CDialog::OnDestroy();

	BS_UnInitSDK();
}

void CSDK_TestAppDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

void CSDK_TestAppDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); 

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

HCURSOR CSDK_TestAppDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CSDK_TestAppDlg::OnBnClickedBtnStart()
{
	UpdateData();

	//Set event procedure.
	BS_SetConnectedCallback( ConnectedProc, (bool)m_UseFunctionLock, (bool)m_UseAutoResponse );
	BS_SetDisconnectedCallback( DisconnectedProc, (bool)m_UseFunctionLock );
	BS_SetRequestStartedCallback( RequestStartProc, (bool)m_UseFunctionLock, (bool)m_UseAutoResponse );	
    BS_SetLogCallback( LogProc, m_UseFunctionLock, (bool)m_UseAutoResponse );
    BS_SetImageLogCallback( ImageLogProc, m_UseFunctionLock, (bool)m_UseAutoResponse );
    BS_SetRequestUserInfoCallback( RequestUserInfoProc, (bool)m_UseFunctionLock );
    BS_SetRequestMatchingCallback( RequestMatchingProc, (bool)m_UseFunctionLock );

	BS_SetSynchronousOperation( m_UseLock );

	BS_RET_CODE result = BS_StartServerApp( m_Port, m_Connections, "C:\\OpenSSL\\bin\\openssl.exe", "12345678" );
		
    if( result == BS_SUCCESS )
    {
        SetWindowText(_T("Server SDK Test Started ..."));
    }
	else
	{
        MessageBox(_T("BS_StartServerApp Fail!"));
	}

	m_Count = 0;
}

void CSDK_TestAppDlg::OnBnClickedBtnStop()
{
	BS_StopServerApp();


    EnterCriticalSection(&g_cs);
    m_DeviceList.DeleteAllItems();
    LeaveCriticalSection(&g_cs);

    SetWindowText(_T("Server SDK Test Stoped ..."));
}


DWORD CSDK_TestAppDlg::ThreadProc( void* arg )
{
	CSDK_TestAppDlg *handler = (CSDK_TestAppDlg *)arg;

	int nItem = -1;
	int handle = 0;
	int nStatus = 0;
    int nCount = 0;

	CString itemString;

	while( true )
	{
        EnterCriticalSection(&g_cs);
        nCount = handler->m_DeviceList.GetItemCount();
        LeaveCriticalSection(&g_cs);


        for(int nItem=0; nItem < nCount; nItem++)
		{
            EnterCriticalSection(&g_cs);
			handle = _ttoi( handler->m_DeviceList.GetItemText( nItem, LIST_HANDLE_POS ) );

			if( handler->m_DeviceList.GetItemText( nItem, LIST_TYPE_POS ) == _T("D-Station") )
			{
				nStatus = BS_CheckSystemStatus(handle);
			}
            LeaveCriticalSection(&g_cs);

            Sleep(1000);
		}

        Sleep(1000);
	}
}

LRESULT CSDK_TestAppDlg::OnUmUpdate(WPARAM wParam, LPARAM lParam)
{
    BOOL bFlag = FALSE;
    if (lParam > 0)
        bFlag = TRUE;

    UpdateData(lParam);

    return 1L;
}

BS_RET_CODE __stdcall ConnectedProc( int handle, unsigned deviceID, int deviceType, int connectionType, int functionType, char* ipAddress )
{
    EnterCriticalSection(&g_cs);

	CListCtrl* listCtrl = &((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->m_DeviceList;

	int itemCount = listCtrl->GetItemCount();
	int i = 0;	

	for( i = 0; i < itemCount; i++ )
	{
		if( _ttoi( listCtrl->GetItemText( i, LIST_ID_POS ) ) == deviceID 
			&& _ttoi( listCtrl->GetItemText( i, LIST_HANDLE_POS ) ) == handle )
		{
			break;
		}
	}

	CString itemString;
		
	if( i == itemCount )
	{
		itemString.Format( _T("%d"), deviceID );
		listCtrl->InsertItem( LVIF_TEXT , i, itemString, 0, 0, 0, 0 );

		((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->m_Count++;
	}

	itemString.Format( _T("255.255.255.255") );		
	MultiByteToWideChar( CP_ACP, 0, ipAddress, -1, itemString.GetBuffer(), itemString.GetLength() );
	listCtrl->SetItem( i, LIST_IP_POS, LVIF_TEXT, itemString, 0, 0, 0, 0 );

	itemString.Format( _T("%d"), handle );
	listCtrl->SetItem( i, LIST_HANDLE_POS, LVIF_TEXT, itemString, 0, 0, 0, 0 );

	switch ( deviceType )
    {
        case BS_DEVICE_FSTATION:
    		itemString.Format( _T("FaceStation") );
            break;
        case BS_DEVICE_BIOSTATION2:
    		itemString.Format( _T("BioStation T2") );
            break;
        case BS_DEVICE_DSTATION:
    		itemString.Format( _T("D-Station") );
	        break;
        case BS_DEVICE_XSTATION:
    		itemString.Format( _T("X-Station") );
            break;
        case BS_DEVICE_BIOSTATION:
			itemString.Format( _T("BioStation") );
            break;
        case BS_DEVICE_BIOENTRY_PLUS:
    		itemString.Format( _T("BioEntry") );
	        break;
        case BS_DEVICE_BIOENTRY_W:
    		itemString.Format( _T("BioEntry W") );
	        break;
        case BS_DEVICE_BIOLITE:
    		itemString.Format( _T("BioLiteNet") );
            break;
        case BS_DEVICE_XPASS:
    		itemString.Format( _T("Xpass") );
            break;
        case BS_DEVICE_XPASS_SLIM:
    		itemString.Format( _T("Xpass Slim") );
            break;
        case BS_DEVICE_XPASS_SLIM2:
    		itemString.Format( _T("Xpass S2") );
            break;
        default:
    		itemString.Format( _T("Unknown") );
            break;
    }
	
	listCtrl->SetItem( i, LIST_TYPE_POS, LVIF_TEXT, itemString, 0, 0, 0, 0 );

	if( connectionType == 1 )
	{
		itemString.Format( _T("SSL") );
	}
	else
	{
		itemString.Format( _T("Normal") );
	}

	listCtrl->SetItem( i, LIST_CONNECTION_POS, LVIF_TEXT, itemString, 0, 0, 0, 0 );

	itemString.Format( _T("Connected...") );
	listCtrl->SetItem( i, LIST_STATUS_POS, LVIF_TEXT, itemString, 0, 0, 0, 0 );

	itemString.Format(_T("Connected : %d"), ((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->m_Count );
	((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->SetWindowText( itemString );
	
    LeaveCriticalSection(&g_cs);

	return BS_SUCCESS;
}

BS_RET_CODE __stdcall RequestStartProc( int handle, unsigned deviceID, int deviceType, int connectionType, int functionType, char* ipAddress )
{
    EnterCriticalSection(&g_cs);

	CListCtrl* listCtrl = &((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->m_DeviceList;

	int itemCount = listCtrl->GetItemCount();
	int i = 0;	

	CString itemString;
	
	for( i = 0; i < itemCount; i++ )
	{
		if( _ttoi( listCtrl->GetItemText( i, LIST_ID_POS ) ) == deviceID 
			&& _ttoi( listCtrl->GetItemText( i, LIST_HANDLE_POS ) ) == handle )
		{
			itemString.Format( _T("Request Started...") );
			listCtrl->SetItem( i, LIST_STATUS_POS, LVIF_TEXT, itemString, 0, 0, 0, 0 );

			break;
		}
	}

    LeaveCriticalSection(&g_cs);

	return BS_SUCCESS;
}

BS_RET_CODE __stdcall DisconnectedProc( int handle, unsigned deviceID, int deviceType, int connectionType, int functionType, char* ipAddress )
{
    EnterCriticalSection(&g_cs);

    CListCtrl* listCtrl = &((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->m_DeviceList;

	int itemCount = listCtrl->GetItemCount();
	int i = 0;	
	
	for( i = 0; i < itemCount; i++ )
	{
		if( _ttoi( listCtrl->GetItemText( i, LIST_ID_POS ) ) == deviceID 
			&& _ttoi( listCtrl->GetItemText( i, LIST_HANDLE_POS ) ) == handle )
		{
			listCtrl->DeleteItem( i );
			((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->m_Count--;

			break;
		}
	}

	CString itemString;
	itemString.Format(_T("Disconnected : %d"), ((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->m_Count );
	((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->SetWindowText( itemString );

    LeaveCriticalSection(&g_cs);

	return BS_SUCCESS;
}


BS_RET_CODE __stdcall LogProc( int handle, unsigned deviceID, int deviceType, int connectionType, void* data )
{
    EnterCriticalSection(&g_cs);

    CListCtrl* listCtrl = &((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->m_DeviceList;

    if( deviceType == BS_DEVICE_FSTATION || 
        deviceType == BS_DEVICE_BIOSTATION2 || 
        deviceType == BS_DEVICE_DSTATION || 
        deviceType == BS_DEVICE_XSTATION )
    {
        BSLogRecordEx* logRecord = (BSLogRecordEx *)data;

	    CString itemString;
	    for(int i = 0; i < listCtrl->GetItemCount(); i++ )
	    {
		    if( _ttoi( listCtrl->GetItemText( i, LIST_ID_POS ) ) == deviceID 
			    && _ttoi( listCtrl->GetItemText( i, LIST_HANDLE_POS ) ) == handle )
		    {
			    itemString.Format( _T("Log Event : 0x%02x"), logRecord->event );
			    listCtrl->SetItem( i, LIST_STATUS_POS, LVIF_TEXT, itemString, 0, 0, 0, 0 );

                if( logRecord->imageSlot > BSLogRecordEx::NO_IMAGE )
                {
                    TRACE2("There is a Image log at event=%lu time=%lu\r\n", logRecord->event, logRecord->eventTime);
                }

			    break;
		    }
	    }
    }
    else
    {
        BSLogRecord* logRecord = (BSLogRecord *)data;

	    CString itemString;
	    for(int i = 0; i < listCtrl->GetItemCount(); i++ )
	    {
		    if( _ttoi( listCtrl->GetItemText( i, LIST_ID_POS ) ) == deviceID 
			    && _ttoi( listCtrl->GetItemText( i, LIST_HANDLE_POS ) ) == handle )
		    {
			    itemString.Format( _T("Log Event : 0x%02x"), logRecord->event );
			    listCtrl->SetItem( i, LIST_STATUS_POS, LVIF_TEXT, itemString, 0, 0, 0, 0 );
			    break;
		    }
	    }
    }

    LeaveCriticalSection(&g_cs);

	return BS_SUCCESS;
}


BS_RET_CODE __stdcall ImageLogProc( int handle, unsigned deviceID, int deviceType, int connectionType, void *data, int nSize )
{
    if( deviceType == BS_DEVICE_FSTATION || 
        deviceType == BS_DEVICE_BIOSTATION2 ||
        deviceType == BS_DEVICE_DSTATION || 
        deviceType == BS_DEVICE_XSTATION )
    {
		BSImageLogHdr ImageLogHdr = {0};
		memcpy(&ImageLogHdr, data, sizeof(ImageLogHdr));	

        unsigned char *pImageData = (unsigned char *)data + sizeof(ImageLogHdr);
        
        CStdioFile file;
        CString strFilename;
        
        strFilename.Format(_T("C:\\temp\\%lu_%lu_%lu.jpg"), ImageLogHdr.deviceID, ImageLogHdr.event, ImageLogHdr.eventTime);
        if( file.Open(strFilename, CFile::modeCreate|CFile::modeWrite|CFile::typeBinary) )
        {
            file.Write(pImageData, ImageLogHdr.imageSize);
            file.Close();
        }
    }

    return BS_SUCCESS;
}


BS_RET_CODE __stdcall RequestMatchingProc( int handle, unsigned deviceID, int deviceType, int connectionType, 
                                           int matchingType, unsigned ID, unsigned char* templateData, void* userHdr, int* isDuress )
{
    EnterCriticalSection(&g_cs);
    
    unsigned userID;
	CListCtrl* listCtrl = &((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->m_DeviceList;

	int itemCount = listCtrl->GetItemCount();
	int i = 0;	

	CString itemString;
	
	for( i = 0; i < itemCount; i++ )
	{
		if( _ttoi( listCtrl->GetItemText( i, LIST_ID_POS ) ) == deviceID 
			&& _ttoi( listCtrl->GetItemText( i, LIST_HANDLE_POS ) ) == handle )
		{
			if( matchingType == REQUEST_IDENTIFY )
			{
				itemString.Format( _T("1:N Matching") );
				userID = 100;
			}
			else
			{
				itemString.Format( _T("1:1 Matching") );
				userID = ID;
			}
			
			listCtrl->SetItem( i, LIST_STATUS_POS, LVIF_TEXT, itemString, 0, 0, 0, 0 );
			break;
		}
	}

	((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->SendMessage(UM_UPDATE, 0, 1);
	if( ((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->m_MatchingFail )
	{
        LeaveCriticalSection(&g_cs);
		return BS_ERR_NOT_FOUND;
	}

	if( deviceType == BS_DEVICE_BIOENTRY_PLUS || deviceType == BS_DEVICE_BIOENTRY_W)
	{
		BEUserHdr* beUserHdr = (BEUserHdr*)userHdr;

		beUserHdr->version = 0;
		beUserHdr->userID = userID;
		beUserHdr->startTime = 0;
		beUserHdr->expiryTime = 0;
		beUserHdr->cardID = 0;

		beUserHdr->cardCustomID = 0;
		beUserHdr->commandCardFlag = 0;
		beUserHdr->cardFlag = 0;
		beUserHdr->cardVersion = BEUserHdr::CARD_VERSION_1;

		beUserHdr->adminLevel = BEUserHdr::USER_LEVEL_NORMAL;
		beUserHdr->securityLevel = BEUserHdr::USER_SECURITY_DEFAULT;

		beUserHdr->accessGroupMask = 0xFFFFFFFF;

		beUserHdr->numOfFinger = 1;

	}
	else if( deviceType == BS_DEVICE_BIOLITE )
	{
		BEUserHdr* beUserHdr = (BEUserHdr*)userHdr;

		beUserHdr->version = 0;
		beUserHdr->userID = userID;
		beUserHdr->startTime = 0;
		beUserHdr->expiryTime = 0;
		beUserHdr->cardID = 0;

		beUserHdr->cardCustomID = 0;
		beUserHdr->commandCardFlag = 0;
		beUserHdr->cardFlag = 0;
		beUserHdr->cardVersion = BEUserHdr::CARD_VERSION_1;

		beUserHdr->adminLevel = BEUserHdr::USER_LEVEL_NORMAL;
		beUserHdr->securityLevel = BEUserHdr::USER_SECURITY_DEFAULT;

		beUserHdr->accessGroupMask = 0xFFFFFFFF;

		beUserHdr->numOfFinger = 1;
	}
	else if( deviceType == BS_DEVICE_BIOSTATION )
	{
		BSUserHdrEx* bsUserHdr = (BSUserHdrEx*)userHdr;

		bsUserHdr->ID = userID;
		bsUserHdr->accessGroupMask = 0xffffffff; // no access group
		strcpy( bsUserHdr->name, "Test User" );
		strcpy( bsUserHdr->department, "R&D" );
		bsUserHdr->adminLevel = BS_USER_NORMAL;
		bsUserHdr->securityLevel = BS_USER_SECURITY_DEFAULT;
		bsUserHdr->duressMask = 0x00; // no duress finger
		bsUserHdr->numOfFinger = 1;
    }
    else if( deviceType == BS_DEVICE_DSTATION )
	{
		DSUserHdr* bsUserHdr = (DSUserHdr*)userHdr;

		bsUserHdr->ID = userID;
		bsUserHdr->accessGroupMask = 0xffffffff; // no access group
		strcpy((char*) bsUserHdr->name, "Test User" );
		strcpy((char*)  bsUserHdr->department, "R&D" );
		bsUserHdr->adminLevel = BS_USER_NORMAL;
		bsUserHdr->securityLevel = BS_USER_SECURITY_DEFAULT;
		bsUserHdr->numOfFinger = 1;
    }
    else if( deviceType == BS_DEVICE_XSTATION )
	{
		XSUserHdr* bsUserHdr = (XSUserHdr*)userHdr;

		bsUserHdr->ID = userID;
		bsUserHdr->accessGroupMask = 0xffffffff; // no access group
		strcpy((char*)  bsUserHdr->name, "Test User" );
		strcpy((char*)  bsUserHdr->department, "R&D" );
		bsUserHdr->adminLevel = BS_USER_NORMAL;
		bsUserHdr->securityLevel = BS_USER_SECURITY_DEFAULT;
    }
    else if( deviceType == BS_DEVICE_BIOSTATION2 )
	{
		BS2UserHdr* bsUserHdr = (BS2UserHdr*)userHdr;

		bsUserHdr->ID = userID;
		bsUserHdr->accessGroupMask = 0xffffffff; // no access group
		strcpy((char*) bsUserHdr->name, "Test User" );
		strcpy((char*)  bsUserHdr->department, "R&D" );
		bsUserHdr->adminLevel = BS_USER_NORMAL;
		bsUserHdr->securityLevel = BS_USER_SECURITY_DEFAULT;
		bsUserHdr->numOfFinger = 1;
    }

	*isDuress = NORMAL_FINGER;

    LeaveCriticalSection(&g_cs);

	return BS_SUCCESS;
}

BS_RET_CODE __stdcall RequestUserInfoProc( int handle, unsigned deviceID, int deviceType, int connectionType, 
                                           int idType, unsigned ID, unsigned customID, void* userHdr )
{
    EnterCriticalSection(&g_cs);

    CListCtrl* listCtrl = &((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->m_DeviceList;

 	int itemCount = listCtrl->GetItemCount();
	int i = 0;	

	CString itemString;
	
	for( i = 0; i < itemCount; i++ )
	{
		if( _ttoi( listCtrl->GetItemText( i, LIST_ID_POS ) ) == deviceID 
			&& _ttoi( listCtrl->GetItemText( i, LIST_HANDLE_POS ) ) == handle )
		{
			if( idType == ID_USER )
			{
				itemString.Format( _T("ID Request") );
			}
			else
			{
				itemString.Format( _T("Card ID Request") );
			}
			
			listCtrl->SetItem( i, LIST_STATUS_POS, LVIF_TEXT, itemString, 0, 0, 0, 0 );
			break;
		}
	}

	((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->SendMessage(UM_UPDATE, 0, 1);
	if( ((CSDK_TestAppDlg*)AfxGetApp()->m_pMainWnd)->m_MatchingFail )
	{
        LeaveCriticalSection(&g_cs);
		return BS_ERR_NOT_FOUND;
	}

	if( deviceType == BS_DEVICE_BIOENTRY_PLUS || deviceType == BS_DEVICE_BIOENTRY_W)
	{
		BEUserHdr* beUserHdr = (BEUserHdr*)userHdr;

		beUserHdr->version = 0;
		beUserHdr->userID = ID;
		beUserHdr->startTime = 0;
		beUserHdr->expiryTime = 0;
		beUserHdr->cardID = 0;

		beUserHdr->cardCustomID = 0;
		beUserHdr->commandCardFlag = 0;
		beUserHdr->cardFlag = 0;
		beUserHdr->cardVersion = BEUserHdr::CARD_VERSION_1;

		beUserHdr->adminLevel = BEUserHdr::USER_LEVEL_NORMAL;
		beUserHdr->securityLevel = BEUserHdr::USER_SECURITY_DEFAULT;

		beUserHdr->accessGroupMask = 0xFFFFFFFF;

		beUserHdr->numOfFinger = 1;
	}
	else if( deviceType == BS_DEVICE_XPASS || deviceType == BS_DEVICE_XPASS_SLIM || deviceType == BS_DEVICE_XPASS_SLIM2)
	{
		BEUserHdr* beUserHdr = (BEUserHdr*)userHdr;

		beUserHdr->version = 0;
		beUserHdr->userID = ID;
		beUserHdr->startTime = 0;
		beUserHdr->expiryTime = 0;
		beUserHdr->cardID = 0;

		beUserHdr->cardCustomID = 0;
		beUserHdr->commandCardFlag = 0;
		beUserHdr->cardFlag = 0;
		beUserHdr->cardVersion = BEUserHdr::CARD_VERSION_1;

		beUserHdr->adminLevel = BEUserHdr::USER_LEVEL_NORMAL;
		beUserHdr->securityLevel = BEUserHdr::USER_SECURITY_DEFAULT;

		beUserHdr->accessGroupMask = 0xFFFFFFFF;

		beUserHdr->numOfFinger = 1;
	}
	else if( deviceType == BS_DEVICE_BIOLITE )
	{
		BEUserHdr* beUserHdr = (BEUserHdr*)userHdr;

        switch (idType)
        {
            case ID_USER:
                {
		            beUserHdr->version = 0;
		            beUserHdr->userID = ID;
		            beUserHdr->startTime = 0;
		            beUserHdr->expiryTime = 0;
		            beUserHdr->cardID = 0;

		            beUserHdr->cardCustomID = 0;
		            beUserHdr->commandCardFlag = 0;
		            beUserHdr->cardFlag = 0;
		            beUserHdr->cardVersion = BEUserHdr::CARD_VERSION_1;

		            beUserHdr->adminLevel = BEUserHdr::USER_LEVEL_NORMAL;
		            beUserHdr->securityLevel = BEUserHdr::USER_SECURITY_DEFAULT;

		            beUserHdr->accessGroupMask = 0xFFFFFFFF;

		            beUserHdr->numOfFinger = 1;
                }
                break;
            case ID_CARD:
                {
		            beUserHdr->version = 0;
		            beUserHdr->userID = ID;
		            beUserHdr->startTime = 0;
		            beUserHdr->expiryTime = 0;
		            beUserHdr->cardID = 0;

		            beUserHdr->cardCustomID = 0;
		            beUserHdr->commandCardFlag = 0;
		            beUserHdr->cardFlag = 1;
		            beUserHdr->cardVersion = BEUserHdr::CARD_VERSION_1;

		            beUserHdr->adminLevel = BEUserHdr::USER_LEVEL_NORMAL;
		            beUserHdr->securityLevel = BEUserHdr::USER_SECURITY_DEFAULT;

		            beUserHdr->accessGroupMask = 0xFFFFFFFF;

		            beUserHdr->numOfFinger = 1;
                }
                break;
        }
	}
	else if( deviceType == BS_DEVICE_BIOSTATION )
	{
		BSUserHdrEx* bsUserHdr = (BSUserHdrEx*)userHdr;

		bsUserHdr->ID = ID;
		bsUserHdr->accessGroupMask = 0xffffffff; // no access group
		strcpy( bsUserHdr->name, "Test User" );
		strcpy( bsUserHdr->department, "R&D" );
		bsUserHdr->adminLevel = BS_USER_NORMAL;
		bsUserHdr->securityLevel = BS_USER_SECURITY_DEFAULT;
		bsUserHdr->duressMask = 0x00; // no duress finger
		bsUserHdr->numOfFinger = 1;
	}
	else if( deviceType == BS_DEVICE_DSTATION )
	{
		DSUserHdr* bsUserHdr = (DSUserHdr*)userHdr;

		bsUserHdr->ID = ID;
		bsUserHdr->accessGroupMask = 0xffffffff; // no access group
		strcpy((char*)  bsUserHdr->name, "Test User" );
		strcpy((char*)  bsUserHdr->department, "R&D" );
		bsUserHdr->adminLevel = BS_USER_NORMAL;
		bsUserHdr->securityLevel = BS_USER_SECURITY_DEFAULT;
		bsUserHdr->numOfFinger = 1;
	}
	else if( deviceType == BS_DEVICE_XSTATION )
	{
		XSUserHdr* bsUserHdr = (XSUserHdr*)userHdr;

		bsUserHdr->ID = ID;
		bsUserHdr->accessGroupMask = 0xffffffff; // no access group
		strcpy((char*)  bsUserHdr->name, "Test User" );
		strcpy((char*)  bsUserHdr->department, "R&D" );
		bsUserHdr->adminLevel = BS_USER_NORMAL;
		bsUserHdr->securityLevel = BS_USER_SECURITY_DEFAULT;
	}
	else if( deviceType == BS_DEVICE_BIOSTATION2 )
	{
		BS2UserHdr* bsUserHdr = (BS2UserHdr*)userHdr;

		bsUserHdr->ID = ID;
		bsUserHdr->accessGroupMask = 0xffffffff; // no access group
		strcpy((char*)  bsUserHdr->name, "Test User" );
		strcpy((char*)  bsUserHdr->department, "R&D" );
		bsUserHdr->adminLevel = BS_USER_NORMAL;
		bsUserHdr->securityLevel = BS_USER_SECURITY_DEFAULT;
		bsUserHdr->numOfFinger = 1;
	}

    LeaveCriticalSection(&g_cs);
	
	return BS_SUCCESS;
}

void CSDK_TestAppDlg::OnBnClickedBtnRequest()
{
    EnterCriticalSection(&g_cs);

    CString itemString;

	POSITION pos = m_DeviceList.GetFirstSelectedItemPosition();
	while( pos )
	{
		int nItem = m_DeviceList.GetNextSelectedItem(pos);
		int handle = _ttoi( m_DeviceList.GetItemText( nItem, LIST_HANDLE_POS ) );
		int deviceType = -1;

        CString text = m_DeviceList.GetItemText( nItem, LIST_TYPE_POS );

        if      (text == _T("FaceStation"))	    deviceType = BS_DEVICE_FSTATION;
        else if (text == _T("BioStation T2"))	deviceType = BS_DEVICE_BIOSTATION2;
        else if (text == _T("D-Station"))		deviceType = BS_DEVICE_DSTATION;
		else if (text == _T("X-Station"))		deviceType = BS_DEVICE_XSTATION;
		else if (text == _T("BioStation"))		deviceType = BS_DEVICE_BIOSTATION;
		else if (text == _T("BioLiteNet"))		deviceType = BS_DEVICE_BIOLITE;
		else if (text == _T("BioEntry"))		deviceType = BS_DEVICE_BIOENTRY_PLUS;
		else if (text == _T("BioEntry W"))		deviceType = BS_DEVICE_BIOENTRY_W;
        else if (text == _T("Xpass"))	    	deviceType = BS_DEVICE_XPASS;
        else if (text == _T("Xpass Slim"))	   	deviceType = BS_DEVICE_XPASS_SLIM;
        else if (text == _T("Xpass S2"))	   	deviceType = BS_DEVICE_XPASS_SLIM2;
        else                                    deviceType = -1;


		if( BS_StartRequest( handle, deviceType, m_Port ) == BS_SUCCESS )
		{
			itemString.Format( _T("StartRequest Success") ); 
		}
		else
		{
			itemString.Format( _T("StartRequest Fail") ); 
		}

		m_DeviceList.SetItem( nItem, LIST_STATUS_POS, LVIF_TEXT, itemString, 0, 0, 0, 0 );			
	}			

    LeaveCriticalSection(&g_cs);
}

void CSDK_TestAppDlg::OnBnClickedBtnIssue()
{
    EnterCriticalSection(&g_cs);

    CString itemString;

	POSITION pos = m_DeviceList.GetFirstSelectedItemPosition();
	while( pos )
	{
		int nItem = m_DeviceList.GetNextSelectedItem(pos);
		int handle = _ttoi( m_DeviceList.GetItemText( nItem, LIST_HANDLE_POS ) );
		int deviceID = _ttoi( m_DeviceList.GetItemText( nItem, LIST_ID_POS ) );

		if( m_DeviceList.GetItemText( nItem, LIST_TYPE_POS ) != _T("BioStation") )
		{
			itemString.Format( _T("Invalid Device Type") ); 
			m_DeviceList.SetItem( nItem, LIST_STATUS_POS, LVIF_TEXT, itemString, 0, 0, 0, 0 );		

			break;
		}

		if( BS_IssueCertificate( handle, deviceID ) == BS_SUCCESS )
		{
			itemString.Format( _T("IssueCert Success") ); 
		}
		else
		{
			itemString.Format( _T("IssueCert Fail") ); 
		}

		m_DeviceList.SetItem( nItem, LIST_STATUS_POS, LVIF_TEXT, itemString, 0, 0, 0, 0 );			
	}		

    LeaveCriticalSection(&g_cs);
}

void CSDK_TestAppDlg::OnBnClickedBtnDelete()
{
    EnterCriticalSection(&g_cs);

    CString itemString;

	POSITION pos = m_DeviceList.GetFirstSelectedItemPosition();
	while( pos )
	{
		int nItem = m_DeviceList.GetNextSelectedItem(pos);
		int handle = _ttoi( m_DeviceList.GetItemText( nItem, LIST_HANDLE_POS ) );

		if( m_DeviceList.GetItemText( nItem, LIST_TYPE_POS ) != _T("BioStation") )
		{
			itemString.Format( _T("Invalid Device Type") ); 
			m_DeviceList.SetItem( nItem, LIST_STATUS_POS, LVIF_TEXT, itemString, 0, 0, 0, 0 );	

			break;
		}

		if( BS_DeleteCertificate( handle ) == BS_SUCCESS )
		{
			itemString.Format( _T("DeleteCert Success") ); 
		}
		else
		{
			itemString.Format( _T("DeleteCert Fail") ); 
		}

		m_DeviceList.SetItem( nItem, LIST_STATUS_POS, LVIF_TEXT, itemString, 0, 0, 0, 0 );			
	}			

    LeaveCriticalSection(&g_cs);
}
