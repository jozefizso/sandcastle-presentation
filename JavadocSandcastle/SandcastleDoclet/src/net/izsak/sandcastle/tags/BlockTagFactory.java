/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.tags;

import net.izsak.sandcastle.DotNetApiNamer;
import net.izsak.sandcastle.IApiNamer;

import com.sun.javadoc.ParamTag;
import com.sun.javadoc.Tag;
import com.sun.javadoc.ThrowsTag;

/**
 * @author Jozef Izso
 *
 */
public class BlockTagFactory {
	public static BlockTagConverter createConverter(Tag blockTag) {
		IApiNamer apiNamer = new DotNetApiNamer();
		
		if (blockTag instanceof ThrowsTag) {
			return new ExceptionBlockTag((ThrowsTag)blockTag, apiNamer);
		}
		if (blockTag instanceof ParamTag) {
			return new ParamBlockTag((ParamTag)blockTag, apiNamer);
		}
		if ("@return".equals(blockTag.kind())) {
			return new ReturnsBlockTag(blockTag, apiNamer);
		}
		
		return new BlockTagConverter(blockTag, apiNamer);
	}
}
