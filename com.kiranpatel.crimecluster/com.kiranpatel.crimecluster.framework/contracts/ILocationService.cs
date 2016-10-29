
namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Contract for operations on the Location Service
	/// </summary>
	public interface ILocationService : IEntityService<Location>
	{
		/// <summary>
		/// Calculate the difference between location 1 and location 2 
		/// </summary>
		/// <returns>difference between locations, else null</returns>
		/// <param name="location1">Location1.</param>
		/// <param name="location2">Location2.</param>
		Double? calculateDifference(Location location1, Location location2);

		/// <summary>
		/// Finds the closest location in the list of locations to the target location
		/// </summary>
		/// <returns>The closest.</returns>
		/// <param name="targetLocation">Target location.</param>
		/// <param name="listOfLocations">List of locations.</param>
		Location findClosest(Location targetLocation, ICollection<Location> listOfLocations);
	}
}