namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq;
	using System.Collections.Generic;

	/// <summary>
	/// Extensions for IQueryable. 
	/// </summary>
	public static class IQueryableExtensions
	{
		/// <summary>
		/// Converts a queryable into a HashSet. 
		/// </summary>
		/// <returns>The hash set.</returns>
		/// <param name="queryable">Queryable.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static HashSet<T> ToHashSet<T>(this IQueryable<T> queryable)
		{
			return new HashSet<T>(queryable); 
		}
	}
}