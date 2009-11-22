/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import nu.xom.Element;

import com.sun.javadoc.Tag;

/**
 * @author Jozef Izso
 *
 */
public class BlockTagConverter {
	
	private Tag tag;

	protected BlockTagConverter() {
		this(null);
	}
	
	public BlockTagConverter(Tag tag) {
		this.tag = tag;
	}
	
	/**
	 * @return Returns the name of the XML documentation element.
	 */
	public String getName() {
		return this.tag.name().substring(1);
	}
	
	public Tag getTag() {
		return this.tag;
	}
	
	public Tag[] getInlineTags() {
		return this.tag.inlineTags();
	}
	
	public Element toXml() {
		Element elmTag = new Element(this.getName());
		addAttributes(elmTag);
		
		InlineTagConverter inlines = new InlineTagConverter(this.getTag(), this.getInlineTags());
		inlines.toXml(elmTag);
		
		return elmTag;
	}
	
	protected void addAttributes(Element element) {
	}
}
