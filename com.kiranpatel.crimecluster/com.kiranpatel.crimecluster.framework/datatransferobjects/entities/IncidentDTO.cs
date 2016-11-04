namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Data Transfer Object for Incidents
	/// </summary>
	public class IncidentDTO : EntityBase
	{
		/// <summary>
		/// Gets or sets the summary.
		/// </summary>
		/// <value>The summary.</value>
		public string Summary { get; set; }

		/// <summary>
		/// Gets or sets the date created.
		/// </summary>
		/// <value>The date created.</value>
		public string DateCreated { get; set; }

		/// <summary>
		/// Gets or sets the location identifier.
		/// </summary>
		/// <value>The location identifier.</value>
		public string LocationID { get; set; }

		/// <summary>
		/// Gets or sets the incident graiding.
		/// </summary>
		/// <value>The incident graiding.</value>
		public string IncidentGrading { get; set; }

		/// <summary>
		/// Gets or sets the outcome identifier.
		/// </summary>
		/// <value>The outcome identifier.</value>
		public string OutcomeIDs { get; set; }
	}
}