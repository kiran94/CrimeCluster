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
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.SerialisationService"/> class.
		/// </summary>
		/// <param name="logger">Logger.</param>
		public SerialisationService(ILogger logger)
		{
			this.logger = logger; 
		}

		// <inheritdoc>
		public string serialise<T>(T toSerialise)
		{
			this.logger.info("Serialising Object"); 
			return JsonConvert.SerializeObject(toSerialise); 
		}

		// <inheritdoc>
		public T deserialise<T>(string toDeserialise)
		{
			this.logger.info("Deserialising Object");
			return JsonConvert.DeserializeObject<T>(toDeserialise); 
		}
	}
}