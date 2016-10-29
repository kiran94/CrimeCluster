namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Represents an Officer
	/// </summary>
	public class Officer : Person
	{
		/// <summary>
		/// Gets or sets the badge number.
		/// </summary>
		/// <value>The badge number.</value>
		public virtual String BadgeNumber { get; set; }

		/// <summary>
		/// Gets or sets the location.
		/// </summary>
		/// <value>The location.</value>
		public virtual Location Location { get; set; }
	}
}