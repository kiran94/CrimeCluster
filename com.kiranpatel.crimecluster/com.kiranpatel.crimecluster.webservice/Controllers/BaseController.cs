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
	public class BaseController : Controller, IDisposable 
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

		/// <summary>
		/// Releases all resource used by the
		/// <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.BaseController"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.BaseController"/>. The <see cref="Dispose"/>
		/// method leaves the <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.BaseController"/> in an unusable
		/// state. After calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.BaseController"/> so the garbage collector can
		/// reclaim the memory that the <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.BaseController"/> was occupying.</remarks>
		public new void Dispose()
		{
			base.Dispose(); 
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected virtual new void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.repository != null)
				{
					this.repository.Dispose(); 
				}

				if (this.configService != null)
				{
					this.configService.Dispose(); 
				}

				if (this.serialisationService != null)
				{
					this.serialisationService.Dispose(); 
				}
			}
		}
    }
}