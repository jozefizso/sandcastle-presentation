//=============================================================================
// System  : Sandcastle Help File Builder Utilities
// File    : TocEntry.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 02/11/2009
// Note    : Copyright 2006-2009, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a class representing a table of contents entry.  This is
// used to build the table of contents entries for additional content items.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://www.CodePlex.com/SHFB.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.3.0.0  09/17/2006  EFW  Created the code
// 1.3.3.1  12/08/2006  EFW  Added NeedsColorizing property
// 1.4.0.0  03/09/2007  EFW  Added support for <code source="file" /> tags
// 1.5.0.2  07/03/2007  EFW  Added support for saving as a site map file
// 1.6.0.7  04/12/2008  EFW  Added support for a split table of contents
// 1.8.0.0  08/11/2008  EFW  Modified to support the new project format
//=============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

using SandcastleBuilder.Utils.Design;

namespace SandcastleBuilder.Utils.ConceptualContent
{
    /// <summary>
    /// This represents a table of contents entry.  This is used to build the
    /// table of contents entries for additional content items.
    /// </summary>
    public class TocEntry : IComparable<TocEntry>, ICloneable
    {
        #region Private data members
        //=====================================================================
        // Private data members

        private IBasePathProvider pathProvider;
        private TocEntryCollection parent, children;
        private FilePath sourceFile;
        private string destFile, title;
        private bool isDefaultTopic, includePage, hasLinks, needsColorizing,
            hasProjectTags, hasCodeBlocks, splitToc;
        private int sortOrder;
        #endregion

        #region Properties
        //=====================================================================
        // Properties

        /// <summary>
        /// This is used to track the topic's parent collection
        /// </summary>
        /// <remarks>This is used by the designer to move items around within
        /// the collection.</remarks>
        [Browsable(false)]
        public TocEntryCollection Parent
        {
            get { return parent; }
            internal set { parent = value; }
        }

        /// <summary>
        /// This returns the <see cref="IBasePathProvider" /> for the entry.
        /// </summary>
        [Browsable(false)]
        public IBasePathProvider BasePathProvider
        {
            get { return pathProvider; }
        }

        /// <summary>
        /// This returns the child table of contents collection for this entry
        /// </summary>
        /// <value>If empty, this is a single item in the table of contents.
        /// If it has children, they are listed below this one.  A file may
        /// or may not be associated with this entry if it is a root node.</value>
        [Browsable(false)]
        public TocEntryCollection Children
        {
            get { return children; }
        }

        /// <summary>
        /// This is used to get or set the entry's source file path.
        /// </summary>
        [Category("Content Item"), Description("The source file " +
          "associated with this entry.  If blank, the entry will not have " +
          "a display page in the help file."), DefaultValue(null),
          Editor(typeof(FilePathObjectEditor), typeof(UITypeEditor)),
          RefreshProperties(RefreshProperties.All),
         FileDialog("Select the additional content",
           "HTML files (*.htm, *.html, *.topic)|*.htm;*.html;*.topic|" +
           "All Files (*.*)|*.*", FileDialogType.FileOpen)]
        public FilePath SourceFile
        {
            get { return sourceFile; }
            set
            {
                if(value == null)
                    sourceFile = new FilePath(pathProvider);
                else
                    sourceFile = value;
            }
        }

        /// <summary>
        /// This is used to get or set the entry's destination file path.
        /// </summary>
        [Browsable(false)]
        public string DestinationFile
        {
            get { return destFile; }
            set { destFile = value; }
        }

        /// <summary>
        /// This is used to get or set the entry's title in the table of
        /// contents.
        /// </summary>
        [Category("Content Item"), Description("The table of contents title")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// This is used to get or set whether or not the page will appear in
        /// the table of contents.
        /// </summary>
        /// <remarks>For root entries that have children, the node will appear
        /// in the table of contents but will have no page associated with it.
        /// The other options such as <see cref="SortOrder" /> will still have
        /// an effect.</remarks>
        [Browsable(false)]
        public bool IncludePage
        {
            get { return includePage; }
            set { includePage = value; }
        }

        /// <summary>
        /// This is used to get or set whether or not the item is the default
        /// topic for the help file.
        /// </summary>
        [Browsable(false)]
        public bool IsDefaultTopic
        {
            get { return isDefaultTopic; }
            set { isDefaultTopic = value; }
        }

        /// <summary>
        /// This is used to get or set whether or not the topic has links that
        /// need to be resolved when the file is copied.
        /// </summary>
        [Browsable(false)]
        public bool HasLinks
        {
            get { return hasLinks; }
            set { hasLinks = value; }
        }

        /// <summary>
        /// This is used to get or set whether or not the topic has
        /// <c>&lt;pre&gt;</c> blocks that have a <c>lang</c>
        /// attribute to indicate that they should be colorized.
        /// </summary>
        [Browsable(false)]
        public bool NeedsColorizing
        {
            get { return needsColorizing; }
            set { needsColorizing = value; }
        }

        /// <summary>
        /// This is used to get or set whether or not the topic has
        /// <c>&lt;code /&gt;</c> blocks that need expanding.
        /// </summary>
        [Browsable(false)]
        public bool HasCodeBlocks
        {
            get { return hasCodeBlocks; }
            set { hasCodeBlocks = value; }
        }

        /// <summary>
        /// This is used to get or set whether or not the topic has tags
        /// that should be resolved to project options.
        /// </summary>
        [Browsable(false)]
        public bool HasProjectTags
        {
            get { return hasProjectTags; }
            set { hasProjectTags = value; }
        }

        /// <summary>
        /// This is used to get or set whether or not the table of contents is
        /// split after this entry.
        /// </summary>
        /// <remarks>This is only valid on root entries and the first one seen
        /// will cause all items from that tiem onward to appear below the API
        /// content.  All items prior to the entry will appear before the API
        /// content.  If used, the <c>ContentPlacement</c> project property
        /// will be ignored.</remarks>
        [Browsable(false)]
        public bool SplitToc
        {
            get { return splitToc; }
            set { splitToc = value; }
        }

        /// <summary>
        /// This is used to get or set the sort order for the entry within
        /// its group.
        /// </summary>
        /// <value>Entries with identical sort order values will sort by
        /// title as well.  Items with not specific sort order will sort
        /// below those with a defined sort order.</value>
        [Browsable(false)]
        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }
        #endregion

        #region IComparable<TocEntry> Members
        /// <summary>
        /// Compares this instance to another instance and returns an
        /// indication of their relative values.
        /// </summary>
        /// <param name="other">A TocEntry object to compare</param>
        /// <returns>Returns -1 if this instance is less than the
        /// value, 0 if they are equal, or 1 if this instance is
        /// greater than the value or the value is null.</returns>
        /// <remarks>The <see cref="SortOrder"/> property is compared first.
        /// If equal, the <see cref="Title"/> property is used.</remarks>
        public int CompareTo(TocEntry other)
        {
            if(other == null)
                return 1;

            if(this.SortOrder < other.SortOrder)
                return -1;

            if(this.SortOrder > other.SortOrder)
                return 1;

            return String.Compare(this.Title, other.Title, false,
                CultureInfo.CurrentCulture);
        }
        #endregion

        #region ICloneable Members
        //=====================================================================
        // ICloneable implementation

        /// <summary>
        /// Clone this table of contents entry
        /// </summary>
        /// <returns>A clone of this table of contents entry</returns>
        public object Clone()
        {
            TocEntry clone = new TocEntry(pathProvider);
            clone.DestinationFile = destFile;
            clone.HasCodeBlocks = hasCodeBlocks;
            clone.HasLinks = hasLinks;
            clone.HasProjectTags = hasProjectTags;
            clone.IncludePage = includePage;
            clone.IsDefaultTopic = false;   // Can't have more than one
            clone.NeedsColorizing = needsColorizing;
            clone.SortOrder = sortOrder;
            clone.SourceFile = (FilePath)sourceFile.Clone();
            clone.Title = title;

            foreach(TocEntry child in children)
                clone.Children.Add((TocEntry)child.Clone());

            return clone;
        }
        #endregion

        #region Helper methods
        //=====================================================================

        /// <summary>
        /// Mark the project as dirty if the contained lists change
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        /// <remarks>This may not be the best way to handle this.</remarks>
        private void childList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if(parent != null)
                parent.ChildListChanged(this);
        }
        #endregion

        #region Constructor
        //=====================================================================

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="basePathProvider">The base path provider</param>
        public TocEntry(IBasePathProvider basePathProvider)
        {
            pathProvider = basePathProvider;
            includePage = true;
            sortOrder = Int32.MaxValue;
            children = new TocEntryCollection();
            sourceFile = new FilePath(pathProvider);

            children.ListChanged += new ListChangedEventHandler(
                childList_ListChanged);
        }
        #endregion

        #region Equality, hash code, and To String
        //=====================================================================
        // ToString methods

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            TocEntry other = obj as TocEntry;

            if(obj == null)
                return false;

            return (this.SourceFile == other.SourceFile &&
                this.DestinationFile == other.DestinationFile &&
                this.Title == other.Title &&
                this.SortOrder == other.SortOrder &&
                this.IsDefaultTopic == other.IsDefaultTopic &&
                this.SplitToc == other.SplitToc);
        }

        /// <summary>
        /// Overload for equal operator
        /// </summary>
        /// <param name="t1">The first TOC entry object</param>
        /// <param name="t2">The second TOC entry object</param>
        /// <returns>True if equal, false if not.</returns>
        public static bool operator ==(TocEntry t1, TocEntry t2)
        {
            // Do they reference the same entry?
            if(Object.ReferenceEquals(t1, t2))
                return true;

            // Check null reference first (cast to object first to avoid
            // stack overflow).
            if(t1 as object == null)
                return false;

            return t1.Equals(t2);
        }

        /// <summary>
        /// Overload for not equal operator
        /// </summary>
        /// <param name="t1">The first TOC entry object</param>
        /// <param name="t2">The second TOC entry object</param>
        /// <returns>True if not equal, false if they are equal.</returns>
        public static bool operator !=(TocEntry t1, TocEntry t2)
        {
            return !(t1 == t2);
        }

        /// <summary>
        /// Overload for less than operator
        /// </summary>
        /// <param name="t1">The first TOC entry object</param>
        /// <param name="t2">The second TOC entry object</param>
        /// <returns>True if t1 is less than t2, false if not.</returns>
        public static bool operator <(TocEntry t1, TocEntry t2)
        {
            return (t1.CompareTo(t2) < 0);
        }

        /// <summary>
        /// Overload for greater than operator
        /// </summary>
        /// <param name="t1">The first TOC entry object</param>
        /// <param name="t2">The second TOC entry object</param>
        /// <returns>True if t1 is greater than t2, false if not.</returns>
        public static bool operator >(TocEntry t1, TocEntry t2)
        {
            return (t1.CompareTo(t2) > 0);
        }  

        /// <summary>
        /// Get a hash code for this item
        /// </summary>
        /// <returns>Returns the hash code for the <see cref="ToString()" />
        /// value.</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Convert the table of contents entry and its children to a string
        /// </summary>
        /// <returns>The entries in HTML 1.x help format</returns>
        public override string ToString()
        {
            return this.ToString(HelpFileFormat.HtmlHelp1x);
        }

        /// <summary>
        /// Convert the table of contents entry and its children to a string
        /// in the specified help file format.
        /// </summary>
        /// <param name="format">The help file format to use</param>
        /// <returns>The entries in specified help format</returns>
        /// <exception cref="ArgumentException">This is thrown if the
        /// format is not <b>HtmlHelp1x</b> or <b>HtmlHelp2x</b>.</exception>
        public string ToString(HelpFileFormat format)
        {
            if(format != HelpFileFormat.HtmlHelp1x &&
              format != HelpFileFormat.HtmlHelp2x)
                throw new ArgumentException("The format specified must be " +
                    "HtmlHelp1x or HtmlHelp2x only", "format");

            StringBuilder sb = new StringBuilder(1024);
            this.ConvertToString(format, sb);
            return sb.ToString();
        }

        /// <summary>
        /// This is used to convert the collection to a string and append it
        /// to the specified string builder.
        /// </summary>
        /// <param name="format">The help file format to use</param>
        /// <param name="sb">The string builder to which the information is
        /// appended.</param>
        internal void ConvertToString(HelpFileFormat format, StringBuilder sb)
        {
            string guid, url;

            if(format == HelpFileFormat.HtmlHelp1x)
            {
                if(children.Count == 0)
                    sb.AppendFormat("<LI><OBJECT type=\"text/sitemap\">" +
                        "\r\n<param name=\"Name\" value=\"{0}\">\r\n" +
                        "<param name=\"Local\" value=\"{1}\">\r\n" +
                        "</OBJECT></LI>\r\n", HttpUtility.HtmlEncode(title),
                        HttpUtility.HtmlEncode(destFile));
                else
                {
                    if(String.IsNullOrEmpty(destFile))
                        sb.AppendFormat("<LI><OBJECT type=\"text/sitemap\">" +
                            "\r\n<param name=\"Name\" value=\"{0}\">\r\n" +
                            "</OBJECT></LI>\r\n",
                            HttpUtility.HtmlEncode(title));
                    else
                        sb.AppendFormat("<LI><OBJECT type=\"text/sitemap\">" +
                            "\r\n<param name=\"Name\" value=\"{0}\">\r\n" +
                            "<param name=\"Local\" value=\"{1}\">\r\n" +
                            "</OBJECT></LI>\r\n",
                            HttpUtility.HtmlEncode(title),
                            HttpUtility.HtmlEncode(destFile));

                    sb.Append("<UL>\r\n");
                    children.ConvertToString(format, sb);
                    sb.Append("</UL>\r\n");
                }
            }
            else
            {
                if(!String.IsNullOrEmpty(destFile) &&
                  format == HelpFileFormat.Website)
                    url = destFile.Replace("\\", "/");
                else
                    url = destFile;

                if(children.Count == 0)
                    sb.AppendFormat("<HelpTOCNode Url=\"{0}\" Title=\"{1}\" />\r\n",
                        HttpUtility.HtmlEncode(url),
                        HttpUtility.HtmlEncode(title));
                else
                {
                    // Use a GUID to uniquely identify the entries with
                    // children.  This allows the ASP.NET web page to find
                    // them to load the child nodes dynamically.
                    guid = Guid.NewGuid().ToString();

                    // If there is no file for the root node, define the title
                    // property instead.
                    if(String.IsNullOrEmpty(url))
                    {
                        sb.AppendFormat("<HelpTOCNode Id=\"{0}\" Title=\"{1}\">\r\n",
                            guid, HttpUtility.HtmlEncode(title));
                    }
                    else
                        sb.AppendFormat("<HelpTOCNode Id=\"{0}\" Url=\"{1}\" Title=\"{2}\">\r\n",
                            guid, url, HttpUtility.HtmlEncode(title));

                    children.ConvertToString(format, sb);
                    sb.Append("</HelpTOCNode>\r\n");
                }
            }
        }
        #endregion

        #region Convert to link element format
        //=====================================================================

        /// <summary>
        /// Convert the entry to its <c>&lt;a&gt;</c> element form
        /// </summary>
        /// <param name="innerText">Optional inner text</param>
        /// <returns>The topic in its <c>&lt;a&gt;</c> element form</returns>
        public string ToAnchor(string innerText)
        {
            string path = sourceFile.PersistablePath.Replace('\\', '/');

            if(String.IsNullOrEmpty(path))
                path = "#";

            if(path.EndsWith(".topic", StringComparison.OrdinalIgnoreCase))
                path = Path.ChangeExtension(path, ".html");

            if(String.IsNullOrEmpty(innerText))
                return String.Format(CultureInfo.CurrentCulture,
                    "<a href=\"{0}\">{1}</a>", path,
                    this.Title);

            return String.Format(CultureInfo.CurrentCulture,
                "<a href=\"{0}\">{1}</a>", path, innerText);
        }
        #endregion

        #region Site map methods
        //=====================================================================
        // Site map methods

        /// <summary>
        /// This will load information about the entry from the node and will
        /// also load all child nodes.
        /// </summary>
        /// <param name="site">The site map node to use for this entry</param>
        public void LoadSiteMapNode(XmlNode site)
        {
            TocEntry child;

            if(site != null)
            {
                title = site.Attributes["title"].Value;

                if(site.Attributes["url"] == null)
                    sourceFile = new FilePath(pathProvider);
                else
                    sourceFile = new FilePath(site.Attributes["url"].Value,
                        pathProvider);

                if(site.Attributes["isDefault"] != null)
                    isDefaultTopic = true;

                if(site.Attributes["splitToc"] != null)
                    splitToc = true;

                if(site.ChildNodes.Count != 0)
                    foreach(XmlNode childSite in site.ChildNodes)
                    {
                        child = new TocEntry(pathProvider);
                        child.LoadSiteMapNode(childSite);
                        children.Add(child);
                    }
            }
        }

        /// <summary>
        /// Save this node and its children to the specified root node as
        /// site map nodes.
        /// </summary>
        /// <param name="root">The root node to which the current entry
        /// is added.</param>
        public void SaveAsSiteMapNode(XmlNode root)
        {
            XmlAttribute attr;

            XmlNode child = root.OwnerDocument.CreateNode(XmlNodeType.Element,
                "siteMapNode", root.NamespaceURI);

            attr = root.OwnerDocument.CreateAttribute("title");
            attr.Value = title;
            child.Attributes.Append(attr);

            attr = root.OwnerDocument.CreateAttribute("url");

            if(sourceFile.Path.Length == 0)
                attr.Value = String.Empty;
            else
                attr.Value = sourceFile.PersistablePath;

            child.Attributes.Append(attr);

            if(isDefaultTopic)
            {
                attr = root.OwnerDocument.CreateAttribute("isDefault");
                attr.Value = "true";
                child.Attributes.Append(attr);
            }

            if(splitToc)
            {
                attr = root.OwnerDocument.CreateAttribute("splitToc");
                attr.Value = "true";
                child.Attributes.Append(attr);
            }

            root.AppendChild(child);

            if(children.Count != 0)
                foreach(TocEntry te in children)
                    te.SaveAsSiteMapNode(child);
        }

        /// <summary>
        /// See if this entry or one of its children is a match to the
        /// specified source filename.
        /// </summary>
        /// <param name="sourceFilename">The source filename to match</param>
        /// <returns>The match TOC entry or null if not found</returns>
        public TocEntry ContainsMatch(string sourceFilename)
        {
            TocEntry match = null;

            if(sourceFile == sourceFilename)
                return this;

            if(children.Count != 0)
                foreach(TocEntry entry in children)
                {
                    match = entry.ContainsMatch(sourceFilename);

                    if(match != null)
                        break;
                }

            return match;
        }
        #endregion
    }
}
