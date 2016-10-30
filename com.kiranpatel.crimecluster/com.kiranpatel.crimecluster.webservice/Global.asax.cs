namespace com.kiranpatel.crimecluster.webservice
{
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;

	public class Global : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		//https://github.com/ninject/Ninject.Web.Common/wiki/Setting-up-an-IIS-hosted-web-application
	}
}
