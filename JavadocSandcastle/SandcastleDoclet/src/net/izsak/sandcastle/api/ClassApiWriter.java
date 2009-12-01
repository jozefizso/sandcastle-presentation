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
import nu.xom.Node;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.Doc;
import com.sun.javadoc.Type;
import com.sun.javadoc.TypeVariable;

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
		this.writeTemplates();
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
	 * Templates represents generic parameters defined on a class.
	 * <code>class Generic<T extends ArrayList<E> & List<E>, E></code>
	 * 
	 * This class has two generic (type) parameters: T and E.
	 * Parameter T must extend class ArrayList<E> and must implement List<E>.
	 * These constraints will be stored in Reflection XML format inside <constrained> element.
	 * <constrained> element contains one <type> element for a class that T parameter
	 * must extend and a one <type> element inside <implements> for each interface
	 * it must implement.
	 * 
	 * Example code:
	 * <templates>
     *   <template name="T">
     *     <constrained>
     *       <!-- reference to class ArrayList<E> --> 
     *       <type api="T:java.util.ArrayList" ref="true">
     *         <specialization>
     *           <template name="E" api="T:net.izsak.GenericsClass{T,E}" />
     *         </specialization>
     *       </type>
     *       <implements>
     *         <!-- reference to interface List<E> -->
     *         <type api="T:java.util.ArrayList" ref="true">
     *           <specialization>
     *             <template name="E" api="T:net.izsak.GenericsClass{T,E}"/>
     *           </specialization>
     *         </type>
     *       </implements>
     *     </constrained>
     *   </template>
     *   <template name="E"/>
     * </templates>
	 */
	private void writeTemplates() {
		TypeVariable[] params = this.classDoc.typeParameters();
		
		if (params == null || params.length == 0)
			return;
		
		Element elmTemplates = new Element("templates");
		
		for (TypeVariable var : params) {
			Element template = this.writeTypeVariable(var, elmTemplates);
			// <template> elements on this level don't have api="" attribute so remove it.
			Attribute att = template.getAttribute("api");
			if (att != null)
				template.removeAttribute(att);
		}
		
		this.addElement(elmTemplates);
	}

	@Override
	protected void writeTypeVariableType(TypeVariable typeVariable, Element elmTemplate) {
		super.writeTypeVariableType(typeVariable, elmTemplate);
		
		Element elmConstr = new Element("constrained");
		Element elmImplements = new Element("implements");
		
		// Doc owner = typeVariable.owner();
		// Type[] bounds = typeVariable.bounds();
		//HACK: usage of private API: 
//		ClassDocImpl cd = (ClassDocImpl)typeVariable.asClassDoc();
//		
//		Type classBound = cd.superclassType();
//		if (classBound != null)
//			this.writeTypeInfo(classBound, elmConstr);
//		
//		Type[] bounds = cd.interfaceTypes();
		
		Type[] bounds = typeVariable.bounds();
		if (bounds.length == 0)
			return;
		
		for (Type bound : bounds) {
			Element container = bound.asClassDoc().isInterface() ? elmImplements : elmConstr;
			this.writeTypeInfo(bound, container);
		}
		
		elmConstr.appendChild(elmImplements);
		elmTemplate.appendChild(elmConstr);
		// NOTE: perhaps a bug in MRefBuilder - it generates <implements> element twice
		// one is inside <constrained> the other one is its descedant.
		Node elmImpl2 = elmImplements.copy();
		elmTemplate.appendChild(elmImpl2);
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
