namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq;
	using System.Collections.Generic;

	/// <summary>
	/// Hidden markov model implementation.
	/// </summary>
	public class HiddenMarkovModel : IHiddenMarkovModel
	{
		/// <summary>
		/// The transition matrix.
		/// </summary>
		private double[,] _transitionMatrix;

		/// <summary>
		/// The emission matrix.
		/// </summary>
		private double[,] _emissionMatrix;

		// <inheritdoc>
		public double[,] generateEmissionMatrix(List<HashSet<double[]>> clusters)
		{
			var emissionMatrix = new double[2, clusters.Count];

			int count = 0;
			foreach (var currentCluster in clusters)
			{
				emissionMatrix[0, count] = Math.Round(currentCluster.Average(x => x[0]), 4);
				emissionMatrix[1, count] = Math.Round(currentCluster.Average(x => x[1]), 4);
				count++;
			}

			this._emissionMatrix = emissionMatrix; 
			return emissionMatrix;
		}

		// <inheritdoc>
		public double[,] generateTransitionMatrix(ICollection<Incident> incidents, List<Cluster> clusters)
		{
			double[,] transition = new double[clusters.Count, clusters.Count];

			int? currentState = null;
		
			foreach (var currentIncident in incidents)
			{
				double[] currentPoint = { currentIncident.Location.Latitude.Value, currentIncident.Location.Longitude.Value };

				int nextState = 0;
				foreach (var currentCluster in clusters)
				{
					if (currentCluster.contains(currentPoint)) // then we are currently in this cluster (state)
					{
						if (currentState.HasValue)
						{
							transition[currentState.Value, nextState]++; // increment cell with current cell and the next state
						}

						currentState = nextState;
					}

					nextState++;
				}
			}

			transition = this.calculateTransitionMatrixProbabilities(transition); 

			this._transitionMatrix = transition; 
			return transition;
		}

		// <inheritdoc>
		public double[] generateInitialDistribution(ICollection<Incident> incidents, List<Cluster> clusters)
		{
			double[] initial = new double[clusters.Count];

			var initialDate = incidents.Min(x => x.DateCreated);
			var filteredIncidents = incidents.Where(x => x.DateCreated == initialDate);
			var totalObserverations = 0; 

			foreach (var currentIncident in filteredIncidents)
			{
				double[] currentPoint = { currentIncident.Location.Latitude.Value, currentIncident.Location.Longitude.Value };
				int clusterIndex = 0; 

				foreach (var currentCluster in clusters)
				{
					if (currentCluster.contains(currentPoint)) //then increment the current cluster index
					{
						initial[clusterIndex]++;
						totalObserverations++;
					}

					clusterIndex++; 
				}
			}

			if (totalObserverations == 0)
			{
				throw new InvalidOperationException("No matching oberservations found."); 
			}

			//here 
			initial = calculateInitialDistributionProbabilities(initial); 

			return initial; 
		}

		// <inheritdoc>
		public ICollection<Incident> predict(ICollection<Incident> dataSet)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Calculates the transition matrix probabilities by multiplying dividing each cell by the total number of incidents.
		/// </summary>
		/// <returns>The transition matrix probabilities.</returns>
		/// <param name="transition">Transition.</param>
		/// <param name="rowTotals">Row totals.</param>
		private double[,] calculateTransitionMatrixProbabilities(double[,] transition)
		{
			var rowTotals = new double[transition.GetLength(0)];

			for (int i = 0; i < transition.GetLength(0); i++)
			{
				for (int j = 0; j < transition.GetLength(1); j++)
				{
					rowTotals[i] += transition[i, j];
				}
			}

			for (int i = 0; i < transition.GetLength(0); i++)
			{
				for (int j = 0; j < transition.GetLength(1); j++)
				{
					transition[i, j] = transition[i, j] / rowTotals[i];
				}
			}

			return transition;
		}

		private double[] calculateInitialDistributionProbabilities(double[] initial)
		{
			double totalObservations = 0;

			foreach (var currentInitial in initial)
			{
				totalObservations += currentInitial;
			}

			for (int i = 0; i < initial.Length; i++)
			{
				initial[i] = Math.Round(initial[i] / totalObservations, 4);
			}

			return initial;
		}
	}
}
