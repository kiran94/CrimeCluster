using System;
namespace com.kiranpatel.crimecluster.framework
{
	/// <summary>
	/// Contract for Configuration service.
	/// </summary>
	public interface IConfigurationService
	{
		/// <summary>
		/// Get the specified config value for the given key or the default value
		/// </summary>
		/// <param name="key">Config Key</param>
		/// <param name="defaultValue">Default value.</param>
		String Get(ConfigurationKey key, String defaultValue);
	}
}