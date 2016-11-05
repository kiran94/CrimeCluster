
namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using System.Linq; 
	using NUnit.Framework;
	using Moq;
	using System.IO;

	/// <summary>
	/// Tests for the CSV Reader Service
	/// </summary>
	[TestFixture]
	public class CSVReaderServiceTests
	{
		/// <summary>
		/// The file IO Service mock
		/// </summary>
		private Mock<IFileIOService> fileIOService;

		/// <summary>
		/// The config service mock
		/// </summary>
		private Mock<IConfigurationService> configService;

		/// <summary>
		/// The logger service mock
		/// </summary>
		private Mock<ILogger> loggerService;

		/// <summary>
		/// The strategy mock
		/// </summary>
		private ICSVParseStrategy strategy; 

		/// <summary>
		/// The header of a test CSV file
		/// </summary>
		private String header = @"Crime ID,Month,Reported by,Falls within,Longitude,Latitude,Location,LSOA code,LSOA name,Crime type,Last outcome category,Context";

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

			this.fileIOService = new Mock<IFileIOService>();
			this.loggerService = new Mock<ILogger>();
			this.strategy = new IncidentCSVParseStrategy(this.configService.Object, this.loggerService.Object);
		}

		/// <summary>
		/// Ensures when null or empty is passed as file location, null is returned
		/// </summary>
		/// <param name="testCase">null or empty string</param>
		[TestCase(null)]
		[TestCase("")]
		public void parseCSV_NullOrEmptyFileLocation_Null(String testCase)
		{
			var result = this.GetInstance().parseCSV<Incident>(testCase, CSVParseType.IncidentParse, this.strategy, true);
			Assert.Null(result); 
		}

		/// <summary>
		/// Ensures when the data from the stream is empty, an empty collection is returned
		/// </summary>
		[Test]
		public void parseCSV_EmptyData_EmptyCollection()
		{
			String correctLine = String.Empty; 
			String fileLocation = "test.csv";
			using (MemoryStream stream = new MemoryStream())
			using (StreamWriter writer = new StreamWriter(stream))
			{
				writer.WriteLine(this.header);
				writer.WriteLine(correctLine);
				writer.Flush();

				stream.Seek(0, SeekOrigin.Begin);

				this.fileIOService.Setup(x => x.openStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.Read)).Returns(stream);

				var service = this.GetInstance();
				var result = service.parseCSV<Incident>(fileLocation, CSVParseType.IncidentParse, this.strategy, true);

				CollectionAssert.IsEmpty(result); 
			}
		}

		/// <summary>
		/// Ensures when there are less headers then expected, an empty collection is returned
		/// </summary>
		[Test]
		public void parseCSV_LessHeaders_EmptyCollection()
		{
			String correctLine = @"d7f9065ce1efe175d44ea54604d6c9a9023a34aef0a936635c9b35eea78da170,City of London Police,City of London Police,-0.113767,51.517372,On or near Stone Buildings,E01000914,Camden 028B,Bicycle theft,Under investigation,";
			String fileLocation = "test.csv";
			using (MemoryStream stream = new MemoryStream())
			using (StreamWriter writer = new StreamWriter(stream))
			{
				writer.WriteLine(@"Crime ID,Month,Reported by,Falls within,Latitude,Location,LSOA code,LSOA name,Crime type,Last outcome category,Context");
				writer.WriteLine(correctLine);
				writer.Flush();

				stream.Seek(0, SeekOrigin.Begin);

				this.fileIOService.Setup(x => x.openStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.Read)).Returns(stream);

				var service = this.GetInstance();
				var result = service.parseCSV<Incident>(fileLocation, CSVParseType.IncidentParse, this.strategy, true);

				CollectionAssert.IsEmpty(result);
			}
		}

		/// <summary>
		/// Ensures when there is a data with incorrect formated data, it is not added to the collection
		/// </summary>
		[Test]
		public void parseCSV_IncorrectData_DateWrongFormat_NotAddedToCollection()
		{
			String correctLine = @"d7f9065ce1efe175d44ea54604d6c9a9023a34aef0a936635c9b35eea78da170,2016-07676633,City of London Police,City of London Police,-0.113767,51.517372,On or near Stone Buildings,E01000914,Camden 028B,Bicycle theft,Under investigation,";
			String fileLocation = "test.csv";
			using (MemoryStream stream = new MemoryStream())
			using (StreamWriter writer = new StreamWriter(stream))
			{
				writer.WriteLine(this.header);
				writer.WriteLine(correctLine);
				writer.Flush();

				stream.Seek(0, SeekOrigin.Begin);

				this.fileIOService.Setup(x => x.openStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.Read)).Returns(stream);

				var service = this.GetInstance();
				var result = service.parseCSV<Incident>(fileLocation, CSVParseType.IncidentParse, this.strategy, true);

				CollectionAssert.IsEmpty(result);
			}
		}

		/// <summary>
		/// Ensures when no location is given, no collection is added
		/// </summary>
		[Test]
		public void parseCSV_IncorrectData_NoLatitudeGiven_NotAddedToCollection()
		{
			String correctLine = @"d7f9065ce1efe175d44ea54604d6c9a9023a34aef0a936635c9b35eea78da170,2016-07676633,City of London Police,City of London Police,,51.517372,On or near Stone Buildings,E01000914,Camden 028B,Bicycle theft,Under investigation,";
			String fileLocation = "test.csv";
			using (MemoryStream stream = new MemoryStream())
			using (StreamWriter writer = new StreamWriter(stream))
			{
				writer.WriteLine(this.header);
				writer.WriteLine(correctLine);
				writer.Flush();

				stream.Seek(0, SeekOrigin.Begin);

				this.fileIOService.Setup(x => x.openStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.Read)).Returns(stream);

				var service = this.GetInstance();
				var result = service.parseCSV<Incident>(fileLocation, CSVParseType.IncidentParse, this.strategy, true);

				CollectionAssert.IsEmpty(result);
			}
		}

		/// <summary>
		/// Ensures when there is data in the correct format and headers are expected, they are added to collection
		/// </summary>
		[Test]
		public void parseCSV_NormalCase_AddedToCollection()
		{
			String correctLine = @"d7f9065ce1efe175d44ea54604d6c9a9023a34aef0a936635c9b35eea78da170,2016-07,City of London Police,City of London Police,-0.113767,51.517372,On or near Stone Buildings,E01000914,Camden 028B,Bicycle theft,Under investigation,";
			String fileLocation = "test.csv"; 
			using (MemoryStream stream = new MemoryStream())
			using (StreamWriter writer = new StreamWriter(stream))
			{
				writer.WriteLine(this.header);
				writer.WriteLine(correctLine);
				writer.Flush();

				stream.Seek(0, SeekOrigin.Begin); 

				this.fileIOService.Setup(x => x.openStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.Read)).Returns(stream);

				var service = this.GetInstance();
				var result = service.parseCSV<Incident>(fileLocation, CSVParseType.IncidentParse, this.strategy, true);

				Assert.NotNull(result);
				Assert.AreEqual(1, result.Count);

				var resultIncident = result.First(); 

				Assert.AreEqual(new DateTime(2016, 07, 01), resultIncident.DateCreated);
				Assert.AreEqual(Double.Parse("51.517372"), resultIncident.Location.Latitude); 
				Assert.AreEqual(Double.Parse("-0.113767"), resultIncident.Location.Longitude);
				Assert.AreEqual("Bicycle theft", resultIncident.CrimeType);
				Assert.AreEqual("Under investigation", resultIncident.LastOutcomeCategory); 
			}
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private CSVReaderService GetInstance()
		{
			return new CSVReaderService(this.fileIOService.Object, this.configService.Object, this.loggerService.Object); 
		}
	}
}
