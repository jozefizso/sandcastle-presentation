/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import static org.junit.Assert.assertTrue;

import java.io.File;

import org.junit.Test;

import com.sun.javadoc.RootDoc;


/**
 * @author Jozef Izso
 *
 */
public class XmlDocletTest {
	
	@Test
	public void runJavadocTest() {
		File sourceDir = new File("../Sample Javadoc Project/src");
		JavadocWrapper wrapper = new JavadocWrapper(sourceDir, "net");
		RootDoc root = wrapper.getRootDoc();
		
		boolean success = XmlDoclet.start(root);
		
		assertTrue(success);
	}
}
