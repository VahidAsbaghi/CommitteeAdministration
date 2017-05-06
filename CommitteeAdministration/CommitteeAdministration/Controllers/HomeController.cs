using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using CommitteeAdministration.ViewModels;
using Newtonsoft.Json;

namespace CommitteeAdministration.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            FirstPageViewModel model = new FirstPageViewModel();
           var currentDirectory = HostingEnvironment.ApplicationPhysicalPath;
            //var newsString = System.IO.File.ReadAllText(currentDirectory + @"Resources/Links/FirstPage/News.json");
            //var jsonModel = JsonConvert.DeserializeObject<List<News>>(newsString);
            model.News = new List<News>();
            return View(model);
        }

        public ActionResult About()
        {
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