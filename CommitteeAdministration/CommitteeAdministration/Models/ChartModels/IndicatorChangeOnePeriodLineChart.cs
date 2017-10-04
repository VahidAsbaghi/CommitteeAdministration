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
    internal class IndicatorChangeOnePeriodLineChart : IndicatorChangesOnePeriod
    {
        private static readonly IMainContainer MainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private static readonly IChartDrawer ChartDrawer = ModelContainer.Instance.Resolve<IChartDrawer>();
        internal static Highcharts AbstractDrawChart(int indicatorId, DateTime from, DateTime to)
        {
            var indicator = MainContainer.IndicatorRepository.FirstOrDefault(indicatorT => indicatorT.Id == indicatorId);
            var realValues = MainContainer.IndicatorRealValueRepository.All()
                 .Where(
                     realValue => realValue.Indicator.Id == indicatorId);
            var selectedRealValues = realValues.Where(realValue => realValue.Time >= @from &&
                                                                   realValue.Time <= to);
            var values = selectedRealValues.Select(realValueT => realValueT.Value.Value).ToList();
            var listRealValues = selectedRealValues.ToList();
            var categories = new List<string>();
            var valuesObject = new object[values.Count, 2];
            var persianCalendar = new PersianCalendar();
            var objectDic = new SortedDictionary<DateTime, double>();
            //var dates = new List<string>();
            for (var i = 0; i < values.Count; i++)
            {

                var date = listRealValues[i].Time.GetValueOrDefault().Date;
                var shamsiDate = date.MiladiToShamsi();

                objectDic.Add(shamsiDate, values[i]);
                categories.Add(shamsiDate.ToString(CultureInfo.InvariantCulture));
            }
            for (var j = 0; j < values.Count; j++)
            {
                valuesObject[j, 0] = objectDic.Keys.ToList()[j];
                valuesObject[j, 1] = objectDic.Values.ToList()[j];
            }
            objectDic = null;
            var chart = ChartDrawer.DrawLineChart("نمودار تغییرات مقدار واقعی یک شاخص", indicator.Subject,
                categories.ToArray(),
                "مقدار واقعی", "",
                new[] {new Series() {Name = indicator.Subject, Data = new Data(valuesObject)}});
            return chart;
        }
    }
}