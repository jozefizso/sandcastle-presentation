using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Ddue.Tools.CommandLine {

    public class SandcastleBuildException : Exception {

        public SandcastleBuildException(string format, params object[] args)
            : base(String.Format(format, args)) {
            this.LogLevel = LogLevel.Error;
        }

        public SandcastleBuildException(LogLevel logLevel, string message, Exception innerException)
            : base(message, innerException) {
            this.LogLevel = logLevel;
        }

        public SandcastleBuildException(Exception innerException, LogLevel logLevel, string message) : base(message, innerException) {
            this.LogLevel = logLevel;
        }

        public SandcastleBuildException(Exception innerException, LogLevel logLevel, string format, params object[] args)
            : base(String.Format(format, args), innerException) {
            this.LogLevel = logLevel;
        }

        public SandcastleBuildException(Exception innerException, string format, params object[] args)
            : base(String.Format(format, args), innerException) {
            this.LogLevel = LogLevel.Error;
        }

        public LogLevel LogLevel { get; set; }

    }
}
