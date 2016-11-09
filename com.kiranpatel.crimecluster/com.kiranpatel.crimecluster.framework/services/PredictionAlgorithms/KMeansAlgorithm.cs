namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq;
	using System.Collections.Generic;

	using Accord.MachineLearning;

	/// <summary>
	/// K-Means Algorithm Implementation
	/// </summary>
	public class KMeansAlgorithm<T> : IClusteringService<T> where T: EntityBase
	{
		/// <summary>
		/// The config service.
		/// </summary>
		private readonly IConfigurationService configService; 

		/// <summary>
		/// The Logger instance. 
		/// </summary>
		private readonly ILogger logger;

		/// <summary>
		/// The k means.
		/// </summary>
		private readonly KMeans kMeans; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.KMeansAlgorithm`1"/> class.
		/// </summary>
		/// <param name="logger">Logger.</param>
		public KMeansAlgorithm(IConfigurationService configService, ILogger logger)
		{
			this.configService = configService; 
			this.logger = logger;

			int noClusters = Int32.Parse(this.configService.Get(ConfigurationKey.KMeansClusterNumber, "3"));
			this.kMeans = new KMeans(noClusters);
		}

		// <inheritdoc>
		public double[][] Learn(ICollection<T> data, Func<T, double[]> attributes)
		{
			if (data == null || attributes == null)
			{
				return null;
			}

			this.logger.info("Running K-Means Algorithm");
			double[][] obs = data.Select(x => attributes.Invoke(x)).ToArray();

			var collection = this.kMeans.Learn(obs);
			return collection.Clusters.Select(x => x.Centroid).ToArray(); 
		}

		// <inheritdoc>
		public int findNearest(double[] point)
		{
			return this.kMeans.Clusters.Decide(point); 
		}
	}
}