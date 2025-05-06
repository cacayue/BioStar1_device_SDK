#if !defined(AFX_USERMANAGEMENT_H__01BCF239_D286_479C_A027_0E02103C49B4__INCLUDED_)
#define AFX_USERMANAGEMENT_H__01BCF239_D286_479C_A027_0E02103C49B4__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// UserManagement.h : header file
//

#include "BS_API.h"

/////////////////////////////////////////////////////////////////////////////
// CUserManagement dialog

class CUserManagement : public CDialog
{
// Construction
public:
	CUserManagement(CWnd* pParent = NULL);   // standard constructor
	virtual ~CUserManagement();

// Dialog Data
	//{{AFX_DATA(CUserManagement)
	enum { IDD = IDD_USER_MANAGEMENT };
	CListCtrl	m_UserList;
	int		m_AdminLevel;
	int		m_AuthMode;
	UINT	m_CardID;
	int		m_CardType;
	UINT	m_Checksum1;
	UINT	m_Checksum2;
	UINT	m_FaceChecksum;
    UINT    m_FST_FaceNum;
	UINT	m_CustomID;
	CString	m_Device;
	BOOL	m_Duress1;
	BOOL	m_Duress2;
	BOOL	m_Finger1;
	BOOL	m_Finger2;
	BOOL	m_Face_DStation;
    BOOL    m_Face_FaceStation;
	UINT	m_UserID;
	CString	m_Name;
    CString m_Password;
	int		m_NumOfFaceTemplate;
	int		m_NumOfTemplate;
	int		m_NumOfUser;
	int		m_SecurityLevel;
	COleDateTime	m_StartDate;
	COleDateTime	m_ExpiryDate;
    CString	m_AccessGroup;
	BOOL	m_ExtRF;
	//}}AFX_DATA

	bool getUserInfo();
	bool getUserInfo( unsigned userID );
	
	void setDevice( int handle, unsigned deviceID, unsigned deviceAddr, int deviceType );
    void SetControlState();
    void FillAuthmodeData();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CUserManagement)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CUserManagement)
	virtual BOOL OnInitDialog();
	afx_msg void OnClickUserList(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnDelete();
	afx_msg void OnDeleteAll();
	afx_msg void OnRefresh();
	afx_msg void OnUpdate();
	afx_msg void OnAdd();
	afx_msg void OnReadCard();
	afx_msg void OnDestroy();
    afx_msg void OnBnClickedFstScanFace();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	int m_Handle;
	unsigned m_DeviceID;
	unsigned m_DeviceAddr;
	int m_DeviceType;

	unsigned char* m_TemplateData;
	unsigned char* m_FaceTemplate_DST;
    unsigned char* m_FaceTemplate_FST;

    FSUserTemplateHdr m_userTemplateHdr;
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_USERMANAGEMENT_H__01BCF239_D286_479C_A027_0E02103C49B4__INCLUDED_)
