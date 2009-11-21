/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import nu.xom.Element;

import com.sun.javadoc.MethodDoc;

/**
 * @author Jozef Izso
 *
 */
public class MethodMember extends ExecutableMember<MethodDoc> {

	/**
	 * @param doc
	 */
	public MethodMember(MethodDoc doc, String parentName) {
		super(doc, parentName);
	}

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.Member#getTypeChar()
	 */
	@Override
	public String getTypeChar() {
		String name = this.getDoc().name();
		return (name.startsWith("set") || name.startsWith("get")) ? "P" : "M";
	}

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.Member#processChildMembers(nu.xom.Element)
	 */
	@Override
	protected void processChildMembers(Element members) {
	}
}
