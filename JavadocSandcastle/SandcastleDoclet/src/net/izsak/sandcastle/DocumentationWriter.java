/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;


import net.izsak.sandcastle.tags.BlockTagConverter;
import net.izsak.sandcastle.tags.BlockTagFactory;
import net.izsak.sandcastle.tags.SummaryBlockTag;
import nu.xom.Attribute;
import nu.xom.Element;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.Doc;
import com.sun.javadoc.MemberDoc;
import com.sun.javadoc.PackageDoc;
import com.sun.javadoc.Tag;


/**
 * @author Jozef Izso
 *
 */
public class DocumentationWriter extends ApiWriterBase implements IApiWriter {

	protected Element elmMetadata;
	protected Element elmMembers;
	
	public DocumentationWriter() {
		this.elmMetadata = new Element("assembly");
		this.elmMembers = new Element("members");
	}

	
	@Override
	public void writePackage(PackageDoc packageDoc) {
		String qname = this.getApiNamer().getPackageName(packageDoc);
		this.addMember(qname, packageDoc);
	}

	@Override
	public void writeClass(ClassDoc classDoc) {
		String qname = this.getApiNamer().getClassName(classDoc);
		this.addMember(qname, classDoc);
	}

	@Override
	public void writeMember(MemberDoc memberDoc) {
		String qname = this.getApiNamer().getMemberName(memberDoc);
		this.addMember(qname, memberDoc);
	}

	public void writeMetadata(Object tmp) {
		// TODO: write project name
		Element name = new Element("name");
		name.appendChild("*** TMP Project Name ***");
		
		this.elmMetadata.appendChild(name);
	}
	
	protected void addMember(String qname, Doc doc) {
		Element elmMember = new Element("member");
		elmMember.addAttribute(new Attribute("name", qname));
		
		writeSummary(elmMember, doc);
		writeTags(elmMember, doc.tags());
		
		if (elmMember.getChildCount() > 0)
			this.elmMembers.appendChild(elmMember);
	}

	/**
	 * Writes the <summary> element with documentation and formats
	 * any inline tags found in it.
	 * 
	 * @param member Element to which the <summary> will be appended.
	 * @param doc    Documentation of the actual code element.
	 */
	private void writeSummary(Element member, Doc doc) {
		SummaryBlockTag summary = new SummaryBlockTag(doc, this.getApiNamer());
		Element elmSummary = summary.toXml();
		if (elmSummary != null)
			member.appendChild(elmSummary);
	}

	private static void writeTags(Element member, Tag[] tags) {
		for(Tag t : tags) {
			BlockTagConverter converter = BlockTagFactory.createConverter(t);
			Element elmTag = converter.toXml();
			if (elmTag != null)
				member.appendChild(elmTag);
		}
	}
	
	@Override
	protected XmlRootInfo getXmlRootInfo() {
		XmlRootInfo info = new XmlRootInfo();
		info.setRootElementName("doc");
		info.add(this.elmMetadata);
		info.add(this.elmMembers);
		return info;
	}
}
