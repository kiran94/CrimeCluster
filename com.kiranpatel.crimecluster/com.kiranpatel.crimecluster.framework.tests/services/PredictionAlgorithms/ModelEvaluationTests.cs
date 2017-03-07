namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using NUnit.Framework;
	using Moq; 

	/// <summary>
	/// Model evaluation tests.
	/// </summary>
	[TestFixture]
	public class ModelEvaluationTests
	{
		/// <summary>
		/// The mixed markov model mock.
		/// </summary>
		private Mock<IMixedMarkovModel> mixedMarkovModel;

		/// <summary>
		/// The logger mock.
		/// </summary>
		private Mock<ILogger> logger;

		/// <summary>
		/// Ensures in the normal case, the model is evaluated. 
		/// </summary>
		[Test]
		public void Evaluate_NormalCase_ModelEvaluated()
		{
			throw new NotImplementedException(); 
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private ModelEvaluation GetInstance()
		{
			return new ModelEvaluation(this.mixedMarkovModel.Object, this.logger.Object);
		}
	}
}