/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.tags;

import java.io.IOException;

import junit.framework.Assert;
import net.izsak.sandcastle.JavadocTester;
import nu.xom.Document;
import nu.xom.Node;

import org.custommonkey.xmlunit.XMLAssert;
import org.junit.Test;
import org.xml.sax.SAXException;


/**
 * @author Jozef Izso
 *
 */
public class InlineTagConverterTest {
	
	@Test
	public void processTextTag_Test() throws SAXException, IOException {
		String expectedSummary = "<summary>Class with HTML tags.<br />\n This will be on a new line.</summary>";
		
		JavadocTester tester = new JavadocTester("Html01.java");
		Document doc = tester.getDocumentationXml();
		
		Node elm = doc.query("//member[@name='T:samples.Html01']/summary").get(0);
		
		XMLAssert.assertXMLEqual(expectedSummary, elm.toXML());
	}
	
	@Test
	public void processTextTag_NonHtmlTag_Test() throws SAXException, IOException {
		JavadocTester tester = new JavadocTester("Html02.java");
		Document doc = tester.getDocumentationXml();
		
		Node elm = doc.query("//member[@name='T:samples.Html02']/summary").get(0);
		
		// should have just one Text child node.
		Assert.assertEquals(1, elm.getChildCount());
	}
}
