/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import com.sun.javadoc.ExecutableMemberDoc;
import com.sun.javadoc.Parameter;

/**
 * @author Jozef Izso
 *
 */
public abstract class ExecutableMember<T extends ExecutableMemberDoc> extends Member<T> {

	/**
	 * @param doc
	 */
	public ExecutableMember(T doc, String parentName) {
		super(doc, parentName);
	}
	

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.Member#getTypeChar()
	 */
	@Override
	public String getTypeChar() {
		return "M";
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.Member#getFullName()
	 */
	@Override
	public String getFullName() {
		if (this.getDoc().parameters().length == 0)
			return super.getFullName();
		else {
			String fullName = super.getFullName();
			
			return fullName + buildMethodNameByParams(this.getDoc().parameters());
		}
	}


	protected String buildMethodNameByParams(Parameter[] params) {
		if (params.length == 0)
			return "";
		
		StringBuilder sb = new StringBuilder(256);
		sb.append("(");
		
		for (Parameter p : params) {
			sb.append(p.typeName());
			sb.append(",");
		}
		sb.setCharAt(sb.length()-1, ')');
		
		return sb.toString();
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.Member#processTag(com.sun.javadoc.Tag, java.lang.String)
	 */
//	@Override
//	protected Element processTag(Tag tag, String name) {
//		if (tag instanceof ParamTag) {
//			ParamTag pt = (ParamTag)tag;
//			Element elmParam = new Element("param");
//			elmParam.addAttribute(new Attribute("name", pt.parameterName()));
//			elmParam.appendChild(pt.parameterComment());
//			return elmParam;
//		}
//		else
//			return super.processTag(tag, name);
//	}
}
