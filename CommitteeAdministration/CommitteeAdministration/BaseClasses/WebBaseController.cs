using System;
using System.Collections.Generic;
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
    }
}