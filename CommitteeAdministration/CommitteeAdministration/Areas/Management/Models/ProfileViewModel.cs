using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CommitteeAdministration.Areas.Management.Models
{
    public class ProfileViewModel
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