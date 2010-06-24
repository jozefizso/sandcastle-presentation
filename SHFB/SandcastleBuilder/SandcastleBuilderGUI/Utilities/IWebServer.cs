using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SandcastleBuilder.Gui.Utilities
{
    public interface IWebServer : IDisposable
    {
        bool HasExited { get; }

        void RunWebSite(int port, string physicalPath, string virtualPath);

        void TryShutDown();
    }
}
