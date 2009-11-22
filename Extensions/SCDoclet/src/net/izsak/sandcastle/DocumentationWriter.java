/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;

import net.izsak.sandcastle.tags.BlockTagConverter;
import net.izsak.sandcastle.tags.BlockTagFactory;
import net.izsak.sandcastle.tags.SummaryBlockTag;
import nu.xom.Attribute;
import nu.xom.Document;
import nu.xom.Element;
import nu.xom.Serializer;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.Doc;
import com.sun.javadoc.MemberDoc;
import com.sun.javadoc.PackageDoc;
import com.sun.javadoc.Tag;

/**
 * @author Jozef Izso
 *
 */
public class DocumentationWriter implements IApiWriter {
	
	private IApiNamer apiNamer;
	private Element elmMetadata;
	private Element elmMembers;

	public DocumentationWriter() {
		this.elmMetadata = new Element("assembly");
		this.elmMembers = new Element("members");
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.IApiWriter#setApiNamer(net.izsak.sandcastle.IApiNamer)
	 */
	@Override
	public void setApiNamer(IApiNamer apiNamer) {
		this.apiNamer = apiNamer;
	}

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.IApiWriter#writeMetadata(java.lang.Object)
	 */
	@Override
	public void writeMetadata(Object tmp) {
		// TODO: write project name
		Element name = new Element("name");
		name.appendChild("*** TMP Project Name ***");
		
		this.elmMetadata.appendChild(name);
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.IApiWriter#writePackage(com.sun.javadoc.PackageDoc)
	 */
	@Override
	public void writePackage(PackageDoc packageDoc) {
		String qname = this.apiNamer.getPackageName(packageDoc);
		this.addMember(qname, packageDoc);
	}

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.IApiWriter#writeClass(com.sun.javadoc.ClassDoc)
	 */
	@Override
	public void writeClass(ClassDoc classDoc) {
		String qname = this.apiNamer.getClassName(classDoc);
		this.addMember(qname, classDoc);
	}

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.IApiWriter#writeMember(com.sun.javadoc.MemberDoc)
	 */
	@Override
	public void writeMember(MemberDoc memberDoc) {
		String qname = this.apiNamer.getMemberName(memberDoc);
		this.addMember(qname, memberDoc);
	}

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.IApiWriter#toXml()
	 */
	@Override
	public String toXml() {
		Element elmRoot = this.createXmlRoot();
		Document document = new Document(elmRoot);
		return document.toXML();
	}

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.IApiWriter#saveXml(java.lang.String)
	 */
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
	
	private void addMember(String qname, Doc doc) {
		Element elmMember = new Element("member");
		elmMember.addAttribute(new Attribute("name", qname));
		
		writeSummary(elmMember, doc);
		writeTags(elmMember, doc.tags());
		
		this.elmMembers.appendChild(elmMember);
	}
	
	private static void writeSummary(Element member, Doc doc) {
		SummaryBlockTag summary = new SummaryBlockTag(doc);
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
	
	private Element createXmlRoot() {
		Element elmRoot = new Element("doc");
		elmRoot.appendChild(this.elmMetadata);
		elmRoot.appendChild(this.elmMembers);
		return elmRoot;
	}
}
