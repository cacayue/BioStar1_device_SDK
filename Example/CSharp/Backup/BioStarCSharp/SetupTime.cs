using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BioStarCSharp
{
    public partial class SetupTime : Form
    {
        int m_Handle = -1;
        int m_DeviceType = -1;

        public SetupTime()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Dispose();
        }
        
        public void setHandle(int handle)
        {
            m_Handle = handle;
        }

        public void setDeviceType(int type)
        {
            m_DeviceType = type;    
        }

        private void getTime_Click(object sender, EventArgs e)
        {
            int localTime = 0;

            Cursor.Current = Cursors.WaitCursor;

            int result = BSSDK.BS_GetTime(m_Handle, ref localTime);

            Cursor.Current = Cursors.Default;

            if (result != BSSDK.BS_SUCCESS)
            {
                MessageBox.Show("Cannot get the time", "Error");
                return;
            }

            datePicker.Value = new DateTime(1970, 1, 1).AddSeconds(localTime);
            timePicker.Value = datePicker.Value;
        }

        private void setTime_Click(object sender, EventArgs e)
        {
            int localTime = (int)((timePicker.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);

            Cursor.Current = Cursors.WaitCursor;

            EnableControl(m_DeviceType, m_Handle, false, 2);

            int result = BSSDK.BS_SetTime(m_Handle, localTime);

            EnableControl(m_DeviceType, m_Handle, true, 2);

            Cursor.Current = Cursors.Default;

            if (result != BSSDK.BS_SUCCESS)
            {
                MessageBox.Show("Cannot set the time", "Error");
                return;
            }
        }

        private void SetupTime_Load(object sender, EventArgs e)
        {
            datePicker.Value = DateTime.Now;
            timePicker.Value = DateTime.Now;
        }

        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            timePicker.Value = datePicker.Value;
        }

        private int EnableControl(int nDeviceType, int nChannel, bool bEnable, int nTimeout)
        {
	        int nRtn = BSSDK.BS_SUCCESS;
	        switch(nDeviceType)
	        {
                case BSSDK.BS_DEVICE_BIOSTATION:
                case BSSDK.BS_DEVICE_BIOLITE:
                case BSSDK.BS_DEVICE_DSTATION:
                case BSSDK.BS_DEVICE_XSTATION:
                case BSSDK.BS_DEVICE_BIOSTATION2:
		            {
			            for(int nIndex=0; nIndex<2; nIndex++)
			            {
                            nRtn = bEnable ? BSSDK.BS_Enable(nChannel) : BSSDK.BS_Disable(nChannel, nTimeout);
                            if (nRtn == BSSDK.BS_SUCCESS)
					            break;
				            //Sleep(50);
			            }
		            }
		            break;
	            default:
		            break;
	        }
        	
	        return nRtn;
        }
    }
}