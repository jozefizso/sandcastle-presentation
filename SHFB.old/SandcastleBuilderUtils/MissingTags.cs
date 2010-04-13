//=============================================================================
// System  : Sandcastle Help File Builder Utilities
// File    : MissingTags.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 01/16/2009
// Note    : Copyright 2006-2009, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the enumerated type that defines the missing
// documentation tags for which to search.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://www.CodePlex.com/SHFB.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.3.1.0  09/26/2006  EFW  Created the code
// 1.4.0.2  05/11/2007  EFW  Missing namespace messages are now optional
// 1.6.0.7  03/23/2008  EFW  Added TypeParameter option
// 1.8.0.1  01/16/2009  EFW  Added IncludeTargets option
//=============================================================================

using System;

namespace SandcastleBuilder.Utils
{
    /// <summary>
    /// This public enumerated type defines the missing documentation tags for
    /// which to search.
    /// </summary>
    [Flags, Serializable]
    public enum MissingTags
    {
        /// <summary>Do not search for any missing tags.</summary>
        None              = 0x0000,
        /// <summary>Search for missing &lt;summary&gt; tags.</summary>
        Summary           = 0x0001,
        /// <summary>Search for missing &lt;param&gt; tags.</summary>
        Parameter         = 0x0002,
        /// <summary>Search for missing &lt;returns&gt; tags.</summary>
        Returns           = 0x0004,
        /// <summary>Search for missing &lt;value&gt; tags.</summary>
        Value             = 0x0008,
        /// <summary>Search for missing &lt;remarks&gt; tags.</summary>
        Remarks           = 0x0010,
        /// <summary>Automatically document constructors if they are missing a
        /// &lt;summary&gt; tag.</summary>
        AutoDocumentCtors = 0x0020,
        /// <summary>Search for missing namespace comments.</summary>
        Namespace         = 0x0040,
        /// <summary>Search for missing &lt;typeparam&gt; tags.</summary>
        TypeParameter     = 0x0080,
        /// <summary>Search for missing &lt;include&gt; target documentation.</summary>
        IncludeTargets    = 0x0100
    }
}
