namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq; 
	using System.Collections.Generic;

	/// <summary>
	/// Service for operations on the Officer Entity
	/// </summary>
	public class OfficerService : EntityService<Officer>, IOfficerService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.OfficerService"/> class.
		/// </summary>
		/// <param name="repository">Repository.</param>
		/// <param name="logger">Logger.</param>
		public OfficerService(IRepository repository, ILogger logger) 
			: base(repository, logger)
		{
			
		}

		// <inheritdoc>
		public IDictionary<Guid, Location> GetOnlineOfficerLocations()
		{
			this.logger.debug("Getting all online officer locations"); 
			return this.repository.Query<Officer>()
				       .Where(o => o.Status == StatusType.Online)
				       .ToDictionary(o => o.ID, o => o.Location); 
		}

		// <inheritdoc>
		public void SetOfficerOnline(Officer officer)
		{
			if (officer == null)
			{
				return; 
			}

			officer.Status = StatusType.Online;
			this.repository.Update(officer); 
		}

		// <inheritdoc>
		public void SetOfficerOffline(Officer officer)
		{
			if (officer == null)
			{
				return;
			}

			officer.Status = StatusType.Offline;
			this.repository.Update(officer);
		}

		// <inheritdoc>
		public void SetOfficerBusy(Officer officer)
		{
			if (officer == null)
			{
				return;
			}

			officer.Status = StatusType.Busy;
			this.repository.Update(officer);
		}
	}
}