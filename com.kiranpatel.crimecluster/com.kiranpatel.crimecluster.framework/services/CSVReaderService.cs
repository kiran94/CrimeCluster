namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using LumenWorks.Framework.IO.Csv;

	/// <summary>
	/// Service for reading CSV Files
	/// </summary>
	public class CSVReaderService : ICSVReaderService
	{
		/// <summary>
		/// The config service instance
		/// </summary>
		private IConfigurationService configService;

		/// <summary>
		/// The logger instance
		/// </summary>
		private ILogger logger;

		/// <summary>
		/// The file IO Service instance
		/// </summary>
		private IFileIOService fileIOService; 

		/// <summary>
		/// The CSV reader instance
		/// </summary>
		private CsvReader reader; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.CSVReaderService"/> class.
		/// </summary>
		/// <param name="configService">Config service.</param>
		/// <param name="logger">Logger.</param>
		public CSVReaderService(IFileIOService fileIOService, IConfigurationService configService, ILogger logger)
		{
			this.fileIOService = fileIOService; 
			this.configService = configService;
			this.logger = logger; 
		}

		// <inheritdoc>
		public ICollection<T> parseCSV<T>(String fileLocation, CSVParseType parseType) where T : EntityBase
		{
			if (String.IsNullOrEmpty(fileLocation))
			{
				this.logger.warn("File Location was null or empty for parsing CSV"); 
				return null; 
			}

			ICSVParseStrategy parseStrategy = getStrategy(parseType);
			ICollection<T> returnSet = new HashSet<T>();

			using (Stream underlyingStream = this.fileIOService.openStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.Read))
			using (TextReader reader = new StreamReader(underlyingStream))
			using (this.reader = new CsvReader(reader, true))
			{
				var headers = this.reader.GetFieldHeaders();

				while (this.reader.ReadNextRecord())
				{
					String[] row = new String[headers.Length];

					for (int i = 0; i < headers.Length; i++)
					{
						row[i] = this.reader[i]; 
					}

					returnSet.Add((T) parseStrategy.parse(row)); 
				}
			}

			return returnSet; 
		}

		/// <summary>
		/// Gets the strategy for parsing the CSV
		/// </summary>
		/// <returns>The strategy type.</returns>
		/// <param name="type">Type of object to parse.</param>
		private ICSVParseStrategy getStrategy(CSVParseType type) 
		{ 
			switch (type)
			{
				case CSVParseType.IncidentParse:
				return new IncidentCSVParseStrategy(this.configService, this.logger); 

				default:
					return null; 
			}
		}

		/// <summary>
		/// Releases all resource used by the <see cref="T:com.kiranpatel.crimecluster.framework.CSVReaderService"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.CSVReaderService"/>. The <see cref="Dispose"/> method leaves
		/// the <see cref="T:com.kiranpatel.crimecluster.framework.CSVReaderService"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="T:com.kiranpatel.crimecluster.framework.CSVReaderService"/> so the garbage collector can reclaim the
		/// memory that the <see cref="T:com.kiranpatel.crimecluster.framework.CSVReaderService"/> was occupying.</remarks>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.reader != null)
				{
					this.reader.Dispose(); 
				}
			}
		}

	}
}
