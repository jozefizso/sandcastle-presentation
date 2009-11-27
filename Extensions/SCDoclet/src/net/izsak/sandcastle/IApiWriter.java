/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import nu.xom.Document;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.MemberDoc;
import com.sun.javadoc.PackageDoc;

/**
 * @author Jozef Izso
 *
 */
public interface IApiWriter {
	
	public void setApiNamer(IApiNamer apiNamer);
	
	public void writeMetadata(Object tmp);
	
	public void writePackage(PackageDoc packageDoc);
	
	public void writeClass(ClassDoc classDoc);
	
	public void writeMember(MemberDoc memberDoc);
	
	public Document toXmlDocument();
	
	public void saveXml(String filename);
}
