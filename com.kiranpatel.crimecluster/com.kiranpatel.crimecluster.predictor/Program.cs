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
				clusterList.Add(new Cluster(i, clusters[i])); 
			}

			var transitionMatrix = generateTransitionMatrix(incidents, clusterList);

			Console.WriteLine($"Generated {clusters.Count} clusters"); 		
			printArray("Transition Matrix", transitionMatrix);
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
			var model = kernel.Get<IMarkovModel>();
			//MarkovModel model = new MarkovModel(CrimeType.AntiSocialBehaviour, kernel.Get<ILogger>()); 
			return model.generateTransitionMatrix(incidents, clusters); 
		}
			
		/// <summary>
		/// Prints the array.
		/// </summary>
		/// <param name="arr">Array.</param>
		private static void printArray(String title, double[,] arr)
		{
			Console.WriteLine(title); 
			int rowLength = arr.GetLength(0);
			int colLength = arr.GetLength(1);

			for (int i = 0; i < rowLength; i++)
			{
				Console.Write("[");
				for (int j = 0; j < colLength; j++)
				{
					Console.Write(string.Format("{0} ", Math.Round(arr[i, j], 3)));
				}
				Console.Write("]");
				Console.Write(Environment.NewLine + Environment.NewLine);
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
			_kernel.Bind<IMarkovModel>().To<MarkovModel>().WithConstructorArgument("type", CrimeType.AntiSocialBehaviour);			    

			return _kernel;
		}
	}
}