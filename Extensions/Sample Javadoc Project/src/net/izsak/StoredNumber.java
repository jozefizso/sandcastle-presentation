/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak;

import javax.xml.bind.annotation.XmlRootElement;

/**
 * This class tests where Doclet for Sandcastle can handle
 * various features of Java and Javadoc.
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
	public void Increment() {
		this.value++;
	}

	/**
	 * Increments stored number by ammount.
	 */
	public void Increment(int ammount) {
		this.value += ammount;
	}
	
	/**
	 * Decrements stored number by one.
	 * 
	 * @deprecated This method is deprecated in favor of {@link Decrement} method.
	 */
	@Deprecated
	public void Dec() {
		this.Decrement();
	}

	/**
	 * Decrements stored number by one.
	 */
	public int Decrement() {
		return --this.value;
	}
	
	public void Throw() throws IllegalStateException {
		throw new IllegalStateException("Illegal state sample.");
	}
}
