using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Build.Evaluation;

using NUnit.Framework;

namespace SandcastleBuilder.Utils.Tests.MSBuild
{
    [TestFixture]
    public class BuildPropertyTests
    {
        private static List<string> restrictedProps = new List<string>() {
            "AssemblyName", "Configuration", "Name", "Platform", "ProjectGuid",
            "RootNamespace", "SHFBSchemaVersion", "SchemaVersion" };

        private static List<string> sandcastleProjectProps = new List<string>()
        {
            "MSBuildProject",
            "UsingFinalValues",
            "Filename",
            "Configuration",
            "Platform",
            "MSBuildOutDir",
            "IsDirty",
            "References",
            "DocumentationSources",
            "LogFileLocation",
            "DecodedCopyrightText",
            "MissingTags",
            "VisibleItems",
            "FileItems",
            "NamespaceSummaries",
            "ProjectSummary",
            "HtmlHelp1xCompilerPath",
            "HtmlHelp2xCompilerPath",
            "OutputPath",
            "SandcastlePath",
            "WorkingPath",
            "CleanIntermediates",
            "KeepLogFile",
            "BuildLogFile",
            "HelpFileFormat",
            "CppCommentsFixup",
            "FrameworkVersion",
            "ComponentConfigurations",
            "PlugInConfigurations",
            "UserDefinedProperties",
            "ContentPlacement",
            "IndentHtml",
            "Preliminary",
            "RootNamespaceContainer",
            "RootNamespaceTitle",
            "HelpTitle",
            "HtmlHelpName",
            "Language",
            "CopyrightHref",
            "CopyrightText",
            "FeedbackEMailAddress",
            "FeedbackEMailLinkText",
            "HeaderText",
            "FooterText",
            "ProjectLinkType",
            "SdkLinkType",
            "SdkLinkTarget",
            "PresentationStyle",
            "NamingMethod",
            "SyntaxFilters",
            "BinaryTOC",
            "IncludeFavorites",
            "CollectionTocStyle",
            "IncludeStopWordList",
            "PlugInNamespaces",
            "HelpFileVersion",
            "HelpAttributes",
            "ShowMissingNamespaces",
            "ShowMissingSummaries",
            "ShowMissingParams",
            "ShowMissingTypeParams",
            "ShowMissingReturns",
            "ShowMissingValues",
            "ShowMissingRemarks",
            "AutoDocumentConstructors",
            "AutoDocumentDisposeMethods",
            "ShowMissingIncludeTargets",
            "DocumentAttributes",
            "DocumentExplicitInterfaceImplementations",
            "DocumentInheritedMembers",
            "DocumentInheritedFrameworkMembers",
            "DocumentInheritedFrameworkPrivateMembers",
            "DocumentInheritedFrameworkInternalMembers",
            "DocumentInternals",
            "DocumentPrivates",
            "DocumentPrivateFields",
            "DocumentProtected",
            "DocumentProtectedInternalAsProtected",
            "DocumentSealedProtected",
            "ApiFilter",
            "BasePath"
        };

        [Test]
        public void Project_GetUserDefinedProperties()
        {
            Project project = ProjectCollection.GlobalProjectCollection.LoadProject(
                @"Data\Project2.proj");

            var props = GetUserDefinedProperties(project);

            Assert.AreEqual(1, props.Count);
            Assert.IsNotNull(props[0]);
            Assert.AreEqual("MojaVlastnost", props[0].Name);
        }

        /// <summary>
        /// Get a collection containing all user-defined properties
        /// </summary>
        /// <returns>A collection containing all properties determined not to
        /// be help file builder project properties, MSBuild build engine
        /// related properties, or environment variables.</returns>
        internal Collection<ProjectProperty> GetUserDefinedProperties(Project msBuildProject)
        {
            Collection<ProjectProperty> userProps = new Collection<ProjectProperty>();
            
            // Note: type == 0 is NormalProperty
            if (msBuildProject != null)
            {
                foreach (ProjectProperty prop in msBuildProject.AllEvaluatedProperties)
                {
                    if (prop.IsImported || prop.IsEnvironmentProperty || prop.IsGlobalProperty
                        || prop.IsReservedProperty)
                        continue;

                    if (!sandcastleProjectProps.Contains(prop.Name)
                        && !restrictedProps.Contains(prop.Name))
                        userProps.Add(prop);
                }
            }
            return userProps;
        }
    }
}
