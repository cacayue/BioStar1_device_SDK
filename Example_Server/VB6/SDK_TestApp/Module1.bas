Attribute VB_Name = "Module1"
    
Option Explicit

Dim result As Long


Public Function StartServerApp(ByVal nPort As Long, ByVal nMaxConnection As Long, ByVal bUseFunctionLock As Byte, ByVal bUseAutoResponse As Byte, ByVal bUseLock As Byte) As Long

    result = BS_SetConnectedCallback(AddressOf BS_ConnectionProc, bUseFunctionLock, bUseAutoResponse)
    result = BS_SetDisconnectedCallback(AddressOf BS_DisconnectedProc, bUseFunctionLock)
    result = BS_SetRequestStartedCallback(AddressOf BS_RequestStartProc, bUseFunctionLock, bUseAutoResponse)
    result = BS_SetLogCallback(AddressOf BS_LogProc, bUseFunctionLock, bUseAutoResponse)
    result = BS_SetImageLogCallback(AddressOf BS_ImageLogProc, bUseFunctionLock, bUseAutoResponse)
    result = BS_SetRequestMatchingCallback(AddressOf BS_RequestMatchingProc, bUseFunctionLock)
    result = BS_SetRequestUserInfoCallback(AddressOf BS_RequestUserInfoProc, bUseFunctionLock)
    
    result = BS_SDK.BS_SetSynchronousOperation(bUseLock)
    
    
    result = BS_StartServerApp(nPort, nMaxConnection, "C:\\OpenSSL\\bin\\openssl.exe", "12345678", KEEP_ALIVE_INTERVAL)

End Function


Public Function BS_ConnectionProc(ByVal handle As Long, ByVal deviceID As Long, ByVal deviceType As Long, ByVal connectionType As Long, ByVal functionType As Long, ByVal ipAddress As String) As Long

    result = Form1.ConnectionProc(handle, deviceID, deviceType, connectionType, functionType, ipAddress)
    
End Function

Public Function BS_DisconnectedProc(ByVal handle As Long, ByVal deviceID As Long, ByVal deviceType As Long, ByVal connectionType As Long, ByVal functionType As Long, ByVal ipAddress As String) As Long

    result = Form1.DisconnectedProc(handle, deviceID, deviceType, connectionType, functionType, ipAddress)
    
End Function

Public Function BS_RequestStartProc(ByVal handle As Long, ByVal deviceID As Long, ByVal deviceType As Long, ByVal connectionType As Long, ByVal functionType As Long, ByVal ipAddress As String) As Long

    result = Form1.RequestStartProc(handle, deviceID, deviceType, connectionType, functionType, ipAddress)
    
End Function

Public Function BS_LogProc(ByVal handle As Long, ByVal deviceID As Long, ByVal deviceType As Long, ByVal connectionType As Long, ByRef data As BSLogRecord) As Long
    
    Dim eventid As Long
    
    eventid = data.event
    result = Form1.LogProc(handle, deviceID, deviceType, connectionType, eventid)
    
    
End Function

Public Function BS_ImageLogProc(ByVal handle As Long, ByVal deviceID As Long, ByVal deviceType As Long, ByVal connectionType As Long, ByRef data As Long, ByVal dataLen As Long) As Long
    
    result = Form1.ImageLogProc(handle, deviceID, deviceType, connectionType, data, dataLen)
    
End Function

Public Function BS_RequestMatchingProc(ByVal handle As Long, ByVal deviceID As Long, ByVal deviceType As Long, ByVal connectionType As Long, ByVal matchingType As Long, ByVal ID As Long, ByVal templateData As String, ByRef userHdr As BSUserHdrEx, ByRef isDuress As Long) As Long

    'result = Form1.RequestMatchingProc(handle, deviceID, deviceType, connectionType, matchingType, ID, templateData, userHdr, tempDuress)
    

    
    If deviceType = BS_SDK.BS_DEVICE_BIOSTATION Then
    
        userHdr.ID = 10
        'userHdr.adminLevel = 241
        'userHdr.securityLevel = 260
        'userHdr.statusMask = 0
        'userHdr.accessGroupMask = &HFFFFFFFF
        'result = BS_ConvertToUTF8("Test User", userHdr.userName(0), BS_MAX_NAME_LEN)
        'result = BS_ConvertToUTF8("Test Group", userHdr.department(0), BS_MAX_NAME_LEN)
        'userHdr.numOfFinger = 2
        'userHdr.duressMask = &H0
        'userHdr.cardID = 123456
        'userHdr.bypassCard = flase
        'userHdr.authLimitCount = 0
        'userHdr.disabled = False
        'userHdr.expireDateTime = 0
        'userHdr.timedAntiPassback = 0
    
    End If

    
    'isDuress = BS_SDK.NORMAL_FINGER
    
    
End Function

Public Function BS_RequestUserInfoProc(ByVal handle As Long, ByVal deviceID As Long, ByVal deviceType As Long, ByVal connectionType As Long, ByVal idType As Long, ByVal ID As Long, ByVal customID As Long, ByRef userHdr As Long) As Long

    result = Form1.RequestUserInfoProc(handle, deviceID, deviceType, connectionType, idType, ID, customID, userHdr)
    
End Function


