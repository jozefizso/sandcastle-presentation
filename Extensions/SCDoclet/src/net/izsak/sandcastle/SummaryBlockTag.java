/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import com.sun.javadoc.Doc;
import com.sun.javadoc.Tag;

/**
 * @author Jozef Izso
 *
 */
public class SummaryBlockTag extends BlockTagConverter {

	private Doc doc;
	
	/**
	 * @param tag
	 */
	public SummaryBlockTag(Doc doc) {
		super();
		this.doc = doc;
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.BlockTagConverter#getName()
	 */
	@Override
	public String getName() {
		return "summary";
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.BlockTagConverter#getInlineTags()
	 */
	@Override
	public Tag[] getInlineTags() {
		return this.doc.inlineTags();
	}
}
