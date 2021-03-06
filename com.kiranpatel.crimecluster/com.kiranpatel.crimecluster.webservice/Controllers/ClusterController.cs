﻿namespace com.kiranpatel.crimecluster.webservice.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;
	using com.kiranpatel.crimecluster.framework;

	/// <summary>
	/// Controller for Cluster Operations
	/// </summary>
	public class ClusterController : BaseController
    {
		/// <summary>
		/// The incident service.
		/// </summary>
		private readonly IIncidentService incidentService;

		/// <summary>
		/// The clustering service.
		/// </summary>
		private readonly IClusteringService clusteringService; 

		/// <summary>
		/// The crime types.
		/// </summary>
		private readonly IDictionary<int, CrimeType> crimeTypes; 

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.ClusterController"/> class.
		/// </summary>
		/// <param name="repository">Repository.</param>
		/// <param name="configService">Config service.</param>
		/// <param name="logger">Logger.</param>
		/// <param name="serialisationService">Serialisation service.</param>
		/// <param name="incidentService">Incident service.</param>
		public ClusterController(
			IRepository repository,
			IConfigurationService configService,
			ILogger logger,
			ISerialisationService serialisationService,
			IIncidentService incidentService,
			IClusteringService clusteringService) 
			: base(repository, configService, logger, serialisationService)
		{
			this.incidentService = incidentService;
			this.clusteringService = clusteringService;

			var googleMapsKey = this.configService.Get(ConfigurationKey.GoogleMapsKey, string.Empty);
			if (String.IsNullOrEmpty(googleMapsKey))
			{
				throw new InvalidOperationException(nameof(googleMapsKey));
			}

			this.logger.debug("Google Maps Key Loaded"); 
			ViewBag.GoogleMapsKey = googleMapsKey;

			int counter = 0; 
			this.crimeTypes = new Dictionary<int, CrimeType>();
			foreach (CrimeType currentType in Enum.GetValues(typeof(CrimeType)))
			{
				crimeTypes.Add(counter, currentType);
				counter++; 
			}
		}

		/// <summary>
		/// GET: /Index
		/// </summary>
        public ActionResult Index()
		{
			ViewBag.Eps = Convert.ToDouble(this.configService.Get(ConfigurationKey.DJClusterRadiusEps, "0.05"));
			ViewBag.MinPts = Convert.ToInt32(this.configService.Get(ConfigurationKey.DJClusterMinPts, "10"));

			CrimeTypeModel model = new CrimeTypeModel();
            return View (model);
        }

		/// <summary>
		/// POST: /Filter
		/// Filters the crimes by a given crime type and returns the location points.
		/// </summary>
		/// <returns>the filtered json response of incidents</returns>
		/// <param name="crimeType">The crime type to filter by.</param>
		[HttpPost]
		public JsonResult Filter(String crimeType)
		{
			if (String.IsNullOrEmpty(crimeType))
			{
				this.logger.warn($"{nameof(crimeType)} was null or empty");
				return new JsonResult(); 
			}

			int parsedType = 0;
			if (!int.TryParse(crimeType, out parsedType))
			{
				this.logger.warn($"{nameof(crimeType)} could not be parsed into an integer");
				return new JsonResult(); 
			}

			if (!this.crimeTypes.ContainsKey(parsedType))
			{
				this.logger.warn($"{nameof(this.crimeTypes)} does not contain {nameof(parsedType)}");
				return new JsonResult();
			}

			CrimeType descType = this.crimeTypes[parsedType];

			var start = Convert.ToDateTime(configService.Get(ConfigurationKey.TestStartDate, "01/01/2016"));
			var end = Convert.ToDateTime(configService.Get(ConfigurationKey.TestEndDate, "31/12/2016"));

			var relatedIncidents = this.incidentService
			                           .getAllForCrimeType(descType)
			                           .Where(x => x.DateCreated >= start && x.DateCreated <= end);

			List<LocationModel> locations = relatedIncidents
				.Select(x => new LocationModel(x.Location.Latitude.ToString(), x.Location.Longitude.ToString()))
				.ToList();

			JsonResult result = new JsonResult();
			result.Data = this.serialisationService.serialise<List<LocationModel>>(locations);

			return result; 
		}

		/// <summary>
		/// POST: /Cluster
		/// Clusters the crimes by a given crime type and returns those location points along with cluster labels. 
		/// </summary>
		/// <param name="CrimeType">Crime type.</param>
		[HttpPost]
		public JsonResult Cluster(String CrimeType)
		{
			if (String.IsNullOrEmpty(CrimeType))
			{
				this.logger.warn($"{nameof(CrimeType)} was null");
				return new JsonResult(); 
			}

			int parsedType = 0;
			if (!int.TryParse(CrimeType, out parsedType))
			{
				this.logger.warn($"{nameof(CrimeType)} could not be parsed into an integer");
				return new JsonResult(); 
			}

			var descType = this.crimeTypes[parsedType];

			var start = Convert.ToDateTime(configService.Get(ConfigurationKey.TestStartDate, "01/01/2016"));
			var end = Convert.ToDateTime(configService.Get(ConfigurationKey.TestEndDate, "31/12/2016"));

			var incidents = this.incidentService
									   .getAllForCrimeType(descType)
									   .Where(x => x.DateCreated >= start && x.DateCreated <= end);

			if (incidents == null || incidents.Count() == 0)
			{
				this.logger.warn($"{nameof(incidents)} was null or empty");
				return new JsonResult(); 
			}

			var clusters = this.clusteringService.Learn(incidents
														.Select(x => new double[] { x.Location.Latitude.Value, x.Location.Longitude.Value })
														.ToArray());

			int clusterNo = 1;
			var locationModel = new List<LocationModel>(); 
			foreach (var currentCluster in clusters)
			{
				foreach (var currentPoint in currentCluster)
				{
					LocationModel model = new LocationModel(currentPoint[0].ToString(), currentPoint[1].ToString(), clusterNo);
					locationModel.Add(model); 
				}

				clusterNo++; 
			}

			return new JsonResult() { Data = this.serialisationService.serialise<List<LocationModel>>(locationModel) };
		}
    }
}