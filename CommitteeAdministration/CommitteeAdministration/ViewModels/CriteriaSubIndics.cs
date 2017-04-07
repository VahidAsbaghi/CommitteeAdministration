using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.ViewModels
{
    public enum NavigationViewName
    {
        FirstPartial,
        AddCriterion,
        AddSubCriterion,
        AddIndicator,
        None
    }
    public class AddCriteriaSubCriterionIndicators
    {
        public NavigationViewName NavigationViewName { get; set; }
        public Criterion Criterion { get; set; }
        public SubCriterion SubCriterion { get; set; }
        public List<Indicator> Indicators   { get; set; }
        public Committee Committee { get; set; }
    }

    public class AddCriterionViewModel
    {
        public AddCriterionViewModel()
        {
            
        }
        [Display(Name = "عنوان معیار")]
        [Required(ErrorMessage = "مقدار عنوان معیار وارد شود")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "مقدار ضریب معیار وارد شود"),Range(0,1,ErrorMessage = "ضریب در بازه صفر و یک وارد شود")]
        [Display(Name = "ضریب معیار")]
        public double Coefficient { get; set; }

        [Display(Name = "نام ستاد")]
        public SelectList CommitteeName { get; set; }

        public int ReturnedCommitteeId { get; set; }
    }

    public class AddSubCriterionViewModel
    {
        [Display(Name = "عنوان زیر معیار")]
        [Required(ErrorMessage = "مقدار عنوان زیر معیار وارد شود")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "مقدار ضریب زیر معیار وارد شود"), Range(0, 1, ErrorMessage = "ضریب در بازه صفر و یک وارد شود")]
        [Display(Name = "ضریب زیر معیار")]
        public double Coefficient { get; set; }
        public Criterion Criterion { get; set; }
    }

    public class AddIndicatorViewModel
    {
        [Display(Name = "عنوان شاخص")]
        [Required(ErrorMessage = "مقدار عنوان شاخص وارد شود")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "مقدار ضریب شاخص وارد شود"), Range(0, 1, ErrorMessage = "ضریب در بازه صفر و یک وارد شود")]
        [Display(Name = "ضریب شاخـص")]
        public double Coefficient { get; set; }
        [Display(Name = "مهلت زمانی درج مقادیر واقعی"),DataType(DataType.Duration)]
        [Required(ErrorMessage = "مقدار مهلت زمانی وارد شود")]
        public int? DeadLinePeriod { get; set; }
        public SubCriterion SubCriterion { get; set; }
    }

    public class ViewAllViewModel
    {
        public List<Criterion> CriteriaList { get; set; }
        public Criterion Criterion { get; set; }
        public List<SubCriterion> SubCriterionList { get; set; }
        public List<Indicator> Indicators { get; set; }
        public SelectList Committees { get; set; }
        public int SelectedCommitteeId { get; set; }
    }
    /// <summary>
    /// is used to show as a table to users and give capability of inserting data for real values of indicators
    /// </summary>
    public class IndicatorRealValueViewModel
    {
        public SelectList Committees { get; set; }
        public int SelectedCommitteeId { get; set; }
        public List<Criterion> Criteria { get; set; }
        public List<SubCriterion> SubCriterions { get; set; }
        public List<Indicator> Indicators { get; set; }
        public List<IndicatorIdealValue> IdealValues { get; set; }
        public List<IndicatorRealValue> RealValues { get; set; }
       
    }
}