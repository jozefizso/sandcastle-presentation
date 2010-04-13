//=============================================================================
// System  : Sandcastle Help File Builder MSBuild Tasks
// File    : CleanHelp.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 06/29/2008
// Note    : Copyright 2008, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the MSBuild task used to clean (remove) help file output
// from the last build.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://www.CodePlex.com/SHFB.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.8.0.0  06/27/2008  EFW  Created the code
// ============================================================================

using System;
using System.IO;

using Microsoft.Build.BuildEngine;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using SandcastleBuilder.Utils;
using SandcastleBuilder.Utils.BuildEngine;

namespace SandcastleBuilder.Utils.MSBuild
{
    /// <summary>
    /// This task is used to clean (remove) help file output from the last
    /// build.
    /// </summary>
    public class CleanHelp : Task
    {
        #region Private data members
        //=====================================================================

        private string projectFile, outputPath, workingPath, logFileLocation;
        #endregion

        #region Task input properties
        //=====================================================================

        /// <summary>
        /// This is used to pass in the project filename
        /// </summary>
        /// <remarks>Since <see cref="SandcastleProject" /> already wraps the
        /// MSBuild project, it made sense to just load it in the task and
        /// build it that way rather than have one or more other structures
        /// to hold passed in project properties that then got passed to the
        /// build process.</remarks>
        [Required]
        public string ProjectFile
        {
            get { return projectFile; }
            set { projectFile = value; }
        }

        /// <summary>
        /// This is used to pass in the output path that needs to be cleaned
        /// </summary>
        [Required]
        public string OutputPath
        {
            get { return outputPath; }
            set { outputPath = value; }
        }

        /// <summary>
        /// This is used to pass in the optional working path that needs to be
        /// cleaned.
        /// </summary>
        public string WorkingPath
        {
            get { return workingPath; }
            set { workingPath = value; }
        }

        /// <summary>
        /// This is used to pass in the optional log file location that needs
        /// to be cleaned.
        /// </summary>
        public string LogFileLocation
        {
            get { return logFileLocation; }
            set { logFileLocation = value; }
        }
        #endregion

        #region Execute method
        //=====================================================================

        /// <summary>
        /// This is used to execute the task and clean the output folder
        /// </summary>
        /// <returns>True on success or false on failure.</returns>
        public override bool Execute()
        {
            string projectPath;

            try
            {
                projectPath = Path.GetDirectoryName(Path.GetFullPath(
                    projectFile));

                // Make sure we start out in the project's output folder
                // in case the output folder is relative to it.
                Directory.SetCurrentDirectory(Path.GetDirectoryName(
                    Path.GetFullPath(projectPath)));

                // Clean the working folder
                if(!String.IsNullOrEmpty(workingPath))
                {
                    if(!Path.IsPathRooted(workingPath))
                        workingPath = Path.GetFullPath(Path.Combine(
                            projectPath, workingPath));

                    if(Directory.Exists(workingPath))
                    {
                        BuildProcess.VerifySafePath("WorkingPath",
                            workingPath, projectPath);
                        Log.LogMessage("Removing working folder...");
                        Directory.Delete(workingPath, true);
                    }
                }

                if(!Path.IsPathRooted(outputPath))
                    outputPath = Path.GetFullPath(Path.Combine(
                        projectPath, outputPath));

                if(Directory.Exists(outputPath))
                {
                    Log.LogMessage("Removing build files...");
                    BuildProcess.VerifySafePath("OutputPath", outputPath,
                        projectPath);

                    // Read-only and/or hidden files and folders are ignored
                    // as they are assumed to be under source control.
                    foreach(string file in Directory.GetFiles(outputPath))
                        if((File.GetAttributes(file) &
                          (FileAttributes.ReadOnly |
                          FileAttributes.Hidden)) == 0)
                            File.Delete(file);

                    Log.LogMessage("Removing build folders...");

                    foreach(string folder in Directory.GetDirectories(outputPath))
                        if((File.GetAttributes(folder) &
                          (FileAttributes.ReadOnly |
                          FileAttributes.Hidden)) == 0)
                            Directory.Delete(folder, true);

                    // Delete the log file too if it's in a different location
                    if(String.IsNullOrEmpty(logFileLocation))
                        logFileLocation = outputPath;

                    logFileLocation = Path.Combine(logFileLocation,
                        "LastBuildLog.log");

                    if(File.Exists(logFileLocation))
                    {
                        Log.LogMessage("Removing build log...");
                        File.Delete(logFileLocation);
                    }
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Log.LogError(null, "CT0001", "CT0001", "SHFB", 0, 0, 0, 0,
                  "Unable to clean output folder.  Reason: {0}", ex);
                return false;
            }

            return true;
        }
        #endregion
    }
}
