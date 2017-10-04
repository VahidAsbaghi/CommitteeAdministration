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
    public class SubCriterionChangeOnePeriodLineChart : SubCriterionChangeOnePeriod
    {
        private static readonly IMainContainer MainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private static readonly IChartDrawer ChartDrawer = ModelContainer.Instance.Resolve<IChartDrawer>();
        private static readonly ICommitteeStatus CommitteeStatus = ModelContainer.Instance.Resolve<ICommitteeStatus>();
        internal static Highcharts AbstractDrawChart(int subCriterionId, DateTime from, DateTime to)
        {
            var subCriterion = MainContainer.SubCriterionRepository.FirstOrDefault(subCriterionT => subCriterionT.Id == subCriterionId);
            var counter = ((to - from).Duration().Days / 30) + 1;
            var valuesObject = new object[counter, 2];
            var categories = new List<string>();
            var persianCalendar = new PersianCalendar();
            var objectDic = new SortedDictionary<DateTime, double>();
            var timeOfComparison = from;
            for (var i = 0; i < counter; i++)
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
            objectDic = null;

            var chart = ChartDrawer.DrawLineChart("نمودار خطی تغییرات درصد کارایی زیر معیار بر اساس زمان",
                subCriterion.Subject
                , categories.ToArray(),"درصد", "",
                new[] { new Series() { Name = subCriterion.Subject, Data = new Data(valuesObject) } });
            return chart;
        }
    }
}