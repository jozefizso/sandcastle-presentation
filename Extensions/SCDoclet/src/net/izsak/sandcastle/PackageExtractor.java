/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashSet;
import java.util.Set;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.PackageDoc;
import com.sun.javadoc.RootDoc;

/**
 * @author Jozef Izso
 *
 */
public class PackageExtractor {
	
	private RootDoc root;
	private Set<String> packages;
	
	public PackageExtractor(RootDoc root) {
		this.root = root;
		this.packages = new HashSet<String>(); 
	}
	
	public void extractPackages(ClassDoc[] classes) {
		for(ClassDoc c : classes) {
			PackageDoc pkg = c.containingPackage();
			
			if (!packages.contains(pkg.name())) {
				packages.add(pkg.name());
			}
		}
	}
	
	public Collection<PackageDoc> getAllPackages() {
		Collection<PackageDoc> set = new ArrayList<PackageDoc>(this.packages.size());
		for(String name : this.packages) {
			PackageDoc pd = this.root.packageNamed(name);
			set.add(pd);
		}
		return set;
	}
}
