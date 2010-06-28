/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.tags;

import net.izsak.sandcastle.IApiNamer;
import nu.xom.Attribute;
import nu.xom.Element;
import nu.xom.Node;

import com.sun.javadoc.SeeTag;
import com.sun.javadoc.Tag;

/**
 * @author Jozef Izso
 *
 */
public class InlineTagConverter {
	private Tag block;
	private Tag[] inlineTags;
	private IApiNamer apiNamer;
	
	public InlineTagConverter(Tag block, Tag[] inlineTags, IApiNamer apiNamer) {
		this.block = block;
		this.inlineTags = inlineTags;
		this.apiNamer = apiNamer;
	}
	
	public void toXml(Element parent) {
		if (this.inlineTags.length == 0 && hasContent()) {
			parent.appendChild(this.block.text());
			return;
		}
		
		for (Tag inline : this.inlineTags) {
			if (inline.name().equals("Text")) {
				processTextTag(inline, parent);
			} else if (inline.kind().equals("@see")) {
				Element see = processSeeTag((SeeTag)inline);
				parent.appendChild(see);
			} else if (inline.kind().equals("@param")) {
				Element paramref = processParamTag(inline);
				parent.appendChild(paramref);
			}
		}
	}
	
	private boolean hasContent() {
		return (this.block != null) && (!this.block.text().isEmpty());
	}
	
	protected void processTextTag(Tag tag, Element parent) {
		String text = tag.text();
		if (text.contains("<")) {			
			HtmlCleaner cleaner = new HtmlCleaner(text);
			Element body = cleaner.clean();
			
			while (body.getChildCount() > 0) {
				Node n = body.getChild(0);
				n.detach();
				parent.appendChild(n);
			}
		}
		else {
			parent.appendChild(tag.text());
		}
	}
	
	protected Element processSeeTag(SeeTag tag) {
		String qname = getSeeTagTypeName(tag);
		
		if (qname == null) {
			return processLinkTag(tag);
		}

		Element elmLink = new Element("see");
		elmLink.addAttribute(new Attribute("cref", qname));
		return elmLink;
	}
	
	protected Element processLinkTag(SeeTag tag) {
		Element elmLink = new Element("link");
		elmLink.addAttribute(new Attribute("href", tag.referencedClassName()));
		if (tag.label() != null)
			elmLink.appendChild(tag.label());
		return elmLink;
	}
	
	protected Element processParamTag(Tag tag) {
		if (tag.text().startsWith("<"))
			return processTypeParamTag(tag);
		
		Element elmLink = new Element("paramref");
		elmLink.addAttribute(new Attribute("name", tag.text()));
		return elmLink;
	}
	
	protected Element processTypeParamTag(Tag tag) {
		if (!(tag.text().startsWith("<") && tag.text().endsWith(">")))
			throw new IllegalArgumentException("Argument tag does not reference generic type param.");
		
		String text = tag.text().substring(1, tag.text().length()-1);
		Element elmLink = new Element("typeparamref");
		elmLink.addAttribute(new Attribute("name", text));
		return elmLink;
	}
	
	protected String getSeeTagTypeName(SeeTag tag) {
		String qname = null;
		
		// check for member first because referenced members have referencedClass()
		// set also.
		if (tag.referencedMember() != null) {
			qname = this.apiNamer.getMemberName(tag.referencedMember());
		} else if (tag.referencedClass() != null) {
			qname = this.apiNamer.getClassName(tag.referencedClass());
		} else if (tag.referencedPackage() != null) {
			qname = this.apiNamer.getPackageName(tag.referencedPackage());
		}
		
		return qname;
	}
}
