#pragma once


#include "BS_API.h"


// RS485Management ��ȭ �����Դϴ�.

class RS485Management : public CDialog
{
	DECLARE_DYNAMIC(RS485Management)

public:
	RS485Management(CWnd* pParent = NULL);   // ǥ�� �������Դϴ�.
	virtual ~RS485Management();

// ��ȭ ���� �������Դϴ�.
	enum { IDD = IDD_RS485_MANAGEMENT };
	CListCtrl	m_SlaveList;
	CString	m_Device;
	int		m_NumOfSlave;

	void setDevice( int handle, unsigned readerID, unsigned readerAddr, int deviceType );
    CString getDeviceString(int nType);


protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV �����Դϴ�.

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
