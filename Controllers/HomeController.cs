using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YellowAntDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Console.Write("Hello");
            return View();
        }

        public ActionResult About()
        {
            Console.Write("sup?");
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
    }
}