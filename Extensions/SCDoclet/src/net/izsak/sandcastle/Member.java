/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import net.izsak.sandcastle.tags.BlockTagConverter;
import net.izsak.sandcastle.tags.BlockTagFactory;
import net.izsak.sandcastle.tags.SummaryBlockTag;
import nu.xom.Attribute;
import nu.xom.Element;

import com.sun.javadoc.Doc;
import com.sun.javadoc.Tag;

/**
 * @author Jozef Izso
 *
 */
public abstract class Member<T extends Doc> {
	
	private T doc;
	private String parentName;
	
	public Member(T doc) {
		this(doc, "");
	}
	
	public Member(T doc, String parentName) {
		this.doc = doc;
		this.parentName = (parentName == null) ? "" : parentName;
	}
	
	public T getDoc() {
		return this.doc;
	}
	
	public String getFullName() {
		StringBuilder sb = new StringBuilder(256);
		if (this.parentName.length() > 0) {
			sb.append(this.parentName);
			sb.append(".");
		}
		sb.append(this.getName());
		
		return sb.toString();
	}
	
	public String getName() {
		return this.getDoc().name();
	}
	
	public String getQualifiedMemberName() {
		return getTypeChar() + ":" + getFullName();
	}
	
	/**
	 * 
	 * @return Character based on the type of the member:
	 *         N - package (namespace)
	 *         T - class (type)
	 *         M - method
	 *         P - getter/setter bean (property)
	 */
	public abstract String getTypeChar();
	
	protected void addContent(Element members) {
	}
	
	protected abstract void processChildMembers(Element members);
	
	public void processMemberAndChilds(Element members) {
		Element elm = new Element("member");
		elm.addAttribute(new Attribute("name", getQualifiedMemberName()));
		
		if (this.doc.getRawCommentText().length() > 0) {
			SummaryBlockTag summary = new SummaryBlockTag(this.doc); 
			Element elmSummary = summary.toXml();
			if (elmSummary != null)
				elm.appendChild(elmSummary);
		}
		this.processTags(elm);
		addContent(elm);
		
		members.appendChild(elm);
		processChildMembers(members);
	}
	
	protected void processTags(Element member) {
		for(Tag t : this.doc.tags()) {
			BlockTagConverter converter = BlockTagFactory.createConverter(t);
			Element elmTag = converter.toXml();
			if (elmTag != null)
				member.appendChild(elmTag);
		}
	}
}
