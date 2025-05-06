#pragma once


// m_ListFace 대화 상자입니다.

class m_ListFace : public CDialog
{
	DECLARE_DYNAMIC(m_ListFace)

public:
	m_ListFace(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~m_ListFace();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_USER_MANAGEMENT_FST };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

	DECLARE_MESSAGE_MAP()
};
