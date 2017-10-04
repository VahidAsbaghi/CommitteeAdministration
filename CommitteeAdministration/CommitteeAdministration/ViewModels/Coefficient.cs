using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CommitteeAdministration.ViewModels
{
    public  class Coefficient
    {
        [HiddenInput]
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا ضریب را وارد کنید"),Range(0,1,ErrorMessage = "ضریب را در بازه 0 و 1 وارد کنید"),Display(Name = "ضریب")]
        public double CoefficientValue { get; set; }
    }

    public class CriterionCoefficient 
    {
        public List<Coefficient> Coefficients { get; set; }
    }

    public class SubCriterionCoefficient 
    {
        public List<Coefficient> Coefficients { get; set; }
    }

    public class IndicatorCoefficient
    {
        public List<Coefficient> Coefficients { get; set; }
    }
}