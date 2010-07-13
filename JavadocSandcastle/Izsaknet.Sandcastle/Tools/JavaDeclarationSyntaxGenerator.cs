using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Ddue.Tools;
using System.Xml.XPath;

namespace Izsaknet.Sandcastle.Tools
{
	public class JavaDeclarationSyntaxGenerator : JSharpDeclarationSyntaxGenerator
	{

		public JavaDeclarationSyntaxGenerator(XPathNavigator configuration)
			: base(configuration)
		{
			if (String.IsNullOrEmpty(Language))
				this.Language = "Java";
		}

		public override void WriteNormalMethodSyntax(XPathNavigator reflection, SyntaxWriter writer)
		{
			base.WriteNormalMethodSyntax(reflection, writer);
			this.WriteMethodThrowsSyntax(reflection, writer);
		}

		public void WriteMethodThrowsSyntax(XPathNavigator reflection, SyntaxWriter writer)
		{
			XPathNodeIterator exceptions = reflection.Select(methodThrowsExpression);
			
			//Console.WriteLine("Thrown exceptions count: {0}", exceptions.Count);
			//Console.WriteLine(reflection.InnerXml);

			if (exceptions.Count > 0)
			{
				this.WriteMethodThrows(exceptions, reflection, writer);
			}
		}

		protected override XPathExpression GetIsObjectExpression()
		{
			return typeIsJavaObjectExpression;
		}

		private void WriteMethodThrows(XPathNodeIterator exceptions, XPathNavigator reflection, SyntaxWriter writer)
		{
			writer.WriteString(" ");
			writer.WriteKeyword("throws");
			writer.WriteString(" ");

			while (exceptions.MoveNext())
			{
				XPathNavigator exception = exceptions.Current;

				WriteTypeReference(exception, writer);

				if (exceptions.CurrentPosition < exceptions.Count)
					writer.WriteString(",");
				writer.WriteLine();
			}
		}

		protected static XPathExpression methodThrowsExpression = XPathExpression.Compile("throws/type");

		protected static XPathExpression typeIsJavaObjectExpression = XPathExpression.Compile("boolean(local-name()='type' and @api='T:java.lang.Object')");
	}
}
