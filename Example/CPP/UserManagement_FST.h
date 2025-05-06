#pragma once

#include "BS_API.h"
#include "afxwin.h"

#define FACETEMPLATE_FST_SIZE	2000

// CUserManagement_FST dialog

class CUserManagement_FST : public CDialog
{
	DECLARE_DYNAMIC(CUserManagement_FST)

public:
	CUserManagement_FST(CWnd* pParent = NULL);   // standard constructor
	virtual ~CUserManagement_FST();

// Dialog Data
	enum { IDD = IDD_USER_MANAGEMENT_FST };

    CRect    m_ImageRect;
    CListBox m_faceList;
	CListCtrl	m_UserList;
	int		m_AdminLevel;
	int		m_AuthMode;
	UINT	m_CardID;
	int		m_CardType;
	UINT	m_Checksum1;
	UINT	m_Checksum2;
	UINT	m_FaceChecksum;
	UINT	m_CustomID;
	CString	m_Device;
    BOOL    m_Face_FaceStation;
	UINT	m_UserID;
    CString m_Password;
	CString	m_Name;
    int     m_NumOfTemplate;
	int		m_NumOfFaceTemplate;
	int		m_NumOfUser;
	int		m_SecurityLevel;
    int     m_nFaceNum;
	COleDateTime	m_StartDate;
	COleDateTime	m_ExpiryDate;
	CString	m_AccessGroup;
	BOOL	m_ExtRF;
    FSUserHdrEx m_userHdr;

	int      m_Handle;
	unsigned m_DeviceID;
	unsigned m_DeviceAddr;
	int      m_DeviceType;

    FSUserTemplateHdr m_userTemplateHdr;
    unsigned char m_Image[BS_MAX_IMAGE_SIZE];
    unsigned char m_StillcutData[BS_MAX_FACE_TYPE][BS_MAX_IMAGE_SIZE];
    unsigned char m_FaceTemplate[BS_MAX_FACE_TYPE][FACETEMPLATE_FST_SIZE * BS_MAX_FACE_TEMPLATE];


	bool getUserInfo();
	bool getUserInfo( unsigned userID );
	
	void setDevice( int handle, unsigned deviceID, unsigned deviceAddr, int deviceType );
    void FillAuthmodeData();
    void DrawFaceImage(CDC* pDC); 
    void SaveFaceImage(unsigned char *pImage, int nImageLen);
    BOOL GetEncCLSID(WCHAR *mime, CLSID *pClsid);
    BOOL DrawBitmap(HDC hDC, UINT nIDResource, CRect rect, int nOption);

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support.
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg void OnClickUserList(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnDelete();
	afx_msg void OnDeleteAll();
	afx_msg void OnRefresh();
	afx_msg void OnUpdate();
	afx_msg void OnAdd();
	afx_msg void OnReadCard();
	afx_msg void OnDestroy();
    afx_msg void OnBnClickedFstScanFace();
    afx_msg void OnLbnSelchangeListFace();
    afx_msg void OnBnClickedBtnAdd();
    afx_msg void OnBnClickedBtnDelete();
	DECLARE_MESSAGE_MAP()
};
