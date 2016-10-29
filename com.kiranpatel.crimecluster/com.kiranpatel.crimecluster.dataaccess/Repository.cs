namespace com.kiranpatel.crimecluster.dataaccess
{
	using System;
	using System.Linq;
	using com.kiranpatel.crimecluster.framework;
	using NHibernate;
	using NHibernate.Linq;

	/// <summary>
	/// Repository Layer
	/// </summary>
	public class Repository : IRepository
	{
		/// <summary>
		/// Session of the ORM
		/// </summary>
		private readonly ISession session; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.dataaccess.Repository"/> class.
		/// </summary>
		/// <param name="session">ORM session</param>
		public Repository(ISession session)
		{
			this.session = session; 
		}

		// <inheritdoc>
		public void Save<T>(T toSave) where T : EntityBase
		{
			this.session.Save(toSave); 
		}

		// <inheritdoc>
		public T Get<T>(Guid ID) where T : EntityBase
		{
			return this.session.Get<T>(ID); 
		}

		// <inheritdoc>
		public void Update<T>(T toUpdate) where T : EntityBase
		{
			this.session.Update(toUpdate); 
		}

		// <inheritdoc>
		public void Delete<T>(T toDelete) where T : EntityBase
		{
			if (!toDelete.IsDeleted)
			{
				toDelete.IsDeleted = true;
				this.Update(toDelete); 
			}
		}

		// <inheritdoc>
		public IQueryable<T> Query<T>() where T : EntityBase
		{
			return this.session.Query<T>(); 
		}
	}
}