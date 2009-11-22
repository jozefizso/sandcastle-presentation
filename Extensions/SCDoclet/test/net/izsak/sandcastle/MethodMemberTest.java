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
		assertTrue(MethodMember.isBeanProperty("setName"));
		assertTrue(MethodMember.isBeanProperty("getName"));
		assertTrue(MethodMember.isBeanProperty("isEmpty"));
		assertTrue(MethodMember.isBeanProperty("hasValue"));
		
		assertFalse(MethodMember.isBeanProperty("issues"));
		assertFalse(MethodMember.isBeanProperty("addNumber"));
		assertFalse(MethodMember.isBeanProperty("increment"));
	}
}
