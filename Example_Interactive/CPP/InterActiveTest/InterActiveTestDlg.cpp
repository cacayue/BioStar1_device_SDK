// InterActiveTestDlg.cpp : ���� ����
//

#include "stdafx.h"
#include "InterActiveTest.h"
#include "InterActiveTestDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// ���� ���α׷� ������ ���Ǵ� CAboutDlg ��ȭ �����Դϴ�.

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// ��ȭ ���� �������Դϴ�.
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV �����Դϴ�.

// �����Դϴ�.
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


// CInterActiveTestDlg ��ȭ ����




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


// CInterActiveTestDlg �޽��� ó����

BOOL CInterActiveTestDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// �ý��� �޴��� "����..." �޴� �׸��� �߰��մϴ�.

	// IDM_ABOUTBOX�� �ý��� ��� ������ �־�� �մϴ�.
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

	// �� ��ȭ ������ �������� �����մϴ�. ���� ���α׷��� �� â�� ��ȭ ���ڰ� �ƴ� ��쿡��
	//  �����ӿ�ũ�� �� �۾��� �ڵ����� �����մϴ�.
	SetIcon(m_hIcon, TRUE);			// ū �������� �����մϴ�.
	SetIcon(m_hIcon, FALSE);		// ���� �������� �����մϴ�.

	// TODO: ���⿡ �߰� �ʱ�ȭ �۾��� �߰��մϴ�.
	BS_InitSDK();

	return TRUE;  // ��Ŀ���� ��Ʈ�ѿ� �������� ������ TRUE�� ��ȯ�մϴ�.
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

// ��ȭ ���ڿ� �ּ�ȭ ���߸� �߰��� ��� �������� �׸�����
//  �Ʒ� �ڵ尡 �ʿ��մϴ�. ����/�� ���� ����ϴ� MFC ���� ���α׷��� ��쿡��
//  �����ӿ�ũ���� �� �۾��� �ڵ����� �����մϴ�.

void CInterActiveTestDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // �׸��⸦ ���� ����̽� ���ؽ�Ʈ

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Ŭ���̾�Ʈ �簢������ �������� ����� ����ϴ�.
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// �������� �׸��ϴ�.
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// ����ڰ� �ּ�ȭ�� â�� ���� ���ȿ� Ŀ���� ǥ�õǵ��� �ý��ۿ���
//  �� �Լ��� ȣ���մϴ�.
HCURSOR CInterActiveTestDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}


void CInterActiveTestDlg::OnBnClickedOk()
{
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
	return;
	OnOK();
}

void CInterActiveTestDlg::OnBnClickedBtnConnDevice()
{
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
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
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
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
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
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
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
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
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
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
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
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
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
	BS_CloseSocket( m_Handle );
}
