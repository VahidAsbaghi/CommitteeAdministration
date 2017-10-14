using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CommitteeManagement.Model;

namespace CommitteeAdministration.Areas.UM.ViewModels
{
    public class AdminUserViewModel : UserViewModel
    {
        public AdminUserViewModel() : base()
        {
           
        }
       
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
        
    }
}