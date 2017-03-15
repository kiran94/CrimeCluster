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
		/// The model lookup, mapping a crime type to a markov model.
		/// </summary>
		private readonly Dictionary<CrimeType, MarkovModel> modelLookup;

		private Dictionary<CrimeType, HashSet<Incident>> incidentCache; 

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
		/// The start date the Mixed Markov Model covers. 
		/// </summary>
		private readonly DateTime start;

		/// <summary>
		/// The end date the Mixed Markov Model covers. 
		/// </summary>
		private readonly DateTime end; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.MixedMarkovModel"/> class.
		/// </summary>
		/// <param name="clusteringService">Clustering service.</param>
		/// <param name="incidentService">Incident service.</param>
		/// <param name="logger">Logger service.</param>
		/// <param name="start">Start date of the incidents in the MMM.</param>
		/// <param name="end">End date of the incidents in the MMM.</param>
		public MixedMarkovModel(
			IClusteringService clusteringService, 
			IIncidentService incidentService, 
			ILogger logger, 
			DateTime start, 
			DateTime end)
		{
			this.modelLookup = new Dictionary<CrimeType, MarkovModel>();
			this.incidentCache = new Dictionary<CrimeType, HashSet<Incident>>(); 

			this.clusteringService = clusteringService;
			this.incidentService = incidentService; 
			this.logger = logger;

			this.start = start;
			this.end = end; 
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

				this.GenerateModel(currentEnum); 
			}

			this.logger.debug("Mixed Markov Model generated.");
		}

		// <inheritdoc>
		public double[] Predict(CrimeType type)
		{
			this.logger.debug($"Predicting on type {type.ToString()}");

			if (!this.modelLookup.ContainsKey(type))
			{
				string message = $"{type.ToString()} not in lookup";
				var e = new InvalidOperationException(message);
				this.logger.error(message, e);
				throw e; 
			}

			this.modelLookup[type].predict();
			var predictionPoint = this.modelLookup[type].getPredictionPoint();

			if (predictionPoint != null && predictionPoint.Count() < 2)
			{
				this.logger.debug($"Generated Prediction Point, Lat: {predictionPoint[0]}, Long: {predictionPoint[1]}");
			}

			return predictionPoint; 
		}

		// <inheritdoc>
		public void AddIncident(Incident incident)
		{
			if (incident == null)
			{
				this.logger.warn($"Adding incident was null");
				return; 
			}

			CrimeType type = CrimeType.Default;
			if (!Enum.TryParse(incident.CrimeType, out type))
			{
				this.logger.warn($"Incident {incident.ID} has an invalid crime type {incident.CrimeType}");
				return; 
			}

			this.logger.debug($"Adding Incident { incident.ID.ToString() } to cache.");
			if (this.incidentCache.ContainsKey(type))
			{
				this.incidentCache[type].Add(incident);
			}
			else
			{
				this.logger.warn($"attempted to add incident with no model generated: {type}");
			}

			this.GenerateModel(type); 
		}

		/// <summary>
		/// Generates a Markov Model for the passed Crime Type and sets them to the internal dictionary.
		/// </summary>
		/// <param name="currentEnum">Current enum to generate the Markov Model for.</param>
		private void GenerateModel(CrimeType currentEnum)
		{
			this.logger.debug($"Generating Markov Model for {currentEnum.ToString()}");
			HashSet<Incident> currentIncidents; 

			// if already in the cache then get the cache, else make the database call. 
			if (this.incidentCache.ContainsKey(currentEnum))
			{
				currentIncidents = this.incidentCache[currentEnum];
			}
			else
			{
				currentIncidents = this.incidentService.getAllForCrimeType(currentEnum)
				                       .Where(x => x.DateCreated >= this.start && x.DateCreated <= this.end)
				                       .ToHashSet();

				this.incidentCache[currentEnum] = currentIncidents;
			}
					
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

			this.logger.debug($"Generating Transition matrix for {currentEnum.GetDescription()}");

			var model = new MarkovModel(currentEnum, this.logger);
			model.generateTransitionMatrix(currentIncidents, clusters);

			if (this.modelLookup.ContainsKey(currentEnum))
			{
				this.modelLookup[currentEnum] = model;
			}
			else
			{
				this.modelLookup.Add(currentEnum, model);
			}
		}
	}
}