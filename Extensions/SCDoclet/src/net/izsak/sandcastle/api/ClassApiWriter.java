/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.api;

import java.lang.reflect.Modifier;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import net.izsak.sandcastle.IApiNamer;
import nu.xom.Attribute;
import nu.xom.Element;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.Doc;
import com.sun.javadoc.Type;

/**
 * @author Jozef Izso
 *
 */
public class ClassApiWriter extends MemberApiWriterBase {

	private ClassDoc classDoc;
	
	public ClassApiWriter(IApiNamer apiNamer, ClassDoc classDoc) {
		super(apiNamer);
		this.classDoc = classDoc;
	}

	public void write() {
		super.write(this.classDoc);
		
		this.writeApiData(this.classDoc.typeName(), "type",  getSubGroup());
		this.writeTypeData();
		this.writeInheritanceInfo();
		this.writeMembers(this.combineMembers());
	}
	
	public String getSubGroup() {
		return this.classDoc.isOrdinaryClass() ? "class" : "interface";
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
		int modifiers = this.classDoc.modifierSpecifier();
		
		if (Modifier.isPublic(modifiers)) {
			return "public";
		} else if (Modifier.isProtected(modifiers)) {
			return "family"; // protected
		} else if (Modifier.isPrivate(modifiers)) {
			return "private";
		}
		
		return "assembly";
	}
	
	public boolean isSealed() {
		int mod = this.classDoc.modifierSpecifier();
		return Modifier.isFinal(mod);
	}
	
	public boolean isAbstract() {
		int mod = this.classDoc.modifierSpecifier();
		return Modifier.isAbstract(mod);
	}
	
	private void writeTypeData() {
		String visibility = getVisibility();
		boolean sealed = isSealed();
		boolean abstr = isAbstract();
		
		Element elmTypeData = new Element("typedata");
		elmTypeData.addAttribute(new Attribute("visibility", visibility));
		if (sealed)
			elmTypeData.addAttribute(new Attribute("sealed", Boolean.toString(sealed)));
		if (abstr)
			elmTypeData.addAttribute(new Attribute("abstract", Boolean.toString(abstr)));
		
		this.addElement(elmTypeData);
	}

	private void writeInheritanceInfo() {
		Type superclass = this.classDoc.superclassType();
		if (superclass == null)
			return;
		
		String qname = this.getApiNamer().getClassName(superclass.asClassDoc());
		
		Element elmFamily = new Element("family");
		Element elmAncestors = new Element("ancestors");
		Element elmType = new Element("type");
		elmType.addAttribute(new Attribute("api", qname));
		// always true in Java
		elmType.addAttribute(new Attribute("ref", "true"));
		
		elmFamily.appendChild(elmAncestors);
		elmAncestors.appendChild(elmType);
		this.addElement(elmFamily);
	}
	
	private Doc[] combineMembers() {
		List<Doc> members = new ArrayList<Doc>(20);
		
		Collections.addAll(members, this.classDoc.constructors());
		Collections.addAll(members, this.classDoc.fields());
		Collections.addAll(members, this.classDoc.methods());
		
		return members.toArray(new Doc[members.size()]);
	}
}
