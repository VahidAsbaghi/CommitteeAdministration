using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommitteeManagement.Model;

namespace CommitteeAdministration.BaseClasses
{
    public abstract partial class WebBaseController : Controller
    {

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (TempData["ViewData"] != null && !ModelState.Equals(TempData["ViewData"]))
                ModelState.Merge((ModelStateDictionary)TempData["ViewData"]);

            base.OnActionExecuted(filterContext);
        }

        public User CurrentUser
        {
            get
            {
                if (Session["CurrentUser"] != null)
                    return (User)Session["SiteUSER"];
                return null;
            }
        }
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public class ReturnArgs
        {
            public ReturnArgs()
            {
            }

            public int Status { get; set; }
            public string ViewString { get; set; }


        }
    }
}