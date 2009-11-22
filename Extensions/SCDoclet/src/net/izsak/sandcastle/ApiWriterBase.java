/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;

import nu.xom.Document;
import nu.xom.Element;
import nu.xom.Serializer;

/**
 * @author Jozef Izso
 *
 */
public abstract class ApiWriterBase implements IApiWriter {

	private IApiNamer apiNamer;

	public void setApiNamer(IApiNamer apiNamer) {
		this.apiNamer = apiNamer;
	}
	
	public IApiNamer getApiNamer() {
		return this.apiNamer;
	}

	@Override
	public String toXml() {
		Element elmRoot = this.createXmlRoot();
		Document document = new Document(elmRoot);
		return document.toXML();
	}

	@Override
	public void saveXml(String filename) {
		Element elmRoot = this.createXmlRoot();
		Document document = new Document(elmRoot);
		
		try {
			Serializer serializer = new Serializer(new FileOutputStream(filename));
			serializer.setIndent(4);
			serializer.setLineSeparator("\n");
			serializer.write(document);
		} catch (FileNotFoundException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
		} catch (IOException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
		}
	}

	protected abstract XmlRootInfo getXmlRootInfo();
	
	private Element createXmlRoot() {
		XmlRootInfo info = this.getXmlRootInfo();
		
		Element elmRoot = new Element(info.getRootElementName());
		for(Element e : info.getElements())
			elmRoot.appendChild(e);
		
		return elmRoot;
	}
}