VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   9705
   ClientLeft      =   60
   ClientTop       =   450
   ClientWidth     =   5790
   LinkTopic       =   "Form1"
   ScaleHeight     =   9705
   ScaleWidth      =   5790
   StartUpPosition =   3  'Windows 기본값
   Begin VB.CommandButton Command9 
      Caption         =   "SetSlideShow"
      Height          =   495
      Left            =   0
      TabIndex        =   19
      Top             =   3720
      Width           =   2895
   End
   Begin VB.CommandButton Command6 
      Caption         =   "SendNotice Long"
      Height          =   495
      Left            =   2880
      TabIndex        =   18
      Top             =   3240
      Width           =   2895
   End
   Begin VB.CommandButton Command5 
      Caption         =   "SendNotice"
      Height          =   495
      Left            =   0
      TabIndex        =   17
      Top             =   3240
      Width           =   2895
   End
   Begin VB.CommandButton Command8 
      Caption         =   "Set OPMode Info"
      Height          =   495
      Left            =   2880
      TabIndex        =   16
      Top             =   2760
      Width           =   2895
   End
   Begin VB.CommandButton Command7 
      Caption         =   "Get OPMode Info"
      Height          =   495
      Left            =   0
      TabIndex        =   15
      Top             =   2760
      Width           =   2895
   End
   Begin VB.CommandButton Command21 
      Caption         =   "Enroll User"
      Height          =   495
      Left            =   0
      TabIndex        =   14
      Top             =   1800
      Width           =   2895
   End
   Begin VB.CommandButton Command20 
      Caption         =   "Get User"
      Height          =   495
      Left            =   2880
      TabIndex        =   13
      Top             =   1800
      Width           =   2895
   End
   Begin VB.CommandButton Command19 
      Caption         =   "Get User Info"
      Height          =   495
      Left            =   0
      TabIndex        =   12
      Top             =   2280
      Width           =   2895
   End
   Begin VB.CommandButton Command18 
      Caption         =   "Get All User Info"
      Height          =   495
      Left            =   2880
      TabIndex        =   11
      Top             =   2280
      Width           =   2895
   End
   Begin VB.TextBox txtDrive 
      Height          =   270
      Left            =   2280
      TabIndex        =   10
      Text            =   "G:"
      Top             =   480
      Width           =   1095
   End
   Begin VB.CheckBox chkUSB 
      Caption         =   "VT(USB Memory) at"
      Height          =   255
      Left            =   120
      TabIndex        =   9
      Top             =   480
      Width           =   2055
   End
   Begin VB.CommandButton Command4 
      Caption         =   "Read IP Config"
      Height          =   495
      Left            =   -120
      TabIndex        =   8
      Top             =   1320
      Width           =   3015
   End
   Begin VB.CommandButton Command3 
      Caption         =   "Write IP Config "
      Height          =   495
      Left            =   2880
      TabIndex        =   7
      Top             =   1320
      Width           =   2895
   End
   Begin VB.TextBox txtPort 
      Height          =   270
      Left            =   3840
      TabIndex        =   6
      Text            =   "1470"
      Top             =   120
      Width           =   1335
   End
   Begin VB.TextBox txtIPAddress 
      Height          =   270
      Left            =   1200
      TabIndex        =   4
      Text            =   "192.168.1.126"
      Top             =   120
      Width           =   1935
   End
   Begin VB.CommandButton Command2 
      Caption         =   "Read Log(Last an hour) "
      Height          =   495
      Left            =   2880
      TabIndex        =   2
      Top             =   840
      Width           =   2895
   End
   Begin VB.TextBox Text1 
      BeginProperty Font 
         Name            =   "굴림체"
         Size            =   9.75
         Charset         =   129
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   5295
      Left            =   0
      MultiLine       =   -1  'True
      ScrollBars      =   3  '양방향
      TabIndex        =   1
      Top             =   4320
      Width           =   5775
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Read All Log"
      Height          =   495
      Left            =   0
      TabIndex        =   0
      Top             =   840
      Width           =   2895
   End
   Begin VB.Label Label2 
      Caption         =   "Port"
      Height          =   255
      Left            =   3360
      TabIndex        =   5
      Top             =   120
      Width           =   615
   End
   Begin VB.Label Label1 
      Caption         =   "IP Address"
      Height          =   255
      Left            =   120
      TabIndex        =   3
      Top             =   120
      Width           =   975
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim result As Long
Private Sub Command1_Click()
    Dim logRecord() As BSLogRecordEx
    
    If chkUSB.Value = 0 Then
        result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    Else
        result = BS_OpenUSBMemory(txtDrive.Text, handle)
    End If
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetDeviceID(handle, biostationID, deviceType)
    
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    result = BS_SetDeviceID(handle, biostationID, 1)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If

    Text1.SelText = "[Read All Log]" & vbCrLf
    
    
    
    'Get Total log count
    result = BS_GetLogCount(handle, numOfLog)
    If result <> 0 Then
        Text1.SelText = "Cannot read log count : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    Text1.SelText = "Total Log Count " & numOfLog & " logs" & vbCrLf

    
    ReDim logRecord(numOfLog)
    Dim m_numOfLog As Long
    
    
    m_numOfLog = 0
    
    numOfLog = 0

    
    'if the number of records is over 21845, next codes is neeaded.
    Do
        If m_numOfLog = 0 Then
            result = BS_ReadLogEx(handle, 0, 0, numOfLog, logRecord(0))
        Else
            result = BS_ReadNextLogEx(handle, 0, 0, numOfLog, logRecord(m_numOfLog))
        End If
        
        
        If result <> 0 Then
            Text1.SelText = "Cannot read log record : " & result & vbCrLf
            If chkUSB.Value = 0 Then
                BS_CloseSocket (handle)
            Else
                BS_CloseUSBMemory (handle)
            End If
            Exit Sub
        End If
        
        m_numOfLog = m_numOfLog + numOfLog
        
        Text1.SelText = "Retrieved " & numOfLog & " logs" & vbCrLf
        
        
        If numOfLog < 21845 Then Exit Do
    Loop
    
    Dim i, startValue, diff As Long
    
    If m_numOfLog <= 50 Then
        startValue = 0
    Else
        startValue = m_numOfLog - 50
    End If
    
    Dim currentTime As Long
    currentTime = DateDiff("s", "1/1/1970", Now)
    
    For i = startValue To m_numOfLog - 1
        diff = currentTime - logRecord(i).eventTime
           
        Text1.SelText = "Event:" & logRecord(i).event & "  User:" & logRecord(i).userID & "  Time:" & format(DateAdd("s", -(diff), Now), "yyyy-mm-dd hh:mm:ss") _
                        & "  TNA:" & logRecord(i).tnaKey & vbCrLf
    Next
    
    Text1.SelText = "The last 50 of all logs have been displayed..." & vbCrLf
    If chkUSB.Value = 0 Then
        BS_CloseSocket (handle)
    Else
        BS_CloseUSBMemory (handle)
    End If
    
    Text1.SelText = vbCrLf
End Sub

Private Sub Command10_Click()
    If chkUSB.Value = 0 Then
        result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    Else
        result = BS_OpenUSBMemory(txtDrive.Text, handle)
    End If

    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetBiostationID(handle, biostationID)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    result = BS_SetBiostationID(handle, biostationID)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If

    Text1.SelText = "[Change wiegand configuration]" & vbCrLf
    Dim wiegandConfig As BSWiegandConfig
    
    'Wegand custom format total-32bits id-15bits
    'PAAAAAAAAAAAAAAAIIIIIIIIIIIIIIIP
    'EXXXXXXXXXXXXXXX
    '                XXXXXXXXXXXXXXXO

    wiegandConfig.header.format = BS_WIEGAND_CUSTOM
    wiegandConfig.header.totalBits = 32

    wiegandConfig.data.numOfParity = 2
    wiegandConfig.data.numOfField = 2
    wiegandConfig.data.idFieldMask = &H2 'b10

    wiegandConfig.data.parity(0).bitIndex = 0
    wiegandConfig.data.parity(0).type = BS_WIEGAND_EVEN_PARITY
    wiegandConfig.data.parity(0).bitMask(0) = &HFE 'b11111110, bit array 01111111
    wiegandConfig.data.parity(0).bitMask(1) = &HFF 'b11111111, bit array 11111111
    wiegandConfig.data.parity(0).bitMask(2) = &H0
    wiegandConfig.data.parity(0).bitMask(3) = &H0
    wiegandConfig.data.parity(0).bitMask(4) = &H0
    wiegandConfig.data.parity(0).bitMask(5) = &H0
    wiegandConfig.data.parity(0).bitMask(6) = &H0
    wiegandConfig.data.parity(0).bitMask(7) = &H0


    wiegandConfig.data.parity(1).bitIndex = 31
    wiegandConfig.data.parity(1).type = BS_WIEGAND_ODD_PARITY
    wiegandConfig.data.parity(1).bitMask(0) = &H0
    wiegandConfig.data.parity(1).bitMask(1) = &H0
    wiegandConfig.data.parity(1).bitMask(2) = &HFF 'b11111111, bit array 11111111
    wiegandConfig.data.parity(1).bitMask(3) = &H7F 'b01111111, bit array 11111110
    wiegandConfig.data.parity(1).bitMask(4) = &H0
    wiegandConfig.data.parity(1).bitMask(5) = &H0
    wiegandConfig.data.parity(1).bitMask(6) = &H0
    wiegandConfig.data.parity(1).bitMask(7) = &H0

    wiegandConfig.data.field(0).bitIndex = 1
    wiegandConfig.data.field(0).bitLength = 15
    wiegandConfig.fieldValue(0) = 0

    wiegandConfig.data.field(1).bitIndex = 16
    wiegandConfig.data.field(1).bitLength = 15


    'Wiegand Config
    result = BS_WriteWiegandConfig(handle, wiegandConfig)
    If result <> 0 Then
        Text1.SelText = "Cannot write wiegand config : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If

    Text1.SelText = "Wiegand custom format total-32bits id-15bits" & vbCrLf
    Text1.SelText = "PAAAAAAAAAAAAAAAIIIIIIIIIIIIIIIP" & vbCrLf
    Text1.SelText = "EXXXXXXXXXXXXXXX" & vbCrLf
    Text1.SelText = "                XXXXXXXXXXXXXXXO" & vbCrLf


    If chkUSB.Value = 0 Then
        BS_CloseSocket (handle)
    Else
        BS_CloseUSBMemory (handle)
    End If
    
    Text1.SelText = vbCrLf

End Sub

Private Sub Command11_Click()
    If chkUSB.Value = 0 Then
        result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    Else
        result = BS_OpenUSBMemory(txtDrive.Text, handle)
    End If
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetBiostationID(handle, biostationID)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    result = BS_SetBiostationID(handle, biostationID)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    Text1.SelText = "[Release Encryption]" & vbCrLf
    
    Dim encConfig As BSEncryptionConfig
    result = BS_ReadEncryptionConfig(handle, encConfig)
    If result <> 0 Then
        Text1.SelText = "Cannot read encryption config : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    If encConfig.useEncryption = False Then
        Text1.SelText = "Encryption is not used already." & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    'Before you change the encryption configuration, all users in the i9 should be deleted.
    result = BS_DeleteAllUser(handle)
    If result <> 0 Then
        Text1.SelText = "Cannot delete all user : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    Text1.SelText = "All users are deleted." & vbCrLf
    
    'Write encryption configuration to the deivce
    For i = 0 To 31
        encConfig.password(i) = 0
    Next i

    encConfig.useEncryption = False
    
    result = BS_WriteEncryptionConfig(handle, encConfig)
    If result <> 0 Then
        Text1.SelText = "Cannot write encryption config : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    Text1.SelText = "Encryption is off." & vbCrLf
        
    If chkUSB.Value = 0 Then
        BS_CloseSocket (handle)
    Else
        BS_CloseUSBMemory (handle)
    End If
    
    Text1.SelText = vbCrLf

End Sub
Public Function BitShiftRight(ByVal intValue As Long, ByVal intShiftBy As Byte) As Variant

    'Bit shifts a value to the right

    Dim lngShiftVal As Long

    lngShiftVal = 2 ^ intShiftBy

    BitShiftRight = Fix(intValue / lngShiftVal)

End Function

Private Sub Command12_Click()
    If chkUSB.Value = 0 Then
        result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    Else
        result = BS_OpenUSBMemory(txtDrive.Text, handle)
    End If
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetBiostationID(handle, biostationID)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    result = BS_SetBiostationID(handle, biostationID)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
       
    Text1.SelText = "[Set Encryption]" & vbCrLf
    'check encryption
    Dim encConfig As BSEncryptionConfig
    result = BS_ReadEncryptionConfig(handle, encConfig)
    If result <> 0 Then
        Text1.SelText = "Cannot read encryption config : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    If encConfig.useEncryption = True Then
        Text1.SelText = "Encryption is used already." & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    'Before you change the encryption configuration, all users in the i9 should be deleted.
    result = BS_DeleteAllUser(handle)
    If result <> 0 Then
        Text1.SelText = "Cannot delete all user : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    Text1.SelText = "All users are deleted." & vbCrLf

    For i = 0 To 31
        encConfig.password(i) = 0
    Next i
    
    'Write encryption configuration to the deivce
    encConfig.password(0) = Asc("T")
    encConfig.password(1) = Asc("E")
    encConfig.password(2) = Asc("S")
    encConfig.password(3) = Asc("T")
    encConfig.password(4) = Asc("1")
    encConfig.password(5) = Asc("2")
    encConfig.password(6) = Asc("3")
    encConfig.useEncryption = True
    
    result = BS_WriteEncryptionConfig(handle, encConfig)
    If result <> 0 Then
        Text1.SelText = "Cannot write encryption config : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    Text1.SelText = "Encryption is on." & vbCrLf
    
    'Scan the encoded template and enroll a user.
    Dim templateData() As Byte
    Dim checksum As Long
    Dim userHdr As BSUserhdr
    Dim sString(12) As Integer
    
    userHdr.ID = 10
    userHdr.headerVersion = 0
    userHdr.adminLevel = 241
    userHdr.securityLevel = 260
    userHdr.statusMask = 0
    userHdr.accessGroupMask = &HFFFFFFFF
    result = BS_ConvertToUTF16("Test User", sString(0), 9)
    CopyMemory userHdr.userName(0), sString(0), 9
    result = BS_ConvertToUTF16("Test Group", sString(0), 10)
    CopyMemory userHdr.department(0), sString(0), 10
    userHdr.numOfFinger = 1
    userHdr.duressMask = &H0

    ReDim templateData(384 * userHdr.numOfFinger * 2 - 1)
    
    result = BS_Disable(handle, 60)
    For i = 0 To (userHdr.numOfFinger * 2) - 1
        'The scanned template data have been encorded.
        Text1.SelText = "Place a finger." & vbCrLf
        result = BS_ScanTemplate(handle, templateData(i * 384))
        If result <> 0 Then
            Text1.SelText = "Cannot get the template data : " & result & vbCrLf
            BS_Enable (handle)
            If chkUSB.Value = 0 Then
                BS_CloseSocket (handle)
            Else
                BS_CloseUSBMemory (handle)
            End If
            Exit Sub
        End If
        
        checksum = 0
        For j = 0 To 384 - 1
            checksum = checksum + templateData(i * 384 + j)
        Next j
        userHdr.checksum(Int(i / 2) + 1) = BitShiftRight(checksum, 8)
        userHdr.checksum(Int(i / 2)) = checksum And &HFF
    Next i

    result = BS_EnrollUser(handle, userHdr, templateData(0))

    If result <> 0 Then
        Text1.SelText = "Cannot enroll a user : " & result & vbCrLf
        BS_Enable (handle)
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    Text1.SelText = "User Enroll Success with encoded template data."
    BS_Enable (handle)
        
    If chkUSB.Value = 0 Then
        BS_CloseSocket (handle)
    Else
        BS_CloseUSBMemory (handle)
    End If
    
    Text1.SelText = vbCrLf
End Sub

Private Sub Command15_Click()
    Dim privateInfo() As BSPrivateInfo
    Dim numOfUser As Long
    Dim numOfTemplate As Long
    
    result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetBiostationID(handle, biostationID)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If
    
    result = BS_SetBiostationID(handle, biostationID)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If

    Text1.SelText = "[Get All Private Info]" & vbCrLf
    
    result = BS_GetUserDBInfo(handle, numOfUser, numOfTemplate)
    If result <> 0 Then
        Text1.SelText = "Cannot get user db info : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If
    
    
    ReDim privateInfo(numOfUser)

    
    'This function only get the private info( not include image ).
    result = BS_GetAllPrivateInfo(handle, privateInfo(0), numOfUser)
    If result <> 0 Then
        Text1.SelText = "Cannot get all private info : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If

    Text1.SelText = "Get All Private Info success " & vbCrLf
    
    BS_CloseSocket (handle)
    
    Text1.SelText = vbCrLf


End Sub

Private Sub Command16_Click()
    Dim privateInfo As BSPrivateInfo
    
    result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetBiostationID(handle, biostationID)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If
    
    result = BS_SetBiostationID(handle, biostationID)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If

    Text1.SelText = "[Get Private Info]" & vbCrLf
    
    'This function only get the private info( not include image ).
    result = BS_GetPrivateInfo(handle, 10, privateInfo)
    If result <> 0 Then
        Text1.SelText = "Cannot get private info : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If

    Text1.SelText = "Get Private Info success " & vbCrLf
    
    BS_CloseSocket (handle)
    
    Text1.SelText = vbCrLf


End Sub

Private Sub Command17_Click()
    Dim privateInfo As BSPrivateInfo
    Dim displayConfig As BSDisplayConfig
    
    result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetBiostationID(handle, biostationID)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If
    
    result = BS_SetBiostationID(handle, biostationID)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If

    Text1.SelText = "[Set Private Info]" & vbCrLf

    result = BS_ReadDisplayConfig(handle, displayConfig)
    If result <> 0 Then
        Text1.SelText = "Cannot get display config : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If

    displayConfig.usePrivateAuth = True
    result = BS_WriteDisplayConfig(handle, displayConfig)
    If result <> 0 Then
        Text1.SelText = "Cannot set display config : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If


    privateInfo.ID = 10
    privateInfo.useImage = True
    result = BS_ConvertToUTF8("Goog Morning!!" & vbCrLf & "Good Luck!!", privateInfo.greetingMsg(0), BS_MAX_PRIVATE_MSG_LEN)
    
    result = BS_SetPrivateInfo(handle, PRIVATE_TYPE_USER, privateInfo, App.Path & "\pic2.png")
    If result <> 0 Then
        Text1.SelText = "Cannot set private info : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If

    Text1.SelText = "Set Private Info success " & vbCrLf
    
    BS_CloseSocket (handle)
    
    Text1.SelText = vbCrLf

End Sub

Private Sub Command18_Click()
    Dim userHdr() As XSUserhdr
    Dim numOfUser As Long
    Dim numOfTemplate As Long
    
    If chkUSB.Value = 0 Then
        result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    Else
        result = BS_OpenUSBMemory(txtDrive.Text, handle)
    End If
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetBiostationID(handle, biostationID)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    result = BS_SetBiostationID(handle, biostationID)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If

    Text1.SelText = "[Get All User Info]" & vbCrLf
    
    result = BS_GetUserDBInfo(handle, numOfUser, numOfTemplate)
    If result <> 0 Then
        Text1.SelText = "Cannot get user db info : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    
    ReDim userHdr(numOfUser)
    result = BS_GetAllUserInfoXStation(handle, userHdr(0), numOfUser)
    If result <> 0 Then
        Text1.SelText = "Cannot get user info : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
        
    Text1.SelText = "Get All User Info success " & vbCrLf
    If chkUSB.Value = 0 Then
        BS_CloseSocket (handle)
    Else
        BS_CloseUSBMemory (handle)
    End If
    
    Text1.SelText = vbCrLf

End Sub

Private Sub Command19_Click()
    Dim userHdr As XSUserhdr
    
    If chkUSB.Value = 0 Then
        result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    Else
        result = BS_OpenUSBMemory(txtDrive.Text, handle)
    End If
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetBiostationID(handle, biostationID)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    result = BS_SetBiostationID(handle, biostationID)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If

    Text1.SelText = "[Get User Info]" & vbCrLf
    
    result = BS_GetUserInfoXStation(handle, 10, userHdr)
    If result <> 0 Then
        Text1.SelText = "Cannot get user header : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
        
    Text1.SelText = "Get user header success " & vbCrLf
    If chkUSB.Value = 0 Then
        BS_CloseSocket (handle)
    Else
        BS_CloseUSBMemory (handle)
    End If
    
    Text1.SelText = vbCrLf

End Sub

Private Sub Command2_Click()
    Dim i, startValue, diff As Long
    Dim logRecord() As BSLogRecordEx
    
    If chkUSB.Value = 0 Then
        result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    Else
        result = BS_OpenUSBMemory(txtDrive.Text, handle)
    End If
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetBiostationID(handle, biostationID)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    result = BS_SetBiostationID(handle, biostationID)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
   
    
    Text1.SelText = "[Read Last Log]" & vbCrLf
    
    Dim currentTime As Long
    
    ReDim logRecord(21844)

    currentTime = DateDiff("s", "1/1/1970", Now)
    result = BS_ReadLogEx(handle, currentTime - 3600, currentTime, numOfLog, logRecord(0))
    If result <> 0 Then
        Text1.SelText = "Cannot read log record : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    Text1.SelText = "Retrieved " & numOfLog & " logs" & vbCrLf
    
    If numOfLog <= 50 Then
        startValue = 0
    Else
        startValue = numOfLog - 50
    End If
    
    For i = startValue To numOfLog - 1
        diff = currentTime - logRecord(i).eventTime
           
        Text1.SelText = "Event:" & logRecord(i).event & "  User:" & logRecord(i).userID & "  Time:" & format(DateAdd("s", -(diff), Now), "yyyy-mm-dd hh:mm:ss") _
                        & "  TNA:" & logRecord(i).tnaKey & vbCrLf
    Next

    Text1.SelText = "The last 50 of specified logs have been displayed..." & vbCrLf
    
    If chkUSB.Value = 0 Then
        BS_CloseSocket (handle)
    Else
        BS_CloseUSBMemory (handle)
    End If
    
    Text1.SelText = vbCrLf
End Sub

Private Sub Command20_Click()
    Dim userHdr As XSUserhdr
    
    If chkUSB.Value = 0 Then
        result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    Else
        result = BS_OpenUSBMemory(txtDrive.Text, handle)
    End If
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetBiostationID(handle, biostationID)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    result = BS_SetBiostationID(handle, biostationID)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If

    Text1.SelText = "[Get User Header]" & vbCrLf
    
    result = BS_GetUserXStation(handle, 10, userHdr)
    If result <> 0 Then
        Text1.SelText = "Cannot get user header : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
        
    Text1.SelText = "Get user header success " & vbCrLf
    If chkUSB.Value = 0 Then
        BS_CloseSocket (handle)
    Else
        BS_CloseUSBMemory (handle)
    End If
    
    Text1.SelText = vbCrLf
End Sub

Private Sub Command21_Click()
    Dim userHdr As XSUserhdr
    
    If chkUSB.Value = 0 Then
        result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    Else
        result = BS_OpenUSBMemory(txtDrive.Text, handle)
    End If
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetBiostationID(handle, biostationID)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    result = BS_SetBiostationID(handle, biostationID)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If

    Text1.SelText = "[Enroll User]" & vbCrLf

    userHdr.ID = 10
    userHdr.adminLevel = 241
    userHdr.securityLevel = 260
    userHdr.statusMask = 0
    userHdr.accessGroupMask = &HFFFFFFFF
    result = BS_ConvertToUTF16("Test User", userHdr.userName(0), BS_MAX_NAME_LEN)
    result = BS_ConvertToUTF16("Test Group", userHdr.department(0), BS_MAX_NAME_LEN)
    userHdr.numOfFinger = 0
    userHdr.cardID = 123456
    userHdr.bypassCard = flase
    userHdr.disabled = False
    userHdr.expireDateTime = 0
    
    result = BS_EnrollUserXStation(handle, userHdr)

    If result <> 0 Then
        Text1.SelText = "Cannot enroll a user : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If

    Text1.SelText = "Enroll user success " & vbCrLf
    If chkUSB.Value = 0 Then
        BS_CloseSocket (handle)
    Else
        BS_CloseUSBMemory (handle)
    End If
    
    Text1.SelText = vbCrLf
End Sub

Private Sub Command3_Click()
    If chkUSB.Value = 0 Then
        result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    Else
        result = BS_OpenUSBMemory(txtDrive.Text, handle)
    End If
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetBiostationID(handle, biostationID)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    result = BS_SetBiostationID(handle, biostationID)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If

    'Write Config
    Dim ipConfig As BSIPConfig
    
    ipConfig.lanType = BS_IP_WLAN
    ipConfig.useDHCP = False
    ipConfig.port = 1470
    
    ipConfig.ipAddr(0) = Asc("1")
    ipConfig.ipAddr(1) = Asc("9")
    ipConfig.ipAddr(2) = Asc("2")
    ipConfig.ipAddr(3) = Asc(".")
    ipConfig.ipAddr(4) = Asc("1")
    ipConfig.ipAddr(5) = Asc("6")
    ipConfig.ipAddr(6) = Asc("8")
    ipConfig.ipAddr(7) = Asc(".")
    ipConfig.ipAddr(8) = Asc("2")
    ipConfig.ipAddr(9) = Asc("1")
    ipConfig.ipAddr(10) = 0
    
    ipConfig.gateway(0) = Asc("1")
    ipConfig.gateway(1) = Asc("9")
    ipConfig.gateway(2) = Asc("2")
    ipConfig.gateway(3) = Asc(".")
    ipConfig.gateway(4) = Asc("1")
    ipConfig.gateway(5) = Asc("6")
    ipConfig.gateway(6) = Asc("8")
    ipConfig.gateway(7) = Asc(".")
    ipConfig.gateway(8) = Asc("2")
    ipConfig.gateway(9) = Asc("1")
    ipConfig.gateway(10) = 0
    
    ipConfig.subnetMask(0) = Asc("2")
    ipConfig.subnetMask(1) = Asc("5")
    ipConfig.subnetMask(2) = Asc("5")
    ipConfig.subnetMask(3) = Asc(".")
    ipConfig.subnetMask(4) = Asc("2")
    ipConfig.subnetMask(5) = Asc("5")
    ipConfig.subnetMask(6) = Asc("5")
    ipConfig.subnetMask(7) = Asc(".")
    ipConfig.subnetMask(8) = Asc("255")
    ipConfig.subnetMask(9) = Asc("0")
    ipConfig.subnetMask(10) = 0
    
    ipConfig.serverIP(0) = Asc("1")
    ipConfig.serverIP(1) = Asc("9")
    ipConfig.serverIP(2) = Asc("2")
    ipConfig.serverIP(3) = Asc(".")
    ipConfig.serverIP(4) = Asc("1")
    ipConfig.serverIP(5) = Asc("6")
    ipConfig.serverIP(6) = Asc("8")
    ipConfig.serverIP(7) = Asc(".")
    ipConfig.serverIP(8) = Asc("2")
    ipConfig.serverIP(9) = Asc("1")
    ipConfig.serverIP(10) = 0
    
    ipConfig.maxConnextion = 4
    ipConfig.useServer = False
    ipConfig.serverPort = 1480
    ipConfig.syncTimeWithServer = False
    
    result = BS_WriteIPConfig(handle, ipConfig)
    If result <> 0 Then
        Text1.SelText = "Cannot write ip configuration : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    Text1.SelText = "[Write ip configuration]" & vbCrLf
    
    Dim szIpAddress As String
    Dim i As Integer
    
    szIpAddress = ""
    For i = 0 To 9
        szIpAddress = szIpAddress & Chr(ipConfig.ipAddr(i))
    Next i
    
    Text1.SelText = "IP Address : " & szIpAddress & vbCrLf
   
    szIpAddress = ""
    For i = 0 To 9
        szIpAddress = szIpAddress & Chr(ipConfig.gateway(i))
    Next i
    
    Text1.SelText = "Gate Way : " & szIpAddress & vbCrLf
     
    szIpAddress = ""
    For i = 0 To 9
        szIpAddress = szIpAddress & Chr(ipConfig.subnetMask(i))
    Next i
    
    Text1.SelText = "Subnet Mask : " & szIpAddress & vbCrLf
    
    Text1.SelText = "Port : " & ipConfig.port & vbCrLf
    
    szIpAddress = ""
    For i = 0 To 9
        szIpAddress = szIpAddress & Chr(ipConfig.serverIP(i))
    Next i
    
    Text1.SelText = "Server IP : " & szIpAddress & vbCrLf
    
    Text1.SelText = "Server Port : " & ipConfig.serverPort & vbCrLf
    
    If chkUSB.Value = 0 Then
        BS_CloseSocket (handle)
    Else
        BS_CloseUSBMemory (handle)
    End If
    
    Text1.SelText = vbCrLf
End Sub

Private Sub Command4_Click()
    If chkUSB.Value = 0 Then
        result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    Else
        result = BS_OpenUSBMemory(txtDrive.Text, handle)
    End If
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetBiostationID(handle, biostationID)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    result = BS_SetBiostationID(handle, biostationID)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If

    'Function
    Dim ipConfig As BSIPConfig
    
    result = BS_ReadIPConfig(handle, ipConfig)
    If result <> 0 Then
        Text1.SelText = "Cannot write ip configuration : " & result & vbCrLf
        If chkUSB.Value = 0 Then
            BS_CloseSocket (handle)
        Else
            BS_CloseUSBMemory (handle)
        End If
        Exit Sub
    End If
    
    Text1.SelText = "[Read ip configuration]" & vbCrLf
    
    Dim szIpAddress As String
    Dim i As Integer
    
    szIpAddress = ""
    For i = 0 To 9
        szIpAddress = szIpAddress & Chr(ipConfig.ipAddr(i))
    Next i
    
    Text1.SelText = "IP Address : " & szIpAddress & vbCrLf
   
    szIpAddress = ""
    For i = 0 To 9
        szIpAddress = szIpAddress & Chr(ipConfig.gateway(i))
    Next i
    
    Text1.SelText = "Gate Way : " & szIpAddress & vbCrLf
     
    szIpAddress = ""
    For i = 0 To 9
        szIpAddress = szIpAddress & Chr(ipConfig.subnetMask(i))
    Next i
    
    Text1.SelText = "Subnet Mask : " & szIpAddress & vbCrLf
    
    Text1.SelText = "Port : " & ipConfig.port & vbCrLf
    
    szIpAddress = ""
    For i = 0 To 9
        szIpAddress = szIpAddress & Chr(ipConfig.serverIP(i))
    Next i
    
    Text1.SelText = "Server IP : " & szIpAddress & vbCrLf
    
    Text1.SelText = "Server Port : " & ipConfig.serverPort & vbCrLf

    If chkUSB.Value = 0 Then
        BS_CloseSocket (handle)
    Else
        BS_CloseUSBMemory (handle)
    End If
    
    Text1.SelText = vbCrLf
End Sub

Private Sub Command5_Click()
    
    result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetDeviceID(handle, deviceID, deviceType)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If
    
    result = BS_SetDeviceID(handle, deviceID, 1)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If

    Text1.SelText = "[Send NoticeEx]" & vbCrLf
    
    result = BS_Disable(handle, 20)
    
    result = BS_SendNoticeEx(handle, "한글 테스트 공지사항" & "한글 테스트 공지사항" & vbNull, 1)
    If result <> 0 Then
        Text1.SelText = "Cannot send Notice data : " & result & vbCrLf
        result = BS_Enable(handle)
        BS_CloseSocket (handle)
        Exit Sub
    End If
    
    
    result = BS_Enable(handle)
   
    BS_CloseSocket (handle)
End Sub

Private Sub Command6_Click()

    result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetDeviceID(handle, deviceID, deviceType)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If
    
    result = BS_SetDeviceID(handle, deviceID, 1)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If

    Text1.SelText = "[Send NoticeEx]" & vbCrLf
    
    
    result = BS_Disable(handle, 20)
    
    result = BS_SendNoticeEx(handle, String$(1024, "A"), 1)
    If result <> 0 Then
        Text1.SelText = "Cannot send Notice data : " & result & vbCrLf
        result = BS_Enable(handle)
        BS_CloseSocket (handle)
        Exit Sub
    End If
    
    result = BS_Enable(handle)
   
    BS_CloseSocket (handle)
End Sub


Private Sub Command9_Click()

    result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetDeviceID(handle, deviceID, deviceType)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If
    
    result = BS_SetDeviceID(handle, deviceID, 1)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If

    Text1.SelText = "[Send NoticeEx]" & vbCrLf
    
    
    result = BS_Disable(handle, 20)
    
    
    Dim imageFile(3) As String
    
    imageFile(0) = "c:\temp\sample1.jpg"
    imageFile(1) = "c:\temp\sample2.jpg"
    imageFile(2) = "c:\temp\sample3.jpg"
    
    numOfPicture = 3    '이미지 갯수
    
    'delete all
    BS_DeleteSlideShow (handle)
    
    For imageIndex = 0 To 2
    
        result = BS_SetSlideShow(handle, numOfPicture, imageIndex, imageFile(imageIndex))
     
        If result <> 0 Then
            Text1.SelText = "Cannot set Slide Show data : " & result & vbCrLf
            result = BS_Enable(handle)
            BS_CloseSocket (handle)
            Exit Sub
        End If
    Next
    
    result = BS_Enable(handle)
   
    BS_CloseSocket (handle)
End Sub

Private Sub Command7_Click()
    Dim opMode As XSOPModeConfig
    
    result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetBiostationID(handle, biostationID)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If
    
    result = BS_SetBiostationID(handle, biostationID)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If

    Text1.SelText = "[Get OPMode Info]" & vbCrLf
    
    result = BS_ReadXSOPModeConfig(handle, opMode)
    If result <> 0 Then
        Text1.SelText = "Cannot get OPMode Info : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If
        
    Text1.SelText = "Get OPMode Info success " & vbCrLf
    
    Text1.SelText = "Server Matching " & opMode.useServerMatching & vbCrLf
    
    BS_CloseSocket (handle)
    
    Text1.SelText = vbCrLf

End Sub

Private Sub Command8_Click()
    Dim opMode As XSOPModeConfig
    
    result = BS_OpenSocket(txtIPAddress.Text, Int(txtPort.Text), handle)
    
    If result <> 0 Then
        Text1.SelText = "Cannot open the communication channel : " & result & vbCrLf
        Exit Sub
    End If
    
    result = BS_GetBiostationID(handle, biostationID)
    If result <> 0 Then
        Text1.SelText = "Cannot get the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If
    
    result = BS_SetBiostationID(handle, biostationID)
        
    If result <> 0 Then
        Text1.SelText = "Cannot set the ID : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If


    Text1.SelText = "[Get OPMode Info]" & vbCrLf

    result = BS_ReadXSOPModeConfig(handle, opMode)
    If result <> 0 Then
        Text1.SelText = "Cannot get OPMode Info : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If
    
    Text1.SelText = "Get OPMode Info success " & vbCrLf
    Text1.SelText = "Server Matching " & opMode.useServerMatching & vbCrLf


    Text1.SelText = "[Set OPMode Info]" & vbCrLf
    
    If opMode.useServerMatching <> 0 Then
        opMode.useServerMatching = 0
    Else
        opMode.useServerMatching = 1
    End If
    
    
    result = BS_WriteXSOPModeConfig(handle, opMode)
    If result <> 0 Then
        Text1.SelText = "Cannot set OPMode Info : " & result & vbCrLf
        BS_CloseSocket (handle)
        Exit Sub
    End If
        
    Text1.SelText = "Set OPMode Info success " & vbCrLf
    
    BS_CloseSocket (handle)
    
    Text1.SelText = vbCrLf
End Sub




Private Sub Form_Load()
    result = BS_InitSDK()
End Sub

