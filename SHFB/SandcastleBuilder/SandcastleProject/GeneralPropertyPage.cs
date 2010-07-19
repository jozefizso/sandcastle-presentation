using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio;
using SandcastleBuilder.Utils;
using System.Windows.Forms;
using System.Drawing;

namespace Izsaknet.Sandcastle.VisualStudio
{

    [ComVisible(true)]
    [Guid(GuidList.GeneralPropertyPageString)]
    public class GeneralPropertyPage : CommonPropertyPage
    {
        private Control control;

        public GeneralPropertyPage()
        {
            this.Name = "General Properties";
            this.control = new UserControl();
            this.control.BackColor = Color.Azure;
            this.control.Size = new Size(200, 100);
        }

        [Category("Kategória")]
        [DisplayName("Moja vlastnosť")]
        [Description("Popis vlastnosti.")]
        public string MyProperty
        {
            get;
            set;
        }

        [Category("Kategória")]
        [DisplayName("Projekt Sandcastle")]
        [Description("Vlastnosti projektu Sandcastle.")]
        public SandcastleProject SandcastleProject
        {
            get;
            set;
        }

        public override Control Control
        {
            get { return this.control; }
        }

        protected override void BindProperties()
        {
            string filename = this.ProjectMgr.BuildProject.FullPath;
            this.SandcastleProject = new SandcastleProject(filename, true);

            this.MyProperty = this.ProjectMgr.GetProjectProperty("SHFBSchemaVersion");
        }

        protected override int ApplyChanges()
        {
            this.ProjectMgr.SetProjectProperty("SHFBSchemaVersion", this.MyProperty);
            this.IsDirty = false;

            return VSConstants.S_OK;
        }
    }
}
