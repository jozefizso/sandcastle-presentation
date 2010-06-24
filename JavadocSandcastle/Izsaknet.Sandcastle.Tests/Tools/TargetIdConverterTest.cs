using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

using Microsoft.Ddue.Tools;

namespace Izsaknet.Sandcastle.Tools
{
	[TestFixture]
	public class TargetIdConverterTest
	{
		[Test]
		public void Namespace_Converter_Test()
		{
			TargetIdConverter conv = new TargetIdConverter("N:java.lang");
			Target target = conv.ToTarget();

			Assert.AreEqual(LinkType2.External, target.DefaultLinkType);
			Assert.IsInstanceOf<NamespaceTarget>(target);

			NamespaceTarget nt = (NamespaceTarget)target;
			Assert.AreEqual("java.lang", nt.Name);
			Assert.AreEqual("java.lang", nt.Container);
		}

		[Test]
		public void Type_Converter_Test()
		{
			TargetIdConverter conv = new TargetIdConverter("T:java.lang.Integer");
			Target target = conv.ToTarget();

			Assert.AreEqual(LinkType2.External, target.DefaultLinkType);
			Assert.IsInstanceOf<TypeTarget>(target);

			TypeTarget tt = (TypeTarget)target;
			Assert.AreEqual("Integer", tt.Name);
			Assert.AreEqual("java.lang", tt.Container);
			Assert.IsNotNull(tt.Namespace);
			StringAssert.Contains("java.lang", tt.Namespace.Id);
			CollectionAssert.IsEmpty(tt.Templates);
		}

		[Test]
		public void Type_NoNamespace_Converter_Test()
		{
			TargetIdConverter conv = new TargetIdConverter("T:Class1");
			TypeTarget tt = (TypeTarget)conv.ToTarget();
			
			Assert.AreEqual("Class1", tt.Name);
			Assert.IsNotNull(tt.Namespace);
			Assert.AreEqual("N:", tt.Namespace.Id);
		}

		[Test]
		//[Values("T:int", "int")]
		public void PrimitiveType_Converter_Test(/*string primitiveTypeId, string primitiveName*/)
		{
			TargetIdConverter conv = new TargetIdConverter("T:int");
			TypeTarget tt = (TypeTarget)conv.ToTarget();

			Assert.AreEqual("int", tt.Name);
			Assert.IsNull(tt.Namespace);
		}

		[Test]
		public void Method_Converter_Test()
		{
			TargetIdConverter conv = new TargetIdConverter("M:java.lang.Boolean.booleanValue");
			Target target = conv.ToTarget();

			Assert.AreEqual(LinkType2.External, target.DefaultLinkType);
			Assert.IsInstanceOf<MethodTarget>(target);

			MethodTarget mt = (MethodTarget)target;
			Assert.AreEqual("booleanValue", mt.Name);
			//Assert.AreEqual("java.lang", tt.Container);
			//Assert.IsNotNull(mt.Type);
			CollectionAssert.IsEmpty(mt.Templates);
			CollectionAssert.IsEmpty(mt.Parameters);
			CollectionAssert.IsEmpty(mt.TemplateArgs);
		}
	}
}
