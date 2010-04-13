//=============================================================================
// System  : Sandcastle Help File Builder Utilities
// File    : TopicCollection.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 01/03/2009
// Note    : Copyright 2008-2009, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a collection class used to hold the conceptual content
// topics for a project.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://SHFB.CodePlex.com.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.6.0.7  04/24/2008  EFW  Created the code
// 1.8.0.0  08/07/2008  EFW  Modified for use with the new project format
//=============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using Microsoft.Build.BuildEngine;

using SandcastleBuilder.Utils;
using SandcastleBuilder.Utils.BuildEngine;

namespace SandcastleBuilder.Utils.ConceptualContent
{
    /// <summary>
    /// This collection class is used to hold the conceptual content topics
    /// for a project.
    /// </summary>
    /// <remarks>This class is serializable so that it can be copied to the
    /// clipboard.</remarks>
    public class TopicCollection : BindingList<Topic>, ITableOfContents
    {
        #region Private data members
        //=====================================================================

        private FileItem fileItem;
        private Topic defaultTopic, splitTOCTopic;
        #endregion

        #region Properties
        //=====================================================================

        /// <summary>
        /// This read-only property returns the project file item associated
        /// with the collection.
        /// </summary>
        public FileItem FileItem
        {
            get { return fileItem; }
        }

        /// <summary>
        /// This is used to get or set the default topic
        /// </summary>
        /// <value>It returns the default topic or null if one is not set</value>
        public Topic DefaultTopic
        {
            get { return defaultTopic; }
            set { defaultTopic = value; }
        }

        /// <summary>
        /// This is used to get or set the topic at which the table of contents
        /// is split by the API content.
        /// </summary>
        /// <value>This will only be valid if it refers to a root level
        /// topic.  It will return null if a split location has not been
        /// set.</value>
        public Topic SplitTocAtTopic
        {
            get { return splitTOCTopic; }
            set { splitTOCTopic = value; }
        }

        /// <summary>
        /// This can be used to get a topic by its unique ID (case-insensitive)
        /// </summary>
        /// <param name="id">The ID of the item to get.</param>
        /// <value>Returns the topic with the specified
        /// <see cref="Topic.Id" /> or null if not found.</value>
        public Topic this[string id]
        {
            get
            {
                Topic found = null;

                foreach(Topic t in this)
                    if(String.Compare(t.Id, id,
                      StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        found = t;
                        break;
                    }
                    else
                        if(t.Subtopics.Count != 0)
                        {
                            found = t.Subtopics[id];

                            if(found != null)
                                break;
                        }

                return found;
            }
        }
        #endregion

        #region Constructor
        //=====================================================================

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="file">The content layout file associated with the
        /// collection.</param>
        /// <remarks>Topics are not loaded until the <see cref="Load" /> method
        /// is called.  If the <b>file</b> parameter is null, this is assumed
        /// to be a child topic collection.</remarks>
        public TopicCollection(FileItem file)
        {
            fileItem = file;
        }
        #endregion

        #region Sort collection
        //=====================================================================

        /// <summary>
        /// This is used to sort the collection
        /// </summary>
        /// <remarks>Values are sorted by display title.  Comparisons are
        /// case-sensitive.</remarks>
        public void Sort()
        {
            ((List<Topic>)base.Items).Sort(
                delegate(Topic x, Topic y)
                {
                    return String.Compare(x.DisplayTitle, y.DisplayTitle,
                        StringComparison.CurrentCulture);
                });

            this.OnListChanged(new ListChangedEventArgs(
                ListChangedType.ItemMoved, -1));
        }
        #endregion

        #region Read/write the content layout file
        //=====================================================================

        /// <summary>
        /// Load the collection from the related file
        /// </summary>
        /// <remarks>This will be done automatically at constructor.  This can
        /// be called to reload the collection if needed.</remarks>
        public void Load()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader xr = null;
            string defTopicId, splitTopicId;
            Topic t;

            try
            {
                this.Clear();
                settings.CloseInput = true;

                xr = XmlReader.Create(fileItem.FullPath, settings);
                xr.MoveToContent();

                defTopicId = xr.GetAttribute("defaultTopic");
                splitTopicId = xr.GetAttribute("splitTOCTopic");

                while(!xr.EOF && xr.NodeType != XmlNodeType.EndElement)
                {
                    if(xr.NodeType == XmlNodeType.Element && xr.Name == "Topic")
                    {
                        t = new Topic();
                        t.ReadXml(xr);
                        this.Add(t);
                    }

                    xr.Read();
                }
            }
            finally
            {
                if(xr != null)
                    xr.Close();
            }

            // Set the default topic if it exists
            if(!String.IsNullOrEmpty(defTopicId))
            {
                defaultTopic = this[defTopicId];

                if(defaultTopic != null)
                    defaultTopic.IsDefaultTopic = true;
            }

            // Set the split TOC topic if it exists
            if(!String.IsNullOrEmpty(splitTopicId))
                splitTOCTopic = this[splitTopicId];

            this.MatchProjectFilesToTopics();
        }

        /// <summary>
        /// This gets all possible content files from the project and attempts
        /// to match them to the topics in the collection by ID.
        /// </summary>
        public void MatchProjectFilesToTopics()
        {
            SandcastleProject project = fileItem.ProjectElement.Project;
            FileItem topicItem;
            TopicFile topicFile;

            string ext, none = BuildAction.None.ToString(),
                content = BuildAction.Content.ToString();

            foreach(BuildItem item in project.MSBuildProject.EvaluatedItems)
                if(item.Name == none || item.Name == content)
                {
                    ext = Path.GetExtension(item.Include).ToLower(
                        CultureInfo.InvariantCulture);

                    if(ext == ".aml" || ext == ".htm" || ext == ".html" ||
                      ext == ".topic")
                    {
                        topicItem = new FileItem(new ProjectElement(project, item));
                        topicFile = new TopicFile(topicItem);

                        if(topicFile.Id != null)
                            this.SetTopic(topicFile);
                    }
                }
        }

        /// <summary>
        /// Save the topic collection to the related content layout file
        /// </summary>
        public void Save()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = null;

            try
            {
                settings.Indent = true;
                settings.CloseOutput = true;
                writer = XmlWriter.Create(fileItem.FullPath, settings);

                writer.WriteStartDocument();
                writer.WriteStartElement("Topics");

                if(defaultTopic != null)
                    writer.WriteAttributeString("defaultTopic",
                        defaultTopic.Id);

                if(splitTOCTopic != null)
                    writer.WriteAttributeString("splitTOCTopic",
                        splitTOCTopic.Id);

                foreach(Topic t in this)
                    t.WriteXml(writer);

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            finally
            {
                if(writer != null)
                    writer.Close();
            }
        }
        #endregion

        #region Private helper methods
        //=====================================================================

        /// <summary>
        /// This is used by contained items to notify the parent that a child
        /// list changed and thus the collection should be marked as dirty.
        /// </summary>
        /// <param name="changedItem">The item that changed</param>
        internal void ChildListChanged(Topic changedItem)
        {
            this.OnListChanged(new ListChangedEventArgs(
                ListChangedType.ItemChanged, this.IndexOf(changedItem)));
        }

        /// <summary>
        /// Set the topic file in any entry that has a matching ID
        /// </summary>
        /// <param name="topicFile">The topic file</param>
        /// <remarks>The IDs should be unique across all entries but, if a
        /// duplicate exists, this will help find it as we'll get a more
        /// descriptive error later in the build.</remarks>
        private void SetTopic(TopicFile topicFile)
        {
            foreach(Topic t in this)
            {
                if(t.Id == topicFile.Id)
                    t.TopicFile = topicFile;

                if(t.Subtopics.Count != 0)
                    t.Subtopics.SetTopic(topicFile);
            }
        }
        #endregion

        #region Generate conceptual topics for the build
        //=====================================================================

        /// <summary>
        /// This creates copies of the conceptual topic files in the build
        /// process's working folder.
        /// </summary>
        /// <param name="folder">The folder in which to place the topic files</param>
        /// <param name="builder">The build process</param>
        /// <remarks>Each topic file will be named using its
        /// <see cref="Topic.Id" />.  If necessary, its content will be
        /// wrapped in a <c>&lt;topic&gt;</c> element.  Sub-topics are written
        /// out recursively.</remarks>
        public void GenerateConceptualTopics(string folder,
          BuildProcess builder)
        {
            string destFile;

            foreach(Topic t in this)
            {
                if(t.TopicFile == null)
                {
                    // If there is an ID with no file, the file is missing
                    if(!t.NoTopicFile)
                        throw new BuilderException("BE0054", String.Format(
                            CultureInfo.InvariantCulture, "The conceptual " +
                            "topic '{0}' (ID: {1}) does not match any file " +
                            "in the project", t.DisplayTitle, t.ContentId));

                    // A file is required if there are no sub-topics
                    if(t.Subtopics.Count == 0)
                        throw new BuilderException("BE0055", String.Format(
                            CultureInfo.InvariantCulture, "The conceptual " +
                            "topic '{0}' (ID: {1}) must either specify a " +
                            "topic file or it must contains sub-topics",
                            t.DisplayTitle, t.Id));

                    // No file, it's just a container node
                    t.Subtopics.GenerateConceptualTopics(folder, builder);
                    continue;
                }

                builder.ReportProgress("    Parsing topic file '{0}'",
                    t.TopicFile.FullPath);

                // It must exist
                if(t.DocumentType <= DocumentType.NotFound)
                    throw new BuilderException("BE0056", String.Format(
                        CultureInfo.InvariantCulture, "The conceptual " +
                        "content file '{0}' with ID '{1}' does not exist.",
                        t.TopicFile.FullPath, t.Id));

                // And it must be valid
                if(t.DocumentType == DocumentType.Invalid)
                    throw new BuilderException("BE0057", String.Format(
                        CultureInfo.InvariantCulture, "The conceptual " +
                        "content file '{0}' with ID '{1}' is not valid: {2}",
                        t.TopicFile.FullPath, t.Id, t.TopicFile.ErrorMessage));

                if(t.DocumentType != DocumentType.Html)
                    destFile = Path.Combine(folder, t.Id + ".xml");
                else
                {
                    destFile = Path.Combine(Path.Combine(folder,
                        @"..\Output\html"), t.Id + ".htm");

                    if(!Directory.Exists(Path.GetDirectoryName(destFile)))
                        Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                }

                builder.ReportProgress("    {0} -> {1}", t.TopicFile.FullPath,
                    destFile);

                // The IDs must be unique
                if(File.Exists(destFile))
                    throw new BuilderException("BE0058", String.Format(
                        CultureInfo.InvariantCulture, "Two conceptual " +
                        "content files have the same ID ({0}).  The file " +
                        "with the duplicate ID is '{1}'", t.Id,
                        t.TopicFile.FullPath));

                if(t.DocumentType != DocumentType.Html)
                {
                    File.Copy(t.TopicFile.FullPath, destFile);
                    File.SetAttributes(destFile, FileAttributes.Normal);
                }
                else
                    builder.ResolveLinksAndCopy(t.TopicFile.FullPath, destFile,
                        BuildProcess.GetTocInfo(t.TopicFile.FullPath));

                if(t.Subtopics.Count != 0)
                    t.Subtopics.GenerateConceptualTopics(folder, builder);
            }
        }
        #endregion

        #region Add all topic files from a folder
        //=====================================================================

        /// <summary>
        /// Add all topics from the specified folder recursively to the
        /// collection and to the given project file.
        /// </summary>
        /// <param name="folder">The folder from which to get the files</param>
        /// <param name="basePath">The base path to remove from files copied
        /// from another folder into the project folder.  On the first call,
        /// this should match the <paramref name="folder"/> value.</param>
        /// <param name="project">The project to which the files are added</param>
        /// <remarks>Only actual conceptual content topic files are added.
        /// They must have a ".aml" extension and must be one of the valid
        /// <see cref="DocumentType">document types</see>.  Folders will be
        /// added as sub-topics recursively.  If a file with the same name
        /// as the folder exists, it will be associated with the container
        /// node.  If no such file exists, an empty container node is created.
        /// </remarks>
        public void AddTopicsFromFolder(string folder, string basePath,
          SandcastleProject project)
        {
            FileItem newItem;
            Topic topic, removeTopic;
            string[] files = Directory.GetFiles(folder, "*.aml");
            string name, newPath, projectPath = Path.GetDirectoryName(
                project.Filename);

            if(basePath.Length !=0 && basePath[basePath.Length - 1] != '\\')
                basePath += "\\";

            // Add files
            foreach(string file in files)
            {
                try
                {
                    // The file must reside under the project path
                    if(Path.GetDirectoryName(file).StartsWith(projectPath,
                      StringComparison.OrdinalIgnoreCase))
                        newPath = file;
                    else
                        newPath = Path.Combine(projectPath, file.Substring(
                            basePath.Length));

                    // Add the file to the project
                    newItem = project.AddFileToProject(file, newPath);
                    topic = new Topic();
                    topic.TopicFile = new TopicFile(newItem);

                    if(topic.DocumentType > DocumentType.Invalid)
                        this.Add(topic);
                }
                catch
                {
                    // Ignore invalid files
                }
            }

            // Add folders recursively
            files = Directory.GetDirectories(folder);

            foreach(string folderName in files)
            {
                topic = new Topic();
                topic.Title = name = Path.GetFileName(folderName);
                topic.Subtopics.AddTopicsFromFolder(folderName, basePath, project);

                // Ignore empty folders
                if(topic.Subtopics.Count == 0)
                    continue;

                this.Add(topic);

                // Look for a file with the same name as the folder
                removeTopic = null;

                foreach(Topic t in topic.Subtopics)
                    if(t.TopicFile != null && Path.GetFileNameWithoutExtension(
                      t.TopicFile.FullPath) == name)
                    {
                        // If found, remove it as it represents the container
                        // node.
                        topic.Title = null;
                        topic.TopicFile = t.TopicFile;
                        removeTopic = t;
                        break;
                    }

                if(removeTopic != null)
                    topic.Subtopics.Remove(removeTopic);
            }
        }
        #endregion

        #region Overrides
        //=====================================================================

        /// <summary>
        /// This is overridden to set the inserted item's parent to this
        /// collection.
        /// </summary>
        /// <inheritdoc />
        protected override void InsertItem(int index, Topic item)
        {
            base.InsertItem(index, item);
            item.Parent = this;
        }

        /// <summary>
        /// This is overridden to set the inserted item's parent to this
        /// collection.
        /// </summary>
        /// <inheritdoc />
        protected override void SetItem(int index, Topic item)
        {
            base.SetItem(index, item);
            item.Parent = this;
        }

        /// <summary>
        /// This is overridden to clear the parent on the removed item
        /// </summary>
        /// <param name="index">The index of the item to remove</param>
        protected override void RemoveItem(int index)
        {
            Topic item = this[index];

            item.Parent = null;

            if(item == defaultTopic)
                defaultTopic = null;

            if(item == splitTOCTopic)
                splitTOCTopic = null;

            base.RemoveItem(index);
        }

        /// <summary>
        /// This is overridden to clear the default topic and split TOC topic
        /// when the collection is cleared.
        /// </summary>
        protected override void ClearItems()
        {
            defaultTopic = splitTOCTopic = null;
            base.ClearItems();
        }
        #endregion

        #region ITableOfContents implementation
        //=====================================================================

        /// <summary>
        /// This is used to get the build item related to the content layout
        /// file containing the collection items.
        /// </summary>
        public FileItem ContentLayoutFile
        {
            get { return fileItem; }
        }

        /// <summary>
        /// Generate the table of contents for the conceptual topics
        /// </summary>
        /// <param name="toc">The table of contents collection</param>
        /// <param name="pathProvider">The base path provider</param>
        public void GenerateTableOfContents(TocEntryCollection toc,
          IBasePathProvider pathProvider)
        {
            TocEntry entry;

            foreach(Topic t in this)
                if(t.Visible)
                {
                    entry = new TocEntry(pathProvider);

                    if(t.TopicFile != null)
                    {
                        entry.SourceFile = new FilePath(
                            t.TopicFile.FullPath,
                            t.TopicFile.FileItem.ProjectElement.Project);
                        entry.DestinationFile = "html\\" + t.Id + ".htm";
                    }

                    entry.Title = t.DisplayTitle;
                    entry.IsDefaultTopic = t.IsDefaultTopic;
                    entry.SplitToc = (t == splitTOCTopic);

                    if(t.Subtopics.Count != 0)
                        t.Subtopics.GenerateTableOfContents(entry.Children,
                            pathProvider);

                    toc.Add(entry);
                }
        }
        #endregion
    }
}
