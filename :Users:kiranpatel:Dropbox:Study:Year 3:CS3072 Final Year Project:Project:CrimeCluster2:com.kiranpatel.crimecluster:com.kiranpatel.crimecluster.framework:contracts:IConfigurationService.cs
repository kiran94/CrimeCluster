namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Contract for Configuration service.
	/// </summary>
	public interface IConfigurationService
	{
		T Get<T>(ConfigurationKey key, T defaultValue);
	}
}
