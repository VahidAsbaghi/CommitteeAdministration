using System.ComponentModel.DataAnnotations;

namespace CommitteeAdministration.Areas.UM.ViewModels
{
    public class UserProfileViewModel
    {
   

        [Display(Name = "نام ")]
        public string Name { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string FamilyName { get; set; }

        [Display(Name = "تلفن همراه")]
        public string MobileNumber { get; set; }
        [Display(Name = "تاریخ تولد")]
        public string BirthDay { get; set; }
    }
}