using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Ddue.Tools.CommandLine {
    public class ConsoleLogger : IConsoleLogger {

        public void WriteMessage(LogLevel level, string message) {
            Console.WriteLine("{0}: {1}", level, message);
        }
    }
}
