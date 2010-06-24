using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

using NUnit.Framework;
using Moq;

using Izsaknet.Sandcastle.Tools;
using Microsoft.Ddue.Tools;

namespace Izsaknet.Sandcastle.Tools.Tests
{
	[TestFixture]
	public class ResolveJavadocLinksComponentTest
	{
		[Test]
		public void Create_JavadocResolver_Test()
		{
			XPathDocument configFile = new XPathDocument("test-config.xml");
			var navigator = configFile.CreateNavigator();
			var rootNode = navigator.SelectSingleNode("/component");
			var assembler = new BuildAssembler();

			var component = new ResolveJavadocLinksComponent(assembler, rootNode);

			Assert.That(component.ExternalResover, Is.TypeOf<JavadocResolver>());
		}
	}
}
