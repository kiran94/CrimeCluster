namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Contract for the a CSV Parse Stategy
	/// </summary>
	public interface ICSVParseStrategy
	{
		/// <summary>
		/// Parses a row into type T
		/// </summary>
		/// <param name="row">Row.</param>
		object parse(String[] row);
	}
}