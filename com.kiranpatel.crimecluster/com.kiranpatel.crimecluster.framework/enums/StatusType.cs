namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Represents a Persons current Status
	/// </summary>
	public enum StatusType
	{
		/// <summary>
		/// Represents an Offline Person
		/// </summary>
		Offline = 0, 

		/// <summary>
		/// Represents an Online Person
		/// </summary>
		Online = 1, 

		/// <summary>
		/// Represents a Busy Person
		/// </summary>
		Busy = 2
	}
}