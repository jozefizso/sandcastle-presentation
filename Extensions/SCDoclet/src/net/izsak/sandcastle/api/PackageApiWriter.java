/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.api;

import net.izsak.sandcastle.ApiWriterContext;

import com.sun.javadoc.PackageDoc;

/**
 * @author Jozef Izso
 *
 */
public class PackageApiWriter extends CodeApiWriterBase {
	
	private PackageDoc packageDoc;
	
	public PackageApiWriter(ApiWriterContext context, PackageDoc packageDoc) {
		super(context, packageDoc);
		this.packageDoc = packageDoc;
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.api.CodeApiWriterBase#writeApiData()
	 */
	@Override
	protected void writeApiData() {
		super.writeApiData(this.getSimpleName(), this.getGroup());
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.api.CodeApiWriterBase#getSimpleName()
	 */
	@Override
	protected String getSimpleName() {
		String qname = this.packageDoc.name();
		
		int index = qname.lastIndexOf('.');
		if (index >= 0)
			qname = qname.substring(index+1);
		
		return qname;
	}
	
	/* (non-Javadoc)
	 * @see net.izsak.sandcastle.api.MemberApiWriterBase#writeMembers()
	 */
	@Override
	protected void writeMembers() {
		this.writeMembers(packageDoc.allClasses());
	}
	
	/**
	 * Packages have no containers.
	 */
	@Override
	public void writeContainers() {
	}
}
