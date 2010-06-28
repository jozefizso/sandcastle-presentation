namespace SandcastleBuilder.Gui
{
    partial class UserPreferencesDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserPreferencesDlg));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.statusBarTextProvider1 = new SandcastleBuilder.Utils.Controls.StatusBarTextProvider(this.components);
            this.chkVerboseLogging = new System.Windows.Forms.CheckBox();
            this.txtHTMLHelp2ViewerPath = new System.Windows.Forms.TextBox();
            this.btnSelectViewer = new System.Windows.Forms.Button();
            this.udcASPNetDevServerPort = new System.Windows.Forms.NumericUpDown();
            this.tabPreferences = new System.Windows.Forms.TabControl();
            this.pgGeneral = new System.Windows.Forms.TabPage();
            this.chkOpenHelp = new System.Windows.Forms.CheckBox();
            this.cboBeforeBuildAction = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnEditorFont = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblEditorExample = new System.Windows.Forms.Label();
            this.btnBuildFont = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnBuildForeground = new System.Windows.Forms.Button();
            this.btnBuildBackground = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lblBuildExample = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pgContentEditors = new System.Windows.Forms.TabPage();
            this.btnDelete = new System.Windows.Forms.Button();
            this.ilButton = new System.Windows.Forms.ImageList(this.components);
            this.btnAddFile = new System.Windows.Forms.Button();
            this.pgProps = new SandcastleBuilder.Utils.Controls.CustomPropertyGrid();
            this.lbContentEditors = new SandcastleBuilder.Utils.Controls.RefreshableItemListBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.epErrors = new System.Windows.Forms.ErrorProvider(this.components);
            this.chkShowLineNumbers = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.udcASPNetDevServerPort)).BeginInit();
            this.tabPreferences.SuspendLayout();
            this.pgGeneral.SuspendLayout();
            this.pgContentEditors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.epErrors)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(620, 436);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 32);
            this.statusBarTextProvider1.SetStatusBarText(this.btnCancel, "Cancel: Close without saving preferences");
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.toolTip1.SetToolTip(this.btnCancel, "Close without saving");
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(12, 436);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(88, 32);
            this.statusBarTextProvider1.SetStatusBarText(this.btnOK, "OK: Save preferences");
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.toolTip1.SetToolTip(this.btnOK, "Save preferences");
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // chkVerboseLogging
            // 
            this.chkVerboseLogging.Location = new System.Drawing.Point(245, 128);
            this.chkVerboseLogging.Name = "chkVerboseLogging";
            this.chkVerboseLogging.Size = new System.Drawing.Size(292, 24);
            this.statusBarTextProvider1.SetStatusBarText(this.chkVerboseLogging, "Verbose Logging: Check this box to display all output messages.  Uncheck to displ" +
                    "ay summary messages only.");
            this.chkVerboseLogging.TabIndex = 7;
            this.chkVerboseLogging.Text = "&Build window verbose logging enabled";
            this.chkVerboseLogging.UseVisualStyleBackColor = true;
            // 
            // txtHTMLHelp2ViewerPath
            // 
            this.txtHTMLHelp2ViewerPath.Location = new System.Drawing.Point(245, 18);
            this.txtHTMLHelp2ViewerPath.Name = "txtHTMLHelp2ViewerPath";
            this.txtHTMLHelp2ViewerPath.Size = new System.Drawing.Size(358, 22);
            this.statusBarTextProvider1.SetStatusBarText(this.txtHTMLHelp2ViewerPath, "Help 2.x Viewer: Enter the path and filename of the application used to view HTML" +
                    " Help 2.x files");
            this.txtHTMLHelp2ViewerPath.TabIndex = 1;
            // 
            // btnSelectViewer
            // 
            this.btnSelectViewer.Location = new System.Drawing.Point(604, 17);
            this.btnSelectViewer.Name = "btnSelectViewer";
            this.btnSelectViewer.Size = new System.Drawing.Size(32, 25);
            this.statusBarTextProvider1.SetStatusBarText(this.btnSelectViewer, "Select Viewer: Browser for the HTML Help 2.x viewer application");
            this.btnSelectViewer.TabIndex = 2;
            this.btnSelectViewer.Text = "...";
            this.toolTip1.SetToolTip(this.btnSelectViewer, "Select HTML Help 2.x viewer application");
            this.btnSelectViewer.UseVisualStyleBackColor = true;
            this.btnSelectViewer.Click += new System.EventHandler(this.btnSelectViewer_Click);
            // 
            // udcASPNetDevServerPort
            // 
            this.udcASPNetDevServerPort.Location = new System.Drawing.Point(245, 46);
            this.udcASPNetDevServerPort.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.udcASPNetDevServerPort.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udcASPNetDevServerPort.Name = "udcASPNetDevServerPort";
            this.udcASPNetDevServerPort.Size = new System.Drawing.Size(70, 22);
            this.statusBarTextProvider1.SetStatusBarText(this.udcASPNetDevServerPort, "Server Port: Select the port to use when launching the ASP.NET Development Web Se" +
                    "rver");
            this.udcASPNetDevServerPort.TabIndex = 4;
            this.udcASPNetDevServerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.udcASPNetDevServerPort.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // tabPreferences
            // 
            this.tabPreferences.Controls.Add(this.pgGeneral);
            this.tabPreferences.Controls.Add(this.pgContentEditors);
            this.tabPreferences.Location = new System.Drawing.Point(12, 12);
            this.tabPreferences.Name = "tabPreferences";
            this.tabPreferences.SelectedIndex = 0;
            this.tabPreferences.Size = new System.Drawing.Size(696, 418);
            this.statusBarTextProvider1.SetStatusBarText(this.tabPreferences, "User preferences");
            this.tabPreferences.TabIndex = 0;
            // 
            // pgGeneral
            // 
            this.pgGeneral.Controls.Add(this.chkShowLineNumbers);
            this.pgGeneral.Controls.Add(this.chkOpenHelp);
            this.pgGeneral.Controls.Add(this.cboBeforeBuildAction);
            this.pgGeneral.Controls.Add(this.label7);
            this.pgGeneral.Controls.Add(this.btnEditorFont);
            this.pgGeneral.Controls.Add(this.label3);
            this.pgGeneral.Controls.Add(this.lblEditorExample);
            this.pgGeneral.Controls.Add(this.btnBuildFont);
            this.pgGeneral.Controls.Add(this.label4);
            this.pgGeneral.Controls.Add(this.btnBuildForeground);
            this.pgGeneral.Controls.Add(this.btnBuildBackground);
            this.pgGeneral.Controls.Add(this.label5);
            this.pgGeneral.Controls.Add(this.lblBuildExample);
            this.pgGeneral.Controls.Add(this.label6);
            this.pgGeneral.Controls.Add(this.txtHTMLHelp2ViewerPath);
            this.pgGeneral.Controls.Add(this.udcASPNetDevServerPort);
            this.pgGeneral.Controls.Add(this.chkVerboseLogging);
            this.pgGeneral.Controls.Add(this.label2);
            this.pgGeneral.Controls.Add(this.label1);
            this.pgGeneral.Controls.Add(this.btnSelectViewer);
            this.pgGeneral.Location = new System.Drawing.Point(4, 25);
            this.pgGeneral.Name = "pgGeneral";
            this.pgGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.pgGeneral.Size = new System.Drawing.Size(688, 389);
            this.statusBarTextProvider1.SetStatusBarText(this.pgGeneral, "General user preferences");
            this.pgGeneral.TabIndex = 0;
            this.pgGeneral.Text = "General Preferences";
            this.pgGeneral.UseVisualStyleBackColor = true;
            // 
            // chkOpenHelp
            // 
            this.chkOpenHelp.Location = new System.Drawing.Point(245, 158);
            this.chkOpenHelp.Name = "chkOpenHelp";
            this.chkOpenHelp.Size = new System.Drawing.Size(292, 24);
            this.statusBarTextProvider1.SetStatusBarText(this.chkOpenHelp, "Open Help: Check this to open the help file after a successful build");
            this.chkOpenHelp.TabIndex = 8;
            this.chkOpenHelp.Text = "&Open help file after successful build";
            this.chkOpenHelp.UseVisualStyleBackColor = true;
            // 
            // cboBeforeBuildAction
            // 
            this.cboBeforeBuildAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBeforeBuildAction.FormattingEnabled = true;
            this.cboBeforeBuildAction.Items.AddRange(new object[] {
            "Save all changes",
            "Save changes to open documents only",
            "Prompt to save all changes",
            "Don\'t save any changes"});
            this.cboBeforeBuildAction.Location = new System.Drawing.Point(245, 98);
            this.cboBeforeBuildAction.Name = "cboBeforeBuildAction";
            this.cboBeforeBuildAction.Size = new System.Drawing.Size(292, 24);
            this.statusBarTextProvider1.SetStatusBarText(this.cboBeforeBuildAction, "Before Build: Select the action to take before performing a build");
            this.cboBeforeBuildAction.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(118, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 23);
            this.label7.TabIndex = 5;
            this.label7.Text = "B&efore Building";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnEditorFont
            // 
            this.btnEditorFont.Location = new System.Drawing.Point(470, 320);
            this.btnEditorFont.Name = "btnEditorFont";
            this.btnEditorFont.Size = new System.Drawing.Size(32, 25);
            this.btnEditorFont.TabIndex = 18;
            this.btnEditorFont.Text = "...";
            this.btnEditorFont.UseVisualStyleBackColor = true;
            this.btnEditorFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(335, 321);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 23);
            this.label3.TabIndex = 17;
            this.label3.Text = "&Text Editor Font";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEditorExample
            // 
            this.lblEditorExample.BackColor = System.Drawing.SystemColors.Window;
            this.lblEditorExample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEditorExample.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEditorExample.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblEditorExample.Location = new System.Drawing.Point(508, 295);
            this.lblEditorExample.Name = "lblEditorExample";
            this.lblEditorExample.Size = new System.Drawing.Size(128, 75);
            this.lblEditorExample.TabIndex = 19;
            this.lblEditorExample.Text = "Example Text";
            this.lblEditorExample.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnBuildFont
            // 
            this.btnBuildFont.Location = new System.Drawing.Point(470, 230);
            this.btnBuildFont.Name = "btnBuildFont";
            this.btnBuildFont.Size = new System.Drawing.Size(32, 25);
            this.btnBuildFont.TabIndex = 14;
            this.btnBuildFont.Text = "...";
            this.btnBuildFont.UseVisualStyleBackColor = true;
            this.btnBuildFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(418, 231);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 23);
            this.label4.TabIndex = 13;
            this.label4.Text = "Font";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnBuildForeground
            // 
            this.btnBuildForeground.Location = new System.Drawing.Point(380, 230);
            this.btnBuildForeground.Name = "btnBuildForeground";
            this.btnBuildForeground.Size = new System.Drawing.Size(32, 25);
            this.btnBuildForeground.TabIndex = 12;
            this.btnBuildForeground.Text = "...";
            this.btnBuildForeground.UseVisualStyleBackColor = true;
            this.btnBuildForeground.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // btnBuildBackground
            // 
            this.btnBuildBackground.Location = new System.Drawing.Point(245, 230);
            this.btnBuildBackground.Name = "btnBuildBackground";
            this.btnBuildBackground.Size = new System.Drawing.Size(32, 25);
            this.btnBuildBackground.TabIndex = 10;
            this.btnBuildBackground.Text = "...";
            this.btnBuildBackground.UseVisualStyleBackColor = true;
            this.btnBuildBackground.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(283, 231);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 23);
            this.label5.TabIndex = 11;
            this.label5.Text = "Foreground";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBuildExample
            // 
            this.lblBuildExample.BackColor = System.Drawing.SystemColors.Window;
            this.lblBuildExample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBuildExample.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuildExample.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblBuildExample.Location = new System.Drawing.Point(508, 205);
            this.lblBuildExample.Name = "lblBuildExample";
            this.lblBuildExample.Size = new System.Drawing.Size(128, 75);
            this.lblBuildExample.TabIndex = 15;
            this.lblBuildExample.Text = "Example Text";
            this.lblBuildExample.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(65, 231);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(174, 23);
            this.label6.TabIndex = 9;
            this.label6.Text = "B&uild Output Background";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(31, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(208, 47);
            this.label2.TabIndex = 3;
            this.label2.Text = "&ASP.NET Development Web Server Port";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(43, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "&HTML Help 2.x Viewer Path";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pgContentEditors
            // 
            this.pgContentEditors.Controls.Add(this.btnDelete);
            this.pgContentEditors.Controls.Add(this.btnAddFile);
            this.pgContentEditors.Controls.Add(this.pgProps);
            this.pgContentEditors.Controls.Add(this.lbContentEditors);
            this.pgContentEditors.Location = new System.Drawing.Point(4, 25);
            this.pgContentEditors.Name = "pgContentEditors";
            this.pgContentEditors.Padding = new System.Windows.Forms.Padding(3);
            this.pgContentEditors.Size = new System.Drawing.Size(688, 389);
            this.statusBarTextProvider1.SetStatusBarText(this.pgContentEditors, "Content file editors");
            this.pgContentEditors.TabIndex = 1;
            this.pgContentEditors.Text = "Content File Editors";
            this.pgContentEditors.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.ImageIndex = 1;
            this.btnDelete.ImageList = this.ilButton;
            this.btnDelete.Location = new System.Drawing.Point(650, 89);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(32, 32);
            this.statusBarTextProvider1.SetStatusBarText(this.btnDelete, "Delete: Delete the selected definition");
            this.btnDelete.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnDelete, "Delete the selected definition");
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // ilButton
            // 
            this.ilButton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilButton.ImageStream")));
            this.ilButton.TransparentColor = System.Drawing.Color.Magenta;
            this.ilButton.Images.SetKeyName(0, "AddItem.bmp");
            this.ilButton.Images.SetKeyName(1, "Delete.bmp");
            // 
            // btnAddFile
            // 
            this.btnAddFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFile.ImageIndex = 0;
            this.btnAddFile.ImageList = this.ilButton;
            this.btnAddFile.Location = new System.Drawing.Point(650, 6);
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(32, 32);
            this.statusBarTextProvider1.SetStatusBarText(this.btnAddFile, "Add: Add a new content file editor definition");
            this.btnAddFile.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnAddFile, "Add a new content file editor definition");
            this.btnAddFile.UseVisualStyleBackColor = true;
            this.btnAddFile.Click += new System.EventHandler(this.btnAddFile_Click);
            // 
            // pgProps
            // 
            this.pgProps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pgProps.Location = new System.Drawing.Point(6, 127);
            this.pgProps.Name = "pgProps";
            this.pgProps.PropertyNamePaneWidth = 150;
            this.pgProps.Size = new System.Drawing.Size(676, 256);
            this.statusBarTextProvider1.SetStatusBarText(this.pgProps, "Edit the properties of the selected content editor");
            this.pgProps.TabIndex = 3;
            this.pgProps.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgProps_PropertyValueChanged);
            // 
            // lbContentEditors
            // 
            this.lbContentEditors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbContentEditors.FormattingEnabled = true;
            this.lbContentEditors.IntegralHeight = false;
            this.lbContentEditors.ItemHeight = 16;
            this.lbContentEditors.Location = new System.Drawing.Point(6, 6);
            this.lbContentEditors.Name = "lbContentEditors";
            this.lbContentEditors.Size = new System.Drawing.Size(638, 115);
            this.statusBarTextProvider1.SetStatusBarText(this.lbContentEditors, "Select a content editor item");
            this.lbContentEditors.TabIndex = 0;
            this.lbContentEditors.SelectedIndexChanged += new System.EventHandler(this.lbContentEditors_SelectedIndexChanged);
            // 
            // epErrors
            // 
            this.epErrors.ContainerControl = this;
            // 
            // chkShowLineNumbers
            // 
            this.chkShowLineNumbers.AutoSize = true;
            this.chkShowLineNumbers.Location = new System.Drawing.Point(75, 323);
            this.chkShowLineNumbers.Name = "chkShowLineNumbers";
            this.chkShowLineNumbers.Size = new System.Drawing.Size(230, 21);
            this.statusBarTextProvider1.SetStatusBarText(this.chkShowLineNumbers, "Show Line Numbers: Check this box to show line numbers in the text editor");
            this.chkShowLineNumbers.TabIndex = 16;
            this.chkShowLineNumbers.Text = "Sho&w line numbers in text editor";
            this.chkShowLineNumbers.UseVisualStyleBackColor = true;
            // 
            // UserPreferencesDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(720, 480);
            this.Controls.Add(this.tabPreferences);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserPreferencesDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Preferences";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserPreferencesDlg_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.udcASPNetDevServerPort)).EndInit();
            this.tabPreferences.ResumeLayout(false);
            this.pgGeneral.ResumeLayout(false);
            this.pgGeneral.PerformLayout();
            this.pgContentEditors.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.epErrors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private SandcastleBuilder.Utils.Controls.StatusBarTextProvider statusBarTextProvider1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkVerboseLogging;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHTMLHelp2ViewerPath;
        private System.Windows.Forms.Button btnSelectViewer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown udcASPNetDevServerPort;
        private System.Windows.Forms.ErrorProvider epErrors;
        private System.Windows.Forms.TabControl tabPreferences;
        private System.Windows.Forms.TabPage pgGeneral;
        private System.Windows.Forms.TabPage pgContentEditors;
        private SandcastleBuilder.Utils.Controls.CustomPropertyGrid pgProps;
        private SandcastleBuilder.Utils.Controls.RefreshableItemListBox lbContentEditors;
        private System.Windows.Forms.ImageList ilButton;
        private System.Windows.Forms.Button btnAddFile;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEditorFont;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblEditorExample;
        private System.Windows.Forms.Button btnBuildFont;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnBuildForeground;
        private System.Windows.Forms.Button btnBuildBackground;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblBuildExample;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkOpenHelp;
        private System.Windows.Forms.ComboBox cboBeforeBuildAction;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkShowLineNumbers;
    }
}