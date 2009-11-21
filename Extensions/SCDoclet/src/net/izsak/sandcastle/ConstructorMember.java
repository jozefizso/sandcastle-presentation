/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import nu.xom.Element;

import com.sun.javadoc.ConstructorDoc;


/**
 * @author Jozef Izso
 *
 */
public class ConstructorMember extends ExecutableMember<ConstructorDoc> {
	
	/**
	 * @param doc
	 */
	public ConstructorMember(ConstructorDoc doc, String parentName) {
		super(doc, parentName);
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.Member#getName()
	 */
	@Override
	public String getName() {
		return "#ctor";
	}

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.Member#processChildMembers(nu.xom.Element)
	 */
	@Override
	protected void processChildMembers(Element members) {
	}
}
