/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.tags;

import net.izsak.sandcastle.IApiNamer;
import nu.xom.Attribute;
import nu.xom.Element;

import com.sun.javadoc.ParamTag;

/**
 * @author Jozef Izso
 *
 */
public class ParamBlockTag extends BlockTagConverter {
	
	private ParamTag tag;
	
	/**
	 * 
	 */
	public ParamBlockTag(ParamTag tag, IApiNamer apiNamer) {
		super(tag, apiNamer);
		this.tag = tag;
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.BlockTagConverter#addAttributes(nu.xom.Element)
	 */
	@Override
	protected void addAttributes(Element element) {
		super.addAttributes(element);
		element.addAttribute(new Attribute("name", this.tag.parameterName()));
	}
}
