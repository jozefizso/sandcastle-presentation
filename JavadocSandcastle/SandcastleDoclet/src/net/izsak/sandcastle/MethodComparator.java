/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import java.util.Comparator;

import com.sun.javadoc.MethodDoc;

/**
 * @author Jozef Izso
 *
 */
public class MethodComparator implements Comparator<MethodDoc> {

	private IApiNamer apiNamer;
	
	public MethodComparator(IApiNamer apiNamer) {
		this.apiNamer = apiNamer;
	}
	
	/* (non-Javadoc)
	 * @see java.util.Comparator#compare(java.lang.Object, java.lang.Object)
	 */
	@Override
	public int compare(MethodDoc m1, MethodDoc m2) {
		String m1Name = this.apiNamer.getMemberName(m1);
		String m2Name = this.apiNamer.getMemberName(m2);
		
		return m1Name.compareTo(m2Name);
	}

}
