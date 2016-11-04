namespace com.kiranpatel.crimecluster.webservice.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;

	using com.kiranpatel.crimecluster.framework; 

	/// <summary>
	/// Incident controller.
	/// </summary>
	public class IncidentController : BaseController
    {
		/// <summary>
		/// The incident service.
		/// </summary>
		private readonly IIncidentService incidentService;

		/// <summary>
		/// The incident backlog service.
		/// </summary>
		private readonly IIncidentBacklogService incidentBacklogService;

		/// <summary>
		/// Data Transfer Mapper
		/// </summary>
		private readonly IDataTransferService<Incident, IncidentDTO> mapper; 

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.IncidentController"/> class.
		/// </summary>
		/// <param name="repository">Repository.</param>
		/// <param name="configService">Config service.</param>
		/// <param name="logger">Logger.</param>
		/// <param name="serialisationService">Serialisation service.</param>
		/// <param name="incidentService">Incident service.</param>
		/// <param name="incidentBacklogService">Incident backlog service.</param>
		/// <param name="mapper">Data Transfer Mapper</param>
		public IncidentController(
			IRepository repository,
			IConfigurationService configService,
			ILogger logger,
			ISerialisationService serialisationService, 
			IIncidentService incidentService, 
			IIncidentBacklogService incidentBacklogService,
			IDataTransferService<Incident, IncidentDTO> mapper)
			: base(repository, configService, logger, serialisationService)
		{
			this.incidentService = incidentService;
			this.incidentBacklogService = incidentBacklogService;
			this.mapper = mapper;
		}

		/// <summary>
		/// Save the specified serialisedIncident.
		/// POST: /Save
		/// </summary>
		/// <param name="serialisedIncident">Serialised incident.</param>
		[HttpPost]
		public String Save(String serialisedIncident)
		{
			if (String.IsNullOrEmpty(serialisedIncident))
			{
				var model = this.logError("Serialised Incident was null or empty");
				return this.serialisationService.serialise(model);  
			}

			Incident incident;
			if ((incident = this.serialisationService.deserialise<Incident>(serialisedIncident)) == null)
			{
				var model = this.logError("Serialised Incident failed to deserialise");
				return this.serialisationService.serialise(model); 
			}

			if (!this.incidentService.validate(incident))
			{
				var model = this.logError("Incident failed to validation");
				return this.serialisationService.serialise(model);
			}

			this.logger.info(String.Format("Adding Incident {0} to data store and backlog", incident.ID.ToString())); 
			this.incidentService.Save(incident);
			this.incidentBacklogService.add(incident);

			var responseModel = new ResponseResultModel()
			{
				Status = ResponseResultType.OK,
				Message = "Incident Added"
			};

			return this.serialisationService.serialise(responseModel); 	
		}

		/// <summary>
		/// Gets the Incident with the specified ID
		/// GET: /Get
		/// </summary>
		/// <param name="ID">Identifier.</param>
		[HttpGet]
		public String Get(String ID)
		{
			Guid incidentID; 
			if (String.IsNullOrEmpty(ID) || !Guid.TryParse(ID, out incidentID))
			{
				var model = this.logError("Incident ID was invalid");
				return this.serialisationService.serialise(model);
			}

			Incident incident;
			if ((incident = this.incidentService.Get(incidentID)) == null)
			{
				var model = this.logError("Incident does not exist");
				return this.serialisationService.serialise(model);
			}

			var resultModel = new ResponseResultModel()
			{
				Status = ResponseResultType.OK,
				Message = "Incident found",
				Response = this.serialisationService.serialise(this.mapper.toDTO(incident))
			};

			return this.serialisationService.serialise(resultModel);
		}

		/// <summary>
		/// Update the the passed incident
		/// POST: /Update
		/// </summary>
		/// <param name="serialisedIncident">Serialised incident.</param>
		[HttpPost]
		public String Update(String serialisedIncident)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Deletes the passed incident
		/// POST: /Delete
		/// </summary>
		/// <param name="serialisedIncident">Serialised incident.</param>
		[HttpPost]
		public String Delete(String serialisedIncident)
		{
			throw new NotImplementedException();
		}
    }
}