namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Configuration key to retrieve config values
	/// </summary>
	public enum ConfigurationKey
	{
		/// <summary>
		/// The default. 
		/// </summary>
		Default,

		/// <summary>
		/// Represents the Culture of the application
		/// </summary>
		CultureInfo,

		/// <summary>
		/// Represents the Date formate regular expression a csv for an incident must match
		/// </summary>
		CSVIncidentDateFormatRegex,

		/// <summary>
		/// Represents the number of columns from an incident csv file
		/// </summary>
		CSVIncidentColumnNumber
	}
}