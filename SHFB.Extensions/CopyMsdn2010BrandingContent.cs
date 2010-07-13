using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.XPath;

using SandcastleBuilder.Utils;
using SandcastleBuilder.Utils.BuildEngine;
using SandcastleBuilder.Utils.PlugIn;

namespace Izsaknet.Sandcastle.Extensions
{
    public class CopyMsdn2010BrandingContent : PluginBase
    {
        private ExecutionPointCollection executionPoints;
        private BuildProcess build;

        public CopyMsdn2010BrandingContent()
        {
            var executeAfter = new ExecutionPoint(BuildStep.CopyStandardContent, ExecutionBehaviors.After);
            var executeAfterWebsite = new ExecutionPoint(BuildStep.CopyingWebsiteFiles, ExecutionBehaviors.After);
            var executeBeforeProjet = new ExecutionPoint(BuildStep.BuildConceptualTopics, ExecutionBehaviors.After);

            executionPoints = new ExecutionPointCollection() { executeAfter, executeAfterWebsite, /*executeBeforeProjet*/ };
        }

        public override string ConfigurePlugIn(SandcastleProject project, string currentConfig)
        {
            return currentConfig;
        }

        public override string Description
        {
            get { return "This plug-in will copy additional content folders from the MSDN 2010 Branding template."; }
        }

        public override void Execute(ExecutionContext context)
        {
            string srcPath = null, destPath = null;

            switch (context.BuildStep)
            {
                case BuildStep.CopyStandardContent:
                    srcPath = Path.Combine(this.build.PresentationStyleFolder, "images");
                    destPath = Path.Combine(this.build.WorkingFolder, "Output\\images");
                    break;
                case BuildStep.CopyingWebsiteFiles:
                    srcPath = Path.Combine(this.build.WorkingFolder, "images");
                    destPath = Path.Combine(this.build.OutputFolder, "images");
                    break;
                default:
                    return;
            }

            if (srcPath != null && Directory.Exists(srcPath))
            {
                CopyDirectory(srcPath, destPath);
            }
        }

        public override ExecutionPointCollection ExecutionPoints
        {
            get { return this.executionPoints; }
        }

        public override void Initialize(BuildProcess buildProcess, XPathNavigator configuration)
        {
            this.build = buildProcess;
        }

        public override string Name
        {
            get { return "Copy MSDN 2010 Branding Standard Content Files"; }
        }

        protected void CopyDirectory(string sourcePath, string destPath)
        {
            if (!Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);

            var files = Directory.GetFiles(sourcePath, "*.*", SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var newFileFullName = Path.Combine(destPath, fileName);

                File.Copy(file, newFileFullName, true);
                File.SetAttributes(newFileFullName, FileAttributes.Normal);

                this.build.ReportProgress("{0} -> {1}", file, newFileFullName);
            }
        }
    }
}
