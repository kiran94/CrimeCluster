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
		/// Heap data structure (Prority Queue) 
		/// </summary>
		private static IHeap<Incident> heap;

		/// <summary>
		/// The logger instances
		/// </summary>
		private readonly ILogger logger;

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.IncidentBacklogService"/> class.
		/// </summary>
		/// <param name="passedHeap">passed Heap</param>
		/// <param name="logger">Logger.</param>
		public IncidentBacklogService(IHeap<Incident> passedHeap, ILogger logger)
		{
			heap = passedHeap;
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
			heap.add(incident);
		}

		// <inheritdoc>
		public Incident next()
		{
			this.logger.info("Getting next incident from the backlog");
			return heap.getRoot();
		}

		// <inheritdoc>
		public void remove(Incident incident)
		{
			throw new NotSupportedException();
		}

		// <inheritdoc>
		public int backlogSize()
		{
			return heap.getSize();
		}
	}
}