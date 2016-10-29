namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Base Entity for all Entity objects in the system
	/// </summary>
	public class EntityBase
	{
		/// <summary>
		/// Gets or sets the ID
		/// </summary>
		/// <value>The ID.</value>
		public Guid ID { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:com.kiranpatel.crimecluster.framework.EntityBase"/> is deleted.
		/// </summary>
		/// <value><c>true</c> if is deleted; otherwise, <c>false</c>.</value>
		public Boolean IsDeleted { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.EntityBase"/> class.
		/// </summary>
		public EntityBase()
		{
			this.ID = Guid.NewGuid(); 
		}
	}
}