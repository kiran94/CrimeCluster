namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq;
	using System.Collections.Generic;

	/// <summary>
	/// Mixed markov model implementation.
	/// </summary>
	public class MixedMarkovModel : IMixedMarkovModel
	{
		/// <summary>
		/// The model lookup.
		/// </summary>
		private readonly Dictionary<CrimeType, MarkovModel> modelLookup;

		/// <summary>
		/// The clustering service.
		/// </summary>
		private readonly IClusteringService clusteringService;

		/// <summary>
		/// The incident service.
		/// </summary>
		private readonly IIncidentService incidentService;

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly ILogger logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.MixedMarkovModel"/> class.
		/// </summary>
		/// <param name="clusteringService">Clustering service.</param>
		/// <param name="incidentService">Incident service.</param>
		/// <param name="logger">Logger.</param>
		public MixedMarkovModel(IClusteringService clusteringService, IIncidentService incidentService, ILogger logger)
		{
			this.modelLookup = new Dictionary<CrimeType, MarkovModel>();

			this.clusteringService = clusteringService;
			this.incidentService = incidentService; 
			this.logger = logger;
		}

		// <inheritdoc>
		public void GenerateModel()
		{
			this.logger.debug("Generating Mixed Markov Model");
			foreach (CrimeType currentEnum in Enum.GetValues(typeof(CrimeType)))
			{
				if (currentEnum == CrimeType.Default)
				{
					continue; 
				}

				this.logger.info($"Generating Markov Model for {currentEnum.ToString()}");

				var currentIncidents = this.incidentService.getAllForCrimeType(currentEnum)
					.ToHashSet();

				double[][] dataSet = currentIncidents
					.Select(x => new double[] { x.Location.Latitude.Value, x.Location.Longitude.Value })
					.ToArray();

				this.logger.debug($"Clustering Data for {currentEnum.ToString()}");
				var rawClusters = this.clusteringService.Learn(dataSet);

				var clusters = new List<Cluster>();
				for (int i = 0; i < rawClusters.Count; i++)
				{
					clusters.Add(new Cluster(i, rawClusters[i])); 
				}

				//this.logger.debug($"Generating Transition Matrix for {currentEnum.ToString()}");
				var model = new MarkovModel(currentEnum, this.logger);
				model.generateTransitionMatrix(currentIncidents, clusters);

				this.modelLookup.Add(currentEnum, model); 
			}

			this.logger.debug("Mixed Markov Model generated.");
		}

		// <inheritdoc>
		public double[] Predict(CrimeType type)
		{
			this.logger.info($"Predicting on type {type.ToString()}");

			if (!this.modelLookup.ContainsKey(type))
			{
				string message = $"{type.ToString()} not in lookup";
				var e = new InvalidOperationException(message);
				this.logger.error(message, e);
				throw e; 
			}

			this.modelLookup[type].predict();
			var predictionPoint = this.modelLookup[type].getPredictionPoint();

			this.logger.debug($"Generated Prediction Point, Lat: {predictionPoint[0]}, Long: {predictionPoint[1]}");
			return predictionPoint; 
		}
	}
}