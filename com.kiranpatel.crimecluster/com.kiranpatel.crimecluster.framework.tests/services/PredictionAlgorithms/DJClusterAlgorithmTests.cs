namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using System.Linq; 
	using NUnit.Framework; 
	using Moq; 

	/// <summary>
	/// Tests for the <see cref="DJClusterAlgorithm"/> class
	/// </summary>
	[TestFixture]
	public class DJClusterAlgorithmTests
	{
		/// <summary>
		/// The config service mock.
		/// </summary>
		private Mock<IConfigurationService> configService;

		/// <summary>
		/// The logger mock. 
		/// </summary>
		private Mock<ILogger> logger;

		/// <summary>
		/// The distance measure mock.
		/// </summary>
		private IDistanceMeasure distanceMeasure;

		/// <summary>
		/// Sets up.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			this.configService = new Mock<IConfigurationService>();
			this.logger = new Mock<ILogger>();
			this.distanceMeasure = new EuclideanDistance(); 
		}

		/// <summary>
		/// Ensures when the data set is null, an argument null exception is thrown
		/// </summary>
		[Test]
		public void Learn_NullDataSet_ArgumentNullException()
		{
			double[][] dataSet = null;

			Assert.Throws<ArgumentNullException>(delegate
			{
				this.GetInstance().Learn(dataSet);
			}); 
		}

		/// <summary>
		/// Ensures when the data points are populated, clusters are generated
		/// </summary>
		[Test]
		public void Learn_DataWithoutMerge_ClustersGenerated()
		{
			var dataSet = new double[][]
			{
				new double[] {8, 50},  // cluster 1
				new double[] {10, 52}, // cluster 1
				new double[] {40, 10}, // cluster 2
				new double[] {44, 15}  // cluster 2
			};

			this.configService.Setup(x => x.Get(ConfigurationKey.DJClusterRadiusEps, "10")).Returns("10");
			this.configService.Setup(x => x.Get(ConfigurationKey.DJClusterMinPts, "10")).Returns("2");

			var clusters = this.GetInstance().Learn(dataSet);

			Assert.AreEqual(2, clusters.Count);
			Assert.AreEqual(dataSet.Take(2).ToList(), clusters.First().ToList()); 
			Assert.AreEqual(dataSet.Skip(2).Take(2).ToList(), clusters.ElementAt(1).ToList());
		}

		/// <summary>
		/// Ensures when the data points are populated with a potential merge, they are merged
		/// </summary>
		[Test]
		public void Learn_DataWithMerge_ClustersGenerated()
		{
			var dataSet = new double[][]
			{
				new double[] {5, 50}, // cluster 1
				new double[] {8, 45}, // cluster 1
				new double[] {8, 35}, // cluster 1
				new double[] {48, 11},// cluster 2
				new double[] {51, 5}  // cluster 2
			};

			this.configService.Setup(x => x.Get(ConfigurationKey.DJClusterRadiusEps, "10")).Returns("10");
			this.configService.Setup(x => x.Get(ConfigurationKey.DJClusterMinPts, "10")).Returns("2");

			var clusters = this.GetInstance().Learn(dataSet);

			Assert.AreEqual(2, clusters.Count);
			Assert.AreEqual(dataSet.Take(3).ToList(), clusters.First().ToList());
			Assert.AreEqual(dataSet.Skip(3).Take(2).ToList(), clusters.ElementAt(1).ToList());
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private DJClusterAlgorithm GetInstance()
		{
			return new DJClusterAlgorithm(this.configService.Object, this.logger.Object, this.distanceMeasure); 
		}
	}
}