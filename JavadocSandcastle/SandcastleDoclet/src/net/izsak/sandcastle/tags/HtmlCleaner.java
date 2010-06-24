/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.tags;

import java.io.StringReader;

import nu.validator.htmlparser.common.XmlViolationPolicy;
import nu.validator.htmlparser.sax.HtmlParser;
import nu.xom.Element;

import org.xml.sax.InputSource;

/**
 * @author Jozef Izso
 *
 */
public class HtmlCleaner {
	
	private String text;
	
	public HtmlCleaner(String text) {
		this.text = text;
	}
	
	public Element clean() {
		StringReader reader = new StringReader(this.text);
		XomHtmlSerializer serializer = new XomHtmlSerializer();
		
		HtmlParser parser = new HtmlParser(XmlViolationPolicy.ALTER_INFOSET);
		parser.setContentHandler(serializer);
		try {
			parser.setProperty("http://xml.org/sax/properties/lexical-handler", serializer);
			parser.parse(new InputSource(reader));
		} catch (Exception ex) {
			ex.printStackTrace();
		}
		
		Element body = serializer.getRootElement();
		String xml = body.toXML();
		//body.detach();
		return body;
	}
}
