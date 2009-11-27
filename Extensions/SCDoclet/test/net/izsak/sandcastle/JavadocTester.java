/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import java.io.File;

import nu.xom.Document;

/**
 * @author Jozef Izso
 *
 */
public class JavadocTester {
	
	private JavadocWrapper wrapper;

	public JavadocTester(String fileName) {
		File sourceDir = new File("./test");
		File fileToTest = new File("test/samples/"+ fileName);
		wrapper = new JavadocWrapper(sourceDir, fileToTest);
	}

	public Document getDocumentationXml() {
		XmlDoclet doclet = new XmlDoclet();
		Document doc = doclet.processDoc(wrapper.getRootDoc());
		return doc;
	}
}
