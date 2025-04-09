using System;
using System.Collections.Generic;

namespace MetroidMod
{
	internal static class ListExtensions
	{
		/// <summary>
		/// Remove all the elements of the list that match the predicate,
		/// and return them in a separate list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static List<T> RemoveWhere<T>(this List<T> list, Func<T, bool> predicate)
		{
			List<T> removed = new();
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (predicate(list[i]))
				{
					removed.Add(list[i]);
					list.RemoveAt(i);
				}
			}
			return removed;
		}
	}
}
