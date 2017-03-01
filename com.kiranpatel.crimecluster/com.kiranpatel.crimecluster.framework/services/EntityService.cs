namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Service for basic CRUD functionality on all Entities 
	/// </summary>
	/// <typeparam name="T">Entity type to do operations on.</typeparam>
	public class EntityService<T> : IEntityService<T> where T : EntityBase
	{
		/// <summary>
		/// The repository instance.
		/// </summary>
		protected readonly IRepository repository;

		/// <summary>
		/// The logger instance, 
		/// </summary>
		protected readonly ILogger logger; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.EntityService`1"/> class.
		/// </summary>
		/// <param name="repository">Repository.</param>
		/// <param name="logger">Logger.</param>
		public EntityService(IRepository repository, ILogger logger)
		{
			this.repository = repository;
			this.logger = logger; 
		}

		// <inheritdoc>
		public void Save(T toSave)
		{
			if (toSave == null)
			{
				return; 
			}

			this.logger.debug(String.Format("Saving {0}: {1}", typeof(T).ToString(), toSave.ID));
			this.repository.Save(toSave); 
		}

		// <inheritdoc>
		public T Get(Guid ID)
		{
			this.logger.debug(String.Format("Getting {0}: {1}", typeof(T).ToString(), ID.ToString()));
			return this.repository.Get<T>(ID); 
		}

		// <inheritdoc>
		public void Update(T toUpdate)
		{
			if (toUpdate == null)
			{
				return;
			}

			this.logger.debug(String.Format("Updating {0}: {1}", typeof(T).ToString(), toUpdate.ID.ToString()));
			this.repository.Update(toUpdate);
		}

		// <inheritdoc>
		public void Delete(T toDelete)
		{
			if (toDelete == null || toDelete.IsDeleted)
			{
				return; 
			}

			this.logger.debug(String.Format("Deleting {0}: {1}", typeof(T).ToString(), toDelete.ID.ToString()));
			this.repository.Delete(toDelete); 
		}

		// <inheritdoc>
		public void Flush()
		{
			if (this.repository != null)
			{
				this.repository.Flush();
			}
		}

		/// <summary>
		/// Releases all resource used by the <see cref="T:com.kiranpatel.crimecluster.framework.EntityService`1"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.EntityService`1"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.EntityService`1"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.EntityService`1"/> so the garbage collector can reclaim the
		/// memory that the <see cref="T:com.kiranpatel.crimecluster.framework.EntityService`1"/> was occupying.</remarks>
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
				if (this.repository != null)
				{
					this.repository.Dispose(); 
				}
			}
		}
	}
}