using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using CommitteeManagement.Model;

namespace CommitteeAdministration.ViewModels
{
    public class SubCriterionViewModel
    {
    }

    public class CreateSubCriterionViewModel
    {
        [Required(ErrorMessage = "لطفا زیر معیار مورد نظر را وارد کنید"), Display(Name = "عنوان زیر معیار")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "لطفا ضریب را وارد نمایید")]
        [Range(0, 1, ErrorMessage = "ضریب را در بازه 0 و 1 وارد کنید"), Display(Name = "ضریب زیر معیار")]
        public double? Coefficient { get; set; }

        public int? CommitteeId { get; set; }
        public virtual Committee Committee { get; set; }

        public int? CriterionId { get; set; }
        public virtual Criterion Criterion { get; set; }
    }
}