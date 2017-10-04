using System.Collections.Generic;
using System.Linq;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Services.Contract;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Services
{
 
    public class UserInfoManager : IUserInfoManager
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        public UserInfoManager()
        {

        }

        public List<usersReturmModel> GetUsersInfo()
        {
            var returnedUser = new List<usersReturmModel>();

            var queryable = _mainContainer.UserRepository.All();
            foreach (var item in queryable)
            {
                returnedUser.Add(item: new usersReturmModel()
                {
                    Email = item.Email,
                    FirstName = item.Name,
                    LastName = item.LastName,
                    Address = item.ContactInfo == null ? "" : item.ContactInfo.Address1,
                    TelephonNumber = item.PhoneNumber,
                    IsActive = item.IsActive ?? true,
                    UserName = item.UserName,
                    UserId = item.Id
                });
            }
            return returnedUser;


        }

        public usersReturmModel GetUserInfo()
        {
            return null;

        }
        public void UpdateUserInfo(int userId, UserUpdateBindingModel userUpdateBindingModel)
        {

        }





    }
}