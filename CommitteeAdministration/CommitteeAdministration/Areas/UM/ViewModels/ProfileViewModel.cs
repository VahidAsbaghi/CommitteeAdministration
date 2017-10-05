using System.ComponentModel.DataAnnotations;
using CommitteeManagement.Model;

namespace CommitteeAdministration.Areas.UM.ViewModels
{
    /// <summary>
    /// this model is used to show user profile to user
    /// </summary>
    public class ProfileViewModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Display(Name = "نام ")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        [Display(Name = "جنسیت")]
        public GenderEnum Gender { get; set; }
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        [Display(Name = "شهر")]
        public string City { get; set; }
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        [Display(Name = "استان")]
        public string Region { get; set; }
        /// <summary>
        /// Gets or sets the address1.
        /// </summary>
        /// <value>
        /// The address1.
        /// </value>
        [Display(Name = "آدرس پیش فرض")]
        [Required(ErrorMessage = "وارد کردن یک آدرس برای هر کاربر الزامی است")]
        public string Address1 { get; set; }
        /// <summary>
        /// Gets or sets the address2.
        /// </summary>
        /// <value>
        /// The address2.
        /// </value>
        [Display(Name = "آدرس دوم")]
        public string Address2 { get; set; }
        /// <summary>
        /// Gets or sets the name of the committee.
        /// </summary>
        /// <value>
        /// The name of the committee.
        /// </value>
        [Display(Name = "نام ستاد")]
        public string CommitteeName { get; set; }
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

    }
   
}