using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using SandcastleBuilder.Utils.Design;
using System.IO;

namespace SandcastleBuilder.Gui.Utilities
{
    public class WebServerProvider
    {
        private string programFiles;
        IList<IntegratedWebServer> webServers;

        public WebServerProvider()
        {
            string vs2005 = String.Format(
                        CultureInfo.InvariantCulture,
                        @"%SystemRoot%\Microsoft.NET\Framework\v{0}\WebDev.WebServer.exe",
                        FrameworkVersionTypeConverter.LatestMatching("2"));
            vs2005 = Resolve(vs2005);

            string vs2008 = Path.Combine(CommonProgramFiles(), @"Microsoft Shared\DevServer\9.0\WebDev.WebServer.exe");
            string vs2010 = Path.Combine(CommonProgramFiles(), @"Microsoft Shared\DevServer\10.0\WebDev.WebServer40.exe");

            webServers = new List<IntegratedWebServer>();
            webServers.Add(new IntegratedWebServer(vs2010));
            webServers.Add(new IntegratedWebServer(vs2008));
            webServers.Add(new IntegratedWebServer(vs2005));
        }

        public IWebServer GetInstalledWebServer()
        {
            return webServers.FirstOrDefault(server => server.IsInstalled);
        }

        private string Resolve(string path)
        {
            return Environment.ExpandEnvironmentVariables(path);
        }

        private string CommonProgramFiles()
        {
            if (programFiles == null)
            {
                string pfx86 = Environment.GetEnvironmentVariable("CommonProgramFiles(x86)");
                if (pfx86 == null)
                {
                    programFiles = Environment.GetEnvironmentVariable("CommonProgramFiles");
                }
                else
                {
                    programFiles = pfx86;
                }
            }

            return programFiles;
        }
    }
}
