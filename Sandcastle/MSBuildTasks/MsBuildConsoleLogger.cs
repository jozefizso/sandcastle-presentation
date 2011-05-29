using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Ddue.Tools.CommandLine;

namespace Sandcastle.Build.Tasks
{
    public class MsBuildConsoleLogger : IConsoleLogger
    {
        private TaskLoggingHelper log;

        public MsBuildConsoleLogger(TaskLoggingHelper log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            this.log = log;
        }

        public void WriteMessage(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Notice:
                    this.log.LogMessage(MessageImportance.Low, message);
                    break;
                case LogLevel.Info:
                    this.log.LogMessage(message);
                    break;
                case LogLevel.Warn:
                    this.log.LogWarning(message);
                    break;
                case LogLevel.Error:
                    this.log.LogError(message);
                    break;
                default:
                    this.log.LogMessage(message);
                    break;
            }
        }
    }
}
