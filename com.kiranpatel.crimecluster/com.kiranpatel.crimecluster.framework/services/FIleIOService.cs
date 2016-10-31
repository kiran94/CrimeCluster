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

		/// <summary>
		/// Releases all resource used by the <see cref="T:com.kiranpatel.crimecluster.framework.FileIOService"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.FileIOService"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.FileIOService"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.FileIOService"/> so the garbage collector can reclaim the
		/// memory that the <see cref="T:com.kiranpatel.crimecluster.framework.FileIOService"/> was occupying.</remarks>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				
			}
		}
	}
}