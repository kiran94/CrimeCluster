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
		CSVIncidentColumnNumber,

		/// <summary>
		/// Import location for csv incidents
		/// </summary>
		ImportLocation,

		/// <summary>
		/// K Means Number of Clusters
		/// </summary>
		KMeansClusterNumber,

		/// <summary>
		/// Start Date for Crime Sampling 
		/// </summary>
		StartCrimeSamplingDate,

		/// <summary>
		/// End Date for Crime Sampling
		/// </summary>
		EndCrimeSamplingDate,

		/// <summary>
		/// Minimum raduis for DJ Clustering algorithm to consider a point in a cluster
		/// </summary>
		DJClusterRadiusEps,

		/// <summary>
		/// Minimum number of points for DJ Clustering algorithm to consider a cluster a cluster
		/// </summary>
		DJClusterMinPts,

		/// <summary>
		/// Google Maps Key 
		/// </summary>
		GoogleMapsKey
	}
}