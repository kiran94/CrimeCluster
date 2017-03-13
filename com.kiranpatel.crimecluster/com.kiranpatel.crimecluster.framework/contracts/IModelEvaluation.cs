namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Contract for Evaluating the Model.
	/// </summary>
	public interface IModelEvaluation
	{
		/// <summary>
		/// Evaluates the Model by testing the accuracy on the parameter date time range. 
		/// </summary>
		/// <returns>An Accuracy value.</returns>
		/// <param name="testStart">Test start.</param>
		/// <param name="testEnd">Test end.</param>
		/// <param name="Radius">Radius to compare predicted point to an actual point.</param>
		double Evaluate(DateTime testStart, DateTime testEnd, double Radius); 
	}
}