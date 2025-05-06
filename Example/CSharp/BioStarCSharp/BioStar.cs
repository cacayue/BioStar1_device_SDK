using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace BioStarCSharp
{
    public partial class BioStar : Form
    {
        public const int MAX_DEVICE = 128;

        private int m_Handle = -1;

        private int m_NumOfDevice = 0;
        private uint[] m_DeviceID;
        private int[] m_DeviceType;
        private uint[] m_DeviceAddr;

        private int m_NumOfConnectedDevice = 0;
        private int[] m_ConnectedDeviceHandle;
        private uint[] m_ConnectedDeviceID;
        private int[] m_ConnectedDeviceType;
        private uint[] m_ConnectedDeviceAddr;

        public BioStar()
        {
            InitializeComponent();

            m_DeviceID = new uint[MAX_DEVICE];
            m_DeviceType = new int[MAX_DEVICE];
            m_DeviceAddr = new uint[MAX_DEVICE];

            m_ConnectedDeviceHandle = new int[MAX_DEVICE];
            m_ConnectedDeviceID = new uint[MAX_DEVICE];
            m_ConnectedDeviceType = new int[MAX_DEVICE];
            m_ConnectedDeviceAddr = new uint[MAX_DEVICE];
        }

        private void BioStar_Load(object sender, EventArgs e)
        {
            int result = BSSDK.BS_InitSDK();

            if (result != BSSDK.BS_SUCCESS)
            {
                MessageBox.Show("Cannot initialize the SDK", "Error");
                return;
            }

            result = BSSDK.BS_OpenInternalUDP(ref m_Handle);

            if (result != BSSDK.BS_SUCCESS)
            {
                MessageBox.Show("Cannot open internal UDP socket", "Error");
                return;
            }
        }

        private void BioStar_FormClosed(object sender, FormClosedEventArgs e)
        {
            BSSDK.BS_UnInitSDK();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            deviceList.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            int result;

            try
            {
                result = BSSDK.BS_SearchDeviceInLAN(m_Handle, ref m_NumOfDevice, m_DeviceID, m_DeviceType, m_DeviceAddr);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            if (result != BSSDK.BS_SUCCESS)
            {
                MessageBox.Show("Cannot find any device", "Error");
                return;
            }

            for (int i = 0; i < m_NumOfDevice; i++)
            {
                string device = "";

                if (m_DeviceType[i] == BSSDK.BS_DEVICE_BIOSTATION)
                {
                    device += "BioStation ";
                }
                else if (m_DeviceType[i] == BSSDK.BS_DEVICE_DSTATION)
                {
                    device += "D-Station ";
                }
                else if (m_DeviceType[i] == BSSDK.BS_DEVICE_XSTATION)
                {
                    device += "X-Station ";
                }
                else if (m_DeviceType[i] == BSSDK.BS_DEVICE_BIOSTATION2)
                {
                    device += "BioStation T2 ";
                }
                else if (m_DeviceType[i] == BSSDK.BS_DEVICE_FSTATION)
                {
                    device += "FaceStation ";
                }
                else if (m_DeviceType[i] == BSSDK.BS_DEVICE_BIOENTRY_PLUS)
                {
                    device += "BioEntry Plus ";
                }
                else if (m_DeviceType[i] == BSSDK.BS_DEVICE_BIOENTRY_W)
                {
                    device += "BioEntry W ";
                }
                else if (m_DeviceType[i] == BSSDK.BS_DEVICE_BIOLITE)
                {
                    device += "BioLite Net ";
                }
                else if (m_DeviceType[i] == BSSDK.BS_DEVICE_XPASS)
                {
                    device += "Xpass ";
                }
                else if (m_DeviceType[i] == BSSDK.BS_DEVICE_XPASS_SLIM)
                {
                    device += "Xpass Slim";
                }
                else if (m_DeviceType[i] == BSSDK.BS_DEVICE_XPASS_SLIM2)
                {
                    device += "Xpass S2";
                }
				else
				{
					device += "Unknown ";
				}

                device += (m_DeviceAddr[i] & 0xff) + ".";
                device += ((m_DeviceAddr[i] >> 8) & 0xff) + ".";
                device += ((m_DeviceAddr[i] >> 16) & 0xff) + ".";
                device += ((m_DeviceAddr[i] >> 24) & 0xff);

                device += "(" + m_DeviceID[i] + ")";

                deviceList.Items.Add(device);
            }

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Dispose();
            BSSDK.BS_UnInitSDK();
        }

        private void networkConfigButton_Click(object sender, EventArgs e)
        {
            if (deviceList.SelectedIndex < 0)
            {
                MessageBox.Show("Select a device first", "Error");
                return;
            }

            NetworkConfig networkConfig = new NetworkConfig();

            networkConfig.SetDevice(m_Handle, m_DeviceID[deviceList.SelectedIndex], m_DeviceAddr[deviceList.SelectedIndex], m_DeviceType[deviceList.SelectedIndex], this);

            networkConfig.Show();
        }


        public void AddConnectedDevice(uint deviceID, int deviceType, uint deviceAddr, int deviceHandle)
        {
            string device = "";

            if (deviceType == BSSDK.BS_DEVICE_BIOSTATION)
            {
                device += "BioStation ";
            }
            else if (deviceType == BSSDK.BS_DEVICE_DSTATION)
            {
                device += "D-Station ";
            }
            else if (deviceType == BSSDK.BS_DEVICE_XSTATION)
            {
                device += "X-Station ";
            }
            else if (deviceType == BSSDK.BS_DEVICE_BIOSTATION2)
            {
                device += "BioStation T2 ";
            }
            else if (deviceType == BSSDK.BS_DEVICE_FSTATION)
            {
                device += "FaceStation ";
            }
            else if (deviceType == BSSDK.BS_DEVICE_BIOENTRY_PLUS)
            {
                device += "BioEntry Plus ";
            }
            else if (deviceType == BSSDK.BS_DEVICE_BIOENTRY_W)
            {
                device += "BioEntry W ";
            }
            else if (deviceType == BSSDK.BS_DEVICE_XPASS)
            {
                device += "Xpass ";
            }
            else if (deviceType == BSSDK.BS_DEVICE_XPASS_SLIM)
            {
                device += "Xpass Slim";
            }
            else if (deviceType == BSSDK.BS_DEVICE_XPASS_SLIM2)
            {
                device += "Xpass S2";
            }
            else if (deviceType == BSSDK.BS_DEVICE_BIOLITE)
            {
                device += "BioLite Net ";
            }
            else
            {
                device += "Unknown ";
            }

            device += (deviceAddr & 0xff) + ".";
            device += ((deviceAddr >> 8) & 0xff) + ".";
            device += ((deviceAddr >> 16) & 0xff) + ".";
            device += ((deviceAddr >> 24) & 0xff);
            device += "(" + deviceID + ")";

            for (int i = 0; i < m_NumOfConnectedDevice; i++)
            {
                if (m_ConnectedDeviceID[i] == deviceID)
                {
                    m_ConnectedDeviceType[i] = deviceType;
                    m_ConnectedDeviceAddr[i] = deviceAddr;
                    m_ConnectedDeviceHandle[i] = deviceHandle;

                    connectedDeviceList.Items.RemoveAt(i);
                    connectedDeviceList.Items.Insert(i, device);

                    return;
                }
            }

            m_ConnectedDeviceID[m_NumOfConnectedDevice] = deviceID;
            m_ConnectedDeviceType[m_NumOfConnectedDevice] = deviceType;
            m_ConnectedDeviceAddr[m_NumOfConnectedDevice] = deviceAddr;
            m_ConnectedDeviceHandle[m_NumOfConnectedDevice++] = deviceHandle;

            connectedDeviceList.Items.Add(device);
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < m_NumOfConnectedDevice; i++)
            {
                BSSDK.BS_CloseSocket(m_ConnectedDeviceHandle[i]);
            }

            m_NumOfConnectedDevice = 0;

            connectedDeviceList.Items.Clear();
        }

        private void timeTest_Click(object sender, EventArgs e)
        {
            if (connectedDeviceList.SelectedIndex < 0)
            {
                MessageBox.Show("Select a connected device first", "Error");
                return;
            }

            BSSDK.BS_SetDeviceID(m_ConnectedDeviceHandle[connectedDeviceList.SelectedIndex],
                m_ConnectedDeviceID[connectedDeviceList.SelectedIndex], 1);

            SetupTime setupTime = new SetupTime();

            setupTime.setHandle(m_ConnectedDeviceHandle[connectedDeviceList.SelectedIndex]);
            setupTime.setDeviceType(m_ConnectedDeviceType[connectedDeviceList.SelectedIndex]);

            setupTime.Show();
        }

        private void logTest_Click(object sender, EventArgs e)
        {
            if (connectedDeviceList.SelectedIndex < 0)
            {
                MessageBox.Show("Select a connected device first", "Error");
                return;
            }

            BSSDK.BS_SetDeviceID(m_ConnectedDeviceHandle[connectedDeviceList.SelectedIndex],
                m_ConnectedDeviceID[connectedDeviceList.SelectedIndex], 1);

            LogManagement logTest = new LogManagement();

            logTest.SetDevice(m_ConnectedDeviceHandle[connectedDeviceList.SelectedIndex],
                m_ConnectedDeviceID[connectedDeviceList.SelectedIndex], m_ConnectedDeviceType[connectedDeviceList.SelectedIndex]);

            logTest.Show();
        }

        private float GetFirmwareVersion(int handle)
        {
            BSSDK.BSSysInfoConfig sysCofnig = new BSSDK.BSSysInfoConfig();
            IntPtr sysInfo = Marshal.AllocHGlobal(Marshal.SizeOf(sysCofnig));

            BSSDK.BS_ReadSysInfoConfig(handle, sysInfo);

            BSSDK.BSSysInfoConfig SysConfigInfo = (BSSDK.BSSysInfoConfig)Marshal.PtrToStructure(sysInfo, typeof(BSSDK.BSSysInfoConfig));
            string strData = System.Text.Encoding.Default.GetString(SysConfigInfo.firmwareVer);
            string strVersion = "";
            bool bSuccess = false;
            for (int i = 0; i < strData.Length; i++)
            {
                char c = strData[i];
                if (c == '_')
                {
                    bSuccess = true;
                    break;
                }

                if (Char.IsDigit(c) || c == '.')
                {
                    strVersion += c;
                }
            }

            float fVersion = 1.1f;
            if (bSuccess)
            {
                fVersion = Convert.ToSingle(strVersion);
            }

            return fVersion;
        }

        private void userTest_Click(object sender, EventArgs e)
        {
            if (connectedDeviceList.SelectedIndex < 0)
            {
                MessageBox.Show("Select a connected device first", "Error");
                return;
            }

            BSSDK.BS_SetDeviceID(m_ConnectedDeviceHandle[connectedDeviceList.SelectedIndex],
                m_ConnectedDeviceID[connectedDeviceList.SelectedIndex], 1);


 //         BSSDK.BSOPModeConfig OPModeCongif = new BSSDK.BSOPModeConfig();
 //         IntPtr OpModeInfo = Marshal.AllocHGlobal(Marshal.SizeOf(OPModeCongif));
 //         int result = BSSDK.BS_ReadOPModeConfig(m_ConnectedDeviceHandle[connectedDeviceList.SelectedIndex], OpModeInfo);

            //check FaceStation's firmware version
            switch (m_ConnectedDeviceType[connectedDeviceList.SelectedIndex])
            {
                case BSSDK.BS_DEVICE_FSTATION:
                    {
                        float fVersion = GetFirmwareVersion(m_ConnectedDeviceHandle[connectedDeviceList.SelectedIndex]);
                        if (fVersion >= 1.2f)
                        {
                            UserManagement_FST userTest = new UserManagement_FST();
                            userTest.SetDevice(m_ConnectedDeviceHandle[connectedDeviceList.SelectedIndex],
                                                m_ConnectedDeviceID[connectedDeviceList.SelectedIndex],
                                                m_ConnectedDeviceType[connectedDeviceList.SelectedIndex]);
                            userTest.Show();
                        }
                        else
                        {
                            UserManagement userTest = new UserManagement();
                            userTest.SetDevice(m_ConnectedDeviceHandle[connectedDeviceList.SelectedIndex],
                                                m_ConnectedDeviceID[connectedDeviceList.SelectedIndex],
                                                m_ConnectedDeviceType[connectedDeviceList.SelectedIndex]);
                            userTest.Show();
                        }
                    }
                    break;
                default:
                    {
                        UserManagement userTest = new UserManagement();
                        userTest.SetDevice(m_ConnectedDeviceHandle[connectedDeviceList.SelectedIndex],
                                            m_ConnectedDeviceID[connectedDeviceList.SelectedIndex],
                                            m_ConnectedDeviceType[connectedDeviceList.SelectedIndex]);
                        userTest.Show();
                    }
                    break;
            }
        }

        private void LedSet_Click(object sender, EventArgs e)
        {
            if (connectedDeviceList.SelectedIndex < 0)
            {
                MessageBox.Show("Select a connected device first", "Error");
                return;
            }

            int nDeviceType = m_ConnectedDeviceType[connectedDeviceList.SelectedIndex];
            if (nDeviceType != BSSDK.BS_DEVICE_BIOENTRY_PLUS &&
                nDeviceType != BSSDK.BS_DEVICE_BIOENTRY_W    && 
                nDeviceType != BSSDK.BS_DEVICE_XPASS         &&
                nDeviceType != BSSDK.BS_DEVICE_XPASS_SLIM    &&
                nDeviceType != BSSDK.BS_DEVICE_XPASS_SLIM2)
                return;

            if (nDeviceType == BSSDK.BS_DEVICE_BIOENTRY_PLUS ||
                nDeviceType == BSSDK.BS_DEVICE_BIOENTRY_W    || 
                nDeviceType == BSSDK.BS_DEVICE_XPASS         ||
                nDeviceType == BSSDK.BS_DEVICE_XPASS_SLIM    ||
                nDeviceType == BSSDK.BS_DEVICE_XPASS_SLIM2)
            {
                int handle = m_ConnectedDeviceHandle[connectedDeviceList.SelectedIndex];
                uint deviceId = m_ConnectedDeviceID[connectedDeviceList.SelectedIndex];

                int result = BSSDK.BS_SetDeviceID(handle, deviceId, 1);

                IntPtr data = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BEConfigData)));
                int configSize = 0;
                result = BSSDK.BS_ReadConfig(handle, BSSDK.BEPLUS_CONFIG, ref configSize, data);
                if (result != BSSDK.BS_SUCCESS)
                {
                    Cursor.Current = Cursors.Default;
                    Marshal.FreeHGlobal(data);
                    MessageBox.Show("Cannot get the configData", "Error");
                    return;
                }

                BSSDK.BEConfigData configBEPlus = (BSSDK.BEConfigData)Marshal.PtrToStructure(data, typeof(BSSDK.BEConfigData));

                int i = 17;

                configBEPlus.ledBuzzerConfig.ledPattern[i].repeat  = 0;
                configBEPlus.ledBuzzerConfig.ledPattern[i].arg[0]  = 4;
                configBEPlus.ledBuzzerConfig.ledPattern[i].arg[1]  = 0;
                configBEPlus.ledBuzzerConfig.ledPattern[i].arg[2]  = 0;
                configBEPlus.ledBuzzerConfig.ledPattern[i].high[0] = -1;
                configBEPlus.ledBuzzerConfig.ledPattern[i].high[1] = -1;
                configBEPlus.ledBuzzerConfig.ledPattern[i].high[2] = -1;
                configBEPlus.ledBuzzerConfig.ledPattern[i].low[0]  = -1;
                configBEPlus.ledBuzzerConfig.ledPattern[i].low[1]  = 1000;
                configBEPlus.ledBuzzerConfig.ledPattern[i].low[2]  = -1;

    /*
                configBEPlus.ledBuzzerConfig.buzzerPattern[i].repeat = 1;
                configBEPlus.ledBuzzerConfig.buzzerPattern[i].arg[0] = 3136;
                configBEPlus.ledBuzzerConfig.buzzerPattern[i].arg[1] = 3136;
                configBEPlus.ledBuzzerConfig.buzzerPattern[i].arg[2] = 3136;
                configBEPlus.ledBuzzerConfig.buzzerPattern[i].high[0] = 100;
                configBEPlus.ledBuzzerConfig.buzzerPattern[i].high[1] = 100;
                configBEPlus.ledBuzzerConfig.buzzerPattern[i].high[2] = 100;
                configBEPlus.ledBuzzerConfig.buzzerPattern[i].low[0] = 20;
                configBEPlus.ledBuzzerConfig.buzzerPattern[i].low[1] = 20;
                configBEPlus.ledBuzzerConfig.buzzerPattern[i].low[2] = 0;
    */
                Marshal.StructureToPtr(configBEPlus, data, true);

                configSize = Marshal.SizeOf(typeof(BSSDK.BEConfigData));
                result = BSSDK.BS_WriteConfig(handle, BSSDK.BEPLUS_CONFIG, configSize, data);
                if (result != BSSDK.BS_SUCCESS)
                {
                    MessageBox.Show("Cannot write the config", "Error");
                }

                Marshal.FreeHGlobal(data);
            }

        }

        private void SizeOfStructure_Click(object sender, EventArgs e)
        {
            int size = 0;
            String str = "\r\n";
            String temp = "";

            /*
                size = Marshal.SizeOf(typeof(BEConfigDataBLN)); temp = String.Format("BEConfigDataBLN : {0:C}\n", size); str += temp;

                size = Marshal.SizeOf(typeof(BSSDK.BSInputConfig)); temp = String.Format("BSInputConfig : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BSOutputConfig)); temp = String.Format("BSOutputConfig : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BSDoorConfig)); temp = String.Format("BSDoorConfig : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BECommandCard)); temp = String.Format("BECommandCard : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BSWiegandConfig)); temp = String.Format("BSWiegandConfig : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BELEDBuzzerConfigBLN)); temp = String.Format("BELEDBuzzerConfigBLN : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BETnaEventConfig)); temp = String.Format("BETnaEventConfig : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BETnaEventExConfig)); temp = String.Format("BETnaEventExConfig : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BSInputFunction)); temp = String.Format("BSInputFunction : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BSDoor)); temp = String.Format("BSDoor : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BSWiegandFormatHeader)); temp = String.Format("BSWiegandFormatHeader : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BSWiegandFormatData)); temp = String.Format("BSWiegandFormatData : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BEOutputPatternBLN)); temp = String.Format("BEOutputPatternBLN : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BS_WIEGAND_FORMAT)); temp = String.Format("BS_WIEGAND_FORMAT : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BSWiegandPassThruData)); temp = String.Format("BSWiegandPassThruData : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BSWiegandCustomData)); temp = String.Format("BSWiegandCustomData : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BSWiegandField)); temp = String.Format("BSWiegandField : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BSWiegandParity)); temp = String.Format("BSWiegandParity : {0:C}\n", size); str += temp;
                size = Marshal.SizeOf(typeof(BSSDK.BS_WIEGAND_PARITY_TYPE)); temp = String.Format("BS_WIEGAND_PARITY_TYPE : {0:C}\n", size); str += temp;
            */

            size = Marshal.SizeOf(typeof(BSSDK.BEConfigData)); temp = String.Format("BEConfigData : {0:C}\n", size); str += temp;
            //size = Marshal.SizeOf(typeof(BSSDK.BEConfigDataBLN)); temp = String.Format("BEConfigDataBLN : {0:C}\n", size); str += temp;

            size = Marshal.SizeOf(typeof(BSSDK.BETnaEventConfig)); temp = String.Format("BETnaEventConfig : {0:C}\n", size); str += temp;
            size = Marshal.SizeOf(typeof(BSSDK.BETnaEventExConfig)); temp = String.Format("BETnaEventExConfig : {0:C}\n", size); str += temp;

            size = Marshal.SizeOf(typeof(BSSDK.BSInputConfig)); temp = String.Format("BSInputConfig : {0:C}\n", size); str += temp;
            size = Marshal.SizeOf(typeof(BSSDK.BSOutputConfig)); temp = String.Format("BSOutputConfig : {0:C}\n", size); str += temp;
            size = Marshal.SizeOf(typeof(BSSDK.BSDoorConfig)); temp = String.Format("BSDoorConfig : {0:C}\n", size); str += temp;
            size = Marshal.SizeOf(typeof(BSSDK.BECommandCard)); temp = String.Format("BECommandCard : {0:C}\n", size); str += temp;
            size = Marshal.SizeOf(typeof(BSSDK.BSWiegandConfig)); temp = String.Format("BSWiegandConfig : {0:C}\n", size); str += temp;
            size = Marshal.SizeOf(typeof(BSSDK.BELEDBuzzerConfig)); temp = String.Format("BELEDBuzzerConfig : {0:C}\n", size); str += temp;
            size = Marshal.SizeOf(typeof(BSSDK.BSInputFunction)); temp = String.Format("BSInputFunction : {0:C}\n", size); str += temp;
            size = Marshal.SizeOf(typeof(BSSDK.BSDoor)); temp = String.Format("BSDoor : {0:C}\n", size); str += temp;
            size = Marshal.SizeOf(typeof(BSSDK.BSWiegandFormatHeader)); temp = String.Format("BSWiegandFormatHeader : {0:C}\n", size); str += temp;
            size = Marshal.SizeOf(typeof(BSSDK.BSWiegandFormatData)); temp = String.Format("BSWiegandFormatData : {0:C}\n", size); str += temp;
            size = Marshal.SizeOf(typeof(BSSDK.BEOutputPattern)); temp = String.Format("BEOutputPattern : {0:C}\n", size); str += temp;
            //size = Marshal.SizeOf(typeof(BSSDK.BS_WIEGAND_FORMAT)); temp = String.Format("BS_WIEGAND_FORMAT : {0:C}\n", size); str += temp;
            size = Marshal.SizeOf(typeof(BSSDK.BSWiegandPassThruData)); temp = String.Format("BSWiegandPassThruData : {0:C}\n", size); str += temp;
            size = Marshal.SizeOf(typeof(BSSDK.BSWiegandCustomData)); temp = String.Format("BSWiegandCustomData : {0:C}\n", size); str += temp;
            size = Marshal.SizeOf(typeof(BSSDK.BSWiegandField)); temp = String.Format("BSWiegandField : {0:C}\n", size); str += temp;
            size = Marshal.SizeOf(typeof(BSSDK.BSWiegandParity)); temp = String.Format("BSWiegandParity : {0:C}\n", size); str += temp;
            //size = Marshal.SizeOf(typeof(BSSDK.BS_WIEGAND_PARITY_TYPE)); temp = String.Format("BS_WIEGAND_PARITY_TYPE : {0:C}\n", size); str += temp;


            MessageBox.Show(str, "config size");
        }

        private void SetConfig_Click(object sender, EventArgs e)
        {
            if (connectedDeviceList.SelectedIndex < 0)
            {
                MessageBox.Show("Select a connected device first", "Error");
                return;
            }

            int nDeviceType = m_ConnectedDeviceType[connectedDeviceList.SelectedIndex];
            if (nDeviceType != BSSDK.BS_DEVICE_BIOENTRY_PLUS &&
                nDeviceType != BSSDK.BS_DEVICE_BIOENTRY_W    && 
                nDeviceType != BSSDK.BS_DEVICE_XPASS         &&
                nDeviceType != BSSDK.BS_DEVICE_XPASS_SLIM    &&
                nDeviceType != BSSDK.BS_DEVICE_XPASS_SLIM2)
                return;

            if (nDeviceType == BSSDK.BS_DEVICE_BIOENTRY_PLUS ||
                nDeviceType == BSSDK.BS_DEVICE_BIOENTRY_W    || 
                nDeviceType == BSSDK.BS_DEVICE_XPASS         ||
                nDeviceType == BSSDK.BS_DEVICE_XPASS_SLIM    ||
                nDeviceType == BSSDK.BS_DEVICE_XPASS_SLIM2)
            {
                int handle = m_ConnectedDeviceHandle[connectedDeviceList.SelectedIndex];
                uint deviceId = m_ConnectedDeviceID[connectedDeviceList.SelectedIndex];

                int result = BSSDK.BS_SetDeviceID(handle, deviceId, 1);

                IntPtr data = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BEConfigData)));
                int configSize = 0;
                result = BSSDK.BS_ReadConfig(handle, BSSDK.BEPLUS_CONFIG, ref configSize, data);
                if (result != BSSDK.BS_SUCCESS)
                {
                    Cursor.Current = Cursors.Default;
                    Marshal.FreeHGlobal(data);
                    MessageBox.Show("Cannot get the configData", "Error");
                    return;
                }

                BSSDK.BEConfigData configBEPlus = (BSSDK.BEConfigData)Marshal.PtrToStructure(data, typeof(BSSDK.BEConfigData));

                configBEPlus.maxEntry[0] = 1;
                configBEPlus.maxEntry[1] = 2;
                configBEPlus.maxEntry[2] = 3;
                configBEPlus.maxEntry[3] = 4;

                Marshal.StructureToPtr(configBEPlus, data, true);

                configSize = Marshal.SizeOf(typeof(BSSDK.BEConfigData));
                result = BSSDK.BS_WriteConfig(handle, BSSDK.BEPLUS_CONFIG, configSize, data);
                if (result != BSSDK.BS_SUCCESS)
                {
                    MessageBox.Show("Cannot write the config", "Error");
                }

                Marshal.FreeHGlobal(data);
            }
        }

        private void AccessGroup_Click(object sender, EventArgs e)
        {
            if (connectedDeviceList.SelectedIndex < 0)
            {
                MessageBox.Show("Select a connected device first", "Error");
                return;
            }

            BSSDK.BS_SetDeviceID(m_ConnectedDeviceHandle[connectedDeviceList.SelectedIndex],
                m_ConnectedDeviceID[connectedDeviceList.SelectedIndex], 1);

            AccessGroup accessGroupTest = new AccessGroup();

            accessGroupTest.SetDevice(m_ConnectedDeviceHandle[connectedDeviceList.SelectedIndex],
                m_ConnectedDeviceID[connectedDeviceList.SelectedIndex], m_ConnectedDeviceType[connectedDeviceList.SelectedIndex]);

            accessGroupTest.Show();
        }
    }
}