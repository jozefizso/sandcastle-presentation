using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell;

namespace Izsaknet.Sandcastle.VisualStudio
{
    /// <summary>
    /// <see cref="SandcastleProjectPackage"/> class implements the package exposed by
    /// this <b>SandcastleProject</b> assembly.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideProjectFactoryAttribute(
        typeof(SandcastleProjectFactory),
        null,
        "#113",
        "shfbproj",
        "shfbproj",
        @".\\NullPath",
        LanguageVsTemplate = "SandcastleProject")]
    [ProvideObject(typeof(GeneralPropertyPage))]
    [Guid(GuidList.SandcastleProjectPkgString)]
    public sealed class SandcastleProjectPackage : ProjectPackage
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public SandcastleProjectPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        #region ProjectPackage Members

        public override string ProductUserContext
        {
            get { return SandcastleConstants.SandcastleProjectUserContext; }
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            this.RegisterProjectFactory(new SandcastleProjectFactory(this));
        }
        #endregion

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the service.</typeparam>
        public T GetService<T>()
        {
            T service = (T)this.GetService(typeof(T));
            return service;
        }
    }
}
