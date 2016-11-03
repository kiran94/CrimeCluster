﻿namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq; 
	using System.Text.RegularExpressions;

	/// <summary>
	/// Strategy for parsing CSV files into Incidents
	/// </summary>
	public class IncidentCSVParseStrategy : ICSVParseStrategy
	{
		/// <summary>
		/// Expected number of columns from the CSV file
		/// </summary>
		private int expectedColumns;

		/// <summary>
		/// The date regex validation 
		/// </summary>
		private String dateRegex;

		/// <summary>
		/// The culture for the date
		/// </summary>
		private CultureInfo culture; 

		/// <summary>
		/// The config service instance
		/// </summary>
		private readonly IConfigurationService configService;

		/// <summary>
		/// The logger instance
		/// </summary>
		private readonly ILogger logger; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.IncidentCSVParseStrategy`1"/> class.
		/// </summary>
		/// <param name="configService">Config service.</param>
		public IncidentCSVParseStrategy(IConfigurationService configService, ILogger logger)
		{
			this.configService = configService;
			this.logger = logger; 

			this.dateRegex = this.configService.Get(ConfigurationKey.CSVIncidentDateFormatRegex, @"[0-9]{4}-[0-9]{2}");
			this.culture = new CultureInfo(this.configService.Get(ConfigurationKey.CultureInfo, "en-GB"));
			this.expectedColumns = Int32.Parse(this.configService.Get(ConfigurationKey.CSVIncidentColumnNumber, "12")); 
		}

		// <inheritdoc>
		public object parse(String[] row)
		{
			if (row == null || (row.Length != expectedColumns))
			{
				return default(Incident); 
			}

			DateTime extractedDate;
			Double extractedLongitude;
			Double extractedLatitude;
			String extractedCrimeType = row[9];
			String extractedOutcome = row[10];

			bool[] validateChecks =
			{
				this.validateDate(row[1], out extractedDate),
				this.validateLongitudeOrLatitude(row[4], out extractedLatitude),
				this.validateLongitudeOrLatitude(row[5], out extractedLongitude)
			};

			if (validateChecks.All(x => x))
			{
				var incident = new Incident() { DateCreated = extractedDate, Summary = extractedCrimeType };
				var location = new Location() { DateLogged = extractedDate, Latitude = extractedLatitude, Longitude = extractedLongitude };
				var outcome = new IncidentOutcome() { DateCreated = extractedDate, Outcome = extractedOutcome, Incident = incident };
				var grading = new IncidentGrading() { GradeValue = null, Description = "Imported" };

				incident.Location = location;
				incident.Outcome = new List<IncidentOutcome>() { outcome };
				incident.Grading = grading; 

				return incident;
			}

			this.logger.warn("Error parsing CSV row"); 
			return null; 
		}

		/// <summary>
		/// Validates the parsed date against the configurable regex
		/// </summary>
		/// <returns><c>true</c>, if date was validated, <c>false</c> otherwise.</returns>
		/// <param name="date">Date.</param>
		private bool validateDate(String date, out DateTime toConvert)
		{
			var outDate = new DateTime();

			bool result = Regex.IsMatch(date, this.dateRegex) && DateTime.TryParseExact(date, "yyyy-MM", this.culture, DateTimeStyles.None, out outDate);

			toConvert = outDate; 
			return result; 
		}

		/// <summary>
		/// Validates the longitude or latitude.
		/// </summary>
		/// <returns><c>true</c>, if longitude or latitude was validated, <c>false</c> otherwise.</returns>
		/// <param name="locationVal">Location value.</param>
		/// <param name="toConvert">The converted value</param>
		private bool validateLongitudeOrLatitude(String locationVal, out double toConvert)
		{
			return Double.TryParse(locationVal, out toConvert); 
		}
	}
}