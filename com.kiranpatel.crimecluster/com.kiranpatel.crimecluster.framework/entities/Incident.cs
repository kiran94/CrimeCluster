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
		/// Gets or sets the crime identifier.
		/// </summary>
		/// <value>The crime identifier.</value>
		public virtual String CrimeID { get; set; }

		/// <summary>
		/// Gets or sets the date created.
		/// </summary>
		/// <value>The date created.</value>
		public virtual DateTime DateCreated { get; set; }

		/// <summary>
		/// Gets or sets the reported by.
		/// </summary>
		/// <value>The reported by.</value>
		public virtual String ReportedBy { get; set; }

		/// <summary>
		/// Gets or sets the falls within.
		/// </summary>
		/// <value>The falls within.</value>
		public virtual String FallsWithin { get; set; }

		/// <summary>
		/// Gets or sets the location.
		/// </summary>
		/// <value>The location.</value>
		public virtual Location Location { get; set; }

		/// <summary>
		/// Gets or sets the location desc.
		/// </summary>
		/// <value>The location desc.</value>
		public virtual String LocationDesc { get; set; }

		/// <summary>
		/// Gets or sets the LSOAC ode.
		/// </summary>
		/// <value>The LSOAC ode.</value>
		public virtual String LSOACode { get; set; }

		/// <summary>
		/// Gets or sets the LSOAN ame.
		/// </summary>
		/// <value>The LSOAN ame.</value>
		public virtual String LSOAName { get; set; }

		/// <summary>
		/// Gets or sets the type of the crime.
		/// </summary>
		/// <value>The type of the crime.</value>
		public virtual String CrimeType { get; set; }

		/// <summary>
		/// Gets or sets the last outcome category.
		/// </summary>
		/// <value>The last outcome category.</value>
		public virtual String LastOutcomeCategory { get; set; }

		/// <summary>
		/// Gets or sets the context.
		/// </summary>
		/// <value>The context.</value>
		public virtual String Context { get; set; }


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