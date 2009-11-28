using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

using Microsoft.Ddue.Tools;
using Microsoft.Practices.Unity;
using System.Net;

namespace Izsaknet.Sandcastle.Tools
{
	public class JavadocResolver : IExternalReferenceResolver
	{
		private static Dictionary<string, string> PrimitiveTypes = new Dictionary<string,string>()
		{
			{ "byte", "Byte" },
			{ "char", "Character" },
			{ "short", "Short" },
			{ "int", "Integer" },
			{ "long", "Long" },
			{ "float", "Float" },
			{ "double", "Double" },
			{ "boolean", "Boolean" },
			{ "void", "Void" },
		};

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
			int lastDot = targetId.LastIndexOf('.');

			if (lastDot == -1)
			{
				string wrapper;
				if (IsPrimitiveType(targetId, out wrapper))
					targetId = wrapper;
				else
					return null;
			}

			TargetIdConverter conv = new TargetIdConverter(targetId);
			Target t = conv.ToTarget();

			if (t is MethodTarget)
				return null;

			this.EnsureCache();

			string packageName = "";
			NamespaceTarget nt = t as NamespaceTarget;
			if (nt != null)
			{
				packageName = nt.Name;

				string slashedPackage = packageName.Replace('.', '/');
				return this.JavadocUrl + "/" + slashedPackage + "/package-summary.html";
			}

			TypeTarget tt = t as TypeTarget;
			if (tt == null)
				return null;

			packageName = tt.Container;

			if (!this.packagesCache.Contains(packageName))
				return null;

			string slashedTypeName = packageName.Replace('.', '/');
			slashedTypeName += "/" + packageName;
			if (!this.JavadocUrl.EndsWith("/"))
				this.JavadocUrl += "/";

			return this.JavadocUrl + slashedTypeName + ".html";
		}

		#endregion

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

		private bool IsPrimitiveType(string targetId, out string wrapperTypeId)
		{
			string name = targetId.Substring(2);

			string typeName;
			if (PrimitiveTypes.TryGetValue(name, out typeName))
			{
				// return wrapper class reference
				wrapperTypeId = "T:java.lang." + typeName;
				return true;
			}

			wrapperTypeId = null;
			return false;
		}
	}
}
