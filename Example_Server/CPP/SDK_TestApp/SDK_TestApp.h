// SDK_TestApp.h : PROJECT_NAME ���� ���α׷��� ���� �� ��� �����Դϴ�.
//

#pragma once

#ifndef __AFXWIN_H__
	#error "PCH�� ���� �� ������ �����ϱ� ���� 'stdafx.h'�� �����մϴ�."
#endif

#include "resource.h"		// �� ��ȣ�Դϴ�.


// CSDK_TestAppApp:
// �� Ŭ������ ������ ���ؼ��� SDK_TestApp.cpp�� �����Ͻʽÿ�.
//

class CSDK_TestAppApp : public CWinApp
{
public:
	CSDK_TestAppApp();

// �������Դϴ�.
	public:
	virtual BOOL InitInstance();

// �����Դϴ�.

	DECLARE_MESSAGE_MAP()
};

extern CSDK_TestAppApp theApp;