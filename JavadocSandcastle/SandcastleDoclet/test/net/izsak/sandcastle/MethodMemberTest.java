/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;

import org.junit.Test;

/**
 * @author Jozef Izso
 *
 */
public class MethodMemberTest {
	@Test
	public void isBeanProperty_Test() {
		assertTrue(MemberUtil.isBeanProperty("setName"));
		assertTrue(MemberUtil.isBeanProperty("getName"));
		assertTrue(MemberUtil.isBeanProperty("isEmpty"));
		assertTrue(MemberUtil.isBeanProperty("hasValue"));
		
		assertFalse(MemberUtil.isBeanProperty("issues"));
		assertFalse(MemberUtil.isBeanProperty("addNumber"));
		assertFalse(MemberUtil.isBeanProperty("increment"));
	}
}
