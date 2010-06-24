//=============================================================================
// System  : Sandcastle Help File Builder Plug-Ins
// File    : DeploymentConfigDlg.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 03/01/2008
// Note    : Copyright 2007-2008, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a form that is used to configure the settings for the
// Output Deployement plug-in.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://www.CodePlex.com/SHFB.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.5.2.0  09/24/2007  EFW  Created the code
//=============================================================================

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

using SandcastleBuilder.Utils;

namespace SandcastleBuilder.PlugIns
{
    /// <summary>
    /// This form is used to configure the settings for the
    /// <see cref="DeploymentPlugIn"/>.
    /// </summary>
    internal partial class DeploymentConfigDlg : Form
    {
        private XmlDocument config;     // The configuration

        /// <summary>
        /// This is used to return the configuration information
        /// </summary>
        public string Configuration
        {
            get { return config.OuterXml; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="currentConfig">The current XML configuration
        /// XML fragment</param>
        public DeploymentConfigDlg(string currentConfig)
        {
            XPathNavigator navigator, root;
            DeploymentLocation location;
            string value;

            InitializeComponent();

            lnkCodePlexSHFB.Links[0].LinkData = "http://SHFB.CodePlex.com";

            // Load the current settings
            config = new XmlDocument();
            config.LoadXml(currentConfig);
            navigator = config.CreateNavigator();

            root = navigator.SelectSingleNode("configuration");

            if(root.IsEmptyElement)
                return;

            value = root.GetAttribute("deleteAfterDeploy", String.Empty);

            if(!String.IsNullOrEmpty(value))
                chkDeleteAfterDeploy.Checked = Convert.ToBoolean(value,
                    CultureInfo.InvariantCulture);

            // Get HTML Help 1x deployment information
            location = DeploymentLocation.FromXPathNavigator(root, "help1x");
            txt1xTargetLocation.Text = (location.Location != null) ?
                location.Location.OriginalString : null;
            chk1xUseDefaultCredentials.Checked =
                location.UserCredentials.UseDefaultCredentials;
            txt1xUserName.Text = location.UserCredentials.UserName;
            txt1xPassword.Text = location.UserCredentials.Password;

            chk1xUseProxyServer.Checked =
                location.ProxyCredentials.UseProxyServer;
            txt1xProxyServer.Text =
                (location.ProxyCredentials.ProxyServer == null) ? null :
                location.ProxyCredentials.ProxyServer.OriginalString;
            chk1xUseProxyDefCreds.Checked =
                location.ProxyCredentials.Credentials.UseDefaultCredentials;
            txt1xProxyUserName.Text =
                location.ProxyCredentials.Credentials.UserName;
            txt1xProxyPassword.Text =
                location.ProxyCredentials.Credentials.Password;

            // Get HTML Help 2x deployment information
            location = DeploymentLocation.FromXPathNavigator(root, "help2x");
            txt2xTargetLocation.Text = (location.Location != null) ?
                location.Location.OriginalString : null;
            chk2xUseDefaultCredentials.Checked =
                location.UserCredentials.UseDefaultCredentials;
            txt2xUserName.Text = location.UserCredentials.UserName;
            txt2xPassword.Text = location.UserCredentials.Password;

            chk2xUseProxyServer.Checked =
                location.ProxyCredentials.UseProxyServer;
            txt2xProxyServer.Text =
                (location.ProxyCredentials.ProxyServer == null) ? null :
                location.ProxyCredentials.ProxyServer.OriginalString;
            chk2xUseProxyDefCreds.Checked =
                location.ProxyCredentials.Credentials.UseDefaultCredentials;
            txt2xProxyUserName.Text =
                location.ProxyCredentials.Credentials.UserName;
            txt2xProxyPassword.Text =
                location.ProxyCredentials.Credentials.Password;

            // Get website deployment information
            location = DeploymentLocation.FromXPathNavigator(root, "website");
            txtWsTargetLocation.Text = (location.Location != null) ?
                location.Location.OriginalString : null;
            chkWsUseDefaultCredentials.Checked =
                location.UserCredentials.UseDefaultCredentials;
            txtWsUserName.Text = location.UserCredentials.UserName;
            txtWsPassword.Text = location.UserCredentials.Password;

            chkWsUseProxyServer.Checked =
                location.ProxyCredentials.UseProxyServer;
            txtWsProxyServer.Text =
                (location.ProxyCredentials.ProxyServer == null) ? null :
                location.ProxyCredentials.ProxyServer.OriginalString;
            chkWsUseProxyDefCreds.Checked =
                location.ProxyCredentials.Credentials.UseDefaultCredentials;
            txtWsProxyUserName.Text =
                location.ProxyCredentials.Credentials.UserName;
            txtWsProxyPassword.Text =
                location.ProxyCredentials.Credentials.Password;
        }

        #region Helper methods
        //=====================================================================
        // Helper methods

        /// <summary>
        /// This is used to validate deployment location settings
        /// </summary>
        /// <param name="targetLocation">The target location textbox to
        /// validate</param>
        /// <param name="userName">The user name textbox to validate</param>
        /// <param name="password">The password textbox to validate</param>
        /// <param name="proxyServer">The proxy server checkbox to
        /// validate</param>
        /// <param name="proxyUserName">The proxy user name textbox to
        /// validate</param>
        /// <param name="proxyPassword">The proxy password textbox to
        /// validate</param>
        /// <param name="useDefaultCredentials">The default credentials
        /// checkbox to validate</param>
        /// <param name="useProxyServer">The proxy server checkbox to
        /// validate</param>
        /// <param name="useProxyDefCreds">The proxy default credentials
        /// checkbox to validate</param>
        /// <param name="targetUri">The target location URI</param>
        /// <param name="proxyUri">The proxy server URI</param>
        /// <returns>True if the configuration is valid, false if not</returns>
        private bool ValidateDeploymentLocationSettings(TextBox targetLocation,
            TextBox userName, TextBox password, TextBox proxyServer,
            TextBox proxyUserName, TextBox proxyPassword,
            CheckBox useDefaultCredentials, CheckBox useProxyServer,
            CheckBox useProxyDefCreds, out Uri targetUri, out Uri proxyUri)
        {
            bool isValid = true;

            targetUri = proxyUri = null;
            targetLocation.Text = targetLocation.Text.Trim();
            userName.Text = userName.Text.Trim();
            password.Text = password.Text.Trim();
            proxyServer.Text = proxyServer.Text.Trim();
            proxyUserName.Text = proxyUserName.Text.Trim();
            proxyPassword.Text = proxyPassword.Text.Trim();

            if(targetLocation.Text.Length != 0 && !Uri.TryCreate(
              targetLocation.Text, UriKind.RelativeOrAbsolute, out targetUri))
            {
                epErrors.SetError(targetLocation, "The target location does " +
                    "not appear to be valid");
                isValid = false;
            }

            if(!useDefaultCredentials.Checked)
            {
                if(userName.Text.Length == 0)
                {
                    epErrors.SetError(userName, "A user name is required if " +
                        "not using default credentials");
                    isValid = false;
                }

                if(password.Text.Length == 0)
                {
                    epErrors.SetError(password, "A password is required if " +
                        "not using default credentials");
                    isValid = false;
                }
            }

            Uri.TryCreate(proxyServer.Text, UriKind.RelativeOrAbsolute,
                out proxyUri);

            if(useProxyServer.Checked)
            {
                if(proxyServer.Text.Length == 0)
                {
                    epErrors.SetError(proxyServer, "A proxy server is " +
                        "required if one is used");
                    isValid = false;
                }
                else
                    if(proxyServer == null)
                    {
                        epErrors.SetError(proxyServer, "The proxy server " +
                            "name does not appear to be valid");
                        isValid = false;
                    }

                if(!useProxyDefCreds.Checked)
                {
                    if(proxyUserName.Text.Length == 0)
                    {
                        epErrors.SetError(proxyUserName, "A user name is " +
                            "required if not using default credentials");
                        isValid = false;
                    }

                    if(proxyPassword.Text.Length == 0)
                    {
                        epErrors.SetError(proxyPassword, "A password is " +
                            "required if not using default credentials");
                        isValid = false;
                    }
                }
            }

            return isValid;
        }
        #endregion

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
        /// Go to the CodePlex home page of the Sandcastle Help File Builder
        /// project.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void codePlex_LinkClicked(object sender,
          LinkLabelLinkClickedEventArgs e)
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

        /// <summary>
        /// Enable or disable the user name and password controls based on
        /// the Default Credentials check state.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void UseDefaultCredentials_CheckedChanged(object sender,
            EventArgs e)
        {
            if(sender == chk1xUseDefaultCredentials)
                txt1xUserName.Enabled = txt1xPassword.Enabled =
                    !chk1xUseDefaultCredentials.Checked;
            else
                if(sender == chk2xUseDefaultCredentials)
                    txt2xUserName.Enabled = txt2xPassword.Enabled =
                        !chk2xUseDefaultCredentials.Checked;
                else
                    txtWsUserName.Enabled = txtWsPassword.Enabled =
                        !chkWsUseDefaultCredentials.Checked;
        }

        /// <summary>
        /// Enable or disable the proxy server settings based on the Use
        /// Proxy Server check state.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void UseProxyServer_CheckedChanged(object sender, EventArgs e)
        {
            if(sender == chk1xUseProxyServer)
            {
                txt1xProxyServer.Enabled = chk1xUseProxyDefCreds.Enabled =
                    txt1xProxyUserName.Enabled = txt1xProxyPassword.Enabled =
                    chk1xUseProxyServer.Checked;

                if(chk1xUseProxyServer.Checked)
                    UseProxyDefCreds_CheckedChanged(chk1xUseProxyDefCreds, e);
            }
            else
                if(sender == chk2xUseProxyServer)
                {
                    txt2xProxyServer.Enabled = chk2xUseProxyDefCreds.Enabled =
                        txt2xProxyUserName.Enabled = txt2xProxyPassword.Enabled =
                        chk2xUseProxyServer.Checked;

                    if(chk2xUseProxyServer.Checked)
                        UseProxyDefCreds_CheckedChanged(chk2xUseProxyDefCreds, e);
                }
                else
                {
                    txtWsProxyServer.Enabled = chkWsUseProxyDefCreds.Enabled =
                        txtWsProxyUserName.Enabled = txtWsProxyPassword.Enabled =
                        chkWsUseProxyServer.Checked;

                    if(chkWsUseProxyServer.Checked)
                        UseProxyDefCreds_CheckedChanged(chkWsUseProxyDefCreds, e);
                }
        }

        /// <summary>
        /// Enable or disable the proxy user credentials based on the Proxy
        /// Default Credentials check state.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void UseProxyDefCreds_CheckedChanged(object sender, EventArgs e)
        {
            if(sender == chk1xUseProxyDefCreds)
            {
                if(chk1xUseProxyDefCreds.Enabled)
                    txt1xProxyUserName.Enabled = txt1xProxyPassword.Enabled =
                        !chk1xUseProxyDefCreds.Checked;
            }
            else
                if(sender == chk2xUseProxyDefCreds)
                {
                    if(chk2xUseProxyDefCreds.Enabled)
                        txt2xProxyUserName.Enabled = txt2xProxyPassword.Enabled =
                            !chk2xUseProxyDefCreds.Checked;
                }
                else
                    if(chkWsUseProxyDefCreds.Enabled)
                        txtWsProxyUserName.Enabled = txtWsProxyPassword.Enabled =
                            !chkWsUseProxyDefCreds.Checked;
        }

        /// <summary>
        /// Validate the configuration and save it
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            XmlAttribute attr;
            XmlNode root;
            DeploymentLocation location;
            UserCredentials userCreds;
            ProxyCredentials proxyCreds;
            Uri targetUri1x, proxyUri1x, targetUri2x = null, proxyUri2x = null,
                targetUriWs = null, proxyUriWs = null;
            bool isValid = true;

            epErrors.Clear();

            isValid = this.ValidateDeploymentLocationSettings(
                txt1xTargetLocation, txt1xUserName, txt1xPassword,
                txt1xProxyServer, txt1xProxyUserName, txt1xProxyPassword,
                chk1xUseDefaultCredentials, chk1xUseProxyServer,
                chk1xUseProxyDefCreds, out targetUri1x, out proxyUri1x);

            if(!isValid)
                tabConfig.SelectedIndex = 0;
            else
            {
                isValid = this.ValidateDeploymentLocationSettings(
                    txt2xTargetLocation, txt2xUserName, txt2xPassword,
                    txt2xProxyServer, txt2xProxyUserName, txt2xProxyPassword,
                    chk2xUseDefaultCredentials, chk2xUseProxyServer,
                    chk2xUseProxyDefCreds, out targetUri2x, out proxyUri2x);

                if(!isValid)
                    tabConfig.SelectedIndex = 1;
                else
                {
                    isValid = this.ValidateDeploymentLocationSettings(
                        txtWsTargetLocation, txtWsUserName, txtWsPassword,
                        txtWsProxyServer, txtWsProxyUserName, txtWsProxyPassword,
                        chkWsUseDefaultCredentials, chkWsUseProxyServer,
                        chkWsUseProxyDefCreds, out targetUriWs, out proxyUriWs);

                    if(!isValid)
                        tabConfig.SelectedIndex = 2;
                }
            }

            if(isValid && targetUri1x == null && targetUri2x == null &&
              targetUriWs == null)
            {
                tabConfig.SelectedIndex = 0;
                epErrors.SetError(txt1xTargetLocation, "At least one help " +
                    "file format must have a target location specified");
                isValid = false;
            }

            if(!isValid)
                return;

            // Store the changes
            root = config.SelectSingleNode("configuration");
            attr = root.Attributes["deleteAfterDeploy"];

            if(attr == null)
            {
                attr = config.CreateAttribute("deleteAfterDeploy");
                root.Attributes.Append(attr);
            }

            attr.Value = chkDeleteAfterDeploy.Checked.ToString().ToLower(
                CultureInfo.InvariantCulture);

            userCreds = new UserCredentials(chk1xUseDefaultCredentials.Checked,
                txt1xUserName.Text, txt1xPassword.Text);
            proxyCreds = new ProxyCredentials(chk1xUseProxyServer.Checked,
                proxyUri1x, new UserCredentials(chk1xUseProxyDefCreds.Checked,
                txt1xProxyUserName.Text, txt1xProxyPassword.Text));
            location = new DeploymentLocation(targetUri1x, userCreds,
                proxyCreds);
            location.ToXml(config, root, "help1x");

            userCreds = new UserCredentials(chk2xUseDefaultCredentials.Checked,
                txt2xUserName.Text, txt2xPassword.Text);
            proxyCreds = new ProxyCredentials(chk2xUseProxyServer.Checked,
                proxyUri2x, new UserCredentials(chk2xUseProxyDefCreds.Checked,
                txt2xProxyUserName.Text, txt2xProxyPassword.Text));
            location = new DeploymentLocation(targetUri2x, userCreds,
                proxyCreds);
            location.ToXml(config, root, "help2x");

            userCreds = new UserCredentials(chkWsUseDefaultCredentials.Checked,
                txtWsUserName.Text, txtWsPassword.Text);
            proxyCreds = new ProxyCredentials(chkWsUseProxyServer.Checked,
                proxyUriWs, new UserCredentials(chkWsUseProxyDefCreds.Checked,
                txtWsProxyUserName.Text, txtWsProxyPassword.Text));
            location = new DeploymentLocation(targetUriWs, userCreds,
                proxyCreds);
            location.ToXml(config, root, "website");

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
