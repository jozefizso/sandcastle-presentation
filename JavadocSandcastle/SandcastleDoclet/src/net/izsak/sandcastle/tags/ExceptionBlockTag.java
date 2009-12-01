/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.tags;

import net.izsak.sandcastle.IApiNamer;
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
	public ExceptionBlockTag(ThrowsTag tag, IApiNamer apiNamer) {
		super(tag, apiNamer);
		this.tag = tag;
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.BlockTagConverter#addAttributes(nu.xom.Element)
	 */
	@Override
	protected void addAttributes(Element element) {
		super.addAttributes(element);
		String qname = this.getApiNamer().getClassName(this.tag.exceptionType().asClassDoc());		
		element.addAttribute(new Attribute("cref", qname));
	}

}
