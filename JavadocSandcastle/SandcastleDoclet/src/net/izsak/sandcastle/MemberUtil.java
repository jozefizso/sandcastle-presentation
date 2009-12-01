/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;


/**
 * @author Jozef Izso
 *
 */
public class MemberUtil {
	
	public static boolean isBeanProperty(String name) {
		if ((name.startsWith("set") || name.startsWith("get") || name.startsWith("has"))
				&& name.length() > 3) {
			return Character.isUpperCase(name.charAt(3));
		} else if (name.startsWith("is") && name.length() > 2) {
			return Character.isUpperCase(name.charAt(2));
		}
		
		return false;
	}
}
