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
		private static IKernel kernel;

		/// <summary>
		/// The training dataset start.
		/// </summary>
		private static DateTime trainingStart;

		/// <summary>
		/// The training dataset end.
		/// </summary>
		private static DateTime trainingEnd; 

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main(string[] args)
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();
			kernel = GenerateKernel();

			var configService = kernel.Get<IConfigurationService>(); 
			var testStart = Convert.ToDateTime(configService.Get(ConfigurationKey.TestStartDate, "01/01/2016"));
			var testEnd = Convert.ToDateTime(configService.Get(ConfigurationKey.TestEndDate, "31/12/2016"));
			var radius = Convert.ToDouble(configService.Get(ConfigurationKey.ModelEvaluatorRadius, "0.001"));

			var logger = kernel.Get<ILogger>();
			logger.debug("Starting Predictor");

			logger.info($"Training Model on { trainingStart.ToLongDateString() } to { trainingEnd.ToLongDateString() }"); 
			logger.info($"Testing Model on { testStart.ToLongDateString() } to { testEnd.ToLongDateString() }");
			logger.info($"Model Evaluation on Radius { radius }");

			var evaluation = kernel.Get<IModelEvaluation>();
			evaluation.SetUp(); 

			var accuracy = evaluation.Evaluate(testStart, testEnd, radius);
			logger.info($"Accuracy of { accuracy * 100 }% found");

			watch.Stop();
			logger.info($"{ watch.ElapsedMilliseconds } ms."); 
		}

		/// <summary>
		/// Generates the kernel with all the contract/concrete implementation bindings
		/// </summary>
		/// <returns>The kernel.</returns>
		private static IKernel GenerateKernel()
		{
			var configService = new ConfigurationService();
			trainingStart = Convert.ToDateTime(configService.Get(ConfigurationKey.TrainingStartDate, "01/01/2015"));
			trainingEnd = Convert.ToDateTime(configService.Get(ConfigurationKey.TrainingEndDate, "31/12/2015"));

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
			_kernel.Bind<IMixedMarkovModel>().To<MixedMarkovModel>()
			       .WithConstructorArgument("start", trainingStart)
			       .WithConstructorArgument("end", trainingEnd);

			_kernel.Bind<IModelEvaluation>().To<ModelEvaluation>(); 
			
			return _kernel;
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
	}
}