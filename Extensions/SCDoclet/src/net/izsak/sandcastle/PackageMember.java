/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import nu.xom.Element;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.PackageDoc;

/**
 * @author Jozef Izso
 *
 */
public class PackageMember extends Member<PackageDoc> {
	
	public PackageMember(PackageDoc packageDoc) {
		super(packageDoc);
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.Member#getTypeChar()
	 */
	@Override
	public String getTypeChar() {
		return "N";
	}

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.Member#processChildMembers(nu.xom.Element)
	 */
	@Override
	protected void processChildMembers(Element members) {
		for (ClassDoc cd : this.getDoc().allClasses()) {
			ClassMember cm = new ClassMember(cd, this.getFullName());
			cm.processMemberAndChilds(members);
		}
	}
}
