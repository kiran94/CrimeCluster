﻿namespace com.kiranpatel.crimecluster.framework
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
		private readonly double raduisEps;

		/// <summary>
		/// The minimum points for the cluster. 
		/// </summary>
		private readonly int minPoints; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.DJClusterAlgorithm`1"/> class.
		/// </summary>
		/// <param name="configService">configuration service.</param>
		/// <param name="logger">logger service.</param>
		/// <param name="distanceMeasure">distance measure.</param>
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

			this.logger.debug($"Generating clusters with parameters Eps: {this.raduisEps} MinPts: {this.minPoints} Distance Measure: {this.measure.GetType().ToString()}");

			var clusters = new List<HashSet<double[]>>();
			for (int i = 0; i < dataSet.Length; i++)
			{
				var cluster = this.calcNeighbourhood(dataSet[i], dataSet);

				if (cluster == null)
				{
					continue;	
				}

				if (!this.mergeClusters(clusters, cluster))
				{
					clusters.Add(cluster);
				}
			}

			this.logger.debug(String.Format("Generated {0} Clusters", clusters.Count)); 
			return clusters; 
		}

		/// <summary>
		/// Calculates the neighbourhood of a given point using the minimum radius and minpoints
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
		/// <returns><c>true</c>, if clusters were merged, <c>false</c> otherwise.</returns>
		/// <param name="clusters">Existing Clusters.</param>
		/// <param name="cluster">Newly Discovered Cluster.</param>
		private bool mergeClusters(List<HashSet<double[]>> clusters, HashSet<double[]> cluster)
		{
			for (int currentCluster = 0; currentCluster < clusters.Count; currentCluster++)
			{
				if (clusters[currentCluster].Intersect(cluster).Count() != 0)
				{
					clusters[currentCluster] = new HashSet<double[]>(clusters[currentCluster]
					                                                 .Union(cluster));
					return true; 
				}
			}

			return false; 
		}
	}
}