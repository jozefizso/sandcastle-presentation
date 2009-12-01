/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

/**
 * @author Jozef Izso
 *
 */
public class LibraryInfo {

	private String name;
	
	private String version;

	public LibraryInfo() {
		this.name = "";
		this.version = "";
	}
	
	public void setName(String name) {
		this.name = name;
	}

	public String getName() {
		return name;
	}

	public void setVersion(String version) {
		this.version = version;
	}

	public String getVersion() {
		return version;
	}
}
