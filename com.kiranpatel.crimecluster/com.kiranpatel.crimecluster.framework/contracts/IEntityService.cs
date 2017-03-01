using System;
namespace com.kiranpatel.crimecluster.framework
{
	/// <summary>
	/// CRUD functionality for all Entities
	/// </summary>
	public interface IEntityService<T> : IDisposable where T : EntityBase
	{
		/// <summary>
		/// Save the specified toSave.
		/// </summary>
		/// <param name="toSave">To save.</param>
		void Save(T toSave);

		/// <summary>
		/// Get the specified with the ID.
		/// </summary>
		/// <param name="ID">Identifier.</param>
		T Get(Guid ID);

		/// <summary>
		/// Update the specified toUpdate.
		/// </summary>
		/// <param name="toUpdate">To update.</param>
		void Update(T toUpdate);

		/// <summary>
		/// Delete the specified toDelete.
		/// </summary>
		/// <param name="toDelete">To delete.</param>
		void Delete(T toDelete);

		/// <summary>
		/// Flush changes in this instance.
		/// </summary>
		void Flush(); 
	}
}