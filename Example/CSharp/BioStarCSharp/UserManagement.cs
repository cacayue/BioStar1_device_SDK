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
    public partial class UserManagement : Form
    {
        public const int TEMPLATE_SIZE = 384;
        public const int FACETEMPLATE_SIZE = 2284;
        public const int BS_MAX_TEMPLATE_PER_USER = 10;
        public const int BS_MAX_FACE_PER_USER = 5;
        public const int BS_MAX_IMAGE_SIZE = (100 * 1024);

        public const int BS_FST_MAX_FACE_TEMPLATE = 25;
        public const int BS_FST_FACETEMPLATE_SIZE = 2000;

        private int m_Handle = 0;
        private uint m_DeviceID = 0;
        private int m_DeviceType = -1;
        private int m_NumOfUser = 0;
        private int m_NumOfTemplate = 0;
        private byte[] m_TemplateData = null;       //for finger template
        private byte[] m_FaceTemplate = null;       //for DStation face template
        private byte[] m_FaceTemplate_FST = null;   //for FaceStation face template

        IntPtr m_userTemplateData = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.FSUserTemplateHdr)));
        
        public UserManagement()
        {
            m_TemplateData = new byte[TEMPLATE_SIZE * 2 * 2];
            m_FaceTemplate = new byte[FACETEMPLATE_SIZE * BS_MAX_FACE_PER_USER];
            m_FaceTemplate_FST = new byte[BS_FST_FACETEMPLATE_SIZE * BS_FST_MAX_FACE_TEMPLATE];

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

                        face_fst.Enabled = true;
                        checkScanFace.Enabled = true;
                        ScanFaceTemplate.Enabled = true; 
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

        private void UserManagement_Load(object sender, EventArgs e)
        {
            switch( m_DeviceType )
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
            m_NumOfTemplate = 0;

            userList.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            switch( m_DeviceType )
            {
                case BSSDK.BS_DEVICE_BIOENTRY_PLUS:
                case BSSDK.BS_DEVICE_BIOENTRY_W:
                case BSSDK.BS_DEVICE_BIOLITE:
                case BSSDK.BS_DEVICE_XPASS:
                case BSSDK.BS_DEVICE_XPASS_SLIM:
                case BSSDK.BS_DEVICE_XPASS_SLIM2:
                    {
                        result = BSSDK.BS_GetUserDBInfo(m_Handle, ref m_NumOfUser, ref m_NumOfTemplate);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot get user DB info", "Error");
                            return false;
                        }

                        numOfUser.Text = m_NumOfUser.ToString();
                        numOfTemplate.Text = m_NumOfTemplate.ToString();

                        BSSDK.BEUserHdr[] userHdr = new BSSDK.BEUserHdr[m_NumOfUser];
                        IntPtr userInfo = Marshal.AllocHGlobal(m_NumOfUser * Marshal.SizeOf(typeof(BSSDK.BEUserHdr)));

                        result = BSSDK.BS_GetAllUserInfoBEPlus(m_Handle, userInfo, ref m_NumOfUser);

                        if (result != BSSDK.BS_SUCCESS &&  result != BSSDK.BS_ERR_NOT_FOUND )
                        {
                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get user header info", "Error");
                            return false;
                        }

                        for (int i = 0; i < m_NumOfUser; i++)
                        {
                            userHdr[i] = (BSSDK.BEUserHdr)Marshal.PtrToStructure(new IntPtr(userInfo.ToInt32() + i * Marshal.SizeOf(typeof(BSSDK.BEUserHdr))), typeof(BSSDK.BEUserHdr));

                            ListViewItem userItem = userList.Items.Add(userHdr[i].userID.ToString());
                            userItem.SubItems.Add(userHdr[i].numOfFinger.ToString());
                            userItem.SubItems.Add(userHdr[i].cardID.ToString("X"));
                        }

                        Marshal.FreeHGlobal(userInfo);
                    }
                    break;

                case BSSDK.BS_DEVICE_BIOSTATION:
                    {
                        result = BSSDK.BS_GetUserDBInfo(m_Handle, ref m_NumOfUser, ref m_NumOfTemplate);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot get user DB info", "Error");
                            return false;
                        }

                        numOfUser.Text = m_NumOfUser.ToString();
                        numOfTemplate.Text = m_NumOfTemplate.ToString();


                        BSSDK.BSUserHdrEx[] userHdr = new BSSDK.BSUserHdrEx[m_NumOfUser];

                        IntPtr userInfo = Marshal.AllocHGlobal(m_NumOfUser * Marshal.SizeOf(typeof(BSSDK.BSUserHdrEx)));

                        result = BSSDK.BS_GetAllUserInfoEx(m_Handle, userInfo, ref m_NumOfUser);
                        if (result != BSSDK.BS_SUCCESS && result != BSSDK.BS_ERR_NOT_FOUND)
                        {
                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get user header info", "Error");
                            return false;
                        }

                        for (int i = 0; i < m_NumOfUser; i++)
                        {
                            userHdr[i] = (BSSDK.BSUserHdrEx)Marshal.PtrToStructure(new IntPtr(userInfo.ToInt32() + i * Marshal.SizeOf(typeof(BSSDK.BSUserHdrEx))), typeof(BSSDK.BSUserHdrEx));

                            ListViewItem userItem = userList.Items.Add(userHdr[i].ID.ToString());
                            userItem.SubItems.Add(userHdr[i].numOfFinger.ToString());
                            userItem.SubItems.Add(userHdr[i].cardID.ToString("X"));
                        }
                        Marshal.FreeHGlobal(userInfo);
                    }
                    break;

                case BSSDK.BS_DEVICE_DSTATION:
                  {
                        result = BSSDK.BS_GetUserDBInfo(m_Handle, ref m_NumOfUser, ref m_NumOfTemplate);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot get user DB info", "Error");
                            return false;
                        }

                        result = BSSDK.BS_GetUserFaceInfo(m_Handle, ref m_NumOfUser, ref m_NumOfTemplate);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot get user Face info", "Error");
                            return false;
                        }

                        numOfUser.Text = m_NumOfUser.ToString();
                        numOfTemplate.Text = m_NumOfTemplate.ToString();


                        BSSDK.DSUserHdr[] userHdr = new BSSDK.DSUserHdr[m_NumOfUser];

                        IntPtr userInfo = Marshal.AllocHGlobal(m_NumOfUser * Marshal.SizeOf(typeof(BSSDK.DSUserHdr)));

                        result = BSSDK.BS_GetAllUserInfoDStation(m_Handle, userInfo, ref m_NumOfUser);
                        if (result != BSSDK.BS_SUCCESS && result != BSSDK.BS_ERR_NOT_FOUND)
                        {
                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get user header info", "Error");
                            return false;
                        }

                        for (int i = 0; i < m_NumOfUser; i++)
                        {
                            userHdr[i] = (BSSDK.DSUserHdr)Marshal.PtrToStructure(new IntPtr(userInfo.ToInt32() + i * Marshal.SizeOf(typeof(BSSDK.DSUserHdr))), typeof(BSSDK.DSUserHdr));

                            ListViewItem userItem = userList.Items.Add(userHdr[i].ID.ToString());
                            userItem.SubItems.Add(userHdr[i].numOfFinger.ToString());
                            userItem.SubItems.Add(userHdr[i].cardID.ToString("X"));
                        }

                        Marshal.FreeHGlobal(userInfo);
                    }
                    break;

                case BSSDK.BS_DEVICE_XSTATION:
                    {
                        result = BSSDK.BS_GetUserDBInfo(m_Handle, ref m_NumOfUser, ref m_NumOfTemplate);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot get user DB info", "Error");
                            return false;
                        }

                        numOfUser.Text = m_NumOfUser.ToString();
                        numOfTemplate.Text = m_NumOfTemplate.ToString();


                        BSSDK.XSUserHdr[] userHdr = new BSSDK.XSUserHdr[m_NumOfUser];

                        IntPtr userInfo = Marshal.AllocHGlobal(m_NumOfUser * Marshal.SizeOf(typeof(BSSDK.XSUserHdr)));

                        result = BSSDK.BS_GetAllUserInfoXStation(m_Handle, userInfo, ref m_NumOfUser);
                        if (result != BSSDK.BS_SUCCESS && result != BSSDK.BS_ERR_NOT_FOUND)
                        {
                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get user header info", "Error");
                            return false;
                        }

                        for (int i = 0; i < m_NumOfUser; i++)
                        {
                            userHdr[i] = (BSSDK.XSUserHdr)Marshal.PtrToStructure(new IntPtr(userInfo.ToInt32() + i * Marshal.SizeOf(typeof(BSSDK.XSUserHdr))), typeof(BSSDK.XSUserHdr));

                            ListViewItem userItem = userList.Items.Add(userHdr[i].ID.ToString());
                            userItem.SubItems.Add(userHdr[i].numOfFinger.ToString());
                            userItem.SubItems.Add(userHdr[i].cardID.ToString("X"));
                        }
                        Marshal.FreeHGlobal(userInfo);
                    }
                    break;

                case BSSDK.BS_DEVICE_BIOSTATION2:
                    {
                        result = BSSDK.BS_GetUserDBInfo(m_Handle, ref m_NumOfUser, ref m_NumOfTemplate);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot get user DB info", "Error");
                            return false;
                        }

                        numOfUser.Text = m_NumOfUser.ToString();
                        numOfTemplate.Text = m_NumOfTemplate.ToString();


                        BSSDK.BS2UserHdr[] userHdr = new BSSDK.BS2UserHdr[m_NumOfUser];

                        IntPtr userInfo = Marshal.AllocHGlobal(m_NumOfUser * Marshal.SizeOf(typeof(BSSDK.BS2UserHdr)));

                        result = BSSDK.BS_GetAllUserInfoBioStation2(m_Handle, userInfo, ref m_NumOfUser);
                        if (result != BSSDK.BS_SUCCESS && result != BSSDK.BS_ERR_NOT_FOUND)
                        {
                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get user header info", "Error");
                            return false;
                        }

                        for (int i = 0; i < m_NumOfUser; i++)
                        {
                            userHdr[i] = (BSSDK.BS2UserHdr)Marshal.PtrToStructure(new IntPtr(userInfo.ToInt32() + i * Marshal.SizeOf(typeof(BSSDK.BS2UserHdr))), typeof(BSSDK.BS2UserHdr));

                            ListViewItem userItem = userList.Items.Add(userHdr[i].ID.ToString());
                            userItem.SubItems.Add(userHdr[i].numOfFinger.ToString());
                            userItem.SubItems.Add(userHdr[i].cardID.ToString("X"));
                        }

                        Marshal.FreeHGlobal(userInfo);
                    }
                    break;

                case BSSDK.BS_DEVICE_FSTATION:
                    {
                        result = BSSDK.BS_GetUserDBInfo(m_Handle, ref m_NumOfUser, ref m_NumOfTemplate);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot get user DB info", "Error");
                            return false;
                        }

                        numOfUser.Text = m_NumOfUser.ToString();
                        numOfTemplate.Text = m_NumOfTemplate.ToString();


                        BSSDK.FSUserHdr[] userHdr = new BSSDK.FSUserHdr[m_NumOfUser];

                        IntPtr userInfo = Marshal.AllocHGlobal(m_NumOfUser * Marshal.SizeOf(typeof(BSSDK.FSUserHdr)));

                        result = BSSDK.BS_GetAllUserInfoFStation(m_Handle, userInfo, ref m_NumOfUser);
                        if (result != BSSDK.BS_SUCCESS && result != BSSDK.BS_ERR_NOT_FOUND)
                        {
                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get user header info", "Error");
                            return false;
                        }

                        for (int i = 0; i < m_NumOfUser; i++)
                        {
                            userHdr[i] = (BSSDK.FSUserHdr)Marshal.PtrToStructure(new IntPtr(userInfo.ToInt32() + i * Marshal.SizeOf(typeof(BSSDK.FSUserHdr))), typeof(BSSDK.FSUserHdr));

                            ListViewItem userItem = userList.Items.Add(userHdr[i].ID.ToString());
                            userItem.SubItems.Add(userHdr[i].numOfFace.ToString());
                            userItem.SubItems.Add(userHdr[i].cardID.ToString("X"));
                        }

                        Marshal.FreeHGlobal(userInfo);
                    }
                    break;
            }

            Cursor.Current = Cursors.Default;

            if( m_NumOfUser > 0 )
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
            finger1.Checked = false;
            duress1.Checked = false;
            checksum1.Text = "";

            finger2.Checked = false;
            duress2.Checked = false;
            checksum2.Text = "";
            
            Cursor.Current = Cursors.WaitCursor;

            switch (m_DeviceType)
            {
                case BSSDK.BS_DEVICE_BIOENTRY_PLUS:
                case BSSDK.BS_DEVICE_BIOENTRY_W:
                case BSSDK.BS_DEVICE_BIOLITE:
                case BSSDK.BS_DEVICE_XPASS:
                case BSSDK.BS_DEVICE_XPASS_SLIM:
                case BSSDK.BS_DEVICE_XPASS_SLIM2:
                    {
                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BEUserHdr)));

                        int result = BSSDK.BS_GetUserBEPlus(m_Handle, ID, userInfo, m_TemplateData);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get user info", "Error");
                            return;
                        }

                        BSSDK.BEUserHdr userHdr = (BSSDK.BEUserHdr)Marshal.PtrToStructure(userInfo, typeof(BSSDK.BEUserHdr));

                        Marshal.FreeHGlobal(userInfo);

                        userID.Text = userHdr.userID.ToString();
                        userCardID.Text = userHdr.cardID.ToString("X");
                        cardCustomID.Text = userHdr.cardCustomID.ToString();
                        userLevel.SelectedIndex = userHdr.adminLevel;
                        securityLevel.SelectedIndex = userHdr.securityLevel;
                        cardType.SelectedIndex = userHdr.cardFlag;

                        name.Text = "";
                        password.Text = Encoding.Unicode.GetString(userHdr.password);

                        accessGroup.Text = userHdr.accessGroupMask.ToString("X");

                        if (userHdr.opMode >= BSSDK.BS_AUTH_FINGER_ONLY && userHdr.opMode <= BSSDK.BS_AUTH_CARD_ONLY)
                            authMode.SelectedIndex = userHdr.opMode - BSSDK.BS_AUTH_FINGER_ONLY + 1;
                        else
                            authMode.SelectedIndex = 0;

                        startDate.Value = new DateTime(1970, 1, 1).AddSeconds(userHdr.startTime);
                        expiryDate.Value = new DateTime(1970, 1, 1).AddSeconds(userHdr.expiryTime);

                        if( userHdr.numOfFinger > 0 ) 
                        {
                            finger1.Checked = true;
                            duress1.Checked = (userHdr.isDuress[0] == 1);
                            checksum1.Text = userHdr.fingerChecksum[0].ToString();
                        }
                        else 
                        {
                            finger1.Checked = false;
                            duress1.Checked = false;
                            checksum1.Text = "";
                        }

                        if( userHdr.numOfFinger > 1 )
                        {
                            finger2.Checked = true;
                            duress2.Checked = (userHdr.isDuress[1] == 1);
                            checksum2.Text = userHdr.fingerChecksum[1].ToString();
                        }
                        else
                        {
                            finger2.Checked = false;
                            duress2.Checked = false;
                            checksum2.Text = "";
                        }
                    }
                    break;

                case BSSDK.BS_DEVICE_BIOSTATION:
                    {
                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BSUserHdrEx)));

                        int result = BSSDK.BS_GetUserEx(m_Handle, ID, userInfo, m_TemplateData);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Cursor.Current = Cursors.Default;

                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get user info", "Error");
                            return;
                        }

                        BSSDK.BSUserHdrEx userHdr = (BSSDK.BSUserHdrEx)Marshal.PtrToStructure(userInfo, typeof(BSSDK.BSUserHdrEx));

                        Marshal.FreeHGlobal(userInfo);

                        userID.Text = userHdr.ID.ToString();
                        userCardID.Text = userHdr.cardID.ToString("X");
                        cardCustomID.Text = userHdr.customID.ToString();
                        userLevel.SelectedIndex = (userHdr.adminLevel == BSSDK.BS_USER_ADMIN) ? 1 : 0;
                        securityLevel.SelectedIndex = (userHdr.securityLevel >= BSSDK.BS_USER_SECURITY_DEFAULT) ? userHdr.securityLevel - BSSDK.BS_USER_SECURITY_DEFAULT : 0;
                        cardType.SelectedIndex = userHdr.bypassCard;

                        name.Text = Encoding.ASCII.GetString(userHdr.name);
                        password.Text = Encoding.ASCII.GetString(userHdr.password);

                        accessGroup.Text = userHdr.accessGroupMask.ToString("X");

                        if (userHdr.authMode >= BSSDK.BS_AUTH_FINGER_ONLY && userHdr.authMode <= BSSDK.BS_AUTH_CARD_ONLY)
                            authMode.SelectedIndex = userHdr.authMode - BSSDK.BS_AUTH_FINGER_ONLY + 1;
                        else
                            authMode.SelectedIndex = 0;

                        startDate.Value = new DateTime(1970, 1, 1).AddSeconds(userHdr.startDateTime);
                        expiryDate.Value = new DateTime(1970, 1, 1).AddSeconds(userHdr.expireDateTime);

                        if (userHdr.numOfFinger > 0)
                        {
                            finger1.Checked = true;
                            duress1.Checked = (userHdr.duressMask & 0x01) == 0x01;
                            checksum1.Text = userHdr.checksum[0].ToString();
                        }
                        else
                        {
                            finger1.Checked = false;
                            duress1.Checked = false;
                            checksum1.Text = "";
                        }

                        if (userHdr.numOfFinger > 1)
                        {
                            finger2.Checked = true;
                            duress2.Checked = (userHdr.duressMask & 0x02) == 0x02;
                            checksum2.Text = userHdr.checksum[1].ToString();
                        }
                        else
                        {
                            finger2.Checked = false;
                            duress2.Checked = false;
                            checksum2.Text = "";
                        }
                    }
                    break;

                case BSSDK.BS_DEVICE_DSTATION:
                    {
                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.DSUserHdr)));

                        int result = BSSDK.BS_GetUserDStation(m_Handle, ID, userInfo, m_TemplateData, m_FaceTemplate);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Cursor.Current = Cursors.Default;

                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get user info", "Error");
                            return;
                        }

                        BSSDK.DSUserHdr userHdr = (BSSDK.DSUserHdr)Marshal.PtrToStructure(userInfo, typeof(BSSDK.DSUserHdr));

                        Marshal.FreeHGlobal(userInfo);

                        userID.Text = userHdr.ID.ToString();
                        userCardID.Text = userHdr.cardID.ToString("X");
                        cardCustomID.Text = userHdr.customID.ToString();
                        userLevel.SelectedIndex = (userHdr.adminLevel == (ushort)BSSDK.DSUserHdr.ENUM.USER_ADMIN) ? 1 : 0;
                        securityLevel.SelectedIndex = (userHdr.securityLevel >= BSSDK.BS_USER_SECURITY_DEFAULT) ? userHdr.securityLevel - BSSDK.BS_USER_SECURITY_DEFAULT : 0;
                        cardType.SelectedIndex = userHdr.bypassCard;

                        byte[] asBytes = new byte[userHdr.name.Length * sizeof(ushort)];
                        Buffer.BlockCopy(userHdr.name, 0, asBytes, 0, asBytes.Length);
                        name.Text = Encoding.Unicode.GetString(asBytes);

                        byte[] pwdBytes = new byte[userHdr.password.Length * sizeof(ushort)];
                        Buffer.BlockCopy(userHdr.password, 0, pwdBytes, 0, pwdBytes.Length);
                        password.Text = Encoding.Unicode.GetString(pwdBytes);

                        accessGroup.Text = userHdr.accessGroupMask.ToString("X");

                        if (userHdr.authMode >= BSSDK.BS_AUTH_FINGER_ONLY && userHdr.authMode <= BSSDK.BS_AUTH_CARD_ONLY)
                            authMode.SelectedIndex = userHdr.authMode - BSSDK.BS_AUTH_FINGER_ONLY + 1;
                        else
                            authMode.SelectedIndex = 0;

                        startDate.Value = new DateTime(1970, 1, 1).AddSeconds(userHdr.startDateTime);
                        expiryDate.Value = new DateTime(1970, 1, 1).AddSeconds(userHdr.expireDateTime);

                        if (userHdr.numOfFinger > 0)
                        {
                            finger1.Checked = true;
                            duress1.Checked = (userHdr.duress[0] == 0x01) ? true : false;
                            checksum1.Text = userHdr.fingerChecksum[0].ToString();
                        }
                        else
                        {
                            finger1.Checked = false;
                            duress1.Checked = false;
                            checksum1.Text = "";
                        }

                        if (userHdr.numOfFinger > 1)
                        {
                            finger2.Checked = true;
                            duress2.Checked = (userHdr.duress[1] == 0x01) ? true : false;
                            checksum2.Text = userHdr.fingerChecksum[1].ToString();
                        }
                        else
                        {
                            finger2.Checked = false;
                            duress2.Checked = false;
                            checksum2.Text = "";
                        }

                        if (userHdr.numOfFace > 0)
                        {
                            face.Checked = true;
                            //face Checksum
                            uint cs = userHdr.faceChecksum[0];
                            faceChecksum.Text = cs.ToString();
                        }
                        else
                        {
                            face.Checked = false;
                            faceChecksum.Text = ""; 
                        }

                    }
                    break;

                case BSSDK.BS_DEVICE_XSTATION:
                    {
                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.XSUserHdr)));

                        int result = BSSDK.BS_GetUserXStation(m_Handle, ID, userInfo);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Cursor.Current = Cursors.Default;

                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get user info", "Error");
                            return;
                        }

                        BSSDK.XSUserHdr userHdr = (BSSDK.XSUserHdr)Marshal.PtrToStructure(userInfo, typeof(BSSDK.XSUserHdr));

                        Marshal.FreeHGlobal(userInfo);

                        userID.Text = userHdr.ID.ToString();
                        userCardID.Text = userHdr.cardID.ToString("X");
                        cardCustomID.Text = userHdr.customID.ToString();
                        userLevel.SelectedIndex = (userHdr.adminLevel == (ushort)BSSDK.XSUserHdr.ENUM.USER_ADMIN) ? 1 : 0;
                        securityLevel.SelectedIndex = (userHdr.securityLevel >= BSSDK.BS_USER_SECURITY_DEFAULT) ? userHdr.securityLevel - BSSDK.BS_USER_SECURITY_DEFAULT : 0;
                        cardType.SelectedIndex = userHdr.bypassCard;

                        byte[] asBytes = new byte[userHdr.name.Length * sizeof(ushort)];
                        Buffer.BlockCopy(userHdr.name, 0, asBytes, 0, asBytes.Length);
                        name.Text = Encoding.Unicode.GetString(asBytes);

                        byte[] pwdBytes = new byte[userHdr.password.Length * sizeof(ushort)];
                        Buffer.BlockCopy(userHdr.password, 0, pwdBytes, 0, pwdBytes.Length);
                        password.Text = Encoding.Unicode.GetString(pwdBytes);

                        accessGroup.Text = userHdr.accessGroupMask.ToString("X");

                        if (userHdr.authMode >= BSSDK.BS_AUTH_FINGER_ONLY && userHdr.authMode <= BSSDK.BS_AUTH_CARD_ONLY)
                            authMode.SelectedIndex = userHdr.authMode - BSSDK.BS_AUTH_FINGER_ONLY + 1;
                        else
                            authMode.SelectedIndex = 0;

                        startDate.Value = new DateTime(1970, 1, 1).AddSeconds(userHdr.startDateTime);
                        expiryDate.Value = new DateTime(1970, 1, 1).AddSeconds(userHdr.expireDateTime);
                    }
                    break;

                case BSSDK.BS_DEVICE_BIOSTATION2:
                    {
                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BS2UserHdr)));

                        int result = BSSDK.BS_GetUserBioStation2(m_Handle, ID, userInfo, m_TemplateData);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Cursor.Current = Cursors.Default;

                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get user info", "Error");
                            return;
                        }

                        BSSDK.BS2UserHdr userHdr = (BSSDK.BS2UserHdr)Marshal.PtrToStructure(userInfo, typeof(BSSDK.BS2UserHdr));

                        Marshal.FreeHGlobal(userInfo);

                        userID.Text = userHdr.ID.ToString();
                        userCardID.Text = userHdr.cardID.ToString("X");
                        cardCustomID.Text = userHdr.customID.ToString();
                        userLevel.SelectedIndex = (userHdr.adminLevel == (ushort)BSSDK.BS2UserHdr.ENUM.USER_ADMIN) ? 1 : 0;
                        securityLevel.SelectedIndex = (userHdr.securityLevel >= BSSDK.BS_USER_SECURITY_DEFAULT) ? userHdr.securityLevel - BSSDK.BS_USER_SECURITY_DEFAULT : 0;
                        cardType.SelectedIndex = userHdr.bypassCard;

                        byte[] nameBytes = new byte[userHdr.name.Length * sizeof(ushort)];
                        Buffer.BlockCopy(userHdr.name, 0, nameBytes, 0, nameBytes.Length);
                        name.Text = Encoding.Unicode.GetString(nameBytes);


                        byte[] pwdBytes = new byte[userHdr.password.Length * sizeof(ushort)];
                        Buffer.BlockCopy(userHdr.password, 0, pwdBytes, 0, pwdBytes.Length);
                        password.Text = Encoding.Unicode.GetString(pwdBytes);

                        accessGroup.Text = userHdr.accessGroupMask.ToString("X");

                        if (userHdr.authMode >= BSSDK.BS_AUTH_FINGER_ONLY && userHdr.authMode <= BSSDK.BS_AUTH_CARD_ONLY)
                            authMode.SelectedIndex = userHdr.authMode - BSSDK.BS_AUTH_FINGER_ONLY + 1;
                        else
                            authMode.SelectedIndex = 0;

                        startDate.Value = new DateTime(1970, 1, 1).AddSeconds(userHdr.startDateTime);
                        expiryDate.Value = new DateTime(1970, 1, 1).AddSeconds(userHdr.expireDateTime);

                        if (userHdr.numOfFinger > 0)
                        {
                            finger1.Checked = true;
                            duress1.Checked = (userHdr.duress[0] == 0x01) ? true : false;
                            checksum1.Text = userHdr.fingerChecksum[0].ToString();
                        }
                        else
                        {
                            finger1.Checked = false;
                            duress1.Checked = false;
                            checksum1.Text = "";
                        }

                        if (userHdr.numOfFinger > 1)
                        {
                            finger2.Checked = true;
                            duress2.Checked = (userHdr.duress[1] == 0x01) ? true : false;
                            checksum2.Text = userHdr.fingerChecksum[1].ToString();
                        }
                        else
                        {
                            finger2.Checked = false;
                            duress2.Checked = false;
                            checksum2.Text = "";
                        }
                    }
                    break;

                case BSSDK.BS_DEVICE_FSTATION:
                    {
                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.FSUserHdr)));

                        int result = BSSDK.BS_GetUserFStation(m_Handle, ID, userInfo, m_FaceTemplate_FST);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Cursor.Current = Cursors.Default;

                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get user info", "Error");
                            return;
                        }

                        BSSDK.FSUserHdr userHdr = (BSSDK.FSUserHdr)Marshal.PtrToStructure(userInfo, typeof(BSSDK.FSUserHdr));

                        Marshal.FreeHGlobal(userInfo);

                        userID.Text = userHdr.ID.ToString();
                        userCardID.Text = userHdr.cardID.ToString("X");
                        cardCustomID.Text = userHdr.customID.ToString();
                        userLevel.SelectedIndex = (userHdr.adminLevel == (ushort)BSSDK.FSUserHdr.ENUM.USER_ADMIN) ? 1 : 0;
                        securityLevel.SelectedIndex = (userHdr.securityLevel >= BSSDK.BS_USER_SECURITY_DEFAULT) ? userHdr.securityLevel - BSSDK.BS_USER_SECURITY_DEFAULT : 0;
                        cardType.SelectedIndex = userHdr.bypassCard;

                        byte[] nameBytes = new byte[userHdr.name.Length * sizeof(ushort)];
                        Buffer.BlockCopy(userHdr.name, 0, nameBytes, 0, nameBytes.Length);
                        name.Text = Encoding.Unicode.GetString(nameBytes);


                        byte[] pwdBytes = new byte[userHdr.password.Length * sizeof(ushort)];
                        Buffer.BlockCopy(userHdr.password, 0, pwdBytes, 0, pwdBytes.Length);
                        password.Text = Encoding.Unicode.GetString(pwdBytes);

                        accessGroup.Text = userHdr.accessGroupMask.ToString("X");

                        authMode.SelectedIndex = userHdr.authMode;

                        startDate.Value = new DateTime(1970, 1, 1).AddSeconds(userHdr.startDateTime);
                        expiryDate.Value = new DateTime(1970, 1, 1).AddSeconds(userHdr.expireDateTime);
 
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
            if( userList.SelectedItems.Count == 0)
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

        private void updateUser_Click(object sender, EventArgs e)
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

            switch (m_DeviceType)
            {
                case BSSDK.BS_DEVICE_BIOENTRY_PLUS:
                case BSSDK.BS_DEVICE_BIOENTRY_W:
                case BSSDK.BS_DEVICE_BIOLITE:
                case BSSDK.BS_DEVICE_XPASS:
                case BSSDK.BS_DEVICE_XPASS_SLIM:
                case BSSDK.BS_DEVICE_XPASS_SLIM2:
                    {
                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BEUserHdr)));

                        int result = BSSDK.BS_GetUserBEPlus(m_Handle, ID, userInfo, m_TemplateData);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Cursor.Current = Cursors.Default;
                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get the user", "Error");
                            return;
                        }

                        BSSDK.BEUserHdr userHdr = (BSSDK.BEUserHdr)Marshal.PtrToStructure(userInfo, typeof(BSSDK.BEUserHdr));

                        userHdr.adminLevel = (ushort)userLevel.SelectedIndex;
                        userHdr.securityLevel = (ushort)securityLevel.SelectedIndex;
                        userHdr.cardFlag = (byte)cardType.SelectedIndex;
                        userHdr.startTime = (int)((startDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                        userHdr.expiryTime = (int)((expiryDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                        userHdr.isDuress[0] = (duress1.Checked) ? (byte)1 : (byte)0;
                        userHdr.isDuress[1] = (duress2.Checked) ? (byte)1 : (byte)0;
                        userHdr.opMode = (ushort)authMode.SelectedIndex;

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
                            userHdr.cardCustomID = (byte)Int32.Parse(cardCustomID.Text);
                        }
                        catch (Exception)
                        {
                            userHdr.cardCustomID = 0;
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

                        result = BSSDK.BS_EnrollUserBEPlus(m_Handle, userInfo, m_TemplateData);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot enroll the user", "Error");
                        }

                        Marshal.FreeHGlobal(userInfo);
                    }
                    break;

                case BSSDK.BS_DEVICE_BIOSTATION:
                    {
                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BSUserHdrEx)));

                        int result = BSSDK.BS_GetUserEx(m_Handle, ID, userInfo, m_TemplateData);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Cursor.Current = Cursors.Default;
                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get the user", "Error");
                            return;
                        }

                        BSSDK.BSUserHdrEx userHdr = (BSSDK.BSUserHdrEx)Marshal.PtrToStructure(userInfo, typeof(BSSDK.BSUserHdrEx));

                        userHdr.adminLevel = (ushort)((userLevel.SelectedIndex == 1) ? BSSDK.BS_USER_ADMIN : BSSDK.BS_USER_NORMAL);
                        userHdr.securityLevel = (ushort)(securityLevel.SelectedIndex + BSSDK.BS_USER_SECURITY_DEFAULT);

                        userHdr.bypassCard = (byte)cardType.SelectedIndex;
                        userHdr.startDateTime = (uint)((startDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                        userHdr.expireDateTime = (uint)((expiryDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);

                        if (authMode.SelectedIndex == 0)
                            userHdr.authMode = 0;
                        else
                            userHdr.authMode = (ushort)(authMode.SelectedIndex + BSSDK.BS_AUTH_FINGER_ONLY - 1);

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

                        result = BSSDK.BS_EnrollUserEx(m_Handle, userInfo, m_TemplateData);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot enroll the user", "Error");
                        }

                        Marshal.FreeHGlobal(userInfo);
                    }
                    break;

                case BSSDK.BS_DEVICE_DSTATION:
                    {
                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.DSUserHdr)));

                        int result = BSSDK.BS_GetUserDStation(m_Handle, ID, userInfo, m_TemplateData, m_FaceTemplate);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Cursor.Current = Cursors.Default;
                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get the user", "Error");
                            return;
                        }

                        BSSDK.DSUserHdr userHdr = (BSSDK.DSUserHdr)Marshal.PtrToStructure(userInfo, typeof(BSSDK.DSUserHdr));


                        userHdr.adminLevel = (ushort)((userLevel.SelectedIndex == 1) ? BSSDK.DSUserHdr.ENUM.USER_ADMIN : BSSDK.DSUserHdr.ENUM.USER_NORMAL);
                        userHdr.securityLevel = (ushort)(securityLevel.SelectedIndex + BSSDK.BS_USER_SECURITY_DEFAULT);

                        userHdr.bypassCard = (byte)cardType.SelectedIndex;
                        userHdr.startDateTime = (uint)((startDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                        userHdr.expireDateTime = (uint)((expiryDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);

                        if (authMode.SelectedIndex == 0)
                            userHdr.authMode = 0;
                        else
                            userHdr.authMode = (ushort)(authMode.SelectedIndex + BSSDK.BS_AUTH_FINGER_ONLY - 1);

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

                        result = BSSDK.BS_EnrollUserDStation(m_Handle, userInfo, m_TemplateData, m_FaceTemplate);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot enroll the user", "Error");
                        }

                        Marshal.FreeHGlobal(userInfo);
                    }
                    break;

                case BSSDK.BS_DEVICE_XSTATION:
                    {
                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.XSUserHdr)));

                        int result = BSSDK.BS_GetUserXStation(m_Handle, ID, userInfo);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Cursor.Current = Cursors.Default;
                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get the user", "Error");
                            return;
                        }

                        BSSDK.XSUserHdr userHdr = (BSSDK.XSUserHdr)Marshal.PtrToStructure(userInfo, typeof(BSSDK.XSUserHdr));

                        userHdr.adminLevel = (ushort)((userLevel.SelectedIndex == 1) ? BSSDK.XSUserHdr.ENUM.USER_ADMIN : BSSDK.XSUserHdr.ENUM.USER_NORMAL);
                        userHdr.securityLevel = (ushort)(securityLevel.SelectedIndex + BSSDK.BS_USER_SECURITY_DEFAULT);

                        userHdr.bypassCard = (byte)cardType.SelectedIndex;
                        userHdr.startDateTime = (uint)((startDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                        userHdr.expireDateTime = (uint)((expiryDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);

                        if (authMode.SelectedIndex == 0)
                            userHdr.authMode = 0;
                        else
                            userHdr.authMode = (ushort)(authMode.SelectedIndex + BSSDK.BS_AUTH_FINGER_ONLY - 1);

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

                        result = BSSDK.BS_EnrollUserXStation(m_Handle, userInfo);
                         if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot enroll the user", "Error");
                        }

                        Marshal.FreeHGlobal(userInfo);
                    }
                    break;

                case BSSDK.BS_DEVICE_BIOSTATION2:
                    {
                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BS2UserHdr)));

                        int result = BSSDK.BS_GetUserBioStation2(m_Handle, ID, userInfo, m_TemplateData);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Cursor.Current = Cursors.Default;
                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get the user", "Error");
                            return;
                        }

                        BSSDK.BS2UserHdr userHdr = (BSSDK.BS2UserHdr)Marshal.PtrToStructure(userInfo, typeof(BSSDK.BS2UserHdr));

                        userHdr.adminLevel = (ushort)((userLevel.SelectedIndex == 1) ? BSSDK.BS2UserHdr.ENUM.USER_ADMIN : BSSDK.BS2UserHdr.ENUM.USER_NORMAL);
                        userHdr.securityLevel = (ushort)(securityLevel.SelectedIndex + BSSDK.BS_USER_SECURITY_DEFAULT);

                        userHdr.bypassCard = (byte)cardType.SelectedIndex;
                        userHdr.startDateTime = (uint)((startDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                        userHdr.expireDateTime = (uint)((expiryDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);

                        if (authMode.SelectedIndex == 0)
                            userHdr.authMode = 0;
                        else
                            userHdr.authMode = (ushort)(authMode.SelectedIndex + BSSDK.BS_AUTH_FINGER_ONLY - 1);

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

                        result = BSSDK.BS_EnrollUserBioStation2(m_Handle, userInfo, m_TemplateData);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot enroll the user", "Error");
                        }

                        Marshal.FreeHGlobal(userInfo);
                    }
                    break;

                case BSSDK.BS_DEVICE_FSTATION:
                    {
                        byte[] faceTemplate = new byte[BS_FST_FACETEMPLATE_SIZE * BS_FST_MAX_FACE_TEMPLATE];
                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.FSUserHdr)));

                        int result = BSSDK.BS_GetUserFStation(m_Handle, ID, userInfo, faceTemplate);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            Cursor.Current = Cursors.Default;
                            Marshal.FreeHGlobal(userInfo);
                            MessageBox.Show("Cannot get the user", "Error");
                            return;
                        }

                        BSSDK.FSUserHdr userHdr = (BSSDK.FSUserHdr)Marshal.PtrToStructure(userInfo, typeof(BSSDK.FSUserHdr));

                        if (checkScanFace.Checked == true)
                        {
                            BSSDK.FSUserTemplateHdr userTemplateHdr = (BSSDK.FSUserTemplateHdr)Marshal.PtrToStructure(m_userTemplateData, typeof(BSSDK.FSUserTemplateHdr));
                            //face num
                            userHdr.numOfFace = (ushort)userTemplateHdr.numOfFace;
                            userHdr.numOfUpdatedFace = (ushort)userTemplateHdr.numOfUpdatedFace;

                            // face template's length
                            for (int i = 0; i < BS_FST_MAX_FACE_TEMPLATE; i++)
                            {
                                userHdr.faceLen[i] = userTemplateHdr.faceLen[i];
                            }

                            // face template's checksum
                            int offset = 0;
                            for (int i = 0; i < BS_FST_MAX_FACE_TEMPLATE; i++)
                            {
                                int nLen = userTemplateHdr.faceLen[i];
                                if (nLen > 0)
                                {
                                    byte[] templateData = new byte[nLen];
                                    Buffer.BlockCopy(m_FaceTemplate_FST, offset, templateData, 0, nLen);

                                    for (int j = 0; j < userTemplateHdr.faceLen[i]; j++)
                                    {
                                        userHdr.faceChecksum[i] += templateData[j];
                                    }
                                    offset += nLen;
                                }
                            }
                            //

                            // face temp data
                            Buffer.BlockCopy(userTemplateHdr.faceTemp, 0, userHdr.faceTemp, 0, 256);

                        }

                        // name 
                        string username = name.Text;
                        byte[] nameBytes = Encoding.Unicode.GetBytes(username);             // UTF16
                        Buffer.BlockCopy(nameBytes, 0, userHdr.name, 0, nameBytes.Length);

                        // pwd
                        string pwd = password.Text;
                        byte[] pwdBytes = Encoding.Unicode.GetBytes(pwd);
                        Buffer.BlockCopy(pwdBytes, 0, userHdr.password, 0, pwdBytes.Length);


                        userHdr.adminLevel = (ushort)((userLevel.SelectedIndex == 1) ? BSSDK.FSUserHdr.ENUM.USER_ADMIN : BSSDK.FSUserHdr.ENUM.USER_NORMAL);
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

                        result = BSSDK.BS_EnrollUserFStation(m_Handle, userInfo, m_FaceTemplate_FST);
                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot enroll the user", "Error");
                        }

                        Marshal.FreeHGlobal(userInfo);
                    }
                    break;
            }

            Cursor.Current = Cursors.Default;
        }

        private void addUser_Click(object sender, EventArgs e)
        {
            uint ID = 0;

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


            int result = 0;
            ushort fingerChecksum1 = 0;
            ushort fingerChecksum2 = 0;

            int numOfFinger = 0;

            if( finger1.Checked == true )
            {
                switch (m_DeviceType)
                {
                    case BSSDK.BS_DEVICE_BIOSTATION:
                    case BSSDK.BS_DEVICE_BIOENTRY_PLUS:
                    case BSSDK.BS_DEVICE_BIOENTRY_W:
                    case BSSDK.BS_DEVICE_BIOLITE:
                    case BSSDK.BS_DEVICE_DSTATION:
                    case BSSDK.BS_DEVICE_BIOSTATION2:
                        {
                            byte[] templateData = new byte[TEMPLATE_SIZE * BS_MAX_TEMPLATE_PER_USER];

                            numOfFinger++;

                            Cursor.Current = Cursors.WaitCursor;
                            result = BSSDK.BS_ScanTemplate(m_Handle, templateData);
                            Cursor.Current = Cursors.Default;

                            if (result != BSSDK.BS_SUCCESS) 
                            {
                                MessageBox.Show("Cannot scan the finger", "Error"); 
                                return;
                            }

                            Buffer.BlockCopy(templateData, 0, m_TemplateData, 0, TEMPLATE_SIZE);

                            Cursor.Current = Cursors.WaitCursor;
                            result = BSSDK.BS_ScanTemplate(m_Handle, templateData);
                            Cursor.Current = Cursors.Default;

                            if (result != BSSDK.BS_SUCCESS) 
                            {
                                MessageBox.Show("Cannot scan the finger", "Error");
                                return;
                            }

                            Buffer.BlockCopy(templateData, 0, m_TemplateData, TEMPLATE_SIZE, TEMPLATE_SIZE);

                            for (int i = 0; i < TEMPLATE_SIZE; i++)
                            {
                                fingerChecksum1 += m_TemplateData[i];
                            }

                            checksum1.Text = fingerChecksum1.ToString();


                            if (finger2.Checked == true)
                            {
                                numOfFinger++;

                                Cursor.Current = Cursors.WaitCursor;
                                result = BSSDK.BS_ScanTemplate(m_Handle, templateData);
                                Cursor.Current = Cursors.Default;

                                if (result != BSSDK.BS_SUCCESS)
                                {
                                    MessageBox.Show("Cannot scan the finger", "Error");
                                    return;
                                }

                                Buffer.BlockCopy(templateData, 0, m_TemplateData, TEMPLATE_SIZE * 2, TEMPLATE_SIZE);

                                Cursor.Current = Cursors.WaitCursor;
                                result = BSSDK.BS_ScanTemplate(m_Handle, templateData);
                                Cursor.Current = Cursors.Default;

                                if (result != BSSDK.BS_SUCCESS)
                                {
                                    MessageBox.Show("Cannot scan the finger", "Error");
                                    return;
                                }

                                Buffer.BlockCopy(templateData, 0, m_TemplateData, TEMPLATE_SIZE * 3, TEMPLATE_SIZE);

                                for (int i = 0; i < TEMPLATE_SIZE; i++)
                                {
                                    fingerChecksum2 += m_TemplateData[TEMPLATE_SIZE * 2 + i];
                                }

                                checksum2.Text = fingerChecksum2.ToString();
                            }
                        }
                        break;
                }
            }

            // D-Station Only
	        if( face.Checked == true )
	        {
 		        switch( m_DeviceType )
                {
                    case BSSDK.BS_DEVICE_DSTATION:
		                {
			                int imageLen = 0;
                            DialogResult nRet = DialogResult.Yes;
                            byte[] imageData = new byte[BS_MAX_IMAGE_SIZE];
                            byte[] faceTemplate = new byte[FACETEMPLATE_SIZE * BS_MAX_FACE_PER_USER];

                            do 
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                result = BSSDK.BS_ReadFaceData(m_Handle, ref imageLen, imageData, faceTemplate);
                                Cursor.Current = Cursors.Default;

                                if (result != BSSDK.BS_SUCCESS)
                                {
                                    nRet = MessageBox.Show("Error Capture User Face!!!\r\nTry capture again?",
                                                            "Error",
                                                            MessageBoxButtons.RetryCancel);

                                    if (nRet != DialogResult.Retry)
                                    {
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }
                                else
                                {
                                    Buffer.BlockCopy(faceTemplate, 0, m_FaceTemplate, 0, TEMPLATE_SIZE);
                                    break;
                                }

                            } while (nRet == DialogResult.Retry);

                            System.Threading.Thread.Sleep(500);     // The delay is required that is more than five miliseconds.
		                }
                        break;
                }
	        }


            // FaceStation Only
            if (face_fst.Checked == true)
            {
                switch (m_DeviceType)
                {
                    case BSSDK.BS_DEVICE_FSTATION:
                        {
                            DialogResult nRet = DialogResult.Yes;
                            byte[] imageData = new byte[BS_MAX_IMAGE_SIZE];
                            byte[] faceTemplate = new byte[BS_FST_FACETEMPLATE_SIZE * BS_FST_MAX_FACE_TEMPLATE];

                            do
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                result = BSSDK.BS_ScanFaceTemplate(m_Handle, m_userTemplateData, imageData, faceTemplate);
                                Cursor.Current = Cursors.Default;

                                if (result != BSSDK.BS_SUCCESS)
                                {
                                    nRet = MessageBox.Show("Error Capture User Face!!!\r\nTry capture again?",
                                                            "Error",
                                                            MessageBoxButtons.RetryCancel);

                                    if (nRet != DialogResult.Yes)
                                    {
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }
                                else
                                {
                                    int offset = 0, offset2 = 0;

                                    BSSDK.FSUserTemplateHdr userTemplateHdr = (BSSDK.FSUserTemplateHdr)Marshal.PtrToStructure(m_userTemplateData, typeof(BSSDK.FSUserTemplateHdr));

                                    int nCount = (int)userTemplateHdr.numOfFace;
                                    numOfFace.Text = nCount.ToString();

                                    // face template data (max 25)
                                    for (int i = 0; i < BS_FST_MAX_FACE_TEMPLATE; i++)
                                    {
                                        Buffer.BlockCopy(faceTemplate, offset, m_FaceTemplate_FST, offset2, userTemplateHdr.faceLen[i]);
                                        offset += userTemplateHdr.faceLen[i];
                                        offset2 += userTemplateHdr.faceLen[i];
                                    }


                                    // save profile image
                                    if (userTemplateHdr.imageSize > 0)
                                    {
                                        byte[] imageBytes = new byte[userTemplateHdr.imageSize];
                                        System.Buffer.BlockCopy(imageData, 0, imageBytes, 0, imageBytes.Length);

                                        try
                                        {
                                            String filename = "c:\\temp\\profile.jpg";
                                            FileStream fs = new FileStream(filename, FileMode.CreateNew, FileAccess.ReadWrite);
                                            BinaryWriter write_fs = new BinaryWriter(fs);
                                            write_fs.Write(imageBytes);
                                            write_fs.Close();
                                            fs.Close();
                                        }
                                        catch (Exception saveE)
                                        {
                                            MessageBox.Show(this, saveE.Message);
                                        }
                                    }

                                    break;
                                }

                            } while (nRet == DialogResult.Yes);

                            System.Threading.Thread.Sleep(500);     // The delay is required that is more than five miliseconds.
                        }
                        break;
                }
            }


            switch (m_DeviceType)
            {
                case BSSDK.BS_DEVICE_BIOENTRY_PLUS:
                case BSSDK.BS_DEVICE_BIOENTRY_W:
                case BSSDK.BS_DEVICE_BIOLITE:
                case BSSDK.BS_DEVICE_XPASS:
                case BSSDK.BS_DEVICE_XPASS_SLIM:
                case BSSDK.BS_DEVICE_XPASS_SLIM2:
                    {
                        BSSDK.BEUserHdr userHdr = new BSSDK.BEUserHdr();
                        userHdr.fingerChecksum = new ushort[2];
                        userHdr.isDuress = new byte[2];
                        userHdr.numOfFinger = (ushort)numOfFinger;

                        userHdr.fingerChecksum[0] = (ushort)fingerChecksum1;
                        userHdr.fingerChecksum[1] = (ushort)fingerChecksum2;

                        // pwd
                        string pwd = password.Text;
                        userHdr.password = new byte[17];
                        byte[] tmppw = Encoding.Unicode.GetBytes(pwd);
                        Buffer.BlockCopy(tmppw, 0, userHdr.password, 0, tmppw.Length);

                        userHdr.userID = ID;
                        userHdr.adminLevel = (ushort)userLevel.SelectedIndex;
                        userHdr.securityLevel = (ushort)securityLevel.SelectedIndex;
                        userHdr.cardFlag = (byte)cardType.SelectedIndex;
                        userHdr.startTime = (int)((startDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                        userHdr.expiryTime = (int)((expiryDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                        userHdr.isDuress[0] = (duress1.Checked) ? (byte)1 : (byte)0;
                        userHdr.isDuress[1] = (duress2.Checked) ? (byte)1 : (byte)0;
                        userHdr.opMode = (ushort)authMode.SelectedIndex;

                        if (authMode.SelectedIndex == 0)
                            userHdr.opMode = 0;
                        else
                            userHdr.opMode = (ushort)(authMode.SelectedIndex + BSSDK.BS_AUTH_FINGER_ONLY - 1);

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
                            userHdr.cardCustomID = (byte)Int32.Parse(cardCustomID.Text);
                        }
                        catch (Exception)
                        {
                            userHdr.cardCustomID = 0;
                        }

                        userHdr.cardVersion = BSSDK.BE_CARD_VERSION_1;
                        userHdr.disabled = 0;
                        userHdr.dualMode = 0;

                        try
                        {
                            userHdr.accessGroupMask = UInt32.Parse(accessGroup.Text, System.Globalization.NumberStyles.HexNumber);
                        }
                        catch (Exception)
                        {
                            userHdr.accessGroupMask = 0xffffffff;
                        }

                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BEUserHdr)));
                        Marshal.StructureToPtr(userHdr, userInfo, true);

                        Cursor.Current = Cursors.WaitCursor;
                        result = BSSDK.BS_EnrollUserBEPlus(m_Handle, userInfo, m_TemplateData);
                        Cursor.Current = Cursors.Default;

                        Marshal.FreeHGlobal(userInfo);

                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot enroll the user", "Error");
                            return;
                        }

                        ReadUserInfo();
                    }
                    break;

                case BSSDK.BS_DEVICE_BIOSTATION:
                    {
                        BSSDK.BSUserHdrEx userHdr = new BSSDK.BSUserHdrEx();

                        userHdr.checksum = new ushort[5];
                        userHdr.name = new byte[33];
                        userHdr.department = new byte[33];
                        userHdr.password = new byte[17];

                        userHdr.authLimitCount = 0;
                        userHdr.timedAntiPassback = 0;
                        userHdr.disabled = 0;

                        userHdr.numOfFinger = (ushort)numOfFinger;

                        userHdr.checksum[0] = (ushort)fingerChecksum1;
                        userHdr.checksum[1] = (ushort)fingerChecksum2;


                        // name 
                        string username = name.Text;
                        byte[] nameBytes = Encoding.ASCII.GetBytes(username);             // UTF8
                        Buffer.BlockCopy(nameBytes, 0, userHdr.name, 0, nameBytes.Length);

                        // pwd
                        string pwd = password.Text;
                        userHdr.password = new byte[17];
                        byte[] tmppw = Encoding.ASCII.GetBytes(pwd);
                        Buffer.BlockCopy(tmppw, 0, userHdr.password, 0, tmppw.Length);


                        userHdr.ID = ID;
                        userHdr.adminLevel = (ushort)((userLevel.SelectedIndex == 1) ? BSSDK.BS_USER_ADMIN : BSSDK.BS_USER_NORMAL);
                        userHdr.securityLevel = (ushort)(securityLevel.SelectedIndex + BSSDK.BS_USER_SECURITY_DEFAULT);

                        userHdr.bypassCard = (byte)cardType.SelectedIndex;
                        userHdr.startDateTime = (uint)((startDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                        userHdr.expireDateTime = (uint)((expiryDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);

                        userHdr.duressMask = 0;
                        if (duress1.Checked)
                            userHdr.duressMask |= 0x01;

                        if (duress2.Checked)
                            userHdr.duressMask |= 0x02;
                        
                        if (authMode.SelectedIndex == 0)
                            userHdr.authMode = 0;
                        else
                            userHdr.authMode = (ushort)(authMode.SelectedIndex + BSSDK.BS_AUTH_FINGER_ONLY - 1);

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

                        userHdr.version = BSSDK.BE_CARD_VERSION_1;

                        try
                        {
                            userHdr.accessGroupMask = UInt32.Parse(accessGroup.Text, System.Globalization.NumberStyles.HexNumber);
                        }
                        catch (Exception)
                        {
                            userHdr.accessGroupMask = 0xffffffff;
                        }

                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BSUserHdrEx)));
                        Marshal.StructureToPtr(userHdr, userInfo, true);

                        Cursor.Current = Cursors.WaitCursor;
                        result = BSSDK.BS_EnrollUserEx(m_Handle, userInfo, m_TemplateData);
                        Cursor.Current = Cursors.Default;

                        Marshal.FreeHGlobal(userInfo);

                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot enroll the user", "Error");
                            return;
                        }

                        ReadUserInfo();
                    }
                    break;

                case BSSDK.BS_DEVICE_DSTATION:
                    {
                        BSSDK.DSUserHdr userHdr = new BSSDK.DSUserHdr();

                        userHdr.name = new ushort[48];
                        userHdr.department = new ushort[48];
                        userHdr.password = new ushort[16];
                        userHdr.duress = new byte[10];
                        userHdr.reserved = new byte[2];
                        userHdr.fingerType = new byte[10];
                        userHdr.reserved1 = new byte[2];
                        userHdr.fingerChecksum = new uint[10];
                        userHdr.faceChecksum = new uint[5];
                        userHdr.reserved2 = new uint[10];

                        userHdr.disabled = 0;
                        userHdr.numOfFinger = (ushort)numOfFinger;
                        userHdr.fingerChecksum[0] = (ushort)fingerChecksum1;
                        userHdr.fingerChecksum[1] = (ushort)fingerChecksum2;

                        if (face.Checked == true)
                        {
                            // face template's checksum
                            int offset = 0;

                            userHdr.numOfFace = 1;
                            userHdr.faceChecksum[0] = 0;
 
                            byte[] templateData = new byte[FACETEMPLATE_SIZE];
                            Buffer.BlockCopy(m_FaceTemplate, offset, templateData, 0, FACETEMPLATE_SIZE);

                            for (int j = 0; j < FACETEMPLATE_SIZE; j++)
                            {
                                userHdr.faceChecksum[0] += templateData[j];
                            }
                            offset += FACETEMPLATE_SIZE; 
                        }
                        else 
                            userHdr.numOfFace = 0;  


                        // name 
                        string username = name.Text;
                        byte[] nameBytes = Encoding.Unicode.GetBytes(username);             // UTF16
                        Buffer.BlockCopy(nameBytes, 0, userHdr.name, 0, nameBytes.Length);

                        // pwd
                        string pwd = password.Text;
                        byte[] tmppw = Encoding.Unicode.GetBytes(pwd);
                        Buffer.BlockCopy(tmppw, 0, userHdr.password, 0, tmppw.Length);

                        userHdr.ID = ID;
                        userHdr.adminLevel = (ushort)((userLevel.SelectedIndex == 1) ? BSSDK.DSUserHdr.ENUM.USER_ADMIN : BSSDK.DSUserHdr.ENUM.USER_NORMAL);
                        userHdr.securityLevel = (ushort)(securityLevel.SelectedIndex + BSSDK.BS_USER_SECURITY_DEFAULT);

                        userHdr.bypassCard = (byte)cardType.SelectedIndex;
                        userHdr.startDateTime = (uint)((startDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                        userHdr.expireDateTime = (uint)((expiryDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);


                        userHdr.duress[0] = 0;
                        userHdr.duress[1] = 0;

                        if (duress1.Checked)
                            userHdr.duress[0] = 0x01;

                        if (duress2.Checked)
                            userHdr.duress[1] = 0x01;

                        if (authMode.SelectedIndex == 0)
                            userHdr.authMode = 0;
                        else
                            userHdr.authMode = (ushort)(authMode.SelectedIndex + BSSDK.BS_AUTH_FINGER_ONLY - 1);

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

                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.DSUserHdr)));
                        Marshal.StructureToPtr(userHdr, userInfo, true);

                        Cursor.Current = Cursors.WaitCursor;
                        result = BSSDK.BS_EnrollUserDStation(m_Handle, userInfo, m_TemplateData, m_FaceTemplate);
                        Cursor.Current = Cursors.Default;

                        Marshal.FreeHGlobal(userInfo);

                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot enroll the user", "Error");
                            return;
                        }

                        ReadUserInfo(); 
                    }
                    break;

                case BSSDK.BS_DEVICE_XSTATION:
                    {
                        BSSDK.XSUserHdr userHdr = new BSSDK.XSUserHdr();

                        userHdr.name = new ushort[48];
                        userHdr.department = new ushort[48];
                        userHdr.password = new ushort[16];
                        userHdr.duress = new byte[10];
                        userHdr.reserved = new byte[2];
                        userHdr.fingerType = new byte[10];
                        userHdr.reserved1 = new byte[2];
                        userHdr.fingerChecksum = new uint[10];
                        userHdr.faceChecksum = new uint[5];
                        userHdr.reserved2 = new uint[10];

                        // name 
                        string username = name.Text;
                        byte[] nameBytes = Encoding.Unicode.GetBytes(username);             // UTF16
                        Buffer.BlockCopy(nameBytes, 0, userHdr.name, 0, nameBytes.Length);

                        // pwd
                        string pwd = password.Text;
                        byte[] tmppw = Encoding.Unicode.GetBytes(pwd);
                        Buffer.BlockCopy(tmppw, 0, userHdr.password, 0, tmppw.Length);


                        userHdr.ID = ID;
                        userHdr.startDateTime = (uint)((startDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                        userHdr.expireDateTime = (uint)((expiryDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);

                        userHdr.adminLevel = (ushort)((userLevel.SelectedIndex == 1) ? BSSDK.XSUserHdr.ENUM.USER_ADMIN : BSSDK.XSUserHdr.ENUM.USER_NORMAL);
                        userHdr.securityLevel = (ushort)(securityLevel.SelectedIndex + BSSDK.BS_USER_SECURITY_DEFAULT);
                        userHdr.bypassCard = (byte)cardType.SelectedIndex;

                        if (authMode.SelectedIndex == 0)
                            userHdr.authMode = 0;
                        else
                            userHdr.authMode = (ushort)(authMode.SelectedIndex + BSSDK.BS_AUTH_FINGER_ONLY - 1);

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

                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.XSUserHdr)));
                        Marshal.StructureToPtr(userHdr, userInfo, true);

                        Cursor.Current = Cursors.WaitCursor;
                        result = BSSDK.BS_EnrollUserXStation(m_Handle, userInfo);
                        Cursor.Current = Cursors.Default;

                        Marshal.FreeHGlobal(userInfo);

                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot enroll the user", "Error");
                            return;
                        }

                        ReadUserInfo();
                    }
                    break;


                case BSSDK.BS_DEVICE_BIOSTATION2:
                    {
                        BSSDK.BS2UserHdr userHdr = new BSSDK.BS2UserHdr();

                        userHdr.name = new ushort[48];
                        userHdr.department = new ushort[48];
                        userHdr.password = new ushort[16];
                        userHdr.duress = new byte[10];
                        userHdr.reserved = new byte[2];
                        userHdr.fingerType = new byte[10];
                        userHdr.reserved1 = new byte[2];
                        userHdr.fingerChecksum = new uint[10];
                        userHdr.faceChecksum = new uint[5];
                        userHdr.reserved2 = new uint[10];

                        userHdr.disabled = 0;
                        userHdr.numOfFinger = (ushort)numOfFinger;
                        userHdr.fingerChecksum[0] = (ushort)fingerChecksum1;
                        userHdr.fingerChecksum[1] = (ushort)fingerChecksum2;


                        userHdr.ID = ID;
                        userHdr.adminLevel = (ushort)((userLevel.SelectedIndex == 1) ? BSSDK.BS2UserHdr.ENUM.USER_ADMIN : BSSDK.BS2UserHdr.ENUM.USER_NORMAL);
                        userHdr.securityLevel = (ushort)(securityLevel.SelectedIndex + BSSDK.BS_USER_SECURITY_DEFAULT);

                        userHdr.bypassCard = (byte)cardType.SelectedIndex;
                        userHdr.startDateTime = (uint)((startDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                        userHdr.expireDateTime = (uint)((expiryDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);


                        userHdr.duress[0] = 0;
                        userHdr.duress[1] = 0;

                        if (duress1.Checked)
                            userHdr.duress[0] = 0x01;

                        if (duress2.Checked)
                            userHdr.duress[1] = 0x01;

                        if (authMode.SelectedIndex == 0)
                            userHdr.authMode = 0;
                        else
                            userHdr.authMode = (ushort)(authMode.SelectedIndex + BSSDK.BS_AUTH_FINGER_ONLY - 1);

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

                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.BS2UserHdr)));
                        Marshal.StructureToPtr(userHdr, userInfo, true);

                        Cursor.Current = Cursors.WaitCursor;
                        result = BSSDK.BS_EnrollUserBioStation2(m_Handle, userInfo, m_TemplateData);
                        Cursor.Current = Cursors.Default;

                        Marshal.FreeHGlobal(userInfo);

                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot enroll the user", "Error");
                            return;
                        }

                        ReadUserInfo();
                    }
                    break;

                case BSSDK.BS_DEVICE_FSTATION:
                    {
                        BSSDK.FSUserHdr userHdr = new BSSDK.FSUserHdr();

                        BSSDK.FSUserTemplateHdr userTemplateHdr = (BSSDK.FSUserTemplateHdr)Marshal.PtrToStructure(m_userTemplateData, typeof(BSSDK.FSUserTemplateHdr));

                        userHdr.name = new ushort[48];
                        userHdr.department = new ushort[48];
                        userHdr.password = new ushort[16];

                        userHdr.faceLen = new ushort[25];
                        userHdr.faceTemp = new byte[256];
                        userHdr.faceChecksum = new uint[25];

                        userHdr.disabled = 0;
                        userHdr.numOfFace = (ushort)userTemplateHdr.numOfFace;
                        userHdr.numOfUpdatedFace = (ushort)userTemplateHdr.numOfUpdatedFace;

                        // face template's length
                        for (int i=0; i < BS_FST_MAX_FACE_TEMPLATE; i++)
                        {
                            userHdr.faceLen[i] = userTemplateHdr.faceLen[i];
                        }
                        
                        // face template's checksum

                        int offset = 0;
                        for (int i=0; i < BS_FST_MAX_FACE_TEMPLATE; i++)
                        {
                            int nLen = userTemplateHdr.faceLen[i];
                            if (nLen > 0)
                            {
                                byte[] templateData = new byte[nLen];
                                Buffer.BlockCopy(m_FaceTemplate_FST, offset, templateData, 0, nLen);

                                for (int j=0; j < userTemplateHdr.faceLen[i]; j++)
                                {
                                    userHdr.faceChecksum[i] += templateData[j];
                                }
                                offset += nLen;
                            }
                        }
                        //

                        // face temp data
                        Buffer.BlockCopy(userTemplateHdr.faceTemp, 0, userHdr.faceTemp, 0, 256);


                        // name 
                        string username = name.Text;
                        byte[] nameBytes = Encoding.Unicode.GetBytes(username);             // UTF16
                        Buffer.BlockCopy(nameBytes, 0, userHdr.name, 0, nameBytes.Length);

                        // pwd
                        string pwd = password.Text;
                        byte[] pwdBytes = Encoding.Unicode.GetBytes(pwd);
                        Buffer.BlockCopy(pwdBytes, 0, userHdr.password, 0, pwdBytes.Length);

                        userHdr.ID = ID;
                        userHdr.adminLevel = (ushort)((userLevel.SelectedIndex == 1) ? BSSDK.FSUserHdr.ENUM.USER_ADMIN : BSSDK.FSUserHdr.ENUM.USER_NORMAL);
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

                        IntPtr userInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BSSDK.FSUserHdr)));
                        Marshal.StructureToPtr(userHdr, userInfo, true);

                        Cursor.Current = Cursors.WaitCursor;
                        result = BSSDK.BS_EnrollUserFStation(m_Handle, userInfo, m_FaceTemplate_FST);
                        Cursor.Current = Cursors.Default;

                        Marshal.FreeHGlobal(userInfo);

                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot enroll the user", "Error");
                            return;
                        }

                        ReadUserInfo();
                    }
                    break;
            }

        }

        private void EnrollMulti_Click(object sender, EventArgs e)
        {
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

            switch (m_DeviceType)
            {
                case BSSDK.BS_DEVICE_BIOENTRY_PLUS:
                case BSSDK.BS_DEVICE_BIOENTRY_W:
                case BSSDK.BS_DEVICE_BIOLITE:
                case BSSDK.BS_DEVICE_XPASS:
                case BSSDK.BS_DEVICE_XPASS_SLIM:
                case BSSDK.BS_DEVICE_XPASS_SLIM2:
                    {
                        int numOfUser = 5;

                        BSSDK.BEUserHdr[] userHdr = new BSSDK.BEUserHdr[numOfUser];

                        for (int i = 0; i < numOfUser; i++)
                        {
                            userHdr[i].fingerChecksum = new ushort[2];
                            userHdr[i].isDuress = new byte[2];
                            userHdr[i].numOfFinger = (ushort) 0;

                            userHdr[i].fingerChecksum[0] = (ushort)0;
                            userHdr[i].fingerChecksum[1] = (ushort)0;

                            userHdr[i].password = new byte[17];
                            byte[] tmppw = Encoding.Unicode.GetBytes("1234");
                            Buffer.BlockCopy(tmppw, 0, userHdr[i].password, 0, 4);

                            uint ID = (uint)(i + 1) * 1000;
                            userHdr[i].userID = ID;
                            userHdr[i].adminLevel = (ushort)userLevel.SelectedIndex;
                            userHdr[i].securityLevel = (ushort)securityLevel.SelectedIndex;
                            userHdr[i].cardFlag = (byte)cardType.SelectedIndex;
                            userHdr[i].startTime = (int)((startDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                            userHdr[i].expiryTime = (int)((expiryDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                            userHdr[i].isDuress[0] = (duress1.Checked) ? (byte)1 : (byte)0;
                            userHdr[i].isDuress[1] = (duress2.Checked) ? (byte)1 : (byte)0;
                            userHdr[i].opMode = (ushort)authMode.SelectedIndex;

                            try
                            {
                                userHdr[i].cardID = UInt32.Parse(userCardID.Text, System.Globalization.NumberStyles.HexNumber);
                            }
                            catch (Exception)
                            {
                                userHdr[i].cardID = 0;
                            }

                            try
                            {
                                userHdr[i].cardCustomID = (byte)Int32.Parse(cardCustomID.Text);
                            }
                            catch (Exception)
                            {
                                userHdr[i].cardCustomID = 0;
                            }

                            userHdr[i].cardVersion = BSSDK.BE_CARD_VERSION_1;
                            userHdr[i].disabled = 0;
                            userHdr[i].dualMode = 0;

                            try
                            {
                                userHdr[i].accessGroupMask = UInt32.Parse(accessGroup.Text, System.Globalization.NumberStyles.HexNumber);
                            }
                            catch (Exception)
                            {
                                userHdr[i].accessGroupMask = 0xffffffff;
                            }
                        }

                        IntPtr userInfo = Marshal.AllocHGlobal(numOfUser * Marshal.SizeOf(typeof(BSSDK.BEUserHdr)));
                        IntPtr ptrRunner = userInfo;
                        for (int i = 0; i < numOfUser; i++)
                        {
                            Marshal.StructureToPtr(userHdr[i], ptrRunner, true);
                            ptrRunner = (IntPtr)((int)ptrRunner + Marshal.SizeOf(typeof(BSSDK.BEUserHdr)));
                        }

                        Cursor.Current = Cursors.WaitCursor;
                        int result = BSSDK.BS_EnrollMultipleUserBEPlus(m_Handle, numOfUser, userInfo, m_TemplateData);
                        Cursor.Current = Cursors.Default;

                        Marshal.FreeHGlobal(userInfo);

                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot enroll the user", "Error");
                            return;
                        }

                        ReadUserInfo();
                    }
                    break;

                case BSSDK.BS_DEVICE_BIOSTATION2:
                    {
                        int numOfUser = 5;

                        BSSDK.BS2UserHdr[] userHdr = new BSSDK.BS2UserHdr[numOfUser];

                        for (int i = 0; i < numOfUser; i++)
                        {
                            userHdr[i].numOfFinger = (ushort) 0;

                            userHdr[i].fingerChecksum = new uint[10];
                            userHdr[i].duress = new byte[10];

                            for (int j = 0; j < 10; j++)
                            {
                                userHdr[i].fingerChecksum[j] = 0;
                                userHdr[i].duress[j] = 0;
                            }

                            userHdr[i].name = new ushort[48];
                            byte[] tmpname = Encoding.Unicode.GetBytes("John");   // covert to UTF16
                            Buffer.BlockCopy(tmpname, 0, userHdr[i].name, 0, tmpname.Length);


                            userHdr[i].password = new ushort[16];
                            byte[] tmppw = Encoding.Unicode.GetBytes("1234");   // covert to UTF16
                            Buffer.BlockCopy(tmppw, 0, userHdr[i].password, 0, tmppw.Length);

                            uint ID = (uint)(i + 1) * 1000;
                            userHdr[i].ID = ID;
                            userHdr[i].adminLevel = (ushort)((userLevel.SelectedIndex == 1) ? BSSDK.BS2UserHdr.ENUM.USER_ADMIN : BSSDK.BS2UserHdr.ENUM.USER_NORMAL);
                            userHdr[i].securityLevel = (ushort)securityLevel.SelectedIndex;
                            userHdr[i].startDateTime = (uint)((startDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                            userHdr[i].expireDateTime = (uint)((expiryDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000);
                            userHdr[i].authMode = (ushort)authMode.SelectedIndex;

                            try
                            {
                                userHdr[i].cardID = UInt32.Parse(userCardID.Text, System.Globalization.NumberStyles.HexNumber);
                            }
                            catch (Exception)
                            {
                                userHdr[i].cardID = 0;
                            }

                            try
                            {
                                userHdr[i].customID = (byte)Int32.Parse(cardCustomID.Text);
                            }
                            catch (Exception)
                            {
                                userHdr[i].customID = 0;
                            }

                            userHdr[i].disabled = 0;

                            try
                            {
                                userHdr[i].accessGroupMask = UInt32.Parse(accessGroup.Text, System.Globalization.NumberStyles.HexNumber);
                            }
                            catch (Exception)
                            {
                                userHdr[i].accessGroupMask = 0xffffffff;
                            }
                        }

                        IntPtr userInfo = Marshal.AllocHGlobal(numOfUser * Marshal.SizeOf(typeof(BSSDK.BS2UserHdr)));
                        IntPtr ptrRunner = userInfo;
                        for (int i = 0; i < numOfUser; i++)
                        {
                            Marshal.StructureToPtr(userHdr[i], ptrRunner, true);
                            ptrRunner = (IntPtr)((int)ptrRunner + Marshal.SizeOf(typeof(BSSDK.BS2UserHdr)));
                        }

                        Cursor.Current = Cursors.WaitCursor;
                        int result = BSSDK.BS_EnrollMultipleUserBioStation2(m_Handle, numOfUser, userInfo, m_TemplateData);
                        Cursor.Current = Cursors.Default;

                        Marshal.FreeHGlobal(userInfo);

                        if (result != BSSDK.BS_SUCCESS)
                        {
                            MessageBox.Show("Cannot enroll the user", "Error");
                            return;
                        }

                        ReadUserInfo();
                    }
                    break;

            }


        }

        private void ScanFaceTemplate_Click(object sender, EventArgs e)
        {
            int result = BSSDK.BS_SUCCESS;
            DialogResult nRet = DialogResult.Yes;
            byte[] imageData = new byte[BS_MAX_IMAGE_SIZE];
            byte[] faceTemplate = new byte[BS_FST_FACETEMPLATE_SIZE * BS_FST_MAX_FACE_TEMPLATE];

            do
            {
                Cursor.Current = Cursors.WaitCursor;
                result = BSSDK.BS_ScanFaceTemplate(m_Handle, m_userTemplateData, imageData, faceTemplate);
                Cursor.Current = Cursors.Default;

                if (result != BSSDK.BS_SUCCESS)
                {
                    nRet = MessageBox.Show("Error Capture User Face!!!\r\nTry capture again?",
                                            "Error",
                                            MessageBoxButtons.RetryCancel);

                    if (nRet != DialogResult.Retry)
                    {
                        Cursor.Current = Cursors.Default;
                        checkScanFace.Checked = false;
                        return;
                    }
                }
                else
                {
                    int offset = 0, offset2 = 0;

                    BSSDK.FSUserTemplateHdr userTemplateHdr = (BSSDK.FSUserTemplateHdr)Marshal.PtrToStructure(m_userTemplateData, typeof(BSSDK.FSUserTemplateHdr));

                    int nCount = (int)userTemplateHdr.numOfFace;
                    numOfFace.Text = nCount.ToString();

                    // face template data (max 25)
                    for (int i = 0; i < BS_FST_MAX_FACE_TEMPLATE; i++)
                    {
                        Buffer.BlockCopy( faceTemplate, offset, m_FaceTemplate_FST, offset2, userTemplateHdr.faceLen[i] );
                        offset += userTemplateHdr.faceLen[i];
                        offset2 += userTemplateHdr.faceLen[i];
                    }


                    // save profile image
                    if (userTemplateHdr.imageSize > 0)
                    {
                        byte[] imageBytes = new byte[userTemplateHdr.imageSize];
                        System.Buffer.BlockCopy(imageData, 0, imageBytes, 0, imageBytes.Length);

                        try
                        {
                            String filename = "c:\\temp\\profile.jpg";
                            FileStream fs = new FileStream(filename, FileMode.CreateNew, FileAccess.ReadWrite);
                            BinaryWriter write_fs = new BinaryWriter(fs);
                            write_fs.Write(imageBytes);
                            write_fs.Close();
                            fs.Close();
                        }
                        catch (Exception saveE)
                        {
                            MessageBox.Show(this, saveE.Message);
                        }
                    }

                    checkScanFace.Checked = true;
                    break;
                }

            } while (nRet == DialogResult.Retry);

            System.Threading.Thread.Sleep(500);     // The delay is required that is more than five miliseconds.
        }

    }
}