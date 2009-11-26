using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Ddue.Tools;
using Microsoft.Practices.Unity;
using System.Net;

namespace Izsaknet.Sandcastle.Tools
{
	public class JavadocResolver : IExternalReferenceResolver
	{
		public JavadocResolver()
		{
			this.Locale = "en-us";
			this.packagesCache = new HashSet<string>();
		}

		[Dependency]
		public string JavadocUrl
		{
			get;
			set;
		}

		private ICollection<string> packagesCache;

		#region IExternalReferenceResolver Members

		public string ResolverName
		{
			get { return "Javadoc Resolver"; }
		}

		public bool IsDisabled
		{
			get { return false; }
		}

		public string Locale
		{
			get;
			set;
		}

		public string GetExternalUrl(string targetId)
		{
			string typeName = ParseTargetId(targetId);
			string packageName = typeName.Substring(0, typeName.LastIndexOf('.'));

			this.EnsureCache();
			
			if (!this.packagesCache.Contains(packageName))
				return null;

			string slashedTypeName = typeName.Replace('.', '/');
			if (!this.JavadocUrl.EndsWith("/"))
				this.JavadocUrl += "/";

			return this.JavadocUrl + slashedTypeName + ".html";
		}

		#endregion

		private string ParseTargetId(string targetId)
		{
			if (!targetId.StartsWith("T:"))
				throw new ArgumentException("Argument targetId has unknown value.", "targetId");

			string objName = targetId.Substring(2);

			if (String.IsNullOrEmpty(objName))
				throw new ArgumentException("No type name was specified in argument targetId.", "targetId");

			return objName;
		}

		private void EnsureCache()
		{
			if (this.packagesCache.Count == 0)
			{
				if (String.IsNullOrEmpty(this.JavadocUrl))
					throw new InvalidOperationException("Property JavadocUrl must be set to a URL pointing to the root of a Javadoc documentation.");

				WebClient client = new WebClient();
				string file = client.DownloadString(this.JavadocUrl + "/package-list");
				
				LineParser parser = new LineParser(file);
				foreach (string package in parser.ReadLines())
				{
					this.packagesCache.Add(package);
				}
			}
		}
	}
}
