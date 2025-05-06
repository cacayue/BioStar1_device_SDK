namespace SDK_TestApp
{
    partial class Form1
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

            workerObject.RequestStop(); 

            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.deviceList = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkMatchFail = new System.Windows.Forms.CheckBox();
            this.textConnection = new System.Windows.Forms.TextBox();
            this.textPort = new System.Windows.Forms.TextBox();
            this.checkUseLock = new System.Windows.Forms.CheckBox();
            this.checkUseFunctionLock = new System.Windows.Forms.CheckBox();
            this.checkUseAutoResponse = new System.Windows.Forms.CheckBox();
            this.buttonStartService = new System.Windows.Forms.Button();
            this.buttonStopService = new System.Windows.Forms.Button();
            this.buttonIssueSSLCert = new System.Windows.Forms.Button();
            this.buttonDeleteSSLCert = new System.Windows.Forms.Button();
            this.buttonStartRequest = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // deviceList
            // 
            this.deviceList.Location = new System.Drawing.Point(17, 18);
            this.deviceList.Name = "deviceList";
            this.deviceList.Size = new System.Drawing.Size(650, 337);
            this.deviceList.TabIndex = 0;
            this.deviceList.UseCompatibleStateImageBehavior = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deviceList);
            this.groupBox1.Location = new System.Drawing.Point(13, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(682, 361);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Device List";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.checkMatchFail);
            this.groupBox2.Controls.Add(this.textConnection);
            this.groupBox2.Controls.Add(this.textPort);
            this.groupBox2.Controls.Add(this.checkUseLock);
            this.groupBox2.Controls.Add(this.checkUseFunctionLock);
            this.groupBox2.Controls.Add(this.checkUseAutoResponse);
            this.groupBox2.Location = new System.Drawing.Point(14, 376);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(681, 104);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(223, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Max Connection";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "Port";
            // 
            // checkMatchFail
            // 
            this.checkMatchFail.AutoSize = true;
            this.checkMatchFail.Location = new System.Drawing.Point(485, 40);
            this.checkMatchFail.Name = "checkMatchFail";
            this.checkMatchFail.Size = new System.Drawing.Size(100, 16);
            this.checkMatchFail.TabIndex = 5;
            this.checkMatchFail.Text = "Matching Fail";
            this.checkMatchFail.UseVisualStyleBackColor = true;
            this.checkMatchFail.CheckedChanged += new System.EventHandler(this.checkMatchFail_CheckedChanged);
            // 
            // textConnection
            // 
            this.textConnection.Location = new System.Drawing.Point(327, 35);
            this.textConnection.Name = "textConnection";
            this.textConnection.Size = new System.Drawing.Size(100, 21);
            this.textConnection.TabIndex = 4;
            this.textConnection.Text = "32";
            // 
            // textPort
            // 
            this.textPort.Location = new System.Drawing.Point(88, 35);
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(100, 21);
            this.textPort.TabIndex = 3;
            this.textPort.Text = "5001";
            // 
            // checkUseLock
            // 
            this.checkUseLock.AutoSize = true;
            this.checkUseLock.Location = new System.Drawing.Point(378, 74);
            this.checkUseLock.Name = "checkUseLock";
            this.checkUseLock.Size = new System.Drawing.Size(77, 16);
            this.checkUseLock.TabIndex = 2;
            this.checkUseLock.Text = "Use Lock";
            this.checkUseLock.UseVisualStyleBackColor = true;
            this.checkUseLock.CheckedChanged += new System.EventHandler(this.checkUseLock_CheckedChanged);
            // 
            // checkUseFunctionLock
            // 
            this.checkUseFunctionLock.AutoSize = true;
            this.checkUseFunctionLock.Location = new System.Drawing.Point(210, 74);
            this.checkUseFunctionLock.Name = "checkUseFunctionLock";
            this.checkUseFunctionLock.Size = new System.Drawing.Size(129, 16);
            this.checkUseFunctionLock.TabIndex = 1;
            this.checkUseFunctionLock.Text = "Use Function Lock";
            this.checkUseFunctionLock.UseVisualStyleBackColor = true;
            this.checkUseFunctionLock.CheckedChanged += new System.EventHandler(this.checkUseFunctionLock_CheckedChanged);
            // 
            // checkUseAutoResponse
            // 
            this.checkUseAutoResponse.AutoSize = true;
            this.checkUseAutoResponse.Location = new System.Drawing.Point(41, 74);
            this.checkUseAutoResponse.Name = "checkUseAutoResponse";
            this.checkUseAutoResponse.Size = new System.Drawing.Size(136, 16);
            this.checkUseAutoResponse.TabIndex = 0;
            this.checkUseAutoResponse.Text = "Use Auto Response";
            this.checkUseAutoResponse.UseVisualStyleBackColor = true;
            this.checkUseAutoResponse.CheckedChanged += new System.EventHandler(this.checkUseAutoResponse_CheckedChanged);
            // 
            // buttonStartService
            // 
            this.buttonStartService.Location = new System.Drawing.Point(13, 489);
            this.buttonStartService.Name = "buttonStartService";
            this.buttonStartService.Size = new System.Drawing.Size(109, 81);
            this.buttonStartService.TabIndex = 3;
            this.buttonStartService.Text = "Start Service";
            this.buttonStartService.UseVisualStyleBackColor = true;
            this.buttonStartService.Click += new System.EventHandler(this.buttonStartService_Click);
            // 
            // buttonStopService
            // 
            this.buttonStopService.Location = new System.Drawing.Point(123, 489);
            this.buttonStopService.Name = "buttonStopService";
            this.buttonStopService.Size = new System.Drawing.Size(109, 81);
            this.buttonStopService.TabIndex = 4;
            this.buttonStopService.Text = "Stop Service";
            this.buttonStopService.UseVisualStyleBackColor = true;
            this.buttonStopService.Click += new System.EventHandler(this.buttonStopService_Click);
            // 
            // buttonIssueSSLCert
            // 
            this.buttonIssueSSLCert.Location = new System.Drawing.Point(281, 489);
            this.buttonIssueSSLCert.Name = "buttonIssueSSLCert";
            this.buttonIssueSSLCert.Size = new System.Drawing.Size(127, 81);
            this.buttonIssueSSLCert.TabIndex = 5;
            this.buttonIssueSSLCert.Text = "Issue SSL Cert";
            this.buttonIssueSSLCert.UseVisualStyleBackColor = true;
            // 
            // buttonDeleteSSLCert
            // 
            this.buttonDeleteSSLCert.Location = new System.Drawing.Point(409, 489);
            this.buttonDeleteSSLCert.Name = "buttonDeleteSSLCert";
            this.buttonDeleteSSLCert.Size = new System.Drawing.Size(127, 81);
            this.buttonDeleteSSLCert.TabIndex = 6;
            this.buttonDeleteSSLCert.Text = "Delete SSL Cert";
            this.buttonDeleteSSLCert.UseVisualStyleBackColor = true;
            // 
            // buttonStartRequest
            // 
            this.buttonStartRequest.Location = new System.Drawing.Point(568, 489);
            this.buttonStartRequest.Name = "buttonStartRequest";
            this.buttonStartRequest.Size = new System.Drawing.Size(127, 81);
            this.buttonStartRequest.TabIndex = 7;
            this.buttonStartRequest.Text = "Start Request";
            this.buttonStartRequest.UseVisualStyleBackColor = true;
            this.buttonStartRequest.Click += new System.EventHandler(this.buttonStartRequest_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 580);
            this.Controls.Add(this.buttonStartRequest);
            this.Controls.Add(this.buttonDeleteSSLCert);
            this.Controls.Add(this.buttonIssueSSLCert);
            this.Controls.Add(this.buttonStopService);
            this.Controls.Add(this.buttonStartService);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Closed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView deviceList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkMatchFail;
        private System.Windows.Forms.TextBox textConnection;
        private System.Windows.Forms.TextBox textPort;
        private System.Windows.Forms.CheckBox checkUseLock;
        private System.Windows.Forms.CheckBox checkUseFunctionLock;
        private System.Windows.Forms.CheckBox checkUseAutoResponse;
        private System.Windows.Forms.Button buttonStartService;
        private System.Windows.Forms.Button buttonStopService;
        private System.Windows.Forms.Button buttonIssueSSLCert;
        private System.Windows.Forms.Button buttonDeleteSSLCert;
        private System.Windows.Forms.Button buttonStartRequest;
    }
}

