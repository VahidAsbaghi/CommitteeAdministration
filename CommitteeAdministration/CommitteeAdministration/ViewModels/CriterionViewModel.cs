using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommitteeManagement.Model;

namespace CommitteeAdministration.ViewModels
{
    public class CriterionViewModel
    {
    }

    public class CreateCriterionViewModel
    {
        [Required(ErrorMessage = "لطفا معیار مورد نظر را وارد کنید"),Display(Name = "عنوان معیار")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "لطفا ضریب را وارد نمایید")][Range(0,1, ErrorMessage = "ضریب را در بازه 0 و 1 وارد کنید"),Display(Name = "ضریب معیار")]
        public double? Coefficient { get; set; }
        public int? CommitteeId { get; set; }
        [ForeignKey("CommitteeId")]
        public virtual Committee Committee { get; set; }
       
    }
}