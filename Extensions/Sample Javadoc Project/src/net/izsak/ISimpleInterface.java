/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak;

import java.util.Comparator;


/**
 * @author Jozef Izso
 *
 */
public interface ISimpleInterface<T extends SimpleBean> {
	public Comparator<T> createComparator(T bean);
	
	public int sum(T... bean);
}
