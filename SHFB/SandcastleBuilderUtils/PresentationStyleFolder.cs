using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SandcastleBuilder.Utils
{
	/// <summary>
	/// This represents a folder where presentation styles will be looked for.
	/// </summary>
	/// <remarks>
	/// Default path where presentation styles are looked for is %DXROOT%\Presentation.
	/// </remarks>
	public class PresentationStyleFolder : PropertyBasedCollectionItem
	{
		private string path;

		internal PresentationStyleFolder(string path, SandcastleProject project)
			: base(project)
		{
			this.path = path;
		}

		public string Path
		{
			get { return path; }
			set
			{
				base.CheckProjectIsEditable();

				this.path = value;
			}
		}
	}
}
