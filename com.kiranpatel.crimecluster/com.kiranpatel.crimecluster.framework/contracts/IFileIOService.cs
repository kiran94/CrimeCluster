namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.IO;

	/// <summary>
	/// Contract for the File IO Service
	/// </summary>
	public interface IFileIOService : IDisposable
	{
		/// <summary>
		/// Flag indicating if the file exists on the file system
		/// </summary>
		/// <returns><c>true</c>, if file exist, <c>false</c> otherwise.</returns>
		/// <param name="fileLocation">File location.</param>
		bool Exists(String fileLocation);

		/// <summary>
		/// Opens the stream to the given file location
		/// </summary>
		/// <returns>The stream.</returns>
		/// <param name="fileLocation">File location.</param>
		/// <param name="mode">Mode to open the file.</param>
		/// <param name="access">Access for read, writing or both</param>
		/// <param name="share">Control access for other processes on the resource</param>
		Stream openStream(String fileLocation, FileMode mode, FileAccess access, FileShare share = FileShare.Read); 
	}
}