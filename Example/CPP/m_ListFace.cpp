// m_ListFace.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "BioStarCPP.h"
#include "m_ListFace.h"


// m_ListFace 대화 상자입니다.

IMPLEMENT_DYNAMIC(m_ListFace, CDialog)

m_ListFace::m_ListFace(CWnd* pParent /*=NULL*/)
	: CDialog(m_ListFace::IDD, pParent)
{

}

m_ListFace::~m_ListFace()
{
}

void m_ListFace::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(m_ListFace, CDialog)
END_MESSAGE_MAP()


// m_ListFace 메시지 처리기입니다.
