namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using System.Linq;
	using System.Collections.Generic; 
	using NUnit.Framework;
	using Moq; 

	[TestFixture]
	public class MarkovModelTests
	{
		/// <summary>
		/// The logger.
		/// </summary>
		Mock<ILogger> logger;

		/// <summary>
		/// Sets up.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			this.logger = new Mock<ILogger>(); 
		}

		/// <summary>
		/// Ensures when generating transition matrix and incident is passed as null, an exception is thrown
		/// </summary>
		[Test]
		public void generateTransitionMatrix_NullIncident_ThrowException()
		{
			ICollection<Incident> incidents = null;
			List<Cluster> clusters = new List<Cluster>();

			Assert.Throws<InvalidOperationException>(delegate
			{
				this.GetInstance().generateTransitionMatrix(incidents, clusters);
			}); 
		}

		/// <summary>
		/// Ensures when the clusters passed is null, an exception is thrown.
		/// </summary>
		[Test]
		public void generateTransitionMatrix_NullClusters_ThrowException()
		{
			ICollection<Incident> incidents = new List<Incident>();
			List<Cluster> clusters = null;

			Assert.Throws<InvalidOperationException>(delegate
			{
				this.GetInstance().generateTransitionMatrix(incidents, clusters);
			});
		}

		/// <summary>
		/// Ensures when clusters with incidents are generated, a transition matrix is generated. 
		/// </summary>
		[Test]
		public void generateTransitionMatrix_ClustersWithIncidents_TransitionMatrix()
		{
			var rawCluster1 = new HashSet<double[]>();
			var rawCluster2 = new HashSet<double[]>();
			var rawCluster3 = new HashSet<double[]>(); 

			var incidents = new List<Incident>();

			int twos = 0;
			int threes = 0;
			int other = 0; 

			for (int i = 1; i <= 100; i++)
			{
				var incident = new Incident() { Location = new Location() { Latitude = i, Longitude = 100 % i } };
				incidents.Add(incident);

				if (i % 2 == 0)
				{
					rawCluster1.Add(new double[] { incident.Location.Latitude.Value, incident.Location.Longitude.Value });
					twos++; 
				}
				else if (i % 3 == 0)
				{
					rawCluster2.Add(new double[] { incident.Location.Latitude.Value, incident.Location.Longitude.Value });
					threes++;
				}
				else
				{
					rawCluster3.Add(new double[] { incident.Location.Latitude.Value, incident.Location.Longitude.Value });
					other++;
				}
			}

			var clusters = new List<Cluster>() 
			{ 
				new Cluster(0, rawCluster1), 
				new Cluster(1, rawCluster2), 
				new Cluster(2, rawCluster3) 
			};

			var result = this.GetInstance().generateTransitionMatrix(incidents, clusters); 








			throw new NotImplementedException(); 
		}

		/// <summary>
		/// Ensures when no points are found in any clusters then an invalid operation exception is found. 
		/// </summary>
		[Test]
		public void generateTransitionMatrix_NoClusterTransitionsFound_InvalidOperationException()
		{
			throw new NotImplementedException(); 
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private MarkovModel GetInstance()
		{
			return new MarkovModel(CrimeType.AntiSocialBehaviour, this.logger.Object); 
		}
	}
}
