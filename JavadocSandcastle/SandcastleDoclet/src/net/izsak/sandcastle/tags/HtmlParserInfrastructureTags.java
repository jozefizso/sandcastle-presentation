/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.tags;

import java.util.HashMap;
import java.util.Map;

/**
 * @author Jozef Izso
 *
 */
public class HtmlParserInfrastructureTags {
	
	private Map<String, Boolean> tags;
	private boolean allTagsWritten;
	
	@SuppressWarnings("serial")
	public HtmlParserInfrastructureTags() {
		this.tags = new HashMap<String, Boolean>() {{
			put("html", false);
			put("head", false);
			put("body", false);
		}};
		this.allTagsWritten = false;
	}
	
	public boolean isInfrastrucuteTag(String localName) {
		return this.tags.containsKey(localName);
	}
	
	public void markTagAsUsed(String localName) {
		this.tags.put(localName, true);
		
		for (Boolean b : this.tags.values()) {
			if (b == false)
				return;
		}
		this.allTagsWritten = true;
	}
	
	public boolean allTagsWritten() {
		return this.allTagsWritten;
	}
}
