﻿namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using com.kiranpatel.crimecluster.framework;

	/// <summary>
	/// Contract for the Incident Service
	/// </summary>
	public interface IIncidentService : IEntityService<Incident>, IValidator<Incident>
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
		IEnumerable<Incident> getAll();

		/// <summary>
		/// Gets all the incidents for a particular crime type
		/// </summary>
		/// <returns>A collection of incidents.</returns>
		/// <param name="type">crime type to filter by.</param>
		ICollection<Incident> getAllForCrimeType(CrimeType type); 
	}
}