using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace TestNamespace.Generics
{
	/// <summary>
	/// Same test interface.
	/// </summary>
	public interface IWriter<T>
	{
		/// <summary>
		/// Writes text to output and returns object of type <typeparamref name="T"/>
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		T Write(string text);
	}

	/// <summary>
	/// Holds x and y axis values.
	/// </summary>
	public class Class1<T, E> where T : List<E>, IList<E>
	{
		/// <summary>
		/// Will do comparison....
		/// </summary>
		/// <param name="comparator"></param>
		/// <returns></returns>
		public bool Compare(T comparator)
		{
			return false;
		}

		/// <summary>
		/// Test for generic constraints.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="someType"></param>
		/// <param name="comparator"></param>
		/// <returns></returns>
		public U Test<U>(U someType, T comparator) where U : Dictionary<String, List<int>>
		{
			return default(U);
		}

		public Dictionary<String, V> Dictionary<V>()
		{
			return null;
		}
	}
}
