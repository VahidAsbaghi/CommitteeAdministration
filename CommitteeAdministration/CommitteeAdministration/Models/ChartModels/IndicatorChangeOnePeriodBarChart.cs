using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Services.Contract;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using DotNet.Highcharts;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Models.ChartModels
{
    internal class IndicatorChangeOnePeriodBarChart : IndicatorChangesOnePeriod
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
                                                                   realValue.Time <= to).ToList();
            var values=selectedRealValues.Select(realValueT => realValueT.Value.Value).ToList();
            var listRealValues = selectedRealValues.ToList();
            var valuesObject = new object[values.Count,2];
            var categories=new List<string>();
            var persianCalendar = new PersianCalendar();
            var objectDic = new SortedDictionary<DateTime, double>();
            for (var i = 0; i < values.Count; i++)
            {
                
                var date = selectedRealValues[i].Time.GetValueOrDefault().Date;
                var dateStringShamsi =
                        $"{persianCalendar.GetYear(date)}/{persianCalendar.GetMonth(date)}/{persianCalendar.GetDayOfMonth(date)}";

                objectDic.Add(DateTime.Parse(dateStringShamsi), values[i]);
                categories.Add(DateTime.Parse(dateStringShamsi).ToString(CultureInfo.CurrentCulture));
            }
            for (var j = 0; j < values.Count; j++)
            {
                valuesObject[j, 0] = objectDic.Keys.ToList()[j];
                valuesObject[j, 1] = objectDic.Values.ToList()[j];
            }
            objectDic = null;

            var chart = ChartDrawer.DrawColumnChart("نمودار ستونی تغییرات مقدار واقعی شاخص بر اساس زمان",
                indicator.Subject,
                "مقدار واقعی", "", categories,
                new[] {new Series() {Name = indicator.Subject, Data = new Data(valuesObject)}});
            return chart;
        }
    }
}