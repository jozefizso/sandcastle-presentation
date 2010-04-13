using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace SandcastleBuilder.Gui.Utilities
{
    public class IntegratedWebServer : IWebServer
    {
        private string path;
        private Process webServer;

        public IntegratedWebServer(string path)
        {
            this.path = path;
        }

        public string WebServerExecutable { get { return this.path; } }

        public bool IsInstalled
        {
            get
            {
                return File.Exists(this.WebServerExecutable);
            }
        }

        public bool HasExited
        {
            get
            {
                if (this.webServer == null)
                    return false;
                return this.webServer.HasExited;
            }
        }

        public void RunWebSite(int port, string physicalPath, string virtualPath)
        {
            webServer = new Process();
            var psi = webServer.StartInfo;

            psi.FileName = WebServerExecutable;
            psi.Arguments = String.Format(CultureInfo.InvariantCulture,
                @"/port:{0} /path:""{1}"" /vpath:""{2}""",
                port, physicalPath, virtualPath);
            psi.WorkingDirectory = physicalPath;
            psi.UseShellExecute = false;

            webServer.Start();
            webServer.WaitForInputIdle(30000);
        }

        public void TryShutDown()
        {
            if (this.webServer != null)
            {
                this.webServer.Kill();
                this.webServer.Dispose();
                this.webServer = null;
            }
        }

        public void Dispose()
        {
            TryShutDown();
        }
    }
}
