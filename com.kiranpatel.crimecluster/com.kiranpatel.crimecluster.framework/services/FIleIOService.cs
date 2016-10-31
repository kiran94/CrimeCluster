namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.IO;

	/// <summary>
	/// Service for interacting with Files
	/// </summary>
	public class FileIOService : IFileIOService
	{
		// <inheritdoc>
		public bool Exists(string fileLocation)
		{
			return File.Exists(fileLocation); 
		}

		// <inheritdoc>
		public Stream openStream(String fileLocation, FileMode mode, FileAccess access, FileShare share = FileShare.Read)
		{
			if (String.IsNullOrEmpty(fileLocation))
			{
				return null; 
			}

			return File.Open(fileLocation, mode, access, share);
		}
	}
}