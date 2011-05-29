using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace Microsoft.Ddue.Tools
{
    public class ConfigurationContext {
        public string ConfigurationFile { get; set; }

        public XPathDocument Configuration { get; set; }
    }
}
