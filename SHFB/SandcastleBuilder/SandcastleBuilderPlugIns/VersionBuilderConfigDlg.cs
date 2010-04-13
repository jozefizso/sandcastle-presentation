//=============================================================================
// System  : EWSoftware Design Time Attributes and Editors
// File    : VersionBuilderConfigDlg.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 08/13/2008
// Note    : Copyright 2007-2008, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the form used to edit the version builder plug-in
// configuration.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://SHFB.CodePlex.com.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.6.0.3  12/01/2007  EFW  Created the code
// 1.8.0.0  08/13/2008  EFW  Updated to support the new project format
//=============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

using SandcastleBuilder.Utils;

namespace SandcastleBuilder.PlugIns
{
    /// <summary>
    /// This form is used to edit the <see cref="VersionBuilderPlugIn"/>
    /// configuration.
    /// </summary>
    internal partial class VersionBuilderConfigDlg : Form
    {
        #region Private data members
        //=====================================================================

        private VersionSettingsCollection items;
        private XmlDocument config;     // The configuration
        private SandcastleProject project;
        #endregion

        #region Properties
        //=====================================================================

        /// <summary>
        /// This is used to return the configuration information
        /// </summary>
        public string Configuration
        {
            get { return config.OuterXml; }
        }
        #endregion

        #region Constructor
        //=====================================================================

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="currentProject">The current project</param>
        /// <param name="currentConfig">The current XML configuration
        /// XML fragment</param>
        public VersionBuilderConfigDlg(SandcastleProject currentProject,
          string currentConfig)
        {
            XPathNavigator navigator, root, node;

            InitializeComponent();
            project = currentProject;

            lnkCodePlexSHFB.Links[0].LinkData = "http://SHFB.CodePlex.com";
            lbVersionInfo.DisplayMember = lbVersionInfo.ValueMember =
                "ListDescription";

            items = new VersionSettingsCollection();

            // Load the current settings
            config = new XmlDocument();
            config.LoadXml(currentConfig);
            navigator = config.CreateNavigator();

            root = navigator.SelectSingleNode("configuration");

            if(root.IsEmptyElement)
                return;

            node = root.SelectSingleNode("currentProject");
            if(node != null)
            {
                txtLabel.Text = node.GetAttribute("label", String.Empty);
                txtVersion.Text = node.GetAttribute("version", String.Empty);
            }

            items.FromXml(currentProject, root);

            if(items.Count == 0)
                pgProps.Enabled = btnDelete.Enabled = false;
            else
            {
                // Binding the collection to the list box caused some
                // odd problems with the property grid so we'll add the
                // items to the list box directly.
                foreach(VersionSettings vs in items)
                    lbVersionInfo.Items.Add(vs);

                lbVersionInfo.SelectedIndex = 0;
            }
        }
        #endregion

        #region Event handlers
        //=====================================================================

        /// <summary>
        /// Close without saving
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Add a new help file builder project to the version settings.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void btnAddFile_Click(object sender, EventArgs e)
        {
            VersionSettings newItem;
            int idx = 0;

            using(OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Select the help file builder project(s)";
                dlg.Filter = "Sandcastle Help File Builder Project Files " +
                    "(*.shfbproj)|*.shfbproj|All Files (*.*)|*.*";
                dlg.InitialDirectory = Directory.GetCurrentDirectory();
                dlg.DefaultExt = "shfbproj";
                dlg.Multiselect = true;

                // If selected, add the file(s)
                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    foreach(string file in dlg.FileNames)
                    {
                        newItem = new VersionSettings();
                        newItem.HelpFileProject = new FilePath(file, project);

                        // It will end up on the last one added
                        idx = lbVersionInfo.Items.Add(newItem);
                    }

                    pgProps.Enabled = btnDelete.Enabled = true;
                    lbVersionInfo.SelectedIndex = idx;
                }
            }
        }

        /// <summary>
        /// Delete a version settings item
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int idx = lbVersionInfo.SelectedIndex;

            if(idx == -1)
                lbVersionInfo.SelectedIndex = 0;
            else
            {
                lbVersionInfo.Items.RemoveAt(idx);

                if(lbVersionInfo.Items.Count == 0)
                    pgProps.Enabled = btnDelete.Enabled = false;
                else
                    if(idx < lbVersionInfo.Items.Count)
                        lbVersionInfo.SelectedIndex = idx;
                    else
                        lbVersionInfo.SelectedIndex =
                            lbVersionInfo.Items.Count - 1;
            }
        }

        /// <summary>
        /// Update the property grid with the selected item
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void lbVersionInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lbVersionInfo.SelectedItem != null)
            {
                VersionSettings vs = (VersionSettings)lbVersionInfo.SelectedItem;
                pgProps.SelectedObject = vs;
            }
            else
                pgProps.SelectedObject = null;

            pgProps.Refresh();
        }

        /// <summary>
        /// Refresh the list box item when a property changes
        /// </summary>
        /// <param name="s">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void pgProps_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            lbVersionInfo.Refresh(lbVersionInfo.SelectedIndex);
        }

        /// <summary>
        /// Validate the configuration and save it
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> projects = new Dictionary<int, string>();
            VersionSettings vs;
            XmlAttribute attr;
            XmlNode root, node;
            string hash;
            bool isValid = true;

            txtLabel.Text = txtLabel.Text.Trim();
            txtVersion.Text = txtVersion.Text.Trim();
            epErrors.Clear();
            epErrors.SetIconAlignment(lbVersionInfo,
                ErrorIconAlignment.BottomRight);

            if(txtVersion.Text.Length == 0)
            {
                epErrors.SetError(txtVersion, "A version for the containing " +
                    "project is required");
                isValid = false;
            }

            items.Clear();

            hash = txtLabel.Text + txtVersion.Text;
            projects.Add(hash.GetHashCode(), "@CurrentProject");

            for(int idx = 0; idx < lbVersionInfo.Items.Count; idx++)
            {
                vs = (VersionSettings)lbVersionInfo.Items[idx];

                // There can't be duplicate IDs or projects
                if(projects.ContainsKey(vs.GetHashCode()) ||
                  projects.ContainsValue(vs.HelpFileProject))
                {
                    epErrors.SetError(lbVersionInfo, "Label + Version values " +
                        "and project filenames must be unique");
                    isValid = false;
                    break;
                }

                if(vs.Version == txtVersion.Text)
                {
                    epErrors.SetError(lbVersionInfo, "A prior version cannot " +
                        "match the current project's version");
                    isValid = false;
                    break;
                }

                items.Add(vs);
                projects.Add(vs.GetHashCode(), vs.HelpFileProject);
            }

            if(!isValid)
                return;

            // Store the changes
            root = config.SelectSingleNode("configuration");

            node = root.SelectSingleNode("currentProject");
            if(node == null)
            {
                node = config.CreateNode(XmlNodeType.Element,
                    "currentProject", null);
                root.AppendChild(node);

                attr = config.CreateAttribute("label");
                node.Attributes.Append(attr);
                attr = config.CreateAttribute("version");
                node.Attributes.Append(attr);
            }

            node.Attributes["label"].Value = txtLabel.Text;
            node.Attributes["version"].Value = txtVersion.Text;
            items.ToXml(config, root);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Launch the URL in the web browser
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void lnkCodePlexSHFB_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start((string)e.Link.LinkData);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                MessageBox.Show("Unable to launch link target.  " +
                    "Reason: " + ex.Message, Constants.AppName,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        #endregion
    }
}
