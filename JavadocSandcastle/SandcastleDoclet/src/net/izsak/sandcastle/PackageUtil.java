/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.PackageDoc;
import com.sun.javadoc.RootDoc;

/**
 * @author Jozef Izso
 *
 */
public class PackageUtil {
	
	private RootDoc root;
	
	public PackageUtil(RootDoc root) {
		this.root = root; 
	}
	
	public List<PackageDoc> getAllPackages() {
		Set<String> packages = extractPackages(this.root.classes());
		
		List<PackageDoc> set = new ArrayList<PackageDoc>(packages.size());
		for(String name : packages) {
			PackageDoc pd = this.root.packageNamed(name);
			set.add(pd);
		}
		return set;
	}
	
	private Set<String> extractPackages(ClassDoc[] classes) {
		Set<String> packages = new HashSet<String>();
		for(ClassDoc c : classes) {
			PackageDoc pkg = c.containingPackage();
			
			if (!packages.contains(pkg.name())) {
				packages.add(pkg.name());
			}
		}
		return packages;
	}
}
