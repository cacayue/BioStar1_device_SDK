namespace BioStarCSharp
{
    partial class UserManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.securityLevel = new System.Windows.Forms.ComboBox();
            this.expiryDate = new System.Windows.Forms.DateTimePicker();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.userLevel = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numOfFaces = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numOfTemplate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numOfUser = new System.Windows.Forms.TextBox();
            this.deviceInfo = new System.Windows.Forms.TextBox();
            this.name = new System.Windows.Forms.TextBox();
            this.userID = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cardType = new System.Windows.Forms.ComboBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.deleteAllButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.updateUser = new System.Windows.Forms.Button();
            this.cardID = new System.Windows.Forms.ColumnHeader();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.readCard = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.cardCustomID = new System.Windows.Forms.TextBox();
            this.userCardID = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.numOfFinger = new System.Windows.Forms.ColumnHeader();
            this.ID = new System.Windows.Forms.ColumnHeader();
            this.userList = new System.Windows.Forms.ListView();
            this.label10 = new System.Windows.Forms.Label();
            this.checksum1 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.accessGroup = new System.Windows.Forms.TextBox();
            this.authMode = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.checkScanFace = new System.Windows.Forms.CheckBox();
            this.numOfFace = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.ScanFaceTemplate = new System.Windows.Forms.Button();
            this.face_fst = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.faceChecksum = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.face = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.duress2 = new System.Windows.Forms.CheckBox();
            this.finger2 = new System.Windows.Forms.CheckBox();
            this.checksum2 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.duress1 = new System.Windows.Forms.CheckBox();
            this.finger1 = new System.Windows.Forms.CheckBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.addUser = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // securityLevel
            // 
            this.securityLevel.FormattingEnabled = true;
            this.securityLevel.Items.AddRange(new object[] {
            "Default",
            "Lowest",
            "Lower",
            "Normal",
            "Higher",
            "Highest"});
            this.securityLevel.Location = new System.Drawing.Point(233, 73);
            this.securityLevel.Name = "securityLevel";
            this.securityLevel.Size = new System.Drawing.Size(91, 20);
            this.securityLevel.TabIndex = 11;
            // 
            // expiryDate
            // 
            this.expiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.expiryDate.Location = new System.Drawing.Point(233, 46);
            this.expiryDate.Name = "expiryDate";
            this.expiryDate.Size = new System.Drawing.Size(91, 21);
            this.expiryDate.TabIndex = 10;
            // 
            // startDate
            // 
            this.startDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDate.Location = new System.Drawing.Point(233, 20);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(91, 21);
            this.startDate.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(173, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "Security";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(173, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 7;
            this.label7.Text = "Expiry";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(173, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 12);
            this.label8.TabIndex = 6;
            this.label8.Text = "Start";
            // 
            // userLevel
            // 
            this.userLevel.FormattingEnabled = true;
            this.userLevel.Items.AddRange(new object[] {
            "Normal",
            "Admin"});
            this.userLevel.Location = new System.Drawing.Point(71, 73);
            this.userLevel.Name = "userLevel";
            this.userLevel.Size = new System.Drawing.Size(91, 20);
            this.userLevel.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numOfFaces);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numOfTemplate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numOfUser);
            this.groupBox1.Controls.Add(this.deviceInfo);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(618, 58);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Device";
            // 
            // numOfFaces
            // 
            this.numOfFaces.Location = new System.Drawing.Point(551, 21);
            this.numOfFaces.Name = "numOfFaces";
            this.numOfFaces.ReadOnly = true;
            this.numOfFaces.Size = new System.Drawing.Size(56, 21);
            this.numOfFaces.TabIndex = 6;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(511, 25);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(36, 12);
            this.label18.TabIndex = 5;
            this.label18.Text = "faces";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(370, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "templates";
            // 
            // numOfTemplate
            // 
            this.numOfTemplate.Location = new System.Drawing.Point(436, 20);
            this.numOfTemplate.Name = "numOfTemplate";
            this.numOfTemplate.ReadOnly = true;
            this.numOfTemplate.Size = new System.Drawing.Size(66, 21);
            this.numOfTemplate.TabIndex = 3;
            this.numOfTemplate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(257, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "users";
            // 
            // numOfUser
            // 
            this.numOfUser.Location = new System.Drawing.Point(298, 20);
            this.numOfUser.Name = "numOfUser";
            this.numOfUser.ReadOnly = true;
            this.numOfUser.Size = new System.Drawing.Size(66, 21);
            this.numOfUser.TabIndex = 1;
            this.numOfUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // deviceInfo
            // 
            this.deviceInfo.Location = new System.Drawing.Point(16, 21);
            this.deviceInfo.Name = "deviceInfo";
            this.deviceInfo.ReadOnly = true;
            this.deviceInfo.Size = new System.Drawing.Size(235, 21);
            this.deviceInfo.TabIndex = 0;
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(71, 46);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(91, 21);
            this.name.TabIndex = 4;
            // 
            // userID
            // 
            this.userID.Location = new System.Drawing.Point(71, 18);
            this.userID.Name = "userID";
            this.userID.Size = new System.Drawing.Size(91, 21);
            this.userID.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.password);
            this.groupBox3.Controls.Add(this.securityLevel);
            this.groupBox3.Controls.Add(this.expiryDate);
            this.groupBox3.Controls.Add(this.startDate);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.userLevel);
            this.groupBox3.Controls.Add(this.name);
            this.groupBox3.Controls.Add(this.userID);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(265, 21);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(340, 122);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Info";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(16, 100);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(30, 12);
            this.label16.TabIndex = 13;
            this.label16.Text = "Pwd";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(71, 96);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(91, 21);
            this.password.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "Admin";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "ID";
            // 
            // cardType
            // 
            this.cardType.FormattingEnabled = true;
            this.cardType.Items.AddRange(new object[] {
            "Normal",
            "Bypass"});
            this.cardType.Location = new System.Drawing.Point(71, 46);
            this.cardType.Name = "cardType";
            this.cardType.Size = new System.Drawing.Size(91, 20);
            this.cardType.TabIndex = 11;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(555, 619);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 13;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // deleteAllButton
            // 
            this.deleteAllButton.Location = new System.Drawing.Point(369, 619);
            this.deleteAllButton.Name = "deleteAllButton";
            this.deleteAllButton.Size = new System.Drawing.Size(75, 23);
            this.deleteAllButton.TabIndex = 12;
            this.deleteAllButton.Text = "Delete All";
            this.deleteAllButton.UseVisualStyleBackColor = true;
            this.deleteAllButton.Click += new System.EventHandler(this.deleteAllButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(292, 619);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 11;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // updateUser
            // 
            this.updateUser.Location = new System.Drawing.Point(94, 619);
            this.updateUser.Name = "updateUser";
            this.updateUser.Size = new System.Drawing.Size(75, 23);
            this.updateUser.TabIndex = 10;
            this.updateUser.Text = "Update";
            this.updateUser.UseVisualStyleBackColor = true;
            this.updateUser.Click += new System.EventHandler(this.updateUser_Click);
            // 
            // cardID
            // 
            this.cardID.Text = "Card ID";
            this.cardID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.cardID.Width = 90;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.readCard);
            this.groupBox4.Controls.Add(this.cardType);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.cardCustomID);
            this.groupBox4.Controls.Add(this.userCardID);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Location = new System.Drawing.Point(265, 144);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(340, 82);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Card";
            // 
            // readCard
            // 
            this.readCard.Location = new System.Drawing.Point(232, 46);
            this.readCard.Name = "readCard";
            this.readCard.Size = new System.Drawing.Size(75, 23);
            this.readCard.TabIndex = 12;
            this.readCard.Text = "Read Card";
            this.readCard.UseVisualStyleBackColor = true;
            this.readCard.Click += new System.EventHandler(this.readCard_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(173, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 12);
            this.label11.TabIndex = 6;
            this.label11.Text = "Custom";
            // 
            // cardCustomID
            // 
            this.cardCustomID.Location = new System.Drawing.Point(232, 18);
            this.cardCustomID.Name = "cardCustomID";
            this.cardCustomID.ReadOnly = true;
            this.cardCustomID.Size = new System.Drawing.Size(91, 21);
            this.cardCustomID.TabIndex = 4;
            // 
            // userCardID
            // 
            this.userCardID.Location = new System.Drawing.Point(71, 18);
            this.userCardID.Name = "userCardID";
            this.userCardID.ReadOnly = true;
            this.userCardID.Size = new System.Drawing.Size(91, 21);
            this.userCardID.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 50);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(34, 12);
            this.label13.TabIndex = 1;
            this.label13.Text = "Type";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(14, 23);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 12);
            this.label14.TabIndex = 0;
            this.label14.Text = "Card ID";
            // 
            // numOfFinger
            // 
            this.numOfFinger.Text = "Finger";
            this.numOfFinger.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numOfFinger.Width = 62;
            // 
            // ID
            // 
            this.ID.Text = "User ID";
            this.ID.Width = 71;
            // 
            // userList
            // 
            this.userList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.numOfFinger,
            this.cardID});
            this.userList.FullRowSelect = true;
            this.userList.GridLines = true;
            this.userList.Location = new System.Drawing.Point(17, 21);
            this.userList.MultiSelect = false;
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(235, 501);
            this.userList.TabIndex = 0;
            this.userList.UseCompatibleStateImageBehavior = false;
            this.userList.View = System.Windows.Forms.View.Details;
            this.userList.SelectedIndexChanged += new System.EventHandler(this.userList_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(173, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 12);
            this.label10.TabIndex = 1;
            this.label10.Text = "Checksum";
            // 
            // checksum1
            // 
            this.checksum1.Location = new System.Drawing.Point(245, 18);
            this.checksum1.Name = "checksum1";
            this.checksum1.ReadOnly = true;
            this.checksum1.Size = new System.Drawing.Size(79, 21);
            this.checksum1.TabIndex = 4;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(13, 44);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(86, 12);
            this.label17.TabIndex = 16;
            this.label17.Text = "Access Group";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.accessGroup);
            this.groupBox6.Controls.Add(this.authMode);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this.label15);
            this.groupBox6.Location = new System.Drawing.Point(266, 443);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(340, 69);
            this.groupBox6.TabIndex = 18;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Access Control";
            // 
            // accessGroup
            // 
            this.accessGroup.Location = new System.Drawing.Point(174, 40);
            this.accessGroup.Name = "accessGroup";
            this.accessGroup.Size = new System.Drawing.Size(150, 21);
            this.accessGroup.TabIndex = 18;
            // 
            // authMode
            // 
            this.authMode.FormattingEnabled = true;
            this.authMode.Items.AddRange(new object[] {
            "Disabled",
            "Finger only",
            "Finger and Password",
            "Finger or Password",
            "Password only",
            "Card only"});
            this.authMode.Location = new System.Drawing.Point(174, 17);
            this.authMode.Name = "authMode";
            this.authMode.Size = new System.Drawing.Size(150, 20);
            this.authMode.TabIndex = 17;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(13, 21);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(66, 12);
            this.label15.TabIndex = 12;
            this.label15.Text = "Auth Mode";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox8);
            this.groupBox2.Controls.Add(this.groupBox7);
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.userList);
            this.groupBox2.Location = new System.Drawing.Point(11, 85);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(618, 528);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "User DB";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.checkScanFace);
            this.groupBox8.Controls.Add(this.numOfFace);
            this.groupBox8.Controls.Add(this.label19);
            this.groupBox8.Controls.Add(this.ScanFaceTemplate);
            this.groupBox8.Controls.Add(this.face_fst);
            this.groupBox8.Location = new System.Drawing.Point(267, 387);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(338, 50);
            this.groupBox8.TabIndex = 23;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Face(FaceStation Only)";
            // 
            // checkScanFace
            // 
            this.checkScanFace.AutoSize = true;
            this.checkScanFace.Enabled = false;
            this.checkScanFace.Location = new System.Drawing.Point(226, 23);
            this.checkScanFace.Name = "checkScanFace";
            this.checkScanFace.Size = new System.Drawing.Size(15, 14);
            this.checkScanFace.TabIndex = 4;
            this.checkScanFace.UseVisualStyleBackColor = true;
            // 
            // numOfFace
            // 
            this.numOfFace.Location = new System.Drawing.Point(146, 20);
            this.numOfFace.Name = "numOfFace";
            this.numOfFace.ReadOnly = true;
            this.numOfFace.Size = new System.Drawing.Size(74, 21);
            this.numOfFace.TabIndex = 3;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(72, 24);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(70, 12);
            this.label19.TabIndex = 2;
            this.label19.Text = "numOfFace";
            // 
            // ScanFaceTemplate
            // 
            this.ScanFaceTemplate.Enabled = false;
            this.ScanFaceTemplate.Location = new System.Drawing.Point(244, 19);
            this.ScanFaceTemplate.Name = "ScanFaceTemplate";
            this.ScanFaceTemplate.Size = new System.Drawing.Size(81, 22);
            this.ScanFaceTemplate.TabIndex = 1;
            this.ScanFaceTemplate.Text = "Scan Face";
            this.ScanFaceTemplate.UseVisualStyleBackColor = true;
            this.ScanFaceTemplate.Click += new System.EventHandler(this.ScanFaceTemplate_Click);
            // 
            // face_fst
            // 
            this.face_fst.AutoSize = true;
            this.face_fst.Enabled = false;
            this.face_fst.Location = new System.Drawing.Point(12, 24);
            this.face_fst.Name = "face_fst";
            this.face_fst.Size = new System.Drawing.Size(52, 16);
            this.face_fst.TabIndex = 0;
            this.face_fst.Text = "Face";
            this.face_fst.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.faceChecksum);
            this.groupBox7.Controls.Add(this.label12);
            this.groupBox7.Controls.Add(this.face);
            this.groupBox7.Location = new System.Drawing.Point(265, 333);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(339, 43);
            this.groupBox7.TabIndex = 22;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Face (D-Station Only)";
            // 
            // faceChecksum
            // 
            this.faceChecksum.Location = new System.Drawing.Point(246, 16);
            this.faceChecksum.Name = "faceChecksum";
            this.faceChecksum.ReadOnly = true;
            this.faceChecksum.Size = new System.Drawing.Size(79, 21);
            this.faceChecksum.TabIndex = 21;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(174, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(66, 12);
            this.label12.TabIndex = 20;
            this.label12.Text = "Checksum";
            // 
            // face
            // 
            this.face.AutoSize = true;
            this.face.Location = new System.Drawing.Point(14, 18);
            this.face.Name = "face";
            this.face.Size = new System.Drawing.Size(52, 16);
            this.face.TabIndex = 19;
            this.face.Text = "Face";
            this.face.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.duress2);
            this.groupBox5.Controls.Add(this.finger2);
            this.groupBox5.Controls.Add(this.checksum2);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.duress1);
            this.groupBox5.Controls.Add(this.finger1);
            this.groupBox5.Controls.Add(this.checksum1);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Location = new System.Drawing.Point(266, 242);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(340, 82);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Fingerprint";
            // 
            // duress2
            // 
            this.duress2.AutoSize = true;
            this.duress2.Location = new System.Drawing.Point(92, 48);
            this.duress2.Name = "duress2";
            this.duress2.Size = new System.Drawing.Size(64, 16);
            this.duress2.TabIndex = 17;
            this.duress2.Text = "Duress";
            this.duress2.UseVisualStyleBackColor = true;
            // 
            // finger2
            // 
            this.finger2.AutoSize = true;
            this.finger2.Location = new System.Drawing.Point(14, 48);
            this.finger2.Name = "finger2";
            this.finger2.Size = new System.Drawing.Size(69, 16);
            this.finger2.TabIndex = 16;
            this.finger2.Text = "Finger 2";
            this.finger2.UseVisualStyleBackColor = true;
            // 
            // checksum2
            // 
            this.checksum2.Location = new System.Drawing.Point(245, 46);
            this.checksum2.Name = "checksum2";
            this.checksum2.ReadOnly = true;
            this.checksum2.Size = new System.Drawing.Size(79, 21);
            this.checksum2.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(173, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 12);
            this.label9.TabIndex = 14;
            this.label9.Text = "Checksum";
            // 
            // duress1
            // 
            this.duress1.AutoSize = true;
            this.duress1.Location = new System.Drawing.Point(92, 20);
            this.duress1.Name = "duress1";
            this.duress1.Size = new System.Drawing.Size(64, 16);
            this.duress1.TabIndex = 13;
            this.duress1.Text = "Duress";
            this.duress1.UseVisualStyleBackColor = true;
            // 
            // finger1
            // 
            this.finger1.AutoSize = true;
            this.finger1.Location = new System.Drawing.Point(14, 20);
            this.finger1.Name = "finger1";
            this.finger1.Size = new System.Drawing.Size(69, 16);
            this.finger1.TabIndex = 12;
            this.finger1.Text = "Finger 1";
            this.finger1.UseVisualStyleBackColor = true;
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(463, 619);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 14;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // addUser
            // 
            this.addUser.Location = new System.Drawing.Point(13, 619);
            this.addUser.Name = "addUser";
            this.addUser.Size = new System.Drawing.Size(75, 23);
            this.addUser.TabIndex = 15;
            this.addUser.Text = "Add";
            this.addUser.UseVisualStyleBackColor = true;
            this.addUser.Click += new System.EventHandler(this.addUser_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(175, 619);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "EnrollMulti";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.EnrollMulti_Click);
            // 
            // UserManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 655);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.addUser);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.deleteAllButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.updateUser);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.refreshButton);
            this.Name = "UserManagement";
            this.Text = "User Management";
            this.Load += new System.EventHandler(this.UserManagement_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox securityLevel;
        private System.Windows.Forms.DateTimePicker expiryDate;
        private System.Windows.Forms.DateTimePicker startDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox userLevel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox numOfTemplate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox numOfUser;
        private System.Windows.Forms.TextBox deviceInfo;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.TextBox userID;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cardType;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button deleteAllButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button updateUser;
        private System.Windows.Forms.ColumnHeader cardID;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox cardCustomID;
        private System.Windows.Forms.TextBox userCardID;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ColumnHeader numOfFinger;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ListView userList;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox checksum1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox duress2;
        private System.Windows.Forms.CheckBox finger2;
        private System.Windows.Forms.TextBox checksum2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox duress1;
        private System.Windows.Forms.CheckBox finger1;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button addUser;
        private System.Windows.Forms.Button readCard;
        private System.Windows.Forms.TextBox accessGroup;
        private System.Windows.Forms.ComboBox authMode;
        private System.Windows.Forms.CheckBox face;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox faceChecksum;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckBox face_fst;
        private System.Windows.Forms.TextBox numOfFaces;
        private System.Windows.Forms.TextBox numOfFace;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox checkScanFace;
        private System.Windows.Forms.Button ScanFaceTemplate;
    }
}