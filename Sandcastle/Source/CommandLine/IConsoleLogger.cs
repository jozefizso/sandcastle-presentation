using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Ddue.Tools.CommandLine {

    public interface IConsoleLogger {
        void WriteMessage(LogLevel level, string message);
    }
}
