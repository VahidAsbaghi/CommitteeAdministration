using System;
using System.Web.Mvc;
using System.Web.Routing;
using CommitteeAdministration.Helper;
using CommitteeManagement.Repository;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.ActionFilters
{
    /// <summary>
    /// Authorize attribute
    /// </summary>
    /// <seealso cref="System.Web.Mvc.AuthorizeAttribute" />
    public class RBACAttribute : AuthorizeAttribute
    {

        /// <summary>
        /// Called when [authorization].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //Create permission string based on the requested controller name and action name in the format 'controllername-action'
            //string requiredPermission = String.Format("{0}-{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName);

            //Create an instance of our custom user authorization object passing requesting user's 'Windows Username' into constructor
            RBACUser requestingUser = new RBACUser(filterContext.RequestContext.HttpContext.User.Identity.Name,ModelContainer.Instance.Resolve<IMainContainer>());
            //Check if the requesting user has the permission to run the controller's action
            if (!requestingUser.HasPermission((Enums.PermissionType)Enum.Parse(typeof (Enums.PermissionType),filterContext.ActionDescriptor.ActionName),null, (Enums.PermissionObject)Enum.Parse(typeof (Enums.PermissionObject),filterContext.ActionDescriptor.ControllerDescriptor.ControllerName)))// & !requestingUser.IsSysAdmin)
            {
                //User doesn't have the required permission and is not a SysAdmin, return our custom “401 Unauthorized” access error
                //Since we are setting filterContext.Result to contain an ActionResult page, the controller's action will not be run.
                //The custom “401 Unauthorized” access error will be returned to the browser in response to the initial request.
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Index" }, { "controller", "Unauthorised" } });
            }
            //If the user has the permission to run the controller's action, then filterContext.Result will be uninitialized and
            //executing the controller's action is dependant on whether filterContext.Result is uninitialized.
        }
    }
}
