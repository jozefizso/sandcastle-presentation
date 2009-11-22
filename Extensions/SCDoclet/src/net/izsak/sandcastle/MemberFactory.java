/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.ConstructorDoc;
import com.sun.javadoc.Doc;
import com.sun.javadoc.MemberDoc;
import com.sun.javadoc.MethodDoc;
import com.sun.javadoc.PackageDoc;
import com.sun.javadoc.SeeTag;

/**
 * @author Jozef Izso
 *
 */
public class MemberFactory {
	@SuppressWarnings("unchecked")
	public static <T extends Doc> Member<T> createInstance(T doc) {
		Member<?> member = null;
		
		if (doc.isOrdinaryClass()) {
			member = new ClassMember((ClassDoc)doc, "");
		} else if (doc.isConstructor()) {
			member = new ConstructorMember((ConstructorDoc)doc, "");
		} else if (doc.isMethod()) {
			member = new MethodMember((MethodDoc)doc, "");
		} else {
			throw new IllegalArgumentException("Argument doc cannot be converter to Member object.");
		}
		
		return (Member<T>)member;
	}

	@SuppressWarnings("unchecked")
	public static <T extends Doc> Member<T> createInstance(SeeTag tag) {
		Member<?> member = null;

		if (tag.referencedClass() != null) {
			member = new ClassMember(tag.referencedClass(), "");
		} else if (tag.referencedMember() != null) {
			MemberDoc memberDoc = tag.referencedMember();
			member = createInstance(memberDoc);
		} else if (tag.referencedPackage() != null) {
			member = new PackageMember((PackageDoc)tag.referencedPackage());
		} else {
			throw new IllegalArgumentException("Argument doc cannot be converter to Member object.");
		}
		return (Member<T>)member;
	}
}
