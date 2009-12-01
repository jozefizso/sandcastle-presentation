/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.doc;

import net.izsak.sandcastle.JavadocTester;
import nu.xom.Document;
import nu.xom.Node;

import org.custommonkey.xmlunit.XMLAssert;
import org.junit.Test;

/**
 * @author Jozef Izso
 *
 */
public class ClassDocTest {
	
	@Test
	public void simpleClass_Test() throws Exception {
		JavadocTester tester = new JavadocTester("Class01.java");
		Document doc = tester.getDocumentationXml();
		
		Node elm = doc.query("//member[@name='T:samples.Class01']").get(0);
		
		XMLAssert.assertXMLEqual("<summary>Just a simple class.</summary>", elm.getChild(0).toXML());
	}
	
	@Test
	public void genericClass01_Test() throws Exception {
		JavadocTester tester = new JavadocTester("GenericClass01.java");
		Document doc = tester.getDocumentationXml();
		
		String expSummary = "<summary>"+
            				    "Generic class with one type parameter <typeparamref name='T' />."+
            			    "</summary>";
		String expTypePar = "<typeparam name='T'>Type parameter.</typeparam>";
		
		Node elm = doc.query("//member[@name='T:samples.GenericClass01{T}']").get(0);
		Node summary = elm.getChild(0);
		Node typePar = elm.getChild(1);
		
		XMLAssert.assertXMLEqual(expSummary, summary.toXML());
		XMLAssert.assertXMLEqual(expTypePar, typePar.toXML());
	}
	
	@Test
	public void genericClass02_Test() throws Exception {
		JavadocTester tester = new JavadocTester("GenericClass02.java");
		Document doc = tester.getDocumentationXml();
		
		String expSummary = "<summary>"+
            				    "Generic class with two type parameters."+
            			    "</summary>";
		String expTypeParT = "<typeparam name='T'>First type parameter.</typeparam>";
		String expTypeParE = "<typeparam name='E'>Second type parameter.</typeparam>";
		
		Node elm = doc.query("//member[@name='T:samples.GenericClass02{T,E}']").get(0);
		Node summary = elm.getChild(0);
		Node typeParT = elm.getChild(1);
		Node typeParE = elm.getChild(2);
		
		XMLAssert.assertXMLEqual(expSummary, summary.toXML());
		XMLAssert.assertXMLEqual(expTypeParT, typeParT.toXML());;
		XMLAssert.assertXMLEqual(expTypeParE, typeParE.toXML());
	}
	
	@Test
	public void genericClass03_Test() throws Exception {
		JavadocTester tester = new JavadocTester("GenericClass03.java");
		Document doc = tester.getDocumentationXml();
		
		String expSummary = "<summary>"+
		                        "<see cref='M:samples.GenericClass03.method1{T}(T)' /> "+
            				    "takes just one templated parameter "+
            				    "<paramref name='obj'/>\n of type <typeparamref name='T'/>."+
            			    "</summary>";
		String expTypeParT = "<typeparam name='T'>Type of the input object.</typeparam>";
		String expParam    = "<param name='obj'>Actual object.</param>";
		String expReturn   = "<returns>An object of type <typeparamref name='T'/>.</returns>";
		
		Node elm = doc.query("//member[@name='M:samples.GenericClass03.method1{T}(T)']").get(0);
		Node summary = elm.getChild(0);
		Node typeParT = elm.getChild(1);
		Node typeParam = elm.getChild(2);
		Node ret = elm.getChild(3);
		
		XMLAssert.assertXMLEqual(expSummary, summary.toXML());
		XMLAssert.assertXMLEqual(expTypeParT, typeParT.toXML());
		XMLAssert.assertXMLEqual(expParam, typeParam.toXML());
		XMLAssert.assertXMLEqual(expReturn, ret.toXML());
	}
}
