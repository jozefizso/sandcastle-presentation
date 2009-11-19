using System;
using System.Collections.Generic;
using System.Xml;

namespace Izsaknet.Xml
{
	public class XmlEnvVarResolver : XmlUrlResolver
	{
		public XmlEnvVarResolver()
			: base()
		{ }

		public override Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			return base.ResolveUri(baseUri, Environment.ExpandEnvironmentVariables(relativeUri));
		}
	}
}
