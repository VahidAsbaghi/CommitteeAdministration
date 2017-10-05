using System.ComponentModel.DataAnnotations;

namespace CommitteeAdministration.Controllers
{
    public class EditUserPassword
    {
        [Required(ErrorMessage = "وارد کردن رمز عبور الزامی است")]
        [Display(Name = "رمز عبور قدیمی")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "وارد کردن رمز عبور جدید الزامی است")]
        [Display(Name = "رمز عبور جدید")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "وارد کردن تایید رمز عبور جدید الزامی است")]
        [Display(Name = "تکرار رمز عبور جدید")]
        public string ConfirmNewPassword { get; set; }


    }
}