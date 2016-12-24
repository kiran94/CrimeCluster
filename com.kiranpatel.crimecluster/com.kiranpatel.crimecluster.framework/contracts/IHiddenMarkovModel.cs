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
		/// Generates the transition matrix by going through the incidents and marking down the clusters any of the incidents are in and 
		/// the next cluster they transition into. The frequencies are counted and then probabilites are determined from the total observerations
		/// </summary>
		/// <returns>The transition matrix.</returns>
		/// <param name="incidents">Incidents.</param>
		/// <param name="clusters">Clusters.</param>
		double[,] generateTransitionMatrix(ICollection<Incident> incidents, List<Cluster> clusters);

		/// <summary>
		/// Generates the emission matrix by going through the clusters and getting the averages for all the observerations by cluster. 
		/// </summary>
		/// <returns>The emission matrix.</returns>
		/// <param name="clusters">Clusters.</param>
		double[,] generateEmissionMatrix(List<HashSet<double[]>> clusters);

		/// <summary>
		/// Generates the initial distribution by looking at the first incidents indicated by the date and determining where each of these lie 
		/// within each cluster. These are counted and divided by the total observations to give a probability. 
		/// </summary>
		/// <returns>The initial distribution.</returns>
		/// <param name="incidents">Incidents.</param>
		/// <param name="clusters">Clusters.</param>
		double[] generateInitialDistribution(ICollection<Incident> incidents, List<Cluster> clusters);
	}
}