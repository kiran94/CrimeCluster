namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using NUnit.Framework;
	using Moq; 

	/// <summary>
	/// Tests for the Entity Service
	/// </summary>
	[TestFixture]
	public class EntityServiceTests
	{
		/// <summary>
		/// The repository mock
		/// </summary>
		private Mock<IRepository> repository;

		/// <summary>
		/// The logger mock
		/// </summary>
		private Mock<ILogger> logger;

		/// <summary>
		/// Sets up.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			this.repository = new Mock<IRepository>();
			this.logger = new Mock<ILogger>(); 
		}

		/// <summary>
		/// Ensures when the entity is null, nothing is saved
		/// </summary>
		[Test]
		public void Save_NullEntity_NothingSaved()
		{
			Location loc = null;
			this.repository.Setup(x => x.Save(loc)).Verifiable();

			this.GetInstance().Save(loc);

			this.repository.Verify(x => x.Save(loc), Times.Never); 
		}

		/// <summary>
		/// Ensures when the entity is passed, it is saved
		/// </summary>
		[Test]
		public void Save_Entity_Saved()
		{
			Location loc = new Location();
			this.repository.Setup(x => x.Save(loc)).Verifiable();

			this.GetInstance().Save(loc);

			this.repository.Verify(x => x.Save(loc), Times.Once);
		}

		/// <summary>
		/// Ensures when the entity exists, it is returned
		/// </summary>
		[Test]
		public void Get_Exists_Returned()
		{
			Location loc = new Location(); 
			this.repository.Setup(x => x.Get<Location>(loc.ID)).Returns(loc).Verifiable();

			var result = this.GetInstance().Get(loc.ID);

			Assert.AreSame(loc, result); 

			this.repository.Verify(x => x.Get<Location>(loc.ID), Times.Once);
		}

		/// <summary>
		/// Ensures when the entity does not exist, null is returned 
		/// </summary>
		[Test]
		public void Get_NotExists_NullReturned()
		{
			Location loc = new Location();
			this.repository.Setup(x => x.Get<Location>(loc.ID)).Returns<Location>(null).Verifiable();

			var result = this.GetInstance().Get(loc.ID);

			Assert.IsNull(result); 

			this.repository.Verify(x => x.Get<Location>(loc.ID), Times.Once);
		}

		/// <summary>
		/// Ensures when the passed entity is null, nothing is updated
		/// </summary>
		[Test]
		public void Update_NullEntity_NotUpdated()
		{
			Location loc = null;
			this.repository.Setup(x => x.Update<Location>(loc)).Verifiable();

			this.GetInstance().Update(loc);

			this.repository.Verify(x => x.Update<Location>(loc), Times.Never);
		}

		/// <summary>
		/// Ensures when the entity is passed, it is updated
		/// </summary>
		[Test]
		public void Update_Entity_Updated()
		{
			Location loc = new Location();
			this.repository.Setup(x => x.Update<Location>(loc)).Verifiable();

			this.GetInstance().Update(loc);

			this.repository.Verify(x => x.Update<Location>(loc), Times.Once);
		}

		/// <summary>
		/// Ensures when the entity is unll, nothing is deleted
		/// </summary>
		[Test]
		public void Delete_NullEntity_NothingDeleted()
		{
			Location loc = null;
			this.repository.Setup(x => x.Delete<Location>(loc)).Verifiable();

			this.GetInstance().Delete(loc);

			this.repository.Verify(x => x.Delete<Location>(loc), Times.Never);
		}

		/// <summary>
		/// Ensures when the entity is passed, it it deleted
		/// </summary>
		[Test]
		public void Delete_Entity_Deleted()
		{
			Location loc = new Location();
			this.repository.Setup(x => x.Delete<Location>(loc)).Verifiable();

			this.GetInstance().Delete(loc);

			this.repository.Verify(x => x.Delete<Location>(loc), Times.Once);
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private EntityService<Location> GetInstance()
		{
			return new EntityService<Location>(repository.Object, logger.Object); 
		}

	}
}