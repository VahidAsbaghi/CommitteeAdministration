using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Services;
using CommitteeAdministration.Services.Contract;
using CommitteeManagement.Repository;
using DotNet.Highcharts;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Models.ChartModels
{
    public class CriterionChangeOnePeriod : Chart
    {
        private static readonly IMainContainer MainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private static readonly IChartDrawer ChartDrawer = ModelContainer.Instance.Resolve<IChartDrawer>();
        private static readonly ICommitteeStatus CommitteeStatus = ModelContainer.Instance.Resolve<ICommitteeStatus>();
        public CriterionChangeOnePeriod()
        {
            Id = 7;
            ChartIndicType = ChartIndicType.Criterion;
            ChartNameString = ChartNameString.CriterionChangeOnePeriod;
            ChartType = new List<string> { ChartModels.ChartType.BarChart.ToString(), ChartModels.ChartType.lineChart.ToString() };
            IsMultiple = false;
            if (AllCharts.Contains(7))
                return;
            AllCharts.Add(7, this);
        }
        public static Highcharts DrawChart(int criterionId, DateTime from, DateTime to, ChartType chartType)
        {
            var criterion = MainContainer.CriterionRepository.FirstOrDefault(criterionT => criterionT.Id == criterionId);
            var counter = ((to - from).Duration().Days / 30) + 1;
            var valuesObject = new object[counter, 2];
            var categories = new List<string>();
            var objectDic = new SortedDictionary<DateTime, double>();
            var timeOfComparison = from;

            for (var i = 0; i < counter; i++)
            {
                var percentage = CommitteeStatus.GetCriterionPercentage(criterion, timeOfComparison);
                var shamsiDate = timeOfComparison.MiladiToShamsi();
                objectDic.Add(shamsiDate, percentage);
                categories.Add(shamsiDate.ToString(CultureInfo.CurrentCulture));
                timeOfComparison = timeOfComparison.AddMonths(1);
            }

            for (var j = 0; j < counter; j++)
            {
                valuesObject[j, 0] = objectDic.Keys.ToList()[j];
                valuesObject[j, 1] = objectDic.Values.ToList()[j];
            }
            
            switch (chartType)
            {
                case ChartModels.ChartType.BarChart:
                    return ChartDrawer.DrawColumnChart("نمودار ستونی تغییرات درصد کارایی معیار بر اساس زمان",
                        criterion.Subject,
                        "درصد", "", categories,
                        new[] { new Series() { Name = criterion.Subject, Data = new Data(valuesObject) } });
                case ChartModels.ChartType.lineChart:
                    return ChartDrawer.DrawLineChart("نمودار خطی تغییرات درصد کارایی معیار بر اساس زمان",
                        criterion.Subject, categories.ToArray(),
                        "درصد", "",
                        new[] { new Series() { Name = criterion.Subject, Data = new Data(valuesObject) } });
                default:
                    return null;

            }
        }
    }
}