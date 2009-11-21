/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import com.sun.javadoc.DocErrorReporter;
import com.sun.javadoc.Doclet;
import com.sun.javadoc.LanguageVersion;
import com.sun.javadoc.RootDoc;

/**
 * @author Jozef Izso
 * 
 */
public class XmlDoclet extends Doclet {

	public static boolean start(RootDoc root) {
		Documentation doc = new Documentation("Sandcastle Doclet");
		
		PackageExtractor pe = new PackageExtractor(root);
		pe.extractPackages(root.classes());
		
		doc.processPackages(pe.getAllPackages());
		
		doc.saveXml("javadoc.xml");
		return true;
	}

	public static int optionLength(String option) {
		return 2;
	}

	public static boolean validOptions(String options[][],
			DocErrorReporter reporter) {

		return true;
	}

	public static LanguageVersion languageVersion() {
		return LanguageVersion.JAVA_1_5;
	}
}
