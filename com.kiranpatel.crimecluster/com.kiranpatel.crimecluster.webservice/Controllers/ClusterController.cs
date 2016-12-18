namespace com.kiranpatel.crimecluster.webservice.Controllers
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
			IIncidentService incidentService) 
			: base(repository, configService, logger, serialisationService)
		{
			this.incidentService = incidentService; 

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
			CrimeTypeModel model = new CrimeTypeModel();
            return View (model);
        }

		/// <summary>
		/// POST: /Filter
		/// </summary>
		/// <returns>the filtered json response of incidents</returns>
		/// <param name="crimeType">The crime type to filter by.</param>
		[HttpPost]
		public JsonResult Filter(String crimeType)
		{
			if (String.IsNullOrEmpty(crimeType))
			{
				this.logger.warn("Crime Type was null or empty");
				return new JsonResult(); 
			}

			int parsedType = 0;
			if (!int.TryParse(crimeType, out parsedType))
			{
				this.logger.warn("crime type could not be parsed into an integer");
				return new JsonResult(); 
			}

			if (!this.crimeTypes.ContainsKey(parsedType))
			{
				this.logger.warn("dictionary does not contain the parsed type key");
				return new JsonResult();
			}

			CrimeType descType = this.crimeTypes[parsedType];

			var relatedIncidents = this.incidentService.getAllForCrimeType(descType);

			List<LocationModel> locations = relatedIncidents
				.Select(x => new LocationModel(x.Location.Latitude.ToString(), x.Location.Longitude.ToString()))
				.ToList();

			JsonResult result = new JsonResult();
			result.Data = this.serialisationService.serialise<List<LocationModel>>(locations);

			return result; 
		}
    }
}
