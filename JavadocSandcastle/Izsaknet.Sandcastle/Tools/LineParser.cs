using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Izsaknet.Sandcastle.Tools
{
	public class LineParser
	{
		private string data;

		public LineParser(string data)
		{
			this.data = data;
		}

		public IEnumerable<string> ReadLines()
		{
			using (StringReader reader = new StringReader(this.data))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
					yield return line;
			}
		}
	}
}
