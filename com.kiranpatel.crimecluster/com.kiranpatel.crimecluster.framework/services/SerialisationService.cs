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
		public string serialise<T>(T toSerialise) where T : EntityBase
		{
			this.logger.info(String.Format("Serialising Object {0} of Type {1}", toSerialise.ID.ToString(), typeof(T).ToString())); 
			return JsonConvert.SerializeObject(toSerialise); 
		}

		// <inheritdoc>
		public T deserialise<T>(string toDeserialise) where T : EntityBase
		{
			this.logger.info(String.Format("Deserialising Object to type {0}", typeof(T).ToString()));
			return JsonConvert.DeserializeObject<T>(toDeserialise); 
		}
	}
}