namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Hidden markov model implementation.
	/// </summary>
	public class MarkovModel : IMarkovModel
	{
		/// <summary>
		/// Gets the type of the crime.
		/// </summary>
		/// <value>The type of the crime.</value>
		public CrimeType crimeType { get; private set; }

		/// <summary>
		/// Gets the current state of the model. 
		/// </summary>
		/// <value>The state of the current.</value>
		public int currentState { get; private set; }

		/// <summary>
		/// The transition matrix.
		/// </summary>
		private double[,] _transitionMatrix;

		/// <summary>
		/// The clusters (states).
		/// </summary>
		private IList<Cluster> clusters;

		/// <summary>
		/// Flag determining if the model has been generated yet. 
		/// </summary>
		private bool modelGenerated; 

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly ILogger logger; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.MarkovModel"/> class.
		/// </summary>
		/// <param name="type">crime type this markov model instance is for.</param>
		/// <param name="logger">Logger.</param>
		public MarkovModel(CrimeType type, ILogger logger)
		{
			this.crimeType = type;
			this.modelGenerated = false;
			this.currentState = 0; 
			this.logger = logger; 
		}

		// <inheritdoc>
		public double[,] generateTransitionMatrix(ICollection<Incident> incidents, List<Cluster> clusters)
		{
			if (incidents == null)
			{
				throw new InvalidOperationException(nameof(incidents) + " should not be null");
			}

			if (clusters == null)
			{
				throw new InvalidOperationException(nameof(clusters) + " should not be null");
			}

			this.logger.debug($"Generating Transition matrix for {this.crimeType.GetDescription()}"); 

			var clustersFound = new Queue<int>();
			foreach (var currentIncident in incidents)
			{
				// maybe improve this? O(n*c)
				foreach (var currentCluster in clusters)
				{
					if (currentCluster.Contains(
						currentIncident.Location.Latitude.Value, 
						currentIncident.Location.Longitude.Value))
					{
						clustersFound.Enqueue(currentCluster.Label);
					}
				}
			}

			this._transitionMatrix = this.generateTransitionMatrix(clustersFound, clusters.Count);
			this.clusters = clusters;
			this.modelGenerated = true; 
			return this._transitionMatrix; 
		}

		// <inheritdoc>
		public int predict()
		{
			if (!this.modelGenerated)
			{
				var message = "Called predict when model was not generated."; 
				var e = new InvalidOperationException(message);
				this.logger.error(message, e);
				throw e; 
			}

			this.logger.debug($"Predicting for {this.crimeType.GetDescription()}");

			int nextState = this.currentState;
			double maxValue = Int32.MinValue;

			for (int i = 0; i < this._transitionMatrix.GetLength(1); i++)
			{
				double currentValue = this._transitionMatrix[this.currentState, i];

				if (currentValue > maxValue)
				{
					maxValue = currentValue;
					nextState = i;
				}
			}

			this.currentState = nextState;
			return this.currentState;
		}

		/// <summary>
		/// Gets the prediction point for the current state
		/// </summary>
		/// <returns>The prediction point.</returns>
		public double[] getPredictionPoint()
		{
			if (this.currentState >= this.clusters.Count)
			{
				var e = new InvalidOperationException($"Invalid State {this.currentState}");
				this.logger.error("Error getting prediction point", e);
				throw e;
			}

			return this.clusters[this.currentState].GetAveragePoint();
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
				// may need a value to determine how long ago this specific item took place so we can take into account crimes that took place a long time ago
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
	}
}