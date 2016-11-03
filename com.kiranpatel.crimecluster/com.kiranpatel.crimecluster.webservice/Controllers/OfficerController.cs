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
		/// The incident service.
		/// </summary>
		private readonly IIncidentService incidentService;

		/// <summary>
		/// The outcome service. 
		/// </summary>
		private readonly IIncidentOutcomeService outcomeService;

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
		/// <param name="incidentService"Incident service.</param>
		public OfficerController(
			IRepository repository,
			IConfigurationService configService,
			ILogger logger,
			ISerialisationService serialisationService,
			IOfficerService officerService,
			ILocationService locationService,
			IIncidentService incidentService, 
			IIncidentOutcomeService incidentOutcomeService)
			: base(repository, configService, logger, serialisationService)
		{
			this.officerService = officerService;
			this.locationService = locationService;
			this.incidentService = incidentService;
			this.outcomeService = incidentOutcomeService; 
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
		/// <returns>Result of the save operation</returns>
		/// <param name="serialisedOfficer">serialised officer object</param>
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
		/// <returns>The result of the update location operation</returns>
		/// <param name="ID">The ID of the Officer</param>
		/// <param name="serialisedLocation">Serialised location object</param>
		[HttpPost]
		public String updateLocation(String ID, String serialisedLocation)
		{
			Guid officerID;
			if (String.IsNullOrEmpty(ID) || String.IsNullOrEmpty(serialisedLocation) || !Guid.TryParse(ID, out officerID))
			{
				var model = this.logError("Officer Update Location: invalid parameters");
				return this.serialisationService.serialise(model);
			}

			Location location = null;
			if ((location = this.serialisationService.deserialise<Location>(serialisedLocation)) == null 
			    || !this.locationService.validate(location))
			{
				var model = this.logError("Officer: location failed to serialise or did not validate");
				return this.serialisationService.serialise(model);
			}

			Officer officer;
			if ((officer = this.officerService.Get(officerID)) == null)
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
		/// Saves an IncidentOutcome to an Incident
		/// POST: /SaveOutcome
		/// </summary>
		/// <returns>result of the outcome save operation</returns>
		/// <param name="serialisedOutcome">Serialised incident outcome.</param>
		[HttpPost]
		public String SaveOutcome(String serialisedOutcome)
		{
			if (String.IsNullOrEmpty(serialisedOutcome))
			{
				var model = this.logError("Officer Save Outcome: serialised outcome was null or empty");
				return this.serialisationService.serialise(model); 
			}

			IncidentOutcome incidentOutcome;
			if ((incidentOutcome = this.serialisationService.deserialise<IncidentOutcome>(serialisedOutcome)) == null)
			{
				var model = this.logError("Officer Save Outcome: Deserialisation Failed");
				return this.serialisationService.serialise(model); 
			}

			Incident incident;
			if ((incident = this.incidentService.Get(incidentOutcome.Incident.ID)) == null)
			{
				var model = this.logError("Officer Save Outcome: Incident Not Found");
				return this.serialisationService.serialise(model); 
			}

			if (!this.outcomeService.validate(incidentOutcome))
			{
				var model = this.logError("Officer Save Outcome: IncidentOutcome failed validation");
				return this.serialisationService.serialise(model);
			}

			if (incident.Outcome == null) incident.Outcome = new List<IncidentOutcome>(); 

			incident.Outcome.Add(incidentOutcome);
			this.incidentService.Update(incident);

			this.logger.info(String.Format("Saved Incident Outcome {0} to Incident {1}", incidentOutcome.ID.ToString(), incident.ID.ToString()));

			var responseResultModel = new ResponseResultModel()
			{
				Status = ResponseResultType.OK,
				Message = "Saved Incident Outcome"
			};

			return this.serialisationService.serialise<ResponseResultModel>(responseResultModel); 
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

			if (this.locationService != null)
			{
				this.locationService.Dispose(); 
			}

			if (this.outcomeService != null)
			{
				this.outcomeService.Dispose();
			}
		}
    }
}