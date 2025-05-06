// InterActiveTestDlg.cpp : 구현 파일
//

#include "stdafx.h"
#include "InterActiveTest.h"
#include "InterActiveTestDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// 응용 프로그램 정보에 사용되는 CAboutDlg 대화 상자입니다.

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

// 구현입니다.
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


// CInterActiveTestDlg 대화 상자




CInterActiveTestDlg::CInterActiveTestDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CInterActiveTestDlg::IDD, pParent)
	, m_IpAddress(_T(""))
	, m_Port(0)
	, m_SoundIndex(0)
	, m_UseDisplayImage(FALSE)
	, m_UseKeyImage(FALSE)
	, m_DisplayText(_T(""))
	, m_KeyText(_T(""))
	, m_DisplayTime(0)
	, m_WaitKeyTime(0)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CInterActiveTestDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Text(pDX, ID_ED_IP, m_IpAddress);
	DDX_Text(pDX, ID_ED_PORT, m_Port);
	DDX_Text(pDX, IDC_ED_SOUND_INDEX, m_SoundIndex);
	DDX_Check(pDX, ID_CHK_USE_IMAGE, m_UseDisplayImage);
	DDX_Check(pDX, IDC_CHK_KEY_IMAGE, m_UseKeyImage);
	DDX_Text(pDX, ID_ED_DISPLAY, m_DisplayText);
	DDX_Text(pDX, ID_ED_KEY, m_KeyText);
	DDX_Text(pDX, IDC_EDIT2, m_DisplayTime);
	DDX_Text(pDX, IDC_EDIT3, m_WaitKeyTime);
}

BEGIN_MESSAGE_MAP(CInterActiveTestDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_BN_CLICKED(IDOK, &CInterActiveTestDlg::OnBnClickedOk)
	ON_BN_CLICKED(ID_BTN_CONN_DEVICE, &CInterActiveTestDlg::OnBnClickedBtnConnDevice)
	ON_BN_CLICKED(ID_BTN_DISPLAY, &CInterActiveTestDlg::OnBnClickedBtnDisplay)
	ON_BN_CLICKED(ID_BTN_CUSTOM_SOUND, &CInterActiveTestDlg::OnBnClickedBtnCustomSound)
	ON_BN_CLICKED(ID_BTN_STOP_DISPLAY, &CInterActiveTestDlg::OnBnClickedBtnStopDisplay)
	ON_BN_CLICKED(IDC_BTN_PLAY, &CInterActiveTestDlg::OnBnClickedBtnPlay)
	ON_BN_CLICKED(ID_BTN_WAIT_KEY, &CInterActiveTestDlg::OnBnClickedBtnWaitKey)
	ON_BN_CLICKED(ID_BTN_DISCONN_DEVICE, &CInterActiveTestDlg::OnBnClickedBtnDisconnDevice)
	ON_WM_DESTROY()
END_MESSAGE_MAP()


// CInterActiveTestDlg 메시지 처리기

BOOL CInterActiveTestDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// 시스템 메뉴에 "정보..." 메뉴 항목을 추가합니다.

	// IDM_ABOUTBOX는 시스템 명령 범위에 있어야 합니다.
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

	// 이 대화 상자의 아이콘을 설정합니다. 응용 프로그램의 주 창이 대화 상자가 아닐 경우에는
	//  프레임워크가 이 작업을 자동으로 수행합니다.
	SetIcon(m_hIcon, TRUE);			// 큰 아이콘을 설정합니다.
	SetIcon(m_hIcon, FALSE);		// 작은 아이콘을 설정합니다.

	// TODO: 여기에 추가 초기화 작업을 추가합니다.
	BS_InitSDK();

	return TRUE;  // 포커스를 컨트롤에 설정하지 않으면 TRUE를 반환합니다.
}

void CInterActiveTestDlg::OnDestroy()
{
	CDialog::OnDestroy();

	BS_UnInitSDK();
}

void CInterActiveTestDlg::OnSysCommand(UINT nID, LPARAM lParam)
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

// 대화 상자에 최소화 단추를 추가할 경우 아이콘을 그리려면
//  아래 코드가 필요합니다. 문서/뷰 모델을 사용하는 MFC 응용 프로그램의 경우에는
//  프레임워크에서 이 작업을 자동으로 수행합니다.

void CInterActiveTestDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 그리기를 위한 디바이스 컨텍스트

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 클라이언트 사각형에서 아이콘을 가운데에 맞춥니다.
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 아이콘을 그립니다.
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// 사용자가 최소화된 창을 끄는 동안에 커서가 표시되도록 시스템에서
//  이 함수를 호출합니다.
HCURSOR CInterActiveTestDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}


void CInterActiveTestDlg::OnBnClickedOk()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	return;
	OnOK();
}

void CInterActiveTestDlg::OnBnClickedBtnConnDevice()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	UpdateData();

	char ipAddress[16];

	memset( ipAddress, 0x00, sizeof(ipAddress) );
	WideCharToMultiByte( CP_ACP, 0, m_IpAddress.GetBuffer(), m_IpAddress.GetLength(), ipAddress, sizeof(ipAddress), NULL, NULL ); 
	BS_RET_CODE result = BS_OpenSocket( ipAddress, m_Port, &m_Handle );

	if( result != BS_SUCCESS )
	{
		TCHAR msg[128];
		wsprintf( msg, _T("Can not Open Socket: %d"), result );
		MessageBox( msg );
		return;
	}

	unsigned deviceID;
	int type;
	result = BS_GetDeviceID( m_Handle, &deviceID, &type );

	if( result != BS_SUCCESS )
	{
		TCHAR msg[128];
		wsprintf( msg, _T("Can get device ID: %d"), result );
		MessageBox( msg );
		return;
	}

	if( type  >  0 )
	{
		TCHAR msg[128];
		wsprintf( msg, _T("Can not support this device"));
		MessageBox( msg );
		return;
	}

	BS_SetDeviceID( m_Handle, deviceID, type );
}

void CInterActiveTestDlg::OnBnClickedBtnDisplay()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
    UpdateData();

	char customMsg[64];
	memset( customMsg, 0x00, sizeof(customMsg) );
	WideCharToMultiByte( CP_UTF8, 0, m_DisplayText.GetBuffer(), m_DisplayText.GetLength(), customMsg, sizeof(customMsg), NULL, NULL ); 
	
	BS_RET_CODE result;

	if( m_UseDisplayImage )
	{
		result = BS_DisplayCustomInfo( m_Handle, m_DisplayTime, customMsg, ".\\data\\Folder_48.png" );
	}
	else
	{
		result = BS_DisplayCustomInfo( m_Handle, m_DisplayTime, customMsg, NULL );
	}

	if( result != BS_SUCCESS )
	{
		TCHAR msg[128];
		wsprintf( msg, _T("Can not display custom info: %d"), result );
		MessageBox( msg );
	}
}

void CInterActiveTestDlg::OnBnClickedBtnCustomSound()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
    BS_RET_CODE result = BS_PlayCustomSound( m_Handle, ".\\data\\Question.wav" );
	if( result != BS_SUCCESS )
	{
		TCHAR msg[128];
		wsprintf( msg, _T("Can not display custom info: %d"), result );
		MessageBox( msg );
	}
}

void CInterActiveTestDlg::OnBnClickedBtnStopDisplay()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	BS_RET_CODE result = BS_CancelDisplayCustomInfo( m_Handle );
	if( result != BS_SUCCESS )
	{
		TCHAR msg[128];
		wsprintf( msg, _T("Can not display custom info: %d"), result );
		MessageBox( msg );
	}
}

void CInterActiveTestDlg::OnBnClickedBtnPlay()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	UpdateData();

	BS_RET_CODE result = BS_PlaySound( m_Handle, m_SoundIndex );
	if( result != BS_SUCCESS )
	{
		TCHAR msg[128];
		wsprintf( msg, _T("Can not display custom info: %d"), result );
		MessageBox( msg );
	}
}

void CInterActiveTestDlg::OnBnClickedBtnWaitKey()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
    UpdateData();

	char customMsg[64];
	memset( customMsg, 0x00, sizeof(customMsg) );
	WideCharToMultiByte( CP_UTF8, 0, m_KeyText.GetBuffer(), m_KeyText.GetLength(), customMsg, sizeof(customMsg), NULL, NULL ); 

	char key[64];
    int keyLen = 0;
    memset( key, 0x00, sizeof(key) );
  
	BS_RET_CODE result;
	if( m_UseKeyImage )
	{
		result = BS_WaitCustomKeyInput( m_Handle, m_WaitKeyTime, customMsg, ".\\data\\Folder_48.png", key, &keyLen );
	}
	else
	{
		result = BS_WaitCustomKeyInput( m_Handle, m_WaitKeyTime, customMsg, NULL, key, &keyLen );
	}

	if( result != BS_SUCCESS )
	{
		TCHAR msg[128];
		wsprintf( msg, _T("Can not display custom info: %d"), result );
		MessageBox( msg );
	}
	else
	{
		TCHAR msg[128];
		TCHAR tKey[64];
		memset( tKey, 0x00, sizeof(tKey) );
		MultiByteToWideChar( CP_ACP, 0, key, keyLen, tKey, keyLen );
		wsprintf( msg, _T("Received Key: %s    Length : %d "), tKey, keyLen );
		MessageBox( msg );
	}
}

void CInterActiveTestDlg::OnBnClickedBtnDisconnDevice()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	BS_CloseSocket( m_Handle );
}
