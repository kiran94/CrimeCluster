namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using System.Linq;
	using System.Collections.Generic; 
	using NUnit.Framework;
	using Moq; 

	/// <summary>
	/// Markov model tests.
	/// </summary>
	[TestFixture]
	public class MarkovModelTests
	{
		/// <summary>
		/// The logger.
		/// </summary>
		Mock<ILogger> logger;

		/// <summary>
		/// The precision of comparing double values. 
		/// </summary>
		const double PRECISION = 0.005;

		/// <summary>
		/// Sets up.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			this.logger = new Mock<ILogger>(); 
		}

		/// <summary>
		/// Ensures when generating transition matrix and incident is passed as null, error is logged.
		/// </summary>
		[Test]
		public void generateTransitionMatrix_NullIncident_ThrowException()
		{
			this.logger.Setup(x => x.debug("incidents should not be null or empty for AntiSocialBehaviour")).Verifiable(); 
			
			ICollection<Incident> incidents = null;
			List<Cluster> clusters = new List<Cluster>();

			var result = this.GetInstance().generateTransitionMatrix(incidents, clusters);

			CollectionAssert.IsEmpty(result); 
			this.logger.VerifyAll(); 
		}

		/// <summary>
		/// Ensures when the clusters passed is null, error is logged. 
		/// </summary>
		[Test]
		public void generateTransitionMatrix_NullClusters_ErrorLogged()
		{
			this.logger.Setup(x => x.debug("clusters should not be null or empty for AntiSocialBehaviour")).Verifiable();

			ICollection<Incident> incidents = new List<Incident>() { new Incident() };
			List<Cluster> clusters = null;

			var result = this.GetInstance().generateTransitionMatrix(incidents, clusters);

			CollectionAssert.IsEmpty(result);
			this.logger.VerifyAll();
		}

		/// <summary>
		/// Ensures when clusters with incidents are generated, a transition matrix is generated. 
		/// </summary>
		[Test]
		public void generateTransitionMatrix_ClustersWithIncidents_TransitionMatrix()
		{
			var rawCluster1 = new HashSet<double[]>();
			var rawCluster2 = new HashSet<double[]>();
		
			var incidents = new List<Incident>();

			for (int i = 1; i <= 100; i++)
			{
				var actualIncident = new Incident() { Location = new Location() { Latitude = i, Longitude = 100 % i } };
				var inputIncident = new double[] { actualIncident.Location.Latitude.Value, actualIncident.Location.Longitude.Value };
				incidents.Add(actualIncident);

				if (i % 2 == 1)
				{
					rawCluster1.Add(inputIncident);
				}
				else
				{
					rawCluster2.Add(inputIncident);
				}
			}

			for (int i = 0; i < 50; i++)
			{
				var actualIncident = new Incident() { Location = new Location() { Latitude = i % 100, Longitude = 1 } };
				var inputIncident = new double[] { actualIncident.Location.Latitude.Value, actualIncident.Location.Longitude.Value };
				incidents.Add(actualIncident);
				rawCluster1.Add(inputIncident);
			}

			for (int i = 0; i < 10; i++)
			{
				var actualIncident = new Incident() { Location = new Location() { Latitude = i % 50, Longitude = 1 } };
				var inputIncident = new double[] { actualIncident.Location.Latitude.Value, actualIncident.Location.Longitude.Value };
				incidents.Add(actualIncident);
				rawCluster2.Add(inputIncident);
			}

			var clusters = new List<Cluster>() 
			{ 
				new Cluster(0, rawCluster1), 
				new Cluster(1, rawCluster2)			
			};

			var result = this.GetInstance().generateTransitionMatrix(incidents, clusters);

			Assert.That(result[0, 0], Is.EqualTo(0.363636).Within(PRECISION));
			Assert.That(result[0, 1], Is.EqualTo(0.636363).Within(PRECISION));
			Assert.That(result[1, 0], Is.EqualTo(0.971813).Within(PRECISION));
			Assert.That(result[1, 1], Is.EqualTo(0.028169).Within(PRECISION));
		}

		/// <summary>
		/// Ensures when no points are found in any clusters then an invalid operation exception is found. 
		/// </summary>
		[Test]
		public void generateTransitionMatrix_NoClusterTransitionsFound_InvalidOperationException()
		{
			this.logger.Setup(x => x.error("No Points were found in Clusters", It.IsAny<InvalidOperationException>())).Verifiable(); 

			var rawCluster1 = new HashSet<double[]>();
			var rawCluster2 = new HashSet<double[]>();

			var incidents = new List<Incident>();

			for (int i = 1; i <= 100; i++)
			{
				var actualIncident = new Incident() { Location = new Location() { Latitude = i, Longitude = 100 % i } };
				var inputIncident = new double[] { actualIncident.Location.Latitude.Value, actualIncident.Location.Longitude.Value };

				if (i % 2 == 1)
				{
					rawCluster1.Add(inputIncident);
				}
				else
				{
					rawCluster2.Add(inputIncident);
				}
			}

			for (int i = 1; i <= 10; i++)
			{
				incidents.Add(new Incident() { Location = new Location() { Latitude = 100 % i, Longitude = i } }); 					
			}

			var clusters = new List<Cluster>();
			clusters.Add(new Cluster(0, rawCluster1));
			clusters.Add(new Cluster(1, rawCluster2));

			var result = this.GetInstance().generateTransitionMatrix(incidents, clusters);
			Assert.IsNull(result); 

			this.logger.Verify(x => x.error("No Points were found in Clusters", It.IsAny<InvalidOperationException>()), Times.Once); 
		}

		/// <summary>
		/// Ensures when the model is generated, and the predict method is called, the max transition from the current state is choosen and the next state returned. 
		/// </summary>
		[Test]
		public void predict_ModelGenerated_MaxTransitionChosenAndNextStateChosen()
		{
			var rawCluster1 = new HashSet<double[]>();
			var rawCluster2 = new HashSet<double[]>();

			var incidents = new List<Incident>();

			for (int i = 1; i <= 100; i++)
			{
				var actualIncident = new Incident() { Location = new Location() { Latitude = i, Longitude = 100 % i } };
				var inputIncident = new double[] { actualIncident.Location.Latitude.Value, actualIncident.Location.Longitude.Value };
				incidents.Add(actualIncident);

				if (i % 2 == 1)
				{
					rawCluster1.Add(inputIncident);
				}
				else
				{
					rawCluster2.Add(inputIncident);
				}
			}

			for (int i = 0; i < 50; i++)
			{
				var actualIncident = new Incident() { Location = new Location() { Latitude = i % 100, Longitude = 1 } };
				var inputIncident = new double[] { actualIncident.Location.Latitude.Value, actualIncident.Location.Longitude.Value };
				incidents.Add(actualIncident);
				rawCluster1.Add(inputIncident);
			}

			for (int i = 0; i < 10; i++)
			{
				var actualIncident = new Incident() { Location = new Location() { Latitude = i % 50, Longitude = 1 } };
				var inputIncident = new double[] { actualIncident.Location.Latitude.Value, actualIncident.Location.Longitude.Value };
				incidents.Add(actualIncident);
				rawCluster2.Add(inputIncident);
			}

			var clusters = new List<Cluster>()
			{
				new Cluster(0, rawCluster1),
				new Cluster(1, rawCluster2)
			};

			var model = this.GetInstance();
			model.generateTransitionMatrix(incidents, clusters);
			int nextState = model.predict();

			Assert.AreEqual(1, nextState); 
		}

		/// <summary>
		/// Ensures when the model is not generated and predict is called, an invalid operation exception is thrown. 
		/// </summary>
		[Test]
		public void predict_ModelNotGenerated_InvalidOperationException()
		{
			this.logger.Setup(x => x.debug("Called predict when model was not generated for AntiSocialBehaviour")).Verifiable(); 
			var model = this.GetInstance(); 

			var result = model.predict();

			Assert.AreEqual(0, result);
			this.logger.VerifyAll(); 
		}

		/// <summary>
		/// Ensures when the current state is set, the average point is returned. 
		/// </summary>
		[Test]
		public void getPredictionPoint_CurrentState1_AveragePoint()
		{
			var rawCluster1 = new HashSet<double[]>();
			var rawCluster2 = new HashSet<double[]>();

			var incidents = new List<Incident>();

			for (int i = 1; i <= 100; i++)
			{
				var actualIncident = new Incident() { Location = new Location() { Latitude = i, Longitude = 100 % i } };
				var inputIncident = new double[] { actualIncident.Location.Latitude.Value, actualIncident.Location.Longitude.Value };
				incidents.Add(actualIncident);

				if (i % 2 == 1)
				{
					rawCluster1.Add(inputIncident);
				}
				else
				{
					rawCluster2.Add(inputIncident);
				}
			}

			for (int i = 0; i < 50; i++)
			{
				var actualIncident = new Incident() { Location = new Location() { Latitude = i % 100, Longitude = 1 } };
				var inputIncident = new double[] { actualIncident.Location.Latitude.Value, actualIncident.Location.Longitude.Value };
				incidents.Add(actualIncident);
				rawCluster1.Add(inputIncident);
			}

			for (int i = 0; i < 10; i++)
			{
				var actualIncident = new Incident() { Location = new Location() { Latitude = i % 50, Longitude = 1 } };
				var inputIncident = new double[] { actualIncident.Location.Latitude.Value, actualIncident.Location.Longitude.Value };
				incidents.Add(actualIncident);
				rawCluster2.Add(inputIncident);
			}

			var clusters = new List<Cluster>()
			{
				new Cluster(0, rawCluster1),
				new Cluster(1, rawCluster2)
			};

			var model = this.GetInstance();
			model.generateTransitionMatrix(incidents, clusters);

			var state = model.predict();
			var predictionPoint = model.getPredictionPoint();

			Assert.AreEqual(1, state);
			Assert.That(predictionPoint[0], Is.EqualTo(43.6134D).Within(PRECISION));
			Assert.That(predictionPoint[1], Is.EqualTo(14.2857D).Within(PRECISION));
		}

		/// <summary>
		/// Ensures when the model has not been generated and the prediction point has been called, a invalid operation exception is called. 
		/// </summary>
		[Test]
		public void getPredictionPoint_ModelNotGenerated_InvalidOperationException()
		{
			this.logger.Setup(x => x.debug("Called get prediction point when model was not generated for AntiSocialBehaviour")).Verifiable();
			
			var model = this.GetInstance(); 
			var result = model.getPredictionPoint();

			Assert.IsNull(result);
			this.logger.VerifyAll(); 
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