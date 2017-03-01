namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using System.Linq;
	using NUnit.Framework;
	using Moq;
	using System.Collections.Generic;

	/// <summary>
	/// Mixed Markov Model tests.
	/// </summary>
	[TestFixture] 
	public class MixedMarkovModelTests
	{
		/// <summary>
		/// mock clustering service.
		/// </summary>
		private Mock<IClusteringService> clusteringService;

		/// <summary>
		/// mock incident service.
		/// </summary>
		private Mock<IIncidentService> incidentService;

		/// <summary>
		/// mock logger.
		/// </summary>
		private Mock<ILogger> logger;

		/// <summary>
		/// Sets up.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			this.clusteringService = new Mock<IClusteringService>();
			this.incidentService = new Mock<IIncidentService>();
			this.logger = new Mock<ILogger>();
		}

		/// <summary>
		/// Ensures when the incident passed is null, nothing is added. 
		/// </summary>
		[Test]
		public void AddIncident_NullIncident_NothingAdded()
		{
			this.clusteringService.Setup(x => x.Learn(It.IsAny<double[][]>())).Verifiable();
			this.incidentService.Setup(x => x.Save(It.IsAny<Incident>())).Verifiable();

			Incident incident = null;
			this.GetInstance().AddIncident(incident); 

			this.clusteringService.Verify(x => x.Learn(It.IsAny<double[][]>()), Times.Never);
			this.incidentService.Verify(x => x.Save(It.IsAny<Incident>()), Times.Never); 
		}

		/// <summary>
		/// Ensures when the incident added is not null, the incident is added and the markov model regenereted.
		/// </summary>
		[Test]
		public void AddIncident_IncidentNotNull_AddedAndMarkovModelRegenerated()
		{
			Incident incident = new Incident()
			{
				CrimeType = CrimeType.Burglary.ToString(),
				Location = new Location() { Latitude = 1, Longitude = 2 }
			};

			List<HashSet<double[]>> clusters = new List<HashSet<double[]>>();
			var coord = new double[] { incident.Location.Latitude.Value, incident.Location.Longitude.Value }; 
			clusters.Add(new HashSet<double[]>() { coord }); 

			this.clusteringService.Setup(x => x.Learn(It.IsAny<double[][]>())).Returns(clusters).Verifiable();
			this.incidentService.Setup(x => x.Save(It.IsAny<Incident>())).Verifiable();
			this.incidentService.Setup(x => x.Flush()).Verifiable(); 

			var incidentList = new List<Incident>() { incident };
			this.incidentService.Setup(x => x.getAllForCrimeType(CrimeType.Burglary)).Returns(incidentList.AsQueryable()); 
						
			this.GetInstance().AddIncident(incident);

			this.clusteringService.Verify(x => x.Learn(It.IsAny<double[][]>()), Times.Once);
			this.incidentService.Verify(x => x.Save(incident), Times.Once);
			this.incidentService.Verify(x => x.Flush(), Times.Once); 
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private MixedMarkovModel GetInstance()
		{
			return new MixedMarkovModel(this.clusteringService.Object, this.incidentService.Object, this.logger.Object); 
		}
	}
}
