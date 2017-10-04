using System;

namespace CommitteeAdministration.Services
{
    public class UserUpdateBindingModel
    {
    }
    public class usersReturmModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string TelephonNumber { get; set; }
        public string Address { get; set; }
        
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


    }
}
