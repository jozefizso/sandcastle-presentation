//=============================================================================
// System  : Sandcastle Help File Builder Utilities
// File    : ExecutionPointCollection.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 11/14/2008
// Note    : Copyright 2007-2008, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a collection class that holds execution point information
// for a plug-in.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy
// of the license should be distributed with the code.  It can also be found
// at the project website: http://www.CodePlex.com/SHFB.   This notice, the
// author's name, and all copyright notices must remain intact in all
// applications, documentation, and source files.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.5.2.0  09/10/2007  EFW  Created the code
// 1.8.0.1  11/14/2008  EFW  Added execution priority support
//=============================================================================

using System;
using System.Collections.ObjectModel;

using SandcastleBuilder.Utils.BuildEngine;

// All classes go in the SandcastleBuilder.Utils.PlugIn namespace
namespace SandcastleBuilder.Utils.PlugIn
{
    /// <summary>
    /// This collection class holds execution point information for a plug-in
    /// process.
    /// </summary>
    public class ExecutionPointCollection : Collection<ExecutionPoint>
    {
        /// <summary>
        /// This is used to determine if the collection contains an entry for
        /// the specified build step and behavior.
        /// </summary>
        /// <param name="step">The build step</param>
        /// <param name="behavior">The behavior</param>
        /// <returns>True if the collection contains an entry for the specified
        /// build step and behavior or false if it does not.</returns>
        public bool RunsAt(BuildStep step, ExecutionBehaviors behavior)
        {
            foreach(ExecutionPoint p in this)
                if(p.BuildStep == step && (p.Behavior & behavior) != 0)
                    return true;

            return false;
        }

        /// <summary>
        /// This is used to obtain the execution priority for a plug-in
        /// in the given build step and behavior.
        /// </summary>
        /// <param name="step">The build step</param>
        /// <param name="behavior">The behavior</param>
        /// <returns>The execution priority is used to determine the order in
        /// which the plug-ins will be executed.  Those with a higher priority
        /// value will be executed before those with a lower value.  Those with
        /// an identical priority may be executed in any order within their
        /// group.</returns>
        public int PriorityFor(BuildStep step, ExecutionBehaviors behavior)
        {
            foreach(ExecutionPoint p in this)
                if(p.BuildStep == step && (p.Behavior & behavior) != 0)
                    return p.Priority;

            return ExecutionPoint.DefaultPriority;
        }
    }
}
