namespace BioStarCSharp
{
    partial class BioStar
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.AccessGroup = new System.Windows.Forms.Button();
            this.SetConfig = new System.Windows.Forms.Button();
            this.SizeOfStructure = new System.Windows.Forms.Button();
            this.LedSet = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.logTest = new System.Windows.Forms.Button();
            this.userTest = new System.Windows.Forms.Button();
            this.timeTest = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.networkConfigButton = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.deviceList = new System.Windows.Forms.ListBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.connectedDeviceList = new System.Windows.Forms.ListBox();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.AccessGroup);
            this.groupBox3.Controls.Add(this.SetConfig);
            this.groupBox3.Controls.Add(this.SizeOfStructure);
            this.groupBox3.Controls.Add(this.LedSet);
            this.groupBox3.Controls.Add(this.closeButton);
            this.groupBox3.Controls.Add(this.logTest);
            this.groupBox3.Controls.Add(this.userTest);
            this.groupBox3.Controls.Add(this.timeTest);
            this.groupBox3.Location = new System.Drawing.Point(12, 314);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(518, 121);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Function Test";
            // 
            // AccessGroup
            // 
            this.AccessGroup.Location = new System.Drawing.Point(21, 93);
            this.AccessGroup.Name = "AccessGroup";
            this.AccessGroup.Size = new System.Drawing.Size(113, 23);
            this.AccessGroup.TabIndex = 10;
            this.AccessGroup.Text = "Access Group";
            this.AccessGroup.UseVisualStyleBackColor = true;
            this.AccessGroup.Click += new System.EventHandler(this.AccessGroup_Click);
            // 
            // SetConfig
            // 
            this.SetConfig.Location = new System.Drawing.Point(262, 62);
            this.SetConfig.Name = "SetConfig";
            this.SetConfig.Size = new System.Drawing.Size(125, 23);
            this.SetConfig.TabIndex = 9;
            this.SetConfig.Text = "Read/Write Config";
            this.SetConfig.UseVisualStyleBackColor = true;
            this.SetConfig.Click += new System.EventHandler(this.SetConfig_Click);
            // 
            // SizeOfStructure
            // 
            this.SizeOfStructure.Location = new System.Drawing.Point(148, 62);
            this.SizeOfStructure.Name = "SizeOfStructure";
            this.SizeOfStructure.Size = new System.Drawing.Size(93, 23);
            this.SizeOfStructure.TabIndex = 8;
            this.SizeOfStructure.Text = "struct size";
            this.SizeOfStructure.UseVisualStyleBackColor = true;
            this.SizeOfStructure.Click += new System.EventHandler(this.SizeOfStructure_Click);
            // 
            // LedSet
            // 
            this.LedSet.Location = new System.Drawing.Point(21, 62);
            this.LedSet.Name = "LedSet";
            this.LedSet.Size = new System.Drawing.Size(93, 23);
            this.LedSet.TabIndex = 7;
            this.LedSet.Text = "Led Setting";
            this.LedSet.UseVisualStyleBackColor = true;
            this.LedSet.Click += new System.EventHandler(this.LedSet_Click);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(402, 21);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(93, 23);
            this.closeButton.TabIndex = 6;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // logTest
            // 
            this.logTest.Location = new System.Drawing.Point(275, 21);
            this.logTest.Name = "logTest";
            this.logTest.Size = new System.Drawing.Size(93, 23);
            this.logTest.TabIndex = 5;
            this.logTest.Text = "Log";
            this.logTest.UseVisualStyleBackColor = true;
            this.logTest.Click += new System.EventHandler(this.logTest_Click);
            // 
            // userTest
            // 
            this.userTest.Location = new System.Drawing.Point(148, 21);
            this.userTest.Name = "userTest";
            this.userTest.Size = new System.Drawing.Size(93, 23);
            this.userTest.TabIndex = 4;
            this.userTest.Text = "User";
            this.userTest.UseVisualStyleBackColor = true;
            this.userTest.Click += new System.EventHandler(this.userTest_Click);
            // 
            // timeTest
            // 
            this.timeTest.Location = new System.Drawing.Point(21, 21);
            this.timeTest.Name = "timeTest";
            this.timeTest.Size = new System.Drawing.Size(93, 23);
            this.timeTest.TabIndex = 3;
            this.timeTest.Text = "Time";
            this.timeTest.UseVisualStyleBackColor = true;
            this.timeTest.Click += new System.EventHandler(this.timeTest_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.networkConfigButton);
            this.groupBox1.Controls.Add(this.searchButton);
            this.groupBox1.Controls.Add(this.deviceList);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 296);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Devices";
            // 
            // networkConfigButton
            // 
            this.networkConfigButton.Location = new System.Drawing.Point(135, 261);
            this.networkConfigButton.Name = "networkConfigButton";
            this.networkConfigButton.Size = new System.Drawing.Size(93, 23);
            this.networkConfigButton.TabIndex = 2;
            this.networkConfigButton.Text = "Config";
            this.networkConfigButton.UseVisualStyleBackColor = true;
            this.networkConfigButton.Click += new System.EventHandler(this.networkConfigButton_Click);
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(28, 261);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(93, 23);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // deviceList
            // 
            this.deviceList.FormattingEnabled = true;
            this.deviceList.ItemHeight = 12;
            this.deviceList.Location = new System.Drawing.Point(11, 19);
            this.deviceList.Name = "deviceList";
            this.deviceList.Size = new System.Drawing.Size(235, 232);
            this.deviceList.TabIndex = 0;
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(82, 261);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(93, 23);
            this.clearButton.TabIndex = 3;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.clearButton);
            this.groupBox2.Controls.Add(this.connectedDeviceList);
            this.groupBox2.Location = new System.Drawing.Point(274, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 296);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Connected Devices";
            // 
            // connectedDeviceList
            // 
            this.connectedDeviceList.FormattingEnabled = true;
            this.connectedDeviceList.ItemHeight = 12;
            this.connectedDeviceList.Location = new System.Drawing.Point(11, 20);
            this.connectedDeviceList.Name = "connectedDeviceList";
            this.connectedDeviceList.Size = new System.Drawing.Size(235, 232);
            this.connectedDeviceList.TabIndex = 0;
            // 
            // BioStar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 447);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "BioStar";
            this.Text = "BioStar SDK Example";
            this.Load += new System.EventHandler(this.BioStar_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BioStar_FormClosed);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button logTest;
        private System.Windows.Forms.Button userTest;
        private System.Windows.Forms.Button timeTest;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button networkConfigButton;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.ListBox deviceList;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox connectedDeviceList;
        private System.Windows.Forms.Button LedSet;
        private System.Windows.Forms.Button SizeOfStructure;
        private System.Windows.Forms.Button SetConfig;
        private System.Windows.Forms.Button AccessGroup;
    }
}

