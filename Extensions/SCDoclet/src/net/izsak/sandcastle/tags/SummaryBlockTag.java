/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.tags;

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
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.BlockTagConverter#hasContent()
	 */
	@Override
	protected boolean hasContent() {
		//return this.doc.commentText().length() > 0;
		return this.getInlineTags().length > 0;
	}
}
