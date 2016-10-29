namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Service for basic CRUD functionality on all Entities 
	/// </summary>
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
			if (ID == null)
			{
				return null;
			}

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
	}
}