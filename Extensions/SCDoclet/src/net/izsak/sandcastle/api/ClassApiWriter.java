/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.api;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import net.izsak.sandcastle.ApiWriterContext;
import nu.xom.Attribute;
import nu.xom.Element;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.Doc;

/**
 * @author Jozef Izso
 *
 */
public class ClassApiWriter extends ProgramElementApiWriter {

	ClassDoc classDoc;
	
	public ClassApiWriter(ApiWriterContext context, ClassDoc classDoc) {
		super(context, classDoc);
		this.classDoc = classDoc;
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.api.MemberApiWriterBase#writeApiData()
	 */
	@Override
	public void writeApiData() {
		super.writeApiData();
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.api.CodeApiWriterBase#writeOtherData()
	 */
	@Override
	protected void writeOtherData() {
		super.writeOtherData();
		
		this.writeTypeData();
		this.writeInheritanceInfo();
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.api.MemberApiWriterBase#writeMembers()
	 */
	@Override
	protected void writeMembers() {
		super.writeMembers(this.combineMembers());
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.api.MemberApiWriterBase#writeContainersCore(nu.xom.Element)
	 */
	@Override
	protected void writeContainersCore(Element elmContainers) {
		super.writeNamespaceToContainers(elmContainers, this.classDoc.containingPackage());
	}
	
	public String getSubGroup() {
		return this.classDoc.isOrdinaryClass() ? "class" : "interface";
	}
	
	private void writeTypeData() {
		String visibility = getVisibility();
		boolean sealed = isFinal();
		boolean abstr = isAbstract();
		
		Element elmTypeData = new Element("typedata");
		elmTypeData.addAttribute(new Attribute("visibility", visibility));
		elmTypeData.addAttribute(new Attribute("sealed", Boolean.toString(sealed)));
		elmTypeData.addAttribute(new Attribute("abstract", Boolean.toString(abstr)));
		elmTypeData.addAttribute(new Attribute("serializable", Boolean.toString(false)));
		
		this.addElement(elmTypeData);
	}

	/**
	 * Generates a list of super classes.
	 * <family>
     *  <ancestors>
     *    <type api="T:java.lang.Object" ref="true" />
     *  </ancestors>
     * </family>
	 */
	private void writeInheritanceInfo() {
		ClassDoc superclass = this.classDoc.superclass();
		if (superclass == null)
			return;

		Element elmFamily = new Element("family");
		Element elmAncestors = new Element("ancestors");
		
		writeTypeInfo(superclass, elmAncestors);
		
		elmFamily.appendChild(elmAncestors);
		this.addElement(elmFamily);
	}

	/**
	 * Combines constructors, methods and fields into one array
	 * so an list of members defined for a class can be created.
	 * @return
	 */
	private Doc[] combineMembers() {
		List<Doc> members = new ArrayList<Doc>(20);
		
		Collections.addAll(members, this.classDoc.constructors());
		Collections.addAll(members, this.classDoc.fields());
		Collections.addAll(members, this.classDoc.methods());
		
		return members.toArray(new Doc[members.size()]);
	}
}
