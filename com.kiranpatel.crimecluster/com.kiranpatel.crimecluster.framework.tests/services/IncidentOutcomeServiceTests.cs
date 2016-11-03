namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using NUnit.Framework;
	using Moq;

	/// <summary>
	/// Tests for the <see cref="IncidentOutcome"/> class
	/// </summary>
	[TestFixture]
	public class IncidentOutcomeServiceTests
	{
		/// <summary>
		/// The repository instance
		/// </summary>
		private Mock<IRepository> repository;

		/// <summary>
		/// The logger instance
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
		/// Ensures when the null incident outcome is null, false is returned
		/// </summary>
		[Test]
		public void validate_nullIncidentOutcome_False()
		{
			Assert.False(this.GetInstance().validate((IncidentOutcome)null)); 
		}

		/// <summary>
		/// Ensures when the date created is null, false is returned
		/// </summary>
		[Test]
		[TestCase("")]
		[TestCase(null)]
		public void validate_OutcomeNullEmpty_False(String param)
		{
			var outcome = new IncidentOutcome()
			{
				Outcome = param,
				Incident = new Incident(),
				Officer = new Officer()
			};

			Assert.False(this.GetInstance().validate(outcome)); 
		}

		/// <summary>
		/// Ensures when the officer is null, false is returned
		/// </summary>
		[Test]
		public void validate_NullOfficer_False()
		{
			var outcome = new IncidentOutcome()
			{
				Outcome = "outcome",
				Incident = null,
				Officer = new Officer()
			};

			Assert.False(this.GetInstance().validate(outcome));
		}

		/// <summary>
		/// Ensures when the outcome is valid, true is returned
		/// </summary>
		[Test]
		public void validate_ValidOutcome_True()
		{
			var outcome = new IncidentOutcome()
			{
				Outcome = "outcome",
				Incident = new Incident(),
				Officer = new Officer()
			};

			Assert.True(this.GetInstance().validate(outcome));
		}


		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private IncidentOutcomeService GetInstance()
		{
			return new IncidentOutcomeService(this.repository.Object, this.logger.Object); 
		}
	}
}