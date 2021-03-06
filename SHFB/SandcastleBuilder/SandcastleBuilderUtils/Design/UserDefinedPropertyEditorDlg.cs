﻿//=============================================================================
// System  : EWSoftware Design Time Attributes and Editors
// File    : UserDefinedPropertyEditorDlg.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 12/06/2009
// Note    : Copyright 2008-2009, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the form used to edit the user-defined project properties
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://SHFB.CodePlex.com.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.8.0.1  12/18/2008  EFW  Created the code
//=============================================================================

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Windows.Forms;

using Microsoft.Build.Evaluation;
using Microsoft.Build.Construction;

namespace SandcastleBuilder.Utils.Design
{
    /// <summary>
    /// This form is used to edit user-defined project properties
    /// </summary>
    /// <remarks>Note that the MSBuild project object does not provide a way
    /// to delete or rename properties.  As such, you have to edit the project
    /// file by hand to do those tasks.</remarks>
    internal partial class UserDefinedPropertyEditorDlg : Form
    {
        #region Private property editor class
        //=====================================================================

        /// <summary>
        /// This is used to edit the user-defined project property items
        /// </summary>
        private sealed class PropertyItem
        {
            #region Private data members
            //=====================================================================

            private ProjectPropertyElement buildProp;
            private string name, condition, propValue;
            #endregion

            #region Properties
            //=====================================================================

            /// <summary>
            /// This is used to get or set the owning dialog
            /// </summary>
            [Browsable(false)]
            public UserDefinedPropertyEditorDlg Owner { get; set; }

            /// <summary>
            /// This is used to get or set the property name
            /// </summary>
            /// <remarks>Existing properties cannot be renamed as the MSBuild
            /// project object doesn't provide a way to do it.  Nor does it
            /// allow deleting properties.</remarks>
            [Category("Name"), Description("The property name")]
            public string Name
            {
                get { return name; }
                set
                {
                    this.Owner.CheckProjectIsEditable();

                    if(String.IsNullOrEmpty(value))
                        throw new ArgumentException("Name cannot be null or blank");

                    if(buildProp == null)
                    {
                        if(!this.Owner.Project.IsValidUserDefinedPropertyName(value))
                            throw new ArgumentException("The entered name matches " +
                                "an existing project or reserved property name");

                        if(this.Owner.UserDefinedProperties.Where(
                          p => p != this && p.Name == value).Count() != 0)
                            throw new ArgumentException("The entered name matches " +
                                "an existing user-defined property name");
                    }
                    else
                        throw new InvalidOperationException("Existing " +
                            "properties cannot be renamed via the designer");

                    name = value;
                }
            }

            /// <summary>
            /// This is used to get or set the Condition attribute value for
            /// the property.
            /// </summary>
            [Category("Value"), Description("An optional condition used to " +
              "determine when the property value is defined"),
              Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
            public string Condition
            {
                get { return condition; }
                set
                {
                    this.Owner.CheckProjectIsEditable();
                    condition = value;
                }
            }

            /// <summary>
            /// This is used to get or set the value for the property
            /// </summary>
            [Category("Value"), Description("The property value"),
              Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
            public string Value
            {
                get { return propValue; }
                set
                {
                    this.Owner.CheckProjectIsEditable();
                    propValue = value;
                }
            }
            #endregion

            #region Constructor
            //=====================================================================

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="owner">The owning dialog</param>
            /// <param name="buildProperty">The build property to edit or null
            /// for a new property</param>
            public PropertyItem(UserDefinedPropertyEditorDlg owner,
              ProjectPropertyElement buildProperty)
            {
                string newPropName;
                int idx = 1;

                this.Owner = owner;
                this.buildProp = buildProperty;

                if(buildProp != null)
                {
                    name = buildProp.Name;
                    condition = buildProp.Condition;
                    propValue = buildProp.Value;
                }
                else
                {
                    do
                    {
                        newPropName = "NewProperty" + idx.ToString(
                            CultureInfo.InvariantCulture);
                        idx++;

                    } while(!this.Owner.Project.IsValidUserDefinedPropertyName(newPropName) ||
                        this.Owner.UserDefinedProperties.Where(
                        p => p.Name == newPropName).Count() != 0);

                    name = newPropName;
                    propValue = String.Empty;
                }
            }
            #endregion

            #region Method overrides
            //=====================================================================

            /// <summary>
            /// Return the name of the build property
            /// </summary>
            public override string  ToString()
            {
                return this.Name;
            }
            #endregion

        }
        #endregion

        #region Properties
        //=====================================================================

        /// <summary>
        /// This is used to get or set the user-defined property collection
        /// </summary>
        private Collection<PropertyItem> UserDefinedProperties { get; set; }

        /// <summary>
        /// This is used to get or set the project file reference
        /// </summary>
        private SandcastleProject Project { get; set; }
        #endregion

        #region Constructor
        //=====================================================================

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="project">The project file reference</param>
        public UserDefinedPropertyEditorDlg(SandcastleProject project)
        {
            PropertyItem propItem;

            InitializeComponent();

            this.Project = project;
            this.UserDefinedProperties = new Collection<PropertyItem>();

            lbProperties.Sorted = true;

            try
            {
                foreach(ProjectProperty prop in this.Project.GetUserDefinedProperties())
                {
                    propItem = new PropertyItem(this, prop.Xml);
                    this.UserDefinedProperties.Add(propItem);
                    lbProperties.Items.Add(propItem);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unable to load user-defined properties.  " +
                    "Error " + ex.Message, Constants.AppName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            lbProperties.Sorted = false;

            if(lbProperties.Items.Count == 0)
                pgProps.Enabled = false;
            else
                lbProperties.SelectedIndex = 0;
        }
        #endregion

        #region Helper method
        //=====================================================================

        /// <summary>
        /// This is used to see if the project can be edited.  If not, abort
        /// the change by throwing an exception.
        /// </summary>
        private void CheckProjectIsEditable()
        {
            CancelEventArgs ce = new CancelEventArgs();
            this.Project.OnQueryEditProjectFile(ce);

            if(ce.Cancel)
                throw new OperationCanceledException(
                    "Project cannot be edited");

            this.Project.MarkAsDirty();
        }
        #endregion

        #region Event handlers
        //=====================================================================

        /// <summary>
        /// Close the form
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            foreach(PropertyItem item in lbProperties.Items)
            {
                var newProp = this.Project.MSBuildProject.SetProperty(item.Name, item.Value);
                newProp.Xml.Condition = item.Condition;
            }

            this.Close();
        }

        /// <summary>
        /// View help for this form
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void btnHelp_Click(object sender, EventArgs e)
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
                    "html/da405a33-3eeb-4451-9aa8-a55be5026434.htm#UserDefProps");
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

        /// <summary>
        /// This is used to add a new user-defined property to the project
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.CheckProjectIsEditable();

            PropertyItem newProp = new PropertyItem(this, null);
            this.UserDefinedProperties.Add(newProp);
            
            lbProperties.SelectedIndex = lbProperties.Items.Add(newProp);
        }

        /// <summary>
        /// Update the property grid with the selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lbProperties.SelectedItem != null)
            {
                PropertyItem item = (PropertyItem)lbProperties.SelectedItem;
                pgProps.SelectedObject = item;
                pgProps.Enabled = true;
            }
            else
            {
                pgProps.SelectedObject = null;
                pgProps.Enabled = false;
            }

            pgProps.Refresh();
        }

        /// <summary>
        /// Refresh the list box item when a property changes
        /// </summary>
        /// <param name="s">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void pgProps_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            lbProperties.Refresh(lbProperties.SelectedIndex);
        }
        #endregion
    }
}
