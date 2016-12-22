namespace com.kiranpatel.crimecluster.importer
{
	using System;
	using System.IO;
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
			var incidentGrading = kernel.Get<IIncidentGradingService>();
			var csvParseStrategy = kernel.Get<ICSVParseStrategy>();

			var importFiles = GetImportFiles(configService.Get(ConfigurationKey.ImportLocation, "/data/CrimeCluster/"));
			setDefaultGrading(incidentGrading, csvParseStrategy); 

			int count = 0;
			int printThreshold = 50000; 
			using (var csvService = kernel.Get<ICSVReaderService>())
			using (var incidentService = kernel.Get<IIncidentService>())
			{
				foreach (String currentFile in importFiles)
				{
					logger.info($"Importing {currentFile}");
					var importedIncidents = csvService.parseCSV<Incident>(currentFile, CSVParseType.IncidentParse, csvParseStrategy, true);

					foreach (Incident currentIncident in importedIncidents)
					{
						if (incidentService.validate(currentIncident))
						{
							if (++count % printThreshold == 0)
							{
								logger.info($"Imported {printThreshold} Incidents.");
							}

							incidentService.Save(currentIncident);
						}
					}
				}
				logger.info("Flushing.."); 
			}

			logger.info($"Imported {count} in total. Complete.");
		}

		/// <summary>
		/// Sets the default grading.
		/// </summary>
		/// <param name="incidentGradingService">Incident grading service.</param>
		/// <param name="csvParseStrategy">Csv parse strategy.</param>
		private static void setDefaultGrading(IIncidentGradingService incidentGradingService, ICSVParseStrategy csvParseStrategy)
		{
			var defaultGrading = incidentGradingService.GetImportIncidentGrading();

			if (defaultGrading == null)
			{
				defaultGrading = new IncidentGrading() { GradeValue = null, Description = "Imported" };
			}

			csvParseStrategy.setDefaultValue(defaultGrading);
		}

		/// <summary>
		/// Gets all the import files recursivly from all files in the import location
		/// </summary>
		/// <returns>The import files.</returns>
		/// <param name="DirectoryLocation">Directory location.</param>
		private static IList<String> GetImportFiles(String DirectoryLocation)
		{
			IList<String> files = new List<String>(); 
			foreach (String currentDirectory in Directory.GetDirectories(DirectoryLocation))
			{
				foreach (String currentFile in Directory.GetFiles(currentDirectory, "*.csv"))
				{
					files.Add(currentFile); 
				}
			}

			return files;
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

			return kernel;
		}
	}
}