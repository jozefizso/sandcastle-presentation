//=============================================================================
// System  : Sandcastle Help File Builder - HTML Extract
// File    : AssemblyInfo.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 05/31/2009
// Note    : Copyright 2008-2009, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// Sandcastle Help File Builder HTML Extract attributes.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://www.CodePlex.com/SHFB.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.6.0.5  02/02/2008  EFW  Created the code
//=============================================================================

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

//
// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyProduct("Sandcastle Help File Builder HTML Extract")]
[assembly: AssemblyTitle("Sandcastle Help File Builder HTML Extract")]
[assembly: AssemblyDescription("A command line tool that parses the HTML " +
    "files to extract table of contents and keyword information for CHM " +
    "help files.  It can also convert encodings for CHM builds.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Eric Woodruff")]
[assembly: AssemblyCopyright("Copyright \xA9 2008-2009, Eric Woodruff, All Rights Reserved")]
[assembly: AssemblyTrademark("Eric Woodruff, All Rights Reserved")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]

// No special permissions are required by this assembly
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers
// by using the '*' as shown below:

[assembly: AssemblyVersion("1.8.0.2")]
