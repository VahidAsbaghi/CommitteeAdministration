using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CommitteeAdministration.Helper;
using CommitteeAdministration.ViewModels;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Controllers
{
    /// <summary>
    /// ViewProfile and Edit Profile
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class ProfileController : Controller
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        /// <summary>
        /// view profile index controller
        /// </summary>
        /// <returns></returns>
        // GET: Profile
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
            var model = new ViewProfileViewModel()
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
        public async Task<ActionResult> EditProfile(EditProfileViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                bool isNewContactInfo = false;
                var user =
                    _mainContainer.UserRepository.FirstOrDefault(userT => userT.UserName == User.Identity.Name);

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
                return RedirectToAction("ViewProfile");
            }

            return View(userModel);

        }
    }
}