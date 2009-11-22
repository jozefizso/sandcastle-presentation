/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.configuration;

import org.apache.commons.cli.Option;
import org.apache.commons.cli.Options;

/**
 * @author Jozef Izso
 *
 */
public class DocletOptions extends Options {
	
	public static final Option OUTPUT_FILE = new Option("doc", true, "Documentation output file name.");
	public static final Option PROJECT_NAME = new Option("project", true, "Project name.");
	public static final Option METADATA_FILE = new Option("metadata", true, "Path to file with metadata configuration.");
	
	private static final long serialVersionUID = 1L;

	public DocletOptions() {
		this.addOption((Option)OUTPUT_FILE.clone());
		this.addOption((Option)PROJECT_NAME.clone());
		this.addOption((Option)METADATA_FILE.clone());
	}
}
