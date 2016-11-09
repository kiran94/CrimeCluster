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
			IKernel kernel = GenerateKernel();

			ILogger logger = kernel.Get<ILogger>();
			logger.debug("Starting Predictor");

			IConfigurationService configService = kernel.Get<IConfigurationService>(); 
			IIncidentService incidentService = kernel.Get<IIncidentService>();
			IClusteringService<Incident> clusteringService = kernel.Get<IClusteringService<Incident>>(); 
			//IPredictionService predictionService = kernel.Get<IPredictionService>();

			String startStr = configService.Get(ConfigurationKey.StartCrimeSamplingDate, "01/08/2015");
			String endStr = configService.Get(ConfigurationKey.EndCrimeSamplingDate, "01/09/2015");
			String crimeType = CrimeType.AntiSocialBehaviour.GetDescription();

			DateTime start = Convert.ToDateTime(startStr); 
			DateTime end = Convert.ToDateTime(endStr); 

			IQueryable<Incident> query = incidentService.getAll().Where(o => o.DateCreated >= start && o.DateCreated <= end && o.CrimeType == crimeType);
			ICollection<Incident> incidents = new HashSet<Incident>(query);

			clusteringService.Learn(incidents, x => new double[] { x.Location.Latitude.Value, x.Location.Longitude.Value }); 
			//predictionService.predict(incidents);
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

			kernel.Bind<IClusteringService<Incident>>().To<KMeansAlgorithm<Incident>>(); 

			return kernel;
		}
	}
}