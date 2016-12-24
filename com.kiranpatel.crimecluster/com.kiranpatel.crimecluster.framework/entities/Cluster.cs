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
		public String Label { get; private set; }

		/// <summary>
		/// Gets the points.
		/// </summary>
		/// <value>The points.</value>
		public HashSet<double[]> Points { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.Cluster"/> class.
		/// </summary>
		/// <param name="label">Label.</param>
		/// <param name="cluster">Cluster.</param>
		public Cluster(String label, HashSet<double[]> cluster)
		{
			this.Label = label;
			this.Points = cluster; 
		}

		/// <summary>
		/// Checks wheather the passed point is contained in Points
		/// </summary>
		/// <param name="toCheck">To check.</param>
		public bool contains(double[] toCheck)
		{
			//return this.Points.Contains(toCheck); 
			return this.Points.Any(x => x[0] == toCheck[0] && x[1] == toCheck[1]); 
		}
	}
}