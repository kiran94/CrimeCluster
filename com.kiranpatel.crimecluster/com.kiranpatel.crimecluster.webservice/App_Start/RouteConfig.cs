namespace com.kiranpatel.crimecluster.webservice
{
	using System.Web.Mvc;
	using System.Web.Routing;

	/// <summary>
	/// Configuration for MVC Routes
	/// </summary>
	public class RouteConfig
	{
		/// <summary>
		/// Registers the routes.
		/// </summary>
		/// <param name="routes">Routes Collection.</param>
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}