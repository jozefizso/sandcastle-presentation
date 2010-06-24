// <copyright>
//     Copyright (c) 2009, Jozef Izso. All rights reserved. 
//     
//     Redistribution and use in source and binary forms, with or without 
//     modification, are permitted provided that the following conditions are 
//     met: 
//     
//     1. Redistributions of source code must retain the above copyright notice, 
//        this list of conditions and the following disclaimer. 
//     
//     2. Redistributions in binary form must reproduce the above copyright 
//        notice, this list of conditions and the following disclaimer in the 
//        documentation and/or other materials provided with the distribution. 
//     
//     3. Neither the name of the author nor the names of its contributors 
//        may be used to endorse or promote products derived from this software 
//        without specific prior written permission. 
//     
//     THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS 
//     IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED 
//     TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
//     PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT 
//     HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//     SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
//     TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
//     PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
//     LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
//     NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS 
//     SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
// </copyright>

using System;
using System.Collections.Generic;
using System.Xml;

namespace Izsaknet.Xml
{
	/// <summary>
	/// XmlEnvVarResolver class extends <see cref="XmlUrlResolver"/> class
	/// and expands environment variables in paths in XML documents.
	/// </summary>
	public class XmlEnvVarResolver : XmlUrlResolver
	{
		public XmlEnvVarResolver()
			: base()
		{ }

		public override Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			return base.ResolveUri(baseUri, Environment.ExpandEnvironmentVariables(relativeUri));
		}
	}
}
