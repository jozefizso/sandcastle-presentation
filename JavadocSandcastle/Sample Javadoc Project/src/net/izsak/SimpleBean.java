/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak;


/**
 * More information about Java beans you can find at article
 * {@link http://en.wikipedia.org/wiki/Java_Bean Java Bean}
 * @author Jozef Izso
 *
 */
public class SimpleBean {
	private int x;
	
	public SimpleBean() {
	}
	
	/**
	 * Returns "Simple bean" text.
	 * @return
	 */
	public String getName() {
		return "Simple bean";
	}
	
	/**
	 * Sets the value of the bean.
	 * @param x Actual value to be set.
	 */
	public void setValue(int x) {
		this.x = x;
	}
	
	/**
	 * Returns true when value of x is greater then 0.
	 * @return
	 */
	public boolean hasValue() {
		return this.x > 0;
	}
}
