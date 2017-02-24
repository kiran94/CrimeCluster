﻿namespace com.kiranpatel.crimecluster.framework
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

			this.addHelper(this.root, latitude, longitude); 
		}

		/// <summary>
		/// Recursively finds the position to add the latitude and longitude.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		private void addHelper(LocationBinaryNode node, double latitude, double longitude)
		{			
			if (Math.Abs(node.Latitude - latitude) < Double.Epsilon)
			{
				node.AddLongitude(longitude);
				return; 
			}

			if (latitude < node.Latitude)
			{
				if (node.Left == null)
				{
					node.Left = new LocationBinaryNode(latitude, longitude);
					return; 
				}

				this.addHelper(node.Left, latitude, longitude); 
			}
			else
			{
				if (node.Right == null)
				{
					node.Right = new LocationBinaryNode(latitude, longitude);
					return;
				}

				this.addHelper(node.Right, latitude, longitude); 
			}
		}

		/// <summary>
		/// Searches to determine if there is a node with the passed latitude and longitude. 
		/// </summary>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		public bool Search(double latitude, double longitude)
		{
			return this.searchHelper(this.root, latitude, longitude);  
		}

		/// <summary>
		/// Recursively searches through the binary tree for the passed latitude and longitude. 
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		private bool searchHelper(LocationBinaryNode node, double latitude, double longitude)
		{
			if (node == null)
			{
				return false; 
			}

			if (Math.Abs(node.Latitude - latitude) < Double.Epsilon)
			{
				return node.HasLongitude(longitude);
			}

			if (latitude < node.Latitude)
			{
				return this.searchHelper(node.Left, latitude, longitude);
			}
			else
			{
				return this.searchHelper(node.Right, latitude, longitude); 
			}
		}

		/// <summary>
		/// Calculates the average point for all locations in the location binary search tree. 
		/// </summary>
		/// <returns>The point.</returns>
		public double[] averagePoint()
		{			
			var points = new double[3]; 

			if (this.root == null)
			{
				return points; 
			}

			averagePoint(this.root, points);

			points[0] /= points[2];
			points[1] /= points[2]; 

			return points; 
		}

		/// <summary>
		/// In-Order Traversal of the location binary tree where at each node we are:
		/// For each longitude, add the latitude and longitude pairs to the total sum 
		/// For each longitude (implies a latitude) add one to the total count. 
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="points">Points.</param>
		private void averagePoint(LocationBinaryNode node, double[] points)
		{
			if (node != null)
			{
				averagePoint(node.Left, points);

				foreach (var currentLongitude in node.LongitudeList)
				{
					points[0] += node.Latitude;
					points[1] += currentLongitude;
					points[2]++;
				}

				averagePoint(node.Right, points);
			}
		}
	}
}