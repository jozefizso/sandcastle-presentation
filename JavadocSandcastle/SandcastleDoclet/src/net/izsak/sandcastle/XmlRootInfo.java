/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import java.util.ArrayList;
import java.util.List;

import nu.xom.Element;

/**
 * @author Jozef Izso
 *
 */
public class XmlRootInfo {
	private String rootElementName;
	private List<Element> elements;
	
	public XmlRootInfo() {
		this.elements = new ArrayList<Element>();
	}
	
	public void setRootElementName(String rootElementName) {
		this.rootElementName = rootElementName;
	}
	
	public String getRootElementName() {
		return rootElementName;
	}
	
	public void add(Element element) {
		this.elements.add(element);
	}
	
	public List<Element> getElements() {
		return elements;
	}
}
