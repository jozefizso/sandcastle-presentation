namespace SandcastleBuilder.PlugIns
{
    partial class DeploymentConfigDlg
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
            if(disposing && (components != null))
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
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lnkCodePlexSHFB = new System.Windows.Forms.LinkLabel();
            this.epErrors = new System.Windows.Forms.ErrorProvider(this.components);
            this.txt1xPassword = new System.Windows.Forms.TextBox();
            this.txt1xUserName = new System.Windows.Forms.TextBox();
            this.txt1xTargetLocation = new System.Windows.Forms.TextBox();
            this.txt1xProxyUserName = new System.Windows.Forms.TextBox();
            this.txt1xProxyPassword = new System.Windows.Forms.TextBox();
            this.txt1xProxyServer = new System.Windows.Forms.TextBox();
            this.txt2xUserName = new System.Windows.Forms.TextBox();
            this.txt2xPassword = new System.Windows.Forms.TextBox();
            this.txt2xTargetLocation = new System.Windows.Forms.TextBox();
            this.txt2xProxyUserName = new System.Windows.Forms.TextBox();
            this.txt2xProxyServer = new System.Windows.Forms.TextBox();
            this.txt2xProxyPassword = new System.Windows.Forms.TextBox();
            this.txtWsUserName = new System.Windows.Forms.TextBox();
            this.txtWsPassword = new System.Windows.Forms.TextBox();
            this.txtWsTargetLocation = new System.Windows.Forms.TextBox();
            this.txtWsProxyUserName = new System.Windows.Forms.TextBox();
            this.txtWsProxyServer = new System.Windows.Forms.TextBox();
            this.txtWsProxyPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chk1xUseDefaultCredentials = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chk1xUseProxyServer = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chk1xUseProxyDefCreds = new System.Windows.Forms.CheckBox();
            this.tabConfig = new System.Windows.Forms.TabControl();
            this.pgHelp1x = new System.Windows.Forms.TabPage();
            this.pgHelp2x = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.chk2xUseDefaultCredentials = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chk2xUseProxyServer = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.chk2xUseProxyDefCreds = new System.Windows.Forms.CheckBox();
            this.pgWebsite = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.chkWsUseDefaultCredentials = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chkWsUseProxyServer = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.chkWsUseProxyDefCreds = new System.Windows.Forms.CheckBox();
            this.chkDeleteAfterDeploy = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.epErrors)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabConfig.SuspendLayout();
            this.pgHelp1x.SuspendLayout();
            this.pgHelp2x.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.pgWebsite.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(538, 357);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 32);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.toolTip1.SetToolTip(this.btnCancel, "Exit without saving changes");
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(12, 357);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(88, 32);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.toolTip1.SetToolTip(this.btnOK, "Save changes to configuration");
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lnkCodePlexSHFB
            // 
            this.lnkCodePlexSHFB.Location = new System.Drawing.Point(210, 362);
            this.lnkCodePlexSHFB.Name = "lnkCodePlexSHFB";
            this.lnkCodePlexSHFB.Size = new System.Drawing.Size(218, 23);
            this.lnkCodePlexSHFB.TabIndex = 4;
            this.lnkCodePlexSHFB.TabStop = true;
            this.lnkCodePlexSHFB.Text = "Sandcastle Help File Builder";
            this.lnkCodePlexSHFB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lnkCodePlexSHFB, "http://SHFB.CodePlex.com");
            this.lnkCodePlexSHFB.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.codePlex_LinkClicked);
            // 
            // epErrors
            // 
            this.epErrors.ContainerControl = this;
            // 
            // txt1xPassword
            // 
            this.txt1xPassword.Enabled = false;
            this.epErrors.SetIconPadding(this.txt1xPassword, 35);
            this.txt1xPassword.Location = new System.Drawing.Point(398, 44);
            this.txt1xPassword.MaxLength = 50;
            this.txt1xPassword.Name = "txt1xPassword";
            this.txt1xPassword.Size = new System.Drawing.Size(164, 22);
            this.txt1xPassword.TabIndex = 4;
            // 
            // txt1xUserName
            // 
            this.txt1xUserName.Enabled = false;
            this.epErrors.SetIconPadding(this.txt1xUserName, 35);
            this.txt1xUserName.Location = new System.Drawing.Point(128, 44);
            this.txt1xUserName.MaxLength = 50;
            this.txt1xUserName.Name = "txt1xUserName";
            this.txt1xUserName.Size = new System.Drawing.Size(164, 22);
            this.txt1xUserName.TabIndex = 2;
            // 
            // txt1xTargetLocation
            // 
            this.epErrors.SetIconPadding(this.txt1xTargetLocation, 35);
            this.txt1xTargetLocation.Location = new System.Drawing.Point(134, 18);
            this.txt1xTargetLocation.MaxLength = 256;
            this.txt1xTargetLocation.Name = "txt1xTargetLocation";
            this.txt1xTargetLocation.Size = new System.Drawing.Size(412, 22);
            this.txt1xTargetLocation.TabIndex = 1;
            // 
            // txt1xProxyUserName
            // 
            this.txt1xProxyUserName.Enabled = false;
            this.epErrors.SetIconPadding(this.txt1xProxyUserName, 35);
            this.txt1xProxyUserName.Location = new System.Drawing.Point(128, 100);
            this.txt1xProxyUserName.MaxLength = 50;
            this.txt1xProxyUserName.Name = "txt1xProxyUserName";
            this.txt1xProxyUserName.Size = new System.Drawing.Size(164, 22);
            this.txt1xProxyUserName.TabIndex = 5;
            // 
            // txt1xProxyPassword
            // 
            this.txt1xProxyPassword.Enabled = false;
            this.epErrors.SetIconPadding(this.txt1xProxyPassword, 35);
            this.txt1xProxyPassword.Location = new System.Drawing.Point(398, 100);
            this.txt1xProxyPassword.MaxLength = 50;
            this.txt1xProxyPassword.Name = "txt1xProxyPassword";
            this.txt1xProxyPassword.Size = new System.Drawing.Size(164, 22);
            this.txt1xProxyPassword.TabIndex = 7;
            // 
            // txt1xProxyServer
            // 
            this.txt1xProxyServer.Enabled = false;
            this.epErrors.SetIconPadding(this.txt1xProxyServer, 35);
            this.txt1xProxyServer.Location = new System.Drawing.Point(128, 48);
            this.txt1xProxyServer.MaxLength = 256;
            this.txt1xProxyServer.Name = "txt1xProxyServer";
            this.txt1xProxyServer.Size = new System.Drawing.Size(412, 22);
            this.txt1xProxyServer.TabIndex = 2;
            // 
            // txt2xUserName
            // 
            this.txt2xUserName.Enabled = false;
            this.epErrors.SetIconPadding(this.txt2xUserName, 35);
            this.txt2xUserName.Location = new System.Drawing.Point(128, 44);
            this.txt2xUserName.MaxLength = 50;
            this.txt2xUserName.Name = "txt2xUserName";
            this.txt2xUserName.Size = new System.Drawing.Size(164, 22);
            this.txt2xUserName.TabIndex = 2;
            // 
            // txt2xPassword
            // 
            this.txt2xPassword.Enabled = false;
            this.epErrors.SetIconPadding(this.txt2xPassword, 35);
            this.txt2xPassword.Location = new System.Drawing.Point(398, 44);
            this.txt2xPassword.MaxLength = 50;
            this.txt2xPassword.Name = "txt2xPassword";
            this.txt2xPassword.Size = new System.Drawing.Size(164, 22);
            this.txt2xPassword.TabIndex = 4;
            // 
            // txt2xTargetLocation
            // 
            this.epErrors.SetIconPadding(this.txt2xTargetLocation, 35);
            this.txt2xTargetLocation.Location = new System.Drawing.Point(134, 18);
            this.txt2xTargetLocation.MaxLength = 256;
            this.txt2xTargetLocation.Name = "txt2xTargetLocation";
            this.txt2xTargetLocation.Size = new System.Drawing.Size(412, 22);
            this.txt2xTargetLocation.TabIndex = 5;
            // 
            // txt2xProxyUserName
            // 
            this.txt2xProxyUserName.Enabled = false;
            this.epErrors.SetIconPadding(this.txt2xProxyUserName, 35);
            this.txt2xProxyUserName.Location = new System.Drawing.Point(128, 100);
            this.txt2xProxyUserName.MaxLength = 50;
            this.txt2xProxyUserName.Name = "txt2xProxyUserName";
            this.txt2xProxyUserName.Size = new System.Drawing.Size(164, 22);
            this.txt2xProxyUserName.TabIndex = 5;
            // 
            // txt2xProxyServer
            // 
            this.txt2xProxyServer.Enabled = false;
            this.epErrors.SetIconPadding(this.txt2xProxyServer, 35);
            this.txt2xProxyServer.Location = new System.Drawing.Point(128, 48);
            this.txt2xProxyServer.MaxLength = 256;
            this.txt2xProxyServer.Name = "txt2xProxyServer";
            this.txt2xProxyServer.Size = new System.Drawing.Size(412, 22);
            this.txt2xProxyServer.TabIndex = 2;
            // 
            // txt2xProxyPassword
            // 
            this.txt2xProxyPassword.Enabled = false;
            this.epErrors.SetIconPadding(this.txt2xProxyPassword, 35);
            this.txt2xProxyPassword.Location = new System.Drawing.Point(398, 100);
            this.txt2xProxyPassword.MaxLength = 50;
            this.txt2xProxyPassword.Name = "txt2xProxyPassword";
            this.txt2xProxyPassword.Size = new System.Drawing.Size(164, 22);
            this.txt2xProxyPassword.TabIndex = 7;
            // 
            // txtWsUserName
            // 
            this.txtWsUserName.Enabled = false;
            this.epErrors.SetIconPadding(this.txtWsUserName, 35);
            this.txtWsUserName.Location = new System.Drawing.Point(128, 44);
            this.txtWsUserName.MaxLength = 50;
            this.txtWsUserName.Name = "txtWsUserName";
            this.txtWsUserName.Size = new System.Drawing.Size(164, 22);
            this.txtWsUserName.TabIndex = 2;
            // 
            // txtWsPassword
            // 
            this.txtWsPassword.Enabled = false;
            this.epErrors.SetIconPadding(this.txtWsPassword, 35);
            this.txtWsPassword.Location = new System.Drawing.Point(398, 44);
            this.txtWsPassword.MaxLength = 50;
            this.txtWsPassword.Name = "txtWsPassword";
            this.txtWsPassword.Size = new System.Drawing.Size(164, 22);
            this.txtWsPassword.TabIndex = 4;
            // 
            // txtWsTargetLocation
            // 
            this.epErrors.SetIconPadding(this.txtWsTargetLocation, 35);
            this.txtWsTargetLocation.Location = new System.Drawing.Point(134, 18);
            this.txtWsTargetLocation.MaxLength = 256;
            this.txtWsTargetLocation.Name = "txtWsTargetLocation";
            this.txtWsTargetLocation.Size = new System.Drawing.Size(412, 22);
            this.txtWsTargetLocation.TabIndex = 5;
            // 
            // txtWsProxyUserName
            // 
            this.txtWsProxyUserName.Enabled = false;
            this.epErrors.SetIconPadding(this.txtWsProxyUserName, 35);
            this.txtWsProxyUserName.Location = new System.Drawing.Point(128, 100);
            this.txtWsProxyUserName.MaxLength = 50;
            this.txtWsProxyUserName.Name = "txtWsProxyUserName";
            this.txtWsProxyUserName.Size = new System.Drawing.Size(164, 22);
            this.txtWsProxyUserName.TabIndex = 5;
            // 
            // txtWsProxyServer
            // 
            this.txtWsProxyServer.Enabled = false;
            this.epErrors.SetIconPadding(this.txtWsProxyServer, 35);
            this.txtWsProxyServer.Location = new System.Drawing.Point(128, 48);
            this.txtWsProxyServer.MaxLength = 256;
            this.txtWsProxyServer.Name = "txtWsProxyServer";
            this.txtWsProxyServer.Size = new System.Drawing.Size(412, 22);
            this.txtWsProxyServer.TabIndex = 2;
            // 
            // txtWsProxyPassword
            // 
            this.txtWsProxyPassword.Enabled = false;
            this.epErrors.SetIconPadding(this.txtWsProxyPassword, 35);
            this.txtWsProxyPassword.Location = new System.Drawing.Point(398, 100);
            this.txtWsProxyPassword.MaxLength = 50;
            this.txtWsProxyPassword.Name = "txtWsProxyPassword";
            this.txtWsProxyPassword.Size = new System.Drawing.Size(164, 22);
            this.txtWsProxyPassword.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(17, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(111, 23);
            this.label7.TabIndex = 0;
            this.label7.Text = "&Target Location";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chk1xUseDefaultCredentials
            // 
            this.chk1xUseDefaultCredentials.Checked = true;
            this.chk1xUseDefaultCredentials.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk1xUseDefaultCredentials.Location = new System.Drawing.Point(128, 20);
            this.chk1xUseDefaultCredentials.Name = "chk1xUseDefaultCredentials";
            this.chk1xUseDefaultCredentials.Size = new System.Drawing.Size(190, 21);
            this.chk1xUseDefaultCredentials.TabIndex = 0;
            this.chk1xUseDefaultCredentials.Text = "Use &Default Credentials";
            this.chk1xUseDefaultCredentials.UseVisualStyleBackColor = true;
            this.chk1xUseDefaultCredentials.CheckedChanged += new System.EventHandler(this.UseDefaultCredentials_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(311, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Pass&word";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(34, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "&User Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt1xUserName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txt1xPassword);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.chk1xUseDefaultCredentials);
            this.groupBox2.Location = new System.Drawing.Point(6, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(587, 78);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "User Credentials";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chk1xUseProxyServer);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txt1xProxyUserName);
            this.groupBox3.Controls.Add(this.txt1xProxyServer);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.txt1xProxyPassword);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.chk1xUseProxyDefCreds);
            this.groupBox3.Location = new System.Drawing.Point(6, 130);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(587, 138);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Proxy Credentials";
            // 
            // chk1xUseProxyServer
            // 
            this.chk1xUseProxyServer.Location = new System.Drawing.Point(128, 21);
            this.chk1xUseProxyServer.Name = "chk1xUseProxyServer";
            this.chk1xUseProxyServer.Size = new System.Drawing.Size(153, 21);
            this.chk1xUseProxyServer.TabIndex = 0;
            this.chk1xUseProxyServer.Text = "User Pr&oxy Server";
            this.chk1xUseProxyServer.UseVisualStyleBackColor = true;
            this.chk1xUseProxyServer.CheckedChanged += new System.EventHandler(this.UseProxyServer_CheckedChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(14, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 23);
            this.label5.TabIndex = 1;
            this.label5.Text = "Pro&xy Server";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(311, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Pa&ssword";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(34, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Us&er Name";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chk1xUseProxyDefCreds
            // 
            this.chk1xUseProxyDefCreds.Checked = true;
            this.chk1xUseProxyDefCreds.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk1xUseProxyDefCreds.Enabled = false;
            this.chk1xUseProxyDefCreds.Location = new System.Drawing.Point(128, 76);
            this.chk1xUseProxyDefCreds.Name = "chk1xUseProxyDefCreds";
            this.chk1xUseProxyDefCreds.Size = new System.Drawing.Size(190, 21);
            this.chk1xUseProxyDefCreds.TabIndex = 3;
            this.chk1xUseProxyDefCreds.Text = "Use &Default &Credentials";
            this.chk1xUseProxyDefCreds.UseVisualStyleBackColor = true;
            this.chk1xUseProxyDefCreds.CheckedChanged += new System.EventHandler(this.UseProxyDefCreds_CheckedChanged);
            // 
            // tabConfig
            // 
            this.tabConfig.Controls.Add(this.pgHelp1x);
            this.tabConfig.Controls.Add(this.pgHelp2x);
            this.tabConfig.Controls.Add(this.pgWebsite);
            this.tabConfig.Location = new System.Drawing.Point(12, 42);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.SelectedIndex = 0;
            this.tabConfig.Size = new System.Drawing.Size(614, 310);
            this.tabConfig.TabIndex = 1;
            // 
            // pgHelp1x
            // 
            this.pgHelp1x.Controls.Add(this.label7);
            this.pgHelp1x.Controls.Add(this.groupBox2);
            this.pgHelp1x.Controls.Add(this.txt1xTargetLocation);
            this.pgHelp1x.Controls.Add(this.groupBox3);
            this.pgHelp1x.Location = new System.Drawing.Point(4, 25);
            this.pgHelp1x.Name = "pgHelp1x";
            this.pgHelp1x.Padding = new System.Windows.Forms.Padding(3);
            this.pgHelp1x.Size = new System.Drawing.Size(606, 281);
            this.pgHelp1x.TabIndex = 0;
            this.pgHelp1x.Text = "HTML Help 1.x";
            this.pgHelp1x.UseVisualStyleBackColor = true;
            // 
            // pgHelp2x
            // 
            this.pgHelp2x.Controls.Add(this.label6);
            this.pgHelp2x.Controls.Add(this.groupBox1);
            this.pgHelp2x.Controls.Add(this.txt2xTargetLocation);
            this.pgHelp2x.Controls.Add(this.groupBox4);
            this.pgHelp2x.Location = new System.Drawing.Point(4, 25);
            this.pgHelp2x.Name = "pgHelp2x";
            this.pgHelp2x.Padding = new System.Windows.Forms.Padding(3);
            this.pgHelp2x.Size = new System.Drawing.Size(606, 281);
            this.pgHelp2x.TabIndex = 1;
            this.pgHelp2x.Text = "HTML Help 2.x";
            this.pgHelp2x.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(17, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 23);
            this.label6.TabIndex = 4;
            this.label6.Text = "&Target Location";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt2xUserName);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txt2xPassword);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.chk2xUseDefaultCredentials);
            this.groupBox1.Location = new System.Drawing.Point(6, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(587, 78);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Credentials";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(311, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 23);
            this.label8.TabIndex = 3;
            this.label8.Text = "Pass&word";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(34, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 23);
            this.label9.TabIndex = 1;
            this.label9.Text = "&User Name";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chk2xUseDefaultCredentials
            // 
            this.chk2xUseDefaultCredentials.Checked = true;
            this.chk2xUseDefaultCredentials.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk2xUseDefaultCredentials.Location = new System.Drawing.Point(128, 20);
            this.chk2xUseDefaultCredentials.Name = "chk2xUseDefaultCredentials";
            this.chk2xUseDefaultCredentials.Size = new System.Drawing.Size(190, 21);
            this.chk2xUseDefaultCredentials.TabIndex = 0;
            this.chk2xUseDefaultCredentials.Text = "Use &Default Credentials";
            this.chk2xUseDefaultCredentials.UseVisualStyleBackColor = true;
            this.chk2xUseDefaultCredentials.CheckedChanged += new System.EventHandler(this.UseDefaultCredentials_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chk2xUseProxyServer);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.txt2xProxyUserName);
            this.groupBox4.Controls.Add(this.txt2xProxyServer);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.txt2xProxyPassword);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.chk2xUseProxyDefCreds);
            this.groupBox4.Location = new System.Drawing.Point(6, 130);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(587, 138);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Proxy Credentials";
            // 
            // chk2xUseProxyServer
            // 
            this.chk2xUseProxyServer.Location = new System.Drawing.Point(128, 21);
            this.chk2xUseProxyServer.Name = "chk2xUseProxyServer";
            this.chk2xUseProxyServer.Size = new System.Drawing.Size(153, 21);
            this.chk2xUseProxyServer.TabIndex = 0;
            this.chk2xUseProxyServer.Text = "User Pr&oxy Server";
            this.chk2xUseProxyServer.UseVisualStyleBackColor = true;
            this.chk2xUseProxyServer.CheckedChanged += new System.EventHandler(this.UseProxyServer_CheckedChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(14, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(108, 23);
            this.label10.TabIndex = 1;
            this.label10.Text = "Pro&xy Server";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(311, 100);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 23);
            this.label11.TabIndex = 6;
            this.label11.Text = "Pa&ssword";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(34, 100);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 23);
            this.label12.TabIndex = 4;
            this.label12.Text = "Us&er Name";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chk2xUseProxyDefCreds
            // 
            this.chk2xUseProxyDefCreds.Checked = true;
            this.chk2xUseProxyDefCreds.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk2xUseProxyDefCreds.Enabled = false;
            this.chk2xUseProxyDefCreds.Location = new System.Drawing.Point(128, 76);
            this.chk2xUseProxyDefCreds.Name = "chk2xUseProxyDefCreds";
            this.chk2xUseProxyDefCreds.Size = new System.Drawing.Size(190, 21);
            this.chk2xUseProxyDefCreds.TabIndex = 3;
            this.chk2xUseProxyDefCreds.Text = "Use &Default &Credentials";
            this.chk2xUseProxyDefCreds.UseVisualStyleBackColor = true;
            this.chk2xUseProxyDefCreds.CheckedChanged += new System.EventHandler(this.UseProxyDefCreds_CheckedChanged);
            // 
            // pgWebsite
            // 
            this.pgWebsite.Controls.Add(this.label13);
            this.pgWebsite.Controls.Add(this.groupBox5);
            this.pgWebsite.Controls.Add(this.txtWsTargetLocation);
            this.pgWebsite.Controls.Add(this.groupBox6);
            this.pgWebsite.Location = new System.Drawing.Point(4, 25);
            this.pgWebsite.Name = "pgWebsite";
            this.pgWebsite.Padding = new System.Windows.Forms.Padding(3);
            this.pgWebsite.Size = new System.Drawing.Size(606, 281);
            this.pgWebsite.TabIndex = 2;
            this.pgWebsite.Text = "Website";
            this.pgWebsite.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(17, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(111, 23);
            this.label13.TabIndex = 4;
            this.label13.Text = "&Target Location";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtWsUserName);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.txtWsPassword);
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this.chkWsUseDefaultCredentials);
            this.groupBox5.Location = new System.Drawing.Point(6, 46);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(587, 78);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "User Credentials";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(311, 44);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(81, 23);
            this.label14.TabIndex = 3;
            this.label14.Text = "Pass&word";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(34, 44);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(88, 23);
            this.label15.TabIndex = 1;
            this.label15.Text = "&User Name";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkWsUseDefaultCredentials
            // 
            this.chkWsUseDefaultCredentials.Checked = true;
            this.chkWsUseDefaultCredentials.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWsUseDefaultCredentials.Location = new System.Drawing.Point(128, 20);
            this.chkWsUseDefaultCredentials.Name = "chkWsUseDefaultCredentials";
            this.chkWsUseDefaultCredentials.Size = new System.Drawing.Size(190, 21);
            this.chkWsUseDefaultCredentials.TabIndex = 0;
            this.chkWsUseDefaultCredentials.Text = "Use &Default Credentials";
            this.chkWsUseDefaultCredentials.UseVisualStyleBackColor = true;
            this.chkWsUseDefaultCredentials.CheckedChanged += new System.EventHandler(this.UseDefaultCredentials_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.chkWsUseProxyServer);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.txtWsProxyUserName);
            this.groupBox6.Controls.Add(this.txtWsProxyServer);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this.txtWsProxyPassword);
            this.groupBox6.Controls.Add(this.label18);
            this.groupBox6.Controls.Add(this.chkWsUseProxyDefCreds);
            this.groupBox6.Location = new System.Drawing.Point(6, 130);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(587, 138);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Proxy Credentials";
            // 
            // chkWsUseProxyServer
            // 
            this.chkWsUseProxyServer.Location = new System.Drawing.Point(128, 21);
            this.chkWsUseProxyServer.Name = "chkWsUseProxyServer";
            this.chkWsUseProxyServer.Size = new System.Drawing.Size(153, 21);
            this.chkWsUseProxyServer.TabIndex = 0;
            this.chkWsUseProxyServer.Text = "User Pr&oxy Server";
            this.chkWsUseProxyServer.UseVisualStyleBackColor = true;
            this.chkWsUseProxyServer.CheckedChanged += new System.EventHandler(this.UseProxyServer_CheckedChanged);
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(14, 48);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(108, 23);
            this.label16.TabIndex = 1;
            this.label16.Text = "Pro&xy Server";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(311, 100);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(81, 23);
            this.label17.TabIndex = 6;
            this.label17.Text = "Pa&ssword";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(34, 100);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(88, 23);
            this.label18.TabIndex = 4;
            this.label18.Text = "Us&er Name";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkWsUseProxyDefCreds
            // 
            this.chkWsUseProxyDefCreds.Checked = true;
            this.chkWsUseProxyDefCreds.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWsUseProxyDefCreds.Enabled = false;
            this.chkWsUseProxyDefCreds.Location = new System.Drawing.Point(128, 76);
            this.chkWsUseProxyDefCreds.Name = "chkWsUseProxyDefCreds";
            this.chkWsUseProxyDefCreds.Size = new System.Drawing.Size(190, 21);
            this.chkWsUseProxyDefCreds.TabIndex = 3;
            this.chkWsUseProxyDefCreds.Text = "Use &Default &Credentials";
            this.chkWsUseProxyDefCreds.UseVisualStyleBackColor = true;
            this.chkWsUseProxyDefCreds.CheckedChanged += new System.EventHandler(this.UseProxyDefCreds_CheckedChanged);
            // 
            // chkDeleteAfterDeploy
            // 
            this.chkDeleteAfterDeploy.Location = new System.Drawing.Point(12, 12);
            this.chkDeleteAfterDeploy.Name = "chkDeleteAfterDeploy";
            this.chkDeleteAfterDeploy.Size = new System.Drawing.Size(291, 24);
            this.chkDeleteAfterDeploy.TabIndex = 0;
            this.chkDeleteAfterDeploy.Text = "Delete source files &after deploying them";
            this.chkDeleteAfterDeploy.UseVisualStyleBackColor = true;
            // 
            // DeploymentConfigDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(638, 401);
            this.Controls.Add(this.chkDeleteAfterDeploy);
            this.Controls.Add(this.tabConfig);
            this.Controls.Add(this.lnkCodePlexSHFB);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeploymentConfigDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Output Deployment Plug-In";
            ((System.ComponentModel.ISupportInitialize)(this.epErrors)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabConfig.ResumeLayout(false);
            this.pgHelp1x.ResumeLayout(false);
            this.pgHelp1x.PerformLayout();
            this.pgHelp2x.ResumeLayout(false);
            this.pgHelp2x.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.pgWebsite.ResumeLayout(false);
            this.pgWebsite.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ErrorProvider epErrors;
        private System.Windows.Forms.LinkLabel lnkCodePlexSHFB;
        private System.Windows.Forms.CheckBox chk1xUseDefaultCredentials;
        private System.Windows.Forms.TextBox txt1xPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt1xUserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt1xTargetLocation;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chk1xUseProxyServer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt1xProxyUserName;
        private System.Windows.Forms.TextBox txt1xProxyServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt1xProxyPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chk1xUseProxyDefCreds;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabConfig;
        private System.Windows.Forms.TabPage pgHelp1x;
        private System.Windows.Forms.TabPage pgHelp2x;
        private System.Windows.Forms.TabPage pgWebsite;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt2xUserName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt2xPassword;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chk2xUseDefaultCredentials;
        private System.Windows.Forms.TextBox txt2xTargetLocation;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chk2xUseProxyServer;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt2xProxyUserName;
        private System.Windows.Forms.TextBox txt2xProxyServer;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt2xProxyPassword;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chk2xUseProxyDefCreds;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtWsUserName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtWsPassword;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox chkWsUseDefaultCredentials;
        private System.Windows.Forms.TextBox txtWsTargetLocation;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox chkWsUseProxyServer;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtWsProxyUserName;
        private System.Windows.Forms.TextBox txtWsProxyServer;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtWsProxyPassword;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox chkWsUseProxyDefCreds;
        private System.Windows.Forms.CheckBox chkDeleteAfterDeploy;
    }
}