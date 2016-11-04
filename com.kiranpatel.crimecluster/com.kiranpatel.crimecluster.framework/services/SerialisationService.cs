namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using Newtonsoft.Json; 

	/// <summary>
	/// Service for Serialisating and Deserialising Objects
	/// </summary>
	public class SerialisationService : ISerialisationService
	{
		/// <summary>
		/// The logger instance
		/// </summary>
		private readonly ILogger logger;

		/// <summary>
		/// The config service instance 
		/// </summary>
		private readonly IConfigurationService configService; 

		/// <summary>
		/// JSON Serialiser Settings
		/// </summary>
		private JsonSerializerSettings settings;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.SerialisationService"/> class.
		/// </summary>
		/// <param name="logger">Logger.</param>
		/// <param name="configService">Config Service.</param>
		public SerialisationService(ILogger logger, IConfigurationService configService)
		{
			this.logger = logger;
			this.settings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

			var indent = Boolean.Parse(this.configService.Get(ConfigurationKey.SerialiserIndent, "false"));
			if (indent)
			{
				this.settings.Formatting = Formatting.Indented;
			}
		}

		// <inheritdoc>
		public string serialise<T>(T toSerialise)
		{
			this.logger.info("Serialising Object"); 
			return JsonConvert.SerializeObject(toSerialise, this.settings); 
		}

		// <inheritdoc>
		public T deserialise<T>(string toDeserialise)
		{
			this.logger.info("Deserialising Object");
			return JsonConvert.DeserializeObject<T>(toDeserialise, this.settings); 
		}

		/// <summary>
		/// Releases all resource used by the <see cref="T:com.kiranpatel.crimecluster.framework.SerialisationService"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.SerialisationService"/>. The <see cref="Dispose"/> method
		/// leaves the <see cref="T:com.kiranpatel.crimecluster.framework.SerialisationService"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.SerialisationService"/> so the garbage collector can reclaim
		/// the memory that the <see cref="T:com.kiranpatel.crimecluster.framework.SerialisationService"/> was occupying.</remarks>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				
			}
		}
	}
}