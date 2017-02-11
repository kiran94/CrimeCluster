namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq;
	using System.Collections.Generic;

	/// <summary>
	/// Represents a Cluster
	/// </summary>
	public class Cluster
	{
		/// <summary>
		/// Gets the label.
		/// </summary>
		/// <value>The label.</value>
		public string Label { get; private set; }

		/// <summary>
		/// Gets the points.
		/// </summary>
		/// <value>The points.</value>
		public LocationBinaryTree Points { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.Cluster"/> class.
		/// </summary>
		/// <param name="label">Label.</param>
		/// <param name="cluster">Cluster.</param>
		public Cluster(String label, HashSet<double[]> cluster)
		{
			this.Label = label;

			foreach (var currentPoint in cluster)
			{
				this.Points.Add(currentPoint[0], currentPoint[1]); 	
			}
		}

		/// <summary>
		/// Checks wheather the passed point is contained in the cluster.
		/// </summary>
		/// <param name="toCheck">To check.</param>
		/// <returns>flag indicating if the cluster contains the point.</returns>
		public bool contains(double[] toCheck)
		{
			return this.Points.Search(toCheck[0], toCheck[1]); 
		}
	}
}