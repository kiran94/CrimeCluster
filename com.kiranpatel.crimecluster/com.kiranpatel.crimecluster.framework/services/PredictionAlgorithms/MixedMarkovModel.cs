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
		/// The logger.
		/// </summary>
		private readonly ILogger logger;

		/// <summary>
		/// The clustering service.
		/// </summary>
		private readonly IClusteringService clusteringService; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.MixedMarkovModel"/> class.
		/// </summary>
		/// <param name="logger">Logger.</param>
		public MixedMarkovModel(IClusteringService clusteringService, ILogger logger)
		{
			this.modelLookup = new Dictionary<CrimeType, MarkovModel>();
			this.clusteringService = clusteringService;
			this.logger = logger;
		}

		// <inheritdoc>
		public void GenerateModel(IQueryable<Incident> incidents)
		{
			foreach (CrimeType currentEnum in Enum.GetValues(typeof(CrimeType)))
			{
				// Be careful here.. Not sure if doing x.Location will eager load the whole instance in..
				IEnumerable<Incident> currentIncidents = incidents
					.Where(x => x.CrimeType.Equals(currentEnum))
					.OrderBy(x => x.Location.DateLogged);

				var incidentHashSet = new HashSet<Incident>(currentIncidents);

				double[][] dataSet = incidentHashSet
					.Select(x => new double[] { x.Location.Latitude.Value, x.Location.Longitude.Value })
					.ToArray();

				// factory?

				var rawClusters = this.clusteringService.Learn(dataSet);
				var clusters = new List<Cluster>();

				for (int i = 0; i < rawClusters.Count; i++)
				{
					clusters.Add(new Cluster(i, rawClusters[i])); 
				}

				MarkovModel model = new MarkovModel(currentEnum, this.logger);
				model.generateTransitionMatrix(incidentHashSet, clusters);

				this.modelLookup.Add(currentEnum, model); 
			}


			throw new NotImplementedException(); 
		}

		// <inheritdoc>
		public double[] Predict(CrimeType type)
		{
			if (!this.modelLookup.ContainsKey(type))
			{
				string message = $"{type.ToString()} not in lookup";
				var e = new InvalidOperationException(message);
				this.logger.error(message, e);
				throw e; 
			}

			this.modelLookup[type].predict();
			return this.modelLookup[type].getPredictionPoint();

			throw new NotImplementedException();
		}
	}
}