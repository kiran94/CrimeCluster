using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using com.kiranpatel.crimecluster.framework;

namespace com.kiranpatel.crimecluster.webservice.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ILogger logger = LoggerService.GetInstance();
			IConfigurationService configService = new ConfigurationService();
			String configResult = configService.Get(ConfigurationKey.test, "fault");
			logger.debug(configResult); 


			var mvcName = typeof(Controller).Assembly.GetName();
			var isMono = Type.GetType("Mono.Runtime") != null;

			ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
			ViewData["Runtime"] = isMono ? "Mono" : ".NET";

			return View();
		}
	}
}
