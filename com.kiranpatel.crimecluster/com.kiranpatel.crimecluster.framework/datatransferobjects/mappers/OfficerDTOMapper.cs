namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Officer Data Transfer Mapper
	/// </summary>
	public class OfficerDTOMapper : IDataTransferService<Officer, OfficerDTO>
	{
		// <inheritdoc>
		public OfficerDTO toDTO(Officer domainEntity)
		{
			var dto = new OfficerDTO();

			dto.ID = domainEntity.ID;
			dto.Name = String.Concat(domainEntity.Title, " ", domainEntity.FirstName, " ", domainEntity.LastName);
			dto.BadgeNumber = domainEntity.BadgeNumber;
			dto.LocationID = ((domainEntity.Location != null) ? domainEntity.Location.ID.ToString() : null);
			dto.IncidentID = ((domainEntity.Incident != null) ? domainEntity.Incident.ID.ToString() : null);

			return dto; 
		}
	}
}