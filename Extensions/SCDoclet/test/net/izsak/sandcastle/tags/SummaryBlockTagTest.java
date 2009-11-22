/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle.tags;

import java.util.Arrays;

import junit.framework.Assert;
import nu.xom.Element;
import nu.xom.Text;

import org.jmock.Expectations;
import org.jmock.Mockery;
import org.jmock.integration.junit4.JMock;
import org.jmock.integration.junit4.JUnit4Mockery;
import org.junit.Test;
import org.junit.runner.RunWith;

import com.sun.javadoc.ClassDoc;
import com.sun.javadoc.Tag;

/**
 * @author Jozef Izso
 *
 */
@RunWith(JMock.class)
public class SummaryBlockTagTest {
	Mockery context = new JUnit4Mockery();
	
	@Test
	public void simple_summary_Test() {
		final ClassDoc doc = context.mock(ClassDoc.class);
		final Tag tag = context.mock(Tag.class);
		
		context.checking(new Expectations() {{
			allowing(tag).name(); will(returnValue("Text"));
			allowing(tag).text(); will(returnValue("Sample summary."));
			
			allowing(doc).inlineTags(); will(returnValue(Arrays.asList(tag).toArray()));
		}});
		
		SummaryBlockTag summary = new SummaryBlockTag(doc);
		
		Element elm = summary.toXml();
		
		Assert.assertEquals("summary", elm.getLocalName());
		Assert.assertEquals(0, elm.getAttributeCount());
		Assert.assertEquals(1, elm.getChildCount());
		Assert.assertEquals(Text.class, elm.getChild(0).getClass());
		Assert.assertEquals("Sample summary.", ((Text)elm.getChild(0)).getValue());
	}
}
