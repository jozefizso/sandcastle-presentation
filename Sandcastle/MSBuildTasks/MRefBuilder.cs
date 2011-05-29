using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Ddue.Tools;
using Microsoft.Ddue.Tools.CommandLine;

namespace Sandcastle.Build.Tasks
{
    public class MRefBuilder : Task
    {
        public MRefBuilder()
        {
        }

        [Required]
        public ITaskItem[] Assemblies { get; set; }

        public ITaskItem[] Dependencies { get; set; }

        [Required]
        public string OutputFile { get; set; }

        public bool IncludeInternalMembers { get; set; }

        public override bool Execute()
        {
            string configFile = GetConfigFileName();
            var assemblies = this.Assemblies.Select(item => item.ItemSpec);
            var dependencies = this.Dependencies.Select(item => item.ItemSpec);

            ConsoleApplication.SetConsoleLogger(new MsBuildConsoleLogger(this.Log));
            MRefBuilderApp builder = new MRefBuilderApp(configFile, assemblies, dependencies, this.OutputFile, this.IncludeInternalMembers);

            try
            {
                builder.VisitApis();
            }
            catch (SandcastleBuildException ex)
            {
                this.Log.LogError("MRefBuilder task failed: {0}", ex.Message);
                if (ex.InnerException != null)
                {
                    this.Log.LogErrorFromException(ex.InnerException);
                }

                return false;
            }

            return true;
        }

        private string GetConfigFileName()
        {
            return Path.Combine(Environment.GetEnvironmentVariable("DXROOT"), "ProductionTools", "MRefBuilder.config");
        }

    }
}
