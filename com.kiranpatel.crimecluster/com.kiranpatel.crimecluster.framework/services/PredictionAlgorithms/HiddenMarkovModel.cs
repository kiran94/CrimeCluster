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
			throw new NotImplementedException(); 
		}

		// <inheritdoc>
		public double[,] generateTransitionMatrix(ICollection<Incident> incidents, List<Cluster> clusters)
		{
			throw new NotImplementedException(); 
		}

		// <inheritdoc>
		public double[] generateInitialDistribution(ICollection<Incident> incidents, List<Cluster> clusters)
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
