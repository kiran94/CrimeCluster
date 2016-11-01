namespace com.kiranpatel.crimecluster.webservice.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;

	using com.kiranpatel.crimecluster.framework;

	/// <summary>
	/// Officer controller.
	/// </summary>
	public class OfficerController : BaseController
    {
		/// <summary>
		/// The officer service.
		/// </summary>
		private readonly IOfficerService officerService;

		/// <summary>
		/// The location service.
		/// </summary>
		private readonly ILocationService locationService; 

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.OfficerController"/> class.
		/// </summary>
		/// <param name="repository">Repository.</param>
		/// <param name="configService">Config service.</param>
		/// <param name="logger">Logger.</param>
		/// <param name="serialisationService">Serialisation service.</param>
		/// <param name="officerService">Officer service.</param>
		/// <param name="locationService">Location service.</param>
		public OfficerController(
			IRepository repository,
			IConfigurationService configService,
			ILogger logger,
			ISerialisationService serialisationService,
			IOfficerService officerService,
			ILocationService locationService)
			: base(repository, configService, logger, serialisationService)
		{
			this.officerService = officerService;
			this.locationService = locationService;
		}

		///// <summary>
		///// Gets data for officer with the identifier
		///// GET: /Get
		///// </summary>
		///// <param name="ID">Identifier.</param>
		public String Get(String ID)
		{
			Guid officerID; 

			if (String.IsNullOrEmpty(ID))
			{
				var model = this.logError("Officer: ID was null or Empty");
				return this.serialisationService.serialise(model); 
			}

			if (!Guid.TryParse(ID, out officerID))
			{
				var model = this.logError("Officer: Failed to parse Guid");
				return this.serialisationService.serialise(model);
			}

			Officer officer = this.officerService.Get(officerID);

			if (officer == null)
			{
				var model = this.logError("Officer: Officer not found");
				return this.serialisationService.serialise(model);
			}

			var responseResultModel = new ResponseResultModel() 
			{ 
				Status = ResponseResultType.OK, 
				Message = "Officer Retrieved", 
				Response = this.serialisationService.serialise(officer)
			};

			return this.serialisationService.serialise(responseResultModel); 
		}

		/// <summary>
		/// Recieves a serialised officer from a client and passes to the service layer
		/// POST: /Save
		/// </summary>
		[HttpPost]
		public String Save(String serialisedOfficer)
		{
			if (String.IsNullOrEmpty(serialisedOfficer))
			{
				var model = this.logError("Officer: Officer null or empty");
				return this.serialisationService.serialise(model); 
			}

			var officer = this.serialisationService.deserialise<Officer>(serialisedOfficer);

			if (officer == null)
			{
				var model = this.logError("Officer: Officer is wrong format");
				return this.serialisationService.serialise(model);
			}

			if (!this.officerService.validate(officer))
			{
				var model = this.logError("Officer: Validation Failed");
				return this.serialisationService.serialise(model);
			}

			this.logger.debug("Officer: Saved"); 
			this.officerService.Save(officer);

			var responseModel = new ResponseResultModel()
			{
				Status = ResponseResultType.OK,
				Message = "Officer: Saved"
			};

			return this.serialisationService.serialise(responseModel); 
		}

		/// <summary>
		/// Updates the location of an officer
		/// </summary>
		/// <returns>The location.</returns>
		/// <param name="ID">Identifier.</param>
		/// <param name="serialisedLocation">Serialised location.</param>
		[HttpPost]
		public String updateLocation(String ID, String serialisedLocation)
		{
			if (String.IsNullOrEmpty(ID))
			{
				var model = this.logError("Officer: null or empty ID");
				return this.serialisationService.serialise(model);
			}

			if (String.IsNullOrEmpty(serialisedLocation))
			{
				var model = this.logError("Officer: null or empty serialise location");
				return this.serialisationService.serialise(model);
			}

			Guid officerID;
			if (!Guid.TryParse(ID, out officerID))
			{
				var model = this.logError("Officer: invalid officer id");
				return this.serialisationService.serialise(model);
			}

			Location location = this.serialisationService.deserialise<Location>(serialisedLocation);
			if (location == null || !this.locationService.validate(location))
			{
				var model = this.logError("Officer: location failed to serialise or did not validate");
				return this.serialisationService.serialise(model);
			}

			Officer officer = this.officerService.Get(officerID);
			if (officer == null)
			{
				var model = this.logError("Officer: officer could not be found");
				return this.serialisationService.serialise(model);
			}

			this.logger.debug(String.Format("Updating Officer {0} location", officer.ID.ToString())); 
			officer.Location = location;
			this.officerService.Update(officer);

			var resultReturnModel = new ResponseResultModel()
			{
				Status = ResponseResultType.OK,
				Message = "Officer: Location Updated"
			};

			return this.serialisationService.serialise(resultReturnModel); 
		}

		/// <summary>
		/// Releases all resource used by the
		/// <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.OfficerController"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.OfficerController"/>. The <see cref="Dispose"/>
		/// method leaves the <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.OfficerController"/> in an
		/// unusable state. After calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.OfficerController"/> so the garbage collector can
		/// reclaim the memory that the <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.OfficerController"/>
		/// was occupying.</remarks>
		public new void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected virtual new void Dispose(bool disposing)
		{
			if (this.officerService != null)
			{
				this.officerService.Dispose(); 
			}
		}
    }
}