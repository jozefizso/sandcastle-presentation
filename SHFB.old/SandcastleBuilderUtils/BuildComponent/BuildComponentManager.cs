//=============================================================================
// System  : Sandcastle Help File Builder Utilities
// File    : BuildComponentManager.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 02/11/2009
// Note    : Copyright 2007-2009, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the class that manages the set of third party build
// components.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://www.CodePlex.com/SHFB.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.6.0.2  11/01/2007  EFW  Created the code
// 1.8.0.0  10/06/2008  EFW  Changed the default location of custom components
//=============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.XPath;

using SandcastleBuilder.Utils.BuildEngine;

// All classes go in the SandcastleBuilder.Utils.BuildComponent namespace
namespace SandcastleBuilder.Utils.BuildComponent
{
    /// <summary>
    /// This class is used to manage the set of third party build components.
    /// </summary>
    public static class BuildComponentManager
    {
        #region Private data members
        //=====================================================================
        // Private data members

        private static Dictionary<string, BuildComponentInfo> buildComponents;
        private static string sandcastlePath, shfbFolder, buildComponentFolder;

        private static Regex reMatchPath = new Regex(
            @"[A-Z]:\\.[^;]+\\Sandcastle(?=\\Prod)", RegexOptions.IgnoreCase);
        private static Regex reMatchShfbFolder = new Regex("{@SHFBFolder}",
            RegexOptions.IgnoreCase);
        private static Regex reMatchCompFolder = new Regex("{@ComponentsFolder}",
            RegexOptions.IgnoreCase);
        #endregion

        #region Properties
        //=====================================================================
        // Properties

        /// <summary>
        /// This read-only property returns the build components folder
        /// </summary>
        public static string BuildComponentsFolder
        {
            get { return buildComponentFolder; }
        }

        /// <summary>
        /// This is used to set or get the Sandcastle installation folder
        /// </summary>
        public static string SandcastlePath
        {
            get
            {
                // Figure out where Sandcastle is if not specified
                if(String.IsNullOrEmpty(sandcastlePath))
                {
                    // Try to find it based on the DXROOT environment variable
                    sandcastlePath = Environment.GetEnvironmentVariable("DXROOT");
                    if(String.IsNullOrEmpty(sandcastlePath) ||
                      !sandcastlePath.Contains(@"\Sandcastle"))
                        sandcastlePath = String.Empty;

                    if(sandcastlePath.Length == 0)
                    {
                        // Search for it in the PATH environment variable
                        Match m = reMatchPath.Match(
                            Environment.GetEnvironmentVariable("PATH"));

                        // If not found in the path, search all fixed drives
                        if(m.Success)
                            sandcastlePath = m.Value;
                        else
                        {
                            sandcastlePath = BuildProcess.FindOnFixedDrives(
                                @"\Sandcastle");

                            // If not found there, try the VS 2005 SDK folders
                            if(sandcastlePath.Length == 0)
                            {
                                sandcastlePath = BuildProcess.FindSdkExecutable(
                                    "MRefBuilder.exe");

                                if(sandcastlePath.Length != 0)
                                    sandcastlePath = sandcastlePath.Substring(0,
                                        sandcastlePath.LastIndexOf('\\'));
                            }
                        }
                    }
                }
                else
                    if(!File.Exists(sandcastlePath + @"ProductionTools\MRefBuilder.exe"))
                        sandcastlePath = String.Empty;

                return sandcastlePath;
            }
            set
            {
                if(String.IsNullOrEmpty(value))
                    sandcastlePath = null;
                else
                    sandcastlePath = value;
            }
        }

        /// <summary>
        /// This returns a dictionary containing the loaded build component
        /// information.
        /// </summary>
        /// <value>The dictionary keys are the component IDs.</value>
        public static Dictionary<string, BuildComponentInfo> BuildComponents
        {
            get
            {
                if(buildComponents == null)
                    LoadBuildComponents();

                return buildComponents;
            }
        }
        #endregion

        /// <summary>
        /// Load the build components found in the ".\Build Components" folder
        /// and its subfolders.
        /// </summary>
        private static void LoadBuildComponents()
        {
            XPathDocument configFile;
            XPathNavigator navConfig;
            Assembly asm;
            BuildComponentInfo info;
            string[] files;

            buildComponents = new Dictionary<string, BuildComponentInfo>();
            asm = Assembly.GetExecutingAssembly();

            // Third party build components should be located in a
            // "EWSoftware\Sandcastle Help File Builder\Build Components"
            // folder in the common application data folder.
            shfbFolder = asm.Location;
            shfbFolder = shfbFolder.Substring(0,
                shfbFolder.LastIndexOf('\\') + 1);
            buildComponentFolder = FolderPath.TerminatePath(Path.Combine(
                Environment.GetFolderPath(
                Environment.SpecialFolder.CommonApplicationData),
                Constants.ComponentsFolder));

            if(Directory.Exists(buildComponentFolder))
                files = Directory.GetFiles(buildComponentFolder,
                    "*.config", SearchOption.AllDirectories);
            else
                files = new string[0];

            List<string> allFiles = new List<string>(files);

            // Add the standard config file too
            allFiles.Insert(0, Path.Combine(shfbFolder,
                "SandcastleBuilder.Components.config"));

            foreach(string file in allFiles)
            {
                configFile = new XPathDocument(file);
                navConfig = configFile.CreateNavigator();

                foreach(XPathNavigator component in navConfig.Select(
                  "components/component"))
                {
                    info = new BuildComponentInfo(component);

                    // Ignore components with duplicate IDs
                    if(!buildComponents.ContainsKey(info.Id))
                        buildComponents.Add(info.Id, info);
                }
            }
        }

        /// <summary>
        /// This is used to resolve replacement tags and environment variables
        /// in a build component's assembly path and return the actual path
        /// to it.
        /// </summary>
        /// <param name="path">The path to resolve</param>
        /// <returns>The actual absolute path to the assembly</returns>
        public static string ResolveComponentPath(string path)
        {
            if(String.IsNullOrEmpty(shfbFolder))
                LoadBuildComponents();

            path = reMatchShfbFolder.Replace(path, shfbFolder);
            path = reMatchCompFolder.Replace(path, buildComponentFolder);
            return Environment.ExpandEnvironmentVariables(path);
        }
    }
}
