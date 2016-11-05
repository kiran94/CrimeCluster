namespace com.kiranpatel.crimecluster.importer
{
	using System;
	using com.kiranpatel.crimecluster.framework;
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
			var configService = kernel.Get<IConfigurationService>();

			var import = configService.Get(ConfigurationKey.ImportLocation, "defaultimport"); 

			logger.debug(import);
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

			return kernel;
		}
	}
}