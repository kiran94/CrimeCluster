namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq;
	using System.Collections.Generic;

	/// <summary>
	/// Service backlogging incidents in terms of prority
	/// </summary>
	public class IncidentBacklogService : IIncidentBacklogService
	{
		/// <summary>
		/// The logger instances
		/// </summary>
		private readonly ILogger logger;

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.IncidentBacklogService"/> class.
		/// </summary> 
		/// <param name="logger">Logger.</param>
		public IncidentBacklogService(ILogger logger)
		{
			this.logger = logger;
		}

		// <inheritdoc>
		public void add(Incident incident)
		{
			if (incident == null)
			{
				return;
			}

			this.logger.info(String.Format("Adding Incident {0} to the backlog", incident.ID.ToString()));
			IncidentHeap.Heap.add(incident);
		}

		// <inheritdoc>
		public Incident next()
		{
			this.logger.info("Getting next incident from the backlog");
			return IncidentHeap.Heap.getRoot();
		}

		// <inheritdoc>
		public void remove(Incident incident)
		{
			throw new NotSupportedException();
		}

		// <inheritdoc>
		public int backlogSize()
		{
			return IncidentHeap.Heap.getSize();
		}
	}
}