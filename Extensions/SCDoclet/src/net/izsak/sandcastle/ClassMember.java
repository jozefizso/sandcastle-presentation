/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import nu.xom.Element;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.ConstructorDoc;
import com.sun.javadoc.MethodDoc;

/**
 * @author Jozef Izso
 *
 */
public class ClassMember extends Member<ClassDoc> {

	/**
	 * @param doc
	 */
	public ClassMember(ClassDoc doc, String parentName) {
		super(doc, parentName);
	}

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.Member#getTypeChar()
	 */
	@Override
	public String getTypeChar() {
		return "T";
	}

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.Member#processChildMembers(nu.xom.Element)
	 */
	@Override
	protected void processChildMembers(Element members) {
		processConstructors(members);
		processMethods(members);
	}

	private void processConstructors(Element members) {
		for (ConstructorDoc cd : this.getDoc().constructors()) {
			ConstructorMember mm = new ConstructorMember(cd, this.getFullName());
			
			mm.processMemberAndChilds(members);
		}
	}

	private void processMethods(Element members) {
		for (MethodDoc md : this.getDoc().methods()) {
			MethodMember mm = new MethodMember(md, this.getFullName());
			
			mm.processMemberAndChilds(members);
		}
	}
}
