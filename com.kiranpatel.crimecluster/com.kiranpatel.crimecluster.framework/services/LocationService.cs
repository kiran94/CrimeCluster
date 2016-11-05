namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Linq; 
	using System.Collections.Generic;

	/// <summary>
	/// Service for the operations on the Location Entity
	/// </summary>
	public class LocationService : EntityService<Location>, ILocationService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.LocationService"/> class.
		/// </summary>
		/// <param name="repository">Repository.</param>
		/// <param name="logger">Logger.</param>
		public LocationService(IRepository repository, ILogger logger) 
			: base(repository, logger)
		{
			
		}

		// <inheritdoc>
		public Double? calculateDifference(Location location1, Location location2)
		{
			if (location1 == null || location2 == null)
			{
				this.logger.debug("Location 1 or 2 was null in caculate");
				return null;
			}

			return calculate(location1.Latitude.Value, location1.Longitude.Value, location2.Latitude.Value, location2.Longitude.Value); 
		}

		// <inheritdoc>
		public Location findClosest(Location targetLocation, ICollection<Location> listOfLocations)
		{
			if (targetLocation == null)
			{
				return null; 
			}
			 
			if (listOfLocations.IsNullOrEmpty())
			{
				return null; 
			}

			var locationsToDifferences = listOfLocations
				.ToDictionary(o => o, o => this.calculateDifference(targetLocation, o))
				.OrderBy(o => o.Value)
				.First();

			return locationsToDifferences.Key;
		}

		// <inheritdoc>
		public bool validate(Location location)
		{
			return !(!location.Latitude.HasValue || !location.Longitude.HasValue || location.DateLogged == null);
		}

		/// <summary>
		/// Calculate the difference between lat1, lon1, lat2 and lon2. https://rosettacode.org/wiki/Haversine_formula#C.23
		/// </summary>
		/// <param name="lat1">Lat1.</param>
		/// <param name="lon1">Lon1.</param>
		/// <param name="lat2">Lat2.</param>
		/// <param name="lon2">Lon2.</param>
		private static double calculate(double lat1, double lon1, double lat2, double lon2)
		{
			var R = 6372.8; // In kilometers
			var dLat = toRadians(lat2 - lat1);
			var dLon = toRadians(lon2 - lon1);
			lat1 = toRadians(lat1);
			lat2 = toRadians(lat2);

			var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
			return R * 2 * Math.Asin(Math.Sqrt(a));
		}

		/// <summary>
		/// To radians. https://rosettacode.org/wiki/Haversine_formula#C.23
		/// </summary>
		/// <returns>The radians.</returns>
		/// <param name="angle">Angle.</param>
		private static double toRadians(double angle)
		{
			return Math.PI * angle / 180.0;
		}
	}
}
