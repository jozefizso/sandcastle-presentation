/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak;

import javax.xml.bind.annotation.XmlRootElement;

/**
 * This class tests where Doclet for Sandcastle can handle
 * various features of Java and Javadoc.
 * It is stored in the package {@link net.izsak}.
 * 
 * @version 1.0
 */
@XmlRootElement
public final class StoredNumber {
	
	private int value;
	
	/**
	 * Initializes new {@link StoredNumber} instance with defined value.
	 * 
	 * @param i The value to which this instance will be initialized.
	 */
	public StoredNumber(int i) {
		this.value = i;
	}
	
	/**
	 * Increments stored number by one.
	 */
	public void increment() {
		this.value++;
	}

	/**
	 * Increments stored number by ammount.
	 */
	public void increment(int ammount) {
		this.value += ammount;
	}
	
	/**
	 * Decrements stored number by one.
	 * 
	 * @deprecated This method is deprecated in favor of {@link #decrement()} method.
	 */
	@Deprecated
	public void dec() {
		this.decrement();
	}

	/**
	 * Decrements stored number by one.
	 */
	public int decrement() {
		return --this.value;
	}
	
	/**
	 * Add the value of the parameter {@param number} to the value of current object.
	 * 
	 * @param number Other number.
	 * @throws IllegalArgumentException Thrown when {@param number} is null.
	 */
	public void add(StoredNumber number) throws IllegalArgumentException {
		if (number == null)
			throw new IllegalArgumentException("number");
		
		this.value += number.value;
	}
	
	/**
	 * Always throws an exception.
	 * 
	 * @throws IllegalStateException
	 */
	public static void throwMethod() throws IllegalStateException {
		throw new IllegalStateException("Illegal state sample.");
	}
}
