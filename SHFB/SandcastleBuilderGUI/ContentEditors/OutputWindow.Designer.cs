namespace SandcastleBuilder.Gui.ContentEditors
{
    partial class OutputWindow
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputWindow));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tcbViewOutput = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPrint = new System.Windows.Forms.ToolStripButton();
            this.tslLogFile = new System.Windows.Forms.ToolStripLabel();
            this.statusBarTextProvider1 = new SandcastleBuilder.Utils.Controls.StatusBarTextProvider(this.components);
            this.txtBuildOutput = new System.Windows.Forms.RichTextBox();
            this.wbLogContent = new System.Windows.Forms.WebBrowser();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tcbViewOutput,
            this.toolStripSeparator1,
            this.tsbPrint,
            this.tslLogFile});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(473, 28);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(129, 25);
            this.toolStripLabel1.Text = "&Show output from";
            // 
            // tcbViewOutput
            // 
            this.tcbViewOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tcbViewOutput.Items.AddRange(new object[] {
            "Build",
            "Log File"});
            this.tcbViewOutput.Name = "tcbViewOutput";
            this.tcbViewOutput.Size = new System.Drawing.Size(121, 28);
            this.statusBarTextProvider1.SetStatusBarText(this.tcbViewOutput, "Select the output to view");
            this.tcbViewOutput.SelectedIndexChanged += new System.EventHandler(this.tcbViewOutput_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // tsbPrint
            // 
            this.tsbPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPrint.Image = global::SandcastleBuilder.Gui.Properties.Resources.Print;
            this.tsbPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrint.Name = "tsbPrint";
            this.tsbPrint.Size = new System.Drawing.Size(23, 25);
            this.statusBarTextProvider1.SetStatusBarText(this.tsbPrint, "Print log file");
            this.tsbPrint.ToolTipText = "Print log file";
            this.tsbPrint.Click += new System.EventHandler(this.tsbPrint_Click);
            // 
            // tslLogFile
            // 
            this.tslLogFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tslLogFile.Name = "tslLogFile";
            this.tslLogFile.Size = new System.Drawing.Size(0, 25);
            // 
            // txtBuildOutput
            // 
            this.txtBuildOutput.BackColor = System.Drawing.SystemColors.Window;
            this.txtBuildOutput.DetectUrls = false;
            this.txtBuildOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBuildOutput.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBuildOutput.HideSelection = false;
            this.txtBuildOutput.Location = new System.Drawing.Point(0, 28);
            this.txtBuildOutput.Name = "txtBuildOutput";
            this.txtBuildOutput.ReadOnly = true;
            this.txtBuildOutput.Size = new System.Drawing.Size(473, 255);
            this.statusBarTextProvider1.SetStatusBarText(this.txtBuildOutput, "Build output");
            this.txtBuildOutput.TabIndex = 1;
            this.txtBuildOutput.Text = "";
            this.txtBuildOutput.WordWrap = false;
            // 
            // wbLogContent
            // 
            this.wbLogContent.AllowWebBrowserDrop = false;
            this.wbLogContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbLogContent.Location = new System.Drawing.Point(0, 28);
            this.wbLogContent.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbLogContent.Name = "wbLogContent";
            this.wbLogContent.Size = new System.Drawing.Size(473, 255);
            this.statusBarTextProvider1.SetStatusBarText(this.wbLogContent, "Log file content");
            this.wbLogContent.TabIndex = 2;
            this.wbLogContent.Visible = false;
            // 
            // OutputWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(473, 283);
            this.Controls.Add(this.wbLogContent);
            this.Controls.Add(this.txtBuildOutput);
            this.Controls.Add(this.toolStrip1);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "OutputWindow";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
            this.ShowInTaskbar = false;
            this.TabText = "Build Output";
            this.Text = "Build Output";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox tcbViewOutput;
        private SandcastleBuilder.Utils.Controls.StatusBarTextProvider statusBarTextProvider1;
        private System.Windows.Forms.RichTextBox txtBuildOutput;
        private System.Windows.Forms.WebBrowser wbLogContent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbPrint;
        private System.Windows.Forms.ToolStripLabel tslLogFile;
    }
}
