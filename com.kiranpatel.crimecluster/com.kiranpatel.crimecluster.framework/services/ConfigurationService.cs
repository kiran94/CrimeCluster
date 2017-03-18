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
		public string Get(ConfigurationKey key, string defaultValue)
		{
			var val = this.Get(key);

			if (string.IsNullOrEmpty(val))
			{
				val = defaultValue; 
			}

			return val;
		}

		// <inheritdoc>
		public string Get(ConfigurationKey key)
		{
			return ConfigurationManager.AppSettings[key.ToString()]; 
		}

		// <inheritdoc>
		public string GetConnectionString(string name)
		{
			return ConfigurationManager.ConnectionStrings[name].ConnectionString; 
		}

		/// <summary>
		/// Releases all resource used by the <see cref="T:com.kiranpatel.crimecluster.framework.ConfigurationService"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.ConfigurationService"/>. The <see cref="Dispose"/> method
		/// leaves the <see cref="T:com.kiranpatel.crimecluster.framework.ConfigurationService"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.ConfigurationService"/> so the garbage collector can reclaim
		/// the memory that the <see cref="T:com.kiranpatel.crimecluster.framework.ConfigurationService"/> was occupying.</remarks>
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