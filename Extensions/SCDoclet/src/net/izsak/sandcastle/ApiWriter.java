/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import net.izsak.sandcastle.api.ClassApiWriter;
import net.izsak.sandcastle.api.MemberApiWriterBase;
import net.izsak.sandcastle.api.PackageApiWriter;
import nu.xom.Element;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.MemberDoc;
import com.sun.javadoc.PackageDoc;


/**
 * @author Jozef Izso
 *
 */
public class ApiWriter extends ApiWriterBase implements IApiWriter {

	protected Element elmMetadata;
	protected Element elmApis;
	
	public ApiWriter() {
		this.elmMetadata = new Element("assemblies");
		this.elmApis = new Element("apis");
	}
	
	@Override
	public void writePackage(PackageDoc packageDoc) {
		PackageApiWriter paw = new PackageApiWriter(this.getApiNamer());
		paw.write(packageDoc);
		
		this.addMember(paw);
	}

	@Override
	public void writeClass(ClassDoc classDoc) {
		ClassApiWriter caw = new ClassApiWriter(this.getApiNamer(), classDoc);
		caw.write();
		this.addMember(caw);
	}
	@Override
	public void writeMember(MemberDoc memberDoc) {
		//String qname = this.getApiNamer().getMemberName(memberDoc);
		//this.addMember(qname, memberDoc);
	}
	
	protected void addMember(MemberApiWriterBase writer) {
		Element elmApi = writer.getApiElement();
		this.elmApis.appendChild(elmApi);
	}

	@Override
	protected XmlRootInfo getXmlRootInfo() {
		XmlRootInfo info = new XmlRootInfo();
		info.setRootElementName("reflection");
		info.add(this.elmMetadata);
		info.add(this.elmApis);
		return info;
	}

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.IApiWriter#writeMetadata(java.lang.Object)
	 */
	@Override
	public void writeMetadata(Object tmp) {
	}

}
