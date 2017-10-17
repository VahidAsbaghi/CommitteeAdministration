using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CommitteeAdministration.Areas.UM.ViewModels;
using CommitteeAdministration.BaseClasses;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Services.Contract;
using CommitteeAdministration.ViewModels;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Areas.UM.Controllers
{
    /// <summary>
    /// ViewProfile and Edit Profile
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class ProfileController : WebBaseController
    {

        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private readonly IUserInfoManager _userInfoManager = ModelContainer.Instance.Resolve<IUserInfoManager>();
        private ApplicationUserManager _userManager;


        public ProfileController()
        {

        }
        public ProfileController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        /// <summary>
        /// view profile index controller
        /// </summary>
        /// <returns></returns>
        // GET: Profile

        [Authorize]
        public ActionResult Index()
        {
            //var z = User.Identity.Name;

            //var users = _userInfoManager.GetUsersInfo();
            //var z2 = HttpContext.User.Identity.GetUserId();
            //User targetUser = null;
            //if (string.IsNullOrWhiteSpace(userId))
            //{
            //    targetUser = UserManager.Users.FirstOrDefault(x => x.Id == userId);
            //    if (targetUser != null)
            //    {
            //        return View();
            //    }
            //}

            //ViewBag.Users = users;
            //Must Return Roles
            if (TempData["ViewData"] != null)
            {
                //ViewData = (ViewDataDictionary)TempData["ViewData"];
                return View();
            }

            if (TempData["UserUpdateProfile"] != null)
            {
                var userUpdatedData = TempData["UserUpdateProfile"] as ProfileViewModel;
                return View(userUpdatedData);
            }

            var userId = HttpContext.User.Identity.GetUserId();
            var targetUser = UserManager.Users.FirstOrDefault(x => x.Id == userId);
            var profileViewModel = new ProfileViewModel()
            {
                Name = targetUser.Name,
                LastName = targetUser.LastName,
                UserName = targetUser.UserName,
                Address2 = targetUser.ContactInfo == null ? "" : targetUser.ContactInfo.Address2,
                Address1 = targetUser.ContactInfo == null ? "" : targetUser.ContactInfo.Address1,
                City = targetUser.ContactInfo == null ? "" : targetUser.ContactInfo.City,
                Region = targetUser.ContactInfo == null ? "" : targetUser.ContactInfo.Region,
                CommitteeName = targetUser.Committee.Name,
                Gender = targetUser.Gender

            };

            return View(profileViewModel);
        }



        [Authorize]
        public ActionResult ViewProfile()
        {
            var userName = User.Identity.Name;
            var userRepository = _mainContainer.UserRepository;
            var user = userRepository.FirstOrDefault(userT => userT.UserName == userName);
            var contactInfo=new ContactInfo();
            if (user.ContactInfo!=null)
            {
                contactInfo = user.ContactInfo;
            }
            var model = new ProfileViewModel()
            {
                Address1 = contactInfo.Address1??"",
                Address2 = contactInfo.Address2??"",
                City = contactInfo.City??"",
                Gender = user.Gender,
                CommitteeName = user.Committee.Name,
                LastName = user.LastName,
                Name = user.Name,
                Region = contactInfo.Region??"",
                UserName = user.UserName
            };
            return View(model);
        }
        /// <summary>
        /// Edits the profile. Get
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditProfile()
        {
            var userName = User.Identity.Name;
            ViewBag.UserNameData = userName;
            return View();
        }
        /// <summary>
        /// Edits the profile. Post
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> EditProfile(ProfileViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                bool isNewContactInfo = false;
                var user = _mainContainer.UserRepository.FirstOrDefault(userT => userT.UserName == User.Identity.Name);

                user.Name = userModel.Name;
                user.LastName = userModel.LastName;
                user.Gender = userModel.Gender;
                var contactInfo = _mainContainer.ContactInfoRepository.FirstOrDefault(cInfo => cInfo.UserId == user.Id);
                if (contactInfo == null)
                {
                    isNewContactInfo = true;
                    contactInfo = new ContactInfo()
                    {
                        User = user,
                        UserId = user.Id
                    };
                }

                contactInfo.Address1 = userModel.Address1;
                contactInfo.Address2 = userModel.Address2;
                contactInfo.City = userModel.City;
                contactInfo.ModifiedTime = DateTime.Now;
                contactInfo.Region = userModel.Region;
                user.ContactInfo = contactInfo;
                if (isNewContactInfo)
                {
                    _mainContainer.ContactInfoRepository.Add(contactInfo);
                }
                else
                {
                    _mainContainer.ContactInfoRepository.Attach(contactInfo);
                }
                _mainContainer.UserRepository.Attach(user);

                await _mainContainer.SaveChangesAsync();
                //return RedirectToAction("ViewProfile");
                //return RedirectToAction("action", "controller", new { area = "area" });
                TempData["UserUpdateProfile"] = userModel;
                return RedirectToAction("Index");
            }
            //TempData["ViewData"] = ViewData;
            TempData["ViewData"] = ModelState;
            return RedirectToAction("Index");

        }
    }
}