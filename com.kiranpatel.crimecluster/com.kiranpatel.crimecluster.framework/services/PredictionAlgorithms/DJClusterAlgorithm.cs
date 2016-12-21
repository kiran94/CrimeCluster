namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq;
	using System.Collections.Generic;

	/// <summary>
	/// DJ Cluster Algorithm implementation
	/// </summary>
	public class DJClusterAlgorithm : IClusteringService
	{
		/// <summary>
		/// The config service.
		/// </summary>
		private readonly IConfigurationService configService;

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly ILogger logger;

		/// <summary>
		/// The distance measure.
		/// </summary>
		private readonly IDistanceMeasure measure; 

		/// <summary>
		/// The raduis eps for the points. 
		/// </summary>
		private double raduisEps;

		/// <summary>
		/// The minimum points for the cluster. 
		/// </summary>
		private int minPoints; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.DJClusterAlgorithm`1"/> class.
		/// </summary>
		/// <param name="configService">Config service.</param>
		/// <param name="logger">Logger.</param>
		/// <param name="distanceMeasure">Distance Measure.</param>
		public DJClusterAlgorithm(
			IConfigurationService configService, 
			ILogger logger, 
			IDistanceMeasure distanceMeasure)
		{
			this.configService = configService;
			this.logger = logger;
			this.measure = distanceMeasure; 

			this.raduisEps = Convert.ToDouble(this.configService.Get(ConfigurationKey.DJClusterRadiusEps, "0.05"));
			this.minPoints = Convert.ToInt32(this.configService.Get(ConfigurationKey.DJClusterMinPts, "10")); 
		}

		// <inheritdoc>
		public List<double[]> CalculateCentroids(List<HashSet<double[]>> clusters)
		{
			var centroids = new List<double[]>();
			foreach (var currentCluster in clusters)
			{
				var currentCentroid = new double[2]; 
				currentCentroid[0] = currentCluster.Average(x => x[0]); 
				currentCentroid[1] = currentCluster.Average(x => x[1]);

				centroids.Add(currentCentroid); 
			}

			return centroids; 
		}

		// <inheritdoc>
		public List<HashSet<double[]>> Learn(double[][] dataSet)
		{
			if (dataSet == null)
			{
				throw new ArgumentNullException(nameof(dataSet)); 
			}

			this.logger.info(String.Format("Generating clusters with paramters: Eps: {0} MinPts: {1}", this.raduisEps, this.minPoints));

			var clusters = new List<HashSet<double[]>>();
			for (int i = 0; i < dataSet.Length; i++)
			{
				var cluster = this.calcNeighbourhood(dataSet[i], dataSet);

				if (cluster == null)
				{
					continue;	
				}

				if (!mergeClusters(clusters, cluster))
				{
					clusters.Add(cluster);
				}
			}

			this.logger.debug(String.Format("Generated {0} Clusters", clusters.Count)); 
			return clusters; 
		}

		/// <summary>
		/// Calculates the neighbourhood of a given point using the minimum raduis and minpoints
		/// </summary>
		/// <returns>clustering of the point</returns>
		/// <param name="currentPoint">Current point.</param>
		/// <param name="dataSet">Data set.</param>
		private HashSet<double[]> calcNeighbourhood(double[] currentPoint, double[][] dataSet)
		{
			var cluster = new HashSet<double[]>();
			for (int i = 0; i < dataSet.GetLength(0); i++)
			{
				if (this.measure.measure(currentPoint, dataSet[i]) <= this.raduisEps)
				{
					cluster.Add(dataSet[i]);
				}
			}

			return (cluster.Count >= this.minPoints) ? cluster : null;
		}

		/// <summary>
		/// Merges the generated cluster with the list of clusters if there are any intersecting points, 
		/// if there are any intersecting points the clusters are merged.
		/// </summary>
		/// <returns><c>true</c>, if clusters was merged, <c>false</c> otherwise.</returns>
		/// <param name="clusters">Clusters.</param>
		/// <param name="cluster">Cluster.</param>
		private bool mergeClusters(List<HashSet<double[]>> clusters, HashSet<double[]> cluster)
		{
			for (int currentCluster = 0; currentCluster < clusters.Count; currentCluster++)
			{
				if (clusters[currentCluster].Intersect(cluster).Count() != 0)
				{
					clusters[currentCluster] = new HashSet<double[]>(clusters[currentCluster].Union(cluster));
					return true; 
				}
			}

			return false; 
		}
	}
}