namespace BioStarCSharp
{
    partial class AccessGroup
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
            this.deviceInfo = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.ReadWriteDoorConfig = new System.Windows.Forms.Button();
            this.SetAccessGroup = new System.Windows.Forms.Button();
            this.SetTimecode = new System.Windows.Forms.Button();
            this.SetHoliday = new System.Windows.Forms.Button();
            this.WriteDoorConfig = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // deviceInfo
            // 
            this.deviceInfo.Location = new System.Drawing.Point(16, 21);
            this.deviceInfo.Name = "deviceInfo";
            this.deviceInfo.ReadOnly = true;
            this.deviceInfo.Size = new System.Drawing.Size(235, 21);
            this.deviceInfo.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deviceInfo);
            this.groupBox1.Location = new System.Drawing.Point(-2, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(529, 58);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Device";
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(415, 331);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 11;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // ReadWriteDoorConfig
            // 
            this.ReadWriteDoorConfig.Location = new System.Drawing.Point(14, 82);
            this.ReadWriteDoorConfig.Name = "ReadWriteDoorConfig";
            this.ReadWriteDoorConfig.Size = new System.Drawing.Size(153, 23);
            this.ReadWriteDoorConfig.TabIndex = 12;
            this.ReadWriteDoorConfig.Text = "Read Door Config";
            this.ReadWriteDoorConfig.UseVisualStyleBackColor = true;
            this.ReadWriteDoorConfig.Click += new System.EventHandler(this.ReadDoorConfig_Click);
            // 
            // SetAccessGroup
            // 
            this.SetAccessGroup.Location = new System.Drawing.Point(14, 141);
            this.SetAccessGroup.Name = "SetAccessGroup";
            this.SetAccessGroup.Size = new System.Drawing.Size(153, 23);
            this.SetAccessGroup.TabIndex = 14;
            this.SetAccessGroup.Text = "Set Access group";
            this.SetAccessGroup.UseVisualStyleBackColor = true;
            this.SetAccessGroup.Click += new System.EventHandler(this.SetAccessGroup_Click);
            // 
            // SetTimecode
            // 
            this.SetTimecode.Location = new System.Drawing.Point(14, 184);
            this.SetTimecode.Name = "SetTimecode";
            this.SetTimecode.Size = new System.Drawing.Size(153, 23);
            this.SetTimecode.TabIndex = 15;
            this.SetTimecode.Text = "Set Timecode";
            this.SetTimecode.UseVisualStyleBackColor = true;
            this.SetTimecode.Click += new System.EventHandler(this.SetTimecode_Click);
            // 
            // SetHoliday
            // 
            this.SetHoliday.Location = new System.Drawing.Point(14, 226);
            this.SetHoliday.Name = "SetHoliday";
            this.SetHoliday.Size = new System.Drawing.Size(153, 23);
            this.SetHoliday.TabIndex = 16;
            this.SetHoliday.Text = "Set Holiday";
            this.SetHoliday.UseVisualStyleBackColor = true;
            this.SetHoliday.Click += new System.EventHandler(this.SetHoliday_Click);
            // 
            // WriteDoorConfig
            // 
            this.WriteDoorConfig.Location = new System.Drawing.Point(202, 82);
            this.WriteDoorConfig.Name = "WriteDoorConfig";
            this.WriteDoorConfig.Size = new System.Drawing.Size(153, 23);
            this.WriteDoorConfig.TabIndex = 17;
            this.WriteDoorConfig.Text = "Write Door Config";
            this.WriteDoorConfig.UseVisualStyleBackColor = true;
            this.WriteDoorConfig.Click += new System.EventHandler(this.WriteDoorConfig_Click);
            // 
            // AccessGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 366);
            this.Controls.Add(this.WriteDoorConfig);
            this.Controls.Add(this.SetHoliday);
            this.Controls.Add(this.SetTimecode);
            this.Controls.Add(this.SetAccessGroup);
            this.Controls.Add(this.ReadWriteDoorConfig);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.groupBox1);
            this.Name = "AccessGroup";
            this.Text = "AccessGroup";
            this.Load += new System.EventHandler(this.AccessGroup_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox deviceInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button ReadWriteDoorConfig;
        private System.Windows.Forms.Button SetAccessGroup;
        private System.Windows.Forms.Button SetTimecode;
        private System.Windows.Forms.Button SetHoliday;
        private System.Windows.Forms.Button WriteDoorConfig;
    }
}