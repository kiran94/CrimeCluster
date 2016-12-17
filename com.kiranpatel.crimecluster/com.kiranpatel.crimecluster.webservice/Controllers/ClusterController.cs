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
	public class ClusterController : Controller
    {
		/// <summary>
		/// The logger instance. 
		/// </summary>
		private readonly ILogger logger;

		/// <summary>
		/// The config service.
		/// </summary>;
		private readonly IConfigurationService configService;

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.ClusterController"/> class.
		/// </summary>
		/// <param name="logger">Logger.</param>
		/// <param name="configService">Config service.</param>
		public ClusterController(ILogger logger, IConfigurationService configService)
		{
			this.logger = logger;
			this.configService = configService;

			var googleMapsKey = this.configService.Get(ConfigurationKey.GoogleMapsKey, string.Empty);
			if (String.IsNullOrEmpty(googleMapsKey))
			{
				throw new InvalidOperationException(nameof(googleMapsKey));
			}

			this.logger.debug("Google Maps Key Loaded"); 
			ViewBag.GoogleMapsKey = googleMapsKey;
		}

		/// <summary>
		/// GET: /Index
		/// </summary>
        public ActionResult Index()
		{
			CrimeTypeModel model = new CrimeTypeModel();
            return View (model);
        }
    }
}
