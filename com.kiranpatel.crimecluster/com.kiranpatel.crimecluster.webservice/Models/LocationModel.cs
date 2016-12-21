namespace com.kiranpatel.crimecluster.webservice
{
	using System;

	/// <summary>
	/// Location model.
	/// </summary>
	public class LocationModel
	{
		/// <summary>
		/// Gets the cluster no.
		/// </summary>
		/// <value>The cluster no.</value>
		public int ClusterNo { get; private set; }

		/// <summary>
		/// Gets the latitude.
		/// </summary>
		/// <value>The latitude.</value>
		public String Latitude { get; private set; }

		/// <summary>
		/// Gets the longitude.
		/// </summary>
		/// <value>The longitude.</value>
		public String Longitude { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.webservice.LocationModel"/> class.
		/// </summary>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		public LocationModel(String latitude, String longitude)
		{
			this.Latitude = latitude;
			this.Longitude = longitude;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.webservice.LocationModel"/> class.
		/// </summary>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		/// <param name="clusterNo">Cluster no.</param>
		public LocationModel(String latitude, String longitude, int clusterNo) 
			: this (latitude, longitude)
		{
			this.ClusterNo = clusterNo;
		}
	}
}