//=============================================================================
// System  : Sandcastle Help File Builder Utilities
// File    : LinkTypeEnums.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 10/30/2007
// Note    : Copyright 2006, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains the enumerated type that defines the reference link
// types for SDK links.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://SHFB.CodePlex.com.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.1.0.0  08/28/2006  EFW  Created the code
//=============================================================================

using System;

namespace SandcastleBuilder.Utils
{
    /// <summary>
    /// This public enumerated type defines the type of links used to reference
    /// other help topics referring to framework (SDK) help topics.
    /// </summary>
    [Serializable]
    public enum SdkLinkType
    {
        /// <summary>No active links.</summary>
        None,
        /// <summary>MS-Help style links for use within an HTML 2.x help
        /// file.</summary>
        Index,
        /// <summary>Links to framework topics on MSDN.  This is the
        /// default.  This type is compatible with HTML 1.x and HTML
        /// 2.x help files.</summary>
        Msdn
    }
}
