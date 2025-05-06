namespace BioStarCSharp
{
    partial class UserManagement_FST
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.securityLevel = new System.Windows.Forms.ComboBox();
            this.cardType = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.accessGroup = new System.Windows.Forms.TextBox();
            this.authMode = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.button_Delete = new System.Windows.Forms.Button();
            this.button_Add = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.numOfFace = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.ScanFaceTemplate = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.readCard = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.cardCustomID = new System.Windows.Forms.TextBox();
            this.userCardID = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.expiryDate = new System.Windows.Forms.DateTimePicker();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.userLevel = new System.Windows.Forms.ComboBox();
            this.name = new System.Windows.Forms.TextBox();
            this.userID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.userList = new System.Windows.Forms.ListView();
            this.ID = new System.Windows.Forms.ColumnHeader();
            this.numOfFinger = new System.Windows.Forms.ColumnHeader();
            this.cardID = new System.Windows.Forms.ColumnHeader();
            this.addUser = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numOfFaces = new System.Windows.Forms.TextBox();
            this.faces = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numOfUser = new System.Windows.Forms.TextBox();
            this.deviceInfo = new System.Windows.Forms.TextBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.deleteAllButton = new System.Windows.Forms.Button();
            this.updateUser = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.groupBox8);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.userList);
            this.groupBox2.Location = new System.Drawing.Point(14, 85);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(618, 528);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "User DB";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.accessGroup);
            this.groupBox6.Controls.Add(this.authMode);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this.label15);
            this.groupBox6.Location = new System.Drawing.Point(265, 232);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(340, 69);
            this.groupBox6.TabIndex = 24;
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
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(13, 44);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(86, 12);
            this.label17.TabIndex = 16;
            this.label17.Text = "Access Group";
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
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.button_Delete);
            this.groupBox8.Controls.Add(this.button_Add);
            this.groupBox8.Controls.Add(this.pictureBox1);
            this.groupBox8.Controls.Add(this.listBox1);
            this.groupBox8.Controls.Add(this.numOfFace);
            this.groupBox8.Controls.Add(this.label19);
            this.groupBox8.Controls.Add(this.ScanFaceTemplate);
            this.groupBox8.Location = new System.Drawing.Point(267, 307);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(338, 215);
            this.groupBox8.TabIndex = 23;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Face(FaceStation Only)";
            // 
            // button_Delete
            // 
            this.button_Delete.Location = new System.Drawing.Point(240, 160);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(81, 22);
            this.button_Delete.TabIndex = 7;
            this.button_Delete.Text = "Delete";
            this.button_Delete.UseVisualStyleBackColor = true;
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // button_Add
            // 
            this.button_Add.Location = new System.Drawing.Point(241, 132);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(81, 22);
            this.button_Add.TabIndex = 6;
            this.button_Add.Text = "Add";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(14, 130);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(81, 82);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(14, 38);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(307, 88);
            this.listBox1.TabIndex = 4;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // numOfFace
            // 
            this.numOfFace.Location = new System.Drawing.Point(247, 13);
            this.numOfFace.Name = "numOfFace";
            this.numOfFace.ReadOnly = true;
            this.numOfFace.Size = new System.Drawing.Size(74, 21);
            this.numOfFace.TabIndex = 3;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(173, 17);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(64, 12);
            this.label19.TabIndex = 2;
            this.label19.Text = "Face Num";
            // 
            // ScanFaceTemplate
            // 
            this.ScanFaceTemplate.Location = new System.Drawing.Point(241, 187);
            this.ScanFaceTemplate.Name = "ScanFaceTemplate";
            this.ScanFaceTemplate.Size = new System.Drawing.Size(81, 22);
            this.ScanFaceTemplate.TabIndex = 1;
            this.ScanFaceTemplate.Text = "Scan Face";
            this.ScanFaceTemplate.UseVisualStyleBackColor = true;
            this.ScanFaceTemplate.Click += new System.EventHandler(this.ScanFaceTemplate_Click);
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
            // ID
            // 
            this.ID.Text = "User ID";
            this.ID.Width = 71;
            // 
            // numOfFinger
            // 
            this.numOfFinger.Text = "Face";
            this.numOfFinger.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numOfFinger.Width = 62;
            // 
            // cardID
            // 
            this.cardID.Text = "Card ID";
            this.cardID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.cardID.Width = 90;
            // 
            // addUser
            // 
            this.addUser.Location = new System.Drawing.Point(16, 619);
            this.addUser.Name = "addUser";
            this.addUser.Size = new System.Drawing.Size(75, 23);
            this.addUser.TabIndex = 24;
            this.addUser.Text = "Add";
            this.addUser.UseVisualStyleBackColor = true;
            this.addUser.Click += new System.EventHandler(this.addUser_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(410, 619);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 23;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numOfFaces);
            this.groupBox1.Controls.Add(this.faces);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numOfUser);
            this.groupBox1.Controls.Add(this.deviceInfo);
            this.groupBox1.Location = new System.Drawing.Point(15, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(618, 58);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Device";
            // 
            // numOfFaces
            // 
            this.numOfFaces.Location = new System.Drawing.Point(422, 21);
            this.numOfFaces.Name = "numOfFaces";
            this.numOfFaces.ReadOnly = true;
            this.numOfFaces.Size = new System.Drawing.Size(66, 21);
            this.numOfFaces.TabIndex = 6;
            // 
            // faces
            // 
            this.faces.AutoSize = true;
            this.faces.Location = new System.Drawing.Point(378, 25);
            this.faces.Name = "faces";
            this.faces.Size = new System.Drawing.Size(36, 12);
            this.faces.TabIndex = 5;
            this.faces.Text = "faces";
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
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(558, 619);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 22;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(255, 619);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 20;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // deleteAllButton
            // 
            this.deleteAllButton.Location = new System.Drawing.Point(332, 619);
            this.deleteAllButton.Name = "deleteAllButton";
            this.deleteAllButton.Size = new System.Drawing.Size(75, 23);
            this.deleteAllButton.TabIndex = 21;
            this.deleteAllButton.Text = "Delete All";
            this.deleteAllButton.UseVisualStyleBackColor = true;
            this.deleteAllButton.Click += new System.EventHandler(this.deleteAllButton_Click);
            // 
            // updateUser
            // 
            this.updateUser.Location = new System.Drawing.Point(97, 619);
            this.updateUser.Name = "updateUser";
            this.updateUser.Size = new System.Drawing.Size(75, 23);
            this.updateUser.TabIndex = 19;
            this.updateUser.Text = "Update";
            this.updateUser.UseVisualStyleBackColor = true;
            this.updateUser.Click += new System.EventHandler(this.updateUser_Click);
            // 
            // UserManagement_FST
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 655);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.addUser);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.deleteAllButton);
            this.Controls.Add(this.updateUser);
            this.Name = "UserManagement_FST";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.UserManagement_FST_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox securityLevel;
        private System.Windows.Forms.ComboBox cardType;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TextBox numOfFace;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button ScanFaceTemplate;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button readCard;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox cardCustomID;
        private System.Windows.Forms.TextBox userCardID;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.DateTimePicker expiryDate;
        private System.Windows.Forms.DateTimePicker startDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox userLevel;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.TextBox userID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView userList;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader numOfFinger;
        private System.Windows.Forms.ColumnHeader cardID;
        private System.Windows.Forms.Button addUser;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox numOfFaces;
        private System.Windows.Forms.Label faces;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox numOfUser;
        private System.Windows.Forms.TextBox deviceInfo;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button deleteAllButton;
        private System.Windows.Forms.Button updateUser;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox accessGroup;
        private System.Windows.Forms.ComboBox authMode;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button_Delete;
        private System.Windows.Forms.Button button_Add;
    }
}