using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.Project;

using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace Izsaknet.Sandcastle.VisualStudio
{
    [Guid(GuidList.SandcastleProjectFactoryString)]
    public class SandcastleProjectFactory : ProjectFactory
    {
        private SandcastleProjectPackage package;

        public SandcastleProjectFactory(SandcastleProjectPackage package)
            : base(package)
        {
            this.package = package;
        }

        protected override ProjectNode CreateProject()
        {
            var project = new SandcastleProjectNode(this.package);
            var oleService = this.package.GetService<IOleServiceProvider>();
            project.SetSite(oleService);
            return project;
        }
    }
}
