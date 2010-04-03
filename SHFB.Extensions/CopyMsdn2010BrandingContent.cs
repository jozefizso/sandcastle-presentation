using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SandcastleBuilder.Utils.PlugIn;
using SandcastleBuilder.Utils;
using SandcastleBuilder.Utils.BuildEngine;
using System.Xml.XPath;
using System.IO;

namespace Izsaknet.Sandcastle.Extensions
{
    public class CopyMsdn2010BrandingContent : IPlugIn
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

        public string ConfigurePlugIn(SandcastleProject project, string currentConfig)
        {
            return currentConfig;
        }

        public string Copyright
        {
            get { return "(c) 2010 Jozef Izso."; }
        }

        public string Description
        {
            get { return "This plug-in will copy additional content folders from the MSDN 2010 Branding template."; }
        }

        public void Execute(ExecutionContext context)
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

        public ExecutionPointCollection ExecutionPoints
        {
            get { return this.executionPoints; }
        }

        public void Initialize(BuildProcess buildProcess, XPathNavigator configuration)
        {
            this.build = buildProcess;
        }

        public string Name
        {
            get { return "Copy MSDN 2010 Branding Standard Content Files"; }
        }

        public bool RunsInPartialBuild
        {
            get { return false; }
        }

        public Version Version
        {
            get { return new Version(1, 0); }
        }

        public void Dispose()
        {
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
