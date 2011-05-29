using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Ddue.Tools.Reflection;
using System.Xml.XPath;
using System.IO;
using Microsoft.Ddue.Tools.CommandLine;
using System.Xml;
using System.Compiler;
using System.Reflection;
using System.Configuration;
using System.Runtime.Versioning;

namespace Microsoft.Ddue.Tools {

    public class MRefBuilderApp {

        public MRefBuilderApp(string configFile, IEnumerable<string> assemblies, IEnumerable<string> dependencies, string outputFile, bool includeInternalMembers) {

            this.Assemblies = assemblies;
            this.Dependencies = dependencies;
            this.OutputFile = outputFile;
            this.BuilderAddins = new List<MRefBuilderAddIn>();

            this.LoadConfiguration(configFile, includeInternalMembers);
        }

        public string ConfigurationFile { get; protected set; }

        public IEnumerable<string> Assemblies { get; set; }

        public IEnumerable<string> Dependencies { get; set; }

        public string OutputFile { get; set; }

        public FrameworkName FrameworkName { get; protected set; }

        public ApiNamer ApiNamer { get; protected set; }

        public ApiFilter ApiFilter { get; protected set; }

        public AssemblyResolver Resolver { get; protected set; }

        public ICollection<MRefBuilderAddIn> BuilderAddins { get; protected set; }

        public void VisitApis() {
            ConsoleApplication.WriteBanner();

            using (TextWriter writer = new StreamWriter(this.OutputFile, false, Encoding.UTF8))
            using (ManagedReflectionWriter builder = new ManagedReflectionWriter(writer, this.ApiFilter, this.ApiNamer)) {

                builder.Resolver = this.Resolver;

                // register add-ins to the builder
                builder.RegisterAddins(this.BuilderAddins);

                // load dependent bits
                foreach (string dependency in this.Dependencies) {
                    try {
                        builder.LoadAccessoryAssemblies(dependency);
                    } catch (IOException e) {
                        throw new SandcastleBuildException(e, "An error occured while loading dependency assembly. Assembly path: '{0}'.", dependency);
                    }
                }

                // parse the bits
                foreach (string dllPath in this.Assemblies) {
                    try {
                        builder.LoadAssemblies(dllPath);
                    } catch (IOException e) {
                        throw new SandcastleBuildException(e, "An error occured while loading assembly for reflection. Assembly path: '{0}'.", dllPath);
                    }
                }

                ConsoleApplication.WriteMessage(LogLevel.Info, "Loaded {0} assemblies for reflection and {1} dependency assemblies.", builder.Assemblies.Length, builder.AccessoryAssemblies.Length);

                builder.VisitApis();

                ConsoleApplication.WriteMessage(LogLevel.Info, "Wrote information on {0} namespaces, {1} types, and {2} members.", builder.Namespaces.Length, builder.Types.Length, builder.Members.Length);
            }
        }

        private void LoadConfiguration(string configFile, bool includeInternalMembers) {
            // load the configuration file
            XPathDocument config;
            string configDirectory = Path.GetDirectoryName(configFile);
            try {
                config = new XPathDocument(configFile);
            } catch (IOException e) {
                throw new SandcastleBuildException(e, "An error occured while attempting to read the configuration file '{0}'", configFile);
            } catch (UnauthorizedAccessException e) {
                throw new SandcastleBuildException(e, "An error occured while attempting to read the configuration file '{0}'", configFile);
            } catch (XmlException e) {
                throw new SandcastleBuildException(e, "The configuration file '{0}' is not well-formed.", configFile);
            }

            ConfigurationContext configContext = new ConfigurationContext();
            configContext.ConfigurationFile = configFile;
            configContext.Configuration = config;

            // adjust the target platform
            XPathNodeIterator platformNodes = config.CreateNavigator().Select("/configuration/dduetools/platform");
            if (platformNodes.MoveNext()) {
                XPathNavigator platformNode = platformNodes.Current;
                string version = platformNode.GetAttribute("version", String.Empty);
                string path = platformNode.GetAttribute("path", String.Empty);
                path = Environment.ExpandEnvironmentVariables(path);
                if (!Directory.Exists(path)) {
                    throw new SandcastleBuildException("The specifed target platform directory '{0}' does not exist. Specifie platform version '{1}'.", path, version);
                }
                if (version == "2.0") {
                    TargetPlatform.SetToV2(path);
                } else if (version == "1.1") {
                    TargetPlatform.SetToV1_1(path);
                } else if (version == "1.0") {
                    TargetPlatform.SetToV1(path);
                } else {
                    throw new SandcastleBuildException(null, "The specifed target platform '{0}' is not supported.", version);
                }

                this.FrameworkName = new FrameworkName(".NET Framework, Version=" + version);
            }

            // create a namer
            ApiNamer namer = TypeLoader.CreateType<ApiNamer>(configContext, "/configuration/dduetools/namer", () => new OrcasNamer());

            // create a resolver
            AssemblyResolver resolver = TypeLoader.CreateType<AssemblyResolver>(configContext, "/configuration/dduetools/resolver", () => new AssemblyResolver());

            XPathNodeIterator addinNodes = config.CreateNavigator().Select("/configuration/dduetools/addins/addin");
            foreach (XPathNavigator addinNode in addinNodes) {
                MRefBuilderAddIn addin = TypeLoader.CreateType<MRefBuilderAddIn>(addinNode, configDirectory, addinNode);
                if (addin != null)
                    this.BuilderAddins.Add(addin);
            }


            XPathNavigator filterConfig = config.CreateNavigator().SelectSingleNode("/configuration/dduetools");
            if (includeInternalMembers)
                this.ApiFilter = new AllDocumentedFilter(filterConfig);
            else
                this.ApiFilter = new ExternalDocumentedFilter(filterConfig);

            this.ApiNamer = namer;
            this.Resolver = resolver;
        }
    }
}
