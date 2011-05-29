// Copyright © Microsoft Corporation.
// This source file is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.IO;
using System.Reflection;
using System.Xml.XPath;
using System.Diagnostics;

namespace Microsoft.Ddue.Tools.CommandLine {

    public static class ConsoleApplication {

        private static IConsoleLogger logger;

        static ConsoleApplication() {
            logger = new ConsoleLogger();
        }

        public static void SetConsoleLogger(IConsoleLogger consoleLogger) {
            if (consoleLogger == null)
                throw new ArgumentNullException("consoleLogger");

            logger = consoleLogger;
        }

        public static XPathDocument GetConfigurationFile() {
            return (GetConfigurationFile(Assembly.GetCallingAssembly().Location + ".config"));
        }

        public static XPathDocument GetConfigurationFile(string file) {
            return (new XPathDocument(file));
        }

        public static string[] GetFiles(string filePattern) {

            // get the full path to the relevent directory
            string directoryPath = Path.GetDirectoryName(filePattern);
            if ((directoryPath == null) || (directoryPath.Length == 0)) directoryPath = Environment.CurrentDirectory;
            directoryPath = Path.GetFullPath(directoryPath);

            // get the file name, which may contain wildcards
            string filePath = Path.GetFileName(filePattern);

            // look up the files and load them
            string[] files = Directory.GetFiles(directoryPath, filePath);

            return (files);

        }

        // Write the name, version, and copyright information of the calling assembly

        public static void WriteBanner() {
            Assembly application = Assembly.GetCallingAssembly();
            AssemblyName applicationData = application.GetName();
            WriteMessage(LogLevel.Info, "{0} (v{1})", applicationData.Name, applicationData.Version);
            Object[] copyrightAttributes = application.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true);
            foreach (AssemblyCopyrightAttribute copyrightAttribute in copyrightAttributes) {
                WriteMessage(LogLevel.Info, copyrightAttribute.Copyright);
            }
        }

        public static void WriteMessage(LogLevel level, string message) {
            logger.WriteMessage(level, message);
        }

        public static void WriteMessage(LogLevel level, string format, object arg1) {
            string message = String.Format(format, arg1);
            WriteMessage(level, message);
        }

        public static void WriteMessage(LogLevel level, string format, object arg1, object arg2) {
            string message = String.Format(format, arg1, arg2);
            WriteMessage(level, message);
        }

        public static void WriteMessage(LogLevel level, string format, object arg1, object arg2, object arg3) {
            string message = String.Format(format, arg1, arg2, arg3);
            WriteMessage(level, message);
        }

        public static void WriteMessage(LogLevel level, string format, params object[] arg)
        {
            string message = String.Format(format, arg);
            WriteMessage(level, message);
        }

    }

}
