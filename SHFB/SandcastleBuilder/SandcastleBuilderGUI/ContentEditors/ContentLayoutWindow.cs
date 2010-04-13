//=============================================================================
// System  : Sandcastle Help File Builder
// File    : ContentLayoutWindow.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 01/21/2009
// Note    : Copyright 2008-2009, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the form used to edit the conceptual content items.
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
// 1.8.0.0  09/04/2008  EFW  Reworked for use with the new project format
//=============================================================================

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

using SandcastleBuilder.Utils;
using SandcastleBuilder.Utils.ConceptualContent;
using SandcastleBuilder.Utils.Design;

using WeifenLuo.WinFormsUI.Docking;

namespace SandcastleBuilder.Gui.ContentEditors
{
    /// <summary>
    /// This form is used to edit the conceptual content items
    /// </summary>
    public partial class ContentLayoutWindow : BaseContentEditor
    {
        #region Private data Members
        //=====================================================================

        private TopicCollection topics;
        private TreeNode defaultNode, splitTocNode, firstNode;
        private Topic firstSelection;
        private static object cutClipboard, copyClipboard;

        //=====================================================================
        #endregion

        #region Constructor
        //=====================================================================

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileItem">The project file item to edit</param>
        public ContentLayoutWindow(FileItem fileItem)
        {
            EventHandler onClick = new EventHandler(templateFile_OnClick);
            ToolStripMenuItem miTemplate;
            Image itemImage;
            string name;

            InitializeComponent();

            sbStatusBarText.InstanceStatusBar = MainForm.Host.StatusBarTextLabel;

            // Add the topic templates to the New Topic context menu
            string[] files = Directory.GetFiles(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "ConceptualTemplates"), "*.aml");

            if(files.Length == 0)
                miStandardSibling.Enabled = miStandardChild.Enabled = false;
            else
                foreach(string file in files)
                {
                    name = Path.GetFileNameWithoutExtension(file);
                    itemImage = null;

                    // For Conceptual.aml, make it the default action when the
                    // toolbar button is clicked.
                    if(name == "Conceptual")
                    {
                        tsbAddSiblingTopic.ButtonClick -= new EventHandler(
                            tsbAddTopic_ButtonClick);
                        tsbAddChildTopic.ButtonClick -= new EventHandler(
                            tsbAddTopic_ButtonClick);
                        tsbAddSiblingTopic.ButtonClick += new EventHandler(onClick);
                        tsbAddChildTopic.ButtonClick += new EventHandler(onClick);
                        tsbAddSiblingTopic.Tag = tsbAddChildTopic.Tag = file;

                        itemImage = miAddEmptySibling.Image;
                        miAddEmptySibling.Image = miAddEmptyChild.Image = null;
                    }

                    miTemplate = new ToolStripMenuItem(name, null, onClick);
                    miTemplate.Image = itemImage;
                    miTemplate.Tag = file;
                    sbStatusBarText.SetStatusBarText(miTemplate, "Add new '" +
                        name + "' topic");
                    miStandardSibling.DropDownItems.Add(miTemplate);

                    miTemplate = new ToolStripMenuItem(name, null, onClick);
                    miTemplate.Image = itemImage;
                    miTemplate.Tag = file;
                    sbStatusBarText.SetStatusBarText(miTemplate, "Add new '" +
                        name + "' topic");
                    miStandardChild.DropDownItems.Add(miTemplate);
                }

            // Look for custom templates in the local application data folder
            name = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
                Constants.ConceptualTemplates);

            if(!Directory.Exists(name))
                miCustomSibling.Enabled = miCustomChild.Enabled = false;
            else
            {
                files = Directory.GetFiles(name, "*.aml");

                if(files.Length == 0)
                    miCustomSibling.Enabled = miCustomChild.Enabled = false;
                else
                    foreach(string file in files)
                    {
                        name = Path.GetFileNameWithoutExtension(file);
                        itemImage = null;

                        miTemplate = new ToolStripMenuItem(name, null, onClick);
                        miTemplate.Image = itemImage;
                        miTemplate.Tag = file;
                        sbStatusBarText.SetStatusBarText(miTemplate,
                            "Add new '" + name + "' topic");
                        miCustomSibling.DropDownItems.Add(miTemplate);

                        miTemplate = new ToolStripMenuItem(name, null, onClick);
                        miTemplate.Image = itemImage;
                        miTemplate.Tag = file;
                        sbStatusBarText.SetStatusBarText(miTemplate,
                            "Add new '" + name + "' topic");
                        miCustomChild.DropDownItems.Add(miTemplate);
                    }
            }

            topics = new TopicCollection(fileItem);
            topics.Load();
            topics.ListChanged += new ListChangedEventHandler(topics_ListChanged);

            this.Text = Path.GetFileName(fileItem.FullPath);
            this.ToolTipText = fileItem.FullPath;
            this.LoadTopics(null);
        }
        #endregion

        #region Helper methods
        //=====================================================================

        /// <summary>
        /// Save the topic to a new filename
        /// </summary>
        /// <param name="filename">The new filename</param>
        /// <returns>True if saved successfully, false if not</returns>
        /// <overloads>There are two overloads for this method</overloads>
        public bool Save(string filename)
        {
            string projectPath = Path.GetDirectoryName(
                topics.FileItem.ProjectElement.Project.Filename);

            if(!filename.StartsWith(projectPath, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("The file must reside in the project folder " +
                    "or a folder below it", Constants.AppName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            topics.FileItem.Include = new FilePath(filename,
                topics.FileItem.ProjectElement.Project);
            this.Text = Path.GetFileName(filename);
            this.ToolTipText = filename;
            this.IsDirty = true;
            return this.Save();
        }

        /// <summary>
        /// Load the tree view with the topics and set the form up to edit them
        /// </summary>
        /// <param name="selectedEntry">If not null, the node containing the
        /// specified entry is set as the selected node.  If null, the first
        /// node is selected.</param>
        private void LoadTopics(Topic selectedEntry)
        {
            TreeNode node;

            try
            {
                tvContent.SuspendLayout();
                tvContent.Nodes.Clear();

                defaultNode = splitTocNode = firstNode = null;
                firstSelection = selectedEntry;

                if(topics.Count != 0)
                {
                    foreach(Topic t in topics)
                    {
                        node = tvContent.Nodes.Add(t.DisplayTitle);
                        node.Name = t.Id;
                        node.Tag = t;

                        if(topics.DefaultTopic == t)
                            defaultNode = node;

                        // This is only valid at the root level
                        if(topics.SplitTocAtTopic == t)
                            splitTocNode = node;

                        if(t == firstSelection)
                            firstNode = node;

                        if(t.Subtopics.Count != 0)
                            this.AddChildren(t.Subtopics, node);
                    }

                    if(defaultNode != null)
                    {
                        defaultNode.ToolTipText = "Default topic";
                        defaultNode.ImageIndex = defaultNode.SelectedImageIndex = 1;
                    }

                    if(splitTocNode != null)
                    {
                        splitTocNode.ToolTipText = "Split Table of Contents";
                        splitTocNode.ImageIndex = splitTocNode.SelectedImageIndex = 10;
                    }

                    if(defaultNode != null && defaultNode == splitTocNode)
                    {
                        defaultNode.ToolTipText = "Default topic/Split TOC";
                        defaultNode.ImageIndex = defaultNode.SelectedImageIndex = 11;
                    }

                    tvContent.ExpandAll();

                    if(firstNode != null)
                    {
                        tvContent.SelectedNode = firstNode;
                        firstNode.EnsureVisible();
                    }
                    else
                    {
                        tvContent.SelectedNode = tvContent.Nodes[0];
                        tvContent.SelectedNode.EnsureVisible();
                    }
                }
                else
                    this.UpdateControlStatus();
            }
            finally
            {
                tvContent.ResumeLayout();
            }
        }

        /// <summary>
        /// Add child nodes to the tree view recursively
        /// </summary>
        /// <param name="children">The collection of entries to add</param>
        /// <param name="root">The root to which they are added</param>
        private void AddChildren(TopicCollection children, TreeNode root)
        {
            TreeNode node;

            foreach(Topic t in children)
            {
                node = root.Nodes.Add(t.DisplayTitle);
                node.Name = t.Id;
                node.Tag = t;

                if(topics.DefaultTopic == t)
                    defaultNode = node;

                if(t == firstSelection)
                    firstNode = node;

                if(t.Subtopics.Count != 0)
                    this.AddChildren(t.Subtopics, node);
            }
        }

        /// <summary>
        /// This is used to update the state of the form controls
        /// </summary>
        private void UpdateControlStatus()
        {
            TreeNode current = tvContent.SelectedNode;

            tsbPaste.Enabled = tsmiPasteAsSibling.Enabled =
                tsmiPasteAsChild.Enabled = miPaste.Enabled =
                miPasteAsChild.Enabled = (cutClipboard != null);

            if(tvContent.Nodes.Count == 0)
            {
                miDefaultTopic.Enabled = miSplitToc.Enabled =
                    miMoveUp.Enabled = miMoveDown.Enabled =
                    miAddChild.Enabled = miDelete.Enabled =
                    miCut.Enabled = miCopyAsLink.Enabled =
                    tsbDefaultTopic.Enabled = tsbSplitTOC.Enabled =
                    tsbAddChildTopic.Enabled = tsbDeleteTopic.Enabled =
                    tsbMoveUp.Enabled = tsbMoveDown.Enabled = tsbCut.Enabled =
                    tsbEditTopic.Enabled = miEditTopic.Enabled =
                    pgProps.Enabled = false;

                pgProps.SelectedObject = null;
            }
            else
            {
                miDefaultTopic.Enabled = miAddChild.Enabled =
                    miDelete.Enabled = miCut.Enabled = miCopyAsLink.Enabled =
                    tsbDefaultTopic.Enabled = tsbAddChildTopic.Enabled =
                    tsbDeleteTopic.Enabled = tsbCut.Enabled =
                    pgProps.Enabled = true;

                tsbEditTopic.Enabled = miEditTopic.Enabled =
                    (((Topic)current.Tag).TopicFile != null);
                tsbSplitTOC.Enabled = miSplitToc.Enabled =
                    (current.Parent == null);
                tsbMoveDown.Enabled = miMoveDown.Enabled =
                    (current.NextNode != null);
                tsbMoveUp.Enabled = miMoveUp.Enabled =
                    (current.PrevNode != null);

                if(pgProps.SelectedObject != current.Tag)
                    pgProps.SelectedObject = current.Tag;
            }
        }
        #endregion

        #region Method overrides
        //=====================================================================

        /// <inheritdoc />
        public override bool CanClose
        {
            get
            {
                if(!this.IsDirty)
                    return true;

                DialogResult dr = MessageBox.Show("Do you want to save your " +
                    "changes to '" + this.ToolTipText + "?  Click YES to " +
                    "to save them, NO to discard them, or CANCEL to stay " +
                    "here and make further changes.", "Topic Editor",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button3);

                if(dr == DialogResult.Cancel)
                    return false;

                if(dr == DialogResult.Yes)
                {
                    this.Save();

                    if(this.IsDirty)
                        return false;
                }
                else
                    this.IsDirty = false;    // Don't ask again

                return true;
            }
        }

        /// <inheritdoc />
        public override bool CanSaveContent
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override bool IsContentDocument
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override bool Save()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if(this.IsDirty)
                {
                    topics.Save();
                    this.Text = Path.GetFileName(this.ToolTipText);
                    this.IsDirty = false;
                }

                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unable to save file.  Reason: " + ex.Message,
                    "Content Layout Editor", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        /// <inheritdoc />
        public override bool SaveAs()
        {
            using(SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Save Content Layout File As";
                dlg.Filter = "Content layout files (*.content)|*.content|" +
                    "All Files (*.*)|*.*";
                dlg.DefaultExt = Path.GetExtension(this.ToolTipText);
                dlg.InitialDirectory = Path.GetDirectoryName(this.ToolTipText);

                if(dlg.ShowDialog() == DialogResult.OK)
                    return this.Save(dlg.FileName);
            }

            return false;
        }
        #endregion

        #region General event handlers
        //=====================================================================

        /// <summary>
        /// This is used to mark the file as dirty when the collection changes
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        void topics_ListChanged(object sender, ListChangedEventArgs e)
        {
            if(!this.IsDirty)
            {
                this.IsDirty = true;
                this.Text += "*";
            }
        }

        /// <summary>
        /// This is used to prompt for save when closing
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void ContentLayoutWindow_FormClosing(object sender,
          FormClosingEventArgs e)
        {
            cutClipboard = copyClipboard = null;
            e.Cancel = !this.CanClose;
        }

        /// <summary>
        /// Update the node text when a property changes if it affects the
        /// node text.
        /// </summary>
        /// <param name="s">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void pgProps_PropertyValueChanged(object s,
          PropertyValueChangedEventArgs e)
        {
            TreeNode selectedNode = tvContent.SelectedNode;
            string label = e.ChangedItem.Label;
            Topic t;

            if(label == "Id" || label == "Title" ||
              label == "TocTitle" || label == "TopicFile")
            {
                t = (Topic)tvContent.SelectedNode.Tag;
                selectedNode.Name = t.Id;
                selectedNode.Text = t.DisplayTitle;
                UpdateControlStatus();
            }

            this.topics_ListChanged(this, new ListChangedEventArgs(
                ListChangedType.ItemChanged, -1));
        }

        /// <summary>
        /// Find a topic by ID when Enter is hit in the text box
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void txtFindId_KeyDown(object sender, KeyEventArgs e)
        {
            TreeNode[] nodes;

            if(e.KeyCode != Keys.Enter || txtFindId.Text.Trim().Length == 0)
                return;

            nodes = tvContent.Nodes.Find(txtFindId.Text.Trim(), true);

            if(nodes.Length > 0)
            {
                tvContent.SelectedNode = nodes[0];
                pgProps.Focus();
                e.SuppressKeyPress = e.Handled = true;
            }
        }
        #endregion

        #region Topic toolstrip event handlers
        //=====================================================================

        /// <summary>
        /// Set the selected node as the default topic
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The sender of the event</param>
        private void tsbDefaultTopic_Click(object sender, EventArgs e)
        {
            TreeNode newDefault = tvContent.SelectedNode;

            if(defaultNode != null && defaultNode == splitTocNode)
            {
                defaultNode.ToolTipText = "Split Table of Contents";
                defaultNode.ImageIndex = defaultNode.SelectedImageIndex = 10;
            }
            else
                if(defaultNode != null)
                {
                    defaultNode.ToolTipText = null;
                    defaultNode.ImageIndex = defaultNode.SelectedImageIndex = 0;
                }

            if(defaultNode != newDefault)
            {
                if(newDefault == splitTocNode)
                {
                    newDefault.ToolTipText = "Default topic/Split TOC";
                    newDefault.ImageIndex = newDefault.SelectedImageIndex = 11;
                }
                else
                {
                    newDefault.ToolTipText = "Default topic";
                    newDefault.ImageIndex = newDefault.SelectedImageIndex = 1;
                }

                defaultNode = newDefault;
                topics.DefaultTopic = (Topic)defaultNode.Tag;
            }
            else
                if(defaultNode != null)
                {
                    defaultNode = null;     // Turn it off altogether
                    topics.DefaultTopic = null;
                }

            this.topics_ListChanged(this, new ListChangedEventArgs(
                ListChangedType.ItemChanged, -1));
        }

        /// <summary>
        /// Set the selected node as the split TOC location
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The sender of the event</param>
        private void tsbSplitTOC_Click(object sender, EventArgs e)
        {
            TreeNode newSplit = tvContent.SelectedNode;

            if(splitTocNode != null && defaultNode == splitTocNode)
            {
                splitTocNode.ToolTipText = "Default topic";
                splitTocNode.ImageIndex = splitTocNode.SelectedImageIndex = 1;
            }
            else
                if(splitTocNode != null)
                {
                    splitTocNode.ToolTipText = null;
                    splitTocNode.ImageIndex = splitTocNode.SelectedImageIndex = 0;
                }

            if(splitTocNode != newSplit)
            {
                if(newSplit == defaultNode)
                {
                    newSplit.ToolTipText = "Default topic/Split TOC";
                    newSplit.ImageIndex = newSplit.SelectedImageIndex = 11;
                }
                else
                {
                    newSplit.ToolTipText = "Split Table of Contents";
                    newSplit.ImageIndex = newSplit.SelectedImageIndex = 10;
                }

                splitTocNode = newSplit;
                topics.SplitTocAtTopic = (Topic)splitTocNode.Tag;
            }
            else
                if(splitTocNode != null)
                {
                    splitTocNode = null;    // Turn it off altogether
                    topics.SplitTocAtTopic = null;
                }

            this.topics_ListChanged(this, new ListChangedEventArgs(
                ListChangedType.ItemChanged, -1));
        }

        /// <summary>
        /// Move the selected node up or down within the group
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The sender of the event</param>
        private void tsbMoveItem_Click(object sender, EventArgs e)
        {
            TreeNode moveNode, insertNode;
            TreeNodeCollection tnc;
            TopicCollection parent;
            Topic topic;
            int index;

            moveNode = tvContent.SelectedNode;
            topic = (Topic)moveNode.Tag;
            parent = topic.Parent;
            index = parent.IndexOf(topic);

            if(moveNode.Parent == null)
                tnc = tvContent.Nodes;
            else
                tnc = moveNode.Parent.Nodes;

            if(sender == tsbMoveUp || sender == miMoveUp)
            {
                insertNode = moveNode.PrevNode;
                tnc.Remove(moveNode);
                tnc.Insert(tnc.IndexOf(insertNode), moveNode);

                parent.RemoveAt(index);
                parent.Insert(index - 1, topic);
            }
            else
            {
                insertNode = moveNode.NextNode;
                tnc.Remove(moveNode);
                tnc.Insert(tnc.IndexOf(insertNode) + 1, moveNode);

                parent.RemoveAt(index);
                parent.Insert(index + 1, topic);
            }

            tvContent.SelectedNode = moveNode;
        }

        /// <summary>
        /// Add an existing topic file
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void AddExistingTopicFile_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem miSelection = (ToolStripMenuItem)sender;
            TreeNode selectedNode = tvContent.SelectedNode;

            using(OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Select the conceptual topic file(s)";
                dlg.Filter = "Conceptual Topics (*.aml)|*.aml|HTML Files " +
                    "(*.htm, *.html)|*.html;*.htm|All files (*.*)|*.*";
                dlg.DefaultExt = "aml";
                dlg.InitialDirectory = Directory.GetCurrentDirectory();
                dlg.Multiselect = true;

                // If selected, add the new file(s).  Filenames that are
                // already in the collection are ignored.
                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    foreach(string filename in dlg.FileNames)
                    {
                        this.AddTopicFile(filename,
                            miSelection.Owner == cmsNewChildTopic);
                        tvContent.SelectedNode = selectedNode;
                    }

                    MainForm.Host.ProjectExplorer.RefreshProject();
                }
            }
        }

        /// <summary>
        /// Add a new item based on the selected template file
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void templateFile_OnClick(object sender, EventArgs e)
        {
            TreeNode selectedNode = tvContent.SelectedNode;
            Topic selectedTopic;
            ToolStripItem miSelection = (ToolStripItem)sender;
            string file = (string)miSelection.Tag;
            Guid guid = Guid.NewGuid();

            using(SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Save New Topic As";
                dlg.Filter = "Conceptual Topics (*.aml)|*.aml|" +
                    "All files (*.*)|*.*";
                dlg.FileName = guid.ToString() + ".aml";
                dlg.DefaultExt = "aml";
                dlg.InitialDirectory = Directory.GetCurrentDirectory();

                if(selectedNode != null)
                {
                    selectedTopic = (Topic)selectedNode.Tag;

                    if(selectedTopic != null && selectedTopic.TopicFile != null)
                        dlg.InitialDirectory = Path.GetDirectoryName(
                            selectedTopic.TopicFile.FullPath);
                }

                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    // Set the unique ID in the new topic, save it to the
                    // specified filename, and add it to the project.
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(file);

                        XmlNode node = doc.SelectSingleNode("topic");

                        if(node == null)
                            throw new InvalidOperationException(
                                "Unable to locate root topic node");

                        if(node.Attributes["id"] == null)
                            throw new InvalidOperationException(
                                "Unable to locate 'id' attribute on root topic node");

                        node.Attributes["id"].Value = guid.ToString();
                        doc.Save(dlg.FileName);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Unable to set topic ID.  Reason:" +
                            ex.Message, Constants.AppName, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    this.AddTopicFile(dlg.FileName,
                        sender == tsbAddChildTopic ||
                        miSelection.OwnerItem == miStandardChild ||
                        miSelection.OwnerItem == miCustomChild);

                    MainForm.Host.ProjectExplorer.RefreshProject();
                    tsbEditTopic_Click(sender, e);
                }
            }
        }

        /// <summary>
        /// Add a new topic file
        /// </summary>
        /// <param name="filename">The filename of the topic to add</param>
        /// <param name="addAsChild">True to add as a child of the selected
        /// node or false to add it as a sibling.</param>
        private void AddTopicFile(string filename, bool addAsChild)
        {
            Topic topic, parentTopic;
            TreeNode tnNew, tnParent;
            string newPath = filename, projectPath = Path.GetDirectoryName(
                topics.FileItem.ProjectElement.Project.Filename);

            // The file must reside under the project path
            if(!Path.GetDirectoryName(filename).StartsWith(projectPath,
              StringComparison.OrdinalIgnoreCase))
                newPath = Path.Combine(projectPath, Path.GetFileName(filename));

            // Add the file to the project
            FileItem newItem = topics.FileItem.ProjectElement.Project.AddFileToProject(
                filename, newPath);
            topic = new Topic();
            topic.TopicFile = new TopicFile(newItem);

            tnNew = new TreeNode(topic.DisplayTitle);
            tnNew.Name = topic.Id;
            tnNew.Tag = topic;

            if(addAsChild)
            {
                tvContent.SelectedNode.Nodes.Add(tnNew);
                parentTopic = (Topic)tvContent.SelectedNode.Tag;
                parentTopic.Subtopics.Add(topic);
            }
            else
                if(tvContent.SelectedNode == null)
                {
                    tvContent.Nodes.Add(tnNew);
                    topics.Add(topic);
                }
                else
                {
                    if(tvContent.SelectedNode.Parent == null)
                        tvContent.Nodes.Insert(tvContent.Nodes.IndexOf(
                            tvContent.SelectedNode) + 1, tnNew);
                    else
                    {
                        tnParent = tvContent.SelectedNode.Parent;
                        tnParent.Nodes.Insert(tnParent.Nodes.IndexOf(
                            tvContent.SelectedNode) + 1, tnNew);
                    }

                    parentTopic = (Topic)tvContent.SelectedNode.Tag;
                    parentTopic.Parent.Insert(
                        parentTopic.Parent.IndexOf(parentTopic) + 1,
                        topic);
                }

            tvContent.SelectedNode = tnNew;
        }

        /// <summary>
        /// Add an empty container node that is not associated with any topic
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void tsbAddTopic_ButtonClick(object sender, EventArgs e)
        {
            ToolStripItem tiAdd = (ToolStripItem)sender;
            Topic topic, parentTopic;
            TreeNode tnNew, tnParent;

            topic = new Topic();
            topic.Title = "Table of Contents Container";
            topic.TopicFile = null;
            tnNew = new TreeNode(topic.DisplayTitle);
            tnNew.Name = topic.Id;
            tnNew.Tag = topic;

            if(tiAdd == tsbAddChildTopic || tiAdd.Owner == cmsNewChildTopic)
            {
                tvContent.SelectedNode.Nodes.Add(tnNew);
                parentTopic = (Topic)tvContent.SelectedNode.Tag;
                parentTopic.Subtopics.Add(topic);
            }
            else
                if(tvContent.SelectedNode == null)
                {
                    tvContent.Nodes.Add(tnNew);
                    topics.Add(topic);
                }
                else
                {
                    if(tvContent.SelectedNode.Parent == null)
                        tvContent.Nodes.Insert(tvContent.Nodes.IndexOf(
                            tvContent.SelectedNode) + 1, tnNew);
                    else
                    {
                        tnParent = tvContent.SelectedNode.Parent;
                        tnParent.Nodes.Insert(tnParent.Nodes.IndexOf(
                            tvContent.SelectedNode) + 1, tnNew);
                    }

                    parentTopic = (Topic)tvContent.SelectedNode.Tag;
                    parentTopic.Parent.Insert(
                        parentTopic.Parent.IndexOf(parentTopic) + 1,
                        topic);
                }

            tvContent.SelectedNode = tnNew;
        }

        /// <summary>
        /// Add all topic files found in the selected folder
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void AddAllTopicsInFolder_Click(object sender, EventArgs e)
        {
            ToolStripItem tiAdd = (ToolStripItem)sender;
            TopicCollection parent, newTopics = new TopicCollection(null);
            Topic selectedTopic;
            int idx;

            using(FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select a folder to add all of its content";
                dlg.SelectedPath = Directory.GetCurrentDirectory();

                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        newTopics.AddTopicsFromFolder(dlg.SelectedPath,
                            dlg.SelectedPath, topics.FileItem.ProjectElement.Project);
                        MainForm.Host.ProjectExplorer.RefreshProject();
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }

                if(newTopics.Count != 0)
                {
                    if(tiAdd.Owner != cmsNewChildTopic)
                    {
                        // Insert as siblings
                        if(tvContent.SelectedNode == null)
                        {
                            parent = topics;
                            idx = 0;
                        }
                        else
                        {
                            selectedTopic = (Topic)tvContent.SelectedNode.Tag;
                            parent = selectedTopic.Parent;
                            idx = parent.IndexOf(selectedTopic) + 1;
                        }

                        foreach(Topic t in newTopics)
                        {
                            parent.Insert(idx, t);
                            idx++;
                        }
                    }
                    else    // Insert as children
                    {
                        parent = ((Topic)tvContent.SelectedNode.Tag).Subtopics;

                        foreach(Topic t in newTopics)
                            parent.Add(t);
                    }

                    // Take the easy way out and reload the tree
                    this.LoadTopics(newTopics[0]);
                }
            }
        }

        /// <summary>
        /// Associate a topic file with the selected node
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void miAssociateTopic_Click(object sender, EventArgs e)
        {
            FileItem newItem;
            Topic topic = (Topic)tvContent.SelectedNode.Tag;
            string newPath, projectPath = Path.GetDirectoryName(
                topics.FileItem.ProjectElement.Project.Filename);

            using(OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Select the conceptual topic file";
                dlg.Filter = "Conceptual Topics (*.aml)|*.aml|HTML Files " +
                    "(*.htm, *.html)|*.html;*.htm|All files (*.*)|*.*";
                dlg.DefaultExt = "aml";
                dlg.InitialDirectory = Directory.GetCurrentDirectory();

                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    // The file must reside under the project path
                    newPath = dlg.FileName;

                    if(!Path.GetDirectoryName(newPath).StartsWith(projectPath,
                      StringComparison.OrdinalIgnoreCase))
                        newPath = Path.Combine(projectPath, Path.GetFileName(newPath));

                    // Add the file to the project if not already there
                    newItem = topics.FileItem.ProjectElement.Project.AddFileToProject(
                        dlg.FileName, newPath);

                    topic.TopicFile = new TopicFile(newItem);
                    pgProps.Refresh();
                    this.topics_ListChanged(this, new ListChangedEventArgs(
                        ListChangedType.ItemChanged, -1));

                    MainForm.Host.ProjectExplorer.RefreshProject();
                }
            }
        }

        /// <summary>
        /// Clear the topic associated with the selected node
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        /// <remarks>When cleared, the node becomes an container node</remarks>
        private void miClearTopic_Click(object sender, EventArgs e)
        {
            Topic topic = (Topic)tvContent.SelectedNode.Tag;

            if(MessageBox.Show("Do you want to clear the topic associated " +
              "with this node?", Constants.AppName, MessageBoxButtons.YesNo,
              MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
              DialogResult.Yes)
            {
                topic.TopicFile = null;
                pgProps.Refresh();
                this.topics_ListChanged(this, new ListChangedEventArgs(
                    ListChangedType.ItemChanged, -1));
            }
        }

        /// <summary>
        /// Refresh the topic file associations to reflect changes made to the
        /// project elsewhere (i.e. in the Project Explorer).
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void miRefreshAssociations_Click(object sender, EventArgs e)
        {
            topics.MatchProjectFilesToTopics();
            pgProps.Refresh();
        }

        /// <summary>
        /// Delete a content item
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void tsbDeleteTopic_Click(object sender, EventArgs e)
        {
            TreeNode tn = tvContent.SelectedNode;
            Topic topic = (Topic)tn.Tag;

            if(MessageBox.Show(String.Format(CultureInfo.CurrentCulture,
              "Are you sure you want to delete the content item '{0}' and " +
              "all of its sub-items?", tn.Text), Constants.AppName,
              MessageBoxButtons.YesNo, MessageBoxIcon.Question,
              MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                topic.Parent.Remove(topic);
                tn.Remove();
            }

            this.UpdateControlStatus();
        }

        /// <summary>
        /// Copy as a link to the Windows clipboard for pasting into a topic
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void miCopyAsLink_Click(object sender, EventArgs e)
        {
            DataObject msg;
            TreeNode selectedNode = tvContent.SelectedNode;

            if(selectedNode != null)
            {
                // The topic is too complex to serialize to the Windows
                // clipboard so we'll just serialize a static delegate that
                // returns this object instead.
                copyClipboard = selectedNode.Tag;

                msg = new DataObject();
                msg.SetData(typeof(ClipboardDataHandler),
                    new ClipboardDataHandler(GetClipboardData));
                Clipboard.SetDataObject(msg, false);
            }
        }

        /// <summary>
        /// This is used to return the actual object to paste from the Windows
        /// clipboard
        /// </summary>
        /// <returns>The object to paste</returns>
        private static object GetClipboardData()
        {
            return copyClipboard;
        }

        /// <summary>
        /// Cut or copy the selected node to the internal clipboard
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        /// <remarks><b>NOTE:</b> Copying within the file is currently not
        /// enabled.  Copying would require cloning the topic, its children,
        /// and all related properties.  Copying a topic may not be of any use
        /// anyway since the IDs have to be unique.</remarks>
        private void tsbCutCopy_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = tvContent.SelectedNode;

            if(selectedNode != null)
            {
                // The topic is too complex to serialize to the Windows
                // clipboard so we'll just use a class member instead.
                cutClipboard = selectedNode.Tag;

                if(sender == tsbCut || sender == miCut)
                {
                    Topic t = (Topic)cutClipboard;
                    t.Parent.Remove(t);
                    selectedNode.Remove();
                }

                this.UpdateControlStatus();
            }
        }

        /// <summary>
        /// Paste the node from the internal clipboard
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void tsbPaste_ButtonClick(object sender, EventArgs e)
        {
            TreeNode target = tvContent.SelectedNode;
            Topic targetTopic, newTopic = null;

            if(cutClipboard != null)
                newTopic = cutClipboard as Topic;

            // Don't allow pasting multiple copies of the same item in here
            cutClipboard = null;

            if(newTopic != null)
            {
                if(target == null)
                    topics.Add(newTopic);
                else
                {
                    targetTopic = (Topic)target.Tag;

                    if(sender == miPasteAsChild || sender == tsmiPasteAsChild)
                        targetTopic.Subtopics.Add(newTopic);
                    else
                        targetTopic.Parent.Insert(targetTopic.Parent.IndexOf(
                            targetTopic) + 1, newTopic);
                }

                // Trying to synch up the new nodes with new tree nodes would
                // be difficult so we'll take the easy way out and reload
                // the tree.
                this.LoadTopics(newTopic);
            }
        }

        /// <summary>
        /// Edit the selected topic file
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void tsbEditTopic_Click(object sender, EventArgs e)
        {
            Topic t = (Topic)tvContent.SelectedNode.Tag;

            if(t.TopicFile != null)
            {
                string fullName = t.TopicFile.FullPath;

                // If the document is already open, just activate it
                foreach(IDockContent content in this.DockPanel.Documents)
                    if(String.Compare(content.DockHandler.ToolTipText, fullName,
                      true, CultureInfo.CurrentCulture) == 0)
                    {
                        content.DockHandler.Activate();
                        return;
                    }

                if(File.Exists(fullName))
                {
                    TopicEditorWindow editor = new TopicEditorWindow(fullName);
                    editor.Show(this.DockPanel);
                }
                else
                    MessageBox.Show("File does not exist: " + fullName,
                        Constants.AppName, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("No file is associated with this entry",
                    Constants.AppName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        /// <summary>
        /// Sort the topics by display title
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void miSortTopics_Click(object sender, EventArgs e)
        {
            Topic t = tvContent.SelectedNode.Tag as Topic;

            if(t != null)
            {
                t.Parent.Sort();
                this.LoadTopics(t);
            }
        }

        /// <summary>
        /// View help for this editor
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void tsbHelp_Click(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location);

            try
            {
#if DEBUG
                path += @"\..\..\..\Doc\Help\SandcastleBuilder.chm";
#else
                path += @"\SandcastleBuilder.chm";
#endif

                Form form = new Form();
                form.CreateControl();
                Help.ShowHelp(form, path, HelpNavigator.Topic,
                    "html/54e3dc97-5125-441e-8e84-7f9303e95f26.htm");
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                MessageBox.Show(String.Format(CultureInfo.CurrentCulture,
                    "Unable to open help file '{0}'.  Reason: {1}",
                    path, ex.Message), Constants.AppName,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        #endregion

        #region Find ID textbox drag and drop event handlers
        //=====================================================================

        /// <summary>
        /// This displays the drop cursor when the mouse drags into
        /// the Find textbox.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void txtFindId_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// This handles the drop operation for the Find textbox
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void txtFindId_DragDrop(object sender, DragEventArgs e)
        {
            if(!e.Data.GetDataPresent(DataFormats.Text))
                return;

            txtFindId.Text = (string)e.Data.GetData(DataFormats.Text);
            txtFindId.Focus();
        }
        #endregion

        #region TreeView drag and drop
        /// <summary>
        /// This initiates drag and drop for the tree view nodes
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void tvContent_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DataObject data = new DataObject();
            TreeNode node = e.Item as TreeNode;

            if(node != null && e.Button == MouseButtons.Left)
            {
                // The tree supports dragging and dropping of nodes
                data.SetData(typeof(TreeNode), node);
                data.SetData(typeof(Topic), node.Tag);

                this.DoDragDrop(data, DragDropEffects.All);
            }
        }

        /// <summary>
        /// This validates the drop target during the drag operation
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void tvContent_DragOver(object sender, DragEventArgs e)
        {
            TreeNode targetNode, dropNode;

            if(!e.Data.GetDataPresent(typeof(TreeNode)))
                return;

            // As the mouse moves over nodes, provide feedback to the user
            // by highlighting the node that is the current drop target.
            targetNode = tvContent.GetNodeAt(tvContent.PointToClient(
                new Point(e.X, e.Y)));

            // Select the node currently under the cursor
            if(targetNode != null && tvContent.SelectedNode != targetNode)
                tvContent.SelectedNode = targetNode;
            else
                if(targetNode == null)
                    targetNode = tvContent.SelectedNode;

            // Check that the selected node is not the dropNode and also
            // that it is not a child of the dropNode and therefore an
            // invalid target
            dropNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            // Don't allow drop from some other tree view (i.e. the
            // ones in the other content layout editors or project
            // explorer).
            if(dropNode != null && dropNode.TreeView != tvContent)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            while(targetNode != null)
            {
                if(targetNode == dropNode)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                targetNode = targetNode.Parent;
            }

            // Currently selected node is a suitable target
            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// This handles the drop operation for the tree view
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void tvContent_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode dropNode, targetNode;
            Topic movedEntry, targetEntry;
            int offset;

            if(!e.Data.GetDataPresent(typeof(TreeNode)))
                return;

            // Get the TreeNode being dragged
            dropNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            // The target node should be selected from the DragOver event
            targetNode = tvContent.SelectedNode;

            if(targetNode == null || dropNode == targetNode)
                return;

            // Remove the entry from its current location
            movedEntry = (Topic)dropNode.Tag;
            targetEntry = (Topic)targetNode.Tag;

            // If the drop node is the next sibling of the target node,
            // insert it in the target's location (swap them).
            offset = (targetNode.NextNode == dropNode) ? 0 : 1;

            movedEntry.Parent.Remove(movedEntry);
            dropNode.Remove();

            // If Shift is not held down, make it a sibling of the drop node.
            // If Shift is help down, make it a child of the drop node.
            if((e.KeyState & 4) == 0)
            {
                targetEntry.Parent.Insert(targetEntry.Parent.IndexOf(
                    targetEntry) + offset, movedEntry);

                if(targetNode.Parent == null)
                    tvContent.Nodes.Insert(tvContent.Nodes.IndexOf(
                        targetNode) + offset, dropNode);
                else
                    targetNode.Parent.Nodes.Insert(targetNode.Parent.Nodes.IndexOf(
                        targetNode) + offset, dropNode);
            }
            else
            {
                targetEntry.Subtopics.Add(movedEntry);
                targetNode.Nodes.Add(dropNode);
            }

            // Ensure that the moved node is visible to the user and select it
            dropNode.EnsureVisible();
            tvContent.SelectedNode = dropNode;
        }
        #endregion

        #region Other tree view event handlers
        //=====================================================================

        /// <summary>
        /// Update the state of the controls based on the current selection
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void tvContent_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateControlStatus();
        }

        /// <summary>
        /// This is used to select the clicked node and display the context
        /// menu when a right click occurs.  This ensures that the correct
        /// node is affected by the selected operation.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void tvContent_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt;
            TreeNode targetNode;

            if(e.Button == MouseButtons.Right)
            {
                pt = new Point(e.X, e.Y);

                if(pt == Point.Empty)
                    targetNode = tvContent.SelectedNode;
                else
                    targetNode = tvContent.GetNodeAt(pt);

                if(targetNode != null)
                    tvContent.SelectedNode = targetNode;

                cmsTopics.Show(tvContent, pt);
            }
        }

        /// <summary>
        /// Edit the node if it is double-clicked
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void tvContent_NodeMouseDoubleClick(object sender,
          TreeNodeMouseClickEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
                tsbEditTopic_Click(sender, e);
        }

        /// <summary>
        /// Handle shortcut keys in the tree view
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void tvContent_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Apps)
            {
                tvContent_MouseDown(sender, new MouseEventArgs(
                    MouseButtons.Right, 1, 0, 0, 0));
                e.Handled = e.SuppressKeyPress = true;
                return;
            }

            if(tvContent.SelectedNode != null)
                switch(e.KeyCode)
                {
                    case Keys.Delete:
                        if(!e.Shift)
                        {
                            tsbDeleteTopic_Click(sender, e);
                            e.Handled = e.SuppressKeyPress = true;
                        }
                        else
                        {
                            tsbCutCopy_Click(tsbCut, e);
                            e.Handled = e.SuppressKeyPress = true;
                        }
                        break;

                    case Keys.Insert:
                        if(e.Shift)
                        {
                            if(!e.Alt)
                                tsbPaste_ButtonClick(tsbPaste, e);
                            else
                                tsbPaste_ButtonClick(miPasteAsChild, e);

                            e.Handled = e.SuppressKeyPress = true;
                        }
                        break;

                    case Keys.U:
                        if(e.Control && tvContent.SelectedNode.PrevNode != null)
                        {
                            tsbMoveItem_Click(tsbMoveUp, e);
                            e.Handled = e.SuppressKeyPress = true;
                        }
                        break;

                    case Keys.D:
                        if(e.Control && tvContent.SelectedNode.NextNode != null)
                        {
                            tsbMoveItem_Click(tsbMoveDown, e);
                            e.Handled = e.SuppressKeyPress = true;
                        }
                        break;

                    case Keys.X:
                        if(e.Control)
                        {
                            tsbCutCopy_Click(tsbCut, e);
                            e.Handled = e.SuppressKeyPress = true;
                        }
                        break;

                    case Keys.C:
                        if(e.Control)
                        {
                            miCopyAsLink_Click(miCopyAsLink, e);
                            e.Handled = e.SuppressKeyPress = true;
                        }
                        break;

                    case Keys.V:
                        if(e.Control)
                        {
                            if(!e.Alt)
                                tsbPaste_ButtonClick(tsbPaste, e);
                            else
                                tsbPaste_ButtonClick(miPasteAsChild, e);

                            e.Handled = e.SuppressKeyPress = true;
                        }
                        break;

                    case Keys.E:
                        if(e.Control)
                        {
                            tsbEditTopic_Click(sender, e);
                            e.Handled = e.SuppressKeyPress = true;
                        }
                        break;

                    default:
                        break;
                }
        }
        #endregion
    }
}
