namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Represents a Location Entity
	/// </summary>
	public class Location : EntityBase
	{
		/// <summary>
		/// Gets or sets the latitude.
		/// </summary>
		/// <value>The latitude.</value>
		public virtual double? Latitude { get; set; }

		/// <summary>
		/// Gets or sets the longitude.
		/// </summary>
		/// <value>The longitude.</value>
		public virtual double? Longitude { get; set; }

		/// <summary>
		/// Gets or sets the date logged.
		/// </summary>
		/// <value>The date logged.</value>
		public virtual DateTime? DateLogged { get; set; }
	}
}