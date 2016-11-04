namespace com.kiranpatel.crimecluster.webservice.tests
{
	using System;
	using NUnit.Framework;
	using Moq;
	using com.kiranpatel.crimecluster.framework;
	using com.kiranpatel.crimecluster.webservice.Controllers;

	/// <summary>
	/// Tests for the <see cref="IncidentController"/> class
	/// </summary>
	public class IncidentControllerTests
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
		/// The incident service mock
		/// </summary>
		private Mock<IIncidentService> incidentService;

		/// <summary>
		/// The backlog service mock
		/// </summary>
		private Mock<IIncidentBacklogService> backlogService;

		/// <summary>
		/// The dto service mock.
		/// </summary>
		private Mock<IDataTransferService<Incident, IncidentDTO>> dtoService;

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
			this.incidentService = new Mock<IIncidentService>();
			this.backlogService = new Mock<IIncidentBacklogService>();
			this.dtoService = new Mock<IDataTransferService<Incident, IncidentDTO>>(); 
		}

		/// <summary>
		/// Ensures when the serialised incident is null or empty, an error is returned
		/// </summary>
		[Test]
		[TestCase("")]
		[TestCase(null)]
		public void Save_NullOrEmptySerialised_ErrorReturned(String param)
		{
			this.serialisationService
			    .Setup(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Serialised Incident was null or empty")))
			    .Verifiable();

			this.GetInstance().Save(param);

			this.serialisationService
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Serialised Incident was null or empty")), Times.Once);
		}

		/// <summary>
		/// Ensures when the deserialisation of the incident fails, an error is returned
		/// </summary>
		[Test]
		public void Save_FailedDeseriaisation_ErrorReturned()
		{
			String serialisedIncident = "serialised"; 
			this.serialisationService.Setup(x => x.deserialise<Incident>(serialisedIncident)).Returns((Incident) null);
			this.serialisationService.Setup(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Serialised Incident failed to deserialise"))).Verifiable();

			this.GetInstance().Save(serialisedIncident);

			this.serialisationService.Verify(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Serialised Incident failed to deserialise")), Times.Once);
		}

		/// <summary>
		/// Ensures when the incident failed validation, an error is returned
		/// </summary>
		[Test]
		public void Save_IncidentFailedValidation_ErrorReturned()
		{
			String serialisedIncident = "serialised";
			Incident incident = new Incident() { Grading = new IncidentGrading(), Location = new Location() };
			this.serialisationService.Setup(x => x.deserialise<Incident>(serialisedIncident)).Returns(incident);
			this.serialisationService.Setup(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Incident failed to validation"))).Verifiable();
			this.incidentService.Setup(x => x.validate(incident)).Returns(false); 

			this.GetInstance().Save(serialisedIncident);

			this.serialisationService.Verify(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Incident failed to validation")), Times.Once);
		}

		/// <summary>
		/// Ensures when the incident is valid, it is saved and added to the backlog
		/// </summary>
		[Test]
		public void Save_SuccessValidation_NormalCase_Saved_AddedToBacklog()
		{
			var serialisedIncident = "serialised";
			var incident = new Incident() { Grading = new IncidentGrading(), Location = new Location() };

			this.serialisationService.Setup(x => x.deserialise<Incident>(serialisedIncident)).Returns(incident);
			this.serialisationService.Setup(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Incident Added"))).Verifiable();

			this.incidentService.Setup(x => x.validate(incident)).Returns(true);
			this.incidentService.Setup(x => x.Save(incident)).Verifiable();
			this.backlogService.Setup(x => x.add(incident)).Verifiable(); 

			this.GetInstance().Save(serialisedIncident);

			this.incidentService.Verify(x => x.Save(incident), Times.Once);
			this.backlogService.Verify(x => x.add(incident), Times.Once); 
			this.serialisationService.Verify(x => x.serialise(It.Is<ResponseResultModel>(y => y.Message == "Incident Added")), Times.Once);
		}

		/// <summary>
		/// Ensures when the Incident ID is null or empty, an error is returned
		/// </summary>
		/// <param name="param">Parameter.</param>
		[Test]
		[TestCase("")]
		[TestCase(null)]
		public void Get_NullOrEmptyID_ErrorReturned(String param)
		{
			this.serialisationService
				.Setup(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Incident ID was invalid")))
				.Verifiable();
			
			this.GetInstance().Get(param); 

			this.serialisationService
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Incident ID was invalid")), Times.Once);
		}

		/// <summary>
		/// Ensures when the ID is not a valid GUID, an error is returned
		/// </summary>
		[Test]
		public void Get_InvalidID_ErrorReturned()
		{
			String ID = "INVALID"; 

			this.serialisationService
				.Setup(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Incident ID was invalid")))
				.Verifiable();

			this.GetInstance().Get(ID);

			this.serialisationService
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Incident ID was invalid")), Times.Once);
		}

		/// <summary>
		/// Ensures when the Incident associated with the passed ID does not exist, an error is returned
		/// </summary>
		[Test]
		public void Get_IncidentNotExist_ErrorReturned()
		{
			String ID = "a772a609-6380-4579-8d95-9a4febcc697b";

			this.incidentService.Setup(x => x.Get(Guid.Parse(ID))).Returns((Incident)null); 
			this.serialisationService
				.Setup(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Incident does not exist")))
				.Verifiable();

			this.GetInstance().Get(ID);

			this.serialisationService
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Incident does not exist")), Times.Once);
		}

		/// <summary>
		/// Ensures when the incident exists, the incident is returned
		/// </summary>
		[Test]
		public void Get_IncidentExists_IncidentReturned()
		{
			Incident incident = new Incident(); 

			this.incidentService.Setup(x => x.Get(incident.ID)).Returns(incident);
			this.serialisationService
			    .Setup(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Incident found" && y.Status == ResponseResultType.OK)))
				.Verifiable();

			this.GetInstance().Get(incident.ID.ToString());

			this.serialisationService
			    .Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Incident found" && y.Status == ResponseResultType.OK)), Times.Once);
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private IncidentController GetInstance()
		{
			return new IncidentController(
				this.repository.Object,
				this.configService.Object, 
				this.loggerService.Object, 
				this.serialisationService.Object, 
				this.incidentService.Object,
				this.backlogService.Object,
				this.dtoService.Object); 
		}
	}
}