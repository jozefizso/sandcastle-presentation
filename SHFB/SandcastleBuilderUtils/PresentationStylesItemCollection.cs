//=============================================================================
// System  : Sandcastle Help File Builder Utilities
// File    : PresentationStylesItemCollection.cs
// Author  : Jozef Izso (jozef.izso@gmail.com)
// Updated : 12/04/2009
// Note    : Copyright 2009, Jozef Izso, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a collection class used to hold the references to folders
// with Sandcastle presentation styles.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://www.CodePlex.com/SHFB.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.8.0.0  12/04/2009  JIZ  
//=============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Microsoft.Build.BuildEngine;
using System.Xml;
using System.IO;

namespace SandcastleBuilder.Utils
{
	public class PresentationStylesItemCollection : BindingList<PresentationStyleFolder>
	{
		public PresentationStylesItemCollection(SandcastleProject project)
		{
			this.Project = project;
		}

		public SandcastleProject Project { get; private set; }

		internal void FromXml(string value)
		{
			XmlReaderSettings settings = new XmlReaderSettings()
			{
				ConformanceLevel = ConformanceLevel.Fragment,
				IgnoreComments = true,
			};

			using(var sr = new StringReader(value))
			using (XmlReader reader = XmlReader.Create(sr, settings))
			{
				reader.MoveToContent();

				while (!reader.EOF)
				{
					if (reader.NodeType == XmlNodeType.Element
						&& reader.LocalName == "Folder")
					{
						string path = reader.ReadElementContentAsString();

						this.Add(new PresentationStyleFolder(path, this.Project));
					}
				}
			}
		}
	}
}
