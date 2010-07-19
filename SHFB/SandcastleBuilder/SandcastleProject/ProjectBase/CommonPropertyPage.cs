/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.VisualStudio.OLE.Interop;

namespace Microsoft.VisualStudio.Project
{
    [CLSCompliant(false)]
    [ComVisible(true)]
    public abstract class CommonPropertyPage : IPropertyPage, IDisposable
    {
        #region fields
        private bool dirty;
        private ProjectNode project;
        private IPropertyPageSite site;
        private static volatile object Mutex = new object();
        private bool isDisposed;
        #endregion

        protected CommonPropertyPage()
        {
        }
        
        #region properties

        public abstract Control Control { get; }

        public bool IsDirty
        {
            get
            {
                return this.dirty;
            }
            set
            {
                if (this.dirty != value)
                {
                    this.dirty = value;
                    if (this.site != null)
                    {
                        this.site.OnStatusChange((uint)(this.dirty ? PropPageStatus.Dirty : PropPageStatus.Clean));
                    }
                }
            }
        }

        public string Name { get; set; }

        public ProjectNode ProjectMgr
        {
            get
            {
                return this.project;
            }
        }

        #endregion

        #region abstract methods

        protected abstract int ApplyChanges();
        protected abstract void BindProperties();

        #endregion

        #region IPropertyPage members

        void IPropertyPage.Activate(IntPtr hWndParent, RECT[] pRect, int bModal)
        {
            NativeMethods.SetParent(this.Control.Handle, hWndParent);
        }

        int IPropertyPage.Apply()
        {
            if (!IsDirty)
                return VSConstants.S_OK;

            try
            {
                return this.ApplyChanges();
            }
            catch (Exception exception)
            {
                Debug.Fail(
                    "Exception occured while applying changes to property page.",
                    exception.Message);
                return Marshal.GetHRForException(exception);
            }
        }

        void IPropertyPage.Deactivate()
        {
            this.Control.Dispose();
        }

        void IPropertyPage.GetPageInfo(PROPPAGEINFO[] pPageInfo)
        {
            if (pPageInfo == null)
            {
                throw new ArgumentNullException("pPageInfo");
            }
            PROPPAGEINFO proppageinfo = new PROPPAGEINFO
            {
                cb = (uint)Marshal.SizeOf(typeof(PROPPAGEINFO)),
                dwHelpContext = 0,
                pszDocString = null,
                pszHelpFile = null,
                pszTitle = this.Name
            };
            proppageinfo.SIZE.cx = this.Control.Width;
            proppageinfo.SIZE.cy = this.Control.Height;
            pPageInfo[0] = proppageinfo;
        }

        void IPropertyPage.Help(string helpDir)
        {
        }

        int IPropertyPage.IsPageDirty()
        {
            return (IsDirty ? VSConstants.S_OK : VSConstants.S_FALSE);
        }

        void IPropertyPage.Move(RECT[] arrRect)
        {
            if (arrRect == null)
            {
                throw new ArgumentNullException("arrRect");
            }
            RECT rect = arrRect[0];
            this.Control.Location = new Point(rect.left, rect.top);
            this.Control.Size = new Size(rect.right - rect.left, rect.bottom - rect.top);
        }

        void IPropertyPage.SetObjects(uint count, object[] punk)
        {
            if (punk == null)
            {
                return;
            }
            if (count > 0)
            {
                if (punk[0] is ProjectConfig)
                {
                    ArrayList configs = new ArrayList();
                    for (int i = 0; i < count; i++)
                    {
                        ProjectConfig config = (ProjectConfig)punk[i];
                        if (this.project == null)
                        {
                            this.project = config.ProjectMgr;
                            break;
                        }
                        configs.Add(config);
                    }
                }
                else if ((punk[0] is NodeProperties) && (this.project == null))
                {
                    this.project = (punk[0] as NodeProperties).Node.ProjectMgr;
                }
            }
            else
            {
                this.project = null;
            }

            if (this.project != null)
            {
                this.UpdateObjects();
            }
        }

        void IPropertyPage.SetPageSite(IPropertyPageSite pPageSite)
        {
            this.site = pPageSite;
        }

        void IPropertyPage.Show(uint nCmdShow)
        {
            this.Control.Visible = true;
            this.Control.Show();
        }

        int IPropertyPage.TranslateAccelerator(MSG[] pMsg)
        {
            if (pMsg == null)
            {
                throw new ArgumentNullException("arrMsg");
            }
            MSG msg = pMsg[0];
            if ((msg.message < NativeMethods.WM_KEYFIRST || msg.message > NativeMethods.WM_KEYLAST) && (msg.message < NativeMethods.WM_MOUSEFIRST || msg.message > NativeMethods.WM_MOUSELAST))
            {
                return 1;
            }

            return (NativeMethods.IsDialogMessageA(this.Control.Handle, ref msg) ? 0 : 1);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected void UpdateObjects()
        {
            try
            {
                this.BindProperties();
            }
            catch (Exception exception)
            {
                Debug.Fail(
                    "Exception occured while loading settings on property page.",
                    exception.Message);
                throw;
            }
        }

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                lock (Mutex)
                {
                    if (disposing)
                    {
                        this.Control.Dispose();
                    }

                    this.isDisposed = true;
                }
            }
        }
    }


}
