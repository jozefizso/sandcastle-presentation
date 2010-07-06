using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Build.BuildEngine;

using NUnit.Framework;
using System.Reflection;

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
            Project project = new Project(Engine.GlobalEngine);
            project.Load(@"Data\Project2.proj");

            var props = GetUserDefinedProperties(project);

            Assert.AreEqual(1, props.Count);
        }

        /// <summary>
        /// Get a collection containing all user-defined properties
        /// </summary>
        /// <returns>A collection containing all properties determined not to
        /// be help file builder project properties, MSBuild build engine
        /// related properties, or environment variables.</returns>
        internal Collection<BuildProperty> GetUserDefinedProperties(Project msBuildProject)
        {
            Collection<BuildProperty> userProps = new Collection<BuildProperty>();
            Type type = typeof(BuildProperty);
            FieldInfo field = type.GetField("type", BindingFlags.NonPublic |
                BindingFlags.Instance);

            var propertyCache = msBuildProject.EvaluatedProperties;

            // Note: type == 0 is NormalProperty
            if (msBuildProject != null && field != null && propertyCache != null)
                foreach (BuildProperty prop in msBuildProject.EvaluatedProperties)
                    if (!prop.IsImported && (int)field.GetValue(prop) == 0 &&
                      !sandcastleProjectProps.Contains(prop.Name) &&
                      restrictedProps.IndexOf(prop.Name) == -1)
                        userProps.Add(prop);

            return userProps;
        }
    }
}
