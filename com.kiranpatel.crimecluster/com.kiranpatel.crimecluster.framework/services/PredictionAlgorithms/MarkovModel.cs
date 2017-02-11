namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq;
	using System.Collections.Generic;

	/// <summary>
	/// Hidden markov model implementation.
	/// </summary>
	public class MarkovModel : IMarkovModel
	{
		/// <summary>
		/// The transition matrix.
		/// </summary>
		private double[,] _transitionMatrix;

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly ILogger logger; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.MarkovModel"/> class.
		/// </summary>
		/// <param name="logger">Logger.</param>
		public MarkovModel(ILogger logger)
		{
			this.logger = logger; 
		}

		// <inheritdoc>
		public double[,] generateTransitionMatrix(ICollection<Incident> incidents, List<Cluster> clusters)
		{
			var clustersFound = new Queue<int>();

			foreach (var currentIncident in incidents)
			{
				// maybe improve this? O(n^2)
				foreach (var currentCluster in clusters)
				{
					if (currentCluster.Contains(
						currentIncident.Location.Latitude.Value, 
						currentIncident.Location.Longitude.Value))
					{
						clustersFound.Enqueue(currentCluster.Label);
						continue; 
					}
				}
			}

			this._transitionMatrix = this.generateTransitionMatrix(clustersFound, clusters.Count);
			return this._transitionMatrix; 
		}

		/// <summary>
		/// Generates a Transition Matrix from the passed queue of cluster labels
		/// </summary>
		/// <returns>The transition matrix.</returns>
		/// <param name="clustersFound">Clusters found.</param>
		/// <param name="NoClusters">No clusters.</param>
		private double[,] generateTransitionMatrix(Queue<int> clustersFound, int NoClusters)
		{			
			if (clustersFound.Count == 0)
			{
				Exception e = new InvalidOperationException();
				this.logger.error("No Points were found in Clusters", e);
				throw e;
			}

			var transitionMatrix = new double[NoClusters, NoClusters];
			var rowTotals = new double[NoClusters]; 

			int previous = clustersFound.Dequeue();
			while (clustersFound.Count != 0)
			{
				int next = clustersFound.Dequeue();
				transitionMatrix[previous, next]++;
				rowTotals[previous]++; 

				previous = next;
			}

			return this.calcFrequencies(transitionMatrix, rowTotals); 
		}

		/// <summary>
		/// Calculates the frequencies of each of the cells in the transition matrix according to the corresponding row total
		/// </summary>
		/// <returns>The frequencies.</returns>
		/// <param name="transitionMatrix">Transition matrix.</param>
		/// <param name="rowTotals">Row totals.</param>
		private double[,] calcFrequencies(double[,] transitionMatrix, double[] rowTotals)
		{
			for (int i = 0; i < transitionMatrix.GetLength(0); i++)
			{
				for (int j = 0; j < transitionMatrix.GetLength(1); j++)
				{
					transitionMatrix[i,j] /= rowTotals[i]; 
				}
			}

			return transitionMatrix;
		}

		// <inheritdoc>
		public ICollection<Incident> predict(ICollection<Incident> dataSet)
		{
			throw new NotImplementedException();
		}
	}
}