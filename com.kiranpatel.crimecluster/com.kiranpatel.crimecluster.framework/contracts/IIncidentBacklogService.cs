
namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Contract for the Incident Backlog Service
	/// </summary>
	public interface IIncidentBacklogService
	{
		/// <summary>
		/// Adds an Incident to the backlog
		/// </summary>
		/// <param name="incident">Incident.</param>
		void add(Incident incident);

		/// <summary>
		/// Removes an Incident from the backlog
		/// </summary>
		/// <param name="incident">Incident.</param>
		void remove(Incident incident);

		/// <summary>
		/// Gets the minimum (highest prority) from the backlog
		/// </summary>
		Incident next();

		/// <summary>
		/// Gets the size of the backlog
		/// </summary>
		/// <returns>The size.</returns>
		int backlogSize(); 
	}
}