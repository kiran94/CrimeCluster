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
		private string dateRegex;

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
		/// The Grading to assign imported incidents 
		/// </summary>
		public IncidentGrading grading { get; set; }

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

		 	String date = row[1];
			String longitude = row[4];
			String latitude = row[5];
		
			DateTime extractedDate;
			Double extractedLongitude;
			Double extractedLatitude;
		
			bool[] validateChecks =
			{
				this.validateDate(date, out extractedDate),
				this.validateLongitudeOrLatitude(longitude, out extractedLongitude),
				this.validateLongitudeOrLatitude(latitude, out extractedLatitude)
			};

			if (validateChecks.All(x => x))
			{
				var incident = new Incident()
				{
					CrimeID = row[0],
					DateCreated = extractedDate,
					ReportedBy = row[2],
					FallsWithin = row[3],
					LocationDesc = row[6],
					LSOACode = row[7],
					LSOAName = row[8],
					CrimeType = validateCrimeType(row[9]).ToString(),
					LastOutcomeCategory = row[10],
					Context = row[11],
					Location = new Location() { DateLogged = extractedDate, Latitude = extractedLatitude, Longitude = extractedLongitude },
					Grading = this.grading
				};

				return incident;
			}

			this.logger.debug(string.Format("Error parsing CSV row: Date {0}, Longitude: {1}, Latitude: {2}",
				validateChecks[0], validateChecks[1], validateChecks[2])); 
			
			return default(Incident); 
		}

		// <inheritdoc>
		public void setDefaultValue(object value)
		{
			this.grading = (IncidentGrading)value;
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

		/// <summary>
		/// Validates the type of the crime to an enumeration.
		/// </summary>
		/// <returns>The crime type.</returns>
		/// <param name="crimeType">Crime type.</param>
		private CrimeType validateCrimeType(string crimeType)
		{
			if (string.IsNullOrEmpty(crimeType))
			{
				this.logger.warn(nameof(crimeType) + " was null or empty");
				return default(CrimeType); 
			}

			var lookup = new Dictionary<string, CrimeType>()
			{
				{"Anti-social behaviour", CrimeType.AntiSocialBehaviour },
				{"Bicycle theft", CrimeType.BicycleTheft },
				{"Burglary", CrimeType.Burglary},
				{"Criminal damage and arson", CrimeType.CriminalDamageandArson},
				{"Drugs", CrimeType.Drugs},
				{"Other crime", CrimeType.OtherCrime},
				{"Other theft", CrimeType.OtherTheft},
				{"Possession of weapons", CrimeType.PossessionOfWeapons},
				{"Public disorder and weapons", CrimeType.PublicDisorderAndWeapons},
				{"Public order", CrimeType.PublicOrder},
				{"Robbery", CrimeType.Robbery},
				{"Shoplifting", CrimeType.Shoplifting},
				{"Theft from the person", CrimeType.TheftFromThePerson},
				{"Vehicle crime", CrimeType.VehicleCrime},
				{"Violence and sexual offences", CrimeType.ViolenceAndSexualOffences},
				{"Violent crime", CrimeType.ViolentCrime},
			};

			if (lookup.ContainsKey(crimeType))
			{
				return lookup[crimeType];
			}

			return default(CrimeType); 
		}
	}
}