namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using NUnit.Framework;
	using Moq;
	using System.Collections.Generic;
	using System.Linq;

	[TestFixture]
	public class OfficerServiceTests
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
		/// Ensures when there are no officers online, an empty list is returned
		/// </summary>
		[Test]
		public void GetOnlineOfficerLocations_NoOfficersOnline_EmptyList()
		{
			IList<Officer> officers = new List<Officer>();
			for (int i = 0; i < 10; i++)
			{
				officers.Add(new Officer() { Status = StatusType.Offline }); 
			}

			this.repository.Setup(o => o.Query<Officer>()).Returns(officers.AsQueryable());

			var result = this.GetInstance().GetOnlineOfficerLocations();

			CollectionAssert.IsEmpty(result); 
		}

		/// <summary>
		/// Ensures when there are some officers online, only those are returned
		/// </summary>
		[Test]
		public void GetOnlineOfficerLocations_SomeOfficersOnline_OnlyOnlineReturned()
		{
			IList<Officer> officers = new List<Officer>();
			for (int i = 0; i < 10; i++)
			{
				officers.Add(new Officer() { Status = (i % 2 == 0) ? StatusType.Offline : StatusType.Online });
			}

			this.repository.Setup(o => o.Query<Officer>()).Returns(officers.AsQueryable());

			var result = this.GetInstance().GetOnlineOfficerLocations();

			Assert.AreEqual(5, result.Count);
			Assert.That(result.All(o => officers.SingleOrDefault(x => x.ID == o.Key).Status == StatusType.Online));  
		}

		/// <summary>
		/// Ensures when all officers are online, all are returned
		/// </summary>
		[Test]
		public void GetOnlineOfficerLocations_AllOfficersOnline_AllReturned()
		{
			IList<Officer> officers = new List<Officer>();
			for (int i = 0; i < 10; i++)
			{
				officers.Add(new Officer() { Status = StatusType.Online });
			}

			this.repository.Setup(o => o.Query<Officer>()).Returns(officers.AsQueryable());

			var result = this.GetInstance().GetOnlineOfficerLocations();

			Assert.AreEqual(officers.Count, result.Count);
			Assert.That(result.All(o => officers.SingleOrDefault(x => x.ID == o.Key).Status == StatusType.Online));
		}

		/// <summary>
		/// Ensures when the officer is passed as null, nothing is updated
		/// </summary>
		[Test]
		public void SetOfficerOnline_NullOfficer_NothingUpdated()
		{
			Officer officer = null;
			this.repository.Setup(o => o.Update(officer)).Verifiable();
			this.GetInstance().SetOfficerOnline(officer);
			this.repository.Verify(o => o.Update(officer), Times.Never); 
		}

		/// <summary>
		/// Ensures when the officer is passed, it is updated
		/// </summary>
		[Test]
		public void SetOfficerOnline_Officer_Updated()
		{
			Officer officer = new Officer();
			this.repository.Setup(o => o.Update(It.Is<Officer>(x => x.Status == StatusType.Online))).Verifiable();
			this.GetInstance().SetOfficerOnline(officer);
			this.repository.Verify(o => o.Update(It.Is<Officer>(x => x.Status == StatusType.Online)), Times.Once);
		}

		/// <summary>
		/// Ensures when the officer is passed as null, nothing is updated
		/// </summary>
		[Test]
		public void SetOfficerOffline_NullOfficer_NothingUpdated()
		{
			Officer officer = null;
			this.repository.Setup(o => o.Update(officer)).Verifiable();
			this.GetInstance().SetOfficerOffline(officer);
			this.repository.Verify(o => o.Update(officer), Times.Never);
		}

		/// <summary>
		/// Ensures when the officer is passed, it is updated
		/// </summary>
		[Test]
		public void SetOfficerOffline_Officer_Updated()
		{
			Officer officer = new Officer() { Status = StatusType.Online };
			this.repository.Setup(o => o.Update(It.Is<Officer>(x => x.Status == StatusType.Offline))).Verifiable();
			this.GetInstance().SetOfficerOffline(officer);
			this.repository.Verify(o => o.Update(It.Is<Officer>(x => x.Status == StatusType.Offline)), Times.Once);
		}

		/// <summary>
		/// Ensures when the officer is null, nothing is updated
		/// </summary>
		[Test]
		public void SetOfficerBusy_NullOfficer_NothingUpdated()
		{
			Officer officer = null;
			this.repository.Setup(o => o.Update(officer)).Verifiable();
			this.GetInstance().SetOfficerBusy(officer);
			this.repository.Verify(o => o.Update(officer), Times.Never);
		}

		/// <summary>
		/// Ensures when the officer is passed, it is updated
		/// </summary>
		[Test]
		public void SetOfficerBusy_Officer_Updated()
		{
			Officer officer = new Officer() { Status = StatusType.Online };
			this.repository.Setup(o => o.Update(It.Is<Officer>(x => x.Status == StatusType.Busy))).Verifiable(); 
			this.GetInstance().SetOfficerBusy(officer);
			this.repository.Verify(o => o.Update(It.Is<Officer>(x => x.Status == StatusType.Busy)), Times.Once);
		}

		/// <summary>
		/// Ensures when one of the required attributes are null, false is returned
		/// </summary>
		[Test]
		public void Validate_InvalidOfficer_False()
		{
			Officer officer = new Officer()
			{
				Title = "title",
				FirstName = "firstName",
				LastName = "lastName",
				BadgeNumber = null,
				DOB = DateTime.Now,
				DateRegistered = DateTime.Now,
				Status = StatusType.Busy
			};

			var result = this.GetInstance().Validate(officer);
			Assert.False(result); 
		}

		/// <summary>
		/// Ensures when all required attributes are not null, true is required 
		/// </summary>
		[Test]
		public void Validate_ValidOfficer_True()
		{
			Officer officer = new Officer()
			{
				Title = "title",
				FirstName = "firstName",
				LastName = "lastName",
				BadgeNumber = "001",
				DOB = DateTime.Now,
				DateRegistered = DateTime.Now,
				Status = StatusType.Busy
			};

			var result = this.GetInstance().Validate(officer);
			Assert.True(result);
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private IOfficerService GetInstance()
		{
			return new OfficerService(this.repository.Object, this.logger.Object); 
		}
	}
}
