namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Represents the result type for a response from a web service
	/// </summary>
	public enum ResponseResultType
	{
		/// <summary>
		/// The default.
		/// </summary>
		DEFAULT = 0,

		/// <summary>
		/// OK Response Type
		/// </summary>
		OK  = 1, 

		/// <summary>
		/// Error Response Type
		/// </summary>
		ERROR = 2,
	}
}