using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

using Microsoft.Ddue.Tools;

namespace SandcastleBuilder.Components
{
    public class RemoveNamespaceDeclarations : BuildComponent
    {
        public RemoveNamespaceDeclarations(BuildAssembler assembler, XPathNavigator configuration)
            : base(assembler, configuration)
        {
        }

        public override void Apply(XmlDocument document, string key)
        {
            var attrsToRemove = new List<XmlAttribute>();

            foreach (XmlAttribute attribute in document.DocumentElement.Attributes)
            {
                if (attribute.Name.StartsWith("xmlns:"))
                    attrsToRemove.Add(attribute);
            }

            attrsToRemove.ForEach(a => document.DocumentElement.Attributes.Remove(a));
        }
    }
}
