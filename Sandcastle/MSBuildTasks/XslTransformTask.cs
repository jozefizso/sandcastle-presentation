using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Sandcastle.Build.Tasks
{
    public class XslTransformTask : Task
    {
        private const string MsBuildNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";

        public XslTransformTask()
        {
        }

        [Required]
        public ITaskItem XmlInputFile { get; set; }

        [Required]
        public ITaskItem XmlOutputFile { get; set; }

        [Required]
        public ITaskItem[] XslTransformationFiles { get; set; }

        public string XslParameters { get; set; }

        public bool IncludeMsBuildParameters { get; set; }

        public override bool Execute()
        {
            if (!ValidateFiles())
                return false;

            XsltArgumentList args = this.ParseArguments(this.XslParameters);
            if (this.IncludeMsBuildParameters)
                this.AddMsBuildParameters(args);

            IList<Tuple<string, XslCompiledTransform>> transforms;
            if (!this.TryLoadXsltFiles(this.XslTransformationFiles, out transforms))
                return false;

            if (!this.TransformFile(this.XmlInputFile.ItemSpec, this.XmlOutputFile.ItemSpec, transforms, args))
                return false;

            return true;
        }

        private bool ValidateFiles()
        {
            bool isValid = true;
            if (!File.Exists(this.XmlInputFile.ItemSpec))
            {
                this.Log.LogError("XmlInputFile '{0}' does not exist.", this.XmlInputFile.ItemSpec);
                isValid = false;
            }

            foreach (ITaskItem xslFile in this.XslTransformationFiles)
            {
                if (!File.Exists(xslFile.ItemSpec))
                {
                    this.Log.LogError("XslTransformationFile '{0}' does not exist.", xslFile.ItemSpec);
                    isValid = false;
                }
            }

            return isValid;
        }

        private XsltArgumentList ParseArguments(string parameters)
        {
            XsltArgumentList args = new XsltArgumentList();

            if (String.IsNullOrEmpty(parameters))
                return args;

            string[] nameValueStrings = parameters.Split(';');
            foreach (string nameValueString in nameValueStrings)
            {
                string[] nameValuePair = nameValueString.Split('=');
                if (nameValuePair.Length != 2)
                {
                    this.Log.LogWarning("XslParameter '{0}' is not valid key/value pair. Use the key=value format.");
                    continue;
                }
                args.AddParam(nameValuePair[0], "", nameValuePair[1]);
            }

            return args;
        }

        private XsltArgumentList AddMsBuildParameters(XsltArgumentList args)
        {
            string projectFile = this.BuildEngine.ProjectFileOfTaskNode;
            Project project = ProjectCollection.GlobalProjectCollection.GetLoadedProjects(projectFile).FirstOrDefault();
            if (project != null)
            {
                var properties = project.AllEvaluatedProperties.Select(pp => new { Name = pp.Name, Value = pp.EvaluatedValue });
                foreach (var prop in properties)
                {
                    args.AddParam(prop.Name, MsBuildNamespace, prop.Value);
                }
            }
            return args;
        }

        private bool TryLoadXsltFiles(ITaskItem[] xsltFiles, out IList<Tuple<string, XslCompiledTransform>> transforms)
        {
            XsltSettings settings = new XsltSettings(enableDocumentFunction: true, enableScript: true);

            transforms = new List<Tuple<string, XslCompiledTransform>>(xsltFiles.Length);
            foreach (ITaskItem xsltFile in xsltFiles)
            {
                if (!this.LoadXsltFile(transforms, xsltFile.ItemSpec, settings))
                    return false;
            }

            return true;
        }

        private bool LoadXsltFile(ICollection<Tuple<string,XslCompiledTransform>> transforms, string xsltFile, XsltSettings settings)
        {
            try
            {
                XslCompiledTransform transform = new XslCompiledTransform();
                transform.Load(xsltFile, settings, new XmlUrlResolver());
                transforms.Add(Tuple.Create(xsltFile, transform));
            }
            catch (IOException ex)
            {
                this.Log.LogError("Failed to load XSL file '{0}'.", xsltFile);
                this.Log.LogErrorFromException(ex, true, true, xsltFile);
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                this.Log.LogError("Failed to load XSL file '{0}'.", xsltFile);
                this.Log.LogErrorFromException(ex, true, true, xsltFile);
                return false;
            }
            catch (XsltException ex)
            {
                this.Log.LogError("Failed to load XSL file '{0}'.", xsltFile);
                this.Log.LogErrorFromException(ex, true, true, xsltFile);
                return false;
            }
            catch (XmlException ex)
            {
                this.Log.LogError("Failed to load XSL file '{0}'.", xsltFile);
                this.Log.LogErrorFromException(ex, true, true, xsltFile);
                return false;
            }

            return true;
        }

        private bool TransformFile(string xmlInputFile, string xmlOutputFile, IList<Tuple<string, XslCompiledTransform>> transforms, XsltArgumentList args)
        {
            string inputFile = xmlInputFile;
            string outputFile = null;

            this.Log.LogMessage("Transforming input XML file '{0}' to output '{1}'.", Path.GetFileName(xmlInputFile), Path.GetFileName(xmlOutputFile));

            this.Log.LogMessage(MessageImportance.Low, "List of XSLT files used in transformation:");
            foreach (string xsltFile in transforms.Select(t => t.Item1))
            {
                this.Log.LogMessage(MessageImportance.Low, "  "+ xsltFile);
            }

            for (int i = 0; i < transforms.Count; i++)
            {
                if (i < (transforms.Count - 1))
                {
                    string tempFile = Path.GetTempFileName();
                    File.SetAttributes(tempFile, FileAttributes.Temporary);
                    outputFile = tempFile;
                }
                else
                {
                    outputFile = xmlOutputFile;
                }

                string xsltFile = transforms[i].Item1;
                string xsltFileName = Path.GetFileName(xsltFile);
                XslCompiledTransform transform = transforms[i].Item2;
                this.Log.LogMessage("Transforming input using the XSLT file '{0}'.", xsltFileName);
                this.Log.LogMessage(MessageImportance.Low, "Transforming XML file '{0}' using the XSLT file '{1}' into the output file '{2}'.", inputFile, xsltFile, outputFile);

                try
                {
                    this.TransformFile(inputFile, outputFile, transform, args);
                }
                catch (XsltException ex)
                {
                    this.Log.LogError("Failed to transform XML file '{0}' using the XSLT file '{1}'.", inputFile, xsltFile);
                    this.Log.LogErrorFromException(ex, true, true, xsltFile);
                    return false;
                }
                catch (XmlException ex)
                {
                    this.Log.LogError("Failed to transform XML file '{0}' using the XSLT file '{1}'.", inputFile, xsltFile);
                    this.Log.LogErrorFromException(ex, true, true, xsltFile);
                    return false;
                }

                if (i > 0)
                {
                    File.Delete(inputFile);
                }

                inputFile = outputFile;
            }

            return true;
        }

        private void TransformFile(string xmlInputFile, string outputFile, XslCompiledTransform transform, XsltArgumentList args)
        {
            using (XmlReader reader = XmlReader.Create(xmlInputFile))
            using (XmlWriter writer = XmlWriter.Create(outputFile))
            {
                transform.Transform(reader, args, writer);
            }
        }
    }
}
