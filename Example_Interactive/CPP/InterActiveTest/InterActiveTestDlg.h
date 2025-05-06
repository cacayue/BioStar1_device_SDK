// InterActiveTestDlg.h : ��� ����
//

#pragma once


// CInterActiveTestDlg ��ȭ ����
class CInterActiveTestDlg : public CDialog
{
// �����Դϴ�.
public:
	CInterActiveTestDlg(CWnd* pParent = NULL);	// ǥ�� �������Դϴ�.

// ��ȭ ���� �������Դϴ�.
	enum { IDD = IDD_INTERACTIVETEST_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV �����Դϴ�.


// �����Դϴ�.
protected:
	HICON m_hIcon;

	// ������ �޽��� �� �Լ�
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();
	CString m_IpAddress;
	int m_Port;
	int m_SoundIndex;
	afx_msg void OnBnClickedBtnConnDevice();


	int m_Handle;
	afx_msg void OnBnClickedBtnDisplay();
	afx_msg void OnBnClickedBtnCustomSound();
	afx_msg void OnBnClickedBtnStopDisplay();
	afx_msg void OnBnClickedBtnPlay();
	afx_msg void OnBnClickedBtnWaitKey();
	afx_msg void OnBnClickedBtnDisconnDevice();
	BOOL m_UseDisplayImage;
	BOOL m_UseKeyImage;
	CString m_DisplayText;
	CString m_KeyText;
	int m_DisplayTime;
	int m_WaitKeyTime;
    afx_msg void OnBnClickedBtnSendnotice();
	afx_msg void OnDestroy();
};
