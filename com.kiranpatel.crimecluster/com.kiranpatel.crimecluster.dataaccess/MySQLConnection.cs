namespace com.kiranpatel.crimecluster.dataaccess
{
	using System;
	using System.Reflection;
	using com.kiranpatel.crimecluster.framework;
	using NHibernate;
	using NHibernate.Cfg;
	using NHibernate.Cfg.MappingSchema;
	using NHibernate.Dialect;
	using NHibernate.Driver;
	using NHibernate.Mapping.ByCode;

	/// <summary>
	/// Entry Point for retreving a MySQL Connection
	/// </summary>
	public class MySQLConnection
	{
		/// <summary>
		/// Configuration Service instance
		/// </summary>
		private readonly IConfigurationService configService;

		/// <summary>
		/// The Session Factory
		/// </summary>
		private ISessionFactory factory;

		/// <summary>
		/// The Nhibernate Configuration
		/// </summary>
		private Configuration config; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.dataaccess.MySQLConnection"/> class.
		/// </summary>
		/// <param name="configService">Config service.</param>
		public MySQLConnection(IConfigurationService configService)
		{
			this.configService = configService; 	
		}

		/// <summary>
		/// Gets a ISession from the factory
		/// </summary>
		/// <returns>The session.</returns>
		private ISession getSession()
		{
			return this.getFactory().OpenSession(); 
		}

		/// <summary>
		/// Gets the SessionFactory
		/// </summary>
		/// <returns>The factory.</returns>
		private ISessionFactory getFactory()
		{
			if (this.factory == null)
			{
				this.factory = this.getConfiguration().BuildSessionFactory(); 
			}

			return this.factory; 
		}

		/// <summary>
		/// Gets the Configuration for the SessionFactory
		/// </summary>
		/// <returns>The configuration.</returns>
		private Configuration getConfiguration()
		{
			if (this.config == null)
			{
				this.config = new Configuration();
				this.config.DataBaseIntegration((db) =>
				{
					db.Dialect<MySQLDialect>();
					db.Driver<MySqlDataDriver>();
					db.ConnectionString = this.configService.GetConnectionString("db"); 
				});
			}

			var mapper = new ModelMapper();
			mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());

			HbmMapping mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
			this.config.AddMapping(mapping);

			return this.config;
		}

	}
}