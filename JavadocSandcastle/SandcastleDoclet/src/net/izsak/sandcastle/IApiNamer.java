/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.Doc;
import com.sun.javadoc.FieldDoc;
import com.sun.javadoc.MemberDoc;
import com.sun.javadoc.PackageDoc;
import com.sun.javadoc.Type;

/**
 * IApiNamer interface defines methods that can be used
 * to generate fully qualified names of source code elements.
 * 
 * @author Jozef Izso
 */
public interface IApiNamer {
	
	public String getApiName(Type type);
	
	public String getApiName(Doc member);
	
	public String getPackageName(PackageDoc packageDoc);
	
	public String getClassName(ClassDoc classDoc);
	
	public String getMemberName(MemberDoc memberDoc);
	
	public String getFieldName(FieldDoc fieldDoc);
}
