//=============================================================================
// System  : Sandcastle Help File Builder Utilities
// File    : SyntaxFilters.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 01/25/2008
// Note    : Copyright 2006-2008, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the enumerated type that defines the languages to include
// in the help topic Syntax section.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://www.CodePlex.com/SHFB.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.3.3.0  11/24/2006  EFW  Created the code
// 1.6.0.4  01/18/2008  EFW  Added support for the JavaScript syntax generator
//=============================================================================

using System;

namespace SandcastleBuilder.Utils
{
    /// <summary>
    /// This public enumerated type defines the languages to include in the
    /// help topic <b>Syntax</b> section.
    /// </summary>
    [Flags, Serializable]
    public enum SyntaxFilters
    {
        /// <summary>Do not include the syntax section.</summary>
        None              = 0x0000,
        /// <summary>Include C# syntax.</summary>
        CSharp            = 0x0001,
        /// <summary>Include VB.NET syntax.</summary>
        VisualBasic       = 0x0002,
        /// <summary>Include C++ syntax.</summary>
        CPlusPlus         = 0x0004,
        /// <summary>Include the J# syntax.</summary>
        JSharp            = 0x0008,
        /// <summary>Include the JScript syntax.</summary>
        JScript           = 0x0010,
        /// <summary>Include the Visual Basic usage syntax.</summary>
        VisualBasicUsage  = 0x0020,
        /// <summary>Include the XAML usage syntax.</summary>
        XamlUsage         = 0x0040,
        /// <summary>Include the JavaScript syntax.</summary>
        JavaScript        = 0x0080,
        /// <summary>The standard set (C#, VB.NET, and C++).</summary>
        Standard          = 0x0007,
        /// <summary>Include all languages except the usage filters</summary>
        AllButUsage       = 0x009F,
        /// <summary>Include all languages and usage.</summary>
        All               = 0x00FF
    }
}
