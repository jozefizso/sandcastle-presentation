/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.javadoc;

import net.izsak.sandcastle.DotNetApiNamer;
import net.izsak.sandcastle.tags.ParamBlockTag;
import nu.xom.Element;

import org.custommonkey.xmlunit.XMLAssert;
import org.junit.Test;

import com.sun.javadoc.ParamTag;

//@RunWith(JMock.class)
public class ParamBlockTagTest {
	//Mockery context = new JUnit4Mockery();
	
	/**
	 * Tests whether "@param obj Sample object parameter." will
	 * be correctly transformed into this xml:
	 * 
	 * <param name="obj">Sample object parameter.</param>
	 */
	@Test
	public void simple_parameter_Test() throws Exception {
		final ParamTag param = new ParamTagMock("obj", "Sample object parameter.");
		
		ParamBlockTag conv = new ParamBlockTag(param, new DotNetApiNamer());
		Element elm = conv.toXml();
		
		XMLAssert.assertXMLEqual("<param name='obj'>Sample object parameter.</param>", elm.toXML());
	}
	
	/**
	 * Tests whether "@param &lt;T&gt; Type parameter." will
	 * be correctly transformed into this xml:
	 * 
	 * <typeparam name="T">Type parameter.</param>
	 */
	@Test
	public void type_parameter_Test() throws Exception {
		final ParamTag param = new ParamTagMock("T", "Type parameter.", true);
		
		ParamBlockTag conv = new ParamBlockTag(param, new DotNetApiNamer());
		Element elm = conv.toXml();
		
		XMLAssert.assertXMLEqual("<typeparam name='T'>Type parameter.</param>", elm.toXML());
	}
}