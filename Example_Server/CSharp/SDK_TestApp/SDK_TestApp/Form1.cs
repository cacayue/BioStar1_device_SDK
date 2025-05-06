using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Collections;

namespace SDK_TestApp
{
    public class RequestToken
    {
        public int handle;
        public int type;
        public int port;
    };

    public class RequestWorker
    {
        Queue requestQueue;
        Object requestLock;
        public void DoWork()
        {
            requestQueue = new Queue();
            requestLock  = new Object();
            while (!_shouldStop)
            {
                RequestToken token = null;
                int found = 0;
                lock (requestLock)
                {
                    if (requestQueue.Count > 0)
                    {
                        Object temp = requestQueue.Dequeue();
                        token = temp as RequestToken;
                        found = 1;
                    }
                } 

                if (found == 1)
                {
                    System.Threading.Thread.Sleep(1000);
                    BSSDK.BS_StartRequest(token.handle, token.type, token.port);
                    Console.WriteLine("startRequest " + token.handle);
                }
                System.Threading.Thread.Sleep(10);
            }
        }

        public void RequestStop()
        {
            _shouldStop = true;
        }

        public void AddRequest(int handle, int type, int port)
        {
            RequestToken token = new RequestToken();
            token.handle = handle;
            token.type = type;
            token.port = port;
            lock (requestLock)
            {
                requestQueue.Enqueue(token);
            }
            Console.WriteLine("add queue " + handle + "," + type);
        }
        private volatile bool _shouldStop;
    }

    public partial class Form1 : Form
    {
        private int m_Port;
        private int m_Connections;
        private int m_Count;
        private bool m_UseFunctionLock;
        private bool m_UseAutoResponse;
        private bool m_UseLock;
        private bool m_MatchingFail;

        private int LIST_ID_POS         = 0;
        private int LIST_HANDLE_POS	    = 1;
        private int LIST_IP_POS	        = 2;
        private int LIST_TYPE_POS       = 3;
        private int LIST_CONNECTION_POS = 4;
        private int LIST_STATUS_POS     = 5;

        Thread workerRequestThread;
        RequestWorker workerObject;

        public static Form1 MainForm;

        BSSDK.BS_ConnectionProc fnCallbackConnected;
        BSSDK.BS_DisconnectedProc fnCallbackDisconnected;
        BSSDK.BS_RequestStartProc fnCallbackRequestStart;
        BSSDK.BS_LogProc fnCallbackLog;
        BSSDK.BS_ImageLogProc fnCallbackImageLog;
        BSSDK.BS_RequestMatchingProc fnCallbackRequestMatching;
        BSSDK.BS_RequestUserInfoProc fnCallbackRequestUserInfo;  
 
        public Form1()
        {
            InitializeComponent();

            BSSDK.BS_InitSDK();

            deviceList.View = View.Details;
            deviceList.GridLines = true;
            deviceList.FullRowSelect = true;

            deviceList.Columns.Add("ID", 60);
            deviceList.Columns.Add("Handle", 80);
            deviceList.Columns.Add("IP", 120);
            deviceList.Columns.Add("Type", 95);
            deviceList.Columns.Add("Conn", 90);
            deviceList.Columns.Add("Status", 200);

            m_Port = 5001;
            m_Connections = 32;

            m_UseFunctionLock = false;
            m_UseAutoResponse = true;
            m_UseLock = false;
            m_Count = 0;
            m_MatchingFail = false;

            MainForm = this;

            workerObject = new RequestWorker();
            workerRequestThread = new Thread(workerObject.DoWork);
            workerRequestThread.Start();
        }

        private void Form_Closed(object sender, FormClosedEventArgs e)
        {
            BSSDK.BS_UnInitSDK();
        }
 
        private void checkMatchFail_CheckedChanged(object sender, EventArgs e)
        {
            m_MatchingFail = MainForm.checkMatchFail.Checked;
        }

        private void checkUseAutoResponse_CheckedChanged(object sender, EventArgs e)
        {
            m_UseAutoResponse = MainForm.checkUseAutoResponse.Checked;
        }

        private void checkUseFunctionLock_CheckedChanged(object sender, EventArgs e)
        {
            m_UseFunctionLock = MainForm.checkUseFunctionLock.Checked;
        }

        private void checkUseLock_CheckedChanged(object sender, EventArgs e)
        {
            m_UseLock = MainForm.checkUseLock.Checked;
        }

        private void buttonStartService_Click(object sender, EventArgs e)
        {
            //Set event procedure. 
            fnCallbackConnected = new BSSDK.BS_ConnectionProc(ConnectedProc);
            BSSDK.BS_SetConnectedCallback(fnCallbackConnected, (bool)m_UseFunctionLock, (bool)m_UseAutoResponse);

            fnCallbackDisconnected = new BSSDK.BS_DisconnectedProc(DisconnectedProc);
            BSSDK.BS_SetDisconnectedCallback(fnCallbackDisconnected, (bool)m_UseFunctionLock);

            fnCallbackRequestStart = new BSSDK.BS_RequestStartProc(RequestStartProc);
            BSSDK.BS_SetRequestStartedCallback(fnCallbackRequestStart, (bool)m_UseFunctionLock, (bool)m_UseAutoResponse);

            fnCallbackLog = new BSSDK.BS_LogProc(LogProc);
            BSSDK.BS_SetLogCallback(fnCallbackLog, m_UseFunctionLock, (bool)m_UseAutoResponse);

            fnCallbackImageLog = new BSSDK.BS_ImageLogProc(ImageLogProc);
            BSSDK.BS_SetImageLogCallback(fnCallbackImageLog, m_UseFunctionLock, (bool)m_UseAutoResponse);

            fnCallbackRequestUserInfo = new BSSDK.BS_RequestUserInfoProc(RequestUserInfoProc);
            BSSDK.BS_SetRequestUserInfoCallback(fnCallbackRequestUserInfo, (bool)m_UseFunctionLock);

            fnCallbackRequestMatching = new BSSDK.BS_RequestMatchingProc(RequestMatchingProc);
            BSSDK.BS_SetRequestMatchingCallback(fnCallbackRequestMatching, (bool)m_UseFunctionLock);

            BSSDK.BS_SetSynchronousOperation(m_UseLock);

            m_Port = (int) System.Convert.ToDecimal(MainForm.textPort.Text);
            m_Connections = (int)System.Convert.ToDecimal(MainForm.textConnection.Text);

            int result = BSSDK.BS_StartServerApp(m_Port, m_Connections, "C:\\OpenSSL\\bin\\openssl.exe", "12345678", BSSDK.KEEP_ALIVE_INTERVAL);

            if (result == BSSDK.BS_SUCCESS)
            {
                this.Text = "Server SDK Test Started ...";
            }
            else
            {
                this.Text = "BS_StartServerApp Fail!";
            }

            m_Count = 0;
        }

        private void buttonStopService_Click(object sender, EventArgs e)
        {
            BSSDK.BS_StopServerApp();

            deviceList.Items.Clear();
        }

        private void buttonStartRequest_Click(object sender, EventArgs e)
        {
            string itemString;

            if (deviceList.SelectedIndices.Count <= 0)
                return;

            int nItem = deviceList.SelectedIndices[0];

            int handle = (int)System.Convert.ToDecimal(deviceList.Items[nItem].SubItems[LIST_HANDLE_POS].Text);
            int deviceType = BSSDK.BS_DEVICE_UNKNOWN;

            string text = deviceList.Items[nItem].SubItems[LIST_TYPE_POS].Text;

            switch (text)
            {
                case "FaceStation":
                    deviceType = BSSDK.BS_DEVICE_FSTATION;
                    break;
                case "BioStation T2":
                    deviceType = BSSDK.BS_DEVICE_BIOSTATION2;
                    break;
                case "D-Station":
                    deviceType = BSSDK.BS_DEVICE_DSTATION;
                    break;
                case "X-Station":
                    deviceType = BSSDK.BS_DEVICE_XSTATION;
                    break;
                case "BioStation":
                    deviceType = BSSDK.BS_DEVICE_BIOSTATION;
                    break;
                case "BioLiteNet": 
                    deviceType = BSSDK.BS_DEVICE_BIOLITE;     
                    break;
                case "BioEntry":   
                    deviceType = BSSDK.BS_DEVICE_BIOENTRY_PLUS; 
                    break;
                case "BioEntry W":
                    deviceType = BSSDK.BS_DEVICE_BIOENTRY_W;
                    break;
                case "Xpass":
                    deviceType = BSSDK.BS_DEVICE_XPASS;
                    break;
                case "Xpass Slim":
                    deviceType = BSSDK.BS_DEVICE_XPASS_SLIM;
                    break;
                case "Xpass S2":
                    deviceType = BSSDK.BS_DEVICE_XPASS_SLIM2;
                    break;
                default:          
                    deviceType = BSSDK.BS_DEVICE_UNKNOWN;      
                    break;
            }

            if (BSSDK.BS_StartRequest(handle, deviceType, m_Port) == BSSDK.BS_SUCCESS)
            {
                itemString = "StartRequest Success";
            }
            else
            {
                itemString = "StartRequest Fail";
            }

            deviceList.Items[nItem].SubItems[LIST_STATUS_POS].Text = itemString;
        }

        //--------------------------------------------------------------------------------------------------------
        //Callback procedure.
        //--------------------------------------------------------------------------------------------------------
        public static int ConnectedProc(int handle, uint deviceID, int deviceType, int connectionType, int functionType, string ipAddress)
        {
            return MainForm.ConnetedProcMethod(handle, deviceID, deviceType, connectionType, functionType, ipAddress);
        }

        public int ConnetedProcMethod(int handle, uint deviceID, int deviceType, int connectionType, int functionType, string ipAddress)
        {
            if (this.InvokeRequired)
            {
                BSSDK.BS_ConnectionProc method = new BSSDK.BS_ConnectionProc(ConnetedProcMethod);
                return (int)Invoke(method, new object[] { handle, deviceID, deviceType, connectionType, functionType, ipAddress });
            }

            int i = 0;
            int itemCount = deviceList.Items.Count;

            for (i = 0; i < itemCount; i++)
            {
                if (System.Convert.ToDecimal(deviceList.Items[i].SubItems[LIST_ID_POS].Text) == deviceID
                 && System.Convert.ToDecimal(deviceList.Items[i].SubItems[LIST_HANDLE_POS].Text) == handle)
                {
                    break;
                }
            }

            string strID = String.Format("{0}", deviceID);
            string strIP = ipAddress;
            string strHandle = String.Format("{0}", handle);
            string strType = "UnKnown";
            string strConnect = "Normal";

            switch( deviceType )
            {
                case BSSDK.BS_DEVICE_FSTATION:
                    strType = String.Format("FaceStation");
                    break;
                case BSSDK.BS_DEVICE_BIOSTATION2:
                    strType = String.Format("BioStation T2");
                    break;
                case BSSDK.BS_DEVICE_DSTATION:
                    strType = String.Format("D-Station");
                    break;
                case BSSDK.BS_DEVICE_XSTATION:
                    strType = String.Format("X-Station");
                    break;
                case BSSDK.BS_DEVICE_BIOSTATION:    
                    strType = String.Format("BioStation");  
                    break;
                case BSSDK.BS_DEVICE_BIOENTRY_PLUS: 
                    strType = String.Format("BioEntry");    
                    break;
                case BSSDK.BS_DEVICE_BIOENTRY_W:
                    strType = String.Format("BioEntry W");
                    break;
                case BSSDK.BS_DEVICE_BIOLITE:       
                    strType = String.Format("BioLiteNet");  
                    break;
                case BSSDK.BS_DEVICE_XPASS:        
                    strType = String.Format("Xpass");       
                    break;
                case BSSDK.BS_DEVICE_XPASS_SLIM:
                    strType = String.Format("Xpass Slim");
                    break;
                case BSSDK.BS_DEVICE_XPASS_SLIM2:
                    strType = String.Format("Xpass S2");
                    break;
                default: 
                    strType = ""; 
                    break;
            }

            if (connectionType == 1) 
                strConnect = String.Format("SSL");
            else 
                strConnect = String.Format("Normal");

            string strStatus = String.Format("Connected...");

            if (i == itemCount)    // if not exist in list, then insert new one.
            {
                ListViewItem a = new ListViewItem(strID);
                a.SubItems.Add(strHandle);
                a.SubItems.Add(strIP); 
                a.SubItems.Add(strType);
                a.SubItems.Add(strConnect);
                a.SubItems.Add(strStatus);

                deviceList.Items.Add(a); 
                m_Count++;
            }
            else
            {
                deviceList.Items[i].SubItems[LIST_ID_POS].Text            = strID;
                deviceList.Items[i].SubItems[LIST_HANDLE_POS].Text        = strHandle;
                deviceList.Items[i].SubItems[LIST_IP_POS].Text            = strIP;
                deviceList.Items[i].SubItems[LIST_TYPE_POS].Text          = strType;
                deviceList.Items[i].SubItems[LIST_CONNECTION_POS].Text    = strConnect;
                deviceList.Items[i].SubItems[LIST_STATUS_POS].Text        = strStatus;
            }

            Text = String.Format("Connected : {0}", m_Count);

            workerObject.AddRequest(handle, deviceType, m_Port);   
            return BSSDK.BS_SUCCESS;
        }



        public static int RequestStartProc(int handle, uint deviceID, int deviceType, int connectionType, int functionType, string ipAddress)
        {
            return MainForm.RequestStartProcMethod(handle, deviceID, deviceType, connectionType, functionType, ipAddress);
        }

        public int RequestStartProcMethod(int handle, uint deviceID, int deviceType, int connectionType, int functionType, string ipAddress)
        {
            if (this.InvokeRequired)
            {
                BSSDK.BS_RequestStartProc method = new BSSDK.BS_RequestStartProc(RequestStartProcMethod);
                return (int)Invoke(method, new object[] { handle, deviceID, deviceType, connectionType, functionType, ipAddress });
            }


            string itemString;
            for (int i = 0; i < deviceList.Items.Count; i++)
            {
                if (System.Convert.ToDecimal(deviceList.Items[i].SubItems[LIST_ID_POS].Text) == deviceID
                 && System.Convert.ToDecimal(deviceList.Items[i].SubItems[LIST_HANDLE_POS].Text) == handle)
                {
                    itemString = "Request Started...";
                    deviceList.Items[i].SubItems[LIST_STATUS_POS].Text = itemString;
                    break;
                }
            }
            return BSSDK.BS_SUCCESS;
        }

        public static int DisconnectedProc(int handle, uint deviceID, int deviceType, int connectionType, int functionType, string ipAddress)
        {
            return MainForm.DisconnectedProcMethod(handle, deviceID, deviceType, connectionType, functionType, ipAddress);
        }
        public int DisconnectedProcMethod(int handle, uint deviceID, int deviceType, int connectionType, int functionType, string ipAddress)
        {
            if (this.InvokeRequired)
            {
                BSSDK.BS_DisconnectedProc method = new BSSDK.BS_DisconnectedProc(DisconnectedProcMethod);
                return (int)Invoke(method, new object[] { handle, deviceID, deviceType, connectionType, functionType, ipAddress });
            }

            for (int i = 0; i < deviceList.Items.Count; i++)
	        {
               if (System.Convert.ToDecimal(deviceList.Items[i].SubItems[LIST_ID_POS].Text) == deviceID
                 && System.Convert.ToDecimal(deviceList.Items[i].SubItems[LIST_HANDLE_POS].Text) == handle)
		        {
			        deviceList.Items.RemoveAt(i);
			        m_Count--;
			        break;
		        }
	        }

	        string itemString;
	        itemString = String.Format("Connected : {0}", m_Count );
	        this.Text = itemString;

            return BSSDK.BS_SUCCESS;
        }

        public static int LogProc(int handle, uint deviceID, int deviceType, int connectionType, IntPtr data) 
        {
            return MainForm.LogProcMethod(handle, deviceID, deviceType, connectionType, data);
        }

        public int LogProcMethod(int handle, uint deviceID, int deviceType, int connectionType, IntPtr data)
        {
            if (this.InvokeRequired)
            {
                BSSDK.BS_LogProc method = new BSSDK.BS_LogProc(LogProcMethod);
                return (int)Invoke(method, new object[] { handle, deviceID, deviceType, connectionType, data });
            }

            if (deviceType == BSSDK.BS_DEVICE_FSTATION || 
                deviceType == BSSDK.BS_DEVICE_BIOSTATION2 ||
                deviceType == BSSDK.BS_DEVICE_DSTATION || 
                deviceType == BSSDK.BS_DEVICE_XSTATION)
            {
                BSLogRecordEx logRecord = (BSLogRecordEx)Marshal.PtrToStructure(data, typeof(BSLogRecordEx));

                string itemString;
                for (int i = 0; i < deviceList.Items.Count; i++)
                {
                    string str1 = deviceList.Items[i].SubItems[LIST_ID_POS].Text;
                    string str2 = deviceList.Items[i].SubItems[LIST_HANDLE_POS].Text;

                    if (System.Convert.ToDecimal(str1) == deviceID && System.Convert.ToDecimal(str2) == handle)
                    {
                        itemString = String.Format("Log Event : 0x{0:x2}", logRecord.Event);
                        deviceList.Items[i].SubItems[LIST_STATUS_POS].Text = itemString;
                        break;
                    }
                }
            }
            else
            {
                BSLogRecord logRecord = (BSLogRecord)Marshal.PtrToStructure(data, typeof(BSLogRecord));

                string itemString;
                for (int i = 0; i < deviceList.Items.Count; i++)
                {
                    string str1 = deviceList.Items[i].SubItems[LIST_ID_POS].Text;
                    string str2 = deviceList.Items[i].SubItems[LIST_HANDLE_POS].Text;

                    if (System.Convert.ToDecimal(str1) == deviceID && System.Convert.ToDecimal(str2) == handle)
                    {
                        itemString = String.Format("Log Event : 0x{0:x2}", logRecord.Event);
                        deviceList.Items[i].SubItems[LIST_STATUS_POS].Text = itemString;
                        break;
                    }
                }
            }

            return BSSDK.BS_SUCCESS;
        }

        public static object RawDeserialize(byte[] rawData, int position, Type anyType)
        {
            int rawsize = Marshal.SizeOf(anyType);
            if (rawsize > rawData.Length)
                return null;

            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.Copy(rawData, position, buffer, rawsize);
            object retobj = Marshal.PtrToStructure(buffer, anyType);
            Marshal.FreeHGlobal(buffer);
            
            return retobj;
        }

        public static byte[] RawSerialize(object anything)
        {
            int rawSize = Marshal.SizeOf(anything);
            IntPtr buffer = Marshal.AllocHGlobal(rawSize);
            Marshal.StructureToPtr(anything, buffer, false);
            byte[] rawDatas = new byte[rawSize];
            Marshal.Copy(buffer, rawDatas, 0, rawSize);
            Marshal.FreeHGlobal(buffer);
            return rawDatas;
        }

        public static int ImageLogProc(int handle, uint deviceID, int deviceType, int connectionType, IntPtr data, int dataLen)
        {
            return MainForm.ImageLogProcMethod(handle, deviceID, deviceType, connectionType, data, dataLen);
        }

        public int ImageLogProcMethod(int handle, uint deviceID, int deviceType, int connectionType, IntPtr data, int dataLen)
        {
            if (deviceType == BSSDK.BS_DEVICE_FSTATION || 
                deviceType == BSSDK.BS_DEVICE_BIOSTATION2 ||
                deviceType == BSSDK.BS_DEVICE_DSTATION || 
                deviceType == BSSDK.BS_DEVICE_XSTATION)
            {
                try
                {
                    byte[] packet = new byte[dataLen];
                    Marshal.Copy(data, packet, 0, dataLen);

                    // copy header
                    IntPtr ptrHeader = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSImageLogHdr)));
                    Marshal.Copy(packet, 0, ptrHeader, Marshal.SizeOf(typeof(BSImageLogHdr)));
                    BSImageLogHdr ImageLogHdr = (BSImageLogHdr)Marshal.PtrToStructure(ptrHeader, typeof(BSImageLogHdr));

                    // copy image data
                    IntPtr ptrImageData = Marshal.AllocHGlobal((int)ImageLogHdr.imageSize);
                    Marshal.Copy(packet, Marshal.SizeOf(typeof(BSImageLogHdr)), ptrImageData, (int)ImageLogHdr.imageSize);

                    string filename = "c:\\temp\\";
                    filename += ImageLogHdr.deviceID.ToString() + "_" + ImageLogHdr.Event.ToString() + "_" + ImageLogHdr.eventTime.ToString() + ".jpg";

                    FileStream fs = new FileStream(filename, FileMode.CreateNew, FileAccess.Write, FileShare.None);

                    byte[] byteArray = new byte[ImageLogHdr.imageSize];
                    Marshal.Copy(ptrImageData, byteArray, 0, (int)ImageLogHdr.imageSize);

                    fs.Write(byteArray, 0, (int)ImageLogHdr.imageSize);
                    fs.Close();

                    Marshal.FreeHGlobal(ptrHeader);
                    Marshal.FreeHGlobal(ptrImageData);

                }
                catch(IOException e)
                {
                    e.ToString();
                }
            }

            return BSSDK.BS_SUCCESS;
        }

        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static int RequestMatchingProc(int handle, uint deviceID, int deviceType, int connectionType, int matchingType, uint ID, IntPtr templateData, IntPtr userHdr, ref int isDuress)
        {
            return MainForm.RequestMatchingProcMethod(handle, deviceID, deviceType, connectionType, matchingType, ID, templateData, userHdr, ref isDuress);
        }

        public int RequestMatchingProcMethod(int handle, uint deviceID, int deviceType, int connectionType, int matchingType, uint ID, IntPtr templateData, IntPtr userHdr, ref int isDuress)
        {
            if (this.InvokeRequired)
            {
                BSSDK.BS_RequestMatchingProc method = new BSSDK.BS_RequestMatchingProc(RequestMatchingProcMethod);
                return (int)Invoke(method, new object[] { handle, deviceID, deviceType, connectionType, matchingType, ID, templateData, userHdr, isDuress });
            } 

            //Control.CheckForIllegalCrossThreadCalls = false;

	        uint userID = 0;
	        string itemString;

            for (int i = 0; i < deviceList.Items.Count; i++)
	        {
                if (System.Convert.ToDecimal(deviceList.Items[i].SubItems[LIST_ID_POS].Text) == deviceID &&
                    System.Convert.ToDecimal(deviceList.Items[i].SubItems[LIST_HANDLE_POS].Text) == handle)
		        {
			        if( matchingType == BSSDK.REQUEST_IDENTIFY )
			        {
				        itemString = "1:N Matching";
				        userID = 100;
			        }
			        else
			        {
				        itemString = "1:1 Matching";
				        userID = ID;
			        }
        			
			        deviceList.Items[i].SubItems[LIST_STATUS_POS].Text = itemString;
			        break;
		        }
	        }

	        if (m_MatchingFail)
	        {
		        return BSSDK.BS_ERR_NOT_FOUND;
	        }

            if (deviceType == BSSDK.BS_DEVICE_BIOENTRY_PLUS || deviceType == BSSDK.BS_DEVICE_BIOENTRY_W)
            {
                BEUserHdr beUserHdr = (BEUserHdr)Marshal.PtrToStructure(userHdr, typeof(BEUserHdr));

                beUserHdr.version = 0;
                beUserHdr.userID = userID;
                beUserHdr.startTime = 0;
                beUserHdr.expiryTime = 0;
                beUserHdr.cardID = 0;

                beUserHdr.cardCustomID = 0;
                beUserHdr.commandCardFlag = 0;
                beUserHdr.cardFlag = 0;
                beUserHdr.cardVersion = BSSDK.BE_CARD_VERSION_1;

                beUserHdr.adminLevel = BSSDK.BE_USER_LEVEL_NORMAL;
                beUserHdr.securityLevel = BSSDK.BE_USER_SECURITY_DEFAULT;

                beUserHdr.accessGroupMask = 0xFFFFFFFF;

                beUserHdr.numOfFinger = 1;

            }
            else if( deviceType == BSSDK.BS_DEVICE_BIOLITE )
            {
                BEUserHdr beUserHdr = (BEUserHdr)Marshal.PtrToStructure(userHdr, typeof(BEUserHdr));
 
                beUserHdr.version = 0;
                beUserHdr.userID = userID;
                beUserHdr.startTime = 0;
                beUserHdr.expiryTime = 0;
                beUserHdr.cardID = 0;

                beUserHdr.cardCustomID = 0;
                beUserHdr.commandCardFlag = 0;
                beUserHdr.cardFlag = 0;
                beUserHdr.cardVersion = BSSDK.BE_CARD_VERSION_1;

                beUserHdr.adminLevel = BSSDK.BE_USER_LEVEL_NORMAL;
                beUserHdr.securityLevel = BSSDK.BE_USER_SECURITY_DEFAULT;

                beUserHdr.accessGroupMask = 0xFFFFFFFF;

                beUserHdr.numOfFinger = 1;

            }
            else if (deviceType == BSSDK.BS_DEVICE_BIOSTATION)
            {
                BSUserHdrEx bsUserHdr = (BSUserHdrEx)Marshal.PtrToStructure(userHdr, typeof(BSUserHdrEx));

                bsUserHdr.ID = userID;
                bsUserHdr.accessGroupMask = 0xffffffff; // no access group

                bsUserHdr.name = StrToByteArray("Test User");
                bsUserHdr.department = StrToByteArray("R&D");
                bsUserHdr.adminLevel = BSSDK.BS_USER_NORMAL;
                bsUserHdr.securityLevel = BSSDK.BS_USER_SECURITY_DEFAULT;
                bsUserHdr.duressMask = 0x00; // no duress finger
                bsUserHdr.numOfFinger = 1;
            }
            else if (deviceType == BSSDK.BS_DEVICE_DSTATION)
            {
                DSUserHdr bsUserHdr = (DSUserHdr)Marshal.PtrToStructure(userHdr, typeof(DSUserHdr));

                bsUserHdr.ID = ID;
                bsUserHdr.accessGroupMask = 0xffffffff; // no access group

                byte[] name = StrToByteArray("Test User");
                byte[] department = StrToByteArray("R&D");
                Buffer.BlockCopy(name, 0, bsUserHdr.name, 0, name.Length);
                Buffer.BlockCopy(department, 0, bsUserHdr.department, 0, department.Length);
                bsUserHdr.adminLevel = BSSDK.BS_USER_NORMAL;
                bsUserHdr.securityLevel = BSSDK.BS_USER_SECURITY_DEFAULT;
                bsUserHdr.numOfFinger = 1;
            }
            else if (deviceType == BSSDK.BS_DEVICE_XSTATION)
            {
                XSUserHdr bsUserHdr = (XSUserHdr)Marshal.PtrToStructure(userHdr, typeof(XSUserHdr));

                bsUserHdr.ID = ID;
                bsUserHdr.accessGroupMask = 0xffffffff; // no access group

                byte[] name = StrToByteArray("Test User");
                byte[] department = StrToByteArray("R&D");
                Buffer.BlockCopy(name, 0, bsUserHdr.name, 0, name.Length);
                Buffer.BlockCopy(department, 0, bsUserHdr.department, 0, department.Length);
                bsUserHdr.adminLevel = BSSDK.BS_USER_NORMAL;
                bsUserHdr.securityLevel = BSSDK.BS_USER_SECURITY_DEFAULT;
            }
            else if (deviceType == BSSDK.BS_DEVICE_BIOSTATION2)
            {

                BS2UserHdr bsUserHdr = (BS2UserHdr)Marshal.PtrToStructure(userHdr, typeof(BS2UserHdr));

                bsUserHdr.ID = ID;
                bsUserHdr.accessGroupMask = 0xffffffff; // no access group

                byte[] name = StrToByteArray("Test User");
                byte[] department = StrToByteArray("R&D");
                Buffer.BlockCopy(name, 0, bsUserHdr.name, 0, name.Length * sizeof(byte));
                Buffer.BlockCopy(department, 0, bsUserHdr.department, 0, department.Length * sizeof(byte));
                
                bsUserHdr.adminLevel = BSSDK.BS_USER_NORMAL;
                bsUserHdr.securityLevel = BSSDK.BS_USER_SECURITY_DEFAULT;
                bsUserHdr.numOfFinger = 1;
            }

            isDuress = BSSDK.NORMAL_FINGER;

            return BSSDK.BS_SUCCESS;
        }

        public static int RequestUserInfoProc(int handle, uint deviceID, int deviceType, int connectionType, int idType, uint ID, uint customID, IntPtr userHdr)
        {
            return MainForm.RequestUserInfoProcMethod(handle, deviceID, deviceType, connectionType, idType, ID, customID, userHdr);
    
        }
        public int RequestUserInfoProcMethod(int handle, uint deviceID, int deviceType, int connectionType, int idType, uint ID, uint customID, IntPtr userHdr)
        {
            if (this.InvokeRequired)
            {
                BSSDK.BS_RequestUserInfoProc method = new BSSDK.BS_RequestUserInfoProc(RequestUserInfoProcMethod);
                return (int)Invoke(method, new object[] { handle, deviceID, deviceType, connectionType, idType, ID, customID, userHdr });
            }

	        string itemString;
	        for( int i = 0; i < deviceList.Items.Count; i++ )
	        {
                if (System.Convert.ToDecimal(deviceList.Items[i].SubItems[LIST_ID_POS].Text) == deviceID &&
                    System.Convert.ToDecimal(deviceList.Items[i].SubItems[LIST_HANDLE_POS].Text) == handle)
		        {
                    if (idType == BSSDK.ID_USER)
				        itemString = "ID Request";
    		        else
	    		        itemString = "Card ID Request";
	        			
                    deviceList.Items[i].SubItems[LIST_STATUS_POS].Text = itemString;
			        break;
		        }
	        }

            if (m_MatchingFail)
            {
                return BSSDK.BS_ERR_NOT_FOUND;
            }

            if (deviceType == BSSDK.BS_DEVICE_BIOENTRY_PLUS || deviceType == BSSDK.BS_DEVICE_BIOENTRY_W)
	        {
                BEUserHdr beUserHdr = (BEUserHdr)Marshal.PtrToStructure(userHdr, typeof(BEUserHdr));

		        beUserHdr.version = 0;
		        beUserHdr.userID = ID;
		        beUserHdr.startTime = 0;
		        beUserHdr.expiryTime = 0;
		        beUserHdr.cardID = 0;

		        beUserHdr.cardCustomID = 0;
		        beUserHdr.commandCardFlag = 0;
		        beUserHdr.cardFlag = 0;
		        beUserHdr.cardVersion = BSSDK.BE_CARD_VERSION_1;

                beUserHdr.adminLevel = BSSDK.BE_USER_LEVEL_NORMAL;
                beUserHdr.securityLevel = BSSDK.BE_USER_SECURITY_DEFAULT;

		        beUserHdr.accessGroupMask = 0xFFFFFFFF;

		        beUserHdr.numOfFinger = 1;

	        }
            else if (deviceType == BSSDK.BS_DEVICE_XPASS || deviceType == BSSDK.BS_DEVICE_XPASS_SLIM || deviceType == BSSDK.BS_DEVICE_XPASS_SLIM2)
	        {
                BEUserHdr beUserHdr = (BEUserHdr)Marshal.PtrToStructure(userHdr, typeof(BEUserHdr));

		        beUserHdr.version = 0;
		        beUserHdr.userID = ID;
		        beUserHdr.startTime = 0;
		        beUserHdr.expiryTime = 0;
		        beUserHdr.cardID = 0;

		        beUserHdr.cardCustomID = 0;
		        beUserHdr.commandCardFlag = 0;
		        beUserHdr.cardFlag = 0;
		        beUserHdr.cardVersion = BSSDK.BE_CARD_VERSION_1;

                beUserHdr.adminLevel = BSSDK.BE_USER_LEVEL_NORMAL;
                beUserHdr.securityLevel = BSSDK.BE_USER_SECURITY_DEFAULT;

		        beUserHdr.accessGroupMask = 0xFFFFFFFF;

		        beUserHdr.numOfFinger = 1;

	        }
            else if( deviceType == BSSDK.BS_DEVICE_BIOLITE )
	        {
                BEUserHdr beUserHdr = (BEUserHdr)Marshal.PtrToStructure(userHdr, typeof(BEUserHdr));

		        beUserHdr.version = 0;
		        beUserHdr.userID = ID;
		        beUserHdr.startTime = 0;
		        beUserHdr.expiryTime = 0;
		        beUserHdr.cardID = 0;

		        beUserHdr.cardCustomID = 0;
		        beUserHdr.commandCardFlag = 0;
		        beUserHdr.cardFlag = 0;
                beUserHdr.cardVersion = BSSDK.BE_CARD_VERSION_1;

                beUserHdr.adminLevel = BSSDK.BE_USER_LEVEL_NORMAL;
                beUserHdr.securityLevel = BSSDK.BE_USER_SECURITY_DEFAULT;

		        beUserHdr.accessGroupMask = 0xFFFFFFFF;

		        beUserHdr.numOfFinger = 1;
	        }
            else if ( deviceType == BSSDK.BS_DEVICE_BIOSTATION )
	        {
                BSUserHdrEx bsUserHdr = (BSUserHdrEx)Marshal.PtrToStructure(userHdr, typeof(BSUserHdrEx));

		        bsUserHdr.ID = ID;
		        bsUserHdr.accessGroupMask = 0xffffffff; // no access group

                bsUserHdr.name = StrToByteArray("Test User");
                bsUserHdr.department = StrToByteArray("R&D");
                bsUserHdr.adminLevel = BSSDK.BS_USER_NORMAL;
                bsUserHdr.securityLevel = BSSDK.BS_USER_SECURITY_DEFAULT;
		        bsUserHdr.duressMask = 0x00; // no duress finger
		        bsUserHdr.numOfFinger = 1;
	        }
            else if (deviceType == BSSDK.BS_DEVICE_DSTATION)
            {
                DSUserHdr bsUserHdr = (DSUserHdr)Marshal.PtrToStructure(userHdr, typeof(DSUserHdr));

                bsUserHdr.ID = ID;
                bsUserHdr.accessGroupMask = 0xffffffff; // no access group

                byte[] name = StrToByteArray("Test User");
                byte[] department = StrToByteArray("R&D");
                Buffer.BlockCopy(name, 0, bsUserHdr.name, 0, name.Length);
                Buffer.BlockCopy(department, 0, bsUserHdr.department, 0, department.Length);
                bsUserHdr.adminLevel = BSSDK.BS_USER_NORMAL;
                bsUserHdr.securityLevel = BSSDK.BS_USER_SECURITY_DEFAULT;
                bsUserHdr.numOfFinger = 1;
            }
            else if (deviceType == BSSDK.BS_DEVICE_XSTATION)
            {
                XSUserHdr bsUserHdr = (XSUserHdr)Marshal.PtrToStructure(userHdr, typeof(XSUserHdr));

                bsUserHdr.ID = ID;
                bsUserHdr.accessGroupMask = 0xffffffff; // no access group

                byte[] name = StrToByteArray("Test User");
                byte[] department = StrToByteArray("R&D");
                Buffer.BlockCopy(name, 0, bsUserHdr.name, 0, name.Length);
                Buffer.BlockCopy(department, 0, bsUserHdr.department, 0, department.Length);
                bsUserHdr.adminLevel = BSSDK.BS_USER_NORMAL;
                bsUserHdr.securityLevel = BSSDK.BS_USER_SECURITY_DEFAULT;
            }
            else if (deviceType == BSSDK.BS_DEVICE_BIOSTATION2)
            {
                BS2UserHdr bsUserHdr = (BS2UserHdr)Marshal.PtrToStructure(userHdr, typeof(BS2UserHdr));

                bsUserHdr.ID = ID;
                bsUserHdr.accessGroupMask = 0xffffffff; // no access group

                byte[] name = StrToByteArray("Test User");
                byte[] department = StrToByteArray("R&D");
                Buffer.BlockCopy(name, 0, bsUserHdr.name, 0, name.Length);
                Buffer.BlockCopy(department, 0, bsUserHdr.department, 0, department.Length);

                bsUserHdr.adminLevel = BSSDK.BS_USER_NORMAL;
                bsUserHdr.securityLevel = BSSDK.BS_USER_SECURITY_DEFAULT;
                bsUserHdr.numOfFinger = 1;

            }

            return BSSDK.BS_SUCCESS;
        }
    }
}