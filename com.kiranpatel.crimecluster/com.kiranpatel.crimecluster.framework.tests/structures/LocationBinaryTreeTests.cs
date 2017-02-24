namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using NUnit.Framework; 

	/// <summary>
	/// Tests the Location Binary Tree 
	/// </summary>
	[TestFixture]
	public class LocationBinaryTreeTests
	{
		/// <summary>
		/// Ensures when a single location is added to the tree, search will return true
		/// </summary>
		[Test]
		public void Add_Search_SingleLocationAddedToBinaryTree_True()
		{
			var tree = this.GetInstance();
			double[] location = { 1, 2 };
			tree.Add(location[0], location[1]);

			Assert.True(tree.Search(location[0], location[1])); 
		}

		/// <summary>
		/// Ensures when multiple locations are added with the same latitude to the tree, search will return true
		/// </summary>
		[Test]
		public void Add_Search_MultipleSameLatitudeAddedToBinaryTree_True()
		{
			var tree = this.GetInstance();
			double[][] locations =
			{
				new double[] { 1, 2 },
				new double[] { 1, 3 },
				new double[] { 1, 4 },
			};

			foreach (var x in locations)
			{
				tree.Add(x[0], x[1]); 
			}

			Assert.True(tree.Search(1, 4));
		}

		/// <summary>
		/// Ensures when there are multiple locations with different latitudes, true is returned
		/// </summary>
		[Test]
		public void Add_Search_MultipleDifferentLatitudeAddedToBinaryTree_True()
		{
			var tree = this.GetInstance();
			double[][] locations =
			{
				new double[] { 2, 2 },
				new double[] { 1, 3 },
				new double[] { 3, 4 },
			};

			foreach (var x in locations)
			{
				tree.Add(x[0], x[1]);
			}

			Assert.True(tree.Search(3, 4));
		}

		/// <summary>
		/// Ensures when the latitude exists and the longitude does not, false is returned
		/// </summary>
		[Test]
		public void Add_Search_LatitudeExistsAndLongitudeDoesNot_False()
		{
			var tree = this.GetInstance();
			double[][] locations =
			{
				new double[] { 2, 2 },
				new double[] { 1, 3 },
				new double[] { 3, 4 },
			};

			foreach (var x in locations)
			{
				tree.Add(x[0], x[1]);
			}

			Assert.False(tree.Search(3, 5));
		}

		/// <summary>
		/// Ensures when the location has not been added, false is returned
		/// </summary>
		[Test]
		public void Add_Search_LatitudeAndLongitudeNotFound_False()
		{
			var tree = this.GetInstance();
			double[][] locations =
			{
				new double[] { 2, 2 },
				new double[] { 1, 3 },
				new double[] { 3, 4 },
			};

			foreach (var x in locations)
			{
				tree.Add(x[0], x[1]);
			}

			Assert.False(tree.Search(5, 5));
		}

		/// <summary>
		/// Ensures when the tree is empty, false is returned
		/// </summary>
		[Test]
		public void Add_Search_EmptyTree_False()
		{
			var tree = this.GetInstance();
			Assert.False(tree.Search(5, 5));
		}

		/// <summary>
		/// Ensures when the tree is empty, the averages are zero. 
		/// </summary>
		[Test]
		public void averagePoint_EmptyTree_Zero()
		{
			var tree = this.GetInstance();
			var result = tree.averagePoint();

			Assert.AreEqual(0, result[0]); 
			Assert.AreEqual(0, result[1]);
		}

		/// <summary>
		/// Ensures when the tree is populated, the averages are returned. 
		/// </summary>
		[Test]
		public void averagePoint_PopulatedTree_AveragesReturned()
		{
			var tree = this.GetInstance();
			tree.Add(2, 4); 
			tree.Add(2, 3);
			tree.Add(3, 3);
			tree.Add(4, 7);
			tree.Add(3, 2);

			var result = tree.averagePoint();

			Assert.AreEqual(2.8, result[0]);
			Assert.AreEqual(3.8, result[1]);
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private LocationBinaryTree GetInstance()
		{
			return new LocationBinaryTree(); 
		}
	}
}