namespace SandcastleBuilder.Utils.Design
{
    partial class UserDefinedPropertyEditorDlg
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lbProperties = new SandcastleBuilder.Utils.Controls.RefreshableItemListBox();
            this.pgProps = new SandcastleBuilder.Utils.Controls.CustomPropertyGrid();
            this.statusBarTextProvider1 = new SandcastleBuilder.Utils.Controls.StatusBarTextProvider(this.components);
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(12, 354);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(88, 32);
            this.statusBarTextProvider1.SetStatusBarText(this.btnAdd, "Add: Add a new user-defined property");
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.Location = new System.Drawing.Point(471, 354);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(88, 32);
            this.statusBarTextProvider1.SetStatusBarText(this.btnHelp, "Help: View help for this form");
            this.btnHelp.TabIndex = 4;
            this.btnHelp.Text = "&Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(565, 354);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 32);
            this.statusBarTextProvider1.SetStatusBarText(this.btnClose, "Close: Close this form");
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lbProperties
            // 
            this.lbProperties.FormattingEnabled = true;
            this.lbProperties.IntegralHeight = false;
            this.lbProperties.ItemHeight = 16;
            this.lbProperties.Location = new System.Drawing.Point(12, 12);
            this.lbProperties.Name = "lbProperties";
            this.lbProperties.Size = new System.Drawing.Size(182, 336);
            this.statusBarTextProvider1.SetStatusBarText(this.lbProperties, "Select a user-defined property to edit");
            this.lbProperties.TabIndex = 0;
            this.lbProperties.SelectedIndexChanged += new System.EventHandler(this.lbProperties_SelectedIndexChanged);
            // 
            // pgProps
            // 
            this.pgProps.Location = new System.Drawing.Point(200, 12);
            this.pgProps.Name = "pgProps";
            this.pgProps.PropertyNamePaneWidth = 90;
            this.pgProps.Size = new System.Drawing.Size(453, 336);
            this.statusBarTextProvider1.SetStatusBarText(this.pgProps, "Edit the user-defined property settings");
            this.pgProps.TabIndex = 1;
            this.pgProps.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgProps_PropertyValueChanged);
            // 
            // UserDefinedPropertyEditorDlg
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(665, 398);
            this.Controls.Add(this.pgProps);
            this.Controls.Add(this.lbProperties);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserDefinedPropertyEditorDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UserDefinedPropertyEditorDlg";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnClose;
        private SandcastleBuilder.Utils.Controls.StatusBarTextProvider statusBarTextProvider1;
        private SandcastleBuilder.Utils.Controls.RefreshableItemListBox lbProperties;
        private SandcastleBuilder.Utils.Controls.CustomPropertyGrid pgProps;
    }
}