namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic; 
	/// <summary>
	/// Contract for the Incident Service
	/// </summary>
	public interface IIncidentService : IEntityService<Incident>
	{
		/// <summary>
		/// Allocates the new incident.
		/// </summary>
		/// <returns><c>true</c>, if new incident was allocated, <c>false</c> otherwise.</returns>
		/// <param name="incident">Incident.</param>
		bool allocateNewIncident(Incident incident);

		/// <summary>
		/// Gets all incidents
		/// </summary>
		/// <returns>The all.</returns>
		ICollection<Incident> getAll(); 
	}
}