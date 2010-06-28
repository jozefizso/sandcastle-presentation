//=============================================================================
// System  : Sandcastle Help File Builder Components
// File    : AssemblyInfo.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 05/31/2009
// Note    : Copyright 2006-2009, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// Sandcastle Help File Builder components assembly attributes.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://www.CodePlex.com/SHFB.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.3.3.0  08/02/2006  EFW  Created the code
// 1.5.0.0  06/19/2007  EFW  Various additions and updates
// 1.6.0.0  06/19/2007  EFW  Various additions and updates
// 1.8.0.0  06/20/2008  EFW  Implemented new MSBuild project format
//=============================================================================

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

// Resources contained within the assembly are English
[assembly: NeutralResourcesLanguageAttribute("en")]

//
// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyProduct("Sandcastle Help File Builder BuildAssembler Components")]
[assembly: AssemblyTitle("Sandcastle Help File Builder BuildAssembler Components")]
[assembly: AssemblyDescription("A set of build components used to " +
    "extend the features of Sandcastle's BuildAssembler executable.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Eric Woodruff")]
[assembly: AssemblyCopyright("Copyright \xA9 2006-2009, Eric Woodruff, All Rights Reserved")]
[assembly: AssemblyTrademark("Eric Woodruff, All Rights Reserved")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(false)]
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
