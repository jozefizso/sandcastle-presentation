using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Xsl;
using Izsaknet.Xml;

namespace Izsaknet.Sandcastle
{
	public class Program
	{
		static void Main(string[] args)
		{
			XslCompiledTransform xslt = new XslCompiledTransform();
			xslt.Load("main.xsl", new XsltSettings(true, true), new XmlEnvVarResolver());

			string s = xslt.ToString();
		}
	}
}
