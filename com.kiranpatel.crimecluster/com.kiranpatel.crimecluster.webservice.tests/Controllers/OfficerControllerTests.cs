namespace com.kiranpatel.crimecluster.webservice.tests
{
	using System;
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
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: ID was null or Empty"))
			            , Times.Once);
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
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Failed to parse Guid"))
						, Times.Once);
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
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Officer not found"))
						, Times.Once);
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
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer Retrieved" && y.Response == "serialise"))
						, Times.Once);

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
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Officer null or empty"))
						, Times.Once);
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
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Officer is wrong format"))
						, Times.Once);
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

			this.officerService.Setup(x => x.Validate(officer)).Returns(false);

			this.GetInstance().Save(serialisedOfficer);

			this.serialisationService
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Validation Failed"))
						, Times.Once);
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

			this.officerService.Setup(x => x.Validate(officer)).Returns(true);
			this.officerService.Setup(x => x.Save(officer)).Verifiable(); 

			this.GetInstance().Save(serialisedOfficer);

			this.serialisationService
				.Verify(x => x.serialise<ResponseResultModel>(It.Is<ResponseResultModel>(y => y.Message == "Officer: Saved"))
						, Times.Once);

			this.officerService.Verify(x => x.Save(officer), Times.Once); 
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
				this.officerService.Object); 
		}
	}
}