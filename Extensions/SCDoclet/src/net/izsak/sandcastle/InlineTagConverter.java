/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import nu.xom.Attribute;
import nu.xom.Element;

import com.sun.javadoc.SeeTag;
import com.sun.javadoc.Tag;

/**
 * @author Jozef Izso
 *
 */
public class InlineTagConverter {
	private Tag block;
	private Tag[] inlineTags;
	
	public InlineTagConverter(Tag block, Tag[] inlineTags) {
		this.block = block;
		this.inlineTags = inlineTags;
	}
	
	public void toXml(Element parent) {
		if (this.inlineTags.length == 0) {
			parent.appendChild(this.block.text());
			return;
		}
		
		for (Tag inline : this.inlineTags) {
			if (inline.name().equals("Text")) {
				parent.appendChild(inline.text());
			} else if (inline.kind().equals("@see")) {
				Element see = processLinkTag((SeeTag)inline);
				parent.appendChild(see);
			} else if (inline.kind().equals("@param")) {
				Element paramref = processParamTag(inline);
				parent.appendChild(paramref);
			}
		}
	}
	
	protected Element processLinkTag(SeeTag tag) {
		Member<?> member = MemberFactory.createInstance(tag);
		
		Element elmLink = new Element("see");
		elmLink.addAttribute(new Attribute("cref", member.getQualifiedMemberName()));
		return elmLink;
	}
	
	protected Element processParamTag(Tag tag) {
		Element elmLink = new Element("paramref");
		elmLink.addAttribute(new Attribute("name", tag.text()));
		return elmLink;
	}
}
