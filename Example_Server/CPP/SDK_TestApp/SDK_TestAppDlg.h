// SDK_TestAppDlg.h : interface of the CSDK_TestAppDlg class
//

#pragma once
#include "afxcmn.h"


#define UM_UPDATE   (WM_USER + 1)

#define LIST_ID_POS			0
#define LIST_HANDLE_POS		1
#define LIST_IP_POS			2
#define LIST_TYPE_POS		3
#define LIST_CONNECTION_POS	4
#define LIST_STATUS_POS		5


BS_RET_CODE __stdcall ConnectedProc( int handle, unsigned deviceID, int deviceType, int connectionType, int functionType, char* ipAddress );
BS_RET_CODE __stdcall DisconnectedProc( int handle, unsigned deviceID, int deviceType, int connectionType, int functionType, char* ipAddress );
BS_RET_CODE __stdcall RequestStartProc( int handle, unsigned deviceID, int deviceType, int connectionType, int functionType, char* ipAddress );
BS_RET_CODE __stdcall LogProc( int handle, unsigned deviceID, int deviceType, int connectionType, void* data );
BS_RET_CODE __stdcall ImageLogProc( int handle, unsigned deviceID, int deviceType, int connectionType, void* data, int nSize );
BS_RET_CODE __stdcall RequestMatchingProc( int handle, unsigned deviceID, int deviceType, int connectionType, 
                                          int matchingType, unsigned ID, unsigned char* templateData, void* userHdr, int* isDuress );
BS_RET_CODE __stdcall RequestUserInfoProc( int handle, unsigned deviceID, int deviceType, int connectionType, 
                                           int idType, unsigned ID, unsigned customID, void* userHdr );

// CSDK_TestAppDlg 
class CSDK_TestAppDlg : public CDialog
{
public:
	CSDK_TestAppDlg(CWnd* pParent = NULL);	

	static DWORD __stdcall ThreadProc(void *);

// Dialog data
	enum { IDD = IDD_SDK_TESTAPP_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	

    static void waiting(int nTimeout);

// Implementation
protected:
	HICON m_hIcon;

    // Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnBnClickedBtnStart();
	afx_msg void OnBnClickedBtnStop();
	afx_msg void OnBnClickedBtnRequest();
	afx_msg void OnBnClickedBtnIssue();
	afx_msg void OnBnClickedBtnDelete();
    afx_msg LRESULT OnUmUpdate(WPARAM wParam, LPARAM lParam);
	DECLARE_MESSAGE_MAP()

public:
	CListCtrl m_DeviceList;
	int m_Port;
	int m_Connections;
	int m_Count;
	BOOL m_UseAutoResponse;
	BOOL m_UseFunctionLock;
	BOOL m_UseLock;
	BOOL m_MatchingFail;
	afx_msg void OnDestroy();
};
