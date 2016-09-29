using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitteeManagement.Model
{
    public class Password
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }
        [Required,MinLength(8,ErrorMessage = "رمز عبور باید بیش از 8 کاراکتر باشد"),MaxLength(100,ErrorMessage = "رمز عبور نباید بیش از 100 کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string PasswordPhrase { get; set; }
        [Display(Name = "رمز عبور")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "تایید رمز عبور یکسان نمیباشد")]
        public string ConfirmPassword { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual User User { get; set; }

    }

}
