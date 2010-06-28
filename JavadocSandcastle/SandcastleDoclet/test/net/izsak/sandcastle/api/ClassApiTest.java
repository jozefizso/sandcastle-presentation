/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.api;

import net.izsak.sandcastle.JavadocTester;
import nu.xom.Document;

import org.junit.Test;

/**
 * @author Jozef Izso
 *
 */
public class ClassApiTest {
	@Test
	public void genericClass03_Test() throws Exception {
		JavadocTester tester = new JavadocTester("GenericClass04.java");
		Document doc = tester.getApiXml();
		
		String xml = doc.toXML();
	}
}
