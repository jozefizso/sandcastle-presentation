using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.IO;

namespace Sandcastle.Build.Tasks
{
    public class MRefBuilder : ToolTask
    {
        [Required]
        public ITaskItem[] Assemblies { get; set; }

        public ITaskItem[] Dependencies { get; set; }

        [Required]
        public string OutputFile { get; set; }

        public bool IncludeInternalMembers { get; set; }

        protected override string ToolName
        {
            get { return "MRefBuilder.exe"; }
        }

        public override bool Execute()
        {
            Log.LogMessage(MessageImportance.Low, "Building reference information for assemblies.");

            return base.Execute();
        }

        protected override string GenerateCommandLineCommands()
        {
            StringBuilder sb = new StringBuilder();

            foreach (ITaskItem assembly in this.Assemblies)
            {
                sb.AppendFormat("\"{0}\" ", assembly.ItemSpec);
            }

            sb.AppendFormat("/out:\"{0}\" ", this.OutputFile);

            if (this.Dependencies != null && this.Dependencies.Length > 0)
            {
                sb.Append("/dep:");
                foreach (ITaskItem dependency in this.Dependencies)
                {
                    sb.AppendFormat("\"{0}\",", dependency.ItemSpec);
                }

                if (sb.ToString().EndsWith(",", StringComparison.OrdinalIgnoreCase))
                    sb.Remove(sb.Length - 1, 1);
            }

            if (this.IncludeInternalMembers)
            {
                sb.Append("/internal+");
            }

            return sb.ToString();
        }

        protected override string GenerateFullPathToTool()
        {
            return Path.Combine(Environment.GetEnvironmentVariable("DXROOT"), @"ProductionTools");
        }

    }
}
