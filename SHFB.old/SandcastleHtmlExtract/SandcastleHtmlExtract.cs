//=============================================================================
// System  : Sandcastle Help File Builder - HTML Extract
// File    : SandcastleHtmlExtract.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 07/15/2008
// Note    : Copyright 2008, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the console mode application used to extract title and
// keyword information from HTML files for use in creating the CHM table of
// contents and keyword index files.  It can also optionally convert the files
// to a different encoding in order to build Help 1.x (CHM) files that use a
// different language.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://www.CodePlex.com/SHFB.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.6.0.5  02/02/2008  EFW  Created the code
// 1.6.0.7  04/01/2008  EFW  Merged changes from Ferdinand Prantl to add a
//                           website keyword index.
// 1.7.0.0  06/14/2008  EFW  Fixed bug in handling of TOC nodes without a file
// 1.8.0.0  07/14/2008  EFW  Added support for running as an MSBuild task
//=============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.XPath;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace SandcastleBuilder.HtmlExtract
{
    /// <summary>
    /// This is the console mode application used to extract title and keyword
    /// information from HTML files for use in creating the CHM table of
    /// contents and keyword index files.
    /// </summary>
    public class SandcastleHtmlExtract : Task
    {
        #region MSBuild task interface
        //=====================================================================

        /// <summary>
        /// This is used to set the project name
        /// </summary>
        [Required]
        public string ProjectName
        {
            get { return projectName; }
            set
            {
                if(!String.IsNullOrEmpty(value))
                    projectName = value;
            }
        }

        /// <summary>
        /// This is used to set the language ID (LCID)
        /// </summary>
        /// <value>This is optional.  If not set, it defaults to 1033.</value>
        public int LanguageId
        {
            get { return langId; }
            set { langId = value; }
        }

        /// <summary>
        /// This is used to specify whether the extract should generate files
        /// for a Help 1 build.
        /// </summary>
        /// <value>This property and/or <see cref="GenerateWebFiles" /> should
        /// be set to true.</value>
        public bool GenerateHelp1Files
        {
            get { return generateHelp1Files; }
            set { generateHelp1Files = value; }
        }

        /// <summary>
        /// This is used to specify whether the extract should generate files
        /// for a website build.
        /// </summary>
        /// <value>This property and/or <see cref="GenerateHelp1Files" />
        /// should be set to true.</value>
        public bool GenerateWebFiles
        {
            get { return generateWebFiles; }
            set { generateWebFiles = value; }
        }

        /// <summary>
        /// This is used to set the HTML output folder name
        /// </summary>
        /// <value>This is optional.  If not set, it defaults to
        /// <b>Output</b>.</value>
        public string HtmlFolder
        {
            get { return htmlFolder; }
            set
            {
                if(!String.IsNullOrEmpty(value))
                    htmlFolder = value;
            }
        }

        /// <summary>
        /// This is used to set the localized output folder name
        /// </summary>
        /// <value>This is optional.  If not set, the HTML files will not
        /// be localized.</value>
        public string LocalizedFolder
        {
            get { return localizedFolder; }
            set
            {
                if(!String.IsNullOrEmpty(value))
                    localizedFolder = value;
            }
        }

        /// <summary>
        /// This is used to set the general output folder name
        /// </summary>
        /// <value>This is optional.  If not set, it defaults to the current
        /// working folder.</value>
        public string OutputFolder
        {
            get { return outputFolder; }
            set
            {
                if(!String.IsNullOrEmpty(value))
                    outputFolder = value;
            }
        }

        /// <summary>
        /// This is used to set the table of contents XML filename
        /// </summary>
        /// <value>This is optional.  If not set, it defaults to
        /// <b>toc.xml</b>.</value>
        public string TocFile
        {
            get { return tocFile; }
            set
            {
                if(!String.IsNullOrEmpty(value))
                    tocFile = value;
            }
        }

        /// <summary>
        /// This is used to execute the task and generate the inherited
        /// documentation.
        /// </summary>
        /// <returns>True on success or false on failure.</returns>
        public override bool Execute()
        {
            isMSBuildTask = true;
            return (Main(new string[0]) == 0);
        }
        #endregion

        #region Title information structure
        //=====================================================================
        // Title information structure

        /// <summary>
        /// This is used to hold title information
        /// </summary>
        private struct TitleInfo
        {
            /// <summary>The title</summary>
            public string Title;
            /// <summary>The file in which it occurs</summary>
            public string File;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="topicTitle">The title</param>
            /// <param name="filename">The filename</param>
            public TitleInfo(string topicTitle, string filename)
            {
                Title = topicTitle;
                File = filename;
            }
        }
        #endregion

        #region Keyword information structure
        //=====================================================================
        // Keyword information structure

        /// <summary>
        /// This is used to hold keyword information
        /// </summary>
        private struct KeywordInfo
        {
            /// <summary>The main entry</summary>
            public string MainEntry;
            /// <summary>An optional sub-entry</summary>
            public string SubEntry;
            /// <summary>The file in which it occurs</summary>
            public string File;
        }
        #endregion

        #region Private data members
        //=====================================================================
        // Private data members

        // Options
        private static string htmlFolder, outputFolder, projectName, tocFile,
            localizedFolder, encodingName;
        private static int langId, codePage;
        private static bool isMSBuildTask, generateHelp1Files, generateWebFiles;

        // Extracted keyword and title information
        private static List<KeywordInfo> keywords;
        private static Dictionary<string, TitleInfo> titles;

        // Regular expressions used for title and keyword extraction
        private static Regex reTitle = new Regex(@"<title>(.*)</title>",
            RegexOptions.IgnoreCase);
        private static Regex reTocTitle = new Regex("<mshelp:toctitle" +
            "\\s+title=\"([^\"]+)\"[^>]+>", RegexOptions.IgnoreCase);
        private static Regex reKKeyword = new Regex("<mshelp:keyword\\s+" +
            "index=\"k\"\\s+term=\"([^\"]+)\"[^>]+>", RegexOptions.IgnoreCase);
        private static Regex reSubEntry = new Regex(@",([^\)\>]+|([^\<\>]*" +
            @"\<[^\<\>]*\>[^\<\>]*)?|([^\(\)]*\([^\(\)]*\)[^\(\)]*)?)$");

        // Localization support members
        private static Dictionary<Regex, string> patterns;
        private static Encoding destEncoding;
        #endregion

        #region Main program entry point
        //=====================================================================
        // Main program entry point

        /// <summary>
        /// Main program entry point
        /// </summary>
        /// <param name="args">The command line arguments</param>
        /// <returns>Zero on success or a non-zero value on failure</returns>
        public static int Main(string[] args)
        {
            XPathDocument config;
            XPathNavigator nav;
            CultureInfo ci;
            Encoding enc;
            KeywordInfo kw;
            OptionInfo lastOption = null;
            List<OptionInfo> options = new List<OptionInfo>();
            string[] fileList;
            string ext, folder, destFile, mainEntry = String.Empty;
            int htmlFiles = 0, returnCode = 0;

            if(String.IsNullOrEmpty(htmlFolder))
                htmlFolder = @"Output";

            if(String.IsNullOrEmpty(outputFolder))
                outputFolder = ".";

            if(String.IsNullOrEmpty(tocFile))
                tocFile = "toc.xml";

            if(langId == 0)
                langId = 1033;

            keywords = new List<KeywordInfo>();
            titles = new Dictionary<string, TitleInfo>();

            Assembly asm = Assembly.GetExecutingAssembly();

            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
            Console.WriteLine("{0}, version {1}\r\n{2}\r\nE-Mail: " +
                "Eric@EWoodruff.us\r\n", fvi.ProductName, fvi.ProductVersion,
                fvi.LegalCopyright);

            try
            {
                // Get a list of options
                foreach(string option in args)
                    options.Add(new OptionInfo(option));

                if(options.Count == 0 && !isMSBuildTask)
                {
                    ShowHelp();
                    return 1;
                }

                // Get the settings for this run
                foreach(OptionInfo option in options)
                {
                    lastOption = option;

                    switch(option.Name)
                    {
                        case "help":
                        case "?":
                            ShowHelp();
                            break;

                        case "help1x":
                            generateHelp1Files = true;
                            break;

                        case "web":
                            generateWebFiles = true;
                            break;

                        case "html":
                            htmlFolder = option.Value;

                            if(!Directory.Exists(htmlFolder))
                                throw new ArgumentException(
                                    "The HTML folder could not be found");
                            break;

                        case "lcid":
                            if(!Int32.TryParse(option.Value, out langId))
                                throw new ArgumentException(
                                    "Invalid LCID value");
                            break;

                        case "localize":
                            localizedFolder = option.Value;
                            break;

                        case "out":
                            outputFolder = option.Value;
                            break;

                        case "project":
                            projectName = option.Value;
                            break;

                        case "toc":
                            tocFile = option.Value;
                            break;

                        default:
                            throw new ArgumentException(
                                "Unknown command line option");
                    }
                }

                lastOption = null;

                // -help1x or -web must be specified
                if(!generateHelp1Files && !generateWebFiles)
                    throw new InvalidOperationException(isMSBuildTask ?
                        "The GenerateHelp1Files and/or GenerateWebFiles property must be set to true" :
                        "-help1x and/or -web must be specified");

                // The project filename must be specified
                if(String.IsNullOrEmpty(projectName))
                    throw new InvalidOperationException(isMSBuildTask ?
                        "A project name must be specified using the ProjectName property" :
                        "A project name must be specified using the /project command line option");

                // The TOC file must exist
                if(!File.Exists(tocFile))
                    throw new ArgumentException("TOC file not found");

                htmlFolder = Path.GetFullPath(htmlFolder);
                if(htmlFolder.Length != 0 && htmlFolder[htmlFolder.Length - 1] == '\\')
                    htmlFolder = htmlFolder.Substring(0, htmlFolder.Length - 1);

                outputFolder = Path.GetFullPath(outputFolder);
                if(outputFolder.Length != 0 && outputFolder[outputFolder.Length - 1] == '\\')
                    outputFolder = outputFolder.Substring(0,
                        outputFolder.Length - 1);

                if(!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);

                // Get the code page to use based on the locale ID
                config = new XPathDocument(Path.GetDirectoryName(asm.Location) +
                    @"\SandcastleHtmlExtract.config");

                nav = config.CreateNavigator().SelectSingleNode(String.Format(
                    CultureInfo.InvariantCulture,
                    "/configuration/languages/language[@id='{0}']", langId));

                // If not found, default to the one for the ANSI code page
                // based on the specified locale ID.
                if(nav == null)
                {
                    ci = new CultureInfo(langId);
                    codePage = ci.TextInfo.ANSICodePage;
                    enc = Encoding.GetEncoding(codePage);
                    encodingName = enc.WebName;

                    Console.WriteLine("SHFB: Warning SHE0001: LCID '{0}' not " +
                        "found in configuration file.  Defaulting to ANSI " +
                        "code page value of '{1}', encoding charset '{2}'.",
                        langId, codePage, encodingName);
                }
                else
                {
                    codePage = Convert.ToInt32(nav.GetAttribute("codepage",
                        String.Empty), CultureInfo.InvariantCulture);
                    encodingName = nav.GetAttribute("charset", String.Empty);

                    Console.WriteLine("Using LCID '{0}', code page '{1}', " +
                        "encoding charset '{2}'.", langId, codePage,
                        encodingName);
                }

                // If localizing, validate the folder and create the regex
                // patterns that will do the conversions.
                if(localizedFolder != null)
                {
                    localizedFolder = Path.GetFullPath(localizedFolder);

                    if(localizedFolder.Length > 0 &&
                      localizedFolder[localizedFolder.Length - 1] == '\\')
                        localizedFolder = localizedFolder.Substring(0,
                            localizedFolder.Length - 1);

                    if(!Directory.Exists(localizedFolder))
                        Directory.CreateDirectory(localizedFolder);

                    patterns = new Dictionary<Regex, string>();
                    destEncoding = Encoding.GetEncoding(encodingName);

                    // Convert unsupported high-order characters to 7-bit ASCII
                    // equivalents.
                    patterns.Add(new Regex(@"\u2018|\u2019"), "'");
                    patterns.Add(new Regex(@"\u201C|\u201D"), "\"");
                    patterns.Add(new Regex(@"\u2026"), "...");

                    if(langId != 1041)
                        patterns.Add(new Regex(@"\u00A0"), "&nbsp;");
                    else
                        patterns.Add(new Regex(@"\u00A0"), " ");

                    if(encodingName != "Windows-1252")
                        patterns.Add(new Regex(@"\u2011|\u2013"), "-");
                    else
                        patterns.Add(new Regex(@"\u2011|\u2013|\u2014"), "-");

                    // Convert other unsupported high-order characters to named
                    // entities.
                    patterns.Add(new Regex(@"\u00A9"), "&copy;");
                    patterns.Add(new Regex(@"\u00AE"), "&reg;");
                    patterns.Add(new Regex(@"\u2014"), "&mdash;");
                    patterns.Add(new Regex(@"\u2122"), "&trade;");

                    // Replace the charset declaration
                    patterns.Add(new Regex("CHARSET=UTF-8",
                        RegexOptions.IgnoreCase), "CHARSET=" + encodingName);

                    Console.WriteLine("Localized content will be written " +
                        "to '{0}'", localizedFolder);
                }

                // Process all *.htm and *.html files in the given folder and
                // all of its subfolders.
                fileList = Directory.GetFiles(htmlFolder, "*.*",
                  SearchOption.AllDirectories);

                foreach(string file in fileList)
                {
                    ext = Path.GetExtension(file).ToLower(
                        CultureInfo.InvariantCulture);

                    if(ext == ".htm" || ext == ".html")
                    {
                        ProcessFile(file);
                        htmlFiles++;
                    }
                    else
                        if(localizedFolder != null)
                        {
                            // Copy supporting files only if localizing
                            destFile = Path.Combine(localizedFolder,
                                file.Substring(htmlFolder.Length + 1));
                            folder = Path.GetDirectoryName(destFile);

                            if(!Directory.Exists(folder))
                                Directory.CreateDirectory(folder);

                            File.Copy(file, destFile, true);
                        }
                }

                Console.WriteLine("Processed {0} HTML files\r\n" +
                    "Sorting keywords and generating See Also indices",
                    htmlFiles);

                // Sort the keywords
                keywords.Sort(delegate(KeywordInfo x, KeywordInfo y)
                {
                    string subX, subY;

                    if(x.MainEntry != y.MainEntry)
                        return String.Compare(x.MainEntry, y.MainEntry,
                            StringComparison.OrdinalIgnoreCase);

                    subX = x.SubEntry;
                    subY = y.SubEntry;

                    if(subX == null)
                        subX = String.Empty;

                    if(subY == null)
                        subY = String.Empty;

                    if(subX != subY)
                        return String.Compare(subX, subY,
                            StringComparison.OrdinalIgnoreCase);

                    return String.Compare(x.File, y.File,
                        StringComparison.OrdinalIgnoreCase);
                });

                // Insert the See Also indices for each sub-entry
                for(int idx = 0; idx < keywords.Count; idx++)
                    if(!String.IsNullOrEmpty(keywords[idx].SubEntry))
                    {
                        if(idx > 0)
                            mainEntry = keywords[idx - 1].MainEntry;

                        if(mainEntry != keywords[idx].MainEntry)
                        {
                            kw = new KeywordInfo();
                            kw.MainEntry = keywords[idx].MainEntry;
                            keywords.Insert(idx, kw);
                        }
                    }

                // Save the Help 1.x keyword index and table of contents
                if(generateHelp1Files)
                {
                    WriteHelp1xKeywordIndex();
                    WriteHelp1xTableOfContents();
                }

                // Save the website keyword index and table of contents
                if(generateWebFiles)
                {
                    WriteWebsiteKeywordIndex();
                    WriteWebsiteTableOfContents();
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                if(lastOption == null)
                    Console.WriteLine("SHFB: Error SHE0002: Unexpected " +
                        "error: {0}", ex);
                else
                    Console.WriteLine("SHFB: Error SHE0003: Unexpected error " +
                        "applying command line option '{0}': {1}",
                        lastOption.OptionText, ex);

                returnCode = 1;
            }
#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();
            }
#endif
            return returnCode;
        }
        #endregion

        #region Show command line help
        //=====================================================================
        // Show command line help

        /// <summary>
        /// This is used to show the command line syntax
        /// </summary>
        private static void ShowHelp()
        {
            Console.WriteLine(@"Syntax:
SandcastleHtmlExtract -project=ProjectName [-OtherOptions] ...

Prefix options with '-' or '/'.  Names and values are case-insensitive.
Property values should be enclosed in double quotes if they contain spaces,
commas, or other special characters.

-help or -?         Show this help.

-project=name       Specify the root project name for the CHM table of contents
                    and keyword index files.  This option is required.

-help1x             Generate HTML Help 1.x files and/or web site files.  At
-web                least one of these is required.  Both can also be
                    specified.

-html=folder        The folder containing the HTML files to parse.  If not
                    specified, it defaults to Output\.

-lcid=IDvalue       The locale ID to use when saving the table of contents
                    and keyword index files.  This is also used to determine
                    the code page to use when rewriting the HTML files if
                    needed.  If not specified, it defaults to 1033 (0x0409).

-localize=folder    The folder in which to store localized copies of the HTML
                    files.  These can be used to build a correctly localized
                    CHM file.  If omitted, no localized files are generated.

-out=folder         The output folder for the table of contents and keyword
                    index files.  If not specified, it defaults to the current
                    folder.

-toc=TOCfile        The intermediate table of contents XML file to use for
                    creating the CHM table of contents file.  If not specified,
                    it defaults to toc.xml.");
        }
        #endregion

        #region File parsing method
        //=====================================================================
        // File parsing method

        /// <summary>
        /// Parse each file looking for the title and index keywords
        /// </summary>
        /// <param name="sourceFile">The file to parse</param>
        private static void ProcessFile(string sourceFile)
        {
            Encoding currentEncoding = Encoding.Default;
            MatchCollection matches;
            Match match;
            KeywordInfo keyword;
            string content, title, term, folder, key;
            byte[] currentBytes, convertedBytes;

            // Read the file in using the proper encoding
            using(StreamReader sr = new StreamReader(sourceFile,
              currentEncoding, true))
            {
                content = sr.ReadToEnd();
                currentEncoding = sr.CurrentEncoding;
            }

            title = String.Empty;

            // Extract the title
            match = reTitle.Match(content);

            if(match.Success)
                title = match.Groups[1].Value;

            // If a TOC title entry is present, use that instead
            match = reTocTitle.Match(content);

            if(match.Success)
                title = match.Groups[1].Value;

            key = Path.GetFileNameWithoutExtension(sourceFile);

            if(titles.ContainsKey(key))
                Console.WriteLine("SHFB: Warning SHE0004: The key '{0}' used " +
                    "for '{1}' is already in use by '{2}'.  '{1}' will be " +
                    "ignored.", key, sourceFile, titles[key].File);
            else
                titles.Add(key, new TitleInfo(HttpUtility.HtmlDecode(title),
                    sourceFile));

            // Extract K index keywords
            matches = reKKeyword.Matches(content);

            foreach(Match m in matches)
            {
                keyword = new KeywordInfo();
                term = m.Groups[1].Value;

                if(!String.IsNullOrEmpty(term))
                {
                    term = HttpUtility.HtmlDecode(term.Replace(
                        "%3C", "<").Replace("%3E", ">").Replace("%2C", ","));

                    // See if there is a sub-entry
                    match = reSubEntry.Match(term);

                    if(match.Success)
                    {
                        keyword.MainEntry = term.Substring(0, match.Index);
                        keyword.SubEntry = term.Substring(
                            match.Index + 1).TrimStart(new char[] { ' ' });
                    }
                    else
                        keyword.MainEntry = term;

                    keyword.File = sourceFile;
                    keywords.Add(keyword);
                }
            }

            // If localizing, perform the substitutions, convert the encoding,
            // and save the file to the localized folder.
            if(localizedFolder != null)
            {
                foreach(KeyValuePair<Regex, string> pair in patterns)
                    content = pair.Key.Replace(content, pair.Value);

                currentBytes = currentEncoding.GetBytes(content);
                convertedBytes = Encoding.Convert(currentEncoding, destEncoding,
                    currentBytes);

                sourceFile = Path.Combine(localizedFolder,
                    sourceFile.Substring(htmlFolder.Length + 1));
                folder = Path.GetDirectoryName(sourceFile);

                if(!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                using(StreamWriter writer = new StreamWriter(sourceFile, false,
                  destEncoding))
                {
                    writer.Write(destEncoding.GetString(convertedBytes));
                }
            }
        }
        #endregion

        #region Write Help 1.x table of contents
        //=====================================================================
        // Write Help 1.x table of contents

        /// <summary>
        /// Write out the Help 1.x table of contents
        /// </summary>
        private static void WriteHelp1xTableOfContents()
        {
            XmlReaderSettings settings;
            XmlReader reader;
            TitleInfo titleInfo;
            string key, title, htmlFile;
            int indentCount, baseFolderLength = htmlFolder.Length + 1;

            Console.WriteLine(@"Saving Help 1.x table of contents to {0}\{1}.hhc",
                outputFolder, projectName);

            settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            reader = XmlReader.Create(tocFile, settings);

            // Write the table of contents using the appropriate encoding
            using(StreamWriter writer = new StreamWriter(String.Format(
              CultureInfo.InvariantCulture, @"{0}\{1}.hhc", outputFolder,
              projectName), false, Encoding.GetEncoding(codePage)))
            {
                writer.WriteLine("<!DOCTYPE HTML PUBLIC \"-//IETF//DTD " +
                    "HTML/EN\">\r\n");
                writer.WriteLine("<HTML>");
                writer.WriteLine("  <BODY>");

                while(reader.Read())
                    switch(reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if(reader.Name == "topic")
                            {
                                key = reader.GetAttribute("file");

                                if(key != null && titles.ContainsKey(key))
                                {
                                    titleInfo = titles[key];
                                    title = titleInfo.Title;
                                    htmlFile = titleInfo.File.Substring(
                                        baseFolderLength);
                                }
                                else
                                {
                                    // Container only topic or unknown element,
                                    // just use the ID attribute.
                                    htmlFile = null;
                                    title = reader.GetAttribute("id");

                                    if(String.IsNullOrEmpty(title))
                                        title = key;
                                }

                                indentCount = reader.Depth;
                                title = HttpUtility.HtmlEncode(title);

                                WriteContentLine(writer, indentCount, "<UL>");
                                WriteContentLine(writer, indentCount,
                                    "  <LI><OBJECT type=\"text/sitemap\">");
                                WriteContentLine(writer, indentCount,
                                    String.Format(CultureInfo.InvariantCulture,
                                    "    <param name=\"Name\" value=\"{0}\">",
                                    title));

                                if(htmlFile != null)
                                    WriteContentLine(writer, indentCount,
                                        String.Format(
                                        CultureInfo.InvariantCulture,
                                        "    <param name=\"Local\" " +
                                        "value=\"{0}\">", htmlFile));

                                WriteContentLine(writer, indentCount,
                                    "  </OBJECT></LI>");

                                if(reader.IsEmptyElement)
                                    WriteContentLine(writer, indentCount,
                                        "</UL>");
                            }
                            break;

                        case XmlNodeType.EndElement:
                            if(reader.Name == "topic")
                                WriteContentLine(writer, reader.Depth, "</UL>");
                            break;

                        default:
                            break;
                    }

                writer.WriteLine();
                writer.WriteLine("  </BODY>");
                writer.WriteLine("</HTML>");
            }
        }

        /// <summary>
        /// Write out a table of contents line with indentation
        /// </summary>
        /// <param name="writer">The writer to which the line is saved</param>
        /// <param name="indentCount">The amount of indent to use</param>
        /// <param name="value">The value to write</param>
        private static void WriteContentLine(TextWriter writer,
          int indentCount, string value)
        {
            writer.WriteLine();

            for(int idx = 0; idx < indentCount; idx++)
                writer.Write("  ");

            writer.Write(value);
        }
        #endregion

        #region Write out the Help 1.x keyword index
        //=====================================================================
        // Write out the Help 1.x keyword index

        /// <summary>
        /// Write out the Help 1.x keyword index
        /// </summary>
        private static void WriteHelp1xKeywordIndex()
        {
            string mainEntry;
            int baseFolderLength = htmlFolder.Length + 1;
            bool inSubEntry = false;

            Console.WriteLine(@"Saving Help 1.x keyword index to {0}\{1}.hhk",
                outputFolder, projectName);

            // Write the keyword index using the appropriate encoding
            using(StreamWriter writer = new StreamWriter(
                String.Format(CultureInfo.InvariantCulture, @"{0}\{1}.hhk",
                outputFolder, projectName), false,
                Encoding.GetEncoding(codePage)))
            {
                writer.WriteLine("<!DOCTYPE HTML PUBLIC \"-//IETF//DTD " +
                    "HTML/EN\">");
                writer.WriteLine("<HTML>");
                writer.WriteLine("  <BODY>");
                writer.WriteLine("    <UL>");

                foreach(KeywordInfo info in keywords)
                {
                    if(String.IsNullOrEmpty(info.MainEntry))
                        continue;

                    mainEntry = info.MainEntry;

                    if(!String.IsNullOrEmpty(info.SubEntry))
                    {
                        if(!inSubEntry)
                        {
                            inSubEntry = true;
                            writer.WriteLine("    <UL>");
                        }

                        mainEntry = info.SubEntry;
                    }
                    else
                        if(inSubEntry)
                        {
                            inSubEntry = false;
                            writer.WriteLine("    </UL>");
                        }

                    mainEntry = HttpUtility.HtmlEncode(mainEntry);

                    writer.WriteLine("      <LI><OBJECT type=\"text/sitemap\">");
                    writer.WriteLine(String.Format(CultureInfo.InvariantCulture,
                        "        <param name=\"Name\" value=\"{0}\">",
                        mainEntry));

                    if(String.IsNullOrEmpty(info.File))
                        writer.WriteLine(String.Format(
                            CultureInfo.InvariantCulture, "        <param " +
                            "name=\"See Also\" value=\"{0}\">", mainEntry));
                    else
                        writer.WriteLine(String.Format(
                            CultureInfo.InvariantCulture, "        <param " +
                            "name=\"Local\" value=\"{0}\">",
                            info.File.Substring(baseFolderLength)));

                    writer.WriteLine("      </OBJECT><LI>");
                }

                writer.WriteLine("    </UL>");
                writer.WriteLine("  </BODY>");
                writer.WriteLine("</HTML>");
            }
        }
        #endregion

        #region Write website table of contents
        //=====================================================================
        // Write website table of contents

        /// <summary>
        /// Write out the website table of contents
        /// </summary>
        private static void WriteWebsiteTableOfContents()
        {
            XmlReaderSettings settings;
            XmlReader reader;
            TitleInfo titleInfo;
            string key, title, htmlFile;
            int indentCount, baseFolderLength = htmlFolder.Length + 1;

            Console.WriteLine(@"Saving website table of contents to {0}\WebTOC.xml",
                outputFolder);

            settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            reader = XmlReader.Create(tocFile, settings);

            // Write the table of contents with UTF-8 encoding
            using(StreamWriter writer = new StreamWriter(String.Format(
              CultureInfo.InvariantCulture, @"{0}\WebTOC.xml", outputFolder),
              false, Encoding.UTF8))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                writer.WriteLine("<HelpTOC>");

                while(reader.Read())
                    switch(reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if(reader.Name == "topic")
                            {
                                key = reader.GetAttribute("file");

                                if(!String.IsNullOrEmpty(key) &&
                                  titles.ContainsKey(key))
                                {
                                    titleInfo = titles[key];
                                    title = titleInfo.Title;
                                    htmlFile = titleInfo.File.Substring(
                                        baseFolderLength).Replace("\\", "/");
                                }
                                else
                                {
                                    // Container only topic or unknown element,
                                    // just use the title attribute if present.
                                    // If no title, use the ID.
                                    htmlFile = null;
                                    title = reader.GetAttribute("title");

                                    if(String.IsNullOrEmpty(title))
                                        title = reader.GetAttribute("id");
                                }

                                indentCount = reader.Depth;
                                title = HttpUtility.HtmlEncode(title);

                                if(reader.IsEmptyElement)
                                    WriteContentLine(writer, indentCount,
                                        String.Format(
                                        CultureInfo.InvariantCulture,
                                        "<HelpTOCNode Title=\"{0}\" " +
                                        "Url=\"{1}\" />", title, htmlFile));
                                else
                                    if(htmlFile != null)
                                        WriteContentLine(writer, indentCount,
                                            String.Format(
                                            CultureInfo.InvariantCulture,
                                            "<HelpTOCNode Id=\"{0}\" " +
                                            "Title=\"{1}\" Url=\"{2}\">",
                                            Guid.NewGuid(), title, htmlFile));
                                    else
                                        WriteContentLine(writer, indentCount,
                                            String.Format(
                                            CultureInfo.InvariantCulture,
                                            "<HelpTOCNode Id=\"{0}\" " +
                                            "Title=\"{1}\">", Guid.NewGuid(),
                                            title));
                            }
                            break;

                        case XmlNodeType.EndElement:
                            if(reader.Name == "topic")
                                WriteContentLine(writer, reader.Depth,
                                    "</HelpTOCNode>");
                            break;

                        default:
                            break;
                    }

                writer.WriteLine();
                writer.WriteLine("</HelpTOC>");
            }
        }
        #endregion

        #region Write out the website keyword index
        //=====================================================================
        // Write out the website keyword index

        /// <summary>
        /// Write out the website keyword index
        /// </summary>
        private static void WriteWebsiteKeywordIndex()
        {
            string mainEntry;
            int indentCount = 1, baseFolderLength = htmlFolder.Length + 1;
            bool inSubEntry = false;

            Console.WriteLine(@"Saving website keyword index to {0}\WebKI.xml",
                outputFolder);

            // Write the keyword index with UTF-8 encoding
            using(StreamWriter writer = new StreamWriter(String.Format(
              CultureInfo.InvariantCulture, @"{0}\WebKI.xml",
              outputFolder), false, Encoding.UTF8))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                writer.WriteLine("<HelpKI>");

                foreach(KeywordInfo info in keywords)
                {
                    if(String.IsNullOrEmpty(info.MainEntry))
                        continue;

                    mainEntry = info.MainEntry;

                    if(!String.IsNullOrEmpty(info.SubEntry))
                    {
                        if(!inSubEntry)
                        {
                            inSubEntry = true;
                            WriteContentLine(writer, indentCount++,
                                String.Format(CultureInfo.InvariantCulture,
                                 "<HelpKINode Title=\"{0}\">",
                                 HttpUtility.HtmlEncode(mainEntry)));
                        }

                        mainEntry = info.SubEntry;
                    }
                    else
                        if(inSubEntry)
                        {
                            inSubEntry = false;
                            WriteContentLine(writer, --indentCount,
                                "</HelpKINode>");
                        }

                    if(!String.IsNullOrEmpty(info.File))
                        WriteContentLine(writer, indentCount, String.Format(
                            CultureInfo.InvariantCulture,
                            "<HelpKINode Title=\"{0}\" Url=\"{1}\" />",
                            HttpUtility.HtmlEncode(mainEntry),
                            info.File.Substring(baseFolderLength).Replace(
                            '\\', '/')));
                }

                writer.WriteLine();
                writer.WriteLine("</HelpKI>");
            }
        }
        #endregion
    }
}
