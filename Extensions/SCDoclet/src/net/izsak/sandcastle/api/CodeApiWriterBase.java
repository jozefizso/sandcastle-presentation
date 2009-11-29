/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.api;

import net.izsak.sandcastle.ApiWriterContext;
import net.izsak.sandcastle.IApiNamer;
import net.izsak.sandcastle.LibraryInfo;
import nu.xom.Attribute;
import nu.xom.Element;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.Doc;
import com.sun.javadoc.MemberDoc;
import com.sun.javadoc.PackageDoc;
import com.sun.javadoc.ParameterizedType;
import com.sun.javadoc.Type;
import com.sun.javadoc.TypeVariable;


/**
 * @author Jozef Izso
 *
 */
public class CodeApiWriterBase {

	private IApiNamer apiNamer;
	private LibraryInfo library;
	private Doc doc;
	private Element api;
	
	public CodeApiWriterBase(ApiWriterContext context, Doc doc) {
		this.apiNamer = context.getApiNamer();
		this.library = context.getLibrary();
		this.doc = doc;
		
		this.api = new Element("api");
	}
	
	public Element getApiElement() {
		return this.api;
	}
	
	public IApiNamer getApiNamer() {
		return this.apiNamer;
	}
	
	public LibraryInfo getLibrary() {
		return this.library;
	}
	
	protected Doc getDoc() {
		return this.doc;
	}
	
	public void write() {
		this.write(this.getDoc());
		
		this.writeApiData();
		this.writeOtherData();
		this.writeMembers();
		this.writeContainers();
	}
	
	protected void write(Doc doc) {
		if (doc == null)
			throw new IllegalStateException("Argument doc cannot be null.");
		
		String qname = this.getApiNamer().getApiName(doc);
		this.api.addAttribute(new Attribute("id", qname));
	}

	protected void writeMembers() {
	}
	
	protected void writeApiData() {
		this.writeApiData(this.getSimpleName(), this.getGroup(), this.getSubgroup());
	}
	
	protected void writeApiData(String simpleName, String group) {
		this.writeApiData(simpleName, group, null);
	}
	
	/**
	 * 
	 * Note: reflection.xsd support 4th parameter: subSubGroup. This is
	 * used for operators overloading and it is no applicable in Java.
	 * 
	 * @param simpleName   Simple name of the current code element.
	 * @param group        Can be one of the following types: namespace, type or member.
	 * @param subGroup     Subgroup defining concrete type of the code element.
	 *                     Can be on of the following values: class, structure, interface,
	 *                     enumeration, delegate, constructor, method, property, field,
	 *                     event.
	 */
	protected void writeApiData(String simpleName, String group, String subGroup) {
		if (!Validator.isValidateGroupName(group))
			throw new IllegalArgumentException("Argument group cannot have value "+ group +".");
		if (subGroup != null && !Validator.isValidSubGroupName(subGroup))
			throw new IllegalArgumentException("Argument subGroup cannot have value "+ subGroup +".");
		
		Element elmApiData = new Element("apidata");
		// simple name
		elmApiData.addAttribute(new Attribute("name", simpleName));
		elmApiData.addAttribute(new Attribute("group", group));
		if (subGroup != null)
			elmApiData.addAttribute(new Attribute("subgroup", subGroup));
		
		this.addElement(elmApiData);
	}

	protected void writeOtherData() {
	}
	
	protected void writeMembers(Doc[] members) {
		Element elmElements = new Element("elements");
		
		for (Doc member : members) {
			Element elm = new Element("element");
			String qname = this.getApiNamer().getApiName(member);
			if (qname == null)
				qname = "";
			elm.addAttribute(new Attribute("api", qname));
			
			elmElements.appendChild(elm);
		}
		
		this.addElement(elmElements);
	}
	
	protected void writeContainers() {
		Element elmContainers = new Element("containers");
		Element elmLib = new Element("library");
		
		String name = this.library.getName();
		elmLib.addAttribute(new Attribute("assembly", name));
		elmLib.addAttribute(new Attribute("module", name));
		elmLib.addAttribute(new Attribute("kind", "DynamicallyLinkedLibrary"));
		
		elmContainers.appendChild(elmLib);
		this.writeContainersCore(elmContainers);
		
		this.addElement(elmContainers);
	}
	
	protected void writeContainersCore(Element elmContainers) {
	}
	
	protected void addElement(Element element) {
		this.getApiElement().appendChild(element);
	}


	protected String getSimpleName() {
		return this.doc.name();
	}

	protected String getGroup() {
		if (this.doc.isClass() || this.doc.isInterface())
			return "type";
		if (this.doc instanceof MemberDoc)
			return "member";
		if (this.doc instanceof PackageDoc)
			return "namespace";
		
		throw new IllegalStateException("Group type for "+ this.doc.name() +" is not supported.");
	}
	
	protected String getSubgroup() {
		if (this.doc.isField())
			return "field";
		if (this.doc.isConstructor())
			return "constructor";
		if (this.doc.isMethod())
			return "method";
		if (this.doc.isOrdinaryClass() || this.doc.isException())
			return "class";
		if (this.doc.isEnum())
			return "enum";
		if (this.doc.isInterface())
			return "interface";
		
		throw new IllegalStateException("SubGroup type for "+ this.doc.name() +" is not supported.");
	}
	

	protected void writeNamespaceToContainers(Element elmContainers, PackageDoc packageDoc) {
		String qname = this.getApiNamer().getPackageName(packageDoc);
		
		Element elmNamespace = new Element("namespace");
		elmNamespace.addAttribute(new Attribute("api", qname));
		elmContainers.appendChild(elmNamespace);
	}

	protected void writeTypeInfo(Type type, Element elmContainer) {
		if (type == null) {
			return;
		}
		String dim = type.dimension();
		if (dim.length() == 0) {
			this.writeSimpleTypeInfo(type, elmContainer);
		}
		else {
			this.writeArrayTypeInfo(type, elmContainer);
		}
	}
	
	protected void writeSimpleTypeInfo(Type type, Element elmContainer) {
		String qname = this.getApiNamer().getApiName(type);
		boolean isRef = !type.isPrimitive();
		boolean isParametrized = type.asParameterizedType() != null;
		
		Element elmType = new Element("type");
		elmType.addAttribute(new Attribute("api", qname));
		elmType.addAttribute(new Attribute("ref", Boolean.toString(isRef)));
		if (isParametrized)
			this.writeGenericTypeSpecializationInfo(type, elmType);
		elmContainer.appendChild(elmType);
	}
	
	protected void writeArrayTypeInfo(Type type, Element elmContainer) {
		ClassDoc arrayType = type.asClassDoc();
		String dim = type.dimension();
		
		// HACK: very weird....
		String[] split = dim.split("]");
		int num = split.length;
		
		Element elmArrayOf = new Element("arrayOf");
		elmArrayOf.addAttribute(new Attribute("rank", Integer.toString(num)));
		if (type.isPrimitive())
			this.writeSimpleTypeInfo(type, elmArrayOf);
		else
			this.writeTypeInfo(arrayType, elmArrayOf);
		
		elmContainer.appendChild(elmArrayOf);
	}
	
	protected void writeGenericTypeSpecializationInfo(Type type, Element elmContainer) {
		ParameterizedType param = type.asParameterizedType();
		Type[] args = param.typeArguments();
		
		Element elmSpec = new Element("specialization");
		
		for (Type paramType : args) {
			if (paramType.typeName().equals("?")) {
				Element elmTemplate = new Element("template");
				elmTemplate.addAttribute(new Attribute("name", "?"));
				elmSpec.appendChild(elmTemplate);
				continue;
			}
			
			TypeVariable typeVar = paramType.asTypeVariable();
			if (typeVar != null)
				this.writeTypeVariable(typeVar, elmSpec);
			else
				this.writeTypeInfo(paramType, elmSpec);
		}
		
		if (elmSpec.getChildCount() > 0)
			elmContainer.appendChild(elmSpec);
	}
	
	/**
	 * Writes template information about generic typed variable.
	 * <code><template name="E" api="T:net.izsak.GenericsClass{T,E}"/></code>
	 * 
	 * @param typeVariable
	 * @param elmContainer  Element to which the <template> will be added.
	 * @return Returns the <template> element.
	 */
	protected Element writeTypeVariable(TypeVariable typeVariable, Element elmContainer) {
		String name = typeVariable.typeName();
		
		Element elmTemplate = new Element("template");
		elmTemplate.addAttribute(new Attribute("name", name));
		this.writeTypeVariableType(typeVariable, elmTemplate);
		
		elmContainer.appendChild(elmTemplate);
		
		return elmTemplate;
	}
	
	protected void writeTypeVariableType(TypeVariable typeVariable, Element elmTemplate) {
		Doc owner = typeVariable.owner();
		String qname = this.getApiNamer().getApiName(owner);
		elmTemplate.addAttribute(new Attribute("api", qname));
	}
}
