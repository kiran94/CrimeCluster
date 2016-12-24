namespace com.kiranpatel.crimecluster.predictor
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using com.kiranpatel.crimecluster.dataaccess;
	using com.kiranpatel.crimecluster.framework;
	using NHibernate;
	using Ninject;

	/// <summary>
	/// Main class.
	/// </summary>
	public class MainClass
	{
		/// <summary>
		/// The kernel.
		/// </summary>
		private static IKernel kernel = GenerateKernel(); 

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main(string[] args)
		{
			var logger = kernel.Get<ILogger>();
			logger.debug("Starting Predictor");

			var incidentService = kernel.Get<IIncidentService>();

			var incidents = incidentService.getAllForCrimeType(CrimeType.AntiSocialBehaviour);
			var dataSet = incidents.Select(x => new double[] { x.Location.Latitude.Value, x.Location.Longitude.Value }).ToArray();

			var clusters = generateClusters(dataSet);

			var clusterList = new List<Cluster>();
			for (int i = 0; i < clusters.Count; i++)
			{
				clusterList.Add(new Cluster(i.ToString(), clusters[i])); 
			}

			var emissionMatrix = generateEmissionMatrix(clusters);
			var transitionMatrix = generateTransitionMatrix(incidents, clusterList);
			var initialDist = generateInitialDistribution(incidents, clusterList); 

			Console.WriteLine($"Generated {clusters.Count} clusters"); 
			printArray("Emission Matrix", emissionMatrix);
			printArray("Transition Matrix", transitionMatrix);
			printArray("Initial Distribution", initialDist); 
		}

		/// <summary>
		/// Generates the clusters.
		/// </summary>
		/// <returns>The clusters.</returns>
		/// <param name="dataset">Dataset.</param>
		private static List<HashSet<double[]>> generateClusters(double[][] dataset)
		{
			var djCluster = kernel.Get<IClusteringService>();
			return djCluster.Learn(dataset);
		}

		/// <summary>
		/// Generates the transition matrix.
		/// </summary>
		/// <returns>The transition matrix.</returns>
		/// <param name="incidents">Incidents.</param>
		private static double[,] generateTransitionMatrix(ICollection<Incident> incidents, List<Cluster> clusters)
		{
			var model = kernel.Get<IHiddenMarkovModel>();
			return model.generateTransitionMatrix(incidents, clusters); 
		}

		/// <summary>
		/// Generates the emission matrix.
		/// </summary>
		/// <returns>The emission matrix.</returns>
		/// <param name="clusters">Clusters.</param>
		private static double[,] generateEmissionMatrix(List<HashSet<double[]>> clusters)
		{
			var model = kernel.Get<IHiddenMarkovModel>();
			return model.generateEmissionMatrix(clusters); 
		}

		/// <summary>
		/// Generates the initial distribution.
		/// </summary>
		/// <returns>The initial distribution.</returns>
		/// <param name="incidents">Incidents.</param>
		/// <param name="clusters">Clusters.</param>
		private static double[] generateInitialDistribution(ICollection<Incident> incidents, List<Cluster> clusters)
		{
			var model = kernel.Get<IHiddenMarkovModel>();
			return model.generateInitialDistribution(incidents, clusters); 
		}

		/// <summary>
		/// Prints the array.
		/// </summary>
		/// <param name="array">Array.</param>
		private static void printArray(String title, double[,] array)
		{
			Console.WriteLine(title); 
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					Console.Write(array[i, j] + "\t"); 
				}

				Console.WriteLine(String.Empty); 
			}
		}

		/// <summary>
		/// Prints the array.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="array">Array.</param>
		private static void printArray(String title, double[] array)
		{
			Console.WriteLine(title);

			for (int i = 0; i < array.Length; i++)
			{
				Console.Write(array[i] + "\t"); 
			}

			Console.WriteLine(string.Empty); 
		}

		/// <summary>
		/// Generates the kernel with all the contract/concrete implementation bindings
		/// </summary>
		/// <returns>The kernel.</returns>
		private static IKernel GenerateKernel()
		{
			IKernel _kernel = new StandardKernel();
			_kernel.Bind<ILogger>().ToMethod(x => LoggerService.GetInstance());
			_kernel.Bind<IConfigurationService>().To<ConfigurationService>();
			_kernel.Bind<IFileIOService>().To<FileIOService>();

			_kernel.Bind<ISession>()
				  .ToMethod(x => new MySQLConnection(_kernel.Get<IConfigurationService>()).getSession())
				  .InThreadScope();

			_kernel.Bind<IRepository>().To<Repository>();
			_kernel.Bind<ICSVReaderService>().To<CSVReaderService>();
			_kernel.Bind<ICSVParseStrategy>().To<IncidentCSVParseStrategy>();

			_kernel.Bind<IOfficerService>().To<OfficerService>();
			_kernel.Bind<ILocationService>().To<LocationService>();
			_kernel.Bind<IIncidentGradingService>().To<IncidentGradingService>();
			_kernel.Bind<IIncidentOutcomeService>().To<IncidentOutcomeService>();
			_kernel.Bind<IIncidentBacklogService>().To<IncidentBacklogService>();
			_kernel.Bind<IIncidentService>().To<IncidentService>();

			_kernel.Bind<IDistanceMeasure>().To<EuclideanDistance>();
			_kernel.Bind<IClusteringService>().To<DJClusterAlgorithm>();
			_kernel.Bind<IHiddenMarkovModel>().To<HiddenMarkovModel>(); 

			return _kernel;
		}
	}
}