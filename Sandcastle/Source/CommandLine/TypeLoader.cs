using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Microsoft.Ddue.Tools.CommandLine {

    public class TypeLoader {

        public static T CreateType<T>(ConfigurationContext context, string xpath, Func<T> defaultInstance, params object[] constructorArgs) {
            XPathNavigator navigator = context.Configuration.CreateNavigator().SelectSingleNode(xpath);

            if (navigator == null) {
                ConsoleApplication.WriteMessage(
                    LogLevel.Notice,
                    "Configuration information for type {0} does not exists at xpath '{1}'. Creating default instance.",
                    typeof(T).FullName,
                    xpath);

                return defaultInstance();
            }

            string assemblyPath = GetAttribute(navigator, "assembly");
            string typeName = GetAttribute(navigator, "type");

            ConsoleApplication.WriteMessage(
                LogLevel.Notice, 
                "Found configuration information for type {0} at xpath '{1}'. Creating new instance using type='{2}' assembly='{3}'.",
                typeof(T).FullName,
                xpath,
                typeName,
                assemblyPath);

            try {
                T obj = CreateType<T>(assemblyPath, typeName, Path.GetDirectoryName(context.ConfigurationFile), constructorArgs);
                return obj;
            } catch (ConfigurationException e) {
                throw new SandcastleBuildException(e, "Failed to create {0} object because the configuration is not valid.", typeof(T).Name);
            }
        }

        public static T CreateType<T>(XPathNavigator navigator, string baseDirectory, params object[] constructorArgs) {
            if (navigator == null)
                throw new ArgumentNullException("navigator");

            string assemblyPath = GetAttribute(navigator, "assembly");
            string typeName = GetAttribute(navigator, "type");

            ConsoleApplication.WriteMessage(
                LogLevel.Notice,
                "Found configuration information for type {0} at xpath '{1}'. Creating new instance using type='{2}' assembly='{3}'.",
                typeof(T).FullName,
                navigator.GetNodePath(),
                typeName,
                assemblyPath);

            try {
                T obj = CreateType<T>(assemblyPath, typeName, baseDirectory, constructorArgs);
                return obj;
            } catch (ConfigurationException e) {
                throw new SandcastleBuildException(e, "Failed to create {0} object because the configuration is not valid.", typeof(T).Name);
            }
        }


        public static T CreateType<T>(string assemblyPath, string typeName, string baseDirectory, params object[] constructorArgs) {
            if (assemblyPath == null)
                throw new ArgumentNullException("assemblyPath");
            if (typeName == null)
                throw new ArgumentNullException("typeName");

            assemblyPath = Environment.ExpandEnvironmentVariables(assemblyPath);
            if (!Path.IsPathRooted(assemblyPath) && baseDirectory != null)
                assemblyPath = Path.Combine(baseDirectory, assemblyPath);

            try {

                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                T obj = (T)assembly.CreateInstance(typeName, false, BindingFlags.Public | BindingFlags.Instance, null, constructorArgs, null, null);

                if (obj == null)
                    throw new SandcastleBuildException("The type '{0}' was not found in the component assembly '{1}'.", typeName, assemblyPath);

                return obj;

            } catch (IOException e) {
                throw new SandcastleBuildException(e, "A file access error occured while attempting to load the component assembly '{0}'. The error message is: {1}", assemblyPath, e.Message);
            } catch (UnauthorizedAccessException e) {
                throw new SandcastleBuildException(e, "A file access error occured while attempting to load the component assembly '{0}'. The error message is: {1}", assemblyPath, e.Message);
            } catch (BadImageFormatException) {
                throw new SandcastleBuildException("The component assembly '{0}' is not a valid managed assembly.", assemblyPath);
            } catch (TypeLoadException e) {
                throw new SandcastleBuildException(e, "The type '{0}' was not found in the component assembly '{1}'.", typeName, assemblyPath);
            } catch (MissingMethodException) {
                throw new SandcastleBuildException("No appropriate constructor exists for the type'{0}' in the component assembly '{1}'.", typeName, assemblyPath);
            } catch (TargetInvocationException e) {
                throw new SandcastleBuildException(e, "An error occured while initializing the type '{0}' in the component assembly '{1}'. The error message and stack trace follows: {2}", typeName, assemblyPath, e.InnerException.ToString());
            } catch (InvalidCastException e) {
                throw new SandcastleBuildException(e, "The type '{0}' in the component assembly '{1}' is not a component type.", typeName, assemblyPath);
            }
        }

        private static string GetAttribute(XPathNavigator navigator, string name) {
            string value = navigator.GetAttribute(name, "");

            if (value == null)
                throw new ConfigurationException(
                    String.Format("Missing attribute '{0}' on the element '{1}'.", name, navigator.GetNodePath()));

            return value;
        }
    }
}
