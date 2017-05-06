using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Services;
using CommitteeAdministration.Services.Contract;
using CommitteeAdministration.ViewModels.ConditionViewModels;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.Practices.Unity;
using Chart = System.Web.Helpers.Chart;
using Image = System.Drawing.Image;
using Point = DotNet.Highcharts.Options.Point;

namespace CommitteeAdministration.Controllers
{
    public class CommitteeStatusController : Controller
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private readonly ICommitteeStatus _committeeStatus = ModelContainer.Instance.Resolve<ICommitteeStatus>();
        // GET: CommitteeStatus
        //public ActionResult Index()
        //{
        //    return View();
        //}
       
        public ActionResult StatusMain()
        {
            var model = new MainViewModel()
            {
                Committees = new SelectList(_mainContainer.CommitteeRepository.All(), "Id", "Name")
            };
            return View(model);
        }

        public PartialViewResult MainCriterionCoefficient_PieChart()
        {
            Highcharts chart = new Highcharts("chart")
                .InitChart(new DotNet.Highcharts.Options.Chart { PlotShadow = false })
                .SetTitle(new Title { Text = "Browser market shares at a specific website, 2010" })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' %'; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        AllowPointSelect = true,
                        Cursor = Cursors.Pointer,
                        DataLabels = new PlotOptionsPieDataLabels
                        {
                            Color = ColorTranslator.FromHtml("#000000"),
                            ConnectorColor = ColorTranslator.FromHtml("#000000"),
                            Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' %'; }"
                        }
                    }
                })
                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    Name = "Browser share",
                    Data = new Data(new object[]
                    {
                        new object[] { "Firefox", 45.0 },
                        new object[] { "IE", 26.8 },
                        new Point
                        {
                            Name = "Chrome",
                            Y = 12.8,
                            Sliced = true,
                            Selected = true
                        },
                        new object[] { "Safari", 8.5 },
                        new object[] { "Opera", 6.2 },
                        new object[] { "Others", 0.7 }
                    })
                });
            return PartialView(chart);
        }
        public PartialViewResult CriterionCoefficientJ(int returnCommitteeId)
        {

            return PartialView("CriterionCoefficient_PieChart");
        }
        public PartialViewResult CriterionCoefficients(int returnCommitteeId)//Committee committee)
        {
            var committee = _mainContainer.CommitteeRepository.FindById(returnCommitteeId);
            var model = new CriteriaCoefficientViewModel()
            {
                Criteria =
                    _mainContainer.CriterionRepository.All()
                        .Where(criterionT => criterionT.Committee.Id == committee.Id)
                        .ToList()
            };
            var chartCriteria = new Chart(600, 400, ChartTheme.Blue)
                .AddTitle("Number of website readers")
                .AddLegend()
                .AddSeries(
                    name: "WebSite",
                    chartType: "Pie",
                    xValue: new[] {"Digg", "DZone", "DotNetKicks", "StumbleUpon"},
                    yValues: new[] {"150000", "180000", "120000", "250000"});
            MemoryStream ms = new MemoryStream();

           //model.ChartCriteria=chartCriteria.Save("~/Views/CommitteeStatus/","jpeg");
            WebImage image = chartCriteria.ToWebImage();
            model.WebImage = image;
            return PartialView("CriterionCoefficient_PieChart", model);
        }

        public ActionResult ImageReturn()
        {
            return null;
        }
        public PartialViewResult CriterionStatus(int committeeId)
        {
            var committee =
                _mainContainer.CommitteeRepository.FirstOrDefault(committeeT => committeeT.Id == committeeId);
           // var committeeStatus = new CommitteeStatus();
            var condition = _committeeStatus.GetCriteriaCondition(DateTime.Now,committee);
            var categories = new List<string>();
            var datas = new List<object>();
            foreach (var conditionVar in condition)
            {
                categories.Add(conditionVar.Criterion.Subject);
                datas.Add(conditionVar.Percentage);
            }
            var chart = DrawColumnChart("نمودار وضعیت ستاد" + committee.Name, "درصد کارایی", categories,
                datas);
            return PartialView("CriterionStatus_ColumnChart", chart);
        }

        public PartialViewResult SubCriterionStatus(int criterionId)
        {
            var criterion = _mainContainer.CriterionRepository.FirstOrDefault(criterionT => criterionT.Id == criterionId);
           // var committeeStatus=new CommitteeStatus();
            var condition = _committeeStatus.GetSubCriterionsCondition(criterion, DateTime.Now);
            var categories = new List<string>();
            var datas = new List<object>();
            foreach (var conditionVar in condition)
            {
                categories.Add(conditionVar.SubCriterion.Subject);
                datas.Add(conditionVar.Percentage);
            }
            var chart = DrawColumnChart("نمودار وضعیت معیار" + criterion.Subject, "درصد کارایی", categories,
                datas);
            return PartialView("SubCriterionStatus_ColumnChart",chart);
        }

        public PartialViewResult IndicatorStatus(int subCriterionId)
        {
            
            var subCriterion =
                _mainContainer.SubCriterionRepository.FirstOrDefault(subCriterionT => subCriterionT.Id == subCriterionId);
            //var committeeStatus=new CommitteeStatus();
            var condition= _committeeStatus.IndicatorsPercentage(subCriterion, DateTime.Now);
            var categories=new List<string>();
            var datas=new List<object>();
            foreach (var conditionVar in condition)
            {
                categories.Add(conditionVar.Indicator.Subject);
                datas.Add(conditionVar.ConditionPercentage);
            }
            var chart = DrawColumnChart("نمودار وضعیت زیر معیار" + subCriterion.Subject, "درصد کارایی", categories,
                datas);
            
                
            return PartialView("IndicatorStatus_BarChart", chart);
        }

        public static Highcharts DrawColumnChart(string title,string yAxisTitle,List<string> categories,List<object> datas)
        {
           
            Highcharts chart = new Highcharts("chart")
               .InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = ChartTypes.Column, Margin = new[] {50, 50, 100, 80}})
               .SetTitle(new Title { Text = title })
               .SetXAxis(new XAxis
               {
                   Categories = categories.ToArray(),

                   Labels = new XAxisLabels
                   {
                       Rotation = -45,
                       Align = HorizontalAligns.Right,
                       Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif'",
                       
                   }
               })
               .SetYAxis(new YAxis
               {
                   Min = 0,
                   Title = new YAxisTitle { Text =  yAxisTitle}
               })
               .SetLegend(new Legend { Enabled = false })
               .SetPlotOptions(new PlotOptions
               {
                   Column = new PlotOptionsColumn
                   {
                       DataLabels = new PlotOptionsColumnDataLabels
                       {
                           Enabled = true,
                           Rotation = -90,
                           Color = ColorTranslator.FromHtml("#FFFFFF"),
                           Align = HorizontalAligns.Right,
                           X = 4,
                           Y = 10,
                           Formatter = "function() { return this.y; }",
                           Crop                           = false,
                           Overflow = "Justify",
                           Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif',textShadow: '0 0 3px black'"
                       }
                   }
               })
               .SetSeries(new Series
               {
                   Name = "Percentage",
                   Data = new Data(datas.ToArray()),

               });
            
            return chart;
        }

        public PartialViewResult GetIndicatorRateChart(int indicatorId, int numberOfPoints)
        {
            var indicator = _mainContainer.IndicatorRepository.FirstOrDefault(indicatorT => indicatorT.Id == indicatorId);
            var indicatorRates=_committeeStatus
                .GetIndicatorConditionRate(indicator, numberOfPoints);
            var categories = indicatorRates.Select(indicatorRate => indicatorRate.ConditionTime.Date.ToString(CultureInfo.InvariantCulture)).ToList();
            var datas = indicatorRates.Select(indicatorRate => (object)indicatorRate.ConditionPercentage).ToList();
            //string[] categories =indicatorRate { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
           // object[] tokioData = { 7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6 };
           // object[] londonData = { 3.9, 4.2, 5.7, 8.5, 11.9, 15.2, 17.0, 16.6, 14.2, 10.3, 6.6, 4.8 };

            Highcharts chart = new Highcharts("chart")
                .InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = ChartTypes.Line })
                .SetTitle(new Title { Text = "نرخ کارایی زیرمعیار" })
                //.SetSubtitle(new Subtitle { Text = "Source: WorldClimate.com" })
                .SetXAxis(new XAxis { Categories = categories.ToArray() })
                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "درصد کارایی" } })
                .SetTooltip(new Tooltip { Enabled = true, Formatter = @"function() { return '<b>'+ this.series.name +'</b><br/>'+ this.x +': '+ this.y +'°C'; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Line = new PlotOptionsLine
                    {
                        DataLabels = new PlotOptionsLineDataLabels
                        {
                            Enabled = true
                        },
                        EnableMouseTracking = false
                    }
                })
                .SetSeries(new[]
                {
                    new Series { Name = indicator.Subject, Data = new Data(datas.ToArray()) },
                   
                });
            return PartialView("IndicatorRate_LineChart", chart);
        }

       
    }
}