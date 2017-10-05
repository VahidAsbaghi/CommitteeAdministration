using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CommitteeAdministration.Areas.UM.ViewModels
{
    public class UserViewModel
    {
      
        public string UserId { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
     
        [Display(Name = "نام ستاد")]
        public SelectList CommitteeName { get; set; }

        public int ReturnedCommitteeId { get; set; }
        [Display(Name = "نام ")]
        public string Name { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string FamilyName { get; set; }
         
    }
}