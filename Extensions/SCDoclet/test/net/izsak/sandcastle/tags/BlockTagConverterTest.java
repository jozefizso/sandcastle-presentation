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

import com.sun.javadoc.Tag;

/**
 * @author Jozef Izso
 *
 */
@RunWith(JMock.class)
public class BlockTagConverterTest {
	Mockery context = new JUnit4Mockery();
	
	@Test
	public void deprecated_Test() {
		final Tag block = context.mock(Tag.class, "BlockTag");
		final Tag inline = context.mock(Tag.class, "InlineTag");
		
		context.checking(new Expectations() {{
			allowing(inline).name(); will(returnValue("Text"));
			allowing(inline).text(); will(returnValue("Deprecated method."));
			
			allowing(block).name(); will(returnValue("@deprecated"));
			allowing(block).text(); will(returnValue("..."));
			allowing(block).inlineTags(); will(returnValue(Arrays.asList(inline).toArray()));
		}});
		
		BlockTagConverter converter = new BlockTagConverter(block);
		
		Element elm = converter.toXml();
		
		Assert.assertEquals("deprecated", elm.getLocalName());
		Assert.assertEquals(0, elm.getAttributeCount());
		Assert.assertEquals(1, elm.getChildCount());
		Assert.assertEquals(Text.class, elm.getChild(0).getClass());
		Assert.assertEquals("Deprecated method.", ((Text)elm.getChild(0)).getValue());
	}

}
