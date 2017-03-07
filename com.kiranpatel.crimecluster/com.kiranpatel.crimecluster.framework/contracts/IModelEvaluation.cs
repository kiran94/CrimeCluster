namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Contract for Evaluating the Model.
	/// </summary>
	public interface IModelEvaluation
	{
		/// <summary>
		/// Evaluates the Model by training the model on data from trainingStart to trainingEnd and then scoring the accuracy against 
		/// data from testStart to testStart. 
		/// </summary>
		/// <returns>An Accuracy value.</returns>
		/// <param name="trainingStart">Training start.</param>
		/// <param name="trainingEnd">Traninig end.</param>
		/// <param name="testStart">Test start.</param>
		/// <param name="testEnd">Test end.</param>
		double Evaluate(DateTime trainingStart, DateTime trainingEnd, DateTime testStart, DateTime testEnd); 
	}
}