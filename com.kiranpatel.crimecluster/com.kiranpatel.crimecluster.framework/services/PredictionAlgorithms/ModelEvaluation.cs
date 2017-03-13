namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq;

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

			this.mixedMarkovModel.GenerateModel(); 
		}

		// <inheritdoc>
		public double Evaluate(DateTime testStart, DateTime testEnd, double Radius)
		{		
			this.logger.info("Evaluating the Model.");	

			double correct = 0;
			double total = 0;

			// Get the incidents for the date range of the test data set and order them. 
			var testIncidents = this.incidentService.getForDateRange(testStart, testEnd).OrderBy(x => x.DateCreated).ToList();

			// Go through each incident in the test incidents
			foreach (Incident currentIncident in testIncidents)
			{				
				//Parse the current incident crime type to an enumeration.
				CrimeType currentType = CrimeType.Default;
				if (!Enum.TryParse(currentIncident.CrimeType, out currentType))
				{
					string message = $"crime type {currentIncident.CrimeType} could not be parsed";
					var e = new InvalidOperationException(message);
					this.logger.error(message, e);
					throw e; 
				}

				//Create a prediction point
				var predictedPoint = this.mixedMarkovModel.Predict(currentType);

				// For each point in the test incident set, check if the predicted point is within range (R) of any test point. 
				foreach (Incident currentCheck in testIncidents)
				{
					var currentPoint = new double[] { currentCheck.Location.Latitude.Value, currentCheck.Location.Longitude.Value };
					if (this.distanceMeasure.measure(currentPoint, predictedPoint) < Radius)
					{
						this.logger.debug($"Match found between predicted { predictedPoint[0] + " " + predictedPoint[1] } and Incident { currentCheck.ID }");
						correct++;
						break; 
					}
				}

				this.mixedMarkovModel.AddIncident(currentIncident); 
				total++;
			}

			return correct / total; 
		}
	}
}
