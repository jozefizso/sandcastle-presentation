/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package samples;

/**
 * Sample class with just one generic method {@see #method1(Object)}
 *
 */
public class GenericClass03 {
	
	/**
	 * {@see #method1} takes just one templated parameter {@param obj}
 	 * of type {@param <T>}.
 	 * 
	 * @param <T> Type of the input object.
	 * @param obj Actual object.
	 * @return    An object of type {@param <T>}.
	 */
	public <T> T method1(T obj) {
		return null;
	}
}
