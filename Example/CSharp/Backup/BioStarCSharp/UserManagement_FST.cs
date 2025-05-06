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
    public partial class UserManagement_FST : Form
    {
        public const int TEMPLATE_SIZE = 384;
        public const int FACETEMPLATE_SIZE = 2284;
        public const int BS_MAX_TEMPLATE_PER_USER = 10;
        public const int BS_MAX_FACE_PER_USER = 5;
        public const int BS_MAX_IMAGE_SIZE = (100 * 1024);

        public const int BS_FST_MAX_FACE_TYPE = 5;
        public const int BS_FST_MAX_FACE_TEMPLATE = 25;
        public const int BS_FST_FACETEMPLATE_SIZE = 2000;

        private int m_Handle = 0;
        private uint m_DeviceID = 0;
        private int m_DeviceType = -1;
        private int m_NumOfUser = 0;
        private int m_NumOfFace = 0;

        private byte[] m_StillcutData = new byte[BS_FST_MAX_FACE_TYPE * BS_MAX_IMAGE_SIZE];
        private byte[] m_FaceTemplate = new byte[BS_FST_MAX_FACE_TYPE * BS_FST_FACETEMPLATE_SIZE * BS_FST_MAX_FACE_TEMPLATE];

        private IntPtr m_userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.FSUserHdrEx)));

        public UserManagement_FST()
        {
            InitializeComponent();
        }

        public void SetDevice(int handle, uint deviceID, int deviceType)
        {
            m_Handle = handle;
            m_DeviceID = deviceID;
            m_DeviceType = deviceType;


            switch (m_DeviceType)
            {
                case BSSDK.BS_DEVICE_FSTATION:
                    {
                        this.authMode.Items.Clear();
                        this.authMode.Items.AddRange(new object[] {
                        "Face Only",
                        "Face and Password",
                        "Card only",
                        "Card and Password",
                        "Card and Face",
                        "Card and Face/Password"});
                    }
                    break;
                default:
                    {
                        this.authMode.Items.Clear();
                        this.authMode.Items.AddRange(new object[] {
                        "Disabled",
                        "Finger only",
                        "Finger and Password",
                        "Finger or Password",
                        "Password only",
                        "Card only"});
                    }
                    break;
            }

        }

        private void UserManagement_FST_Load(object sender, EventArgs e)
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
                case BSSDK.BS_DEVICE_XPASS_SLIM:
                    deviceInfo.Text = "Xpass Slim" + m_DeviceID.ToString();
                    break;
                case BSSDK.BS_DEVICE_XPASS_SLIM2:
                    deviceInfo.Text = "Xpass S2" + m_DeviceID.ToString();
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
                default:
                    deviceInfo.Text = "Unknown " + m_DeviceID.ToString();
                    break;
            }

            accessGroup.Text = "ffffffff";
            userLevel.SelectedIndex = 0;
            securityLevel.SelectedIndex = 0;
            cardType.SelectedIndex = 0;
            authMode.SelectedIndex = 0;

            ReadUserInfo();
        }

        private bool ReadUserInfo()
        {
            int result = 0;

            m_NumOfUser = 0;
            m_NumOfFace = 0;

            userList.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            switch (m_DeviceType)
            {
                case BSSDK.BS_DEVICE_FSTATION:
                    {
                        result = BSSDK.BS_GetUserDBInfo(m_Handle, ref m_NumOfUser, ref m_NumOfFace);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot get user DB info", "Error");
                            return false;
                        }

                        BSSDK.FSUserHdrEx[] userHdr = new BSSDK.FSUserHdrEx[m_NumOfUser];
                        IntPtr userInfo = Marshal.AllocHGlobal(m_NumOfUser * Marshal.SizeOf(typeof(BSSDK.FSUserHdrEx)));

                        result = BSSDK.BS_GetAllUserInfoFStationEx(m_Handle, userInfo, ref m_NumOfUser);
                        if (result != BSSDK.BS_SUCCESS && result != BSSDK.BS_ERR_NOT_FOUND)
                        {
                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get user header info", "Error");
                            return false;
                        }

                        for (int i = 0; i < m_NumOfUser; i++)
                        {
                            userHdr[i] = (BSSDK.FSUserHdrEx)Marshal.PtrToStructure(new IntPtr(userInfo.ToInt32() + i * Marshal.SizeOf(typeof(BSSDK.FSUserHdrEx))), typeof(BSSDK.FSUserHdrEx));

                            ListViewItem userItem = userList.Items.Add(userHdr[i].ID.ToString());
                            userItem.SubItems.Add(userHdr[i].numOfFaceType.ToString());
                            userItem.SubItems.Add(userHdr[i].cardID.ToString("X"));

                            m_NumOfFace += userHdr[i].numOfFaceType;
                        }

                        Marshal.FreeHGlobal(userInfo);

                        numOfUser.Text = m_NumOfUser.ToString();
                        numOfFaces.Text = m_NumOfFace.ToString();
                    }
                    break;
            }

            Cursor.Current = Cursors.Default;

            if (m_NumOfUser > 0)
            {
                try
                {
                    uint ID = UInt32.Parse(userList.Items[0].Text);

                    ReadUser(ID);
                }
                catch (Exception)
                {
                }
            }

            return true;
        }

        private void ReadUser(uint ID)
        {
            Cursor.Current = Cursors.WaitCursor;

            switch (m_DeviceType)
            {
                case BSSDK.BS_DEVICE_FSTATION:
                    {
                        byte[] imageData = new byte[BS_MAX_IMAGE_SIZE * BS_FST_MAX_FACE_TYPE];
                        byte[] faceTemplate = new byte[BS_FST_FACETEMPLATE_SIZE * BS_FST_MAX_FACE_TEMPLATE * BS_FST_MAX_FACE_TYPE];

                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.FSUserHdrEx)));

                        int result = BSSDK.BS_GetUserFStationEx(m_Handle, ID, userInfo, imageData, faceTemplate);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Cursor.Current = Cursors.Default;
                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get user info", "Error");
                            return;
                        }

                        //copy userInfo -> m_userInfo
                        int nSize = Marshal.SizeOf(typeof(BSSDK.FSUserHdrEx));
                        byte[] bytes = new byte[nSize];
                        Marshal.Copy(userInfo, bytes, 0, nSize);
                        Marshal.Copy(bytes, 0, m_userInfo, nSize);


                        BSSDK.FSUserHdrEx userHdr = (BSSDK.FSUserHdrEx)Marshal.PtrToStructure(userInfo, typeof(BSSDK.FSUserHdrEx));

                        userID.Text = userHdr.ID.ToString();
                        userCardID.Text = userHdr.cardID.ToString("X");
                        cardCustomID.Text = userHdr.customID.ToString();
                        userLevel.SelectedIndex = (userHdr.adminLevel == (ushort)BSSDK.FSUserHdr.ENUM.USER_ADMIN) ? 1 : 0;
                        securityLevel.SelectedIndex = (userHdr.securityLevel >= BSSDK.BS_USER_SECURITY_DEFAULT) ? userHdr.securityLevel - BSSDK.BS_USER_SECURITY_DEFAULT : 0;
                        cardType.SelectedIndex = userHdr.bypassCard;

                        byte[] asBytes = new byte[userHdr.name.Length * sizeof(ushort)];
                        Buffer.BlockCopy(userHdr.name, 0, asBytes, 0, asBytes.Length);
                        name.Text = Encoding.Unicode.GetString(asBytes);

                        accessGroup.Text = userHdr.accessGroupMask.ToString("X");

                        if (userHdr.authMode >= BSSDK.PAUTH_FACE_ONLY && userHdr.authMode <= BSSDK.PAUTH_FACE_PIN_KEY)
                            authMode.SelectedIndex = userHdr.authMode;
                        else
                            authMode.SelectedIndex = 0;

                        startDate.Value = new DateTime(1970, 1, 1).AddSeconds(userHdr.startDateTime);
                        expiryDate.Value = new DateTime(1970, 1, 1).AddSeconds(userHdr.expireDateTime);

                        //insert facetemplate into listbox
                        listBox1.Items.Clear();

                        int imagePos = 0;
                        int templatePos = 0;
                        int nTemplateLen = 0;

                        numOfFace.Text = Convert.ToString(userHdr.numOfFaceType);

                        for (int i = 0; i < userHdr.numOfFaceType; i++)
                        {
                            string strText = String.Format("{0,0} : template Num= {1,00} updated= {2,0}", i + 1, userHdr.numOfFace[i], userHdr.numOfUpdatedFace[i]);
                            listBox1.Items.Add(strText);

                            //stillcut
                            Buffer.BlockCopy(imageData, imagePos, m_StillcutData, i * BS_MAX_IMAGE_SIZE, userHdr.faceStillcutLen[i]);
                            imagePos += userHdr.faceStillcutLen[i];

                            //facetemplate
                            nTemplateLen = 0;
                            for (int k = 0; k < userHdr.numOfFace[i]; k++)
                                nTemplateLen += userHdr.faceLen[i * 25 + k];

                            Buffer.BlockCopy(faceTemplate, templatePos, m_FaceTemplate, i * BS_FST_MAX_FACE_TEMPLATE * BS_FST_FACETEMPLATE_SIZE, nTemplateLen);
                            templatePos += nTemplateLen;
                        }

                        //stillcut clear
                        pictureBox1.Image = null;
                        pictureBox1.Invalidate();

                        Marshal.FreeHGlobal(userInfo);
                    }
                    break;
            }

            Cursor.Current = Cursors.Default;
        }

        private void userList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userList.SelectedItems.Count > 0)
            {
                try
                {
                    uint ID = UInt32.Parse(userList.SelectedItems[0].Text);

                    ReadUser(ID);
                }
                catch (Exception)
                {
                }
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            ReadUserInfo();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (userList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a user first", "Error");
                return;
            }

            uint ID = 0;

            try
            {
                ID = UInt32.Parse(userList.SelectedItems[0].Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid ID", "Error");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            int result = BSSDK.BS_DeleteUser(m_Handle, ID);

            Cursor.Current = Cursors.Default;

            if (result != BSSDK.BS_SUCCESS)
            {
                MessageBox.Show("Cannot delete the user", "Error");
                return;
            }

            ReadUserInfo();
        }

        private void deleteAllButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            int result = BSSDK.BS_DeleteAllUser(m_Handle);

            Cursor.Current = Cursors.Default;

            if (result != BSSDK.BS_SUCCESS)
            {
                MessageBox.Show("Cannot delete all users", "Error");
                return;
            }

            ReadUserInfo();
        }

        private void readCard_Click(object sender, EventArgs e)
        {
            uint card_id = 0;
            int custom_id = 0;

            Cursor.Current = Cursors.WaitCursor;

            int result = BSSDK.BS_ReadCardIDEx(m_Handle, ref card_id, ref custom_id);

            Cursor.Current = Cursors.Default;

            if (result != BSSDK.BS_SUCCESS)
            {
                MessageBox.Show("Cannot read the card", "Error");
                return;
            }

            userCardID.Text = card_id.ToString("X");
            cardCustomID.Text = custom_id.ToString();
        }

        private void ScanFaceTemplate_Click(object sender, EventArgs e)
        {
            int nIndex = listBox1.SelectedIndex;
            if (nIndex < 0)
            {
                MessageBox.Show("There is no selected item");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            int result = BSSDK.BS_SUCCESS;
            DialogResult nRet = DialogResult.Yes;

            byte[] imageData = new byte[BS_MAX_IMAGE_SIZE];
            byte[] faceTemplate = new byte[BS_FST_FACETEMPLATE_SIZE * BS_FST_MAX_FACE_TEMPLATE];

            IntPtr userTemplateHdrInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.FSUserTemplateHdr)));

            do
            {
                result = BSSDK.BS_ScanFaceTemplate(m_Handle, userTemplateHdrInfo, imageData, faceTemplate);
                if (result != BSSDK.BS_SUCCESS)
                {
                    nRet = MessageBox.Show("Error Capture User Face!!!\r\nTry capture again?", "Error", MessageBoxButtons.RetryCancel);

                    if (nRet != DialogResult.Retry)
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    int faceTemplateLen = 0;
                    int nOffset = 0;
                    uint nChecksum = 0;

                    BSSDK.FSUserTemplateHdr userTemplateHdr = (BSSDK.FSUserTemplateHdr)Marshal.PtrToStructure(userTemplateHdrInfo, typeof(BSSDK.FSUserTemplateHdr));
                    BSSDK.FSUserHdrEx userHdr = (BSSDK.FSUserHdrEx)Marshal.PtrToStructure(m_userInfo, typeof(BSSDK.FSUserHdrEx));

                    userHdr.numOfFace[nIndex] = userTemplateHdr.numOfFace;
                    userHdr.numOfUpdatedFace[nIndex] = userTemplateHdr.numOfUpdatedFace;
                    userHdr.faceStillcutLen[nIndex] = userTemplateHdr.imageSize;

                    for (int i = 0; i < BS_FST_MAX_FACE_TEMPLATE; i++)
                    {
                        userHdr.faceLen[nIndex * 25 + i] = userTemplateHdr.faceLen[i];
                        faceTemplateLen += userTemplateHdr.faceLen[i];

                        // face template's checksum
                        int nLen = userTemplateHdr.faceLen[i];
                        if (nLen > 0)
                        {
                            byte[] templateBuf = new byte[nLen];
                            Buffer.BlockCopy(faceTemplate, nOffset, templateBuf, 0, nLen);

                            for (int j = 0; j < userTemplateHdr.faceLen[i]; j++)
                            {
                                nChecksum += templateBuf[j];
                            }
                            userHdr.faceChecksum[nIndex * 25 + i] = nChecksum;
                            nOffset += nLen;
                        }
                    }

                    Buffer.BlockCopy(imageData, 0, m_StillcutData, nIndex * BS_MAX_IMAGE_SIZE, userTemplateHdr.imageSize);
                    Buffer.BlockCopy(faceTemplate, 0, m_FaceTemplate, nIndex * BS_FST_FACETEMPLATE_SIZE * BS_FST_MAX_FACE_TEMPLATE, faceTemplateLen);

                    Marshal.StructureToPtr(userHdr, m_userInfo, true);
                    listBox1_SelectedIndexChanged( null, null); 
                    
                    break;
                }

            } while (nRet == DialogResult.Retry);

            System.Threading.Thread.Sleep(500);     // The delay is required that is more than five miliseconds.

            Marshal.FreeHGlobal(userTemplateHdrInfo);

            Cursor.Current = Cursors.Default;
        }

        private void updateUser_Click(object sender, EventArgs e)
        {
            //pwd
            string strPwd = password.Text;
            for (int i = 0; i < strPwd.Length; i++)
            {
                if (!Char.IsDigit(strPwd, i))
                {
                    MessageBox.Show("Digit only press.");
                    return;
                }
            }


            if (startDate.Value.Year < 1970)
            {
                MessageBox.Show("Start Date can not be less than 1970.");
                return;
            }

            if (startDate.Value.Year > 2030)
            {
                MessageBox.Show("Start Date can not be more than 2030.");
                return;
            }

            if (expiryDate.Value.Year < 1970)
            {
                MessageBox.Show("Expire Date can not be less than 1970.");
                return;
            }

            if (expiryDate.Value.Year > 2030)
            {
                MessageBox.Show("Expire Date can not be more than 2030.");
                return;
            }


            Cursor.Current = Cursors.WaitCursor;

            //get user info
            IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.FSUserHdrEx)));

            //copy m_userInfo -> userInfo
            int nSize = Marshal.SizeOf(typeof(BSSDK.FSUserHdrEx));
            byte[] bytes = new byte[nSize];
            Marshal.Copy(m_userInfo, bytes, 0, nSize);
            Marshal.Copy(bytes, 0, userInfo, nSize);
            BSSDK.FSUserHdrEx userHdr = (BSSDK.FSUserHdrEx)Marshal.PtrToStructure(userInfo, typeof(BSSDK.FSUserHdrEx));
            BSSDK.FSUserHdrEx tempHdr = (BSSDK.FSUserHdrEx)Marshal.PtrToStructure(m_userInfo, typeof(BSSDK.FSUserHdrEx));

            //remove empty item
            byte[] imageData = new byte[BS_MAX_IMAGE_SIZE * BS_FST_MAX_FACE_TYPE];
            byte[] faceTemplate = new byte[BS_FST_FACETEMPLATE_SIZE * BS_FST_MAX_FACE_TEMPLATE * BS_FST_MAX_FACE_TYPE];

            //face Stillcut
            int nCount = 0;
            int stillcutPos = 0;
            for (int i = 0; i < BS_FST_MAX_FACE_TYPE; i++)
            {
                if (userHdr.faceStillcutLen[i] > 0)
                {
                    Buffer.BlockCopy(m_StillcutData, i * BS_MAX_IMAGE_SIZE, imageData, stillcutPos, userHdr.faceStillcutLen[i]);
                    stillcutPos += userHdr.faceStillcutLen[i];

                    userHdr.faceStillcutLen[nCount++] = tempHdr.faceStillcutLen[i];
                }
            }

            //face template
            nCount = 0;
            int faceTemplatePos = 0;

            userHdr.numOfFaceType = 0;

            for (int i = 0; i < BS_FST_MAX_FACE_TYPE; i++)
            {
                int len = 0;
                for (int k = 0; k < BS_FST_MAX_FACE_TEMPLATE; k++)
                {
                    len += userHdr.faceLen[i * 25 + k];
                }

                if (len > 0)
                {
                    Buffer.BlockCopy(m_FaceTemplate, i * BS_FST_MAX_FACE_TEMPLATE * BS_FST_FACETEMPLATE_SIZE, faceTemplate, faceTemplatePos, len);
                    faceTemplatePos += len;

                    userHdr.numOfFaceType++;    //faceType
                    for (int k = 0; k < BS_FST_MAX_FACE_TEMPLATE; k++)
                    {
                        userHdr.faceLen[nCount * 25 + k] = tempHdr.faceLen[i * 25 + k]; //faceLen
                        userHdr.faceChecksum[nCount * 25 + k] = tempHdr.faceChecksum[i * 25 + k];    //face Checksum
                    }
                    nCount++;
                }
            }

            //initilize dummy facetemplete data 
	        for(int i=nCount; i<BS_FST_MAX_FACE_TYPE; i++)
	        {
		        userHdr.faceStillcutLen[i] = 0;
                for (int k = 0; k < BS_FST_MAX_FACE_TEMPLATE; k++)
                {
                    userHdr.faceLen[i * 25 + k]      = 0; //faceLen
                    userHdr.faceChecksum[i * 25 + k] = 0; //face Checksum
		        }
	        }  


            // name 
            string username = name.Text;
            byte[] nameBytes = Encoding.Unicode.GetBytes(username);             // UTF16
            Buffer.BlockCopy(nameBytes, 0, userHdr.name, 0, nameBytes.Length);

            // pwd
            string pwd = password.Text;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] pwdBytes = encoding.GetBytes(pwd);
            byte[] pwdOut = new byte[32];
            BSSDK.BS_EncryptSHA256(pwdBytes, pwdBytes.Length, pwdOut);
            Buffer.BlockCopy(pwdOut, 0, userHdr.password, 0, pwdOut.Length);
            //

            userHdr.adminLevel = (ushort)((userLevel.SelectedIndex == 1) ? BSSDK.FSUserHdrEx.ENUM.USER_ADMIN : BSSDK.FSUserHdrEx.ENUM.USER_NORMAL);
            userHdr.securityLevel = (ushort)(securityLevel.SelectedIndex + BSSDK.BS_USER_SECURITY_DEFAULT);

            userHdr.bypassCard = (byte)cardType.SelectedIndex;
            userHdr.startDateTime = (uint)((startDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
            userHdr.expireDateTime = (uint)((expiryDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
            userHdr.authMode = (ushort)authMode.SelectedIndex;

            try
            {
                userHdr.cardID = UInt32.Parse(userCardID.Text, System.Globalization.NumberStyles.HexNumber);
            }
            catch (Exception)
            {
                userHdr.cardID = 0;
            }

            try
            {
                userHdr.customID = (byte)Int32.Parse(cardCustomID.Text);
            }
            catch (Exception)
            {
                userHdr.customID = 0;
            }

            try
            {
                userHdr.accessGroupMask = UInt32.Parse(accessGroup.Text, System.Globalization.NumberStyles.HexNumber);
            }
            catch (Exception)
            {
                userHdr.accessGroupMask = 0xffffffff;
            }


            Marshal.StructureToPtr(userHdr, userInfo, true);

            int result = BSSDK.BS_EnrollUserFStationEx(m_Handle, userInfo, imageData, faceTemplate);
            if (result != BSSDK.BS_SUCCESS)
            {
                MessageBox.Show("Cannot enroll the user", "Error");
            }

            Marshal.FreeHGlobal(userInfo);

            Cursor.Current = Cursors.Default;
        }

        private void addUser_Click(object sender, EventArgs e)
        {
            uint ID = 0;
            int result = 0;

            try
            {
                if (userID.Text.Length > 0)
                {
                    ID = UInt32.Parse(userID.Text);
                }
                else
                {
                    MessageBox.Show("Enter user ID first", "Error");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid ID", "Error");
                return;
            }

            //pwd
            string strPwd = password.Text;
            for (int i = 0; i < strPwd.Length; i++)
            {
                if (!Char.IsDigit(strPwd, i))
                {
                    MessageBox.Show("Digit only press.");
                    return;
                }
            }


            if (startDate.Value.Year < 1970)
            {
                MessageBox.Show("Start Date can not be less than 1970.");
                return;
            }

            if (startDate.Value.Year > 2030)
            {
                MessageBox.Show("Start Date can not be more than 2030.");
                return;
            }

            if (expiryDate.Value.Year < 1970)
            {
                MessageBox.Show("Expire Date can not be less than 1970.");
                return;
            }

            if (expiryDate.Value.Year > 2030)
            {
                MessageBox.Show("Expire Date can not be more than 2030.");
                return;
            }


            byte[] StillcutData = new byte[BS_MAX_IMAGE_SIZE * BS_FST_MAX_FACE_TYPE];
            byte[] FaceTemplateData = new byte[BS_FST_FACETEMPLATE_SIZE * BS_FST_MAX_FACE_TEMPLATE * BS_FST_MAX_FACE_TYPE];

            IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.FSUserHdrEx)));

            int nSize = Marshal.SizeOf(typeof(BSSDK.FSUserHdrEx));
            byte[] bytes = new byte[nSize];
            Marshal.Copy(userInfo, bytes, 0, nSize);
            Array.Clear(bytes, 0, nSize);
            Marshal.Copy(bytes, 0, userInfo, nSize);

            BSSDK.FSUserHdrEx userHdr = (BSSDK.FSUserHdrEx)Marshal.PtrToStructure(userInfo, typeof(BSSDK.FSUserHdrEx));

            //scan face
            bool bNeedScan = false;
            if (MessageBox.Show("Do you want to scan a new face ?", "FaceStation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bNeedScan = true;
            }

            if (bNeedScan == true)
            {
                byte[] imageData = new byte[BS_MAX_IMAGE_SIZE];
                byte[] faceTemplate = new byte[BS_FST_FACETEMPLATE_SIZE * BS_FST_MAX_FACE_TEMPLATE];

                DialogResult nRet = DialogResult.None;
                int nStillcutBufPos = 0;
                int nTemplateBufPos = 0;
                int nOffset = 0;

                for (int i = 0; i < BS_FST_MAX_FACE_TYPE; i++)
                {
                    IntPtr userTemplateHdrInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.FSUserTemplateHdr)));

                    do
                    {
                        nRet = DialogResult.None;

                        Cursor.Current = Cursors.WaitCursor;
                        result = BSSDK.BS_ScanFaceTemplate(m_Handle, userTemplateHdrInfo, imageData, faceTemplate);
                        Cursor.Current = Cursors.Default;

                        if (result != BSSDK.BS_SUCCESS)
                        {
                            nRet = MessageBox.Show("Error Capture User Face!!!\r\nTry capture again?", "Error", MessageBoxButtons.RetryCancel);
                            if (nRet != DialogResult.Retry)
                            {
                                Cursor.Current = Cursors.Default;
                                break;
                            }
                        }
                    } while (nRet == DialogResult.Retry);

                    BSSDK.FSUserTemplateHdr userTemplateHdr = (BSSDK.FSUserTemplateHdr)Marshal.PtrToStructure(userTemplateHdrInfo, typeof(BSSDK.FSUserTemplateHdr));
                    if (nRet == DialogResult.None && result == BSSDK.BS_SUCCESS && userTemplateHdr.numOfFace > 0)
                    {
                        //scan success
                        userHdr.numOfFaceType++;
                        userHdr.numOfFace[i] = userTemplateHdr.numOfFace;
                        userHdr.numOfUpdatedFace[i] = userTemplateHdr.numOfUpdatedFace;

                        //face template's length
                        for (int k = 0; k < BS_FST_MAX_FACE_TEMPLATE; k++)
                        {
                            userHdr.faceLen[i * 25 + k] = userTemplateHdr.faceLen[k];
                        }

                        // face template's checksum
                        nOffset = 0;
                        uint nChecksum = 0;
                        for (int k = 0; k < BS_FST_MAX_FACE_TEMPLATE; k++)
                        {
                            int nLen = userTemplateHdr.faceLen[k];
                            if (nLen > 0)
                            {
                                byte[] templateBuf = new byte[nLen];
                                Buffer.BlockCopy(faceTemplate, nOffset, templateBuf, 0, nLen);

                                for (int j = 0; j < userTemplateHdr.faceLen[k]; j++)
                                {
                                    nChecksum += templateBuf[j];
                                }
                                userHdr.faceChecksum[i * 25 + k] = nChecksum;
                                nOffset += nLen;
                            }
                        }

                        //stillcut length
                        userHdr.faceStillcutLen[i] = userTemplateHdr.imageSize;

                        //fill the Stillcut image data
                        if (userTemplateHdr.imageSize > 0)
                            System.Buffer.BlockCopy(imageData, 0, StillcutData, nStillcutBufPos, userTemplateHdr.imageSize);
                        nStillcutBufPos += userTemplateHdr.imageSize;

                        //fill the facetemplate data
                        nOffset = 0;
                        for (int k = 0; k < BS_FST_MAX_FACE_TEMPLATE; k++)
                        {
                            Buffer.BlockCopy(faceTemplate, nOffset, FaceTemplateData, nTemplateBufPos, userTemplateHdr.faceLen[k]);

                            nTemplateBufPos += userTemplateHdr.faceLen[k];
                            nOffset += userTemplateHdr.faceLen[k];
                        }
                    }

                    System.Threading.Thread.Sleep(500);     // The delay is required that is more than five miliseconds.

                    if (DialogResult.Yes != MessageBox.Show("Do you want to continue scanning ?", "Scanning...", MessageBoxButtons.RetryCancel))
                    {
                        break;
                    }

                    Marshal.FreeHGlobal(userTemplateHdrInfo);
                }
            }


            // name 
            string username = name.Text;
            byte[] nameBytes = Encoding.Unicode.GetBytes(username);             // UTF16
            Buffer.BlockCopy(nameBytes, 0, userHdr.name, 0, nameBytes.Length);

            // pwd
            string pwd = password.Text;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] pwdBytes = encoding.GetBytes(pwd);
            byte[] pwdOut = new byte[32];
            BSSDK.BS_EncryptSHA256(pwdBytes, pwdBytes.Length, pwdOut);
            Buffer.BlockCopy(pwdOut, 0, userHdr.password, 0, pwdOut.Length);
            //

            userHdr.ID = ID;

            userHdr.adminLevel = (ushort)((userLevel.SelectedIndex == 1) ? BSSDK.FSUserHdrEx.ENUM.USER_ADMIN : BSSDK.FSUserHdrEx.ENUM.USER_NORMAL);
            userHdr.securityLevel = (ushort)(securityLevel.SelectedIndex + BSSDK.BS_USER_SECURITY_DEFAULT);

            userHdr.bypassCard = (byte)cardType.SelectedIndex;
            userHdr.startDateTime = (uint)((startDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
            userHdr.expireDateTime = (uint)((expiryDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);


            userHdr.authMode = (ushort)authMode.SelectedIndex;

            try
            {
                userHdr.cardID = UInt32.Parse(userCardID.Text, System.Globalization.NumberStyles.HexNumber);
            }
            catch (Exception)
            {
                userHdr.cardID = 0;
            }

            try
            {
                userHdr.customID = (byte)Int32.Parse(cardCustomID.Text);
            }
            catch (Exception)
            {
                userHdr.customID = 0;
            }

            try
            {
                userHdr.accessGroupMask = UInt32.Parse(accessGroup.Text, System.Globalization.NumberStyles.HexNumber);
            }
            catch (Exception)
            {
                userHdr.accessGroupMask = 0xffffffff;
            }

            //IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.FSUserHdrEx)));
            Marshal.StructureToPtr(userHdr, userInfo, true);

            Cursor.Current = Cursors.WaitCursor;
            result = BSSDK.BS_EnrollUserFStationEx(m_Handle, userInfo, StillcutData, FaceTemplateData);
            Cursor.Current = Cursors.Default;

            Marshal.FreeHGlobal(userInfo);

            if (result != BSSDK.BS_SUCCESS)
            {
                MessageBox.Show("Cannot enroll the user", "Error");
                return;
            }

            ReadUserInfo();
        }

        private void SaveFaceImage(string filename, byte[] imageData, int nLength)
        {
            byte[] imageBytes = new byte[nLength];
            System.Buffer.BlockCopy(imageData, 0, imageBytes, 0, nLength);

            try
            {
                FileStream fs = new FileStream(filename, FileMode.CreateNew, FileAccess.ReadWrite);
                BinaryWriter write_fs = new BinaryWriter(fs);
                write_fs.Write(imageBytes);
                write_fs.Close();
                fs.Close();
            }
            catch (Exception saveE)
            {
                string str = saveE.Message;
                //MessageBox.Show(this, saveE.Message);
            }
        } 

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nIndex = listBox1.SelectedIndex;
            if (nIndex < 0)
            {
                pictureBox1.Image = null;
                pictureBox1.Invalidate();
                return;
            }

            BSSDK.FSUserHdrEx userHdr = (BSSDK.FSUserHdrEx)Marshal.PtrToStructure(m_userInfo, typeof(BSSDK.FSUserHdrEx));

            string filename = "c:\\temp\\profile.jpg";

            FileInfo fileDel = new FileInfo(filename);
            if (fileDel.Exists)
            {
                fileDel.Delete();
            }

            int nLength = userHdr.faceStillcutLen[nIndex];
            if( nLength > 0 )
            {
                byte[] imageBytes = new byte[nLength];
                System.Buffer.BlockCopy(m_StillcutData, nIndex * BS_MAX_IMAGE_SIZE, imageBytes, 0, nLength);
                SaveFaceImage(filename, imageBytes, nLength); 
            }
            else
            {
                pictureBox1.Image = null;
                pictureBox1.Invalidate();
                return;
            } 

            FileInfo fileShow = new FileInfo(filename);
            if (fileShow.Exists != true)
            {
                pictureBox1.Image = null;
                pictureBox1.Invalidate();
            }
            else
            {
                try
                {
                    Image image = Image.FromFile(filename);
                    pictureBox1.Image = image;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                    Image img = pictureBox1.Image;

                    // rotate image right 90'
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);

                    // set image size to 240, 320
                    Bitmap bmpMod = new Bitmap(240, 320);
                    Graphics g = Graphics.FromImage(bmpMod);
                    g.DrawImage(img, 0, 0, bmpMod.Width, bmpMod.Height);
                    g.Dispose();
                    pictureBox1.Image = bmpMod;
                    pictureBox1.Invalidate();
                }
                catch (Exception saveE)
                {
                    string str = saveE.Message;
                    pictureBox1.Image = null;
                    pictureBox1.Invalidate();
                    return;
                }
            }
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            int nCount = listBox1.Items.Count;
            if (nCount >= BS_FST_MAX_FACE_TYPE)
            {
                MessageBox.Show("Cannot add more face. Maximum face is up to 5.");
                return;
            }

            string strText = String.Format("{0,0} : template Num= {1,00} updated= {2,0}", nCount + 1, 0, 0);

            listBox1.Items.Add(strText);

            BSSDK.FSUserHdrEx m_userHdr = (BSSDK.FSUserHdrEx)Marshal.PtrToStructure(m_userInfo, typeof(BSSDK.FSUserHdrEx));
            int nLength = m_userHdr.faceStillcutLen[1];
            pictureBox1.Image = null;
            pictureBox1.Invalidate();
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            int nDeleteIndex = listBox1.SelectedIndex;
            if (nDeleteIndex < 0) return;

            pictureBox1.Image = null;
            pictureBox1.Invalidate();

            listBox1.Items.RemoveAt(nDeleteIndex);

            BSSDK.FSUserHdrEx m_userHdr = (BSSDK.FSUserHdrEx)Marshal.PtrToStructure(m_userInfo, typeof(BSSDK.FSUserHdrEx));
            int i = 0;
            for (i = nDeleteIndex; i < listBox1.Items.Count; i++)
            { 
                int nTemplateLen = 0;
                for (int k = 0; k < BS_FST_MAX_FACE_TEMPLATE; k++)
                {
                    nTemplateLen += m_userHdr.faceLen[(i + 1) * 25 + k];
                }

                Buffer.BlockCopy(m_FaceTemplate, (i + 1) * BS_FST_FACETEMPLATE_SIZE * BS_FST_MAX_FACE_TEMPLATE, m_FaceTemplate, i * BS_FST_FACETEMPLATE_SIZE * BS_FST_MAX_FACE_TEMPLATE, nTemplateLen); 
                Buffer.BlockCopy(m_StillcutData, (i + 1) * BS_MAX_IMAGE_SIZE, m_StillcutData, i * BS_MAX_IMAGE_SIZE, m_userHdr.faceStillcutLen[i + 1]);

                m_userHdr.numOfFace[i]        = m_userHdr.numOfFace[i+1]; 
		        m_userHdr.numOfUpdatedFace[i] = m_userHdr.numOfUpdatedFace[i+1];  
		        m_userHdr.faceUpdatedIndex[i] = m_userHdr.faceUpdatedIndex[i+1]; 
		        m_userHdr.faceStillcutLen[i]  = m_userHdr.faceStillcutLen[i+1];
                for (int k = 0; k < BS_FST_MAX_FACE_TEMPLATE; k++)
                {
                    m_userHdr.faceLen[i * 25 + k] = m_userHdr.faceLen[ (i+1)*25 + k];
                    m_userHdr.faceChecksum[i * 25 + k] = m_userHdr.faceChecksum[ (i+1)*25 + k];     
                } 

            }
            //initilize dummy facetemplate data 
	        for(  ;i < BS_FST_MAX_FACE_TYPE; i++ )  
	        {
		        m_userHdr.faceStillcutLen[i]  = 0;
                for (int k = 0; k < BS_FST_MAX_FACE_TEMPLATE; k++)
                {
                    m_userHdr.faceLen[i * 25 + k] = 0;
                    m_userHdr.faceChecksum[i * 25 + k] = 0;
                } 
	        }
            Marshal.StructureToPtr(m_userHdr, m_userInfo, true); 
            
            pictureBox1.Invalidate();
        }
    }
}