namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using System.Linq; 
	using NUnit.Framework;
	using Moq;
	using com.kiranpatel.crimecluster.framework; 

	/// <summary>
	/// Tests for the Incident CSV Parse Strategy 
	/// </summary>
	[TestFixture]
	public class IncidentCSVParseStrategyTests
	{
		/// <summary>
		/// The config service mock
		/// </summary>
		private Mock<IConfigurationService> configService;

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
			this.configService = new Mock<IConfigurationService>();
			this.configService.Setup(x => x.Get(ConfigurationKey.CSVIncidentDateFormatRegex, @"[0-9]{4}-[0-9]{2}")).Returns(@"[0-9]{4}-[0-9]{2}");
			this.configService.Setup(x => x.Get(ConfigurationKey.CultureInfo, "en-GB")).Returns("en-GB");
			this.configService.Setup(x => x.Get(ConfigurationKey.CSVIncidentColumnNumber, "12")).Returns("12"); 

			this.logger = new Mock<ILogger>(); 
		}

		/// <summary>
		/// Ensures when the row passed is null, null is returned
		/// </summary>
		[Test]
		public void parse_NullRow_Null()
		{
			String[] row = null;
			var result = this.GetInstance().parse(row);

			Assert.IsNull(result);
		}

		/// <summary>
		/// Ensures when a row array is passed with the incorrect number of rows, null is returned
		/// </summary>
		[Test]
		public void parse_IncorrectNumberOfColumns_Null()
		{
			String[] row = { "test", "test" };
			var result = this.GetInstance().parse(row);

			Assert.IsNull(result);
		}

		/// <summary>
		/// Ensures when parse is passed a date with an incorrect date format, null is returned
		/// </summary>
		[Test]
		public void parse_IncorrectDateFormat_NullReturned()
		{
			String[] rows = { "ID", "20010-09", "Reported", "Falls Within", "1.0", "1.0", "Location", "LSOA Code", "LSOA Name", "Crime Type", "Outcome", "context" };
			var result = this.GetInstance().parse(rows);
			Assert.IsNull(result);
		}

		/// <summary>
		/// Ensures when parse is passed an incorrect latitude, null is returned
		/// </summary>
		[Test]
		public void parse_IncorrectLatitude_NullReturned()
		{
			String[] rows = { "ID", "2001-09", "Reported", "Falls Within", "invalidLatitude", "1.0", "Location", "LSOA Code", "LSOA Name", "Crime Type", "Outcome", "context" };
			var result = this.GetInstance().parse(rows);
			Assert.IsNull(result);
		}

		/// <summary>
		/// Ensures when parse is passed an incorrect longitude, null is returned
		/// </summary>
		[Test]
		public void parse_IncorrectLongitude_NullReturned()
		{
			String[] rows = { "ID", "2009-01", "Reported", "Falls Within", "1.0", "invalidLongitude", "Location", "LSOA Code", "LSOA Name", "Crime Type", "Outcome", "context" };
			var result = this.GetInstance().parse(rows);
			Assert.IsNull(result);
		}

		/// <summary>
		/// Ensures when all the correct values are passed, they are translated into an Incident object
		/// </summary>
		[Test]
		public void parse_CorrectValues_ParsedIncident()
		{
			String[] rows = { "ID", "2009-01", "Reported", "Falls Within", "1.0", "2.0", "Location", "LSOA Code", "LSOA Name", "Crime Type", "Outcome", "context" };

			var parser = this.GetInstance(); 
			Incident result = (Incident)parser.parse(rows);

			Assert.NotNull(result);
			Assert.AreEqual(new DateTime(2009, 01, 01), result.DateCreated);
			StringAssert.AreEqualIgnoringCase("Reported", result.ReportedBy); 
			StringAssert.AreEqualIgnoringCase("Falls Within", result.FallsWithin);
			StringAssert.AreEqualIgnoringCase("Location", result.LocationDesc);
			StringAssert.AreEqualIgnoringCase("LSOA Code", result.LSOACode);
			StringAssert.AreEqualIgnoringCase("LSOA Name", result.LSOAName);
			StringAssert.AreEqualIgnoringCase("Crime Type", result.CrimeType);
			StringAssert.AreEqualIgnoringCase("Outcome", result.LastOutcomeCategory);
			StringAssert.AreEqualIgnoringCase("context", result.Context);

			Assert.NotNull(result.Location);
			Assert.AreEqual(2.0D, result.Location.Latitude); 
			Assert.AreEqual(1.0D, result.Location.Longitude);
			Assert.AreEqual(new DateTime(2009, 01, 01), result.Location.DateLogged);

			Assert.NotNull(result.Grading);
			StringAssert.AreEqualIgnoringCase("Imported", result.Grading.Description);
			Assert.IsNull(result.Grading.GradeValue); 
		}

		/*
		 * 
		 * 	0 ID 
			1 Month : yyyy-MM -> DateCreated
			2 Reported By
			3 Falls Within 
			4 Longitude -> Location
			5 Latitude  -> Location
			6 Location 
			7 LSOA Code 
			8 LSOA Name 
			9 Crime Type -> Summary
			10 Last Outcome Category -> Outcome -> Summary
			11 Context 
		 */


		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private IncidentCSVParseStrategy GetInstance()
		{
			return new IncidentCSVParseStrategy(this.configService.Object, this.logger.Object); 
		}
	}
}