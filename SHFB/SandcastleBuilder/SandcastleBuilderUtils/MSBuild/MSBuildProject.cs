//=============================================================================
// System  : Sandcastle Help File Builder MSBuild Tasks
// File    : MSBuildProject.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 01/03/2010
// Note    : Copyright 2008-2010, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains an MSBuild project wrapper used by the Sandcastle Help
// File builder during the build process.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://SHFB.CodePlex.com.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.8.0.0  07/11/2008  EFW  Created the code
// 1.8.1.3  07/05/2010  JI   Updated to use MSBuild 4.0.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;

using Project = Microsoft.Build.Evaluation.Project;
using System.Diagnostics;

namespace SandcastleBuilder.Utils.MSBuild
{
    /// <summary>
    /// This is a simple wrapper around an MSBuild project that is used to
    /// extract information from it during a help file build.
    /// </summary>
    public class MSBuildProject
    {
        #region Private data members
        //=====================================================================

        private Project msBuildProject;
        private BuildPropertyGroup properties;
        private static Regex reInvalidAttribute = new Regex(
            "The attribute \"(.*?)\" in element \\<(.*?)\\> is unrecognized",
            RegexOptions.IgnoreCase);
        #endregion

        #region Properties
        //=====================================================================
        
        /// <summary>
        /// This is used to get the underlying MSBuild project file reference
        /// </summary>
        public Project ProjectFile
        {
            get { return msBuildProject; }
        }

        /// <summary>
        /// This is used to get the assembly name
        /// </summary>
        public string AssemblyName
        {
            get
            {
                string assemblyName, outputType, outputPath = null;

                if(properties == null)
                    throw new InvalidOperationException("Configuration has " +
                        "not been set");

                // Give precedence to OutDir if defined
                if(properties["OutDir"] != null)
                    outputPath = properties["OutDir"].FinalValue;

                if(String.IsNullOrEmpty(outputPath) &&
                  properties["OutputPath"] != null)
                    outputPath = properties["OutputPath"].FinalValue;

                if(!String.IsNullOrEmpty(outputPath))
                {
                    assemblyName = properties["AssemblyName"].FinalValue;

                    if(properties["OutputType"] != null)
                        outputType = properties["OutputType"].FinalValue;
                    else
                        outputType = null;
                }
                else
                    assemblyName = outputType = null;

                if(!String.IsNullOrEmpty(assemblyName))
                {
                    // The values are case insensitive
                    if(String.Compare(outputType, "Library", StringComparison.OrdinalIgnoreCase) == 0)
                        assemblyName += ".dll";
                    else
                        if(String.Compare(outputType, "Exe", StringComparison.OrdinalIgnoreCase) == 0 ||
                          String.Compare(outputType, "WinExe", StringComparison.OrdinalIgnoreCase) == 0)
                            assemblyName += ".exe";
                        else
                            assemblyName = null;

                    if(assemblyName != null)
                        if (Path.IsPathRooted(outputPath))
                            assemblyName = Path.Combine(outputPath, assemblyName);
                        else
                            assemblyName = Path.Combine(Path.Combine(
                                msBuildProject.DirectoryPath,
                                outputPath), assemblyName);
                }

                return assemblyName;
            }
        }

        /// <summary>
        /// This is used to get the XML comments file name
        /// </summary>
        public string XmlCommentsFile
        {
            get
            {
                string docFile = null, outputPath = null;

                if(properties == null)
                    throw new InvalidOperationException("Configuration has " +
                        "not been set");

                if(properties["DocumentationFile"] != null)
                {
                    docFile = properties["DocumentationFile"].FinalValue;

                    if(!String.IsNullOrEmpty(docFile))
                    {
                        // If rooted, take the path as it is
                        if(!Path.IsPathRooted(docFile))
                        {
                            // Give precedence to OutDir if defined
                            if(properties["OutDir"] != null)
                                outputPath = properties["OutDir"].FinalValue;

                            if(!String.IsNullOrEmpty(outputPath))
                            {
                                if(Path.IsPathRooted(outputPath))
                                    docFile = Path.Combine(outputPath,
                                        Path.GetFileName(docFile));
                                else
                                    docFile = Path.Combine(Path.Combine(
                                        msBuildProject.DirectoryPath,
                                        outputPath), Path.GetFileName(docFile));

                                // Fall back to the original location if not found
                                if(!File.Exists(docFile))
                                    docFile = Path.Combine(msBuildProject.DirectoryPath, docFile);
                            }
                            else
                                docFile = Path.Combine(msBuildProject.DirectoryPath, docFile);
                        }
                    }
                }
                else
                {
                    // If not defined, assume it's in the same place as the
                    // assembly with the same name but a ".xml" extension.
                    // This can happen when using Team Build for some reason.
                    docFile = this.AssemblyName;

                    if(!String.IsNullOrEmpty(docFile))
                    {
                        docFile = Path.ChangeExtension(docFile, ".xml");

                        if(!File.Exists(docFile))
                            docFile = null;
                    }
                }

                return docFile;
            }
        }

        /// <summary>
        /// This is used to get the target framework version
        /// </summary>
        public string TargetFrameworkVersion
        {
            get
            {
                if(properties == null)
                    throw new InvalidOperationException("Configuration has " +
                        "not been set");

                if(properties["TargetFrameworkVersion"] != null)
                    return properties["TargetFrameworkVersion"].FinalValue;

                return null;
            }
        }

        /// <summary>
        /// This is used to get the project GUID
        /// </summary>
        public string ProjectGuid
        {
            get
            {
                if(properties == null)
                    throw new InvalidOperationException("Configuration has " +
                        "not been set");

                if(properties["ProjectGuid"] == null)
                    return Guid.Empty.ToString();

                return properties["ProjectGuid"].FinalValue;
            }
        }
        #endregion

        #region Constructor, methods, etc.
        //=====================================================================

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="projectFile">The MSBuild project to load</param>
        public MSBuildProject(string projectFile)
        {
            if (!File.Exists(projectFile))
                throw new BuilderException("BE0051", "The specified project " +
                    "file does not exist: " + projectFile);

            if (Path.GetExtension(projectFile).ToUpperInvariant() == ".VCPROJ")
                throw new BuilderException("BE0068", "Incompatible Visual " +
                    "Studio project file format.  See error code help topic " +
                    "for more information.\r\nC++ project files prior to Visual " +
                    "Studio 2010 are not currently supported.");

            try
            {
                msBuildProject = new Project(
                    projectFile,
                    null,
                    null,
                    ProjectCollection.GlobalProjectCollection);
            }
            catch (InvalidProjectFileException ex)
            {
                // Some MSBuild 4.0 projects cannot be loaded yet.  Their
                // targets must be added as individual documentation sources
                // and reference items.
                if(reInvalidAttribute.IsMatch(ex.Message))
                    throw new BuilderException("BE0068", "Incompatible Visual " +
                        "Studio project file format.  See error code help topic " +
                        "for more information.\r\nThis project may be for a " +
                        "newer version of MSBuild and cannot be loaded.  " +
                        "Error message:", ex);

                throw;
            }
        }

        /// <summary>
        /// This is used to set the active configuration and platform used when
        /// evaluating the properties.
        /// </summary>
        /// <param name="configuration">The active configuration</param>
        /// <param name="platform">The active platform</param>
        /// <param name="outDir">The output directory</param>
        /// <remarks>If the platform is set to any variation of "Any CPU" and
        /// it isn't found in the project, it will be converted to "AnyCPU"
        /// (no space).  This works around an issue with Team Build that
        /// includes the space even though it should not be present.</remarks>
        public void SetConfiguration(string configuration, string platform,
          string outDir)
        {
            if(platform.Equals("Any CPU", StringComparison.OrdinalIgnoreCase))
            {
                var values = msBuildProject.ConditionedProperties[ProjectElement.Platform];

                if (!values.Contains(platform) &&
                    values.Contains(SandcastleProject.DefaultPlatform))
                {
                    platform = SandcastleProject.DefaultPlatform;
                }
            }

            msBuildProject.SetGlobalProperty(ProjectElement.Configuration, configuration);
            msBuildProject.SetGlobalProperty(ProjectElement.Platform, platform);

            if(!String.IsNullOrEmpty(outDir))
                msBuildProject.SetGlobalProperty(ProjectElement.OutDir, outDir);

            ////properties = msBuildProject.EvaluatedProperties;
            msBuildProject.ReevaluateIfNecessary();
        }

        /// <summary>
        /// This is used to set the Visual Studio solution macros based on the
        /// specified project name.
        /// </summary>
        /// <param name="solutionName">The solution name to use</param>
        public void SetSolutionMacros(string solutionName)
        {
            msBuildProject.SetGlobalProperty(
                ProjectElement.SolutionPath, solutionName);
            msBuildProject.SetGlobalProperty(
                ProjectElement.SolutionDir, FolderPath.TerminatePath(
                    Path.GetDirectoryName(solutionName)));
            msBuildProject.SetGlobalProperty(
                ProjectElement.SolutionFileName, Path.GetFileName(solutionName));
            msBuildProject.SetGlobalProperty(
                ProjectElement.SolutionName,
                Path.GetFileNameWithoutExtension(solutionName));
            msBuildProject.SetGlobalProperty(
                ProjectElement.SolutionExt, Path.GetExtension(solutionName));
            
            ////properties = msBuildProject.EvaluatedProperties;
            msBuildProject.ReevaluateIfNecessary();
        }

        /// <summary>
        /// Clone the project's references and add them to the dictionary
        /// </summary>
        /// <param name="references">The dictionary used to contain the
        /// cloned references</param>
        public void CloneReferences(List<ProjectItem> references)
        {
            ////BuildItem refItem;
            ////rootPath, path;
            Project dummyProject = new Project();

            string rootPath = msBuildProject.DirectoryPath;

            foreach(string refType in (new string[] { "Reference",
              "COMReference", "ProjectReference" }))
                foreach (ProjectItem reference in msBuildProject.GetItems(refType))
                {
                    // Ignore nested project references.  We'll assume that
                    // they exist in the folder with the target and they'll
                    // be found automatically.  Imported references are also
                    // ignored since cloning them doesn't turn off the imported
                    // flag and we can't modify them.  Those will have to be
                    // added to the SHFB project as reference items if needed.
                    if(reference.ItemType == "ProjectReference" || reference.IsImported)
                        continue;

                    if (references.Exists(p => p.EvaluatedInclude == reference.EvaluatedInclude))
                        continue;

                    var metadata = new List<ProjectMetadata>(reference.DirectMetadata);
                    
                    if (reference.ItemType == "Reference")
                    {
                        var hintPath = metadata.Find(pm => pm.Name == "HintPath");
                        string path = hintPath.EvaluatedValue;
                        if (hintPath != null && !Path.IsPathRooted(path))
                        {
                            hintPath.UnevaluatedValue = Path.Combine(path);
                        }
                    }

                    var newList = dummyProject.AddItemFast(
                        reference.ItemType,
                        reference.UnevaluatedInclude,
                        metadata.Select(pm => new KeyValuePair<string,string>(pm.Name, pm.UnevaluatedValue))
                        );

                    Debug.Assert(
                        newList.Count > 0,
                        "newList.Count > 0",
                        "No new ProjectItem was created. "+ reference.ToString());

                    if (newList.Count > 0)
                        references.Add(newList[0]);
                }
        }
        #endregion
    }
}
