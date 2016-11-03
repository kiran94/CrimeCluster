[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(com.kiranpatel.crimecluster.webservice.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(com.kiranpatel.crimecluster.webservice.App_Start.NinjectWebCommon), "Stop")]

namespace com.kiranpatel.crimecluster.webservice.App_Start
{
	using System;
	using System.Web;
	using com.kiranpatel.crimecluster.dataaccess;
	using com.kiranpatel.crimecluster.framework;
	using Microsoft.Web.Infrastructure.DynamicModuleHelper;
	using NHibernate;
	using Ninject;
	using Ninject.Web.Common;

	/// <summary>
	/// Ninject web common. 
	/// </summary>
	public static class NinjectWebCommon 
    {
		/// <summary>
		/// The bootstrapper.
		/// </summary>
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Register contracts to services
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
			kernel.Bind<ILogger>().ToMethod(x => LoggerService.GetInstance());
			kernel.Bind<IConfigurationService>().To<ConfigurationService>(); 

			kernel.Bind<ISession>()
			      .ToMethod(x => new MySQLConnection(kernel.Get<IConfigurationService>()).getSession())
			      .InRequestScope();
			
			kernel.Bind<IRepository>().To<Repository>();
			kernel.Bind<ISerialisationService>().To<SerialisationService>();

			kernel.Bind<IOfficerService>().To<OfficerService>();
			kernel.Bind<ILocationService>().To<LocationService>();
			kernel.Bind<IIncidentOutcomeService>().To<IncidentOutcomeService>();
			kernel.Bind<IIncidentService>().To<IncidentService>();
			kernel.Bind<IIncidentBacklogService>().To<IncidentBacklogService>(); 


        }        
    }
}