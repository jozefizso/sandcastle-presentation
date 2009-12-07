using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SandcastleBuilder.Utils.Design
{
	public class PresentationStyleInfo : IComparable<PresentationStyleInfo>, IComparable
	{
		public PresentationStyleInfo()
		{
			this.Name = "";
			this.Path = "";
		}

		public string Name { get; set; }

		public string Path { get; set; }

		public override string ToString()
		{
			return this.Name;
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;

			if (!(obj is PresentationStyleInfo))
				throw new ArgumentException("Argument obj must by of type PresentationStyleInfo.", "obj");

			return this.CompareTo((PresentationStyleInfo)obj);
		}

		#endregion

		#region IComparable<PresentationStyleInfo> Members

		public int CompareTo(PresentationStyleInfo other)
		{
			if (other == null)
				return 1;

			int compare = this.Name.CompareTo(other.Name);
			if (compare == 0)
				compare = this.Path.CompareTo(other.Path);

			return compare;
		}

		#endregion
	}
}
