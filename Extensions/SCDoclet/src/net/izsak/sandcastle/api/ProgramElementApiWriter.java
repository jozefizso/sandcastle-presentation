/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.api;

import java.lang.reflect.Modifier;

import net.izsak.sandcastle.ApiWriterContext;

import com.sun.javadoc.ProgramElementDoc;

/**
 * @author Jozef Izso
 *
 */
public class ProgramElementApiWriter extends CodeApiWriterBase {

	private ProgramElementDoc doc;
	
	/**
	 * @param context
	 * @param doc
	 */
	public ProgramElementApiWriter(ApiWriterContext context, ProgramElementDoc doc) {
		super(context, doc);
		this.doc = doc;
	}

	public boolean isFinal() {
		int mod = this.doc.modifierSpecifier();
		return Modifier.isFinal(mod);
	}

	public boolean isAbstract() {
		int mod = this.doc.modifierSpecifier();
		return Modifier.isAbstract(mod);
	}

	/**
	 * Allowed visibility types:
	 *   - public
	 *   - family
	 *   - assembly
	 *   - family or assembly
	 *   - family and assembly
	 *   - private
	 *   
	 * @param classDoc
	 * @return
	 */
	public String getVisibility() {
		int modifiers = this.doc.modifierSpecifier();
		
		if (Modifier.isPublic(modifiers)) {
			return "public";
		} else if (Modifier.isProtected(modifiers)) {
			return "family"; // protected
		} else if (Modifier.isPrivate(modifiers)) {
			return "private";
		}
		
		return "assembly";
	}
}