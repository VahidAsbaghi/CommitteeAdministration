using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CommitteeManagement.Model;

namespace CommitteeAdministration.Areas.UM.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            
        }
        public string UserId { get; set; }

        [Required(ErrorMessage = "وارد کردن {0} اجباری است.")]
        [EmailAddress]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "وارد کردن {0} اجباری است.")]
        [StringLength(10, ErrorMessage = "{0} باید حداقل دارای {2} و حداکثر {1} کاراکتر باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        [RegularExpression(@"^((?!.*[\s])(?=.*[aA-zZ])(?=.*\d).{6,10})", ErrorMessage = "{0} شما باید ترکیبی از عدد و حروف باشد.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} اجباری است.")]
        [DataType(DataType.Password)]
        [Display(Name = "تایید رمز عبور")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "تایید رمز عبور با رمز عبور وارد شده یکسان نیست")]
        public string ConfirmPassword { get; set; }
     
        //[Display(Name = "نام ستاد")]
        //public SelectList CommitteeName { get; set; }
        [Required(ErrorMessage = "وارد کردن نام ستاد است.")]
        public int ReturnedCommitteeId { get; set; }

        //select list
        private SelectList comitteename = new SelectList(new List<Committee>());
        [Display(Name = "نام ستاد")]
        public SelectList CommitteeName
        {
            get
            {
                return comitteename;
            }
            set
            {
                comitteename = value;
                //CommitteeName = comitteename;
            }
        }



        //[Display(Name = "نام نقش")]
        //public SelectList RoleName { get; set; }
        [Required(ErrorMessage = "شما باید یک نقش را برای کاربر انتخاب کنید")]
        public int ReturnRoleId { get; set; }


        //select list
        private SelectList rolename = new SelectList(new List<Committee>());
        [Display(Name = "نام نقش")]
        public SelectList RoleName
        {
            get
            {
                return rolename;
            }
            set
            {
                rolename = value;
            }
        }



        [Display(Name = "نام ")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی است.")]
        public string Name { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string FamilyName { get; set; }
         
    }
}