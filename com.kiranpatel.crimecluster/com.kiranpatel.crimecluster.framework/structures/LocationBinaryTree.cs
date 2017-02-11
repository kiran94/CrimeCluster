namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Binary Tree for Location Data
	/// </summary>
	public class LocationBinaryTree
	{
		/// <summary>
		/// Root of the Location Binary Tree
		/// </summary>
		private LocationBinaryNode root;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.LocationBinaryTree"/> class.
		/// </summary>
		public LocationBinaryTree()
		{
			//NOP
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.LocationBinaryTree"/> class.
		/// </summary>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		public LocationBinaryTree(double latitude, double longitude)
		{
			this.root = new LocationBinaryNode(latitude, longitude); 
		}

		/// <summary>
		/// Adds a LocationBinaryNode with the latitude and longitude
		/// </summary>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		public void Add(double latitude, double longitude)
		{
			var node = new LocationBinaryNode(latitude, longitude); 
			if (this.root == null)
			{
				this.root = node;
				return; 
			}

			this.add_helper(this.root, latitude, longitude); 
		}

		/// <summary>
		/// Recursively finds the position to add the latitude and longitude.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		private void add_helper(LocationBinaryNode node, double latitude, double longitude)
		{			
			if (Math.Abs(node.Latitude - latitude) < Double.Epsilon)
			{
				node.addLongitude(longitude);
				return; 
			}

			if (latitude < node.Latitude)
			{
				if (node.left == null)
				{
					node.left = new LocationBinaryNode(latitude, longitude);
					return; 
				}

				add_helper(node.left, latitude, longitude); 
			}
			else
			{
				if (node.right == null)
				{
					node.right = new LocationBinaryNode(latitude, longitude);
					return;
				}

				add_helper(node.right, latitude, longitude); 
			}
		}

		/// <summary>
		/// Searches to determine if there is a node with the passed latitude and longitude. 
		/// </summary>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		public bool Search(double latitude, double longitude)
		{
			return this.search(this.root, latitude, longitude);  
		}

		/// <summary>
		/// Recursively searches through the binary tree for the passed latitude and longitude. 
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		public bool search(LocationBinaryNode node, double latitude, double longitude)
		{
			if (node == null)
			{
				return false; 
			}

			if (Math.Abs(node.Latitude - latitude) < Double.Epsilon)
			{
				return node.hasLongitude(longitude);
			}

			if (latitude < node.Latitude)
			{
				return search(node.left, latitude, longitude);
			}
			else
			{
				return search(node.right, latitude, longitude); 
			}
		}
	}
}