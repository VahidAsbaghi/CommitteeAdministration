using System.Web.Mvc;

namespace CommitteeAdministration.Areas.UM
{
    public class ManagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "UM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Management_default",
                "UM/{controller}/{action}/{id}",
                new { Controller="Manage",action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}