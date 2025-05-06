using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace BioStarCSharp
{
    public partial class AccessGroup : Form
    {
        private int m_Handle = 0;
        private uint m_DeviceID = 0;
        private int m_DeviceType = -1;

        public void SetDevice(int handle, uint deviceID, int deviceType)
        {
            m_Handle = handle;
            m_DeviceID = deviceID;
            m_DeviceType = deviceType;
        }

        public AccessGroup()
        {
            InitializeComponent();
        }

        private void AccessGroup_Load(object sender, EventArgs e)
        {
            switch (m_DeviceType)
            {
                case BSSDK.BS_DEVICE_BIOENTRY_PLUS:
                    deviceInfo.Text = "BioEntry Plus " + m_DeviceID.ToString();
                    break;
                case BSSDK.BS_DEVICE_BIOENTRY_W:
                    deviceInfo.Text = "BioEntry W " + m_DeviceID.ToString();
                    break;
                case BSSDK.BS_DEVICE_BIOLITE:
                    deviceInfo.Text = "BioLite Net " + m_DeviceID.ToString();
                    break;
                case BSSDK.BS_DEVICE_XPASS:
                    deviceInfo.Text = "Xpass " + m_DeviceID.ToString();
                    break;
                case BSSDK.BS_DEVICE_BIOSTATION:
                    deviceInfo.Text = "BioStation " + m_DeviceID.ToString();
                    break;
                case BSSDK.BS_DEVICE_DSTATION:
                    deviceInfo.Text = "D-Station " + m_DeviceID.ToString();
                    break;
                case BSSDK.BS_DEVICE_XSTATION:
                    deviceInfo.Text = "X-Station " + m_DeviceID.ToString();
                    break;
                case BSSDK.BS_DEVICE_BIOSTATION2:
                    deviceInfo.Text = "BioStation T2 " + m_DeviceID.ToString();
                    break;
                case BSSDK.BS_DEVICE_FSTATION:
                    deviceInfo.Text = "FaceStation " + m_DeviceID.ToString();
                    break;
                case BSSDK.BS_DEVICE_XPASS_SLIM:
                    deviceInfo.Text = "Xpass Slim" + m_DeviceID.ToString();
                    break;
                case BSSDK.BS_DEVICE_XPASS_SLIM2:
                    deviceInfo.Text = "Xpass S2" + m_DeviceID.ToString();
                    break;
                default:
                    deviceInfo.Text = "Unknown " + m_DeviceID.ToString();
                    break;
            }

            Cursor.Current = Cursors.WaitCursor;

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void ReadDoorConfig_Click(object sender, EventArgs e)
        {
            IntPtr DoorConfig = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BSDoorConfig)));
            int result = BSSDK.BS_ReadDoorConfig(m_Handle, DoorConfig);
            if (result != BSSDK.BS_SUCCESS)
            {
                Cursor.Current = Cursors.Default;
                Marshal.FreeHGlobal(DoorConfig);
                MessageBox.Show("Cannot get the configData", "Error");
                return;
            }

            Marshal.FreeHGlobal(DoorConfig);
        }

        private void WriteDoorConfig_Click(object sender, EventArgs e)
        {
            switch (m_DeviceType)
            {
                case BSSDK.BS_DEVICE_DSTATION:
                case BSSDK.BS_DEVICE_FSTATION:
                case BSSDK.BS_DEVICE_BIOSTATION2:
                case BSSDK.BS_DEVICE_XSTATION:
                case BSSDK.BS_DEVICE_BIOSTATION:    
                    {
                        IntPtr data = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BSDoorConfig)));

                        int result = BSSDK.BS_ReadDoorConfig(m_Handle, data);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Cursor.Current = Cursors.Default;
                            Marshal.FreeHGlobal(data);
                            MessageBox.Show("Cannot get the configData", "Error");
                            return;
                        }

                        BSSDK.BSDoorConfig doorConfig = (BSSDK.BSDoorConfig)Marshal.PtrToStructure(data, typeof(BSSDK.BSDoorConfig));
                        
                        doorConfig.door[0].relay = (int)BSSDK.BSDoor.BSDOOR.HOST_RELAY0;
                        doorConfig.door[0].useRTE = 0;
                        doorConfig.door[0].useDoorSensor = 0;
                        doorConfig.door[0].openEvent = (int)BSSDK.BSDoor.BSDOOR.ALL_EVENT;
                        doorConfig.door[0].openTime = 3;
                        doorConfig.door[0].heldOpenTime = 0;
                        doorConfig.door[0].forcedOpenSchedule = 253;
                        doorConfig.door[0].forcedCloseSchedule = 253;
                        doorConfig.door[0].RTEType = 0;
                        doorConfig.door[0].sensorType = 0;
                        doorConfig.door[0].reader[0] = (short)BSSDK.BSDoor.BSDOOR.HOST_READER;
                        doorConfig.door[0].reader[1] = (short)BSSDK.BSDoor.BSDOOR.HOST_READER;
                        doorConfig.door[0].useRTEEx = 1;
                        doorConfig.door[0].useSoundForcedOpen = 0;
                        doorConfig.door[0].useSoundHeldOpen = 0;
                        doorConfig.door[0].openOnce = 0;
                        doorConfig.door[0].RTE = 512;
                        doorConfig.door[0].useDoorSensorEx = 1;
                        doorConfig.door[0].alarmStatus = 0;
                        doorConfig.door[0].reserved2[0] = 0;
                        doorConfig.door[0].reserved2[1] = 0;
                        doorConfig.door[0].doorSensor = 513;
                        doorConfig.door[0].relayDeviceId = 115; // device ID
                        doorConfig.apbType = 0;
                        doorConfig.apbResetTime = 0;
                        doorConfig.doorMode = (int)BSSDK.BSDoorConfig.BSDOORCONFIG.ONE_DOOR;

                        Marshal.StructureToPtr(doorConfig, data, true);

                        result = BSSDK.BS_WriteDoorConfig(m_Handle, data);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot write the config", "Error");
                        }

                        Marshal.FreeHGlobal(data);
                    }
                    break;

                case BSSDK.BS_DEVICE_BIOENTRY_PLUS:
                case BSSDK.BS_DEVICE_BIOENTRY_W:
                case BSSDK.BS_DEVICE_XPASS:
                case BSSDK.BS_DEVICE_XPASS_SLIM:
                case BSSDK.BS_DEVICE_XPASS_SLIM2:
                    {
                        IntPtr data = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BEConfigData)));
                        int configSize = 0;

                        int result = BSSDK.BS_ReadConfig(m_Handle, BSSDK.BEPLUS_CONFIG, ref configSize, data);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Cursor.Current = Cursors.Default;
                            Marshal.FreeHGlobal(data);
                            MessageBox.Show("Cannot get the configData", "Error");
                            return;
                        }

                        BSSDK.BEConfigData configBEPlus = (BSSDK.BEConfigData)Marshal.PtrToStructure(data, typeof(BSSDK.BEConfigData));

                        configBEPlus.doorConfig.door[0].relay = (int)BSSDK.BSDoor.BSDOOR.HOST_RELAY0;
                        configBEPlus.doorConfig.door[0].useRTE = 0;
                        configBEPlus.doorConfig.door[0].useDoorSensor = 0;
                        configBEPlus.doorConfig.door[0].openEvent = (int)BSSDK.BSDoor.BSDOOR.ALL_EVENT;
                        configBEPlus.doorConfig.door[0].openTime = 3;
                        configBEPlus.doorConfig.door[0].heldOpenTime = 0;
                        configBEPlus.doorConfig.door[0].forcedOpenSchedule = 253;
                        configBEPlus.doorConfig.door[0].forcedCloseSchedule = 253;
                        configBEPlus.doorConfig.door[0].RTEType = 0;
                        configBEPlus.doorConfig.door[0].sensorType = 0;
                        configBEPlus.doorConfig.door[0].reader[0] = (short)BSSDK.BSDoor.BSDOOR.HOST_READER;
                        configBEPlus.doorConfig.door[0].reader[1] = (short)BSSDK.BSDoor.BSDOOR.HOST_READER;
                        configBEPlus.doorConfig.door[0].useRTEEx = 1;
                        configBEPlus.doorConfig.door[0].useSoundForcedOpen = 0;
                        configBEPlus.doorConfig.door[0].useSoundHeldOpen = 0;
                        configBEPlus.doorConfig.door[0].openOnce = 0;
                        configBEPlus.doorConfig.door[0].RTE = 512;
                        configBEPlus.doorConfig.door[0].useDoorSensorEx = 1;
                        configBEPlus.doorConfig.door[0].alarmStatus = 0;
                        configBEPlus.doorConfig.door[0].reserved2[0] = 0;
                        configBEPlus.doorConfig.door[0].reserved2[1] = 0;
                        configBEPlus.doorConfig.door[0].doorSensor = 513;
                        configBEPlus.doorConfig.door[0].relayDeviceId = 115; // device ID
                        configBEPlus.doorConfig.apbType = 0;
                        configBEPlus.doorConfig.apbResetTime = 0;
                        configBEPlus.doorConfig.doorMode = (int)BSSDK.BSDoorConfig.BSDOORCONFIG.ONE_DOOR;

                        Marshal.StructureToPtr(configBEPlus, data, true);

                        configSize = Marshal.SizeOf(typeof(BSSDK.BEConfigData));
                        result = BSSDK.BS_WriteConfig(m_Handle, BSSDK.BEPLUS_CONFIG, configSize, data);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot write the config", "Error");
                        }

                        Marshal.FreeHGlobal(data);
                    }
                    break;

                case BSSDK.BS_DEVICE_BIOLITE:
                    {
                        IntPtr data = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BEConfigDataBLN)));
                        int configSize = 0;

                        int result = BSSDK.BS_ReadConfig(m_Handle, BSSDK.BIOLITE_CONFIG, ref configSize, data);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Cursor.Current = Cursors.Default;
                            Marshal.FreeHGlobal(data);
                            MessageBox.Show("Cannot get the configData", "Error");
                            return;
                        }

                        BSSDK.BEConfigDataBLN configBLN = (BSSDK.BEConfigDataBLN)Marshal.PtrToStructure(data, typeof(BSSDK.BEConfigDataBLN));

                        configBLN.doorConfig.door[0].relay = (int)BSSDK.BSDoor.BSDOOR.HOST_RELAY0;
                        configBLN.doorConfig.door[0].useRTE = 0;
                        configBLN.doorConfig.door[0].useDoorSensor = 0;
                        configBLN.doorConfig.door[0].openEvent = (int)BSSDK.BSDoor.BSDOOR.ALL_EVENT;
                        configBLN.doorConfig.door[0].openTime = 3;
                        configBLN.doorConfig.door[0].heldOpenTime = 0;
                        configBLN.doorConfig.door[0].forcedOpenSchedule = 253;
                        configBLN.doorConfig.door[0].forcedCloseSchedule = 253;
                        configBLN.doorConfig.door[0].RTEType = 0;
                        configBLN.doorConfig.door[0].sensorType = 0;
                        configBLN.doorConfig.door[0].reader[0] = (short)BSSDK.BSDoor.BSDOOR.HOST_READER;
                        configBLN.doorConfig.door[0].reader[1] = (short)BSSDK.BSDoor.BSDOOR.HOST_READER;
                        configBLN.doorConfig.door[0].useRTEEx = 1;
                        configBLN.doorConfig.door[0].useSoundForcedOpen = 0;
                        configBLN.doorConfig.door[0].useSoundHeldOpen = 0;
                        configBLN.doorConfig.door[0].openOnce = 0;
                        configBLN.doorConfig.door[0].RTE = 512;
                        configBLN.doorConfig.door[0].useDoorSensorEx = 1;
                        configBLN.doorConfig.door[0].alarmStatus = 0;
                        configBLN.doorConfig.door[0].reserved2[0] = 0;
                        configBLN.doorConfig.door[0].reserved2[1] = 0;
                        configBLN.doorConfig.door[0].doorSensor = 513;
                        configBLN.doorConfig.door[0].relayDeviceId = 115; // device ID
                        configBLN.doorConfig.apbType = 0;
                        configBLN.doorConfig.apbResetTime = 0;
                        configBLN.doorConfig.doorMode = (int)BSSDK.BSDoorConfig.BSDOORCONFIG.ONE_DOOR;

                        Marshal.StructureToPtr(configBLN, data, true);

                        configSize = Marshal.SizeOf(typeof(BSSDK.BEConfigDataBLN));
                        result = BSSDK.BS_WriteConfig(m_Handle, BSSDK.BIOLITE_CONFIG, configSize, data);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot write the config", "Error");
                        }

                        Marshal.FreeHGlobal(data);
                    }
                    break;
            }

        }

        private void SetAccessGroup_Click(object sender, EventArgs e)
        {
            IntPtr data = Marshal.AllocHGlobal(BSSDK.DF_MAX_ACCESSGROUP * Marshal.SizeOf(typeof(BSSDK.BSAccessGroupEx)));

            BSSDK.BSAccessGroupEx[] AccessGroupEx = new BSSDK.BSAccessGroupEx[BSSDK.DF_MAX_ACCESSGROUP];
            for (int i = 0; i < BSSDK.DF_MAX_ACCESSGROUP; i++)
            {
                AccessGroupEx[i].name = new byte[32];
                AccessGroupEx[i].readerID = new uint[32];
                AccessGroupEx[i].scheduleID = new int[32];
                AccessGroupEx[i].reserved = new int[2];
            }


            int numOfGroup = 2; //support upto 128

	        AccessGroupEx[0].groupID = 1;            //AccessGroupIndex
            byte[] name = System.Text.Encoding.Unicode.GetBytes("AccessGroup1");   // name
            Buffer.BlockCopy(name, 0, AccessGroupEx[0].name, 0, name.Length);
	        AccessGroupEx[0].numOfReader   = 2;			// reader count
	        AccessGroupEx[0].readerID[0]   = 10053;		// reader ID is 100053
	        AccessGroupEx[0].scheduleID[0] = 1;			// nTimezone
	        AccessGroupEx[0].readerID[1]   = 10054;		// reader ID is 100054
	        AccessGroupEx[0].scheduleID[1] = 1;			// nTimezone


	        AccessGroupEx[1].groupID = 2;            //AccessGroupIndex
            byte[] name2 = System.Text.Encoding.Unicode.GetBytes("AccessGroup2");   // name
            Buffer.BlockCopy(name2, 0, AccessGroupEx[1].name, 0, name2.Length);
	        AccessGroupEx[1].numOfReader   = 1;			// reader count
	        AccessGroupEx[1].readerID[0]   = 10055;		// reader ID is 100055
	        AccessGroupEx[1].scheduleID[0] = 2;			// nTimezone
            AccessGroupEx[1].readerID[1]   = 10056;		// reader ID is 100056
            AccessGroupEx[1].scheduleID[1] = 1;			// nTimezone


            long LongPtr = data.ToInt64();
            for (int i = 0; i < AccessGroupEx.Length; i++)
            {
                IntPtr tempPtr = new IntPtr(LongPtr);
                Marshal.StructureToPtr(AccessGroupEx[i], tempPtr, false);
                LongPtr += Marshal.SizeOf(typeof(BSSDK.BSAccessGroupEx));
            }
            
	        int result = BSSDK.BS_SetAllAccessGroupEx(m_Handle, numOfGroup, data);
            if (result != BSSDK.BS_SUCCESS)
            {
                MessageBox.Show("Cannot write the config", "Error");
            }

            Marshal.FreeHGlobal(data);
        }

        private void SetTimecode_Click(object sender, EventArgs e)
        {
            IntPtr data = Marshal.AllocHGlobal(BSSDK.DF_MAX_TIMESCHEDULE * Marshal.SizeOf(typeof(BSSDK.BSTimeScheduleEx)));

            BSSDK.BSTimeScheduleEx[] TimezoneEx = new BSSDK.BSTimeScheduleEx[BSSDK.DF_MAX_TIMESCHEDULE];

            for (int i = 0; i < BSSDK.DF_MAX_TIMESCHEDULE; i++)
            {
                TimezoneEx[i].name = new byte[32];
                TimezoneEx[i].holiday = new int[2];
                TimezoneEx[i].timeCode = new BSSDK.BSTimeCodeEx[9];
                for (int k = 0; k < 9; k++)
                {
                    TimezoneEx[i].timeCode[k].codeElement = new BSSDK.BSTimeCodeElemEx[5];
                }

                TimezoneEx[i].reserved = new int[2];
            }

            int numOfSchedule = 2;  //support upto 128

            //1'st
            TimezoneEx[0].scheduleID = 1;  //Timezone
            
            byte[] name = System.Text.Encoding.Unicode.GetBytes("New Time Zone1");   // name
            Buffer.BlockCopy(name, 0, TimezoneEx[0].name, 0, name.Length);
            
            TimezoneEx[0].holiday[0] = 0;
            TimezoneEx[0].holiday[0] = 0;

            // codeElement support upto 5
            TimezoneEx[0].timeCode[0].codeElement[0].startTime  = 180;
            TimezoneEx[0].timeCode[0].codeElement[0].endTime    = 360;
            TimezoneEx[0].timeCode[0].codeElement[1].startTime  = 0;
            TimezoneEx[0].timeCode[0].codeElement[1].endTime    = 0;
            TimezoneEx[0].timeCode[0].codeElement[2].startTime  = 0;
            TimezoneEx[0].timeCode[0].codeElement[2].endTime    = 0;
            TimezoneEx[0].timeCode[0].codeElement[3].startTime  = 0;
            TimezoneEx[0].timeCode[0].codeElement[3].endTime    = 0;
            TimezoneEx[0].timeCode[0].codeElement[4].startTime  = 0;
            TimezoneEx[0].timeCode[0].codeElement[4].endTime    = 0;

            TimezoneEx[0].reserved[0] = 0;
            TimezoneEx[0].reserved[1] = 0;


            //2'nd
            TimezoneEx[1].scheduleID = 2;  //Timezone
            
            byte[] name2 = System.Text.Encoding.Unicode.GetBytes("New Time Zone2");   // name
            Buffer.BlockCopy(name2, 0, TimezoneEx[1].name, 0, name2.Length);
            
            TimezoneEx[1].holiday[0] = 0;
            TimezoneEx[1].holiday[0] = 0;

            // codeElement support upto 5
            TimezoneEx[1].timeCode[0].codeElement[0].startTime  = 180;
            TimezoneEx[1].timeCode[0].codeElement[0].endTime    = 360;
            TimezoneEx[1].timeCode[0].codeElement[1].startTime  = 0;
            TimezoneEx[1].timeCode[0].codeElement[1].endTime    = 0;
            TimezoneEx[1].timeCode[0].codeElement[2].startTime  = 0;
            TimezoneEx[1].timeCode[0].codeElement[2].endTime    = 0;
            TimezoneEx[1].timeCode[0].codeElement[3].startTime  = 0;
            TimezoneEx[1].timeCode[0].codeElement[3].endTime    = 0;
            TimezoneEx[1].timeCode[0].codeElement[4].startTime  = 0;
            TimezoneEx[1].timeCode[0].codeElement[4].endTime    = 0;

            TimezoneEx[1].reserved[0] = 0;
            TimezoneEx[1].reserved[1] = 0;


            long LongPtr = data.ToInt64();
            for (int i = 0; i < TimezoneEx.Length; i++)
            {
                IntPtr tempPtr = new IntPtr(LongPtr);
                Marshal.StructureToPtr(TimezoneEx[i], tempPtr, false);
                LongPtr += Marshal.SizeOf(typeof(BSSDK.BSTimeScheduleEx));
            }

            int result = BSSDK.BS_SetAllTimeScheduleEx(m_Handle, numOfSchedule, data);
            if (result != BSSDK.BS_SUCCESS)
            {
                MessageBox.Show("Cannot write the config", "Error");
            }

            Marshal.FreeHGlobal(data);
        }

        private void SetHoliday_Click(object sender, EventArgs e)
        {
            IntPtr data = Marshal.AllocHGlobal(BSSDK.BS_MAX_HOLIDAY_EX * Marshal.SizeOf(typeof(BSSDK.BSHolidayEx)));

            BSSDK.BSHolidayEx[] HolidayEx = new BSSDK.BSHolidayEx[BSSDK.BS_MAX_HOLIDAY_EX];

            for (int i = 0; i < BSSDK.BS_MAX_HOLIDAY_EX; i++)
            {
                HolidayEx[i].name = new byte[32];
                HolidayEx[i].holiday = new BSSDK.BSHolidayElemEx[32];
                for (int k = 0; k < 32; k++)
                {
                    HolidayEx[i].holiday[k].reserved = new byte[3];
                }

                HolidayEx[i].reserved = new int[2];
            }

	        int numOfHolidaySchedule = 2;   //support upto 32

            //1'st
            HolidayEx[0].holidayID = 1;    //HolidayIndex
     
            byte[] name = System.Text.Encoding.Unicode.GetBytes("Holiday1");   // name
            Buffer.BlockCopy(name, 0, HolidayEx[0].name, 0, name.Length);

            HolidayEx[0].numOfHoliday = 2;

            // olidayEx[0].holiday supports upto 32

            //(ex) from 2012-9-1 to 2012-9-3
            HolidayEx[0].holiday[0].year	 = 12;      // since 2000
            HolidayEx[0].holiday[0].month	 = 9;       //1~12
            HolidayEx[0].holiday[0].startDay = 1;
            HolidayEx[0].holiday[0].duration = 3;       //pHolidayDataInfo->nDaysLong;
            HolidayEx[0].holiday[0].flag     = 0;   // pHolidayDataInfo->nYearRepeat ? false : true;

            HolidayEx[0].holiday[1].year	 = 12;      // since 2000
            HolidayEx[0].holiday[1].month	 = 10;      //1~12
            HolidayEx[0].holiday[1].startDay = 5;
            HolidayEx[0].holiday[1].duration = 1;
            HolidayEx[0].holiday[1].flag	 = 0;


            //2'nd
            HolidayEx[1].holidayID = 2;    //HolidayIndex

            byte[] name1 = System.Text.Encoding.Unicode.GetBytes("Holiday1");   // name
            Buffer.BlockCopy(name, 0, HolidayEx[1].name, 0, name1.Length);

            HolidayEx[1].numOfHoliday = 1;
            HolidayEx[1].holiday[0].year	 = 12;   // since 2000
            HolidayEx[1].holiday[0].month	 = 12;       //1~12
            HolidayEx[1].holiday[0].startDay = 24;
            HolidayEx[1].holiday[0].duration = 5;
            HolidayEx[1].holiday[0].flag	 = 0;


            long LongPtr = data.ToInt64();
            for (int i = 0; i < HolidayEx.Length; i++)
            {
                IntPtr tempPtr = new IntPtr(LongPtr);
                Marshal.StructureToPtr(HolidayEx[i], tempPtr, false);
                LongPtr += Marshal.SizeOf(typeof(BSSDK.BSHolidayEx));
            }

            int result = BSSDK.BS_SetAllHolidayEx(m_Handle, numOfHolidaySchedule, data);
            if (result != BSSDK.BS_SUCCESS)
            {
                MessageBox.Show("Cannot write the config", "Error");
            }

            Marshal.FreeHGlobal(data);
        }

    }
}