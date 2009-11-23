/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.ConstructorDoc;
import com.sun.javadoc.Doc;
import com.sun.javadoc.FieldDoc;
import com.sun.javadoc.MemberDoc;
import com.sun.javadoc.MethodDoc;
import com.sun.javadoc.PackageDoc;
import com.sun.javadoc.Parameter;
import com.sun.javadoc.Type;

/**
 * Implements {@link IApiNamer} interfaces providing names
 * of elements in the .NET documentation style.
 * 
 * @author Jozef Izso
 *
 */
public class DotNetApiNamer implements IApiNamer {
	
	public String getApiName(Type type) {
		String qn = type.qualifiedTypeName();
		qn = "T:" + qn;
		return qn;
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.IApiNamer#getMemberName(com.sun.javadoc.Doc)
	 */
	@Override
	public String getApiName(Doc member) {
		if (member.isClass()) {
			return this.getClassName((ClassDoc)member);
		}
		else if (member.isMethod()) {
			return this.getMemberName((MemberDoc)member);
		}
		else if (member.isConstructor()) {
			return this.getMemberName((ConstructorDoc)member);
		}
		else if (member.isField()) {
			return this.getFieldName((FieldDoc)member);
		}
		else if (member instanceof PackageDoc) {
			return this.getPackageName((PackageDoc)member);
		}
		return null;
	}
	
	/**
	 * Returns fully qualified class name preceeded with T prefix.
	 * Example: <code>T:net.izsak.sandcastle.DotNetApiNamer</code>
	 */
	@Override
	public String getClassName(ClassDoc classDoc) {
		String qn = classDoc.qualifiedName();
		qn = "T:" + qn;
		return qn;
	}

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.IApiNamer#getMemberName(com.sun.javadoc.MemberDoc)
	 */
	@Override
	public String getMemberName(MemberDoc memberDoc) {
		if (memberDoc.isField())
			return getFieldName((FieldDoc) memberDoc);
		
		String qn = memberDoc.qualifiedName();
		qn = "M:" + qn;
		
		if (memberDoc.isConstructor())
			qn = qn + ".#ctor";
		
		if (memberDoc.isMethod()) {
			MethodDoc md = (MethodDoc)memberDoc;
			if (md.parameters().length > 0) {
				StringBuilder sb = new StringBuilder(256);
				sb.append(qn);
				sb.append("(");
				
				for (Parameter p : md.parameters()) {
					if (p.type().isPrimitive())
						sb.append(p.typeName());
					else
						sb.append(p.type().asClassDoc().qualifiedName());
					
					if (p.type().dimension().length() > 0)
						sb.append(p.type().dimension());
					
					sb.append(",");
				}
				sb.setCharAt(sb.length()-1, ')');
				qn = sb.toString();
			}
		}
		
		return qn;
	}

	public String getFieldName(FieldDoc fieldDoc) {
		String qn = fieldDoc.qualifiedName();
		qn = "F:" + qn;
		return qn;
	}
	
	/**
	 * Returns fully qualified package name preceeded with N prefix.
	 * Example: <code>N:net.izsak.sandcastle</code>
	 */
	@Override
	public String getPackageName(PackageDoc packageDoc) {
		String qn = packageDoc.name();
		qn = "N:" + qn;
		return qn;
	}

}
