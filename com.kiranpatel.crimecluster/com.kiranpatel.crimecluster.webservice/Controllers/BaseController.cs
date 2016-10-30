namespace com.kiranpatel.crimecluster.webservice.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;

	using com.kiranpatel.crimecluster.framework; 

	/// <summary>
	/// Base controller for all other controllers
	/// </summary>
    public class BaseController : Controller
    {
		/// <summary>
		/// The repository instance
		/// </summary>
		protected readonly IRepository repository;

		/// <summary>
		/// The config service instance
		/// </summary>
		protected readonly IConfigurationService configService;

		/// <summary>
		/// The logger instance
		/// </summary>
		protected readonly ILogger logger;

		/// <summary>
		/// The serialisation service instance
		/// </summary>
		protected readonly ISerialisationService serialisationService; 

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.BaseController"/> class.
		/// </summary>
		/// <param name="repository">Repository.</param>
		/// <param name="configService">Config service.</param>
		/// <param name="logger">Logger.</param>
		/// <param name="serialisationService">Serialisation service.</param>
		public BaseController(
			IRepository repository, 
			IConfigurationService configService, 
			ILogger logger, 
			ISerialisationService serialisationService)
		{
			this.repository = repository;
			this.configService = configService;
			this.logger = logger;
			this.serialisationService = serialisationService; 
		}

		protected ResponseResultModel logError(String message)
		{
			this.logger.warn(message);

			ResponseResultModel model = new ResponseResultModel()
			{
				Status = ResponseResultType.ERROR,
				Message = message
			};

			return model;
		}
    }
}