namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Configuration;

	/// <summary>
	/// Service for loading Configuration Keys
	/// </summary>
	public class ConfigurationService : IConfigurationService
	{
		// <inheritdoc>
		public String Get(ConfigurationKey key, String defaultValue)
		{
			return ConfigurationManager.AppSettings[key.ToString()];
		}

		// <inheritdoc>
		public String GetConnectionString(String name)
		{
			return ConfigurationManager.ConnectionStrings[name].ConnectionString; 
		}
	}
}