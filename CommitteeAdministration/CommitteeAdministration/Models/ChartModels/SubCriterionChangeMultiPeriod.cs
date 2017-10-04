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
    public class SubCriterionChangeMultiPeriod : Chart
    {
        private static readonly IMainContainer MainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private static readonly IChartDrawer ChartDrawer = ModelContainer.Instance.Resolve<IChartDrawer>();
        private static readonly ICommitteeStatus CommitteeStatus = ModelContainer.Instance.Resolve<ICommitteeStatus>();
        public SubCriterionChangeMultiPeriod()
        {
            Id = 5;
            ChartIndicType = ChartIndicType.SubCriterion;
            ChartNameString = ChartNameString.SubCriterionChangeMultiPeriod;
            ChartType = new List<string> { ChartModels.ChartType.BarChart.ToString(), ChartModels.ChartType.lineChart.ToString() };
            IsMultiple = false;
            if (AllCharts.Contains(5))
                return;
            AllCharts.Add(5, this);
        }
        public static Highcharts DrawChart(int subCriterionId, List<DateTime> froms, List<DateTime> tos, ChartType chartType)
        {
            var subCriterion = MainContainer.SubCriterionRepository.FirstOrDefault(subCriterionT => subCriterionT.Id == subCriterionId);
            var fromsCount = froms.Count;
            var categories = new List<string>();
            var series = new List<Series>();
            for (var i = 0; i < fromsCount; i++)
            {
                var counter = ((tos[i] - froms[i]).Duration().Days / 30) + 1;
                var valuesObject = new object[counter, 2];
                var objectDic = new SortedDictionary<DateTime, double>();
                var timeOfComparison = froms[i];
                for (var j = 0; j < counter; j++)
                {
                    var percentage = CommitteeStatus.GetSubCriterionPercentage(subCriterion, timeOfComparison);
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
                series.Add(new Series() { Name = subCriterion.Subject, Data = new Data(valuesObject) });
                objectDic = null;
            }
            switch (chartType)
            {
                case ChartModels.ChartType.BarChart:
                    return ChartDrawer.DrawColumnChart("نمودار ستونی تغییرات درصد کارایی زیرمعیار در چند محدوده زمانی", subCriterion.Subject,
                "درصد", "", categories, series.ToArray());
                case ChartModels.ChartType.lineChart:
                    return ChartDrawer.DrawLineChart("نمودار خطی تغییرات درصد کارایی زیرمعیار در چند محدوده زمانی", subCriterion.Subject, categories.ToArray(),
                "درصد", "", series.ToArray());
                default:
                    return null;

            }
        }
    }
}