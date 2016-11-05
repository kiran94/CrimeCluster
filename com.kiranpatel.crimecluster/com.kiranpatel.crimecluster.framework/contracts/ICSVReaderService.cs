namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Contract for reading CSV Files
	/// </summary>
	public interface ICSVReaderService : IDisposable
	{
		/// <summary>
		/// Parses a CSV into a collection of Objects of Type T
		/// </summary>
		/// <returns>The collection of objects</returns>
		/// <param name="fileLocation">File location of the csv</param>
		/// <param name="parseType">Parse type.</param>
		/// <param name="parseStrategy">parse startegy</param>
		/// <param name="hasHeader">Flag indicating the csv file has a header</param>
		/// <typeparam name="T">Type to convert each row into</typeparam>
		ICollection<T> parseCSV<T>(String fileLocation, CSVParseType parseType, ICSVParseStrategy parseStrategy, bool hasHeader = false) where T : EntityBase;
	}
}