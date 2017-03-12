namespace com.kiranpatel.crimecluster.framework
{
	using System.Collections.Generic;

	/// <summary>
	/// Contract for clustering services.
	/// </summary>
	public interface IClusteringService
	{
		/// <summary>
		/// Computes Clusters for a passed dataset.
		/// </summary>
		/// <returns>List of Collections (and the points within those clusters)</returns>
		/// <param name="dataSet">dataset to compute clusters on</param>
		List<HashSet<double[]>> Learn(double[][] dataSet);
	}
}