/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import java.util.Arrays;
import java.util.Collections;
import java.util.List;

import net.izsak.sandcastle.configuration.DocletOptions;
import net.izsak.sandcastle.configuration.DocletOptionsParser;

import org.apache.commons.cli.CommandLine;
import org.apache.commons.cli.MissingOptionException;
import org.apache.commons.cli.ParseException;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.ConstructorDoc;
import com.sun.javadoc.FieldDoc;
import com.sun.javadoc.MethodDoc;
import com.sun.javadoc.PackageDoc;
import com.sun.javadoc.RootDoc;


/**
 * Class that will visit whole API tree and using
 * documentation writer will generate output XML file.
 * 
 * @author Jozef Izso
 *
 */
public class DocumentationApiVisitor {
	
	private CommandLine cmd;
	
	private RootDoc doc;
	private IApiNamer namer;
	private IApiWriter writer;
	
	public DocumentationApiVisitor() {
		this.namer = new DotNetApiNamer();
		this.writer = new DocumentationWriter();
		this.writer.setApiNamer(this.namer);
	}
	
	public void setOptions(String[][] javadocOptions) throws ParseException {
		DocletOptionsParser parser = new DocletOptionsParser();
		CommandLine cmd = parser.parse(javadocOptions);
		this.cmd = cmd;
	}
	
	public void setRootDoc(RootDoc doc) {
		this.doc = doc;
	}
	
	public void setApiWriter(IApiWriter apiWriter) {
		if (apiWriter == null)
			throw new IllegalArgumentException("Argument apiWriter cannot be null.");
		
		this.writer = apiWriter;
		this.writer.setApiNamer(this.namer);
	}
	
	/**
	 * Starts the processing of API and extracting documentation
	 * and/or API information from packages.
	 * 
	 * @param packages
	 */
	public void visitApi() {
		if (this.doc == null)
			throw new IllegalStateException("Property RootDoc is not set to an object.");
		
		this.writer.writeMetadata(null);
		
		PackageUtil pe = new PackageUtil(this.doc);
		this.visitPackages(pe.getAllPackages());
	}
	
	private void visitPackages(List<PackageDoc> packages) {
		Collections.sort(packages, new PackageComparator());
		
		for (PackageDoc pd : packages) {
			this.writer.writePackage(pd);
			
			this.visitClasses(Arrays.asList(pd.allClasses()));
		}
	}
	
	private void visitClasses(List<ClassDoc> classes) {
		Collections.sort(classes, new ClassComparator());
		
		for (ClassDoc cd : classes) {
			this.writer.writeClass(cd);
			this.visitConstructors(Arrays.asList(cd.constructors()));
			this.visitMethods(Arrays.asList(cd.methods()));
			this.visitFields(Arrays.asList(cd.fields(true)));
		}
	}
	
	private void visitConstructors(List<ConstructorDoc> ctors) {
		for (ConstructorDoc cd : ctors) {
			this.writer.writeMember(cd);
		}
	}
	
	private void visitMethods(List<MethodDoc> methods) {
		Collections.sort(methods, new MethodComparator(this.namer));
		for (MethodDoc md : methods) {
			this.writer.writeMember(md);
		}
	}
	
	private void visitFields(List<FieldDoc> fields) {
		for (FieldDoc md : fields) {
			this.writer.writeMember(md);
		}
	}
	
	public String toXml() {
		return this.writer.toXml();
	}
	
	public void saveXml() throws MissingOptionException {
		String optName = DocletOptions.OUTPUT_FILE.getArgName();
		String filename = cmd.getOptionValue(optName);
		if (filename == null)
			throw new MissingOptionException(Arrays.asList(optName));
		
		saveXml(filename);
	}
	
	public void saveXml(String filename) {
		this.writer.saveXml(filename);
	}
}
