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
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main(string[] args)
		{
			var kernel = GenerateKernel();

			var logger = kernel.Get<ILogger>();
			logger.debug("Starting Predictor");

			var incidentService = kernel.Get<IIncidentService>();
			var djCluster = kernel.Get<IClusteringService>();

			// remove magic strings after proof
			var crimeTypeDict = new Dictionary<String, double>(6)
			{
				{ "Anti-social behaviour", 1},
				{ "Bicycle theft", 2},
				{ "Burglary", 3},
				{ "Criminal damage and arson", 4},
				{ "Drugs", 5},
				{ "Other crime", 6},
				{ "Other theft", 7},
				{ "Possession of weapons", 8},
				{ "Public order", 9},
				{ "Robbery", 10},
				{ "Shoplifting", 11},
				{ "Theft from the person", 12},
				{ "Vehicle crime", 13},
				{ "Violence and sexual offences", 14},
			};

			var dataSet = incidentService.getAll()
			                             .Select(x => new double[] { x.Location.Latitude.Value, x.Location.Longitude.Value, crimeTypeDict[x.CrimeType] })
			                             .ToArray();

			var clusters = djCluster.Learn(dataSet);

			// Average for each cluster for each attribute
			// 3 = Number of Attributes
			var emissionMatrix = new double[3, clusters.Count];

			int count = 0; 
			foreach (var currentCluster in clusters)
			{
				emissionMatrix[0, count] = currentCluster.Average(x => x[0]);
				emissionMatrix[1, count] = currentCluster.Average(x => x[1]);
				emissionMatrix[2, count] = currentCluster.Average(x => x[2]);

				Console.WriteLine(String.Format(
					"Cluster {0}: Lat: {1},\tLong: {2},\tType: {3}",
					++count,
					Math.Round(currentCluster.Average(x => x[0]), 4),
					Math.Round(currentCluster.Average(x => x[1]), 4),
					currentCluster.Average(x => x[2])));
			}

			Console.WriteLine(String.Empty); 
			Console.WriteLine("Emission Matrix"); 
			printArray(emissionMatrix); 
		}

		/// <summary>
		/// Generates the kernel with all the contract/concrete implementation bindings
		/// </summary>
		/// <returns>The kernel.</returns>
		private static IKernel GenerateKernel()
		{
			IKernel kernel = new StandardKernel();
			kernel.Bind<ILogger>().ToMethod(x => LoggerService.GetInstance());
			kernel.Bind<IConfigurationService>().To<ConfigurationService>();
			kernel.Bind<IFileIOService>().To<FileIOService>();

			kernel.Bind<ISession>()
				  .ToMethod(x => new MySQLConnection(kernel.Get<IConfigurationService>()).getSession())
				  .InThreadScope();

			kernel.Bind<IRepository>().To<Repository>();
			kernel.Bind<ICSVReaderService>().To<CSVReaderService>();
			kernel.Bind<ICSVParseStrategy>().To<IncidentCSVParseStrategy>();

			kernel.Bind<IOfficerService>().To<OfficerService>();
			kernel.Bind<ILocationService>().To<LocationService>();
			kernel.Bind<IIncidentGradingService>().To<IncidentGradingService>();
			kernel.Bind<IIncidentOutcomeService>().To<IncidentOutcomeService>();
			kernel.Bind<IIncidentBacklogService>().To<IncidentBacklogService>();
			kernel.Bind<IIncidentService>().To<IncidentService>();

			kernel.Bind<IDistanceMeasure>().To<EuclideanDistance>();
			kernel.Bind<IClusteringService>().To<DJClusterAlgorithm>(); 

			return kernel;
		}

		/// <summary>
		/// Prints the array.
		/// </summary>
		/// <param name="array">Array.</param>
		private static void printArray(double[,] array)
		{
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					Console.Write(Math.Round(array[i, j],4) + "\t"); 
				}

				Console.WriteLine(""); 
			}

		}
	}
}