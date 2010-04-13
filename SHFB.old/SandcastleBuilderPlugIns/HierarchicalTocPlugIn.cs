//=============================================================================
// System  : Sandcastle Help File Builder Plug-Ins
// File    : HierarchicalTocPlugIn.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 11/14/2008
// Note    : Copyright 2008, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a plug-in that can be used to rearrange the table of
// contents such that namespaces are nested within their parent namespaces
// rather than appearing as a flat list of all namespaces at the root level.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://www.CodePlex.com/SHFB.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.6.0.6  03/17/2008  EFW  Created the code
// 1.8.0.0  07/22/2008  EFW  Fixed bug caused by root namespace container
//=============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

using SandcastleBuilder.Utils;
using SandcastleBuilder.Utils.BuildEngine;
using SandcastleBuilder.Utils.PlugIn;

namespace SandcastleBuilder.PlugIns
{
    /// <summary>
    /// This plug-in class can be used to rearrange the table of contents such
    /// that namespaces are nested within their parent namespaces rather than
    /// appearing as a flat list of all namespaces at the root level.
    /// </summary>
    public class HierarchicalTocPlugIn : IPlugIn
    {
        #region Private data members
        private ExecutionPointCollection executionPoints;

        private BuildProcess builder;

        private int minParts;
        private bool insertBelow;
        #endregion

        #region IPlugIn implementation
        //=====================================================================
        // IPlugIn implementation

        /// <summary>
        /// This read-only property returns a friendly name for the plug-in
        /// </summary>
        public string Name
        {
            get { return "Hierarchical Table of Contents"; }
        }

        /// <summary>
        /// This read-only property returns the version of the plug-in
        /// </summary>
        public Version Version
        {
            get
            {
                // Use the assembly version
                Assembly asm = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(
                    asm.Location);

                return new Version(fvi.ProductVersion);
            }
        }

        /// <summary>
        /// This read-only property returns the copyright information for the
        /// plug-in.
        /// </summary>
        public string Copyright
        {
            get
            {
                // Use the assembly copyright
                Assembly asm = Assembly.GetExecutingAssembly();
                AssemblyCopyrightAttribute copyright =
                    (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(
                        asm, typeof(AssemblyCopyrightAttribute));

                return copyright.Copyright;
            }
        }

        /// <summary>
        /// This read-only property returns a brief description of the plug-in
        /// </summary>
        public string Description
        {
            get
            {
                return "This plug-in can be used to rearrange the table of " +
                    "contents such that namespaces are nested within their " +
                    "parent namespaces rather than appearing as a flat " +
                    "list of all namespaces at the root level.";
            }
        }

        /// <summary>
        /// This plug-in does not run in partial builds
        /// </summary>
        public bool RunsInPartialBuild
        {
            get { return false; }
        }

        /// <summary>
        /// This read-only property returns a collection of execution points
        /// that define when the plug-in should be invoked during the build
        /// process.
        /// </summary>
        public ExecutionPointCollection ExecutionPoints
        {
            get
            {
                if(executionPoints == null)
                {
                    executionPoints = new ExecutionPointCollection();

                    executionPoints.Add(new ExecutionPoint(
                        BuildStep.GenerateIntermediateTableOfContents,
                        ExecutionBehaviors.After));
                }

                return executionPoints;
            }
        }

        /// <summary>
        /// This method is used by the Sandcastle Help File Builder to let the
        /// plug-in perform its own configuration.
        /// </summary>
        /// <param name="project">A reference to the active project</param>
        /// <param name="currentConfig">The current configuration XML fragment</param>
        /// <returns>A string containing the new configuration XML fragment</returns>
        /// <remarks>The configuration data will be stored in the help file
        /// builder project.</remarks>
        public string ConfigurePlugIn(SandcastleProject project,
          string currentConfig)
        {
            using(HierarchicalTocConfigDlg dlg = new HierarchicalTocConfigDlg(
              currentConfig))
            {
                if(dlg.ShowDialog() == DialogResult.OK)
                    currentConfig = dlg.Configuration;
            }

            return currentConfig;
        }

        /// <summary>
        /// This method is used to initialize the plug-in at the start of the
        /// build process.
        /// </summary>
        /// <param name="buildProcess">A reference to the current build
        /// process.</param>
        /// <param name="configuration">The configuration data that the plug-in
        /// should use to initialize itself.</param>
        public void Initialize(BuildProcess buildProcess,
          XPathNavigator configuration)
        {
            XPathNavigator root;
            string option;
            builder = buildProcess;
            minParts = 2;

            builder.ReportProgress("{0} Version {1}\r\n{2}",
                this.Name, this.Version, this.Copyright);

            // Load the configuration
            root = configuration.SelectSingleNode("configuration/toc");

            if(root != null)
            {
                option = root.GetAttribute("minParts", String.Empty);
                if(!String.IsNullOrEmpty(option))
                    minParts = Convert.ToInt32(option,
                        CultureInfo.InvariantCulture);

                if(minParts < 1)
                    minParts = 1;

                option = root.GetAttribute("insertBelow", String.Empty);
                if(!String.IsNullOrEmpty(option))
                    insertBelow = Convert.ToBoolean(option,
                        CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// This method is used to execute the plug-in during the build process
        /// </summary>
        /// <param name="context">The current execution context</param>
        public void Execute(ExecutionContext context)
        {
            List<string> namespaceList = new List<string>();
            Dictionary<string, XmlNode> namespaceNodes =
                new Dictionary<string, XmlNode>();
            XmlDocument toc;
            XPathNavigator root, navToc;
            XmlAttribute attr;
            XmlNode tocEntry, tocParent;
            string[] parts;
            string name, parent, topicTitle;
            int parentIdx, childIdx, entriesAdded;

            builder.ReportProgress("Retrieving namespace topic title from " +
                "shared content...");

            toc = new XmlDocument();
            toc.Load(builder.PresentationStyleFolder +
                @"content\reference_content.xml");
            tocEntry = toc.SelectSingleNode("content/item[@id=" +
                "'namespaceTopicTitle']");

            if(tocEntry != null)
                topicTitle = tocEntry.InnerText;
            else
            {
                builder.ReportWarning("HTP0001", "Unable to locate namespace " +
                    "topic title in reference content file.  Using default.");
                topicTitle = "{0} Namespace";
            }

            builder.ReportProgress("Creating namespace hierarchy...");

            toc = new XmlDocument();
            toc.Load(builder.WorkingFolder + "toc.xml");
            navToc = toc.CreateNavigator();

            // Get a list of the namespaces.  If a root namespace container
            // node is present, we need to look in it rather than the document
            // root node.
            root = navToc.SelectSingleNode("topics/topic[starts-with(@id, 'R:')]");

            if(root == null)
                root = navToc.SelectSingleNode("topics");

            foreach(XPathNavigator ns in root.Select("topic[starts-with(@id, 'N:')]"))
            {
                name = ns.GetAttribute("id", String.Empty);
                namespaceList.Add(name);
                namespaceNodes.Add(name, ((IHasXmlNode)ns).GetNode());
            }

            // See if any container nodes need to be created for namespaces
            // with a common root name.
            for(parentIdx = 0; parentIdx < namespaceList.Count; parentIdx++)
            {
                parts = namespaceList[parentIdx].Split('.');

                // Only do it for namespaces with a minimum number of parts
                if(parts.Length > minParts)
                {
                    for(childIdx = minParts; childIdx < parts.Length; childIdx++)
                    {
                        name = String.Join(".", parts, 0, childIdx);

                        if(!namespaceList.Contains(name))
                        {
                            if(namespaceList.FindAll(delegate (string ns) {
                              return ns.StartsWith(name + ".",
                                StringComparison.Ordinal); }).Count > 0)
                            {
                                // The nodes will be created later once
                                // we know where to insert them.
                                namespaceList.Add(name);
                                namespaceNodes.Add(name, null);
                            }
                        }
                    }
                }
            }

            // Sort them in reverse order
            namespaceList.Sort(delegate (string n1, string n2) {
                    return String.Compare(n2, n1, StringComparison.Ordinal);
                });

            // If any container namespaces were added, create nodes for them
            // and insert them before the namespace ahead of them in the list.
            foreach(string key in namespaceList)
                if(namespaceNodes[key] == null)
                {
                    tocEntry = toc.CreateElement("topic");
                    attr = toc.CreateAttribute("id");

                    attr.Value = String.Format(CultureInfo.InvariantCulture,
                        topicTitle, (key.Length > 2) ? key.Substring(2) :
                        "Global");
                    tocEntry.Attributes.Append(attr);

                    parentIdx = namespaceList.IndexOf(key);
                    tocParent = namespaceNodes[namespaceList[parentIdx - 1]];
                    tocParent.ParentNode.InsertBefore(tocEntry, tocParent);
                    namespaceNodes[key] = tocEntry;
                }

            for(parentIdx = 1; parentIdx < namespaceList.Count; parentIdx++)
            {
                parent = namespaceList[parentIdx];
                entriesAdded = 0;

                // Check each preceding namespace.  If it starts with the
                // parent's name, insert it as a child of that one.
                for(childIdx = 0; childIdx < parentIdx; childIdx++)
                {
                    name = namespaceList[childIdx];
                    
                    if(name.StartsWith(parent + ".", StringComparison.Ordinal))
                    {
                        tocEntry = namespaceNodes[name];
                        tocParent = namespaceNodes[parent];

                        if(insertBelow && entriesAdded < tocParent.ChildNodes.Count)
                            tocParent.InsertAfter(tocEntry, tocParent.ChildNodes[
                                tocParent.ChildNodes.Count - entriesAdded - 1]);
                        else
                            tocParent.InsertBefore(tocEntry,
                                tocParent.ChildNodes[0]);

                        namespaceList.RemoveAt(childIdx);
                        entriesAdded++;
                        parentIdx--;
                        childIdx--;
                    }
                }
            }

            toc.Save(builder.WorkingFolder + "toc.xml");
        }
        #endregion

        #region IDisposable implementation
        //=====================================================================
        // IDisposable implementation

        /// <summary>
        /// This handles garbage collection to ensure proper disposal of the
        /// plug-in if not done explicity with <see cref="Dispose()"/>.
        /// </summary>
        ~HierarchicalTocPlugIn()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// This implements the Dispose() interface to properly dispose of
        /// the plug-in object.
        /// </summary>
        /// <overloads>There are two overloads for this method.</overloads>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// This can be overridden by derived classes to add their own
        /// disposal code if necessary.
        /// </summary>
        /// <param name="disposing">Pass true to dispose of the managed
        /// and unmanaged resources or false to just dispose of the
        /// unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Nothing to dispose of in this one
        }
        #endregion
    }
}
