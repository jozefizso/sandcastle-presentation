/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import com.sun.javadoc.ParamTag;
import com.sun.javadoc.Tag;
import com.sun.javadoc.ThrowsTag;

/**
 * @author Jozef Izso
 *
 */
public class BlockTagFactory {
	public static BlockTagConverter createConverter(Tag blockTag) {
		if (blockTag instanceof ThrowsTag) {
			return new ExceptionBlockTag((ThrowsTag)blockTag);
		}
		if (blockTag instanceof ParamTag) {
			return new ParamBlockTag((ParamTag)blockTag);
		}
		else {
			return new BlockTagConverter(blockTag);
		}
	}
}
