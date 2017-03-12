namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Contract for a Markov Model
	/// </summary>
	public interface IMarkovModel : IPredictionService
	{
		/// <summary>
		/// Generates the transition matrix by going through the incidents and marking down the clusters any of the incidents are in and 
		/// the next cluster they transition into. The frequencies are counted and then probabilites are determined from the total observerations.
		/// </summary>
		/// <returns>The transition matrix.</returns>
		/// <param name="incidents">Incidents to generate transiton matrix for.</param>
		/// <param name="clusters">Clusters to generate transition matrix for.</param>
		double[,] generateTransitionMatrix(ICollection<Incident> incidents, List<Cluster> clusters);
	}
}