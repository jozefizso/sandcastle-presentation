/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.configuration;

import org.apache.commons.cli.CommandLine;
import org.apache.commons.cli.ParseException;
import org.apache.commons.configuration.Configuration;
import org.apache.commons.configuration.ConfigurationException;
import org.apache.commons.configuration.PropertiesConfiguration;

/**
 * @author Jozef Izso
 *
 */
public class DocletConfig {

	private DocletOptions options;
	
	private String docFileName;
	private String apiFileName;
	
	public DocletConfig() {
		this.options = new DocletOptions();
	}
	
	public void configure(String[][] javadocCmdLine) throws ParseException, ConfigurationException {
		DocletOptionsParser parser = new DocletOptionsParser();
		CommandLine cmd = parser.parse(this.options, javadocCmdLine);
		
		String configFile = cmd.getOptionValue(DocletOptions.CONFIG_FILE.getArgName());
		this.configureFromFile(configFile);
	}
	
	public void configureFromFile(String configFile) throws ConfigurationException {
		Configuration config = new PropertiesConfiguration(configFile);
		
		this.docFileName = config.getString("doc.output");
		this.apiFileName = config.getString("api.output");
	}
	
	public String getDocFileName() {
		return this.docFileName;
	}
	
	public String getApiFileName() {
		return this.apiFileName;
	}
	
	public boolean generateDocFile() {
		return getDocFileName() != null;
	}
	
	public boolean generateApiFile() {
		return getApiFileName() != null;
	}
}
