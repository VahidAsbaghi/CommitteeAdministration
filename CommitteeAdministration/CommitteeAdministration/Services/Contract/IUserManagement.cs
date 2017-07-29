using System.Collections.Generic;

namespace CommitteeAdministration.Services.Contract
{
    public interface IUserInfoManager
    {
        List<usersReturmModel> GetUsersInfo();
        usersReturmModel GetUserInfo();
        void UpdateUserInfo(int userId, UserUpdateBindingModel userUpdateBindingModel);
    }
}