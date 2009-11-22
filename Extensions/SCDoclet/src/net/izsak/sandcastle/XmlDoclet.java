/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import net.izsak.sandcastle.configuration.DocletOptions;

import org.apache.commons.cli.ParseException;

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
		DocumentationApiVisitor visitor = new DocumentationApiVisitor();
		try {
			visitor.setOptions(root.options());
		}
		catch (ParseException ex) {
			ex.printStackTrace();
			return false;
		}
		
		visitor.setRootDoc(root);
		visitor.visitApi();
		
		visitor.saveXml("javadoc.xml");
		return true;
	}

	public static int optionLength(String option) {
		DocletOptions options = new DocletOptions();
		if (options.hasOption(option))
			return options.getOption(option).getArgs() + 1;
		
		return 0;
	}

	public static boolean validOptions(String options[][],
			DocErrorReporter reporter) {

		return true;
	}

	public static LanguageVersion languageVersion() {
		return LanguageVersion.JAVA_1_5;
	}
}
