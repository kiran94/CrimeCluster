namespace com.kiranpatel.crimecluster.importer
{
	using System;
	using System.IO;
	using System.Linq;
	using com.kiranpatel.crimecluster.framework;
	using com.kiranpatel.crimecluster.dataaccess;
	using Ninject;
	using NHibernate;
	using System.Collections.Generic;

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
			logger.info("Importer Started.");

			var configService = kernel.Get<IConfigurationService>();
			var importFiles = GetImportFiles(configService.Get(ConfigurationKey.ImportLocation, "/tmp/CrimeCluster/"));

			int count = 0;
			using (var csvService = kernel.Get<ICSVReaderService>())
			using (var incidentService = kernel.Get<IIncidentService>())
			{
				foreach (String currentFile in importFiles)
				{
					logger.info(String.Format("Importing {0}", currentFile));
					var importedIncidents = csvService.parseCSV<Incident>(currentFile, CSVParseType.IncidentParse, true);

					foreach (Incident currentIncident in importedIncidents)
					{
						if (incidentService.validate(currentIncident))
						{
							if (++count % 100 == 0)
							{
								logger.info("Imported 100 Incidents.");
							}

							incidentService.Save(currentIncident);
						}
					}
				}
			}

			logger.info(String.Format("Imported {0} in total.", count));
			logger.info("Importer Completed.");
		}

		/// <summary>
		/// Gets all the import files recursivly from all files in the import location
		/// </summary>
		/// <returns>The import files.</returns>
		/// <param name="DirectoryLocation">Directory location.</param>
		private static String[] GetImportFiles(String DirectoryLocation)
		{
			IList<String> files = new List<String>(); 
			foreach (String currentDirectory in Directory.GetDirectories(DirectoryLocation))
			{
				foreach (String currentFile in Directory.GetFiles(currentDirectory, "*.csv"))
				{
					files.Add(currentFile); 
				}
			}

			return files.ToArray(); 
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