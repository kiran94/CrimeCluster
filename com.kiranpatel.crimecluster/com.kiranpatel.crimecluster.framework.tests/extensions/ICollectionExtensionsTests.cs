namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using System.Collections.Generic;
	using NUnit.Framework;

	/// <summary>
	/// Tests for the <see cref="ICollectionExtensions"/> class
	/// </summary>
	[TestFixture]
	public class ICollectionExtensionsTests
	{
		/// <summary>
		/// Ensures when the collection is null, false is returned
		/// </summary>
		[Test]
		public void IsNullOrEmpty_NullCollection_True()
		{
			ICollection<int> list = null;
			Assert.True(list.IsNullOrEmpty());
		}

		/// <summary>
		/// Ensures when the collection is empty, false is returned
		/// </summary>
		[Test]
		public void IsNullOrEmpty_EmptyCollection_True()
		{
			ICollection<int> list = new List<int>();
			Assert.True(list.IsNullOrEmpty());
		}

		/// <summary>
		/// Ensures when the collection is populated, true is returned
		/// </summary>
		[Test]
		public void IsNullOrEmpty_Populated_False()
		{
			ICollection<int> list = new List<int>();
			list.Add(1);

			Assert.False(list.IsNullOrEmpty());
		}
	}
}