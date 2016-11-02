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
		/// The logger mock
		/// </summary>
		private Mock<ILogger> logger; 

		/// <summary>
		/// Sets up.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			this.logger = new Mock<ILogger>(); 
		}

		/// <summary>
		/// Ensures when an incident is passed, is it added to the backlog
		/// </summary>
		[Test]
		public void add_Incident_Added()
		{
			Incident incident = new Incident();

			var service = this.GetInstance();
			service.add(incident);

			var result = service.next();

			Assert.AreSame(incident, result); 
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private IIncidentBacklogService GetInstance()
		{
			return new IncidentBacklogService(this.logger.Object); 
		}
	}
}