
namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic; 

	/// <summary>
	/// Represents an Incident Entity
	/// </summary>
	public class Incident : EntityBase
	{
		/// <summary>
		/// Gets or sets the summary.
		/// </summary>
		/// <value>The summary.</value>
		public virtual String Summary { get; set;}

		/// <summary>
		/// Gets or sets the date created.
		/// </summary>
		/// <value>The date created.</value>
		public virtual DateTime DateCreated { get; set; }

		/// <summary>
		/// Gets or sets the location.
		/// </summary>
		/// <value>The location.</value>
		public virtual Location Location { get; set; }

		/// <summary>
		/// Gets or sets the grading.
		/// </summary>
		/// <value>The grading.</value>
		public virtual IncidentGrading Grading { get; set; }

		/// <summary>
		/// Gets or sets the outcome.
		/// </summary>
		/// <value>The outcome.</value>
		public virtual ICollection<IncidentOutcome> Outcome { get; set; }
	}
}