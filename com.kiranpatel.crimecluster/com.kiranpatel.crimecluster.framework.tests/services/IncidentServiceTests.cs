namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using System.Linq;
	using NUnit.Framework;
	using Moq;
	using System.Collections.Generic;

	/// <summary>
	/// Tests for the Incident Service
	/// </summary>
	[TestFixture]
	public class IncidentServiceTests
	{
		/// <summary>
		/// The repository mock
		/// </summary>
		private Mock<IRepository> repository;

		/// <summary>
		/// The logger mock
		/// </summary>
		private Mock<ILogger> logger;

		/// <summary>
		/// The officer service mock
		/// </summary>
		private Mock<IOfficerService> officerService;

		/// <summary>
		/// The outcome service mock
		/// </summary>
		private Mock<IIncidentOutcomeService> outcomeService;

		/// <summary>
		/// The backlog service mock
		/// </summary>
		private Mock<IIncidentBacklogService> backlogService;

		/// <summary>
		/// The location service mock
		/// </summary>
		private Mock<ILocationService> locationService; 

		/// <summary>
		/// Sets up.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			this.repository = new Mock<IRepository>();
			this.logger = new Mock<ILogger>();
			this.officerService = new Mock<IOfficerService>();
			this.outcomeService = new Mock<IIncidentOutcomeService>();
			this.backlogService = new Mock<IIncidentBacklogService>();
			this.locationService = new Mock<ILocationService>(); 
		}

		/// <summary>
		/// Ensures when the incident passed is null, false is returned
		/// </summary>
		[Test]
		public void allocateNewIncident_NullIncident_False()
		{
			Incident incident = null;
			this.repository.Setup(x => x.Save(incident)).Verifiable(); 

			var result = this.GetInstance().allocateNewIncident(incident);

			Assert.False(result);
			this.repository.Verify(x => x.Save(incident), Times.Never); 
		}

		/// <summary>
		/// Ensures when no officers are online, the incident is logged and put on the backlog 
		/// </summary>
		[Test]
		public void allocateNewIncident_NoOfficersOnline_LoggedAndIncidentPutOnBackLog()
		{
			Incident incident = new Incident();
			this.repository.Setup(x => x.Save(incident)).Verifiable();
			this.officerService.Setup(x => x.GetOnlineOfficerLocations()).Returns(new Dictionary<Guid, Location>());
			this.backlogService.Setup(x => x.add(incident)).Verifiable(); 

			var result = this.GetInstance().allocateNewIncident(incident);

			Assert.False(result);
			this.repository.Verify(x => x.Save(incident), Times.Never);
			this.backlogService.Verify(x => x.add(incident), Times.Once); 
		}

		/// <summary>
		/// Ensures when an incident is added and officers are available, the incident is allocated
		/// </summary>
		[Test]
		public void allocateNewIncident_NormalCase_IncidentAllocated()
		{
			Incident incident = new Incident() { Location = new Location() };

			IList<Officer> officers = new List<Officer>();
			IList<Location> locations = new List<Location>();
			IDictionary<Guid, Location> officerLocations = new Dictionary<Guid, Location>();

			for (int i = 0; i < 10; i++)
			{
				Officer officer = new Officer();
				Location location = new Location();

				officerLocations.Add(officer.ID, location);
				officers.Add(officer);
				locations.Add(location); 
			}

			this.officerService.Setup(o => o.GetOnlineOfficerLocations()).Returns(officerLocations);
			this.officerService.Setup(o => o.Get(officers.First().ID)).Returns(officers.First());
			this.officerService.Setup(o => o.SetOfficerBusy(It.Is<Officer>(x => x.ID == officers.First().ID))).Verifiable();

			this.locationService.Setup(o => o.findClosest(incident.Location, officerLocations.Values)).Returns(locations.First());
			this.outcomeService.Setup(o => o.Save(It.Is<IncidentOutcome>(x => x.Incident == incident && x.Officer == officers.First()))).Verifiable();

			var service = this.GetInstance();
			var result = service.allocateNewIncident(incident);

			Assert.IsTrue(result);

			this.officerService.Verify(o => o.SetOfficerBusy(It.Is<Officer>(x => x.ID == officers.First().ID)), Times.Once);
			this.outcomeService.Verify(o => o.Save(It.Is<IncidentOutcome>(x => x.Incident == incident && x.Officer == officers.First())), Times.Once);
		}

		/// <summary>
		/// Ensures when no incidents exist, an empty list is returned
		/// </summary>
		[Test]
		public void getAll_NoIncidentsExist_EmptyList()
		{
			var incidents = new List<Incident>();
			this.repository.Setup(x => x.Query<Incident>()).Returns(incidents.AsQueryable());

			var result = this.GetInstance().getAll();

			CollectionAssert.IsEmpty(result); 	
		}

		/// <summary>
		/// Ensures when incidents exist, they are returned
		/// </summary>
		[Test]
		public void getAll_IncidentsExist_AllReturned()
		{
			var incidents = new List<Incident>();

			for (int i = 0; i < 10; i++)
			{
				incidents.Add(new Incident()); 
			}

			this.repository.Setup(x => x.Query<Incident>()).Returns(incidents.AsQueryable());

			var result = this.GetInstance().getAll();

			CollectionAssert.AreEqual(incidents, result);
		}

		/// <summary>
		/// Ensures when the crime type is set to AntiSocialBehavior then only those are returned. 
		/// </summary>
		[Test]
		public void getAllForCrimeType_AntiSocialBehavior_OnlyAntiSocialBehaviorFiltered()
		{
			var crimes = new List<Incident>()
			{
				new Incident() {CrimeType = CrimeType.AntiSocialBehaviour.ToString() },
				new Incident() {CrimeType = CrimeType.BicycleTheft.ToString() },
				new Incident() {CrimeType = CrimeType.AntiSocialBehaviour.ToString() },
				new Incident() {CrimeType = CrimeType.Burglary.ToString() }
			};

			this.repository.Setup(x => x.Query<Incident>()).Returns(crimes.AsQueryable());

			var result = this.GetInstance().getAllForCrimeType(CrimeType.AntiSocialBehaviour);

			Assert.AreEqual(2, result.Count);
			Assert.That(result.All(x => x.CrimeType == CrimeType.AntiSocialBehaviour.ToString()));  
		}

		/// <summary>
		/// Ensures when there are no matching records with the crime type, non are returned. 
		/// </summary>
		[Test]
		public void getAllForCrimeType_NoneMatching_NoneReturned()
		{
			var crimes = new List<Incident>()
			{
				new Incident() {CrimeType = CrimeType.BicycleTheft.ToString() },
				new Incident() {CrimeType = CrimeType.BicycleTheft.ToString() },
				new Incident() {CrimeType = CrimeType.BicycleTheft.ToString() },
				new Incident() {CrimeType = CrimeType.Burglary.ToString() }
			};

			this.repository.Setup(x => x.Query<Incident>()).Returns(crimes.AsQueryable());

			var result = this.GetInstance().getAllForCrimeType(CrimeType.AntiSocialBehaviour);

			Assert.AreEqual(0, result.Count);
		}

		/// <summary>
		/// Ensures when the incident is invalid, false is returned
		/// </summary>
		[Test]
		public void validate_InvalidIncident_False()
		{
			Incident incident = new Incident()
			{
				DateCreated = new DateTime(),
				Grading = null,
				Location = null
			};

			var result = this.GetInstance().validate(incident);
			Assert.False(result); 
		}

		/// <summary>
		/// Ensures when the incident is valid true is returned
		/// </summary>
		public void validate_ValidIncident_True()
		{
			Incident incident = new Incident()
			{
				DateCreated = new DateTime(),
				Grading = new IncidentGrading(),
				Location = new Location()
			};

			var result = this.GetInstance().validate(incident);
			Assert.True(result);
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private IIncidentService GetInstance()
		{
			return new IncidentService(
				this.officerService.Object, 
				this.outcomeService.Object, 
				this.backlogService.Object, 
				this.locationService.Object, 
				this.repository.Object, 
				this.logger.Object); 
		}

	}
}