/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import static org.junit.Assert.*;

import java.io.File;

import junit.framework.Assert;

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
	
	@Test
	public void optionLength_Test() {
		Assert.assertEquals(2, XmlDoclet.optionLength("-doc"));
		Assert.assertEquals(2, XmlDoclet.optionLength("-metadata"));
		
		Assert.assertEquals(0, XmlDoclet.optionLength("-unknownOption"));
	}
}
