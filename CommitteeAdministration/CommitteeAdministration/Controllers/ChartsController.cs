using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Services;
using CommitteeAdministration.Services.Contract;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.Ajax.Utilities;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Controllers
{
   
    public class ChartsController : Controller
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private readonly IChartDrawer _chartDrawer = ModelContainer.Instance.Resolve<IChartDrawer>();


        public ActionResult sample()
        {
            return View();
        }

        public async Task<PartialViewResult> DrawFirstChart(string[] fromDates, string[] toDates,int[] indics,string type,string chartType,int draw)
        {
            ChartDrawer chartDrawer=new ChartDrawer();
            var fromDatesList = fromDates[0].Split(',');
            var toDatesList = toDates[0].Split(',');
            var indic = indics[0];
          
            if (type=="Indicator")
            {
                IQueryable<double> values;
                IQueryable<IndicatorRealValue> realValues;
                if (chartType == IndicatorChart.RealValuesChangesByTimeColumn.ToString())
                {
                    var fromDate = fromDates[0];
                    var toDate = toDates[0];
                    var englishFrom=fromDate.Convert();
                    var englishTo = toDate.Convert();
                    realValues =
                        _mainContainer.IndicatorRealValueRepository.All()
                            .Where(
                               realValue =>realValue.Indicator.Id == indic);
                     values= realValues.Where(realValue=>realValue.Time >= englishFrom &&
                                             realValue.Time <= englishTo).Select(realValueT=>realValueT.Value.Value);
                    var valuesObject=new List<object>();
                    foreach (var value in values)
                    {
                        valuesObject.Add(value);
                    }
                    //var chart = chartDrawer.DrawColumnChart("نمودار ستونی تغییرات مقدار واقعی شاخص بر اساس زمان",
                    //    "مقدار واقعی", new List<string>() {"مقدار واقعی"}, valuesObject);
                    return PartialView("_BarChart", null);

                }
                else if (chartType==IndicatorChart.RealValuesChangesByMultipleTimesColumn.ToString())
                {
                    var indicatorNull = _mainContainer.IndicatorRepository.Where(indicator => indicator.Id == indic)
                        .FirstOrDefault();
                    if (indicatorNull != null)
                    {
                        var columnChartDataModel = new ColumnChartDataModel
                        {
                            DataSeriesList = new List<List<object>>(),
                            Categories = new List<string>(),
                            SeriesNameList = new List<string>(),
                            CahrtName = "columnChart",
                            MainTitle = "نمودار تغییرات یک شاخص در چند بازه زمانی",
                            SubMainTitle = indicatorNull.Subject,
                            YAxisTitle = "مقدار واقعی"
                        };

                        for (int i = 0; i < fromDatesList.Length; i++)
                        {
                            var englishFrom = fromDatesList[i].Convert();
                            var englishTo = toDatesList[i].Convert();                     
                            realValues =
                                _mainContainer.IndicatorRealValueRepository.All()
                                    .Where(
                                        realValue => realValue.Indicator.Id == indic);
                            values = realValues.Where(realValue => realValue.Time >= englishFrom &&
                                                                       realValue.Time <= englishTo).Select(realValueT => realValueT.Value.Value);
                            var valuesObject = new List<object>();
                            foreach (var value in values)
                            {
                                valuesObject.Add(value);
                            
                            }
                            columnChartDataModel.DataSeriesList.Add(valuesObject);
                            columnChartDataModel.SeriesNameList.Add("از "+ fromDatesList[i]+"تا "+ toDatesList[i]);
                        
                        }
                        //var chart = chartDrawer.BasicColumn(columnChartDataModel);
                        return PartialView("_BarChart", null);
                    }
                }
            }
            else if (type=="SubCriterion")
            {
                IQueryable<double> values;
                IQueryable<IndicatorRealValue> realValues;
                if (chartType==SubCriterionChart.ChangesByTimeColumn.ToString())
                {
                    var fromDate = fromDates[0];
                    var toDate = toDates[0];
                    var englishFrom = fromDate.Convert();
                    var englishTo = toDate.Convert();
                    var subCriterion =
                        _mainContainer.SubCriterionRepository.FirstOrDefault(subCriterionT => subCriterionT.Id == indic);
                    var indicators =
                        _mainContainer.IndicatorRepository.Where(indicator => indicator.SubCriterionId.Value == indic);
                    //TODO : REAL VALUES VALUE???
                    realValues = null;
                      //_mainContainer.IndicatorRealValueRepository.All()
                      //    .Where(
                      //       realValue => realValue.Indicator.Id == indic);
                    values = realValues.Where(realValue => realValue.Time >= englishFrom &&
                                              realValue.Time <= englishTo).Select(realValueT => realValueT.Value.Value);
                    var valuesObject = new List<object>();
                    foreach (var value in values)
                    {
                        valuesObject.Add(value);
                    }
                    //var chart = chartDrawer.DrawColumnChart("نمودار ستونی تغییرات مقدار واقعی شاخص بر اساس زمان",
                    //    "مقدار واقعی", new List<string>() { "مقدار واقعی" }, valuesObject);
                    return PartialView("_BarChart", null);
                }
            }
            
           
            return null;
        }
        // GET: Charts
        [HttpGet]
        public ActionResult Index()
        {
            var model = new DrawChartViewModel()
            {
                Indicators = new SelectList(_mainContainer.IndicatorRepository.All(), "Id", "Subject"),
                Step = DrawChartViewStep.SelectType,
                Committees = new SelectList(_mainContainer.CommitteeRepository.All(), "Id", "Name")
            };
            return View(model);
        }

        public async Task<PartialViewResult> IndexValueSubCriterion(int committeeId, SubCriterionChart selectedChart,
            string selectedIndic)
        {
            var committee = _mainContainer.CommitteeRepository.FirstOrDefault(committeeT => committeeT.Id == committeeId);
            var indicators = new List<Indicator>();
            var criteria = new List<Criterion>();
            var subCriterions = new List<SubCriterion>();
            if (selectedIndic == "SubCriterion")
            {
                if (selectedChart==SubCriterionChart.ChangesByTimeColumn)
                {
                    subCriterions =
                        await
                            _mainContainer.SubCriterionRepository.Where(
                                subCriterion => subCriterion.Committee.Id == committeeId).ToListAsync();
                }
                else if (selectedChart==SubCriterionChart.IndicatorsWeightPie)
                {
                    subCriterions =
                        await
                            _mainContainer.SubCriterionRepository.Where(
                                subCriterion => subCriterion.Committee.Id == committeeId).ToListAsync();
                }
            }
            var model = new DrawChartSelectValues()
            {
                Criteria = new SelectList(criteria, "Id", "Subject"),
                Indicators = new SelectList(indicators, "Id", "Subject"),
                SubCriteions = new SelectList(subCriterions, "Id", "Subject"),
            };
            return PartialView("_IndexValue", model);
        }

        public async Task<PartialViewResult> IndexValueCriterion(int committeeId, CriterionChart selectedChart,
            string selectedIndic)
        {
            var committee = _mainContainer.CommitteeRepository.FirstOrDefault(committeeT => committeeT.Id == committeeId);
            var indicators = new List<Indicator>();
            var criteria = new List<Criterion>();
            var subCriterions = new List<SubCriterion>();
            if (selectedIndic == "Criterion")
            {
                if (selectedChart == CriterionChart.CriteriaByTime)
                {
                    criteria =
                        await
                            _mainContainer.CriterionRepository.Where(criterion => criterion.Committee.Id == committeeId)
                                .ToListAsync();
                }
                else if (selectedChart == CriterionChart.CriteriaWeightPie)
                {
                    criteria =
                        await
                            _mainContainer.CriterionRepository.Where(criterion => criterion.Committee.Id == committeeId)
                                .ToListAsync();
                }
                else if (selectedChart == CriterionChart.CriterionByMultipleTimeColumn)
                {
                    criteria =
                        await
                            _mainContainer.CriterionRepository.Where(criterion => criterion.Committee.Id == committeeId)
                                .ToListAsync();
                }
            }
            var model = new DrawChartSelectValues()
            {
                Criteria = new SelectList(criteria, "Id", "Subject"),
                Indicators = new SelectList(indicators, "Id", "Subject"),
                SubCriteions = new SelectList(subCriterions, "Id", "Subject"),
                
            };
            return PartialView("_IndexValue", model);
        }

        public async Task<PartialViewResult> IndexValueIndicator(int committeeId, IndicatorChart selectedChart, string selectedIndic)
        {
            var committee = _mainContainer.CommitteeRepository.FirstOrDefault(committeeT => committeeT.Id == committeeId);
            var indicators = new List<Indicator>();
            var criteria = new List<Criterion>();
            var subCriterions = new List<SubCriterion>();
            if (selectedIndic == "Indicator")
            {
                if (selectedChart==IndicatorChart.RealValuesChangesByTimeColumn)
                {
                    indicators =
                        await _mainContainer.IndicatorRepository.Where(indicator => indicator.Committee.Id == committeeId).ToListAsync();
                }
                else if(selectedChart==IndicatorChart.RealValuesChangesByMultipleTimesColumn)
                {
                    indicators =
                        await _mainContainer.IndicatorRepository.Where(indicator => indicator.Committee.Id == committeeId).ToListAsync();
                }
            }
            
            else
            {
                subCriterions =
                   await _mainContainer.SubCriterionRepository.Where(subCriterion => subCriterion.Committee.Id == committeeId).ToListAsync();
            }
            var model = new DrawChartSelectValues()
            {
                
                Indicators = new SelectList(indicators, "Id", "Subject"),
                IndicatorChart =selectedChart,

                
            };
            return PartialView("_IndexValue", model);
        }

        public async Task<PartialViewResult> IndexType(int committeeId, string selectedIndic)
        {
            var indicatorCharts = new List<IndicatorChartC>();
            var subCriterionCharts=new List<SubCriterionChartC>();
            if (selectedIndic == "Indicator")
            {

                indicatorCharts = new List<IndicatorChartC>()
                {
                    new IndicatorChartC()
                    {
                        IndicatorChart = IndicatorChart.RealValuesChangesByTimeColumn,
                        Key = IndicatorChart.RealValuesChangesByTimeColumn,
                        Value = "نمودار تغییرات مقادیر واقعی شاخص بر حسب زمان"
                    },
                    new IndicatorChartC()
                    {
                        IndicatorChart = IndicatorChart.RealValuesChangesByMultipleTimesColumn,
                        Key = IndicatorChart.RealValuesChangesByMultipleTimesColumn,
                        Value = "نمودار تغییرات شاخص در چند محدوده زمانی "
                    }
                };
            }
            else if (selectedIndic=="SubCriterion")
            {
                subCriterionCharts = new List<SubCriterionChartC>()
                {
                    new SubCriterionChartC()
                    {
                        SubCriterionChart = SubCriterionChart.ChangesByMultipleTimesColumn,
                        Key = SubCriterionChart.ChangesByMultipleTimesColumn,
                        Value = "نمودار تغییرات زیرمعیار در چند محدوده زمانی"
                    },
                    new SubCriterionChartC()
                    {
                        SubCriterionChart = SubCriterionChart.ChangesByTimeColumn,
                        Key = SubCriterionChart.ChangesByTimeColumn,
                        Value = "نمودار تغییرات زیرمعیار بر حسب زمان"
                    },
                    new SubCriterionChartC()
                    {
                        SubCriterionChart = SubCriterionChart.IndicatorsPercentRealValueDiffFromIdealColumn,
                        Key = SubCriterionChart.IndicatorsPercentRealValueDiffFromIdealColumn,
                        Value = "نمودار اختلاف مقدار واقعی از مقدار ایده آل شاخص های زیر معیار در زمان مشخص"
                    },
                    new SubCriterionChartC()
                    {
                        SubCriterionChart = SubCriterionChart.IndicatorsWeightPie,
                        Key = SubCriterionChart.IndicatorsWeightPie,
                        Value = "نمودار دایره ای ضرایب شاخص های زیر معیار"
                    }

                };
            }
            else
            {
                
            }

            var model=new DrawChartSelectTypeViewModel();
            model.CommitteeId = committeeId;
            model.IndicatorChartCs = new SelectList(indicatorCharts,"Key","Value");
            model.SubCriterionChartCs=new SelectList(subCriterionCharts,"Key","Value");
            model.SelectedIndic = selectedIndic;
            return PartialView("_IndexType",model);

        }
        public PartialViewResult DatePicker()
        {
            return PartialView();
        }
        public PartialViewResult DrawIndicatorTimeColumnChart(DateTime fromDate, DateTime toDate,int indicatorId)
        {
            var realValues =
                _mainContainer.IndicatorRealValueRepository.Where(
                    realValue =>
                        realValue.Indicator.Id == indicatorId && realValue.Time >= fromDate && realValue.Time <= toDate)
                    .ToList();
            var values= realValues.Select(realValue => (object)realValue.Value.GetValueOrDefault(0)).ToList();

            return PartialView("_BarChart", null);
            //_chartDrawer.DrawColumnChart("نمودار تغییرات شاخص بر حسب زمان", "مقدار واقعی شاخص",
            //    new List<string>() {realValues[0].Indicator.Subject}, values));
        }
         
        
    }
    public class DrawChartViewModel
    {
        public SelectList Indicators { get; set; }
        public SelectList SubCriteions { get; set; }
        public SelectList Criteria { get; set; }
        public SelectList Committees { get; set; }
        public int SelectedIndicatorId { get; set; }
        public int SelectedCommitteeId { get; set; }
        public int SelectedCriterionId { get; set; }
        public int SelectedSubCriterionId { get; set; }
        public DrawChartViewStep Step { get; set; }

    }

    public class DrawChartSelectTypeViewModel
    {
        public int CommitteeId { get; set; }
        public SelectList IndicatorChartCs { get; set; }
        public IndicatorChart SelectedIndicatorChart { get; set; }
        public SelectList SubCriterionChartCs { get; set; }
        public SubCriterionChart SelectedSubCriterionChart { get; set; }
        public SelectList CriterionChartCs { get; set; }
        public CriterionChart SelectedCriterionChart { get; set; }
        public string SelectedIndic { get; set; }

    }

    public class DrawChartSelectValues
    {
        public IndicatorChart IndicatorChart { get; set; }
        public SubCriterionChart SubCriterionChart { get; set; }
        public CriterionChart CriterionChart { get; set; }
        public SelectList Indicators { get; set; }
        public SelectList SubCriteions { get; set; }
        public SelectList Criteria { get; set; }
        public List<string> FromDates { get; set; }
        public List<string> ToDates { get; set; }

    }
    public enum DrawChartViewStep
    {
        SelectType,
        SelectValue,
        DrawChart,
    }
    
    public enum IndicatorChart
    {
        RealValuesChangesByTimeColumn,
        RealValuesChangesByMultipleTimesColumn,
        //RealValuesChangesByTimeLine,
        //RealValuesChangesByMultipleTimesLine,
    }
    public enum SubCriterionChart
    {
        ChangesByTimeColumn,
        ChangesByMultipleTimesColumn,
       // MultipleSubCriterionByTimeColumn, remove its nonmeaning to compare different sub criteria due to its type of data and range of data
        //ChangesByTimeLine,
        //ChangesByMultipleTimesLine,
        //MultipleSubCriterionByTimeLine,

        IndicatorsWeightPie,
        IndicatorsPercentRealValueDiffFromIdealColumn,
        IndicatorsRealValueDiffIdealAnyTimeColumn,

    }

    public enum CriterionChart
    {
        SubCriterionsByTimeColumn,
        SubCriterionsLastCondition,
        SubCriterionsWeightPie,

        CriterionByTimeColumn,
        CriterionByMultipleTimeColumn,
        CriteriaByTime,
        CriterionLastCondition,
        CriterionConditionByTime,
        CriteriaWeightPie,
    }

    public enum ChartTypeT
    {
        RealValuesChangesByTimeColumn,
        RealValuesChangesByMultipleTimesColumn,

        ChangesByTimeColumn,
        ChangesByMultipleTimesColumn,
        MultipleSubCriterionByTimeColumn,
        //ChangesByTimeLine,
        //ChangesByMultipleTimesLine,
        //MultipleSubCriterionByTimeLine,

        IndicatorsWeightPie,
        IndicatorsPercentRealValueDiffFromIdealColumn,
        IndicatorsRealValueDiffIdealAnyTimeColumn,

        SubCriterionsByTimeColumn,
        SubCriterionsLastCondition,
        SubCriterionsWeightPie,

        CriterionByTimeColumn,
        CriterionByMultipleTimeColumn,
        CriteriaByTime,
        CriterionLastCondition,
        CriterionConditionByTime,
        CriteriaWeightPie,
    }
    public interface IChart
    {
        Highcharts GetChart(ChartTypeT chart);
    }
    
    public class IndicatorChartC
    {
        public IndicatorChart Key { get; set; }
        public string Value { get; set; }
        public IndicatorChart IndicatorChart { get; set; }
        
    }
    public class SubCriterionChartC
    {
        public SubCriterionChart Key { get; set; }
        public string Value { get; set; }
        public SubCriterionChart SubCriterionChart { get; set; }

    }
    public class CriterionChartC
    {
        public CriterionChart Key { get; set; }
        public string Value { get; set; }
        public CriterionChart CriterionChart { get; set; }

    }

}