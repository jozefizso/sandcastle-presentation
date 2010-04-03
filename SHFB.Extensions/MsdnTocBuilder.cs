using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.XPath;

using SandcastleBuilder.Utils;
using SandcastleBuilder.Utils.BuildEngine;
using SandcastleBuilder.Utils.PlugIn;
using System.Collections.ObjectModel;
using SandcastleBuilder.Utils.ConceptualContent;
using System.Xml;

namespace Izsaknet.Sandcastle.Extensions
{
    public class MsdnTocBuilder : PluginBase
    {
        private ExecutionPointCollection executionPoints;
        private BuildProcess build;

        public MsdnTocBuilder()
        {
            var executeBefore = new ExecutionPoint(BuildStep.BuildConceptualTopics, ExecutionBehaviors.Before);

            executionPoints = new ExecutionPointCollection() { executeBefore };
        }

        public override string Name
        {
            get { return "MSDN 2010 Branding TOC Builder"; }
        }

        public override string Description
        {
            get { return "This plug-in will create a TOC for use in the MSDN 2010 Branding template."; }
        }

        public override ExecutionPointCollection ExecutionPoints
        {
            get { return this.executionPoints; }
        }

        public override string ConfigurePlugIn(SandcastleProject project, string currentConfig)
        {
            return currentConfig;
        }

        public override void Initialize(BuildProcess buildProcess, XPathNavigator configuration)
        {
            this.build = buildProcess;
        }

        public override void Execute(ExecutionContext context)
        {
            var content = this.build.ConceptualContent;

            string destContentFile = Path.Combine(this.build.WorkingFolder, "conceptual_topics.xml");

            WriteConceptualTopicsFile(content.Topics, destContentFile);
        }

        private void WriteConceptualTopicsFile(Collection<TopicCollection> collection, string destContentFile)
        {
            var settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(destContentFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("tocTopics");

                foreach (var topics in collection)
                {
                    writer.WriteStartElement("tocTopics");
                    if (topics.ContentLayoutFile != null)
                        writer.WriteAttributeString("file", topics.ContentLayoutFile.FullPath);

                    WriteTopic(writer, topics);

                    writer.WriteEndElement();

                }

                writer.WriteEndElement();
            }
        }

        private void WriteTopic(XmlWriter writer, TopicCollection topicsList)
        {
            foreach (var topic in topicsList)
            {
                WriteTopic(writer, topic);
            }
        }

        private void WriteTopic(XmlWriter writer, Topic topic)
        {
            writer.WriteStartElement("tocTopic");
            writer.WriteAttributeString("id", topic.Id);
            writer.WriteAttributeString("visible", topic.Visible.ToString());
            if (topic.IsDefaultTopic)
                writer.WriteAttributeString("isDefault", "true");

            if (topic.Subtopics.Count > 0)
            {
                WriteTopic(writer, topic.Subtopics);
            }

            writer.WriteEndElement();
        }
    }
}
