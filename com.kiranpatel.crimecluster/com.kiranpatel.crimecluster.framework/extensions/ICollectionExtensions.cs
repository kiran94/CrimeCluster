namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic; 

	/// <summary>
	/// Extension methods for <see cref="ICollection{T}"/> instances
	/// </summary>
	public static class ICollectionExtensions
	{
		/// <summary>
		/// Determins if a collection is null or empty
		/// </summary>
		/// <returns><c>true</c>, if null or empty, <c>false</c> otherwise.</returns>
		/// <param name="value">collection to check.</param>
		/// <typeparam name="T">Type of the collection</typeparam>
		public static bool IsNullOrEmpty<T>(this ICollection<T> value)
		{
			return value == null || value.Count == 0;
		}
	}
}
