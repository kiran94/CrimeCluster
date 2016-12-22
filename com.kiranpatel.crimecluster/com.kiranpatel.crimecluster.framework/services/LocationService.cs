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
		/// The distance measure.
		/// </summary>
		private readonly IDistanceMeasure distanceMeasure; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.LocationService"/> class.
		/// </summary>
		/// <param name="repository">Repository.</param>
		/// <param name="logger">Logger.</param>
		public LocationService(
			IRepository repository, 
			ILogger logger,
			IDistanceMeasure distanceMeasure) 
			: base(repository, logger)
		{
			this.distanceMeasure = distanceMeasure; 
		}

		// <inheritdoc>
		public Double? calculateDifference(Location location1, Location location2)
		{
			if (location1 == null || location2 == null)
			{
				this.logger.debug("Location 1 or 2 was null in caculate");
				return null;
			}

			double[] set1 = { location1.Latitude.Value, location1.Longitude.Value };
			double[] set2 = { location2.Latitude.Value, location2.Longitude.Value };

			return this.distanceMeasure.measure(set1, set2); 
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
	}
}
