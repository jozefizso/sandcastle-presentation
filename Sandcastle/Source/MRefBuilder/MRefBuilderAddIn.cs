// Copyright © Microsoft Corporation.
// This source file is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

using System.Compiler;

using Microsoft.Ddue.Tools.Reflection;

namespace Microsoft.Ddue.Tools {

    public abstract class MRefBuilderAddIn {

        protected MRefBuilderAddIn(XPathNavigator configuration) { }

        [Obsolete]
        protected MRefBuilderAddIn(ManagedReflectionWriter writer, XPathNavigator configuration) { }

        public abstract void RegisterReflectionWriter(ManagedReflectionWriter writer);
    }

}
