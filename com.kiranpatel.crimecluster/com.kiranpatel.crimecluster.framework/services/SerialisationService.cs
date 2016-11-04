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