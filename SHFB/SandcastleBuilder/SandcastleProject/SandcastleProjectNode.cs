using System;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.VisualStudio.Project;

namespace Izsaknet.Sandcastle.VisualStudio
{
    public class SandcastleProjectNode : ProjectNode
    {
        private SandcastleProjectPackage package;
        private ImageList imageList;
        internal int imageIndex;

        public SandcastleProjectNode(SandcastleProjectPackage package)
        {
            this.package = package;
            InitImageList();
            this.SupportsProjectDesigner = true;
        }

        public override Guid ProjectGuid
        {
            get { return GuidList.SandcastleProjectFactoryGuid; }
        }

        public override string ProjectType
        {
            get { return SandcastleConstants.ProjectType; }
        }

        public override int ImageIndex
        {
            get { return this.imageIndex; }
        }

        public override void AddFileFromTemplate(string source, string target)
        {
            //this.FileTemplateProcessor.AddReplace("$guid1$", Guid.NewGuid().ToString("B"));
            //this.FileTemplateProcessor.AddReplace("$safeprojectname$", "Documentation");

            this.FileTemplateProcessor.UntokenFile(source, target);
            this.FileTemplateProcessor.Reset();
        }

        protected override bool IsItemTypeFileType(string type)
        {
            if (base.IsItemTypeFileType(type))
                return true;

            if (String.Compare(type, SandcastleProjectFileConstants.ContentLayout, StringComparison.OrdinalIgnoreCase) == 0)
                return true;

            return false;
        }

        protected override Guid[] GetConfigurationIndependentPropertyPages()
        {
            return GetTypeGuids<GeneralPropertyPage>();
        }

        //protected override Guid[] GetConfigurationDependentPropertyPages()
        //{
        //    return GetTypeGuids<GeneralPropertyPage>();
        //}

        protected override Guid[] GetPriorityProjectDesignerPages()
        {
            return GetTypeGuids<GeneralPropertyPage>();
        }

        private void InitImageList()
        {
            this.imageList = Utilities.GetImageList(
                this.GetType().Assembly.GetManifestResourceStream(
                    SandcastleConstants.SandcastleProjectNodeIcon));

            this.imageIndex = this.ImageHandler.ImageList.Images.Count;
            foreach (Image img in imageList.Images)
            {
                this.ImageHandler.AddImage(img);
            }
        }

        private static Guid[] GetTypeGuids<T>()
        {
            Guid[] result = new Guid[] {
                typeof(T).GUID
            };

            return result;
        }
    }
}
