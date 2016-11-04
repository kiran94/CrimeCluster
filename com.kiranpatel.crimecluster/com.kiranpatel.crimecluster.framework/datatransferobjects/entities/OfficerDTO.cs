namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Data Transfer Object for <see cref="Officer"/> class
	/// </summary>
	public class OfficerDTO : EntityBase
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the badge number.
		/// </summary>
		/// <value>The badge number.</value>
		public string BadgeNumber { get; set; }

		/// <summary>
		/// Gets or sets the location identifier.
		/// </summary>
		/// <value>The location identifier.</value>
		public string LocationID { get; set; }

		/// <summary>
		/// Gets or sets the incident identifier.
		/// </summary>
		/// <value>The incident identifier.</value>
		public string IncidentID { get; set; }
	}
}