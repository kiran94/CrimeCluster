namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Represents an IncidentOutcome
	/// </summary>
	public class IncidentOutcome : EntityBase
	{
		/// <summary>
		/// Gets or sets the date created.
		/// </summary>
		/// <value>The date created.</value>
		public virtual DateTime DateCreated { get; set; }

		/// <summary>
		/// Gets or sets the outcome.
		/// </summary>
		/// <value>The outcome.</value>
		public virtual String Outcome { get; set; }

		/// <summary>
		/// Gets or sets the incident.
		/// </summary>
		/// <value>The incident.</value>
		public virtual Incident Incident { get; set; }

		/// <summary>
		/// Gets or sets the officer.
		/// </summary>
		/// <value>The officer.</value>
		public virtual Officer Officer { get; set; }
	}
}