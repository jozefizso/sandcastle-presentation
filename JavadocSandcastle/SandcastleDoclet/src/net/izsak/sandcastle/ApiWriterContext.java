/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;


/**
 * @author  Jozef Izso
 */
public class ApiWriterContext {
	private IApiNamer apiNamer;

	private LibraryInfo library;
	
	public ApiWriterContext(IApiNamer apiNamer, LibraryInfo library) {
		this.apiNamer = apiNamer;
		this.setLibrary(library);
	}

	public IApiNamer getApiNamer() {
		return this.apiNamer;
	}

	public void setApiNamer(IApiNamer apiNamer) {
		this.apiNamer = apiNamer;
	}

	public void setLibrary(LibraryInfo library) {
		this.library = library;
	}

	public LibraryInfo getLibrary() {
		return library;
	}
}