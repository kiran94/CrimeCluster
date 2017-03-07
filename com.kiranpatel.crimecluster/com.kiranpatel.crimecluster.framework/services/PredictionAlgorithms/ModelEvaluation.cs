namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Evaluator for Model.
	/// </summary>
	public class ModelEvaluation : IModelEvaluation
	{
		/// <summary>
		/// The logger.
		/// </summary>
		private readonly ILogger logger;

		/// <summary>
		/// The mixed markov model.
		/// </summary>
		private readonly IMixedMarkovModel mixedMarkovModel; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.ModelEvaluation"/> class.
		/// </summary>
		/// <param name="mixedMarkovModel">Mixed markov model.</param>
		/// <param name="logger">Logger.</param>
		public ModelEvaluation(IMixedMarkovModel mixedMarkovModel, ILogger logger)
		{
			this.mixedMarkovModel = mixedMarkovModel; 
			this.logger = logger;

			this.mixedMarkovModel.GenerateModel(); 
		}

		// <inheritdoc>
		public double Evaluate(
			DateTime trainingStart, 
			DateTime trainingEnd, 
			DateTime testStart1, 
			DateTime testEnd)
		{
			this.logger.info("Evaluating the Model.");
			throw new NotImplementedException();
		}
	}
}
