namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Represents an IncidentGrading entity
	/// </summary>
	public class IncidentGrading : EntityBase
	{
		/// <summary>
		/// Gets or sets the grade value.
		/// </summary>
		/// <value>The grade value.</value>
		public int GradeValue { get; set;}

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		public String Description { get; set; }
	}
}