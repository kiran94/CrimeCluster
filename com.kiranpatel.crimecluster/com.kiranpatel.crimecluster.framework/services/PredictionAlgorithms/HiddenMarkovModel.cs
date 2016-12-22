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
		public double[,] generateTransitionMatrix(double[,] dataSet)
		{
			throw new NotImplementedException();
		}

		// <inheritdoc>
		public ICollection<Incident> predict(ICollection<Incident> dataSet)
		{
			throw new NotImplementedException();
		}
	}
}
