using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.Ddue.Tools;

namespace Izsaknet.Sandcastle.Tools
{
	public class TargetIdConverter
	{
		private static Regex MethodPattern = new Regex(@"^(\w):((?:\w+\.)*)(\w+)\.(.+)$", RegexOptions.Compiled);

		/// <summary>
		/// Matches prefix, namespace and class name in TargetId.
		/// </summary>
		private static Regex TypePattern = new Regex(@"^(\w):((?:\w+\.)*)(\w+)", RegexOptions.Compiled);

		private string targetId;

		public TargetIdConverter(string targetId)
		{
			this.targetId = targetId;
		}

		/*
		public static TargetIdConverter Parse(string targetId)
		{
			if (targetId[1] != ':')
				throw new ArgumentException("TargetId has invalid format. Missing : in id.", "targetId");

			TargetIdConverter parser = new TargetIdConverter(targetId);

			string targetType = targetId[0].ToString();
			parser.TargetType = targetType;

			int lastDot = targetId.LastIndexOf('.');
			if (lastDot == -1)
			{
				// no dot in name, it can be root package name or class in (default) namespace.
				switch(targetType)
				{
					case "N":
						parser.Namespace = targetId.Substring(2);
						break;
					default:
						parser.TypeName = targetId.Substring(2);
						break;
				}
			}
			else
			{
				switch(targetType)
				{
					case "M":
						parser.ParseMethod(targetId);
						break;
					case "T":
						parser.ParseType(targetId);
						break;
					case "N":
						parser.ParsePackage(targetId);
						break;
					default:
						throw new InvalidOperationException("Unsupported target type '"+ targetType +"'.");
				}
			}

			return parser;
		}
		
		private void ParseMethod(string targetId)
		{
			Match match = MethodPattern.Match(targetId);
			if (!match.Success)
				throw new ArgumentException(
					String.Format("Argument targetId with value '{0}' cannot be parsed as method name.", targetId),
					"targetId");

			// already set in the Parse method
			////this.TargetType = match.Groups[1].Value;
			this.Namespace = match.Groups[2].Value;
			this.TypeName = match.Groups[3].Value;
			this.MethodName = match.Groups[4].Value;
		}

		private void ParseType(string targetId)
		{
			int lastDot = targetId.LastIndexOf('.');

			this.TypeName = targetId.Substring(lastDot + 1);
			this.Namespace = targetId.Substring(2, lastDot - 3);
		}

		private void ParsePackage(string targetId)
		{
			this.Namespace = targetId.Substring(2);
		}

		public string TargetType { get; set; }

		public string Namespace { get; set; }

		public string TypeName { get; set; }
		
		public string MethodName { get; set; }

		*/

		public Target ToTarget()
		{
			char type = this.targetId[0];

			Target t = null;

			switch (type)
			{
				case 'N':
					t = ToNamespace();
					break;
				case 'T':
					t = ToType();
					break;
				case 'M':
					t = ToMethod();
					break;
				default:
					throw new InvalidOperationException("Target ID was not recognized.");
			}

			t.Id = this.targetId;
			t.DefaultLinkType = LinkType2.External;
			return t;
		}

		private NamespaceTarget ToNamespace()
		{
			NamespaceTarget nt = new NamespaceTarget();
			nt.Name = this.targetId.Substring(2);
			nt.Container = nt.Name;
			return nt;
		}

		private TypeTarget ToType()
		{
			TypeTarget tt = new TypeTarget();
			tt.Templates = new string[0];

			Match match = TypePattern.Match(this.targetId);
			if (!match.Success)
				throw new InvalidOperationException(
					String.Format("TargetId with value '{0}' cannot be parsed as class name.", targetId));

			tt.Name = match.Groups[3].Value;

			tt.Namespace = CreateNamespaceRef(match.Groups[2]);
			tt.Container = tt.Namespace.Id.Substring(2);
			return tt;
		}

		private MethodTarget ToMethod()
		{
			MethodTarget mt = new MethodTarget();
			mt.Templates = new string[0];
			mt.Parameters = new Parameter[0];
			mt.TemplateArgs = new TypeReference[0];

			Match match = MethodPattern.Match(this.targetId);
			if (!match.Success)
				throw new InvalidOperationException(
					String.Format("TargetId with value '{0}' cannot be parsed as method name.", targetId));

			mt.Name = match.Groups[4].Value;

			// optional namespace
			mt.SetTypeReference(null);// match.Groups[2];
			return mt;
		}

		private static NamespaceReference CreateNamespaceRef(Group nsGroup)
		{
			string ns = "";

			// optional namespace
			if (nsGroup.Success && nsGroup.Length > 0)
			{
				ns = nsGroup.Value;
				ns = ns.Remove(ns.Length - 1);
			}

			return new NamespaceReference("N:" + ns);
		}

		public override string ToString()
		{
			return this.targetId;
		}
	}
}
