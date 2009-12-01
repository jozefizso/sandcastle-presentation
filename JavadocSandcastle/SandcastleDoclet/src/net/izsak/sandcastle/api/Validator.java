/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.api;

import java.util.Arrays;
import java.util.List;

/**
 * @author Jozef Izso
 *
 */
public class Validator {
	private static final List<String> AllowedGroupNames = Arrays.asList("namespace", "type", "member"); 
	private static final List<String> AllowedSubGroupNames = Arrays.asList("class", "interface",
			"constructor", "method", "property", "field", "enumeration", "structure", "delegate", 
			"event");
	
	public static boolean isValidateGroupName(String group) {
		return AllowedGroupNames.contains(group);
	}

	public static boolean isValidSubGroupName(String subgroup) {
		return AllowedSubGroupNames.contains(subgroup);
	}
}
