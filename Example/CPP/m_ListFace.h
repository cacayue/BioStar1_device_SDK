#pragma once


// m_ListFace ��ȭ �����Դϴ�.

class m_ListFace : public CDialog
{
	DECLARE_DYNAMIC(m_ListFace)

public:
	m_ListFace(CWnd* pParent = NULL);   // ǥ�� �������Դϴ�.
	virtual ~m_ListFace();

// ��ȭ ���� �������Դϴ�.
	enum { IDD = IDD_USER_MANAGEMENT_FST };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV �����Դϴ�.

	DECLARE_MESSAGE_MAP()
};
