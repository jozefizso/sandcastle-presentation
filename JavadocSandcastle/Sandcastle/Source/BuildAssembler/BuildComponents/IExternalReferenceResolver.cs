using System;

namespace Microsoft.Ddue.Tools
{
	public interface IExternalReferenceResolver
	{
		string ResolverName { get; }
		
		bool IsDisabled { get; }
		
		string Locale { get; set; }

		string GetExternalUrl(string targetId);

	}
}
