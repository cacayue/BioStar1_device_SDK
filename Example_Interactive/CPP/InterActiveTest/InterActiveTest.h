// InterActiveTest.h : PROJECT_NAME ���� ���α׷��� ���� �� ��� �����Դϴ�.
//

#pragma once

#ifndef __AFXWIN_H__
	#error "PCH�� ���� �� ������ �����ϱ� ���� 'stdafx.h'�� �����մϴ�."
#endif

#include "resource.h"		// �� ��ȣ�Դϴ�.


// CInterActiveTestApp:
// �� Ŭ������ ������ ���ؼ��� InterActiveTest.cpp�� �����Ͻʽÿ�.
//

class CInterActiveTestApp : public CWinApp
{
public:
	CInterActiveTestApp();

// �������Դϴ�.
	public:
	virtual BOOL InitInstance();

// �����Դϴ�.

	DECLARE_MESSAGE_MAP()
};

extern CInterActiveTestApp theApp;