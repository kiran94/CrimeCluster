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
		/// Predicts a list of potential <see cref="Incident"/> locations based on a data set of <see cref="Incident"/> cases
		/// </summary>
		/// <param name="dataSet">Data set to make predictions on.</param>
		ICollection<Incident> predict(ICollection<Incident> dataSet);
	}
}