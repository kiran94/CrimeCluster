namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Contract for a clustering algorithm 
	/// </summary>
	public interface IClusteringService<T> where T : EntityBase
	{
		/// <summary>
		/// Learns the clusters with the input data and using attributes function return
		/// </summary>
		/// <returns>cluster coordinates</returns>
		/// <param name="data">data set to learn clusters from.</param>
		/// <param name="attributes">experssion to extract attributes from the object.</param>
		double[][] Learn(ICollection<T> data, Func<T, double[]> attributes);

		/// <summary>
		/// Finds the nearest cluster to the given point
		/// </summary>
		/// <returns>The nearest cluster.</returns>
		/// <param name="point">point to find nearet cluster on.</param>
		int findNearest(double[] point); 
	}
}