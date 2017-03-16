namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using KdTree;
	using KdTree.Math;

	/// <summary>
	/// Represents a Cluster
	/// </summary>
	public class Cluster
	{
		/// <summary>
		/// Gets the label.
		/// </summary>
		/// <value>The label.</value>
		public int Label { get; private set; }

		/// <summary>
		/// Gets the points.
		/// </summary>
		/// <value>The points.</value>
		public KdTree<double, string> Points { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.Cluster"/> class.
		/// </summary>
		/// <param name="label">Label.</param>
		/// <param name="cluster">Cluster.</param>
		public Cluster(int label, HashSet<double[]> cluster)
		{
			this.Label = label;
			this.Points = new KdTree<double, string>(2, new DoubleMath()); 

			foreach (var currentPoint in cluster)
			{				
				this.Points.Add(currentPoint, label.ToString()); 
			}

			this.Points.Balance();
		}

		/// <summary>
		/// Checks if the passed point is contained in the cluster.
		/// </summary>
		/// <returns>flag indicating if the cluster contains the point.</returns>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		public bool Contains(double latitude, double longitude)
		{
			var nearest = this.Points.GetNearestNeighbours(new double[] { latitude, longitude }, 1)[0];

			return ((Math.Abs(latitude - nearest.Point[0]) < double.Epsilon)
					&& (Math.Abs(longitude - nearest.Point[1]) < double.Epsilon));				
		}

		/// <summary>
		/// Gets the average point in the cluster. 
		/// </summary>
		/// <returns>The average point.</returns>
		public double[] GetAveragePoint()
		{
			return this.Points.Average(); 
		}
	}
}