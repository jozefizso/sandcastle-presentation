/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak;

/**
 * This is abstract class.
 * 
 * @author Jozef Izso
 *
 */
public abstract class AbstractClass {

	/**
	 * This is abstract method.
	 */
	public abstract void testMethod();
	
	/**
	 * This is final (sealed) method.
	 * @return Always returns "AbstractClass". 
	 */
	public final String getClassName() {
		return "AbstractClass";
	}
}
