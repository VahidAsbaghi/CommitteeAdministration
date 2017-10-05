using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CommitteeAdministration.Areas.UM.ViewModels;
using CommitteeAdministration.BaseClasses;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Services.Contract;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Areas.UM.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    public class ManageController : WebBaseController
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private readonly IUserInfoManager _userInfoManager = ModelContainer.Instance.Resolve<IUserInfoManager>();
        private ApplicationUserManager _userManager;

        public ManageController()
        {

        }
        public ManageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public ActionResult Index()
        {
            var users = _userInfoManager.GetUsersInfo();
            //ViewBag.Users = users;
            //Must Return Roles
            return View(users);
        }



        public ActionResult CreateUser()
        {

            return null;
        }


        [HandleError]
        public ActionResult CreateUserPartail(string UserId)
        {
            UserViewModel userViewModel = new UserViewModel();
            var listData = _mainContainer.CommitteeRepository.All().ToList();
            var user = UserManager.FindByIdAsync(UserId).Result;
            if (user != null)
            {
                userViewModel.Name = user.Name;
                userViewModel.FamilyName = user.LastName;
                userViewModel.UserId = user.Id;
                userViewModel.Email = user.Email;
                //userViewModel.
                userViewModel.UserId = user.Id;
                if (user.CommitteeRefId != null) userViewModel.ReturnedCommitteeId = user.CommitteeRefId.Value;

            }
            userViewModel.CommitteeName = new SelectList(listData, "Id ", "Name");
            return View("UserPartial", userViewModel);
        }

        [HandleError]
        //[ValidateInput(false)]
        public async Task<ActionResult> SaveUser(UserViewModel userModel)
        {

            if (!ModelState.IsValid)
            {
                // Re-assign select list if returning the view
                var listData = _mainContainer.CommitteeRepository.All().ToList();
                userModel.CommitteeName = new SelectList(listData, "Id ", "Name");
                return PartialView("UserPartial", userModel);
            }

            var user = new User
            {
                UserName = userModel.Email,
                Email = userModel.Email,
                CommitteeRefId = userModel.ReturnedCommitteeId,
                Committee = _mainContainer.CommitteeRepository.FindById(userModel.ReturnedCommitteeId)
            };
            IdentityResult result;
            try
            {
                result = await UserManager.CreateAsync(user, userModel.Password);
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
                throw;
            }
            if (result.Succeeded)
            {

                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                return RedirectToAction("Index", "Home");
            }
            //AddErrors(result);

            userModel.CommitteeName = (SelectList)_mainContainer.CommitteeRepository.All();
            // If we got this far, something failed, redisplay form
            //return System.Web.UI.WebControls.View(model);
            return RedirectToAction("index", "Management");

        }


    }

}
