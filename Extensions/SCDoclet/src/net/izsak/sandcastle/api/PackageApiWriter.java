/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.api;

import net.izsak.sandcastle.IApiNamer;

import com.sun.javadoc.PackageDoc;

/**
 * @author Jozef Izso
 *
 */
public class PackageApiWriter extends MemberApiWriterBase {
	
	public PackageApiWriter(IApiNamer apiNamer) {
		super(apiNamer);
	}

	public void write(PackageDoc packageDoc) {
		super.write(packageDoc);
		this.writeApiData(packageDoc.name(), "namespace");
		this.writeMembers(packageDoc.allClasses());
	}
}
