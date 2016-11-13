namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq;
	using System.Collections.Generic;

	/// <summary>
	/// DJ Cluster Algorithm implementation
	/// </summary>
	public class DJClusterAlgorithm
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
		public List<HashSet<double[]>> Learn(double[][] dataSet)
		{
			if (dataSet == null)
			{
				throw new ArgumentNullException(nameof(dataSet)); 
			}

			var clusters = new List<HashSet<double[]>>();

			this.logger.debug("Generating Clusters"); 
			for (int i = 0; i < dataSet.Length; i++)
			{
				bool mergedCluster = false;
				var cluster = this.calcNeighbourhood(dataSet[i], dataSet);

				if (cluster == null) continue;

				for (int currentCluster = 0; currentCluster < clusters.Count; currentCluster++)
				{
					if (clusters.ElementAt(currentCluster).Intersect(cluster).Count() != 0)
					{
						clusters[currentCluster] = new HashSet<double[]>(clusters.ElementAt(currentCluster).Union(cluster));
						mergedCluster = true;
						break;
					}
				}

				if (!mergedCluster)
				{
					clusters.Add(cluster);
				}
			}

			this.logger.debug(String.Format("Generated {0} clusters", clusters.Count)); 
			return clusters; 
		}

		/// <summary>
		/// Calculates the neighbourhood of a given point
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

			return (cluster.Count <= this.minPoints) ? cluster : null;
		}
	}
}