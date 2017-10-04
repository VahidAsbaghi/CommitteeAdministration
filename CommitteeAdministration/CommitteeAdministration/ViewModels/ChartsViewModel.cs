using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommitteeAdministration.Models.ChartModels;
using CommitteeManagement.Model;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;

namespace CommitteeAdministration.ViewModels
{
    public class ChartsViewModel
    {
       
        [Display(Name = "ستادها")]
        public SelectList Committees { get; set; }
        public int SelectedCommitteeId { get; set; }
        [Display(Name = "مشخصه")]
        public SelectList ChartIndics { get; set; }
        public ChartIndicType SelectedChartIndicType { get; set; }
        public SelectList Charts { get; set; }
        public int SelectedChartId { get; set; }
        public List<DateTime> FromDateTimes { get; set; }
        public List<DateTime> ToDateTimes { get; set; }
        [Display(Name = "معیارها")]
        public SelectList Criteria { get; set; }
        public int[] SelectedCriterionId { get; set; }
        [Display(Name = "زیر معیارها")]
        public SelectList SubCriterions { get; set; }
        public int[] SelectedSubCriterionId { get; set; }
        [Display(Name = "شاخص ها")]
        public SelectList Indicators { get; set; }
        public int[] SelectedIndicatorId { get; set; }
    }

    public class HighChartViewModel
    {
        public Highcharts Highcharts { get; set; }
        public HttpStatusCodeResult CodeResult { get; set; }
    }
    public class SubCriterionChartViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }

    }
    public class CriterionChartViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }

    }
    public class IndicatorChartViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }

    }
}