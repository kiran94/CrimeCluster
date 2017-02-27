namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Contract for Mixed Markov Model.
	/// </summary>
	public interface IMixedMarkovModel
	{
		/// <summary>
		/// Generates the Mixed Markov Model.
		/// </summary>
		/// <param name="incidents">Incidents to generate the model on.</param>
		void GenerateModel(IQueryable<Incident> incidents);

		/// <summary>
		/// Predicts the next location for the given type.
		/// </summary>
		/// <param name="type">type to predict against.</param>
		double[] Predict(CrimeType type);
	}
}