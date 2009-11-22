/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import nu.xom.Attribute;
import nu.xom.Element;

import com.sun.javadoc.ThrowsTag;

/**
 * @author Jozef Izso
 *
 */
public class ExceptionBlockTag extends BlockTagConverter {

	private ThrowsTag tag;
	
	/**
	 * @param elementName
	 * @param tag
	 */
	public ExceptionBlockTag(ThrowsTag tag) {
		super(tag);
		this.tag = tag;
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.BlockTagConverter#addAttributes(nu.xom.Element)
	 */
	@Override
	protected void addAttributes(Element element) {
		super.addAttributes(element);
		ClassMember cm = new ClassMember(this.tag.exceptionType().asClassDoc(), "");
		String qname = cm.getQualifiedMemberName();
		
		element.addAttribute(new Attribute("cref", qname));
	}

}
