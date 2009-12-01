/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.api;

import java.lang.reflect.Modifier;

import net.izsak.sandcastle.ApiWriterContext;
import nu.xom.Attribute;
import nu.xom.Element;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.ExecutableMemberDoc;
import com.sun.javadoc.MemberDoc;
import com.sun.javadoc.MethodDoc;
import com.sun.javadoc.Parameter;
import com.sun.javadoc.Type;

/**
 * @author Jozef Izso
 *
 */
public class MemberApiWriter extends ProgramElementApiWriter {

	MemberDoc memberDoc;
	
	/**
	 * @param context
	 * @param doc
	 */
	public MemberApiWriter(ApiWriterContext context, MemberDoc memberDoc) {
		super(context, memberDoc);
		this.memberDoc = memberDoc;
	}
	
	@Override
	protected String getSimpleName() {
		String name = this.memberDoc.isConstructor() ? ".ctor" : this.memberDoc.name();
		return name;
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.api.CodeApiWriterBase#writeContainersCore(nu.xom.Element)
	 */
	@Override
	protected void writeContainersCore(Element elmContainers) {
		super.writeNamespaceToContainers(elmContainers, this.memberDoc.containingPackage());
		// write class info
		super.writeTypeInfo(this.memberDoc.containingClass(), elmContainers);
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.api.CodeApiWriterBase#writeOtherData()
	 */
	@Override
	protected void writeOtherData() {
		super.writeOtherData();
		
		this.writeMemberData();
		if (this.memberDoc.isMethod())
			this.writeMethodData();
		if (this.memberDoc.isMethod() || this.memberDoc.isConstructor()) {
			this.writeMethodParameters();
			this.writeMethodThrows();
		}
		if (this.memberDoc.isMethod())
			this.writeReturnType();
	}
	
	private void writeMemberData() {
		String visibility = getVisibility();
		boolean isStatic = isStatic();
		boolean isSpecial = this.getDoc().isConstructor();
		// TODO: add support for overload="" attribute
		
		Element elmTypeData = new Element("memberdata");
		elmTypeData.addAttribute(new Attribute("visibility", visibility));
		elmTypeData.addAttribute(new Attribute("static", Boolean.toString(isStatic)));
		elmTypeData.addAttribute(new Attribute("special", Boolean.toString(isSpecial)));
		
		this.addElement(elmTypeData);
	}
	
	private void writeMethodData() {
		boolean isFinal = isFinal();
		boolean abstr = isAbstract();
		
		Element elmProcedure = new Element("proceduredata");
		
		elmProcedure.addAttribute(new Attribute("abstract", Boolean.toString(abstr)));
		elmProcedure.addAttribute(new Attribute("virtual", Boolean.toString(!isFinal)));
		elmProcedure.addAttribute(new Attribute("final", Boolean.toString(isFinal)));
		
		this.addElement(elmProcedure);
	}
	
	private void writeMethodParameters() {
		ExecutableMemberDoc exe = (ExecutableMemberDoc)this.memberDoc;
		Parameter[] params = exe.parameters();
		if (params.length == 0)
			return;
		
		Element elmAllParams = new Element("parameters");
		
		for (int i = 0; i < params.length; i++) {
			Parameter param = params[i];
			String name = param.name();
			boolean isVarargs = exe.isVarArgs() && (i+1 == params.length);
			
			Element elmParam = new Element("parameter");
			elmParam.addAttribute(new Attribute("name", name));
			if (isVarargs)
				elmParam.addAttribute(new Attribute("params", Boolean.toString(true)));
			
			this.writeTypeInfo(param.type(), elmParam);
			elmAllParams.appendChild(elmParam);
		}
		
		this.addElement(elmAllParams);
	}
	
	private void writeMethodThrows() {
		ExecutableMemberDoc exe = (ExecutableMemberDoc)this.memberDoc;
		ClassDoc[] exceptions = exe.thrownExceptions();
		
		if (exceptions.length == 0)
			return;
		
		Element elmThrows = new Element("throws");
		
		for (ClassDoc ex : exceptions) {
			this.writeTypeInfo(ex, elmThrows);
		}
		
		this.addElement(elmThrows);
	}

	protected void writeReturnType() {
		MethodDoc method = (MethodDoc)this.memberDoc;
		
		Type returnType = method.returnType();
		if (returnType.isPrimitive() && returnType.simpleTypeName().equals("void"))
			return;
			
		Element elmReturn = new Element("returns");
		this.writeTypeInfo(returnType, elmReturn);
		this.addElement(elmReturn);
	}
	
	private boolean isStatic() {
		int mod = this.memberDoc.modifierSpecifier();
		return Modifier.isStatic(mod);
	}
}
