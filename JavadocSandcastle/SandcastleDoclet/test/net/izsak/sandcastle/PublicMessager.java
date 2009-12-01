/**
 * Copyright (c) 2009 Jozef Izso. All Rights Reserved.
 */
package net.izsak.sandcastle;

import java.io.PrintWriter;

import com.sun.tools.javac.util.Context;
import com.sun.tools.javadoc.Messager;

/**
 * @author Christian Bauer
 *
 */
public class PublicMessager extends Messager {
	 
    public PublicMessager(Context context, String s) {
        super(context, s);
    }
 
    public PublicMessager(Context context, String s, PrintWriter printWriter, PrintWriter printWriter1, PrintWriter printWriter2) {
        super(context, s, printWriter, printWriter1, printWriter2);
    }
}

