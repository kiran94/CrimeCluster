namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Contract for the Officer Service
	/// </summary>
	public interface IOfficerService : IEntityService<Officer>, IValidator<Officer>
	{
		/// <summary>
		/// Gets all Online officers populated into a dictionary relating ID to Locations
		/// </summary>
		/// <returns>The online officer locations.</returns>
		IDictionary<Guid, Location> GetOnlineOfficerLocations();

		/// <summary>
		/// Sets the officer online.
		/// </summary>
		/// <param name="officer">Officer.</param>
		void SetOfficerOnline(Officer officer); 

		/// <summary>
		/// Sets the officer offline.
		/// </summary>
		/// <param name="officer">Officer.</param>
		void SetOfficerOffline(Officer officer);

		/// <summary>
		/// Sets the officer busy.
		/// </summary>
		/// <param name="officer">Officer.</param>
		void SetOfficerBusy(Officer officer);
	}
}