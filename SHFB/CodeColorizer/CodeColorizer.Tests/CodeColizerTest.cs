using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using ColorizerLibrary;

namespace ColorizerLibrary.Tests
{
	[TestFixture]
	public class CodeColizerTest
	{
		[Test]
		public void ProcessAndHighlightText_CssCode_HighlightedCssCode()
		{
			string css = @"<pre lang=""css"">
				body {
					background-color: white;
					color: black;
				}
				/* paragraphs */
				p { font-size: 12pt; }
			</pre>";

			CodeColorizer colorizer = new CodeColorizer();

			string result = colorizer.ProcessAndHighlightText(css);

			Assert.IsNotNull(result);
			StringAssert.Contains(@"<span class=""highlight-comment"">/* paragraphs */</span>", result);
			StringAssert.Contains(@"p { font-size: <span class=""highlight-number"">12</span>pt; }</pre>", result);
		}
	}
}
