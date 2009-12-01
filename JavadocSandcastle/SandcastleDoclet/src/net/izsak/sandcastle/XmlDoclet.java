/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import net.izsak.sandcastle.configuration.DocletConfig;
import net.izsak.sandcastle.configuration.DocletOptions;
import nu.xom.Document;

import org.apache.commons.cli.ParseException;
import org.apache.commons.configuration.ConfigurationException;

import com.sun.javadoc.DocErrorReporter;
import com.sun.javadoc.Doclet;
import com.sun.javadoc.LanguageVersion;
import com.sun.javadoc.RootDoc;

/**
 * @author Jozef Izso
 * 
 */
public class XmlDoclet extends Doclet {

	public XmlDoclet() {
	}
	
	public Document processApi(RootDoc root) {
		return process(root, new ApiWriter());
	}
	
	public Document processDoc(RootDoc root) {
		return process(root, new DocumentationWriter());
	}

	protected Document process(RootDoc root, IApiWriter apiWriter) {
		DocumentationApiVisitor visitor = new DocumentationApiVisitor();
		
		visitor.setApiWriter(apiWriter);
		visitor.setRootDoc(root);
		visitor.visitApi();
		
		return visitor.toXmlDocument();
	}
	
	
	public static boolean start(RootDoc root) {
		DocletConfig config = new DocletConfig();
		try {
			config.configure(root.options());
		} catch (ConfigurationException ex) {
			ex.printStackTrace();
			return false;
		} catch (ParseException ex) {
			ex.printStackTrace();
			return false;
		}
		
		DocumentationApiVisitor visitor = new DocumentationApiVisitor();
		
		if (config.generateApiFile()) {
			visitor.setApiWriter(new ApiWriter());
			visitor.setRootDoc(root);
			visitor.visitApi();
			visitor.saveXml(config.getApiFileName());
		}
		
		if (config.generateDocFile()) {
			visitor.setApiWriter(new DocumentationWriter());
			visitor.setRootDoc(root);
			visitor.visitApi();
			visitor.saveXml(config.getDocFileName());
		}
		
		return true;
	}

	public static int optionLength(String option) {
		DocletOptions options = new DocletOptions();
		if (options.hasOption(option))
			return options.getOption(option).getArgs() + 1;
		
		return 0;
	}

	public static boolean validOptions(String options[][], DocErrorReporter reporter) {
		return true;
	}

	public static LanguageVersion languageVersion() {
		return LanguageVersion.JAVA_1_5;
	}
}
