namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using NUnit.Framework;
	using Moq;
	using com.kiranpatel.crimecluster.framework;

	/// <summary>
	/// Tests for the Incident Backlog Service
	/// </summary>
	[TestFixture]
	public class IncidentBacklogServiceTests
	{
		/// <summary>
		/// The heap mock
		/// </summary>
		private Mock<IHeap<Incident>> heap; 

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
			this.heap = new Mock<IHeap<Incident>>(); 
			this.logger = new Mock<ILogger>(); 
		}

		/// <summary>
		/// Ensures when an null incident is passed, nothing is added
		/// </summary>
		[Test]
		public void add_NullIncident_NothingAdded()
		{
			Incident incident = null;
			this.heap.Setup(x => x.add(incident)).Verifiable();

			this.GetInstance().add(incident);

			this.heap.Verify(x => x.add(incident), Times.Never); 
		}

		/// <summary>
		/// Ensures when an incident is passed, is it added to the backlog
		/// </summary>
		[Test]
		public void add_Incident_Added()
		{
			Incident incident = new Incident();
			this.heap.Setup(x => x.add(incident)).Verifiable();

			this.GetInstance().add(incident);

			this.heap.Verify(x => x.add(incident), Times.Once);
		}

		/// <summary>
		/// Ensures when the backlog is populated, a non zero value is returned
		/// </summary>
		[Test]
		public void backlogSize_BacklogSizeCalled()
		{
			this.heap.Setup(x => x.getSize()).Verifiable(); 
			this.GetInstance().backlogSize();
			this.heap.Verify(x => x.getSize(), Times.Once); 
		}

		/// <summary>
		/// Ensures when the backlog is empty, null is returned
		/// </summary>
		[Test]
		public void next_BacklogNextCalled()
		{
			this.heap.Setup(x => x.getRoot()).Verifiable();
			this.GetInstance().next();
			this.heap.Verify(x => x.getRoot(), Times.Once);
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private IIncidentBacklogService GetInstance()
		{
			return new IncidentBacklogService(this.heap.Object, this.logger.Object); 
		}
	}
}