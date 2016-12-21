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

			var dataSet = incidentService.getAllForCrimeType(CrimeType.AntiSocialBehaviour)
			                             .Select(x => new double[] { x.Location.Latitude.Value, x.Location.Longitude.Value })
			                             .ToArray();

			var clusters = generateClusters(dataSet);
			var emissionMatrix = generateEmissionMatrix(clusters);

			Console.WriteLine($"Generated {clusters.Count} clusters"); 
			printArray("Emission Matrix", emissionMatrix); 
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
		/// Generates the emission matrix.
		/// </summary>
		/// <returns>The emission matrix.</returns>
		/// <param name="clusters">Clusters.</param>
		private static double[,] generateEmissionMatrix(List<HashSet<double[]>> clusters)
		{
			var emissionMatrix = new double[2, clusters.Count];

			int count = 0;
			foreach (var currentCluster in clusters)
			{
				emissionMatrix[0, count] = Math.Round(currentCluster.Average(x => x[0]), 4);
				emissionMatrix[1, count] = Math.Round(currentCluster.Average(x => x[1]), 4);
				count++;
			}

			return emissionMatrix; 
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

			return _kernel;
		}
	}
}