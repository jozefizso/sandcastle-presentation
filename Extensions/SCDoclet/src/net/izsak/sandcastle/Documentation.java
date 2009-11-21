/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.Collection;

import nu.xom.Document;
import nu.xom.Element;
import nu.xom.Serializer;

import com.sun.javadoc.PackageDoc;


/**
 * @author Jozef Izso
 *
 */
public class Documentation {
	
	private Element rootElement;
	private Element membersElement;
	
	private String projectName;
	
	public Documentation(String projectName) {
		this.projectName = projectName;
		
		createDocumentationRoot();
		createAssemblyName();
		createMembers();
	}
	
	public void processPackages(Collection<PackageDoc> packages) {
		for (PackageDoc pd : packages) {
			PackageMember member = new PackageMember(pd);
			
			member.processMemberAndChilds(this.membersElement);
		}
	}
	
	public String toXml() {
		Document document = new Document(this.rootElement);
		return document.toXML();
	}
	
	public void saveXml(String filename) {
		Document document = new Document(this.rootElement);
		
		try {
			Serializer serializer = new Serializer(new FileOutputStream(filename));
			serializer.setIndent(4);
			serializer.setLineSeparator("\n");
			serializer.write(document);
		} catch (FileNotFoundException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
		} catch (IOException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
		}
		
	}
	
	private void createDocumentationRoot() {
		this.rootElement = new Element("doc");
	}

	private void createAssemblyName() {
		Element assembly = new Element("assembly");
		Element assemblyName = new Element("name");
		
		assemblyName.appendChild(this.projectName);
		assembly.appendChild(assemblyName);
		
		this.rootElement.appendChild(assembly);
	}

	private void createMembers() {
		this.membersElement = new Element("members");
		this.rootElement.appendChild(this.membersElement);
	}
}
