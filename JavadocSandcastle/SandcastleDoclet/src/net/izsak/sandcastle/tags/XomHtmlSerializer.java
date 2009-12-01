/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.tags;

import java.util.Arrays;
import java.util.HashSet;
import java.util.Set;

import nu.xom.Attribute;
import nu.xom.Element;
import nu.xom.ParentNode;
import nu.xom.Text;

import org.xml.sax.Attributes;
import org.xml.sax.ContentHandler;
import org.xml.sax.Locator;
import org.xml.sax.SAXException;
import org.xml.sax.ext.LexicalHandler;

/**
 * @author Jozef Izso
 *
 */
public class XomHtmlSerializer implements ContentHandler, LexicalHandler {
	
	private static final Set<String> ALLOWED_TAGS = new HashSet<String>( Arrays.asList(
			"b", "i", "strong", "em", "p", "table", "br"//, "html", "head", "body"
	));
	
	private static final Set<String> VOID_ELEMENTS = new HashSet<String>( Arrays.asList(
	    "area", "base", "basefont",
        "bgsound", "br", "col", "command", "embed", "event-source",
        "frame", "hr", "img", "input", "keygen", "link", "meta", "param",
        "source", "spacer", "wbr"
    ));

	private HtmlParserInfrastructureTags infrastructure;
	
	private Element root;
	private ParentNode current;
	
	private int ignoreLevel = 0;
	
	protected XomHtmlSerializer() {
		this.infrastructure = new HtmlParserInfrastructureTags();
	}
	
	private void pushCurrentElement(ParentNode elm) {
		this.current = elm;
	}
	
	private void popCurrentElement() {
		this.current = this.current.getParent();
	}
	
	public Element getRootElement() {
		return this.root;
	}
	
	/* (non-Javadoc)
	 * @see org.xml.sax.ContentHandler#startElement(java.lang.String, java.lang.String, java.lang.String, org.xml.sax.Attributes)
	 */
	@Override
	public void startElement(String uri, String localName, String qName, Attributes atts) throws SAXException {
		boolean isInfr = this.infrastructure.isInfrastrucuteTag(localName); 
		if (isInfr) {
			this.infrastructure.markTagAsUsed(localName);
			ignoreLevel++;
			return;
		}
		
		boolean shouldEscape = false; // */ (isInfr && this.infrastructure.allTagsWritten()) || (!isInfr && !ALLOWED_TAGS.contains(localName));
		shouldEscape = !ALLOWED_TAGS.contains(localName);
			
		Element elm = new Element(localName);
		
		for (int i = 0; i < atts.getLength(); i++) {
			String n = atts.getLocalName(i);
			String v = atts.getValue(i);
			elm.addAttribute(new Attribute(n, v));
		}
		
		if (shouldEscape) {
			this.current.appendChild(new Text(elm.toXML()));
			ignoreLevel++;
		}
		else {
			this.current.appendChild(elm);
		
			if (VOID_ELEMENTS.contains(localName)) {
				ignoreLevel++;
			} else {
				pushCurrentElement(elm);
			}
		}
	}
	
	/* (non-Javadoc)
	 * @see org.xml.sax.ContentHandler#endElement(java.lang.String, java.lang.String, java.lang.String)
	 */
	@Override
	public void endElement(String uri, String localName, String qName) throws SAXException {
		if (ignoreLevel > 0) {
			ignoreLevel--;
		} else {
			this.popCurrentElement();
		}
	}

	/* (non-Javadoc)
	 * @see org.xml.sax.ext.LexicalHandler#comment(char[], int, int)
	 */
	@Override
	public void comment(char[] ch, int start, int length) throws SAXException {
		if (ignoreLevel > 0) {
			return;
		}
		
		String text = new String(ch, start, length);
		this.current.appendChild(new Text(text));
	}

	/* (non-Javadoc)
	 * @see org.xml.sax.ContentHandler#startDocument()
	 */
	@Override
	public void startDocument() throws SAXException {
		this.root = new Element("body");
		this.pushCurrentElement(this.root);
	}
	
	/* (non-Javadoc)
	 * @see org.xml.sax.ContentHandler#endDocument()
	 */
	@Override
	public void endDocument() throws SAXException {
	}

	/* (non-Javadoc)
	 * @see org.xml.sax.ContentHandler#characters(char[], int, int)
	 */
	@Override
	public void characters(char[] ch, int start, int length)
			throws SAXException {
		
		String text = new String(ch, start, length);
		this.current.appendChild(new Text(text));
	}

	/* (non-Javadoc)
	 * @see org.xml.sax.ContentHandler#ignorableWhitespace(char[], int, int)
	 */
	@Override
	public void ignorableWhitespace(char[] ch, int start, int length)
			throws SAXException {
		this.characters(ch, start, length);
	}

	
	/* (non-Javadoc)
	 * @see org.xml.sax.ContentHandler#endPrefixMapping(java.lang.String)
	 */
	@Override
	public void endPrefixMapping(String prefix) throws SAXException {
	}

	/* (non-Javadoc)
	 * @see org.xml.sax.ContentHandler#processingInstruction(java.lang.String, java.lang.String)
	 */
	@Override
	public void processingInstruction(String target, String data)
			throws SAXException {
	}

	/* (non-Javadoc)
	 * @see org.xml.sax.ContentHandler#setDocumentLocator(org.xml.sax.Locator)
	 */
	@Override
	public void setDocumentLocator(Locator locator) {
	}

	/* (non-Javadoc)
	 * @see org.xml.sax.ContentHandler#skippedEntity(java.lang.String)
	 */
	@Override
	public void skippedEntity(String name) throws SAXException {
	}

	/* (non-Javadoc)
	 * @see org.xml.sax.ContentHandler#startPrefixMapping(java.lang.String, java.lang.String)
	 */
	@Override
	public void startPrefixMapping(String prefix, String uri)
			throws SAXException {
	}

	/* (non-Javadoc)
	 * @see org.xml.sax.ext.LexicalHandler#endCDATA()
	 */
	@Override
	public void endCDATA() throws SAXException {
	}

	/* (non-Javadoc)
	 * @see org.xml.sax.ext.LexicalHandler#endDTD()
	 */
	@Override
	public void endDTD() throws SAXException {
	}

	/* (non-Javadoc)
	 * @see org.xml.sax.ext.LexicalHandler#endEntity(java.lang.String)
	 */
	@Override
	public void endEntity(String name) throws SAXException {
	}

	/* (non-Javadoc)
	 * @see org.xml.sax.ext.LexicalHandler#startCDATA()
	 */
	@Override
	public void startCDATA() throws SAXException {
	}

	/* (non-Javadoc)
	 * @see org.xml.sax.ext.LexicalHandler#startDTD(java.lang.String, java.lang.String, java.lang.String)
	 */
	@Override
	public void startDTD(String name, String publicId, String systemId)
			throws SAXException {
	}

	/* (non-Javadoc)
	 * @see org.xml.sax.ext.LexicalHandler#startEntity(java.lang.String)
	 */
	@Override
	public void startEntity(String name) throws SAXException {
	}
}
