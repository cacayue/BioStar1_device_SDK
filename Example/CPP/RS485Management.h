#pragma once


#include "BS_API.h"


// RS485Management 대화 상자입니다.

class RS485Management : public CDialog
{
	DECLARE_DYNAMIC(RS485Management)

public:
	RS485Management(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~RS485Management();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_RS485_MANAGEMENT };
	CListCtrl	m_SlaveList;
	CString	m_Device;
	int		m_NumOfSlave;

	void setDevice( int handle, unsigned readerID, unsigned readerAddr, int deviceType );
    CString getDeviceString(int nType);


protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

	DECLARE_MESSAGE_MAP()


	int m_Handle;
	unsigned m_DeviceID;
	unsigned m_DeviceAddr;
	int m_DeviceType;

public:
    afx_msg void OnBnClickedSearch();
    afx_msg void OnBnClickedAddUser();
    afx_msg void OnBnClickedDeleteUser();
    virtual BOOL OnInitDialog();
};
