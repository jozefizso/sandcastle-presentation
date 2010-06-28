/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import java.util.Comparator;

import com.sun.javadoc.ClassDoc;

/**
 * @author Jozef Izso
 *
 */
public class ClassComparator implements Comparator<ClassDoc> {

	/* (non-Javadoc)
	 * @see java.util.Comparator#compare(java.lang.Object, java.lang.Object)
	 */
	@Override
	public int compare(ClassDoc c1, ClassDoc c2) {
		return c1.qualifiedName().compareTo(c2.qualifiedName());
	}

}
