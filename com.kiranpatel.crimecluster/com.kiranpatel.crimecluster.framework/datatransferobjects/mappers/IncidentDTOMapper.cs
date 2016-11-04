namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq; 
	using System.Text;

	/// <summary>
	/// Incident Data Transfer Object Mapper
	/// </summary>
	public class IncidentDTOMapper : IDataTransferService<Incident, IncidentDTO>
	{
		public IncidentDTOMapper()
		{
		}

		// <inheritdoc>
		public IncidentDTO toDTO(Incident domainEntity)
		{
			var dto = new IncidentDTO()
			{
				Summary = domainEntity.Summary,
				DateCreated = domainEntity.DateCreated.ToLongTimeString(),
				LocationID = (domainEntity.Location != null) ? domainEntity.Location.ID.ToString() : null,
				IncidentGrading = (domainEntity.Grading != null) ? domainEntity.Grading.ID.ToString() : null 
			};

			if (domainEntity.Outcome != null || domainEntity.Outcome.Count != 0)
			{
				// not likely to be more than couple of dozen so not worth overhead of string builder
				dto.OutcomeIDs = String.Join(",", domainEntity.Outcome.Select(x => x.ID));
			}

			return dto; 
		}
	}
}
