using System;
namespace com.kiranpatel.crimecluster.framework
{
	/// <summary>
	/// Contract for Configuration service.
	/// </summary>
	public interface IConfigurationService : IDisposable
	{
		/// <summary>
		/// Get the specified config value for the given key or the default value
		/// </summary>
		/// <param name="key">Config Key</param>
		/// <param name="defaultValue">Default value.</param>
		string Get(ConfigurationKey key, string defaultValue);

		/// <summary>
		/// Gets the specified config value. 
		/// </summary>
		/// <param name="key">Key.</param>
		string Get(ConfigurationKey key); 

		/// <summary>
		/// Gets the connection string with the associated name
		/// </summary>
		/// <returns>The connection string.</returns>
		/// <param name="name">Name.</param>
		string GetConnectionString(string name); 
	}
}