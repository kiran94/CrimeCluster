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
		/// The training data start date.
		/// </summary>
		private static DateTime trainingStart;

		/// <summary>
		/// The training data end date.
		/// </summary>
		private static DateTime trainingEnd;

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main(string[] args)
		{
			var logger = kernel.Get<ILogger>();
			logger.debug("Starting Predictor");

			trainingStart = new DateTime(2015, 01, 01);
			trainingEnd = new DateTime(2015, 12, 31);

			var testStart = new DateTime(2016, 01, 01);
			var testEnd = new DateTime(2016, 12, 31);
			var radius = 10D;

			logger.info($"Training Model on {trainingStart.ToLongDateString()} to {trainingEnd.ToLongDateString()}"); 
			logger.info($"Testing Model on {testStart.ToLongDateString()} to {testEnd.ToLongDateString()}");

			var evaluation = kernel.Get<IModelEvaluation>();
			var accuracy = evaluation.Evaluate(testStart, testEnd, radius);

			logger.info($"Accuracy of {accuracy}% found"); 
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