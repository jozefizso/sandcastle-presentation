/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.api;

import net.izsak.sandcastle.ApiWriterContext;
import net.izsak.sandcastle.LibraryInfo;
import nu.xom.Attribute;
import nu.xom.Element;

/**
 * Writes information about actual Java library into the
 * <assemblies> section.
 * 
 * @author Jozef Izso
 *
 */
public class LibraryApiWriter extends CodeApiWriterBase {

	/**
	 * @param context
	 */
	public LibraryApiWriter(ApiWriterContext context) {
		super(context, null);
	}
	
	public void writeLibraryData(Element elmManifest) {
		LibraryInfo lib = this.getLibrary();
		
		Element elmAssembly = new Element("assembly");
		Element elmData = new Element("assemblydata");
		
		elmAssembly.addAttribute(new Attribute("name", lib.getName()));
		elmData.addAttribute(new Attribute("version", lib.getVersion()));
		elmData.addAttribute(new Attribute("culture", ""));
		elmData.addAttribute(new Attribute("key", ""));
		elmData.addAttribute(new Attribute("hash", "SHA1"));
		
		elmAssembly.appendChild(elmData);
		elmManifest.appendChild(elmAssembly);
	}
}
