namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq; 
	using System.Collections.Generic;

	/// <summary>
	/// Service for operations on the Incident Service
	/// </summary>
	public class IncidentService : EntityService<Incident>, IIncidentService
	{
		/// <summary>
		/// The officer service instance. 
		/// </summary>
		private readonly IOfficerService officerService;

		/// <summary>
		/// The outcome service instance. 
		/// </summary>
		private readonly IIncidentOutcomeService outcomeService;

		/// <summary>
		/// The backlog service instance. 
		/// </summary>
		private readonly IIncidentBacklogService backlogService;

		/// <summary>
		/// The location service instance. 
		/// </summary>
		private readonly ILocationService locationService; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.IncidentService"/> class.
		/// </summary>
		/// <param name="officerService">Officer service.</param>
		/// <param name="outcomeService">Outcome service.</param>
		/// <param name="backlogService">Backlog service.</param>
		/// <param name="locationService">Location service.</param>
		/// <param name="repository">Repository.</param>
		/// <param name="logger">Logger.</param>
		public IncidentService(
			IOfficerService officerService, 
			IIncidentOutcomeService outcomeService, 
			IIncidentBacklogService backlogService, 
			ILocationService locationService, 
			IRepository repository, 
			ILogger logger) 
			: base(repository, logger)
		{
			this.officerService = officerService;
			this.outcomeService = outcomeService;
			this.backlogService = backlogService;
			this.locationService = locationService; 
		}

		// <inheritdoc>
		public bool allocateNewIncident(Incident incident)
		{
			if (incident == null)
			{
				this.logger.debug("null incident passed into allocate new incident");
				return false; 
			}

			var onlineOfficers = this.officerService.GetOnlineOfficerLocations();

			if (onlineOfficers == null || onlineOfficers.Count == 0)
			{
				this.logger.debug(String.Format("No Officers currently online, adding incident {0} to backlog", incident.ID.ToString()));
				this.backlogService.add(incident);
				return false; 
			}

			/// This is very infficent, maybe change to something a bit better
			/// also add more logging
			Location closest = this.locationService.findClosest(incident.Location, onlineOfficers.Select(o => o.Value).ToList());
			Guid officerID = onlineOfficers.SingleOrDefault(x => x.Value.ID == closest.ID).Key;
			Officer chooseOfficer = this.officerService.Get(officerID);

			IncidentOutcome outcome = new IncidentOutcome()
			{
				Officer = chooseOfficer,
				Incident = incident,
				DateCreated = DateTime.Now
			};

			// may have to flush here or just let the incident cascade from the outcome
			this.Save(incident); 
			this.outcomeService.Save(outcome);
			this.officerService.SetOfficerBusy(chooseOfficer); 

			return true; 
		}

		// <inheritdoc>
		public IEnumerable<Incident> getAll()
		{
			return this.repository.Query<Incident>().AsEnumerable(); 
		}

		// <inheritdoc>
		public IQueryable<Incident> getAllForCrimeType(CrimeType type)
		{
			return this.repository.Query<Incident>()
				       .Where(x => x.CrimeType.Equals(type.GetDescription()))
				       .OrderBy(x => x.DateCreated)
				       .AsQueryable(); 
		}

		// <inheritdoc>
		public bool validate(Incident toValidate)
		{
			return !(toValidate.Grading == null || toValidate.Location == null);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="T:com.kiranpatel.crimecluster.framework.IncidentService"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.IncidentService"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.IncidentService"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.IncidentService"/> so the garbage collector can reclaim the
		/// memory that the <see cref="T:com.kiranpatel.crimecluster.framework.IncidentService"/> was occupying.</remarks>
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
				if (this.officerService != null)
				{
					this.officerService.Dispose(); 
				}

				if (this.outcomeService != null)
				{
					this.outcomeService.Dispose(); 
				}

				if (this.locationService != null)
				{
					this.locationService.Dispose(); 
				}
			}
		}
	}
}