namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Contract for a Hidden Markov Model
	/// </summary>
	public interface IHiddenMarkovModel : IPredictionService
	{
		/// <summary>
		/// Generates the transition matrix.
		/// </summary>
		/// <returns>The transition matrix.</returns>
		/// <param name="dataSet">Data set.</param>
		double[,] generateTransitionMatrix(double[,] dataSet);

		/// <summary>
		/// Generates the emission matrix.
		/// </summary>
		/// <returns>The emission matrix.</returns>
		/// <param name="clusters">Clusters.</param>
		double[,] generateEmissionMatrix(List<HashSet<double[]>> clusters);
	}
}