namespace com.kiranpatel.crimecluster.webservice
{
	using System;
	using System.ComponentModel;
	using com.kiranpatel.crimecluster.framework;

	/// <summary>
	/// Crime type model.
	/// </summary>
	public class CrimeTypeModel
	{
		[DisplayName("Crime Type")]
		public CrimeType type { get; set; }
	}
}
