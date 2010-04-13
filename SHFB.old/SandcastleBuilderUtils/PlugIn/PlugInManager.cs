//=============================================================================
// System  : Sandcastle Help File Builder Utilities
// File    : PlugInManager.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 10/06/2008
// Note    : Copyright 2007-2008, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the class that manages the set of known plug-ins.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://www.CodePlex.com/SHFB.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.5.2.0  09/09/2007  EFW  Created the code
// 1.6.0.2  11/06/2007  EFW  Added new component config merge build step
// 1.8.0.0  10/06/2008  EFW  Changed the default location of custom plug-ins
//=============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

// All classes go in the SandcastleBuilder.Utils.PlugIn namespace
namespace SandcastleBuilder.Utils.PlugIn
{
    /// <summary>
    /// This class is used to manage the set of known plug-ins.
    /// </summary>
    public static class PlugInManager
    {
        #region Private data members
        //=====================================================================
        // Private data members

        private static Dictionary<string, PlugInInfo> plugIns;
        #endregion

        #region Properties
        //=====================================================================
        // Properties

        /// <summary>
        /// This returns a dictionary containing the loaded plug-ins.
        /// </summary>
        /// <value>The dictionary keys are the plug-in names.</value>
        public static Dictionary<string, PlugInInfo> PlugIns
        {
            get
            {
                if(plugIns == null)
                    LoadPlugIns();

                return plugIns;
            }
        }
        #endregion

        /// <summary>
        /// Load the plug-ins found in the .\Plug-Ins folder and its
        /// subfolders.
        /// </summary>
        private static void LoadPlugIns()
        {
            Assembly asm;
            Type[] types;
            PlugInInfo info;
            string[] files;
            string shfbFolder, plugInsFolder;

            plugIns = new Dictionary<string, PlugInInfo>();
            asm = Assembly.GetExecutingAssembly();

            // Plug-ins should be located in the EWSoftware\Sandcastle Help
            // File Builder\Plug-Ins folder under the common application data
            // folder.
            shfbFolder = asm.Location;
            shfbFolder = shfbFolder.Substring(0,
                shfbFolder.LastIndexOf('\\') + 1);
            plugInsFolder = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.CommonApplicationData),
                Constants.PlugInsFolder);

            if(Directory.Exists(plugInsFolder))
                files = Directory.GetFiles(plugInsFolder, "*.dll",
                    SearchOption.AllDirectories);
            else
                files = new string[0];

            List<string> allFiles = new List<string>(files);

            // Add the standard plug-ins assembly too
            allFiles.Insert(0, Path.Combine(shfbFolder,
                "SandcastleBuilder.PlugIns.dll"));

            foreach(string file in allFiles)
            {
                // Any exceptions that occur during the loading of a plug-in
                // will be handled by the caller.
                asm = Assembly.LoadFrom(file);
                types = asm.GetTypes();

                foreach(Type t in types)
                    if(t.IsPublic && !t.IsAbstract && t.GetInterface(
                      "SandcastleBuilder.Utils.PlugIn.IPlugIn") != null)
                    {
                        info = new PlugInInfo(t);

                        if(!plugIns.ContainsKey(info.Name))
                            plugIns.Add(info.Name, info);
                    }
            }
        }

        /// <summary>
        /// This is used to determine if a plug-in exists with the specified
        /// key and can be used.
        /// </summary>
        /// <param name="key">The key for the plug-in</param>
        /// <returns>True if the plug-in exits and can be used.</returns>
        public static bool IsSupported(string key)
        {
            PlugInInfo info;

            // Does it exist?
            if(!PlugInManager.PlugIns.TryGetValue(key, out info))
                return false;

            return true;
        }
    }
}
