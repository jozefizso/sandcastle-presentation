/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import java.util.Comparator;

import com.sun.javadoc.PackageDoc;

/**
 * @author Jozef Izso
 *
 */
public class PackageComparator implements Comparator<PackageDoc> {

	/* (non-Javadoc)
	 * @see java.util.Comparator#compare(java.lang.Object, java.lang.Object)
	 */
	@Override
	public int compare(PackageDoc p1, PackageDoc p2) {
		return p1.name().compareTo(p2.name());
	}

}
