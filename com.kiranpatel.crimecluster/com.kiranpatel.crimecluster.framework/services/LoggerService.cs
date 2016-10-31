namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.IO;
	using log4net;
	using log4net.Config;

	/// <summary>
	/// Logger service
	/// </summary>
	public class LoggerService : ILogger
	{
		/// <summary>
		/// The Log4Net Logger
		/// </summary>
		private readonly ILog log;

		/// <summary>
		/// Instance of the LoggerService
		/// </summary>
		private static LoggerService instance; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.LoggerService"/> class.
		/// </summary>
		private LoggerService()
		{
			log = LogManager.GetLogger(typeof(LoggerService));
			XmlConfigurator.Configure(new FileInfo("log4net.xml"));
		}

		// <inheritdoc>
		public void debug(string message)
		{
			this.log.Debug(message); 
		}

		// <inheritdoc>
		public void info(string message)
		{
			this.log.Info(message); 
		}

		// <inheritdoc>
		public void warn(string message)
		{
			this.log.Warn(message); 
		}

		// <inheritdoc>
		public void error(string message)
		{
			this.log.Error(message); 
		}

		// <inheritdoc>
		public void error(string message, Exception e)
		{
			this.log.Error(message, e);
		}

		// <inheritdoc>
		public void fatal(string message)
		{
			this.log.Fatal(message);
		}

		// <inheritdoc>
		public void fatal(string message, Exception e)
		{
			this.log.Fatal(message);
		}

		/// <summary>
		/// Gets an instance of the LoggerService if it exists, else creates one and returns it
		/// </summary>
		/// <returns>The instance.</returns>
		public static LoggerService GetInstance()
		{
			if (LoggerService.instance == null)
			{
				LoggerService.instance = new LoggerService(); 
			}

			return LoggerService.instance; 
		}
	}
}