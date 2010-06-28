/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.tags;

import java.io.IOException;
import java.io.StringReader;

import junit.framework.Assert;
import nu.validator.htmlparser.common.XmlViolationPolicy;
import nu.validator.htmlparser.sax.HtmlParser;
import nu.xom.Element;
import nu.xom.ParsingException;
import nu.xom.Text;
import nu.xom.ValidityException;

import org.junit.Ignore;
import org.junit.Test;
import org.xml.sax.InputSource;

/**
 * @author Jozef Izso
 *
 */
public class HtmlCleanerTest {
	@Test
	public void p_tag_xom_test() throws ValidityException, ParsingException, IOException {
		
		String data = "<p>Paragraph.</p>";
		HtmlCleaner cleaner = new HtmlCleaner(data);
		
		Element body = cleaner.clean();
		Element p = (Element)body.getChild(0);
		
		Assert.assertEquals("p", p.getLocalName());
	}

	@Test
	public void br_tag_xom_test() throws ValidityException, ParsingException, IOException {
		
		String data = "First line.<br>Second line.";
		HtmlCleaner cleaner = new HtmlCleaner(data);
		
		Element body = cleaner.clean();
		Text t1 = (Text)body.getChild(0);
		Element br = (Element)body.getChild(1);
		Text t2 = (Text)body.getChild(2);
		
		Assert.assertEquals("", body.getNamespaceURI());
		Assert.assertEquals("First line.", t1.getValue());
		Assert.assertEquals("br", br.getLocalName());
		Assert.assertEquals("", br.getNamespaceURI());
		Assert.assertEquals("Second line.", t2.getValue());
	}

	@Test
	@Ignore
	public void html_test() throws Exception {
		
		String data = "First line.<br>Second line<summary>.";
		StringReader reader = new StringReader(data);
		XomHtmlSerializer serializer = new XomHtmlSerializer();
		
		HtmlParser parser = new HtmlParser(XmlViolationPolicy.ALLOW);
		parser.setContentHandler(serializer);
		parser.setProperty("http://xml.org/sax/properties/lexical-handler", serializer);
		//parser.setErrorHandler(handler);
        
		parser.parse(new InputSource(reader));
		
		Element doc = serializer.getRootElement();
		String xml = doc.toXML();

		Assert.fail();
	}
}
