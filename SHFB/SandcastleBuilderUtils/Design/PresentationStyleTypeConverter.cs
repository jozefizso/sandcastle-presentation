//=============================================================================
// System  : EWSoftware Design Time Attributes and Editors
// File    : PresenationStyleTypeConverter.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 09/17/2007
// Note    : Copyright 2007, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a type converter that allows you to select a presentation
// style folder from those currently installed in the .\Presentation folder
// found in the main installation folder of Sandcastle.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://www.CodePlex.com/SHFB.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0.0  08/08/2006  EFW  Created the code
// 1.5.0.0  06/19/2007  EFW  Updated for use with the June CTP
//=============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using SandcastleBuilder.Utils;
using SandcastleBuilder.Utils.BuildEngine;
using System.Collections.ObjectModel;

namespace SandcastleBuilder.Utils.Design
{
    /// <summary>
    /// This type converter allows you to select a presentation style folder
    /// from those currently installed in the <b>.\Presentation</b> folder
    /// found in the main installation folder of Sandcastle.
    /// </summary>
    internal class PresentationStyleTypeConverter : StringConverter
    {
        //=====================================================================
        // Private data members

		private static List<PresentationStyleInfo> styles = new List<PresentationStyleInfo>();
        private static StandardValuesCollection standardValues = null;
            //PresentationStyleTypeConverter.InitializeStandardValues();

		//private StandardValuesCollection standardValues = null;

        //=====================================================================
        // Properties

        /// <summary>
        /// This returns the default style
        /// </summary>
        /// <value>Returns <b>vs2005</b> if present.  If not, it returns the
        /// first best match or, failing that, the first style in the list.</value>
        public static PresentationStyleInfo DefaultStyle
        {
            get
            {
				string dxroot = Environment.GetEnvironmentVariable("DXROOT");

				var folder = new PresentationStyleInfo();
				folder.Name = "vs2005";
				folder.Path = Path.Combine(dxroot, @"Presentation\vs2005");

				//string defaultStyle = "vs2005";

				//if(!IsPresent(defaultStyle))
				//    defaultStyle = FirstMatching(defaultStyle);

				return folder;
            }
        }

        //=====================================================================
        // Methods

        /// <summary>
        /// This is used to get the standard values by searching for the
        /// .NET Framework versions installed on the current system.
        /// </summary>
		private static void InitializeStandardValues(ITypeDescriptorContext context)
		{
			if (standardValues != null)
				return;

			if (context == null)
				return;

			SandcastleProject proj = context.Instance as SandcastleProject;
			if (proj == null)
				throw new ArgumentException("PresentationStyleTypeConverter requires context.Instance "+
					"property to be set to SandcastleProject object.", "context");

			standardValues = InitializeStandardValues(proj.PresentationStylePaths);
		}

		private static StandardValuesCollection InitializeStandardValues(Collection<PresentationStyleFolder> folders)
		{
			var styles = new List<PresentationStyleInfo>();

			foreach(PresentationStyleFolder folder in folders)
			{
				string path = Environment.ExpandEnvironmentVariables(folder.Path);

				AddPresentationsFromFolder(path, styles);
			}

			return new StandardValuesCollection(styles);
		}

		private static void AddPresentationsFromFolder(string folder, IList<PresentationStyleInfo> styles)
		{
			if (!Directory.Exists(folder))
				return;

			string[] dirs = Directory.GetDirectories(folder);
			
			// The Shared folder is omitted as it contains files
			// common to all presentation styles.
			foreach (string s in dirs)
			{
				if (!s.EndsWith("Shared", StringComparison.OrdinalIgnoreCase))
				{
					var di = new DirectoryInfo(s);

					var ps = new PresentationStyleInfo()
					{
						Name = s.Substring(s.LastIndexOf('\\') + 1),
						Path = di.FullName,
					};

					styles.Add(ps);
				}
			}
		}

        /// <summary>
        /// This is overridden to return the values for the type converter's
        /// dropdown list.
        /// </summary>
        /// <param name="context">The format context object</param>
        /// <returns>Returns the standard values for the type</returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
			InitializeStandardValues(context);
			return standardValues;
        }

        /// <summary>
        /// This is overridden to indicate that the values are exclusive
        /// and values outside the list cannot be entered.
        /// </summary>
        /// <param name="context">The format context object</param>
        /// <returns>Always returns true</returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// This is overridden to indicate that standard values are supported
        /// and can be chosen from a list.
        /// </summary>
        /// <param name="context">The format context object</param>
        /// <returns>Always returns true</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// This is used to find out if the specified style is present on the
        /// system.
        /// </summary>
        /// <param name="style">The style for which to look</param>
        /// <returns>True if present, false if not found</returns>
        public static bool IsPresent(string style)
        {
			return styles.Exists(s => s.Name == style);
        }

        /// <summary>
        /// This is used to get the first style that matches case-insensitively
        /// or, failing that, starts with or contains the given value
        /// case-insensitively.
        /// </summary>
        /// <param name="style">The style for which to look</param>
        /// <returns>The best match or the first style if not found.</returns>
        public static PresentationStyleInfo FirstMatching(string styleName)
        {
            if(!String.IsNullOrEmpty(styleName))
            {
				var q = from s in styles
						where s.Name == styleName
						select s;

				var style = q.FirstOrDefault();

				if (style != null)
					return style;
            }

            // Not found, return the first style
            return styles[0];
        }
    }
}
