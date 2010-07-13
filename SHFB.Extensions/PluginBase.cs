using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SandcastleBuilder.Utils.PlugIn;
using SandcastleBuilder.Utils;
using SandcastleBuilder.Utils.BuildEngine;
using System.Xml.XPath;

namespace Izsaknet.Sandcastle.Extensions
{
    public abstract class PluginBase : IPlugIn
    {
        public string Copyright
        {
            get { return "(c) 2010 Jozef Izso."; }
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

        public abstract string ConfigurePlugIn(SandcastleProject project, string currentConfig);

        public abstract string Description { get; }

        public abstract void Execute(ExecutionContext context);

        public abstract ExecutionPointCollection ExecutionPoints { get; }

        public abstract void Initialize(BuildProcess buildProcess, XPathNavigator configuration);

        public abstract string Name { get; }
    }
}
