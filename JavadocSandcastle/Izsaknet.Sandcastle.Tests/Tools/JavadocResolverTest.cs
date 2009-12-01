using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

using Izsaknet.Sandcastle.Tools;
using Microsoft.Ddue.Tools;
using Izsaknet.Sandcastle.Tests;

namespace Izsaknet.Sandcastle.Tools.Tests
{
	[TestFixture]
	public class JavadocResolverTest
	{
		private JavadocResolver Javadoc { get; set; }

		[SetUp]
		public void TestSetup()
		{
			this.Javadoc = new JavadocResolver()
			{
				JavadocUrl = "http://java.sun.com/javase/6/docs/api"
			};
		}

		[Test]
		public void JavadocUrl_Test()
		{
			var unity = UnityBootstrapper.Unity;

			var javadoc = (JavadocResolver)unity.Resolve<IExternalReferenceResolver>("javadoc");

			Assert.That(javadoc.JavadocUrl, Is.EqualTo("http://java.sun.com/javase/6/docs/api"));
		}

		[Test]
		public void Resolve_JavaLangObject_Test()
		{
			string objectId = "T:java.lang.Object";
			string objectUrl = Javadoc.GetExternalUrl(objectId);

			Assert.That(objectUrl, Is.EqualTo("http://java.sun.com/javase/6/docs/api/java/lang/Object.html"));
		}
	}
}
