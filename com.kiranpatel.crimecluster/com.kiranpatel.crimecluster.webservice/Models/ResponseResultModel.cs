namespace com.kiranpatel.crimecluster.webservice
{
	using System;
	using com.kiranpatel.crimecluster.framework; 

	/// <summary>
	/// ViewModel for the response to a result
	/// </summary>
	public class ResponseResultModel
	{
		/// <summary>
		/// The status of the response 
		/// </summary>
		public ResponseResultType Status;

		/// <summary>
		/// The message of the response
		/// </summary>
		public String Message;

		/// <summary>
		/// The response content of the response
		/// </summary>
		public String Response; 
	}
}