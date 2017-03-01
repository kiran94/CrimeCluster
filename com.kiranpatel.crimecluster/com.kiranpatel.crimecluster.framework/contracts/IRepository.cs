using System;
using System.Linq;

namespace com.kiranpatel.crimecluster.framework
{
	/// <summary>
	/// Repository Pattern for accessing data store
	/// </summary>
	public interface IRepository : IDisposable
	{
		/// <summary>
		/// Saves the Object to the data store
		/// </summary>
		/// <param name="toSave">object to save</param>
		/// <typeparam name="T">Type of object to save</typeparam>
		void Save<T>(T toSave) where T : EntityBase; 

		/// <summary>
		/// Gets the Object with the specified ID
		/// </summary>
		/// <param name="ID">ID of the object.</param>
		/// <typeparam name="T">Type of object to return</typeparam>
		T Get<T>(Guid ID) where T : EntityBase;

		/// <summary>
		/// Updates the object passed
		/// </summary>
		/// <param name="toUpdate">Object to update</param>
		/// <typeparam name="T">Type of the object to update</typeparam>
		void Update<T>(T toUpdate) where T : EntityBase;

		/// <summary>
		/// Soft Deletes the object passed
		/// </summary>
		/// <param name="toDelete">Object to delete</param>
		/// <typeparam name="T">Type of object to delete</typeparam>
		void Delete<T>(T toDelete) where T : EntityBase;

		/// <summary>
		/// Retrieves an IQueryable Object for the specified type
		/// </summary>
		/// <typeparam name="T">Type of object</typeparam>
		IQueryable<T> Query<T>() where T : EntityBase;

		/// <summary>
		/// Flushes the current objects persisted. 
		/// </summary>
		void Flush(); 
	}
}