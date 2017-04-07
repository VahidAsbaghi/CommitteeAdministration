using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using CommitteeManagement.Model;

namespace CommitteeAdministration.ViewModels.ConditionViewModels
{
    public class MainViewModel
    {
        public SelectList Committees { get; set; }
        public int SelectedCommitteeId { get; set; }
    }
    public class CriteriaCoefficientViewModel
    {
        public List<Criterion> Criteria { get; set; }
        public Chart ChartCriteria { get; set; }
        public WebImage WebImage { get; set; }
    }
    public class CriteriaCoefficientViewModel1
    {
        public int Type { get; set; }
    }
}