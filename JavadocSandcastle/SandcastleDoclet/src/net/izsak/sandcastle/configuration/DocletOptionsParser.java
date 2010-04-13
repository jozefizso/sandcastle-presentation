/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.configuration;

import org.apache.commons.cli.CommandLine;
import org.apache.commons.cli.Options;
import org.apache.commons.cli.ParseException;
import org.apache.commons.cli.PosixParser;

/**
 * @author Jozef Izso
 *
 */
public class DocletOptionsParser extends PosixParser {
	
	public CommandLine parse(Options options, String[][] javadocOptions) throws ParseException	{
		String[] arguments = flattenArguments(javadocOptions);
		return super.parse(options, arguments);
	}
	
	private static int countDimmensions(String[][] options) {
		int dim = options.length;
		
		for (int i = 0; i < options.length; i++) {
			dim += options[i].length;
		}
		
		return dim;
	}

	private static String[] flattenArguments(String[][] source) {
		int dim = countDimmensions(source);
		String[] arguments = new String[dim];
		
		int k = 0;
		for (int i = 0; i < source.length; i++) {
			for (int j = 0; j < source[i].length; j++) {
				arguments[k++] = source[i][j];
			}
		}
		
		return arguments;
	}
	
}
