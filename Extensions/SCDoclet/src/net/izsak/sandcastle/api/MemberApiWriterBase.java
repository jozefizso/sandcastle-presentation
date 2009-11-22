/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.api;

import net.izsak.sandcastle.IApiNamer;
import nu.xom.Attribute;
import nu.xom.Element;

import com.sun.javadoc.Doc;


/**
 * @author Jozef Izso
 *
 */
public class MemberApiWriterBase {

	private IApiNamer apiNamer;
	private Element api;
	
	public MemberApiWriterBase(IApiNamer apiNamer) {
		this.apiNamer = apiNamer;
		this.api = new Element("api");
	}
	
	public Element getApiElement() {
		return this.api;
	}
	
	public IApiNamer getApiNamer() {
		return this.apiNamer;
	}
	
	public void write(Doc doc) {
		String qname = this.getApiNamer().getApiName(doc);
		this.api.addAttribute(new Attribute("id", qname));
	}
	
	public void writeApiData(String simpleName, String group) {
		this.writeApiData(simpleName, group, null);
	}
	
	public void writeApiData(String simpleName, String group, String subGroup) {
		Element elmApiData = new Element("apidata");
		// simple name
		elmApiData.addAttribute(new Attribute("name", simpleName));
		elmApiData.addAttribute(new Attribute("group", group));
		if (subGroup != null)
			elmApiData.addAttribute(new Attribute("subgroup", subGroup));
		
		this.addElement(elmApiData);
	}
	
	public void writeMembers(Doc[] members) {
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
	
	protected void addElement(Element element) {
		this.getApiElement().appendChild(element);
	}
}
