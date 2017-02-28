namespace com.kiranpatel.crimecluster.webservice.tests
{
	using System;
	using System.Linq;
	using com.kiranpatel.crimecluster.framework;
	using com.kiranpatel.crimecluster.webservice.Controllers;
	using NUnit.Framework;
	using Moq;
	using System.Web.Mvc;
	using System.Collections.Generic;
	using com.kiranpatel.crimecluster.webservice;

	/// <summary>
	/// Cluster controller tests.
	/// </summary>
	[TestFixture]
	public class ClusterControllerTests
	{
		/// <summary>
		/// The repository instance
		/// </summary>
		private Mock<IRepository> repository;

		/// <summary>
		/// The config service instance
		/// </summary>
		private Mock<IConfigurationService> configService;

		/// <summary>
		/// The logger instance
		/// </summary>
		private Mock<ILogger> logger;

		/// <summary>
		/// The serialisation service instance
		/// </summary>
		private Mock<ISerialisationService> serialisationService;

		/// <summary>
		/// The incident service.
		/// </summary>
		private Mock<IIncidentService> incidentService;

		/// <summary>
		/// The clustering service.
		/// </summary>
		private Mock<IClusteringService> clusteringService; 

		/// <summary>
		/// Sets up.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			this.repository = new Mock<IRepository>();
			this.configService = new Mock<IConfigurationService>();
			this.logger = new Mock<ILogger>();
			this.serialisationService = new Mock<ISerialisationService>();
			this.incidentService = new Mock<IIncidentService>();
			this.clusteringService = new Mock<IClusteringService>();

			this.configService.Setup(x => x.Get(ConfigurationKey.GoogleMapsKey, It.IsAny<String>())).Returns("key"); 
		}

		/// <summary>
		/// Ensures when the passed parameter is null or empty, null is returned
		/// </summary>
		/// <param name="param">Parameter.</param>
		[Test]
		[TestCase("")]
		[TestCase(null)]
		public void Filter_NullOrEmptyString_NullReturned(String param)
		{
			JsonResult result = this.GetInstance().Filter(param);
			Assert.IsNull(result.Data); 
		}

		/// <summary>
		/// Ensures when an invalid crime type is passed (words not numbers) the result is null
		/// </summary>
		[Test]
		public void Filter_LetterPassed_NullReturned()
		{
			JsonResult result = this.GetInstance().Filter("invalid crime type");
			Assert.IsNull(result.Data);
		}

		/// <summary>
		/// Ensures when the crime type does not exist, null is returned
		/// </summary>
		[Test]
		public void Filter_CrimeTypeDoesNotExist_NullReturned()
		{
			JsonResult result = this.GetInstance().Filter("10000");
			Assert.IsNull(result.Data);
		}


		/// <summary>
		/// Ensures when a valid crime type is returned, the serialised output is returned
		/// </summary>
		[Test]
		public void Filter_AntiSocialBehaviour_AntiSocialReturned()
		{
			var incidents = new List<Incident>()
			{
				new Incident() { CrimeType = CrimeType.AntiSocialBehaviour.ToString(), Location = new Location() },
				new Incident() { CrimeType = CrimeType.AntiSocialBehaviour.ToString(), Location = new Location() }
			};

			String serialsied = "serialised";
			this.incidentService.Setup(x => x.getAllForCrimeType(CrimeType.AntiSocialBehaviour)).Returns(incidents.AsQueryable());
			this.serialisationService.Setup(x => x.serialise<List<LocationModel>>(It.IsAny<List<LocationModel>>())).Returns(serialsied);

			var result = this.GetInstance().Filter("1");

			StringAssert.AreEqualIgnoringCase(serialsied, (String)result.Data); 
		}

		/// <summary>
		/// Ensures when the crime type passed is null or empty, null is returned in the data 
		/// </summary>
		[Test]
		[TestCase("")]
		[TestCase(null)]
		public void Cluster_NullOrEmptyCrimeType_ReturnNull(String param)
		{
			var result = this.GetInstance().Cluster(param);
			Assert.IsNull(result.Data); 
		}

		/// <summary>
		/// Ensures when there are no incidents under the crime type, null is returned in the data
		/// </summary>
		[Test]
		public void Cluster_NoIncidentsUnderCrimeType_ReturnNull()
		{
			this.incidentService.Setup(x => x.getAllForCrimeType(CrimeType.AntiSocialBehaviour)).Returns(new List<Incident>().AsQueryable()); 
			var result = this.GetInstance().Cluster("1");
			Assert.IsNull(result.Data);
		}

		/// <summary>
		/// Ensures when there are incidents found in the crime type, the cluster output is returned
		/// </summary>
		[Test]
		public void Cluster_IncidentsFoundInCrimeType_ClustersReturned()
		{
			var incidents = new List<Incident>()
			{
				new Incident() { CrimeType = CrimeType.AntiSocialBehaviour.ToString(), Location = new Location() { Latitude = 1D, Longitude = 1D } },
				new Incident() { CrimeType = CrimeType.AntiSocialBehaviour.ToString(), Location = new Location() { Latitude = 1D, Longitude = 1D } }
			};

			var clusters = new List<HashSet<double[]>>() { new HashSet<double[]>() { new double[] { 1D, 1D } }};
			var centroids = new List<double[]>() { new double[] { 1D, 1D }};

			this.incidentService.Setup(x => x.getAllForCrimeType(CrimeType.AntiSocialBehaviour)).Returns(incidents.AsQueryable());
			this.clusteringService.Setup(x => x.Learn(It.IsAny<double[][]>())).Returns(clusters);
			this.clusteringService.Setup(x => x.CalculateCentroids(clusters)).Returns(centroids); 
			this.serialisationService.Setup(x => x.serialise<List<LocationModel>>(It.IsAny<List<LocationModel>>())).Returns("serialised");

			var result = this.GetInstance().Cluster("1");
			StringAssert.AreEqualIgnoringCase("serialised", result.Data.ToString()); 
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private ClusterController GetInstance()
		{
			return new ClusterController(
				this.repository.Object, 
				this.configService.Object, 
				this.logger.Object, 
				this.serialisationService.Object, 
				this.incidentService.Object,
				this.clusteringService.Object); 
		}
	}
}
