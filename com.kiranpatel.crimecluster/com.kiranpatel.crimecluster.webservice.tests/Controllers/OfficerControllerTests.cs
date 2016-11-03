namespace com.kiranpatel.crimecluster.webservice.tests
{
	using System;
	using System.Linq; 
	using NUnit.Framework;
	using Moq;
	using com.kiranpatel.crimecluster.framework;
	using com.kiranpatel.crimecluster.webservice.Controllers;

	/// <summary>
	/// Officer controller tests.
	/// </summary>
	[TestFixture]
	public class OfficerControllerTests
	{
		/// <summary>
		/// The repository mock
		/// </summary>
		private Mock<IRepository> repository;

		/// <summary>
		/// The config service mock
		/// </summary>
		private Mock<IConfigurationService> configService;

		/// <summary>
		/// The logger service mock
		/// </summary>
		private Mock<ILogger> loggerService;

		/// <summary>
		/// The serialisation service mock
		/// </summary>
		private Mock<ISerialisationService> serialisationService;

		/// <summary>
		/// The officer service mock
		/// </summary>
		private Mock<IOfficerService> officerService;

		/// <summary>
		/// The location service mock
		/// </summary>
		private Mock<ILocationService> locationService;

		/// <summary>
		/// The incident service mock
		/// </summary>
		private Mock<IIncidentService> incidentService;

		/// <summary>
		/// The outcome service mock
		/// </summary>
		private Mock<IIncidentOutcomeService> outcomeService; 

		/// <summary>
		/// Sets up.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			this.repository = new Mock<IRepository>();
			this.configService = new Mock<IConfigurationService>();
			this.loggerService = new Mock<ILogger>();
			this.serialisationService = new Mock<ISerialisationService>();
			this.officerService = new Mock<IOfficerService>();
			this.locationService = new Mock<ILocationService>();
			this.incidentService = new Mock<IIncidentService>();
			this.outcomeService = new Mock<IIncidentOutcomeService>();
		}

		/// <summary>
		/// Ensures when the string passed is null or empty, an error is returned
		/// </summary>
		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void Get_NullOrEmpty_ErrorReturned(String param)
		{
			this.serialisationService
			    .Setup(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: ID was null or Empty")))
			    .Verifiable(); 
			
			this.GetInstance().Get(param);

			this.serialisationService
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: ID was null or Empty")), Times.Once);
		}

		/// <summary>
		/// Ensures when the string passed is an invalid guid, an error is returned
		/// </summary>
		[Test]
		public void Get_InvalidGuid_ErrorReturned()
		{
			this.serialisationService
				.Setup(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Failed to parse Guid")))
				.Verifiable();

			this.GetInstance().Get("INVALID");

			this.serialisationService
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Failed to parse Guid")), Times.Once);
		}

		/// <summary>
		/// Ensures when the passed officer cannot be found, an error is returned
		/// </summary>
		[Test]
		public void Get_OfficerNotFound_ErrorReturned()
		{
			Guid ID = Guid.NewGuid();

			this.officerService.Setup(x => x.Get(ID)).Returns((Officer) null);
			this.serialisationService
				.Setup(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Officer not found")))
				.Verifiable();

			this.GetInstance().Get(ID.ToString());

			this.serialisationService
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Officer not found")), Times.Once);
		}

		/// <summary>
		/// Ensures when the officer is found, the response result is retrieved
		/// </summary>
		[Test]
		public void Get_OfficerFound_OfficerRetrieved()
		{
			Officer officer = new Officer();

			this.officerService.Setup(x => x.Get(officer.ID)).Returns(officer);
			this.serialisationService.Setup(x => x.serialise(officer)).Returns("serialise"); 
			this.serialisationService
			    .Setup(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer Retrieved" && y.Response == "serialise")))
				.Verifiable();
			
			this.GetInstance().Get(officer.ID.ToString()); 

			this.serialisationService
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer Retrieved" && y.Response == "serialise")), Times.Once);

		}

		/// <summary>
		/// Ensures when the passed officer is null or empty, an error is returned
		/// </summary>
		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void Save_NullOrEmptyOfficer_ErrorReturned(String param)
		{
			this.serialisationService
			.Setup(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Officer null or empty")))
			.Verifiable();

			this.GetInstance().Save(param); 

			this.serialisationService
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Officer null or empty")), Times.Once);
		}

		/// <summary>
		/// Ensures when the deserialising of the officer object fails, an error is returned
		/// </summary>
		[Test]
		public void Save_FailedDeserialise_ErrorReturned()
		{
			String fakeOfficer = "INVALID"; 

			this.serialisationService.Setup(x => x.deserialise<Officer>(fakeOfficer)).Returns((Officer)null); 

			this.serialisationService
			.Setup(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Officer is wrong format")))
			.Verifiable();

			this.GetInstance().Save(fakeOfficer); 

			this.serialisationService
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Officer is wrong format")), Times.Once);
		}

		/// <summary>
		/// Ensures when the officer deserialises but fails validation, an error is returned
		/// </summary>
		[Test]
		public void Save_OfficerDeserialisesFailsValidation_ErrorReturned()
		{
			String serialisedOfficer = "mock"; 
			Officer officer = new Officer();

			this.serialisationService.Setup(x => x.deserialise<Officer>(serialisedOfficer)).Returns(officer);

			this.serialisationService
			.Setup(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Validation Failed")))
			.Verifiable();

			this.officerService.Setup(x => x.validate(officer)).Returns(false);

			this.GetInstance().Save(serialisedOfficer);

			this.serialisationService
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Validation Failed")), Times.Once);
		}

		/// <summary>
		/// Ensures when the officer deserialises and succeeds validation, the officer is saved
		/// </summary>
		[Test]
		public void Save_OfficerDeserialisesSucceedsValidation_OfficerSaved()
		{
			String serialisedOfficer = "mock";
			Officer officer = new Officer();

			this.serialisationService.Setup(x => x.deserialise<Officer>(serialisedOfficer)).Returns(officer);

			this.serialisationService
			.Setup(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Saved")))
			.Verifiable();

			this.officerService.Setup(x => x.validate(officer)).Returns(true);
			this.officerService.Setup(x => x.Save(officer)).Verifiable(); 

			this.GetInstance().Save(serialisedOfficer);

			this.serialisationService
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Saved")), Times.Once);

			this.officerService.Verify(x => x.Save(officer), Times.Once); 
		}

		/// <summary>
		/// Ensures when the ID is passed as empty or null, an error is returned
		/// </summary>
		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void updateLocation_EmptyOrNullID_ErrorReturned(String param)
		{
			this.serialisationService
			    .Setup(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer Update Location: invalid parameters")))
			    .Verifiable(); 

			this.GetInstance().updateLocation(param, "seralised");

			this.serialisationService.Verify(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer Update Location: invalid parameters")), Times.Once);
		}

		/// <summary>
		/// Ensures when the serialised location is empty or null, an error is returned
		/// </summary>
		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void updateLocation_EmptyOrNullSerialisedLocation_ErrorReturned(String param)
		{
			this.serialisationService
				.Setup(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer Update Location: invalid parameters")))
				.Verifiable();

			this.GetInstance().updateLocation("ID", param);

			this.serialisationService.Verify(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer Update Location: invalid parameters")), Times.Once);
		}

		/// <summary>
		/// Ensures when the ID string is passed with an incorrect guid format an error is returned
		/// </summary>
		[Test]
		public void updateLocation_IncorrectGuidFormat_ErrorReturned()
		{
			this.serialisationService
				.Setup(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer Update Location: invalid parameters")))
				.Verifiable();

			this.GetInstance().updateLocation("INVALIDID", "serialisedLocation");

			this.serialisationService.Verify(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer Update Location: invalid parameters")), Times.Once);
		}

		/// <summary>
		/// Ensures when the deserialisation of the location string results in null, an error is returned
		/// </summary>
		[Test]
		public void updateLocation_LocationDeserialisationReturnedNull_ErrorReturned()
		{
			String serialisedLocation = "serialisedLocation"; 

			this.serialisationService
				.Setup(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer: location failed to serialise or did not validate")))
				.Verifiable();

			this.serialisationService.Setup(x => x.deserialise<Location>(serialisedLocation)).Returns((Location)null); 
			    
			this.GetInstance().updateLocation("f99f6f3c-7e26-431c-a8b2-8c5bd6f34aa8", serialisedLocation);

			this.serialisationService.Verify(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer: location failed to serialise or did not validate")), Times.Once);
		}

		/// <summary>
		/// Ensures when the deserialised location fails validation checks, an error is returned
		/// </summary>
		[Test]
		public void updateLocation_DeserialisedLocationFailsValidation_ErrorReturned()
		{
			String serialisedLocation = "serialisedLocation";

			this.serialisationService
				.Setup(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer: location failed to serialise or did not validate")))
				.Verifiable();

			Location location = new Location();
			this.serialisationService.Setup(x => x.deserialise<Location>(serialisedLocation)).Returns(location);
			this.locationService.Setup(x => x.validate(location)).Returns(false); 
			                                                                                          

			this.GetInstance().updateLocation("f99f6f3c-7e26-431c-a8b2-8c5bd6f34aa8", serialisedLocation);

			this.serialisationService.Verify(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer: location failed to serialise or did not validate")), Times.Once);
		}

		/// <summary>
		/// Ensures when an officer is not found from the ID, an error is returned
		/// </summary>
		[Test]
		public void updateLocation_OfficerNotFound_ErrorReturned()
		{
			String id = "f99f6f3c-7e26-431c-a8b2-8c5bd6f34aa8"; 
			String serialisedLocation = "serialisedLocation";

			this.serialisationService
				.Setup(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer: officer could not be found")))
				.Verifiable();

			Location location = new Location();
			this.serialisationService.Setup(x => x.deserialise<Location>(serialisedLocation)).Returns(location);
			this.locationService.Setup(x => x.validate(location)).Returns(true);
			this.officerService.Setup(x => x.Get(Guid.Parse(id))).Returns((Officer)null); 

			this.GetInstance().updateLocation(id, serialisedLocation);

			this.serialisationService.Verify(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer: officer could not be found")), Times.Once);
		}

		/// <summary>
		/// Ensures in the normal case that the location is updated to the officer
		/// </summary>
		[Test]
		public void updateLocation_NormalCase_LocationUpdatedToOfficer()
		{
			String id = "f99f6f3c-7e26-431c-a8b2-8c5bd6f34aa8";
			String serialisedLocation = "serialisedLocation";

			this.serialisationService
				.Setup(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer: Location Updated")))
				.Verifiable();

			Location location = new Location();
			this.serialisationService.Setup(x => x.deserialise<Location>(serialisedLocation)).Returns(location);
			this.locationService.Setup(x => x.validate(location)).Returns(true);

			Officer officer = new Officer(); 
			this.officerService.Setup(x => x.Get(Guid.Parse(id))).Returns(officer);
			this.officerService.Setup(x => x.Update(It.Is<Officer>(y => y.ID == officer.ID && y.Location == location))).Verifiable(); 

			this.GetInstance().updateLocation(id, serialisedLocation);

			this.serialisationService.Verify(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer: Location Updated")), Times.Once);
			this.officerService.Verify((x => x.Update(It.Is<Officer>(y => y.ID == officer.ID && y.Location == location))), Times.Once); 
		}

		/// <summary>
		/// Ensures when the string is empty or null, an error is returned
		/// </summary>
		[Test]
		[TestCase("")]
		[TestCase(null)]
		public void SaveOutcome_NullOrEmptySerialisedOutcome_ErrorReturned(String param)
		{
			this.serialisationService
				.Setup(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer Save Outcome: serialised outcome was null or empty")))
				.Verifiable();

			this.GetInstance().SaveOutcome(param);

			this.serialisationService
				.Verify(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer Save Outcome: serialised outcome was null or empty")), Times.Once);                                              
		}

		/// <summary>
		/// Ensures when the IncidentOutcome fails deserialistion, an error is returned
		/// </summary>
		[Test]
		public void SaveOutcome_OutcomeFailsDeserialisation_ErrorReturned()
		{
			String serialisedObject = "serialised";
			this.serialisationService.Setup(x => x.deserialise<IncidentOutcome>(serialisedObject)).Returns((IncidentOutcome)null);
			this.serialisationService.Setup(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer Save Outcome: Deserialisation Failed"))).Verifiable();

			this.GetInstance().SaveOutcome(serialisedObject);

			this.serialisationService.Verify(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer Save Outcome: Deserialisation Failed")), Times.Once);
		}

		/// <summary>
		/// Ensures when the incident is not found, an error is returned
		/// </summary>
		[Test]
		public void SaveOutcome_IncidentNotFound_ErrorReturned()
		{
			String serialised = "serialisedIncidentOutcome"; 

			this.incidentService.Setup(x => x.Get(It.IsAny<Guid>())).Returns((Incident)null);
			this.serialisationService.Setup(x => x.deserialise<IncidentOutcome>(serialised)).Returns(new IncidentOutcome() { Incident = new Incident() }); 
			this.serialisationService
				.Setup(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer Save Outcome: Incident Not Found")))
				.Verifiable();

			this.GetInstance().SaveOutcome(serialised); 

			this.serialisationService
				.Verify(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Officer Save Outcome: Incident Not Found")), Times.Once);
		}

		/// <summary>
		/// Ensures in the normal case, the outcome is assigned to the incident an is saved
		/// </summary>
		[Test]
		public void SaveOutcome_NormalCase_OutcomeAssignedToIncidentAndSaved()
		{
			String incidentOutcome = "serialised";
			IncidentOutcome outcome = new IncidentOutcome() { Incident = new Incident() };

			this.serialisationService.Setup(x => x.deserialise<IncidentOutcome>(incidentOutcome)).Returns(outcome);
			this.incidentService.Setup(x => x.Get(outcome.Incident.ID)).Returns(outcome.Incident);
			this.incidentService.Setup(x => x.Update(It.Is<Incident>(y => y.Outcome.Any(k => k == outcome)))).Verifiable();
			this.outcomeService.Setup(x => x.validate(outcome)).Returns(true); 

			this.GetInstance().SaveOutcome(incidentOutcome);

			this.incidentService.Verify(x => x.Update(It.Is<Incident>(y => y.Outcome.Any(k => k == outcome))), Times.Once); 
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private OfficerController GetInstance()
		{
			return new OfficerController(
				this.repository.Object, 
				this.configService.Object, 
				this.loggerService.Object, 
				this.serialisationService.Object, 
				this.officerService.Object, 
				this.locationService.Object,
				this.incidentService.Object,
				this.outcomeService.Object); 
		}
	}
}