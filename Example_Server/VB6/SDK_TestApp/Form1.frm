VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   8940
   ClientLeft      =   60
   ClientTop       =   450
   ClientWidth     =   8730
   LinkTopic       =   "Form1"
   ScaleHeight     =   8940
   ScaleWidth      =   8730
   StartUpPosition =   3  'Windows ±âº»°ª
   Begin VB.CommandButton btnStartRequest 
      Caption         =   "Start Request"
      Height          =   975
      Left            =   7080
      TabIndex        =   15
      Top             =   7800
      Width           =   1455
   End
   Begin VB.CommandButton btnDeleteSSLCert 
      Caption         =   "Delete SSL Cert"
      Height          =   975
      Left            =   5160
      TabIndex        =   14
      Top             =   7800
      Width           =   1575
   End
   Begin VB.CommandButton btnIssuceSSLCert 
      Caption         =   "Issue SSL Cert"
      Height          =   975
      Left            =   3480
      TabIndex        =   13
      Top             =   7800
      Width           =   1575
   End
   Begin VB.CommandButton btnStopService 
      Caption         =   "Stop Service"
      Height          =   975
      Left            =   1800
      TabIndex        =   12
      Top             =   7800
      Width           =   1335
   End
   Begin VB.CommandButton btnStartService 
      Caption         =   "Start Service"
      Height          =   975
      Left            =   240
      TabIndex        =   11
      Top             =   7800
      Width           =   1455
   End
   Begin VB.Frame Frame2 
      Caption         =   "Option"
      Height          =   1455
      Left            =   240
      TabIndex        =   2
      Top             =   6240
      Width           =   8295
      Begin VB.CheckBox Check_UseLock 
         Caption         =   "Use Lock"
         Height          =   375
         Left            =   5520
         TabIndex        =   10
         Top             =   960
         Width           =   1575
      End
      Begin VB.CheckBox Check_UseFunctionLock 
         Caption         =   "Use Function Lock"
         Height          =   375
         Left            =   2880
         TabIndex        =   9
         Top             =   960
         Width           =   2535
      End
      Begin VB.CheckBox Check_UseAutoResponse 
         Caption         =   "Use Auto Response"
         Height          =   375
         Left            =   360
         TabIndex        =   8
         Top             =   960
         Width           =   2295
      End
      Begin VB.CheckBox Check_MatchFail 
         Caption         =   "Matching Fail"
         Height          =   375
         Left            =   6240
         TabIndex        =   7
         Top             =   360
         Width           =   1815
      End
      Begin VB.TextBox txtMaxConnection 
         Height          =   375
         Left            =   4440
         TabIndex        =   6
         Text            =   "Text2"
         Top             =   360
         Width           =   1095
      End
      Begin VB.TextBox txtPort 
         Height          =   375
         Left            =   1200
         TabIndex        =   4
         Text            =   "Text1"
         Top             =   360
         Width           =   1335
      End
      Begin VB.Label Label2 
         Caption         =   "Max Connection"
         Height          =   255
         Left            =   2880
         TabIndex        =   5
         Top             =   480
         Width           =   1695
      End
      Begin VB.Label Label1 
         Caption         =   "Port"
         Height          =   255
         Left            =   600
         TabIndex        =   3
         Top             =   480
         Width           =   615
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   "Device List"
      Height          =   6015
      Left            =   240
      TabIndex        =   0
      Top             =   120
      Width           =   8295
      Begin VB.ListBox List2 
         Height          =   2580
         Left            =   120
         TabIndex        =   16
         Top             =   3240
         Width           =   7815
      End
      Begin VB.ListBox List1 
         Height          =   2220
         Left            =   120
         TabIndex        =   1
         Top             =   600
         Width           =   7815
      End
      Begin VB.Label Label8 
         Caption         =   "Status"
         Height          =   255
         Left            =   5040
         TabIndex        =   22
         Top             =   360
         Width           =   1575
      End
      Begin VB.Label Label7 
         Caption         =   "Device Type"
         Height          =   255
         Left            =   3480
         TabIndex        =   21
         Top             =   360
         Width           =   1575
      End
      Begin VB.Label Label6 
         Caption         =   "IP Address"
         Height          =   255
         Left            =   2160
         TabIndex        =   20
         Top             =   360
         Width           =   1335
      End
      Begin VB.Label Label5 
         Caption         =   "Handle"
         Height          =   255
         Left            =   1080
         TabIndex        =   19
         Top             =   360
         Width           =   1335
      End
      Begin VB.Label Label4 
         Caption         =   "Device ID"
         Height          =   255
         Left            =   120
         TabIndex        =   18
         Top             =   360
         Width           =   975
      End
      Begin VB.Label Label3 
         Caption         =   "Log List"
         Height          =   255
         Left            =   120
         TabIndex        =   17
         Top             =   3000
         Width           =   3135
      End
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim result As Long
Dim m_Count As Long

Dim m_Port As Long
Dim m_MaxConnection As Long

Dim m_MatchFail As Byte
Dim m_UseFunctionLock As Byte
Dim m_UseAutoResponse As Byte
Dim m_UseLock As Byte



Private Sub Form_Load()
    result = BS_InitSDK()
    
    txtPort.Text = "5001"
    txtMaxConnection.Text = "32"
    
    Check_MatchFail.Value = 0
    Check_UseAutoResponse.Value = 1
    Check_UseLock.Value = 0
    
    m_Count = 0
    
End Sub

Private Sub Form_Terminate()

    BS_StopServerApp
    
End Sub

Private Sub Form_Unload(Cancel As Integer)

    BS_StopServerApp

End Sub

Private Sub btnStopService_Click()

    BS_StopServerApp
    
End Sub


Private Sub btnStartService_Click()

    m_Port = Int(txtPort.Text)
    m_MaxConnection = Int(txtMaxConnection.Text)
 
    m_MatchFail = Check_MatchFail.Value
    m_UseFunctionLock = Check_UseFunctionLock.Value
    m_UseAutoResponse = Check_UseAutoResponse.Value
    m_UseLock = Check_UseLock.Value
    
    result = StartServerApp(m_Port, m_MaxConnection, m_UseFunctionLock, m_UseAutoResponse, m_UseLock)
    
    If result = BS_SUCCESS Then
        Form1.Caption = "Server SDK Test Started ..."
    Else
        Form1.Caption = "BS_StartServerApp Fail!"
    End If
    
    m_Count = 0
    
End Sub


Public Function ConnectionProc(ByVal handle As Long, ByVal deviceID As Long, ByVal deviceType As Long, ByVal connectionType As Long, ByVal functionType As Long, ByVal ipAddress As String) As Long

    Dim strType As String
    
    Select Case deviceType
        Case BS_SDK.BS_DEVICE_BIOSTATION:       strType = "BioStation"
        Case BS_SDK.BS_DEVICE_BIOENTRY_PLUS:    strType = "BioEntry"
        Case BS_SDK.BS_DEVICE_BIOLITE:          strType = "BioLiteNet"
        Case BS_SDK.BS_DEVICE_XPASS:            strType = "XPass"
        Case BS_SDK.BS_DEVICE_DSTATION:         strType = "D-Station"
        Case BS_SDK.BS_DEVICE_XSTATION:         strType = "X-Station"
        Case BS_SDK.BS_DEVICE_BIOSTATION2:      strType = "BioStation T2"
    End Select
    
    
    Dim strConnect As String
    
    If connectionType = 1 Then
        strConnect = "SSL"
    Else
        strConnect = "Normal"
    End If
    
    
    Form1.List1.AddItem Str$(deviceID) & vbTab & Str$(handle) & vbTab & ipAddress & vbTab & strType & vbTab & "connected..."
    
    
    result = BS_StartRequest(handle, deviceType, m_Port)
    
    
End Function



Public Function DisconnectedProc(ByVal handle As Long, ByVal deviceID As Long, ByVal deviceType As Long, ByVal connectionType As Long, ByVal functionType As Long, ByVal ipAddress As String) As Long

End Function


Public Function RequestStartProc(ByVal handle As Long, ByVal deviceID As Long, ByVal deviceType As Long, ByVal connectionType As Long, ByVal functionType As Long, ByVal ipAddress As String) As Long

End Function

Public Function LogProc(ByVal handle As Long, ByVal deviceID As Long, ByVal deviceType As Long, ByVal connectionType As Long, ByVal data As Long) As Long

    Dim strType As String
    Dim strLog As String
    
    Select Case deviceType
        Case BS_SDK.BS_DEVICE_BIOSTATION:       strType = "BioStation"
        Case BS_SDK.BS_DEVICE_BIOENTRY_PLUS:    strType = "BioEntry"
        Case BS_SDK.BS_DEVICE_BIOLITE:          strType = "BioLiteNet"
        Case BS_SDK.BS_DEVICE_XPASS:            strType = "XPass"
        Case BS_SDK.BS_DEVICE_DSTATION:         strType = "D-Station"
        Case BS_SDK.BS_DEVICE_XSTATION:         strType = "X-Station"
        Case BS_SDK.BS_DEVICE_BIOSTATION2:      strType = "BioStation T2"
    End Select

    
    If deviceType = BS_SDK.BS_DEVICE_BIOSTATION Then
        Dim logRecord As BSLogRecord
        
        'CopyMemory((logRecord, data, BSLogRecord.cbSize - 1)
        
    End If
    

    strLog = "Log Event : 0x" & Hex(data)

    Form1.List2.AddItem Str$(deviceID) & vbTab & Str$(handle) & vbTab & ipAddress & vbTab & strType & vbTab & strLog
    
End Function

Public Function ImageLogProc(ByVal handle As Long, ByVal deviceID As Long, ByVal deviceType As Long, ByVal connectionType As Long, ByRef data As Long, ByVal dataLen As Long) As Long

End Function

Public Function RequestMatchingProc(ByVal handle As Long, ByVal deviceID As Long, ByVal deviceType As Long, ByVal connectionType As Long, ByVal matchingType As Long, ByVal ID As Long, ByVal templateData As String, ByRef userHdr As Long, ByRef isDuress As Long) As Long

    If deviceType = BS_SDK.BS_DEVICE_BIOENTRY_PLUS Then
    
        Dim beUserHdr As beUserHdr
        
        CopyMemory1 beUserHdr, userHdr, beUserHdr.cbSize
    
        beUserHdr.version = 0
        beUserHdr.userID = userID
        beUserHdr.startTime = 0
        beUserHdr.expiryTime = 0
        beUserHdr.cardID = 0
    
        beUserHdr.cardCustomID = 0
        beUserHdr.commandCardFlag = 0
        beUserHdr.cardFlag = 0
        beUserHdr.cardVersion = BS_SDK.BE_CARD_VERSION_1
    
        beUserHdr.adminLevel = BS_SDK.BE_USER_LEVEL_NORMAL
        beUserHdr.securityLevel = BS_SDK.BE_USER_SECURITY_DEFAULT
    
        beUserHdr.accessGroupMask = &HFFFFFFFF
    
        beUserHdr.numOfFinger = 1
    
    ElseIf deviceType = BS_SDK.BS_DEVICE_BIOLITE Then
    
        Dim beUserHdr As beUserHdr
        
        CopyMemory1 beUserHdr, userHdr, beUserHdr.cbSize
    
        beUserHdr.version = 0
        beUserHdr.userID = userID
        beUserHdr.startTime = 0
        beUserHdr.expiryTime = 0
        beUserHdr.cardID = 0
    
        beUserHdr.cardCustomID = 0
        beUserHdr.commandCardFlag = 0
        beUserHdr.cardFlag = 0
        beUserHdr.cardVersion = BS_SDK.BE_CARD_VERSION_1

        beUserHdr.adminLevel = BS_SDK.BE_USER_LEVEL_NORMAL
        beUserHdr.securityLevel = BSSDK.BE_USER_SECURITY_DEFAULT
    
        beUserHdr.accessGroupMask = &HFFFFFFFF
    
        beUserHdr.numOfFinger = 1
    
    ElseIf deviceType = BS_SDK.BS_DEVICE_BIOSTATION Then
    
        Dim bsUserHdr As BSUserHdrEx
        
        CopyMemory1 bsUserHdr, userHdr, bsUserHdr.cbSize
    
        bsUserHdr.ID = 10
        bsUserHdr.adminLevel = 241
        bsUserHdr.securityLevel = 260
        bsUserHdr.statusMask = 0
        bsUserHdr.accessGroupMask = &HFFFFFFFF
        result = BS_ConvertToUTF8("Test User", bsUserHdr.userName(0), BS_MAX_NAME_LEN)
        result = BS_ConvertToUTF8("Test Group", bsUserHdr.department(0), BS_MAX_NAME_LEN)
        bsUserHdr.numOfFinger = 2
        bsUserHdr.duressMask = &H0
        bsUserHdr.cardID = 123456
        bsUserHdr.bypassCard = flase
        bsUserHdr.authLimitCount = 0
        bsUserHdr.disabled = False
        bsUserHdr.expireDateTime = 0
        bsUserHdr.timedAntiPassback = 0
        
    ElseIf deviceType = BS_SDK.BS_DEVICE_DSTATION Then

    ElseIf deviceType = BS_SDK.BS_DEVICE_XSTATION Then

    ElseIf deviceType = BS_SDK.BS_DEVICE_BIOSTATION2 Then
    
    End If
    
    isDuress = BS_SDK.NORMAL_FINGER



End Function

Public Function RequestUserInfoProc(ByVal handle As Long, ByVal deviceID As Long, ByVal deviceType As Long, ByVal connectionType As Long, ByVal idType As Long, ByVal ID As Long, ByVal customID As Long, ByVal userHdr As Long) As Long
    If deviceType = BS_SDK.BS_DEVICE_BIOENTRY_PLUS Then
    
        Dim beUserHdr As beUserHdr
    
        beUserHdr.version = 0
        beUserHdr.userID = userID
        beUserHdr.startTime = 0
        beUserHdr.expiryTime = 0
        beUserHdr.cardID = 0
    
        beUserHdr.cardCustomID = 0
        beUserHdr.commandCardFlag = 0
        beUserHdr.cardFlag = 0
        beUserHdr.cardVersion = BS_SDK.BE_CARD_VERSION_1
    
        beUserHdr.adminLevel = BS_SDK.BE_USER_LEVEL_NORMAL
        beUserHdr.securityLevel = BS_SDK.BE_USER_SECURITY_DEFAULT
    
        beUserHdr.accessGroupMask = &HFFFFFFFF
    
        beUserHdr.numOfFinger = 1
    
    ElseIf deviceType = BS_SDK.BS_DEVICE_BIOLITE Then
    
        Dim beUserHdr As beUserHdr
    
        beUserHdr.version = 0
        beUserHdr.userID = userID
        beUserHdr.startTime = 0
        beUserHdr.expiryTime = 0
        beUserHdr.cardID = 0
    
        beUserHdr.cardCustomID = 0
        beUserHdr.commandCardFlag = 0
        beUserHdr.cardFlag = 0
        beUserHdr.cardVersion = BS_SDK.BE_CARD_VERSION_1

        beUserHdr.adminLevel = BS_SDK.BE_USER_LEVEL_NORMAL
        beUserHdr.securityLevel = BSSDK.BE_USER_SECURITY_DEFAULT
    
        beUserHdr.accessGroupMask = &HFFFFFFFF
    
        beUserHdr.numOfFinger = 1
    
    ElseIf deviceType = BS_SDK.BS_DEVICE_BIOSTATION Then
    
        Dim bsUserHdr As BSUserHdrEx
    
        bsUserHdr.ID = 10
        bsUserHdr.adminLevel = 241
        bsUserHdr.securityLevel = 260
        bsUserHdr.statusMask = 0
        bsUserHdr.accessGroupMask = &HFFFFFFFF
        result = BS_ConvertToUTF8("Test User", bsUserHdr.userName(0), BS_MAX_NAME_LEN)
        result = BS_ConvertToUTF8("Test Group", bsUserHdr.department(0), BS_MAX_NAME_LEN)
        bsUserHdr.numOfFinger = 2
        bsUserHdr.duressMask = &H0
        bsUserHdr.cardID = 123456
        bsUserHdr.bypassCard = flase
        bsUserHdr.authLimitCount = 0
        bsUserHdr.disabled = False
        bsUserHdr.expireDateTime = 0
        bsUserHdr.timedAntiPassback = 0
        
    ElseIf deviceType = BS_SDK.BS_DEVICE_DSTATION Then

    ElseIf deviceType = BS_SDK.BS_DEVICE_XSTATION Then

    ElseIf deviceType = BS_SDK.BS_DEVICE_BIOSTATION2 Then
    
    End If
    
End Function


