using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CommitteeAdministration.Areas.Management.Models
{
    public class UserViewModel
    {
        //public User User { get; set; }
        //public List<Committee> Committees { get; set; }

      
        public string UserId { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
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