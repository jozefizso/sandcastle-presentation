/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.tags;

import net.izsak.sandcastle.IApiNamer;
import nu.xom.Element;

import com.sun.javadoc.Tag;

/**
 * @author Jozef Izso
 *
 */
public class BlockTagConverter {
	
	private Tag tag;
	private IApiNamer apiNamer;

	protected BlockTagConverter() {
		this(null, null);
	}
	
	public BlockTagConverter(Tag tag, IApiNamer apiNamer) {
		this.tag = tag;
		this.apiNamer = apiNamer;
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
	
	public IApiNamer getApiNamer() {
		return this.apiNamer;
	}
	
	public Element toXml() {
		if (!hasContent())
			return null;
		
		Element elmTag = new Element(this.getName());
		addAttributes(elmTag);
		
		InlineTagConverter inlines = new InlineTagConverter(
				this.getTag(),
				this.getInlineTags(),
				this.apiNamer);
		
		inlines.toXml(elmTag);
		
		return elmTag;
	}
	
	protected void addAttributes(Element element) {
	}
	
	protected boolean hasContent() {
		return (this.getTag() != null) && (!this.getTag().text().isEmpty());
	}
}
