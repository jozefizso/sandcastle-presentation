using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace TestNamespace.Generics
{
	/// <summary>
	/// Sample class with just one generic method <see cref="Method1"/>.
	/// </summary>
	public class Class1
	{
		/// <summary>
		/// <see cref="Method1"/> takes just one templated parameter <paramref name="obj"/>
		/// of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">Type of the input object.</typeparam>
		/// <param name="obj">Actual object.</param>
		public void Method1<T>(T obj)
		{
		}
	}
}
