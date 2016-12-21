using System;
using System.Collections.Generic;

namespace com.kiranpatel.crimecluster.framework
{
	/// <summary>
	/// Contract for clustering
	/// </summary>
	public interface IClusteringService
	{
		/// <summary>
		/// Computes clusters for a given data set
		/// </summary>
		/// <returns>List of Collections (and the points within those clusters)</returns>
		/// <param name="dataSet">dataset to compute clusters on</param>
		List<HashSet<double[]>> Learn(double[][] dataSet);

		/// <summary>
		/// Calcualtes the centroids of the passed clusters.
		/// </summary>
		/// <returns>The centroids.</returns>
		/// <param name="clusters">Clusters.</param>
		List<double[]> CalculateCentroids(List<HashSet<double[]>> clusters); 
	}
}