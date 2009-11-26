using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Izsaknet.Sandcastle.Tools
{
	public class TargetIdParser
	{
		public static TargetIdParser Parse(string targetId)
		{
			if (targetId[1] != ':')
				throw new ArgumentException("TargetId has invalid format. Missing : in id.", "targetId");

			TargetIdParser parser = new TargetIdParser();

			string targetType = targetId[0].ToString();
			parser.TargetType = targetType;

			int lastDot = targetId.LastIndexOf('.');
			if (lastDot == -1)
			{
				// just name:
				switch(targetType)
				{
					case "N":
						parser.Namespace = targetId.Substring(2);
						break;
					default:
						parser.TypeName = targetId.Substring(2);
						break;
				}
			}
			else
			{
				string package = targetId.Substring(2, lastDot - 2);
				string name = targetId.Substring(lastDot+1);

				parser.Namespace = package;
				parser.TypeName = name;
			}

			return parser;
		}

		public string TargetType { get; set; }

		public string Namespace { get; set; }

		public string TypeName { get; set; }
	}
}
