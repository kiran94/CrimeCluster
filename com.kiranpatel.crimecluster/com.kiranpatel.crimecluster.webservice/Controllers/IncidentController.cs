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
		private readonly IncidentService incidentService;

		/// <summary>
		/// The incident backlog service.
		/// </summary>
		private readonly IncidentBacklogService incidentBacklogService; 

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
		public IncidentController(
			IRepository repository,
			IConfigurationService configService,
			ILogger logger,
			ISerialisationService serialisationService, 
			IncidentService incidentService, 
			IncidentBacklogService incidentBacklogService)
			: base(repository, configService, logger, serialisationService)
		{
			this.incidentService = incidentService;
			this.incidentBacklogService = incidentBacklogService; 
		}

		/// <summary>
		/// Save the specified serialisedIncident.
		/// POST: /Save
		/// </summary>
		/// <param name="serialisedIncident">Serialised incident.</param>
		[HttpPost]
		public String Save(String serialisedIncident)
		{
			throw new NotImplementedException(); 	
		}

		/// <summary>
		/// Gets the Incident with the specified ID
		/// GET: /Get
		/// </summary>
		/// <param name="ID">Identifier.</param>
		[HttpGet]
		public String Get(String ID)
		{
			throw new NotImplementedException();
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