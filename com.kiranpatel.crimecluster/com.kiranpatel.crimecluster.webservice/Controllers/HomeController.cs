using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using com.kiranpatel.crimecluster.framework;
using com.kiranpatel.crimecluster.dataaccess; 

namespace com.kiranpatel.crimecluster.webservice.Controllers
{
	public class HomeController : Controller
	{
		public String Index()
		{
			return "Web Application Started"; 
		}
	}
}