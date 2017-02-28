﻿namespace com.kiranpatel.crimecluster.framework
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
		void GenerateModel();

		/// <summary>
		/// Predicts the next location for the given type.
		/// </summary>
		/// <param name="type">type to predict against.</param>
		double[] Predict(CrimeType type);

		/// <summary>
		/// Adds an incident to the appropiate Markov Model.
		/// </summary>
		/// <param name="incident">Incident.</param>
		void AddIncident(Incident incident);
	}
}