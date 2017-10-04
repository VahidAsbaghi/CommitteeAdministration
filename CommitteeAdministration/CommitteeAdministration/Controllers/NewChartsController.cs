using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Models.ChartModels;
using CommitteeAdministration.ViewModels;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;

namespace CommitteeAdministration.Controllers
{
    public class NewChartsController : Controller
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private static List<DateTime> _fromDateTimes;
        private static List<DateTime> _todDateTimes; 
        // GET: NewCharts
        public ActionResult Index()
        {
            _fromDateTimes = null;
            _todDateTimes = null;
            var chartsViewModel = new ChartsViewModel
            {
                Committees = new SelectList(_mainContainer.CommitteeRepository.All(), "Id", "Name")
            };
            return View(chartsViewModel);
        }

        public ActionResult ReturnCommitteeId(int committeeId)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }
            var subCriterions = _mainContainer.SubCriterionRepository.Where(
                subCriterion => subCriterion.Committee.Id == committeeId);
            _fromDateTimes = null;
            _todDateTimes = null;
            var chartsViewModel = new ChartsViewModel
            {
                Committees = new SelectList(_mainContainer.CommitteeRepository.All()),
                SelectedCommitteeId = committeeId,
                Criteria =
                    new SelectList(
                        _mainContainer.CriterionRepository.Where(criterion => criterion.Committee.Id == committeeId),
                        "Id", "Subject"),
                SubCriterions =new SelectList(new List<SubCriterionChartViewModel>(),"Id","Subject"),
               
                Indicators =
                    new SelectList(
                        new List<IndicatorChartViewModel>(),
                        "Id", "Subject"),
                Charts = new SelectList(
                    new List<Chart>
                    {
                        new IndicatorChangeMultiPeriod(),
                        new IndicatorChangesOnePeriod(),
                        new IndicatorsChangeOnePeriod(),
                        new SubCriterionChangeOnePeriod(),
                        new SubCriterionChangeMultiPeriod(),
                        new SubCriteriaChangeOnePeriod(),
                        new CriteriaChangeOnePeriod(),
                        new CriterionChangeMultiPeriod(),
                        new CriterionChangeOnePeriod(),
                        new IndicatorCoefficients(),
                        new CriterionCoefficients(),
                        new SubCriterionCoefficients()
                    }, "Id", "ChartNameString")
            };

            return PartialView("FillDataPartial",chartsViewModel);
        }

        public ActionResult DrawChart(ChartsViewModel chartsViewModel1,string indicType,string chartType)
        {
            var chartsViewModel = (chartsViewModel1);// (ChartsViewModel)JsonConvert.DeserializeObject(chartsViewModel1.ToString());
            //var chartsViewModel = (ChartsViewModel) chartsViewModel1;
            if (ModelState.IsValid)
            {
                var selectedChart = (Chart)Chart.AllCharts[chartsViewModel.SelectedChartId];
                chartsViewModel.FromDateTimes = _fromDateTimes;
                chartsViewModel.ToDateTimes = _todDateTimes;
                ChartIndicType indicTypeEnum;
                if (!Enum.TryParse(indicType, true,out indicTypeEnum))
                {
                    return PartialView(new HighChartViewModel() {CodeResult = CustomHttpStatusCodes.InvalidSecondParameter,Highcharts = null});
                }
                ChartType typeEnum;
                if (!Enum.TryParse(chartType, true, out typeEnum))
                {
                    return PartialView(new HighChartViewModel() { CodeResult = CustomHttpStatusCodes.InvalidThirdParameter, Highcharts = null });
                }
                chartsViewModel.SelectedChartIndicType = indicTypeEnum;
                var chart = Chart.Draw(selectedChart, chartsViewModel, typeEnum);
                if (chart==null)
                {
                    return PartialView(new HighChartViewModel() { CodeResult = new HttpStatusCodeResult(HttpStatusCode.InternalServerError), Highcharts = null });
                }
                return
                    PartialView(new HighChartViewModel()
                    {
                        CodeResult = new HttpStatusCodeResult(HttpStatusCode.OK),
                        Highcharts = chart
                    });
            }
            return PartialView(new HighChartViewModel() { CodeResult =new HttpStatusCodeResult(HttpStatusCode.BadRequest), Highcharts = null });
            
        }
        /// <summary>
        /// Returns the selected date time.
        /// </summary>
        /// <param name="fromDateTime">From date time.</param>
        /// <param name="toDateTime">To date time.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostSelectedDateTime(string fromDateTime, string toDateTime)
        {
            if (_fromDateTimes==null)
            {
                _fromDateTimes=new List<DateTime>();
            }
            if (_todDateTimes==null)
            {
                _todDateTimes=new List<DateTime>();
            }
            DateTime fDateTime;
            var persianCulture = new CultureInfo("fa-IR");
            fromDateTime = fromDateTime.ConvertPersianToEnglish();
            if (DateTime.TryParseExact(fromDateTime,
                "yyyy/MM/dd H:mm:ss", persianCulture, DateTimeStyles.None, out fDateTime))
            {
                
                 toDateTime = toDateTime.ConvertPersianToEnglish();
                DateTime tDateTime;
                if (DateTime.TryParseExact(toDateTime,
                    "yyyy/MM/dd", persianCulture, DateTimeStyles.None, out tDateTime))
                {
                    _fromDateTimes.Add(fDateTime);
                    _todDateTimes.Add(tDateTime);
                }
                else
                {
                    return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
                }
            }
            else
            {
                return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
            
            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
        }
        [HttpPost]
        public ActionResult GetChart(int selectedChartId)
        {
            var selectedChart = (Chart)Chart.AllCharts[selectedChartId];
            return Json(selectedChart);
        }
        [HttpPost]
        public JsonResult GetSubCriteria(int selectedCriterionId)
        {
            var subCriteria =
               (_mainContainer.SubCriterionRepository.Where(
                    subCriterion =>
                        subCriterion.Criterion.Id == selectedCriterionId && subCriterion.IsDeleted.Value == false).ToList());
            var resultSubCriteria= subCriteria.Select(subCriterion => new SubCriterionChartViewModel() {Id = subCriterion.Id, Subject = subCriterion.Subject}).ToList();
            
            return Json(resultSubCriteria);
        }
        [HttpPost]
        public JsonResult GetIndicators(int selectedSubCriterionId)
        {
            var indicators =
               (_mainContainer.IndicatorRepository.Where(
                    indicator =>
                        indicator.SubCriterion.Id == selectedSubCriterionId && (!indicator.IsDeleted.Value||indicator.IsDeleted==null)).ToList());
            var resultIndicator = indicators.Select(indicator => new IndicatorChartViewModel() { Id = indicator.Id, Subject = indicator.Subject }).ToList();

            return Json(resultIndicator);
        }
    }
   

}