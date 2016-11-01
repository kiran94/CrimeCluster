namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq; 
	using Moq;
	using NUnit.Framework;

	/// <summary>
	/// Tests for the Location Service
	/// </summary>
	[TestFixture]
	public class LocationServiceTests
	{
		/// <summary>
		/// Mock of the repository.
		/// </summary>
		private Mock<IRepository> repository;

		/// <summary>
		/// Mock of the logger.
		/// </summary>
		private Mock<ILogger> logger;

		/// <summary>
		/// Sets up.
		/// </summary>
		[SetUp]
		public void setUp()
		{
			this.repository = new Mock<IRepository>();
			this.logger = new Mock<ILogger>(); 
		}

		/// <summary>
		/// Ensures when location 1 is null, null is returned
		/// </summary>
		[Test]
		public void calculateDifference_NullLocation1_Null()
		{
			Location loc1 = null;
			Location loc2 = new Location();

			var result = this.GetInstance().calculateDifference(loc1, loc2);

			Assert.IsNull(result); 
		}

		/// <summary>
		/// Ensures when the location 2 is null, null is returned
		/// </summary>
		[Test]
		public void calculateDifference_NullLocation2_Null()
		{
			Location loc1 = new Location();
			Location loc2 = null;

			var result = this.GetInstance().calculateDifference(loc1, loc2);

			Assert.IsNull(result);
		}

		/// <summary>
		/// Ensures when both locations are given, the difference is returned within a given tolerance (5%)
		/// </summary>
		[Test]
		public void calculateDifference_BothLocationsGiven_DifferenceReturned()
		{
			Location loc1 = new Location() { Latitude = 36.12, Longitude = -86.67 }; 
			Location loc2 = new Location() { Latitude = 33.94, Longitude = -118.4 };

			var result = this.GetInstance().calculateDifference(loc1, loc2);
			    
			Assert.That(result, Is.EqualTo(2888D).Within(5D).Percent);
		}

		/// <summary>
		/// Ensures when the passed target location is null, null is returned
		/// </summary>
		[Test]
		public void findClosest_NullTargetLocation_Null()
		{
			Location target = null;
			ICollection<Location> locations = new List<Location>() { new Location() };

			Assert.IsNull(GetInstance().findClosest(target, locations)); 
		}

		/// <summary>
		/// Ensures when the passed location list is null, null is returned 
		/// </summary>
		[Test]
		public void findClosest_NullList_Null()
		{
			Location target = new Location();
			ICollection<Location> locations = null; 

			Assert.IsNull(GetInstance().findClosest(target, locations));
		}

		/// <summary>
		/// Ensures when the list is empty, null is returned 
		/// </summary>
		[Test]
		public void findClosest_EmptyList_Null()
		{
			Location target = new Location();
			ICollection<Location> locations = new List<Location>();

			Assert.IsNull(GetInstance().findClosest(target, locations));
		}

		/// <summary>
		/// Ensures when the populated list and target is passed, the closest location is returned
		/// </summary>
		[Test]
		public void findClosest_PopulatedListAndTarget_ClosestReturned()
		{
			Location target = new Location()
			{
				Latitude = 51.532988,
				Longitude = -0.472509
			};

			ICollection<Location> locations = new List<Location>();
			locations.Add(new Location() { Latitude = 51.539635, Longitude = -0.459366 });
			locations.Add(new Location() { Latitude = 51.545994, Longitude = -0.492386 });
			locations.Add(new Location() { Latitude = 51.562684, Longitude = -0.445093 });

			var result = this.GetInstance().findClosest(target, locations);

			Assert.AreEqual(locations.First().ID.ToString(), result.ID.ToString());  
		}

		/// <summary>
		/// Ensures when the latitude is null, false is returned
		/// </summary>
		[Test]
		public void validate_NullLatitude_False()
		{
			Location location = new Location()
			{
				Latitude = null,
				Longitude = 1D,
				DateLogged = DateTime.Now
			};

			var result = this.GetInstance().validate(location);

			Assert.IsFalse(result); 
		}

		/// <summary>
		/// ENsures when the longitude is null, false is returned
		/// </summary>
		[Test]
		public void validate_NullLongitude_False()
		{
			Location location = new Location()
			{
				Latitude = 1D,
				Longitude = null,
				DateLogged = DateTime.Now
			};

			var result = this.GetInstance().validate(location);

			Assert.IsFalse(result);
		}

		/// <summary>
		/// Ensures when the date logged is null, false is returned
		/// </summary>
		[Test]
		public void validate_NullDateLogged_False()
		{
			Location location = new Location()
			{
				Latitude = 2D,
				Longitude = 1D,
				DateLogged = null
			};

			var result = this.GetInstance().validate(location);

			Assert.IsFalse(result);
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private ILocationService GetInstance()
		{
			return new LocationService(repository.Object, logger.Object); 
		}
	}
}