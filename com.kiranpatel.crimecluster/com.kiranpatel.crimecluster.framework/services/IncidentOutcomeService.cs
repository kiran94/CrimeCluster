namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Service for operations on the IncidentOutcome entity
	/// </summary>
	public class IncidentOutcomeService : EntityService<IncidentOutcome>, IIncidentOutcomeService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.IncidentOutcomeService"/> class.
		/// </summary>
		/// <param name="repository">Repository.</param>
		/// <param name="logger">Logger.</param>
		public IncidentOutcomeService(IRepository repository, ILogger logger) 
			: base(repository, logger)
		{
			
		}

		// <inheritdoc>
		public bool validate(IncidentOutcome toValidate)
		{
			if (toValidate == null)
			{
				return false; 
			}

			return !(String.IsNullOrEmpty(toValidate.Outcome)
				|| toValidate.Incident == null
				|| toValidate.Officer == null);
		}
	}
}