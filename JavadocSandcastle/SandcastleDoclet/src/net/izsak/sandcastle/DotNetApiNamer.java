/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import com.sun.javadoc.*;

/**
 * Implements {@link IApiNamer} interfaces providing names
 * of elements in the .NET documentation style.
 * 
 * @author Jozef Izso
 *
 */
public class DotNetApiNamer implements IApiNamer {
	
	public String getApiName(Type type) {
		if (type instanceof Doc)
			return getApiName((Doc)type);
		
		String qn = type.qualifiedTypeName();
		qn = "T:" + qn;
		return qn;
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.IApiNamer#getMemberName(com.sun.javadoc.Doc)
	 */
	@Override
	public String getApiName(Doc member) {
		if (member.isClass() || member.isInterface()) {
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
		StringBuilder sb = new StringBuilder(256);
		
		String qn = classDoc.qualifiedName();
		sb.append("T:");
		sb.append(qn);
		this.addGenericTypes(classDoc.typeParameters(), sb);
		return sb.toString();
	}

	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.IApiNamer#getMemberName(com.sun.javadoc.MemberDoc)
	 */
	@Override
	public String getMemberName(MemberDoc memberDoc) {
		if (memberDoc.isField())
			return getFieldName((FieldDoc) memberDoc);
		
		if (memberDoc.isMethod() || memberDoc.isConstructor())
			return getMethodName((ExecutableMemberDoc)memberDoc);
		
		String qn = memberDoc.qualifiedName();
		qn = "M:" + qn;		
		return qn;
	}

	public String getMethodName(ExecutableMemberDoc methodDoc) {
		StringBuilder sb = new StringBuilder(256);
		String qn = methodDoc.qualifiedName();
		
		sb.append("M:");
		sb.append(qn);
		
		if (methodDoc.isConstructor())
			sb.append(".#ctor");
		
		this.addGenericTypes(methodDoc.typeParameters(), sb);
		
		if (methodDoc.parameters().length > 0) {
			sb.append("(");
			
			for (Parameter p : methodDoc.parameters()) {
				String pname = p.type().toString();
				pname = pname.replace('<', '{');
				pname = pname.replace('>', '}');
				sb.append(pname);
				/*
				if (p.type().isPrimitive())
					sb.append(p.typeName());
				else {
					TypeVariable tvar = p.type().asTypeVariable();
					if (tvar != null) {
						sb.append("{");
						sb.append(tvar.typeName());
						sb.append("}");
					}
					else {
						sb.append(p.type().asClassDoc().qualifiedName());
					}
				}
				*/
				
				/*if (p.type().dimension().length() > 0)
					sb.append(p.type().dimension());*/
				
				sb.append(",");
			}
			sb.setCharAt(sb.length()-1, ')');
		}
		
		return sb.toString();
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
	
	public void addGenericTypes(TypeVariable[] params, StringBuilder sb) {
		if (params != null && params.length > 0) {
			sb.append("{");
			
			for(TypeVariable var : params) {
				String name = var.typeName();
				sb.append(name);
				sb.append(",");
			}
			sb.setCharAt(sb.length()-1, '}');
		}
	}

}
