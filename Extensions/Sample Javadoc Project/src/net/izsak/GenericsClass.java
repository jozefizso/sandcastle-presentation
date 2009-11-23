/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Set;

/**
 * @author Jozef Izso
 *
 */
public class GenericsClass<T extends ArrayList<E> & List<E>, E> {

	public void genericMethod() {
	}
	
	public List<Integer> list() {
		return null;
	}

	public HashMap<String, Set<Double>> hashMap() {
		return null;
	}

	public <U> HashMap<String, U> hashMapWithSpecialization() {
		return null;
	}
}
