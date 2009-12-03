//=============================================================================
// System  : Sandcastle Help File Builder Utilities
// File    : BuildProcess.HelpFileUtils.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 01/22/2009
// Note    : Copyright 2006-2009, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the code used to modify the help file project files to
// create a better table of contents and find the default help file page
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://www.CodePlex.com/SHFB.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0.0  08/07/2006  EFW  Created the code
// 1.2.0.0  09/06/2006  EFW  Added support for TOC content placement
// 1.3.0.0  09/09/2006  EFW  Added support for website output
// 1.3.1.0  10/02/2006  EFW  Added support for the September CTP
// 1.3.2.0  11/04/2006  EFW  Added support for the NamingMethod property
// 1.5.0.0  06/19/2007  EFW  Various additions and updates for the June CTP
// 1.5.2.0  09/13/2007  EFW  Added support for calling plug-ins
// 1.6.0.5  02/04/2008  EFW  Adjusted loading of Help 1 TOC to use an encoding
//                           based on the chosen language.
// 1.6.0.7  04/12/2007  EFW  Added support for a split table of contents
//=============================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Web;

using SandcastleBuilder.Utils.PlugIn;

namespace SandcastleBuilder.Utils.BuildEngine
{
    partial class BuildProcess
    {
        #region Private data members
        //=====================================================================
        // Private data members

        // Regular expressions used for parsing and fix-up
        private static Regex re1xExtractDefTopic = new Regex(
            "param name=\"Local\" value=\"(?<Filename>.*)\"",
            RegexOptions.IgnoreCase);

        private static Regex re2xExtractDefTopic = new Regex(
            "Url=\"(?<Filename>.*)\"", RegexOptions.IgnoreCase);

        private static Regex reInvalidChars = new Regex("[ :.`#<>*?]");
        #endregion

        /// <summary>
        /// This is called to determine the default topic for the help file
        /// and insert any additional table of contents entries for the
        /// additional content files.
        /// </summary>
        /// <param name="format">The format of the table of contents
        /// (HtmlHelp1x, HtmlHelp2x, or Website).</param>
        /// <remarks>In the absence of an additional content item with a
        /// default topic indicator, the default page is determined by
        /// extracting the first entry from the generated table of contents
        /// file.  If an additional content item with a default topic indicator
        /// has been specified, it will be the used instead.  The default
        /// topic is not used by HTML Help 2.x files.</remarks>
        /// <exception cref="ArgumentException">This is thrown if the
        /// format is not <b>HtmlHelp1x</b>, <b>HtmlHelp2x</b>, or
        /// <b>Website</b>.</exception>
        protected void UpdateTableOfContents(HelpFileFormat format)
        {
            XPathDocument config;
            XPathNavigator nav;
            CultureInfo ci;
            Encoding enc = Encoding.Default;
            Match m;

            string tocFile, content, guid, rootEntry;
            bool tocChanged = false, noContent = false;
            int codePage, pos;

            if(format != HelpFileFormat.HtmlHelp1x &&
              format != HelpFileFormat.HtmlHelp2x &&
              format != HelpFileFormat.Website)
                throw new ArgumentException("The format specified must be " +
                    "a single format, not a combination", "format");

            this.ReportProgress(BuildStep.UpdateTableOfContents,
                "Updating table of contents with additional " +
                "content items and determining default topic...");

            if(this.ExecutePlugIns(ExecutionBehaviors.InsteadOf))
                return;

            this.ExecutePlugIns(ExecutionBehaviors.Before);

            // HTML 1.x, HTML 2.x, or website?
            if(format == HelpFileFormat.HtmlHelp1x)
            {
                tocFile = workingFolder + project.HtmlHelpName + ".hhc";

                // For Help 1.x, we need to use an encoding based on the
                // selected language.  Get the code page to use based on the
                // locale ID from the SandcastleHtmlExtract.config file.
                config = new XPathDocument(Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location) +
                    @"\SandcastleHtmlExtract.config");

                nav = config.CreateNavigator().SelectSingleNode(String.Format(
                    CultureInfo.InvariantCulture,
                    "/configuration/languages/language[@id='{0}']",
                    language.LCID));

                // If not found, default to the one for the ANSI code page
                // based on the specified locale ID.
                if(nav == null)
                {
                    ci = new CultureInfo(language.LCID);
                    codePage = ci.TextInfo.ANSICodePage;

                    this.ReportWarning("BE0059", "LCID '{0}' not found in " +
                        "configuration file.  Defaulting to ANSI code page " +
                        "value of '{1}'.", language.LCID, codePage);
                }
                else
                    codePage = Convert.ToInt32(nav.GetAttribute("codepage",
                        String.Empty), CultureInfo.InvariantCulture);

                enc = Encoding.GetEncoding(codePage);

                using(StreamReader sr = new StreamReader(tocFile, enc))
                {
                    content = sr.ReadToEnd();
                }
            }
            else
            {
                if(format == HelpFileFormat.HtmlHelp2x)
                    tocFile = workingFolder + project.HtmlHelpName + ".hxt";
                else
                    tocFile = workingFolder + "WebTOC.xml";

                // When reading the file, use the default encoding but detect
                // the encoding if byte order marks are present.
                content = BuildProcess.ReadWithEncoding(tocFile, ref enc);
            }

            // We only need the default page for HTML Help 1.x files and
            // websites.  Don't bother if an explicit default has been
            // specified.
            if(format != HelpFileFormat.HtmlHelp2x && defaultTopic == null)
            {
                if(format == HelpFileFormat.HtmlHelp1x)
                    m = re1xExtractDefTopic.Match(content);
                else
                    m = re2xExtractDefTopic.Match(content);

                if(m.Success)
                    defaultTopic = m.Groups["Filename"].Value;
                else
                {
                    // If there are no reference topics, pick the first
                    // entry from the TOC.
                    if(toc != null && toc.Count != 0 &&
                      !String.IsNullOrEmpty(toc[0].DestinationFile))
                        defaultTopic = toc[0].DestinationFile;
                    else
                        throw new BuilderException("BE0026", "Unable to " +
                            "determine default topic in '" + tocFile +
                            "'.  You may need to mark one as the default " +
                            "topic manually.");
                }
            }

            if(format == HelpFileFormat.HtmlHelp1x)
            {
                // Add the root namespace container if one is wanted.  The
                // VS2005 and Hana styles do it automatically via the transform.
                if(project.RootNamespaceContainer &&
                  presentationParam == "prototype")
                {
                    if(namespacesTopic != null)
                        rootEntry = String.Format(CultureInfo.InvariantCulture,
                            "<param name=\"Name\" value=\"{0}\">\r\n" +
                            "<param name=\"Local\" value=\"{1}\">\r\n",
                            project.RootNamespaceTitle.Length == 0 ? "Namespaces" :
                                HttpUtility.HtmlEncode(project.RootNamespaceTitle),
                                namespacesTopic);
                    else
                        rootEntry = String.Format(CultureInfo.InvariantCulture,
                            "<param name=\"Name\" value=\"{0}\">\r\n",
                            project.RootNamespaceTitle.Length == 0 ? "Namespaces" :
                                HttpUtility.HtmlEncode(project.RootNamespaceTitle));

                    content = content.Insert(content.IndexOf("<BODY>",
                        StringComparison.Ordinal) + 8,
                        "<UL>\r\n<LI><OBJECT type=\"text/sitemap\">\r\n" +
                        rootEntry + "</OBJECT></LI>\r\n");
                    content = content.Insert(content.IndexOf("</BODY>",
                        StringComparison.Ordinal), "</UL>\r\n");
                }

                tocChanged = true;
            }
            else
            {
                // Add the root namespace container if one is wanted.  The
                // VS2005 and Hana styles do it automatically via the transform.
                if(project.RootNamespaceContainer &&
                  presentationParam == "prototype")
                {
                    tocChanged = true;
                    guid = Guid.NewGuid().ToString();

                    if(namespacesTopic != null)
                        rootEntry = String.Format(CultureInfo.InvariantCulture,
                            "<HelpTOCNode Id=\"{0}\" Title=\"{1}\" " +
                            "Url=\"{2}\">\r\n", guid,
                            project.RootNamespaceTitle.Length == 0 ? "Namespaces" :
                                HttpUtility.HtmlEncode(project.RootNamespaceTitle),
                            format != HelpFileFormat.Website ?  namespacesTopic :
                                namespacesTopic.Replace("\\", "/"));
                    else
                        rootEntry = String.Format(CultureInfo.InvariantCulture,
                            "<HelpTOCNode Id=\"{0}\" Title=\"{1}\">\r\n", guid,
                            project.RootNamespaceTitle.Length == 0 ? "Namespaces" :
                                HttpUtility.HtmlEncode(project.RootNamespaceTitle));

                    pos = content.IndexOf("<HelpTOCNode",
                        StringComparison.Ordinal);

                    if(pos == -1)
                    {
                        pos = content.IndexOf("/>", StringComparison.Ordinal);

                        if(pos != -1)
                        {
                            content = content.Replace("/>",
                                "></HelpTOC>");
                            pos++;
                        }
                        else
                            pos = content.IndexOf("</HelpTOC>",
                                StringComparison.Ordinal);
                    }

                    content = content.Insert(pos, rootEntry);
                    content = content.Insert(content.IndexOf("</HelpTOC>",
                        StringComparison.Ordinal), "</HelpTOCNode>\r\n");
                }
            }

            // Update and save table of contents with additional items
            if(tocChanged || (toc != null && toc.Count != 0))
            {
                // The additional entries can go in ahead of the namespace
                // documentation entries or after them.
                if(toc != null && toc.Count != 0)
                    if(format == HelpFileFormat.HtmlHelp1x)
                    {
                        if(tocAbove != null)
                        {
                            pos = content.IndexOf("<UL>",
                                StringComparison.Ordinal);

                            if(pos == -1)
                            {
                                noContent = true;
                                pos = content.IndexOf("<BODY>",
                                    StringComparison.Ordinal);

                                // We need to add the UL container too in
                                // this case.
                                content = content.Insert(pos + 6, "<UL></UL>");
                                pos += 4;
                            }

                            content = content.Insert(pos + 6, tocAbove.ToString());
                        }

                        if(tocBelow != null)
                        {
                            pos = content.LastIndexOf("</UL>",
                                StringComparison.Ordinal);

                            if(pos == -1 || noContent)
                                pos = content.IndexOf("</BODY>",
                                    StringComparison.Ordinal);

                            content = content.Insert(pos, tocBelow.ToString());
                        }
                    }
                    else
                    {
                        if(tocAbove != null)
                        {
                            pos = content.IndexOf("<HelpTOCNode",
                                StringComparison.Ordinal);

                            if(pos == -1)
                            {
                                pos = content.IndexOf("/>",
                                    StringComparison.Ordinal);

                                if(pos != -1)
                                {
                                    content = content.Replace("/>",
                                        "></HelpTOC>");
                                    pos++;
                                }
                                else
                                    pos = content.IndexOf("</HelpTOC>",
                                        StringComparison.Ordinal);
                            }

                            content = content.Insert(pos, tocAbove.ToString(format));
                        }

                        if(tocBelow != null)
                        {
                            pos = content.IndexOf("</HelpTOC>",
                                StringComparison.Ordinal);

                            if(pos == -1)
                            {
                                pos = content.IndexOf("/>",
                                    StringComparison.Ordinal);

                                if(pos != -1)
                                {
                                    content = content.Replace("/>",
                                        "></HelpTOC>");
                                    pos++;
                                }
                            }

                            content = content.Insert(pos, tocBelow.ToString(format));
                        }
                    }

                // Write the file back out with the appropriate encoding
                using(StreamWriter sw = new StreamWriter(tocFile, false, enc))
                {
                    sw.Write(content);
                }
            }

            this.ExecutePlugIns(ExecutionBehaviors.After);
        }

        /// <summary>
        /// This is called to create the help project output folder and copy
        /// the standard content files (art, media, scripts, and styles) to the
        /// help project folder.
        /// </summary>
        /// <remarks>This creates the folders <b>Output\</b> and
        /// <b>Output\html</b> under the working folder and copies the stock
        /// art, icon, media, script, and style sheet files from the
        /// <b>{@PresentationPath}\art</b>, <b>{@PresentationPath}\icons</b>,
        /// <b>{@PresentationPath}\media</b>,
        /// <b>{@PresentationPath}\scripts</b>, and
        /// <b>{@PresentationPath}\styles</b> folders which are located in the
        /// Sandcastle installation folder.  The art, icons, and media folders
        /// may or may not exist based on the style.</remarks>
        protected void CopyStandardHelpContent()
        {
            this.ReportProgress(BuildStep.CopyStandardContent,
                "Copying standard help content...");

            if(this.ExecutePlugIns(ExecutionBehaviors.InsteadOf))
                return;

            this.ExecutePlugIns(ExecutionBehaviors.Before);

            Directory.CreateDirectory(workingFolder + "Output");
            Directory.CreateDirectory(workingFolder + @"Output\html");

            if(Directory.Exists(presentationFolder + "art"))
                this.RecursiveCopy(presentationFolder + @"art\*.*",
                    workingFolder + @"Output\art\");

            if(Directory.Exists(presentationFolder + "icons"))
                this.RecursiveCopy(presentationFolder + @"icons\*.*",
                    workingFolder + @"Output\icons\");

            if(Directory.Exists(presentationFolder + "media"))
                this.RecursiveCopy(presentationFolder + @"media\*.*",
                    workingFolder + @"Output\media\");

            this.RecursiveCopy(presentationFolder + @"scripts\*.*",
                workingFolder + @"Output\scripts\");
            this.RecursiveCopy(presentationFolder + @"styles\*.*",
                workingFolder + @"Output\styles\");

            this.ExecutePlugIns(ExecutionBehaviors.After);
        }

        /// <summary>
        /// This copies files from the specified source folder to the specified
        /// destination folder.  If any subfolders are found below the source
        /// folder and the wildcard is "*.*", the subfolders are also copied
        /// recursively.
        /// </summary>
        /// <param name="sourcePath">The source path from which to copy</param>
        /// <param name="destPath">The destination path to which to copy</param>
        protected void RecursiveCopy(string sourcePath, string destPath)
        {
            if(sourcePath == null)
                throw new ArgumentNullException("sourcePath");

            if(destPath == null)
                throw new ArgumentNullException("destPath");

            int idx = sourcePath.LastIndexOf('\\');

            string dirName = sourcePath.Substring(0, idx),
                   fileSpec = sourcePath.Substring(idx + 1),
                   filename;

            string[] files = Directory.GetFiles(dirName, fileSpec);

            foreach(string name in files)
            {
                filename = destPath + Path.GetFileName(name);

                if(!Directory.Exists(destPath))
                    Directory.CreateDirectory(destPath);

                // All attributes are turned off so that we can delete
                // it later.
                File.Copy(name, filename, true);
                File.SetAttributes(filename, FileAttributes.Normal);

                this.ReportProgress("{0} -> {1}", name, filename);
            }

            // For "*.*", copy subfolders too
            if(fileSpec == "*.*")
            {
                string[] subFolders = Directory.GetDirectories(dirName);

                // Ignore hidden folders as they may be under source control
                // and are not wanted.
                foreach(string folder in subFolders)
                    if((File.GetAttributes(folder) & FileAttributes.Hidden) !=
                      FileAttributes.Hidden)
                        this.RecursiveCopy(folder + @"\*.*", destPath +
                            folder.Substring(dirName.Length + 1) + @"\");
            }
        }

        /// <summary>
        /// This returns a complete list of files for inclusion in the
        /// compiled help file.
        /// </summary>
        /// <param name="folder">The folder to expand</param>
        /// <param name="format">The HTML help file format</param>
        /// <returns>The full list of all files for the help project</returns>
        /// <remarks>The help file list is expanded to ensure that we get
        /// all additional content including all nested subfolders.  The
        /// <b>format</b> parameter determines the format of the returned
        /// file list.  For HTML 1.x, it returns a list of the filenames.
        /// For HTML 2.x, it returns the list formatted with the necessary
        /// XML markup.</remarks>
        protected string HelpProjectFileList(string folder,
          HelpFileFormat format)
        {
            StringBuilder sb = new StringBuilder(10240);
            string itemFormat, filename, checkName;
            bool encode;

            if(folder == null)
                throw new ArgumentNullException("folder");

            string[] files = Directory.GetFiles(folder, "*.*",
                SearchOption.AllDirectories);

            if(folder.Length != 0 && folder[folder.Length - 1] != '\\')
                folder += @"\";

            if((format & HelpFileFormat.HtmlHelp1x) != 0)
            {
                if(folder.IndexOf(',') != -1 || folder.IndexOf(".h",
                  StringComparison.OrdinalIgnoreCase) != -1)
                    this.ReportWarning("BE0060", "The file path '{0}' " +
                        "contains a comma or '.h' which may cause the Help 1 " +
                        "compiler to fail.", folder);

                if(project.HtmlHelpName.IndexOf(',') != -1 ||
                  project.HtmlHelpName.IndexOf(".h",
                  StringComparison.OrdinalIgnoreCase) != -1)
                    this.ReportWarning("BE0060", "The HTMLHelpName property " +
                        "value '{0}' contains a comma or '.h' which may " +
                        "cause the Help 1 compiler to fail.",
                        project.HtmlHelpName);

                itemFormat = "{0}\r\n";
                encode = false;
            }
            else
            {
                itemFormat = "	<File Url=\"{0}\" />\r\n";
                encode = true;
            }

            foreach(string name in files)
                if(!encode)
                {
                    filename = checkName = name.Replace(folder, String.Empty);

                    if(checkName.EndsWith(".htm", StringComparison.OrdinalIgnoreCase) ||
                      checkName.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
                        checkName = checkName.Substring(0,
                            checkName.LastIndexOf(".htm",
                            StringComparison.OrdinalIgnoreCase));

                    if(checkName.IndexOf(',') != -1 || checkName.IndexOf(".h",
                      StringComparison.OrdinalIgnoreCase) != -1)
                        this.ReportWarning("BE0060", "The filename '{0}' " +
                            "contains a comma or '.h' which may cause the " +
                            "Help 1 compiler to fail.", filename);

                    sb.AppendFormat(itemFormat, filename);
                }
                else
                    sb.AppendFormat(itemFormat,
                        HttpUtility.HtmlEncode(name.Replace(folder,
                            String.Empty)));

            return sb.ToString();
        }

        /// <summary>
        /// This is used to generate the website helper files and copy the
        /// output to the project output folder ready for use as a website.
        /// </summary>
        protected void GenerateWebsite()
        {
            string destFile;

            // Generate the full-text index for the ASP.NET search option
            this.ReportProgress(BuildStep.GenerateFullTextIndex,
                "Generating full-text index for the website...\r\n");

            if(!this.ExecutePlugIns(ExecutionBehaviors.InsteadOf))
            {
                this.ExecutePlugIns(ExecutionBehaviors.Before);

                FullTextIndex index = new FullTextIndex(workingFolder +
                    "StopWordList.txt", language);
                index.CreateFullTextIndex(workingFolder + "Output");
                index.SaveIndex(workingFolder + @"Output\fti\");

                this.ExecutePlugIns(ExecutionBehaviors.After);
            }

            this.ReportProgress(BuildStep.CopyingWebsiteFiles,
                "Copying website files to output folder...\r\n");

            if(this.ExecutePlugIns(ExecutionBehaviors.InsteadOf))
                return;

            this.ExecutePlugIns(ExecutionBehaviors.Before);

            // Copy the TOC, keyword index, index pages, and tree view stuff
            File.Copy(workingFolder + "WebTOC.xml", outputFolder + "WebTOC.xml");
            File.Copy(workingFolder + "WebKI.xml", outputFolder + "WebKI.xml");

            foreach(string file in Directory.GetFiles(webFolder))
                if(file.EndsWith("html", StringComparison.OrdinalIgnoreCase) ||
                  file.EndsWith("aspx", StringComparison.OrdinalIgnoreCase))
                    this.TransformTemplate(Path.GetFileName(file), webFolder,
                        outputFolder);
                else
                {
                    destFile = outputFolder + Path.GetFileName(file);
                    File.Copy(file, destFile, true);
                    File.SetAttributes(destFile, FileAttributes.Normal);
                }

            // Copy the help pages and related content
            this.RecursiveCopy(workingFolder + @"Output\*.*", outputFolder);

            this.GatherBuildOutputFilenames();
            this.ExecutePlugIns(ExecutionBehaviors.After);
        }

        /// <summary>
        /// This is called to generate the HTML table of contents when creating
        /// the website output.
        /// </summary>
        /// <returns>The HTML to insert for the table of contents.</returns>
        protected string GenerateHtmlToc()
        {
            XPathDocument toc;
            XPathNavigator navToc;
            XPathNodeIterator entries;
            Encoding enc = Encoding.Default;
            StringBuilder sb = new StringBuilder(2048);

            string content;

            // When reading the file, use the default encoding but detect the
            // encoding if byte order marks are present.
            content = BuildProcess.ReadWithEncoding(workingFolder +
                "WebTOC.xml", ref enc);

            using(StringReader sr = new StringReader(content))
            {
                toc = new XPathDocument(sr);
            }

            navToc = toc.CreateNavigator();

            // Get the TOC entries from the HelpTOC node
            entries = navToc.Select("HelpTOC/*");

            this.AppendTocEntry(entries, sb);

            return sb.ToString();
        }

        /// <summary>
        /// This is called to recursively append the child nodes to the
        /// HTML table of contents in the specified string builder.
        /// </summary>
        /// <param name="entries">The list over which to iterate
        /// recursively.</param>
        /// <param name="sb">The string builder to which the entries are
        /// appended.</param>
        private void AppendTocEntry(XPathNodeIterator entries, StringBuilder sb)
        {
            string url, target, title;

            foreach(XPathNavigator node in entries)
                if(node.HasChildren)
                {
                    url = node.GetAttribute("Url", String.Empty);
                    title = node.GetAttribute("Title", String.Empty);

                    if(!String.IsNullOrEmpty(url))
                        target = " target=\"TopicContent\"";
                    else
                    {
                        url = "#";
                        target = String.Empty;
                    }

                    sb.AppendFormat("<div class=\"TreeNode\">\r\n" +
                        "<img class=\"TreeNodeImg\" " +
                        "onclick=\"javascript: Toggle(this);\" " +
                        "src=\"Collapsed.gif\"/><a class=\"UnselectedNode\" " +
                        "onclick=\"javascript: return Expand(this);\" " +
                        "href=\"{0}\"{1}>{2}</a>\r\n" +
                        "<div class=\"Hidden\">\r\n", url, target,
                        HttpUtility.HtmlEncode(title));

                    // Append child nodes
                    this.AppendTocEntry(node.Select("*"), sb);

                    // Write out the closing tags for the root node
                    sb.Append("</div>\r\n</div>\r\n");
                }
                else
                {
                    title = node.GetAttribute("Title", String.Empty);
                    url = node.GetAttribute("Url", String.Empty);

                    if(String.IsNullOrEmpty(url))
                        url = "about:blank";

                    // Write out a TOC entry
                    sb.AppendFormat("<div class=\"TreeItem\">\r\n" +
                        "<img src=\"Item.gif\"/>" +
                        "<a class=\"UnselectedNode\" " +
                        "onclick=\"javascript: return SelectNode(this);\" " +
                        "href=\"{0}\" target=\"TopicContent\">{1}</a>\r\n" +
                        "</div>\r\n", url, HttpUtility.HtmlEncode(title));
                }
        }

        /// <summary>
        /// This is used to change the filenames assigned to each member
        /// in the reflection information file.
        /// </summary>
        protected void ModifyHelpTopicFilenames()
        {
            XmlNodeList elements;
            string originalName, memberName, newName;
            bool duplicate, overloadBugWorkaround;
            int idx;

            this.ReportProgress(BuildStep.ModifyHelpTopicFilenames,
                "Modifying help topic filenames in reflection information file");

            if(this.ExecutePlugIns(ExecutionBehaviors.InsteadOf))
                return;

            // It's possible a plug-in might want to change the naming method
            // so run them first.
            this.ExecutePlugIns(ExecutionBehaviors.Before);

            if(project.NamingMethod == NamingMethod.Guid)
            {
                this.ReportProgress("    No changes required");

                // We still run the After behavior as some plug-ins may still
                // need to do something.
                this.ExecutePlugIns(ExecutionBehaviors.After);
                return;
            }

            // The reflection file can contain tens of thousands of entries
            // for large assemblies.  Dictionary<K, T> is much faster at
            // lookups than List<T>.
            Dictionary<string, string> filenames = new Dictionary<string, string>();

            try
            {
                // Find the API node list
                elements = apisNode.SelectNodes("api/file");

                foreach(XmlNode file in elements)
                {
                    originalName = memberName = file.ParentNode.Attributes[
                        "id"].Value;

                    // Remove parameters
                    idx = memberName.IndexOf('(');
                    if(idx != -1)
                        memberName = memberName.Substring(0, idx);

                    // Replace invalid filename characters with an underscore
                    // if member names are used as the filenames.
                    if(project.NamingMethod == NamingMethod.MemberName)
                        newName = memberName = reInvalidChars.Replace(
                            memberName, "_");
                    else
                        newName = memberName;

                    idx = 0;
                    overloadBugWorkaround = false;

                    do
                    {
                        // Hash codes can be used to shorten extremely long
                        // type and member names.
                        if(project.NamingMethod == NamingMethod.HashedMemberName)
                            newName = String.Format(
                                CultureInfo.InvariantCulture, "{0:X}",
                                newName.GetHashCode());

                        // Check for a duplicate (i.e. an overloaded member).
                        // These will be made unique by adding a counter to
                        // the end of the name.
                        duplicate = filenames.ContainsKey(newName);

                        // VS2005/Hana style bug as of Sept 2007 CTP.
                        // Overloads pages sometimes result in a duplicate
                        // reflection file entry but we need to ignore it.
                        if(duplicate && originalName.StartsWith("Overload:",
                          StringComparison.Ordinal))
                        {
                            duplicate = false;
                            overloadBugWorkaround = true;
                        }

                        if(duplicate)
                        {
                            idx++;
                            newName = String.Format(
                                CultureInfo.InvariantCulture, "{0}_{1}",
                                memberName, idx);
                        }

                    } while(duplicate);

                    // Log duplicates that had unique names created
                    if(idx != 0)
                        this.ReportProgress("    Unique name {0} generated " +
                            "for {1}", newName, originalName);

                    file.Attributes["name"].Value = newName;

                    if(!overloadBugWorkaround)
                        filenames.Add(newName, null);
                }
            }
            catch(Exception ex)
            {
                throw new BuilderException("BE0027", "Error modifying help " +
                    "topic filenames: " + ex.Message, ex);
            }

            this.ExecutePlugIns(ExecutionBehaviors.After);
        }
    }
}
