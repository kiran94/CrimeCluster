namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using KdTree;
	using KdTree.Math;

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
		/// The incident service.
		/// </summary>
		private readonly IIncidentService incidentService;

		/// <summary>
		/// The distance measure.
		/// </summary>
		private readonly IDistanceMeasure distanceMeasure; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.ModelEvaluation"/> class.
		/// </summary>
		/// <param name="mixedMarkovModel">Mixed markov model.</param>
		/// <param name="incidentService">Incident Service.</param>
		/// <param name="logger">Logger.</param>
		public ModelEvaluation(IMixedMarkovModel mixedMarkovModel, IIncidentService incidentService, IDistanceMeasure distanceMeasure, ILogger logger)
		{
			this.mixedMarkovModel = mixedMarkovModel;
			this.incidentService = incidentService;
			this.distanceMeasure = distanceMeasure; 
			this.logger = logger;
		}

		// <inheritdoc>
		public void SetUp()
		{
			this.mixedMarkovModel.GenerateModel();
		}

		// <inheritdoc>
		public double Evaluate(DateTime testStart, DateTime testEnd, double Radius)
		{		
			this.logger.info("Evaluating the Model.");	

			IList<Incident> testSet = this.incidentService.getForDateRange(testStart, testEnd).OrderBy(x => x.DateCreated).ToList();
			IKdTree<double, string> kdTree = this.generateKdTree(testSet);

			double correct = 0;
			double error = 0;

			double countDone = 0;
			double countToDo = testSet.Count;

			foreach (var currentIncident in testSet)
			{				
				CrimeType currentType = default(CrimeType);
				if (!Enum.TryParse(currentIncident.CrimeType, out currentType))
				{
					this.logger.debug($"Could not parse crime type {currentIncident.CrimeType}"); 
					continue;
				}

				if (!this.mixedMarkovModel.IsGenerated(currentType))
				{
					this.logger.debug($"model was not generated for {currentType}");
					continue; 
				}

				var predictedPoint = this.mixedMarkovModel.Predict(currentType);
				if (predictedPoint == null)
				{
					this.logger.debug($"predicted point was null for {currentIncident.ID}");
					continue;
				}

				var nearest = kdTree.GetNearestNeighbours(predictedPoint, 1);

				if (this.distanceMeasure.measure(nearest.First().Point, predictedPoint) <= Radius)
				{
					this.logger.debug($"Match found between predicted { predictedPoint[0] } , { predictedPoint[1] } and Incident { nearest.First().Value }");
					correct++;				
				}
				else
				{
					error++; 
				}
												                        			
				this.mixedMarkovModel.AddIncident(currentIncident);
				this.progressCheck(++countDone, countToDo); 
			}

			return correct / (correct + error); 
		}

		/// <summary>
		/// Generates a KD Tree for the test incidents.
		/// </summary>
		/// <returns>The kd tree of test incidents.</returns>
		/// <param name="incidents">Test Incidents.</param>
		private IKdTree<double, string> generateKdTree(IList<Incident> incidents)
		{
			var tree = new KdTree<double, string>(2, new DoubleMath());

			foreach (var currentIncident in incidents)
			{
				tree.Add(
					new double[] { currentIncident.Location.Latitude.Value, currentIncident.Location.Longitude.Value }, 
					currentIncident.ID.ToString()); 
			}

			tree.Balance(); 
			return tree; 
		}

		/// <summary>
		/// Checks if progress should be logged, logged if so. 
		/// </summary>
		/// <param name="countDone">Count done.</param>
		/// <param name="countToDo">Count to do.</param>
		private void progressCheck(double countDone, double countToDo)
		{
			const int interval = 500;
			const int percentDp = 2; 
			double percentDone = countDone / countToDo;

			if (Math.Abs((countDone % interval)) < double.Epsilon)
			{
				this.logger.info($"{ Math.Round(percentDone, percentDp) * 100 }% done");
			}
		}
	}
}
