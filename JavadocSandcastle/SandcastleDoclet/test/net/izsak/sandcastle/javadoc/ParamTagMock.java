/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.javadoc;

import com.sun.javadoc.Doc;
import com.sun.javadoc.ParamTag;
import com.sun.javadoc.SourcePosition;
import com.sun.javadoc.Tag;
import com.sun.tools.doclets.internal.toolkit.util.TextTag;

/**
 * Simple mock of a ParamTag interface.
 * 
 * @author Jozef Izso
 */
public class ParamTagMock implements ParamTag {
	
	private String paramName;
	private String commentText;
	private boolean isTypeParam;

	public ParamTagMock(String paramName, String commentText) {
		this(paramName, commentText, false);
	}
	
	public ParamTagMock(String paramName, String commentText, boolean isTypeParameter) {
		this.paramName = paramName;
		this.commentText = commentText;
		this.isTypeParam = isTypeParameter;
	}
	
	/* (non-Javadoc)
	 * @see com.sun.javadoc.ParamTag#isTypeParameter()
	 */
	@Override
	public boolean isTypeParameter() {
		return isTypeParam;
	}

	/* (non-Javadoc)
	 * @see com.sun.javadoc.ParamTag#parameterComment()
	 */
	@Override
	public String parameterComment() {
		return this.commentText;
	}

	/* (non-Javadoc)
	 * @see com.sun.javadoc.ParamTag#parameterName()
	 */
	@Override
	public String parameterName() {
		return this.paramName;
	}

	/* (non-Javadoc)
	 * @see com.sun.javadoc.Tag#firstSentenceTags()
	 */
	@Override
	public Tag[] firstSentenceTags() {
		return null;
	}

	/* (non-Javadoc)
	 * @see com.sun.javadoc.Tag#holder()
	 */
	@Override
	public Doc holder() {
		return null;
	}

	/* (non-Javadoc)
	 * @see com.sun.javadoc.Tag#inlineTags()
	 */
	@Override
	public Tag[] inlineTags() {
		return new TextTag[] { new TextTag(null, commentText) };
	}

	/* (non-Javadoc)
	 * @see com.sun.javadoc.Tag#kind()
	 */
	@Override
	public String kind() {
		return "@param";
	}

	/* (non-Javadoc)
	 * @see com.sun.javadoc.Tag#name()
	 */
	@Override
	public String name() {
		return "@param";
	}

	/* (non-Javadoc)
	 * @see com.sun.javadoc.Tag#position()
	 */
	@Override
	public SourcePosition position() {
		return null;
	}

	/* (non-Javadoc)
	 * @see com.sun.javadoc.Tag#text()
	 */
	@Override
	public String text() {
		return this.paramName + " " + this.commentText;
	}

}
