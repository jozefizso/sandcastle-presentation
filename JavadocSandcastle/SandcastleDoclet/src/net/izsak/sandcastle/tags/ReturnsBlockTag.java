/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.tags;

import net.izsak.sandcastle.IApiNamer;

import com.sun.javadoc.Tag;

/**
 * Represent the <code>@return</code> tag and will convert it into
 * <returns>doc comment</returns> element.
 * 
 * @author Jozef Izso
 *
 */
public class ReturnsBlockTag extends BlockTagConverter {

	public ReturnsBlockTag(Tag tag, IApiNamer apiNamer) {
		super(tag, apiNamer);
	}

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.tags.BlockTagConverter#getName()
	 */
	@Override
	public String getName() {
		return "returns";
	}
}
