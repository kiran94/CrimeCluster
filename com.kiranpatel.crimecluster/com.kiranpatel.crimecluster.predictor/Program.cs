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

			ILogger logger = kernel.Get<ILogger>();
			logger.debug("Starting Predictor");

			IIncidentService incidentService = kernel.Get<IIncidentService>();

			DateTime start = new DateTime(2015, 08, 01);
			DateTime end = new DateTime(2015, 09, 01); 

			ICollection<Incident> incidents = new HashSet<Incident>(incidentService.getAll()
			                                                        .Where(o => o.DateCreated >= start 
			                                                               && o.DateCreated <= end 
			                                                               && o.CrimeType == "Anti-social behaviour"));

			IPredictionService predictionService = kernel.Get<IPredictionService>();
			predictionService.predict(incidents); 
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

			kernel.Bind<IPredictionService>().To<KMeansAlgorithm>(); 

			return kernel;
		}
	}
}