using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SDK_TestApp
{
    class BSSDK
    {
        //
        // API Declarations
        //
        public const int KEEP_ALIVE_INTERVAL		= 100;
 
        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_InitSDK")]
        public static extern int BS_InitSDK();

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_UnInitSDK")]
        public static extern int BS_UnInitSDK();

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_StartServerApp")]
        public static extern int BS_StartServerApp( int port, int maxConnection, string sslPath, string sslPassword, int connCheckDuration );

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_StopServerApp")]
        public static extern int BS_StopServerApp();

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_SetConnectedCallback")]
        public static extern int BS_SetConnectedCallback(BS_ConnectionProc proc, bool syncOp, bool autoResponse );

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_SetDisconnectedCallback")]
        public static extern int BS_SetDisconnectedCallback(BS_DisconnectedProc proc, bool syncOp);

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_SetRequestStartedCallback")]
        public static extern int BS_SetRequestStartedCallback(BS_RequestStartProc proc, bool syncOp, bool autoResponse);

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_SetLogCallback")]
        public static extern int BS_SetLogCallback(BS_LogProc proc, bool syncOp, bool autoResponse);

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_SetImageLogCallback")]
        public static extern int BS_SetImageLogCallback(BS_ImageLogProc proc, bool syncOp, bool autoResponse);
 
        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_SetRequestUserInfoCallback")]
        public static extern int BS_SetRequestUserInfoCallback(BS_RequestUserInfoProc proc, bool syncOp);
        
        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_SetRequestMatchingCallback")]
        public static extern int BS_SetRequestMatchingCallback(BS_RequestMatchingProc proc, bool syncOp);

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_SetSynchronousOperation")]
        public static extern int BS_SetSynchronousOperation(bool syncOp );

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_IssueCertificate")]
        public static extern int BS_IssueCertificate(int handle, uint deviceID );

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_DeleteCertificate")]
        public static extern int BS_DeleteCertificate(int handle );

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_StartRequest")]
        public static extern int BS_StartRequest(int handle, int deviceType, int port );

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_GetConnectedList")]
        public static extern int BS_GetConnectedList(uint[] deviceList, ref int count );

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_CloseConnection")]
        public static extern int BS_CloseConnection(uint deviceID  );

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_ConvertToUTF8")]
        public static extern int BS_ConvertToUTF8(char[] msg, char[] utf8Msg, int limitLen);

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_ConvertToUTF16")]
        public static extern int BS_ConvertToUTF16(char[] msg, char[] utf16Msg, int limitLen);

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_SetDeviceID")]
        public static extern int BS_SetDeviceID(int handle, int deviceID, int deviceType);

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_WriteConfig")]
        public static extern int BS_WriteConfig(int handle, int configType, int size, IntPtr data);

        [DllImport("BS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "BS_ReadConfig")]
        public static extern int BS_ReadConfig(int handle, int configType, ref int size, ref IntPtr data);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(int hWnd, int msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)] // used for button-down & button-up
        public static extern int PostMessage(int hWnd, int msg, int wParam, int lParam);

        public delegate int BS_ConnectionProc(int handle, uint deviceID, int deviceType, int connectionType, int functionType, string ipAddress);
        public delegate int BS_DisconnectedProc(int handle, uint deviceID, int deviceType, int connectionType, int functionType, string ipAddress);
        public delegate int BS_RequestStartProc(int handle, uint deviceID, int deviceType, int connectionType, int functionType, string ipAddress);
        public delegate int BS_LogProc(int handle, uint deviceID, int deviceType, int connectionType, IntPtr data);
        public delegate int BS_ImageLogProc(int handle, uint deviceID, int deviceType, int connectionType, IntPtr data, int dataLen);
        public delegate int BS_RequestMatchingProc(int handle, uint deviceID, int deviceType, int connectionType, int matchingType, uint ID, IntPtr templateData, IntPtr userHdr, ref int isDuress);
        public delegate int BS_RequestUserInfoProc(int handle, uint deviceID, int deviceType, int connectionType, int idType, uint ID, uint customID, IntPtr userHdr);


        // 
        // Constants
        //

        public const int BS_SUCCESS = 0;

        // communication channel error
        public const int BS_ERR_NO_AVAILABLE_CHANNEL = -100;
        public const int BS_ERR_INVALID_COMM_HANDLE = -101;
        public const int BS_ERR_CANNOT_WRITE_CHANNEL = -102;
        public const int BS_ERR_WRITE_CHANNEL_TIMEOUT = -103;
        public const int BS_ERR_CANNOT_READ_CHANNEL = -104;
        public const int BS_ERR_READ_CHANNEL_TIMEOUT = -105;
        public const int BS_ERR_CHANNEL_OVERFLOW = -106;
        public const int BS_ERR_CHANNEL_CLOSED = -107;

        // socket error
        public const int BS_ERR_CANNOT_INIT_SOCKET = -200;
        public const int BS_ERR_CANNOT_OPEN_SOCKET = -201;
        public const int BS_ERR_CANNOT_CONNECT_SOCKET = -202;
        public const int BS_ERR_SOCKET_CLOSED = -203;

        // serial error
        public const int BS_ERR_CANNOT_OPEN_SERIAL = -220;

        // USB error
        public const int BS_ERR_CANNOT_OPEN_USB = -240;

        // USB memory error
        public const int BS_ERR_INVALID_USB_MEMORY = -260;
        public const int BS_ERR_NO_MORE_USB_MEMORY = -261;
        public const int BS_ERR_CANNOT_FIND_USB_MEMORY = -262;
        public const int BS_ERR_VT_EXIST_IN_MEMORY = -263;
        public const int BS_ERR_USB_MEMORY_FULL = -264;
        public const int BS_ERR_NO_MORE_VT = -265;

        // generic command error code
        public const int BS_ERR_BUSY = -300;
        public const int BS_ERR_INVALID_PACKET = -301;
        public const int BS_ERR_CHECKSUM = -302;
        public const int BS_ERR_UNSUPPORTED = -303;
        public const int BS_ERR_FILE_IO = -304;
        public const int BS_ERR_DISK_FULL = -305;
        public const int BS_ERR_NOT_FOUND = -306;
        public const int BS_ERR_INVALID_PARAM = -307;
        public const int BS_ERR_RTC = -308;
        public const int BS_ERR_MEM_FULL = -309;
        public const int BS_ERR_DB_FULL = -310;
        public const int BS_ERR_INVALID_ID = -311;
        public const int BS_ERR_USB_DISABLED = -312;
        public const int BS_ERR_COM_DISABLED = -313;
        public const int BS_ERR_WRONG_PASSWORD = -314;
        public const int BS_ERR_TRY_AGAIN = -315;
        public const int BS_ERR_EXIST_FINGER = -316;
        // user related error
        public const int BS_ERR_NO_USER = -320;
        public const int BS_ERR_CANNOT_CHANGE_IMG_VIEW = -321;

        // server error
        public const int BS_ERR_NO_MORE_TERMINAL = -400;
        public const int BS_ERR_TERMINAL_NOT_FOUND = -401;
        public const int BS_ERR_TERMINAL_COMM_ERROR = -402;
        public const int BS_ERR_TERMINAL_NOT_AUTHORIZED = -403;
        public const int BS_ERR_TERMINAL_BUSY = -404;

        // server db error
        public const int BS_ERR_DB_NOT_EXIST = -500;
        public const int BS_ERR_CANNOT_CONNECT_TO_DB = -501;
        public const int BS_ERR_DB_INTERNAL_ERROR = -502;

        // SSL error
        public const int BS_ERR_CANNOT_INIT_SSL = -601;
        public const int BS_ERR_SSL_INVALID_CTX = -602;
        public const int BS_ERR_SSL_INVALID_CERTFILE = -603;
        public const int BS_ERR_SSL_INVALID_KEYFILE = -604;
        public const int BS_ERR_SSL_INVALID_CAFILE = -605;
        public const int BS_ERR_SSL_INVALID_PATH = -606;
        public const int BS_ERR_SSL_CANNOT_CONNECT = -607;

        public const int BS_ERR_INVALID_DATA = -608;

        public const int BS_ERR_UNKNOWN = -9999;


        public const int BS_DEVICE_UNKNOWN = -1;
        public const int BS_DEVICE_BIOSTATION = 0;
        public const int BS_DEVICE_BIOENTRY_PLUS = 1;
        public const int BS_DEVICE_BIOLITE = 2;
        public const int BS_DEVICE_XPASS = 3;
        public const int BS_DEVICE_DSTATION = 4;
        public const int BS_DEVICE_XSTATION = 5;
        public const int BS_DEVICE_BIOSTATION2 = 6;
        public const int BS_DEVICE_XPASS_SLIM = 7;
        public const int BS_DEVICE_FSTATION = 10;
        public const int BS_DEVICE_BIOENTRY_W = 11;
        public const int BS_DEVICE_XPASS_SLIM2 = 12;

        public const int BEPLUS_CONFIG = 0x50;
        public const int BEPLUS_CONFIG_SYS_INFO = 0x51;

        public const int BLN_CONFIG = 0x70;
        public const int BLN_CONFIG_SYS_INFO = 0x71;

        public const int BS_CONFIG_SYS_INFO = 0x41;
        public const int BS_CONFIG_TCPIP = 0x10;

        public const int NO_ACCESS_GROUP = 0xFD;
        public const int FULL_ACCESS_GROUP = 0xFE;

        public const int BS_AUTH_MODE_DISABLED = 0;
        public const int BS_AUTH_FINGER_ONLY = 1020;
        public const int BS_AUTH_FINGER_N_PASSWORD = 1021;
        public const int BS_AUTH_FINGER_OR_PASSWORD = 1022;
        public const int BS_AUTH_PASS_ONLY = 1023;
        public const int BS_AUTH_CARD_ONLY = 1024;

        public const int REQUEST_IDENTIFY = 0x01;
        public const int REQUEST_VERIFY = 0x02;

        public const int ID_USER = 0x01;
        public const int ID_CARD = 0x02;

        // bepl
        public const int BE_CARD_VERSION_1 = 0x13;

		public const int BE_USER_LEVEL_NORMAL = 0;
        public const int BE_USER_LEVEL_ADMIN  = 1;

		// Security leve
        public const int BE_USER_SECURITY_DEFAULT = 0;
        public const int BE_USER_SECURITY_LOWER = 1;
        public const int BE_USER_SECURITY_LOW = 2;
        public const int BE_USER_SECURITY_NORMAL = 3;
        public const int BE_USER_SECURITY_HIGH = 4;
        public const int BE_USER_SECURITY_HIGHER = 5;


        public const int NORMAL_FINGER = 0x01;
        public const int DURESS_FINGER = 0x02;

        // user levels for BioStation
        public const int BS_USER_ADMIN = 240;
        public const int BS_USER_NORMAL = 241;

        // security levels for BioStation
        public const int BS_USER_SECURITY_DEFAULT = 260;
        public const int BS_USER_SECURITY_LOWER = 261;
        public const int BS_USER_SECURITY_LOW = 262;
        public const int BS_USER_SECURITY_NORMAL = 263;
        public const int BS_USER_SECURITY_HIGH = 264;
        public const int BS_USER_SECURITY_HIGHER = 265;

        // log events
        public const int BE_EVENT_SCAN_SUCCESS = 0x58;
        public const int BE_EVENT_ENROLL_BAD_FINGER = 0x16;
        public const int BE_EVENT_ENROLL_SUCCESS = 0x17;
        public const int BE_EVENT_ENROLL_FAIL = 0x18;
        public const int BE_EVENT_ENROLL_CANCELED = 0x19;

        public const int BE_EVENT_VERIFY_BAD_FINGER = 0x26;
        public const int BE_EVENT_VERIFY_SUCCESS = 0x27;
        public const int BE_EVENT_VERIFY_FAIL = 0x28;
        public const int BE_EVENT_VERIFY_CANCELED = 0x29;
        public const int BE_EVENT_VERIFY_NO_FINGER = 0x2a;

        public const int BE_EVENT_IDENTIFY_BAD_FINGER = 0x36;
        public const int BE_EVENT_IDENTIFY_SUCCESS = 0x37;
        public const int BE_EVENT_IDENTIFY_FAIL = 0x38;
        public const int BE_EVENT_IDENTIFY_CANCELED = 0x39;

        public const int BE_EVENT_DELETE_BAD_FINGER = 0x46;
        public const int BE_EVENT_DELETE_SUCCESS = 0x47;
        public const int BE_EVENT_DELETE_FAIL = 0x48;
        public const int BE_EVENT_DELETE_ALL_SUCCESS = 0x49;

        public const int BE_EVENT_VERIFY_DURESS = 0x62;
        public const int BE_EVENT_IDENTIFY_DURESS = 0x63;

        public const int BE_EVENT_TAMPER_SWITCH_ON = 0x64;
        public const int BE_EVENT_TAMPER_SWITCH_OFF = 0x65;

        public const int BE_EVENT_SYS_STARTED = 0x6a;
        public const int BE_EVENT_IDENTIFY_NOT_GRANTED = 0x6d;
        public const int BE_EVENT_VERIFY_NOT_GRANTED = 0x6e;

        public const int BE_EVENT_IDENTIFY_LIMIT_COUNT = 0x6f;
        public const int BE_EVENT_IDENTIFY_LIMIT_TIME = 0x70;

        public const int BE_EVENT_IDENTIFY_DISABLED = 0x71;
        public const int BE_EVENT_IDENTIFY_EXPIRED = 0x72;

        public const int BE_EVENT_APB_FAIL = 0x73;
        public const int BE_EVENT_COUNT_LIMIT = 0x74;
        public const int BE_EVENT_TIME_INTERVAL_LIMIT = 0x75;
        public const int BE_EVENT_INVALID_AUTH_MODE = 0x76;
        public const int BE_EVENT_EXPIRED_USER = 0x77;
        public const int BE_EVENT_NOT_GRANTED = 0x78;

        public const int BE_EVENT_DETECT_INPUT0 = 0x54;
        public const int BE_EVENT_DETECT_INPUT1 = 0x55;

        public const int BE_EVENT_TIMEOUT = 0x90;

        public const int BS_EVENT_RELAY_ON = 0x80;
        public const int BS_EVENT_RELAY_OFF = 0x81;

        public const int BE_EVENT_DOOR0_OPEN = 0x82;
        public const int BE_EVENT_DOOR1_OPEN = 0x83;
        public const int BE_EVENT_DOOR0_CLOSED = 0x84;
        public const int BE_EVENT_DOOR1_CLOSED = 0x85;

        public const int BE_EVENT_DOOR0_FORCED_OPEN = 0x86;
        public const int BE_EVENT_DOOR1_FORCED_OPEN = 0x87;

        public const int BE_EVENT_DOOR0_HELD_OPEN = 0x88;
        public const int BE_EVENT_DOOR1_HELD_OPEN = 0x89;

        public const int BE_EVENT_DOOR0_RELAY_ON = 0x8A;
        public const int BE_EVENT_DOOR1_RELAY_ON = 0x8B;

        public const int BE_EVENT_INTERNAL_INPUT0 = 0xA0;
        public const int BE_EVENT_INTERNAL_INPUT1 = 0xA1;
        public const int BE_EVENT_SECONDARY_INPUT0 = 0xA2;
        public const int BE_EVENT_SECONDARY_INPUT1 = 0xA3;

        public const int BE_EVENT_SIO0_INPUT0 = 0xB0;
        public const int BE_EVENT_SIO0_INPUT1 = 0xB1;
        public const int BE_EVENT_SIO0_INPUT2 = 0xB2;
        public const int BE_EVENT_SIO0_INPUT3 = 0xB3;

        public const int BE_EVENT_SIO1_INPUT0 = 0xB4;
        public const int BE_EVENT_SIO1_INPUT1 = 0xB5;
        public const int BE_EVENT_SIO1_INPUT2 = 0xB6;
        public const int BE_EVENT_SIO1_INPUT3 = 0xB7;

        public const int BE_EVENT_SIO2_INPUT0 = 0xB8;
        public const int BE_EVENT_SIO2_INPUT1 = 0xB9;
        public const int BE_EVENT_SIO2_INPUT2 = 0xBA;
        public const int BE_EVENT_SIO2_INPUT3 = 0xBB;

        public const int BE_EVENT_SIO3_INPUT0 = 0xBC;
        public const int BE_EVENT_SIO3_INPUT1 = 0xBD;
        public const int BE_EVENT_SIO3_INPUT2 = 0xBE;
        public const int BE_EVENT_SIO3_INPUT3 = 0xBF;

        public const int BE_EVENT_LOCKED = 0xC0;
        public const int BE_EVENT_UNLOCKED = 0xC1;

        public const int BE_EVENT_TIME_SET = 0xD2;
        public const int BE_EVENT_SOCK_CONN = 0xD3;
        public const int BE_EVENT_SOCK_DISCONN = 0xD4;
        public const int BE_EVENT_SERVER_SOCK_CONN = 0xD5;
        public const int BE_EVENT_SERVER_SOCK_DISCONN = 0xD6;
        public const int BE_EVENT_LINK_CONN = 0xD7;
        public const int BE_EVENT_LINK_DISCONN = 0xD8;
        public const int BE_EVENT_INIT_IP = 0xD9;
        public const int BE_EVENT_INIT_DHCP = 0xDA;
        public const int BE_EVENT_DHCP_SUCCESS = 0xDB;

  	    public const int NO_IMAGE = -1;
		public const int WRITE_ERROR = -2;
    }

    //
    // Structure Declarations
    //
    [ StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	public struct BESysInfoData
	{
        public uint magicNo;
        public int version;
        public uint timestamp;
        public uint checksum;
        [ MarshalAs( UnmanagedType.ByValArray, SizeConst=4 )] 
        public int[] headerReserved;            

        public uint ID;
        [ MarshalAs( UnmanagedType.ByValArray, SizeConst=8 )] 
        public byte[] macAddr;
        [ MarshalAs( UnmanagedType.ByValArray, SizeConst=16 )] 
        public byte[] boardVer;
        [ MarshalAs( UnmanagedType.ByValArray, SizeConst=16 )] 
        public byte[] firmwareVer;
        [ MarshalAs( UnmanagedType.ByValArray, SizeConst=40 )] 
        public int[] reserved;
	};

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct BESysInfoDataBLN
    {
        public uint magicNo;
        public int version;
        public uint timestamp;
        public uint checksum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] headerReserved;

        public uint ID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] macAddr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] boardVer;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] firmwareVer;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] productName;
        public int language;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
        public int[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct BSSysInfoConfig
    {
        public uint ID;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] macAddr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] productName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] boardVer;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] firmwareVer;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] blackfinVer;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] kernelVer;

        public int language;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct BEConfigData
    {
        // header
        public uint magicNo;
        public int version;
        public uint timestamp;
        public uint checksum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    	public int[] headerReserved;

        // operation mode
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] opMode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] opModeSchedule;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] opDoubleMode;
        public int opModePerUser; /* PROHIBITED, ALLOWED */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public int[] opReserved;

        // ip
        [MarshalAs(UnmanagedType.I1)]
        public bool useDHCP;
        public uint ipAddr;
        public uint gateway;
        public uint subnetMask;
        public uint serverIpAddr;
        public int port;
        [MarshalAs(UnmanagedType.I1)]
        public bool useServer;
        [MarshalAs(UnmanagedType.I1)]
        public bool synchTime;
        public int support100BaseT;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public int[] ipReserved;

       	// fingerprint
        public int securityLevel;
        public int fastMode;
        public int fingerReserved1;
        public int timeout; // 1 ~ 20 sec
        public int matchTimeout; // Infinite(0) ~ 10 sec
	    public int templateType;
	    public int fakeDetection;
        public bool useServerMatching;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
	    public int[] fingerReserved;

        // padding
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3016)]
        public int[] padding;
    };


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct BEConfigDataBLN
    {
        // header
        public uint magicNo;
        public int version;
        public uint timestamp;
        public uint checksum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] headerReserved;

        // operation mode
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] opMode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] opModeSchedule;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] opDualMode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] opReserved1;
        public int opModePerUser;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] identificationMode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] identificationModeSchedule;
        public int[] opReserved2;

        // ip
        [MarshalAs(UnmanagedType.I1)]
        public bool useDHCP;
        public uint ipAddr;
        public uint gateway;
        public uint subnetMask;
        public uint serverIpAddr;
        public int port;
        [MarshalAs(UnmanagedType.I1)]
        public bool useServer;
        [MarshalAs(UnmanagedType.I1)]
        public bool synchTime;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public int[] ipReserved;

        // fingerprint
        public int imageQuality;
        public int securityLevel;
        public int fastMode;
        public int fingerReserved1;
        public int timeout; // 1 ~ 20 sec
        public int matchTimeout; // Infinite(0) ~ 10 sec
        public int templateType;
        public int fakeDetection;
        [MarshalAs(UnmanagedType.I1)]
        public bool useServerMatching;
        [MarshalAs(UnmanagedType.I1)]
        public bool useCheckDuplicate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public int[] fingerReserved2;

        // padding
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3930)]
        public int[] padding;
    };


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct BSIPConfig
    {
        public int lanType;
        public bool useDHCP;
        public uint port;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] ipAddr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] gateway;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] subnetMask;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] serverIP;

        public int maxConnection;
        public bool useServer;
        public uint serverPort;
        public bool syncTimeWithServer;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public byte[] reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct BEUserHdr
    {
        public int version;
        public uint userID;

        public int startTime;
        public int expiryTime;

        public uint cardID;
        public byte cardCustomID;
        public byte commandCardFlag;
        public byte cardFlag;
        public byte cardVersion;

        public ushort adminLevel;
        public ushort securityLevel;

        public uint accessGroupMask;

        public ushort numOfFinger;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ushort[] fingerChecksum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] isDuress;

        public int disabled;
        public int opMode;
        public int dualMode;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] password;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public int[] reserved2;
    };


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct BSUserHdrEx
    {
        public uint ID;

        public ushort headerVersion;
        public ushort adminLevel;
        public ushort securityLevel;
        public ushort statusMask; 
        public uint accessGroupMask;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
        public byte[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
        public byte[] department;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public byte[] password;

        public ushort numOfFinger;
        public ushort duressMask;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public ushort[] checksum; // index 0, 1

        public ushort authMode; 
        public ushort authLimitCount; // 0 for no limit 
        public ushort reserved; 
        public ushort timedAntiPassback; // in minutes. 0 for no limit 
        public uint cardID; // 0 for not used
        public byte	bypassCard;
        public byte	disabled;
        public uint expireDateTime;
        public int customID; //card Custom ID
        public int version; // card Info Version
        public uint startDateTime; 
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DSUserHdr
    {
        public uint ID;

        public ushort headerVersion;
        public ushort adminLevel;
        public ushort securityLevel;
        public ushort statusMask;
        public uint accessGroupMask;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public ushort[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public ushort[] department;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public ushort[] password;

        public ushort numOfFinger;
        public ushort numOfFace;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] duress;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] fingerType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] reserved1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public uint[] fingerChecksum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] faceChecksum;

        public ushort authMode;
        public byte bypassCard;
        public byte disabled;

        public uint cardID;   //card ID
        public uint customID; //card Custom ID

        public uint startDateTime;
        public uint expireDateTime;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public uint[] reserved2;

    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct XSUserHdr
    {
        public uint ID;

        public ushort headerVersion;
        public ushort adminLevel;
        public ushort securityLevel;
        public ushort statusMask;
        public uint accessGroupMask;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public ushort[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public ushort[] department;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public ushort[] password;

        public ushort numOfFinger;
        public ushort numOfFace;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] duress;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] fingerType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] reserved1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public uint[] fingerChecksum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] faceChecksum;

        public ushort authMode;
        public byte bypassCard;
        public byte disabled;

        public uint cardID;   //card ID
        public uint customID; //card Custom ID

        public uint startDateTime;
        public uint expireDateTime;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public uint[] reserved2;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct BS2UserHdr
    {
        public uint ID;

        public ushort headerVersion;
        public ushort adminLevel;
        public ushort securityLevel;
        public ushort statusMask;
        public uint accessGroupMask;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public ushort[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public ushort[] department;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public ushort[] password;

        public ushort numOfFinger;
        public ushort numOfFace;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] duress;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] fingerType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] reserved1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public uint[] fingerChecksum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] faceChecksum;

        public ushort authMode;
        public byte bypassCard;
        public byte disabled;

        public uint cardID;   //card ID
        public uint customID; //card Custom ID

        public uint startDateTime;
        public uint expireDateTime;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public uint[] reserved2;
    };


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct BSAccessGroupEx
    {
        public int groupID;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String name;
        public int numOfReader;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public uint[] readerID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public int[] scheduleID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct BSLogRecord
    {
        public byte Event;
        public byte subEvent;
        public ushort tnaEvent;
        public int eventTime;
        public uint userID;
        public uint reserved;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct BSLogRecordEx
    {
	    public byte Event;
	    public byte subEvent;
	    public ushort tnaKey; 
	    public int eventTime;
	    public uint userID;
	    public uint deviceID;
	    public ushort imageSlot; // NO_IMAGE for no image, WRITE_ERROR for error
	    public ushort reserved1;
	    public int reserved2;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct BSImageLogHdr
    {
	    public byte Event;
	    public byte subEvent;
	    public ushort reserved1; 
	    public int eventTime;
	    public uint userID;
	    public uint imageSize;
	    public uint deviceID;
	    public uint reserved2;
    };  
}
