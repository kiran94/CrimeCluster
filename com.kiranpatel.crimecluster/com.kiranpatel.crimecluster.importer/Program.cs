namespace com.kiranpatel.crimecluster.importer
{
	using System;
	using System.Linq;
	using com.kiranpatel.crimecluster.framework;
	using com.kiranpatel.crimecluster.dataaccess;
	using Ninject;
	using NHibernate;

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

			var configService = kernel.Get<IConfigurationService>();
			var logger = kernel.Get<ILogger>();
			logger.info("Importer Started.");

			using (var csvService = kernel.Get<ICSVReaderService>())
			using (var incidentService = kernel.Get<IIncidentService>())
			{
				var importLocation = configService.Get(ConfigurationKey.ImportLocation, "");
				var importedIncidents = csvService.parseCSV<Incident>(importLocation, CSVParseType.IncidentParse, true);
				int count = 0; 

				foreach (Incident currentIncident in importedIncidents)
				{
					if (incidentService.validate(currentIncident))
					{
						if (++count % 100 == 0)
						{
							logger.info("Imported 100 Incidents."); 
						}

						//incidentService.Save(currentIncident);
					}
				}

				logger.info(String.Format("Imported {0} in total.", count));
			}

			logger.info("Importer Completed."); 
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
			kernel.Bind<IIncidentOutcomeService>().To<IncidentOutcomeService>();
			kernel.Bind<IIncidentBacklogService>().To<IncidentBacklogService>();
			kernel.Bind<IIncidentService>().To<IncidentService>();

			return kernel;
		}
	}
}