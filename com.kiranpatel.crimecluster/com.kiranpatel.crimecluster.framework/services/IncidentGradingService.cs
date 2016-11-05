namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq;
	/// <summary>
	/// Service for operations on the Incident Grading Service
	/// </summary>
	public class IncidentGradingService : EntityService<IncidentGrading>, IIncidentGradingService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.IncidentGradingService"/> class.
		/// </summary>
		/// <param name="repository">Repository.</param>
		/// <param name="logger">Logger.</param>
		public IncidentGradingService(
			IRepository repository, 
			ILogger logger) 
			: base(repository, logger)
		{
		}

		// <inheritdoc>
		public IncidentGrading GetImportIncidentGrading()
		{
			return this.repository.Query<IncidentGrading>().SingleOrDefault(x => !x.GradeValue.HasValue); 
		}

	}
}
