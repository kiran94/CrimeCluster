namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic; 

	/// <summary>
	/// Contract for the Prediction Service
	/// </summary>
	public interface IPredictionService
	{
		/// <summary>
		/// Predicts the next state based on the current state
		/// </summary>
		int predict();
	}
}