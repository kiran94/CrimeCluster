using System;
namespace com.kiranpatel.crimecluster.framework
{
	/// <summary>
	/// Represents a Person
	/// </summary>
	public abstract class Person : EntityBase
	{
		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public virtual String Title { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>The first name.</value>
		public virtual String FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		/// <value>The last name.</value>
		public virtual String LastName { get; set; }

		/// <summary>
		/// Gets or sets the dob.
		/// </summary>
		/// <value>The dob.</value>
		public virtual DateTime DOB { get; set; }

		/// <summary>
		/// Gets or sets the date registered.
		/// </summary>
		/// <value>The date registered.</value>
		public virtual DateTime DateRegistered { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		public virtual StatusType Status { get; set; }
	}
}
