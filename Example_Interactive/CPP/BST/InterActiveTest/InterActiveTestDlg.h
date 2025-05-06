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
	CString m_IpAddress;
	int m_Port;
	int m_Handle;
	BOOL m_UseDisplayImage;
	BOOL m_UseKeyImage;
	CString m_DisplayText;
	CString m_KeyText;
	CString m_DisplayTime;
	CString m_WaitKeyTime;
	CString m_SoundIndex;
public:
	// ������ �޽��� �� �Լ�
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnBnClickedOk();
	afx_msg void OnBnClickedBtnConnDevice();
	afx_msg void OnBnClickedBtnDisplay();
	afx_msg void OnBnClickedBtnCustomSound();
	afx_msg void OnBnClickedBtnStopDisplay();
	afx_msg void OnBnClickedBtnPlay();
	afx_msg void OnBnClickedBtnWaitKey();
	afx_msg void OnBnClickedBtnDisconnDevice();
    afx_msg void OnBnClickedBtnSendnotice();
	afx_msg void OnDestroy();
	DECLARE_MESSAGE_MAP()
};
