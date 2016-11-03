namespace com.kiranpatel.crimecluster.webservice.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Mvc.Ajax;

	/// <summary>
	/// Home controller.
	/// </summary>
	public class HomeController : Controller
	{
		/// <summary>
		/// Home Page
		/// </summary>
		public String Index()
		{
			return "Crime Cluster Started."; 
		}
	}
}