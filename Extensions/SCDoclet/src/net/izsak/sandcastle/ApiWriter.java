/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import net.izsak.sandcastle.api.ClassApiWriter;
import net.izsak.sandcastle.api.CodeApiWriterBase;
import net.izsak.sandcastle.api.LibraryApiWriter;
import net.izsak.sandcastle.api.MemberApiWriter;
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

	private LibraryInfo library;
	private Element elmMetadata;
	private Element elmApis;
	
	public ApiWriter() {
		this.elmMetadata = new Element("assemblies");
		this.elmApis = new Element("apis");
		
		this.library = new LibraryInfo();
		this.library.setName("Test library");
		this.library.setVersion("1.0.0");
		new ApiWriterContext(this.getApiNamer(), this.library);
	}

	@Override
	public void writeMetadata(Object tmp) {
		LibraryApiWriter law = new LibraryApiWriter(this.getContext());
		law.writeLibraryData(this.elmMetadata);
	}
	
	@Override
	public void writePackage(PackageDoc packageDoc) {
		PackageApiWriter paw = new PackageApiWriter(this.getContext(), packageDoc);
		paw.write();
		
		this.addMember(paw);
	}

	@Override
	public void writeClass(ClassDoc classDoc) {
		ClassApiWriter caw = new ClassApiWriter(this.getContext(), classDoc);
		caw.write();
		this.addMember(caw);
	}
	
	@Override
	public void writeMember(MemberDoc memberDoc) {
		MemberApiWriter maw = new MemberApiWriter(this.getContext(), memberDoc);
		maw.write();
		this.addMember(maw);
	}
	
	protected void addMember(CodeApiWriterBase writer) {
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
	
	private ApiWriterContext getContext() {
		return new ApiWriterContext(this.getApiNamer(), this.library);
	}
}
