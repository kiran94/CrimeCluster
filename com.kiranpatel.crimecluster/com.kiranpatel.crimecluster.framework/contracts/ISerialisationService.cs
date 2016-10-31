namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using Newtonsoft.Json;

	/// <summary>
	/// Contract for the Serialisation Service 
	/// </summary>
	public interface ISerialisationService : IDisposable
	{
		/// <summary>
		/// Serialise the specified toSerialise.
		/// </summary>
		/// <param name="toSerialise">To serialise.</param>
		/// <typeparam name="T">The type of object to serialise</typeparam>
		String serialise<T>(T toSerialise);

		/// <summary>
		/// Deserialise the specified toDeserialise.
		/// </summary>
		/// <param name="toDeserialise">To deserialise.</param>
		/// <typeparam name="T">The type of object to deserialise</typeparam>
		T deserialise<T>(String toDeserialise);
	}
}