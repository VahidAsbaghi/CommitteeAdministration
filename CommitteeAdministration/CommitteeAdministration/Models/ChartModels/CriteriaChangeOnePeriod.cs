using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Services;
using CommitteeAdministration.Services.Contract;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using DotNet.Highcharts;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Models.ChartModels
{
    public class CriteriaChangeOnePeriod : Chart
    {
        private static readonly IMainContainer MainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private static readonly IChartDrawer ChartDrawer = ModelContainer.Instance.Resolve<IChartDrawer>();
        private static readonly ICommitteeStatus CommitteeStatus = ModelContainer.Instance.Resolve<ICommitteeStatus>();
        public CriteriaChangeOnePeriod()
        {
            Id = 9;
            ChartIndicType = ChartIndicType.Criterion;
            ChartNameString = ChartNameString.CriteriaChangeOnePeriod;
            ChartType = new List<string> { ChartModels.ChartType.BarChart.ToString(), ChartModels.ChartType.lineChart.ToString() };
            IsMultiple = true;
            if (AllCharts.Contains(9))
                return;
            AllCharts.Add(9, this);
        }
        public static Highcharts DrawChart(int[] criterionIds, DateTime from, DateTime to, ChartType chartType)
        {
            var criteria = new List<Criterion>();
            for (var i = 0; i < criterionIds.Length; i++)
            {
                var criterionId1 = criterionIds[i];
                criteria.Add(MainContainer.CriterionRepository.FirstOrDefault(criterionT => criterionT.Id == criterionId1));
            }
            var series = new List<Series>();
            var categories = new List<string>();
            var counter = ((to - from).Duration().Days / 30) + 1;
            foreach (var criterion in criteria)
            {
                var objectDic = new SortedDictionary<DateTime, double>();

                var dataObjects = new object[counter, 2];
                var timeOfComparison = from;
                for (var j = 0; j < counter; j++)
                {
                    var percentage = CommitteeStatus.GetCriterionPercentage(criterion, timeOfComparison);
                    var shamsiDate = timeOfComparison.MiladiToShamsi();
                    objectDic.Add(shamsiDate, percentage);
                    categories.Add(shamsiDate.ToString(CultureInfo.CurrentCulture));

                    timeOfComparison = timeOfComparison.AddMonths(1);
                }
                for (var j = 0; j < counter; j++)
                {
                    dataObjects[j, 0] = objectDic.Keys.ToList()[j];
                    dataObjects[j, 1] = objectDic.Values.ToList()[j];
                }
                series.Add(new Series() { Name = criterion.Subject, Data = new Data(dataObjects) });
                objectDic = null;
            }

            switch (chartType)
            {
                case ChartModels.ChartType.BarChart:
                    return ChartDrawer.DrawColumnChart("نمودار ستونی تغییرات درصد کارایی معیارها بر حسب زمان", "",
                        "درصد", "", categories, series.ToArray());
                case ChartModels.ChartType.lineChart:
                    return ChartDrawer.DrawLineChart("نمودار خطی تغییرات درصد کارایی معیارها بر حسب زمان", "",
                        categories.ToArray(), "درصد", "", series.ToArray());
                default:
                    return null;
            }
        }
    }
}