namespace com.kiranpatel.crimecluster.framework
{	
	/// <summary>
	/// Contract for Mixed Markov Model.
	/// </summary>
	public interface IMixedMarkovModel
	{
		/// <summary>
		/// Generates the Mixed Markov Model.
		/// </summary>
		void GenerateModel();

		/// <summary>
		/// Predicts the next location for the given type.
		/// </summary>
		/// <param name="type">type to predict against.</param>
		double[] Predict(CrimeType type);

		/// <summary>
		/// Adds an incident to the appropriate Markov Model.
		/// </summary>
		/// <param name="incident">Incident.</param>
		void AddIncident(Incident incident);

		/// <summary>
		/// Checks whether the model for the parameter crime type has been genereted. 
		/// </summary>
		/// <returns><c>true</c>, if generated, <c>false</c> otherwise.</returns>
		/// <param name="type">Type.</param>
		bool IsGenerated(CrimeType type);
	}
}