using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Services;
using CommitteeAdministration.Services.Contract;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Models.ChartModels
{
    public class IndicatorChangesOnePeriod:Chart
    {
        private static readonly IMainContainer MainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private static readonly IChartDrawer ChartDrawer = ModelContainer.Instance.Resolve<IChartDrawer>();
        public IndicatorChangesOnePeriod()
        {
            Id = 1;
            ChartIndicType = ChartIndicType.Indicator;
            ChartNameString = ChartNameString.IndicatorChangesOnePeriod;
            ChartType = new List<string>
            {
                ChartModels.ChartType.BarChart.ToString(),
                ChartModels.ChartType.lineChart.ToString()
            };
            if (AllCharts.Contains(1))
                return;
            AllCharts.Add(1, this);
        }
        public static Highcharts DrawChart(int indicatorId,DateTime from,DateTime to,ChartType chartType)
        {
            var indicator = MainContainer.IndicatorRepository.FirstOrDefault(indicatorT => indicatorT.Id == indicatorId);
            var realValues = MainContainer.IndicatorRealValueRepository.All()
                .Where(
                    realValue => realValue.Indicator.Id == indicatorId);
            var selectedRealValues = realValues.Where(realValue => realValue.Time >= @from &&
                                                                   realValue.Time <= to).ToList();
            var values = selectedRealValues.Select(realValueT => realValueT.Value.Value).ToList();
            var valuesObject = new object[values.Count, 2];
            var categories = new List<string>();
            var objectDic = new SortedDictionary<DateTime, double>();
            for (var i = 0; i < values.Count; i++)
            {
                var date = selectedRealValues[i].Time.GetValueOrDefault().Date;
                var dateShamsi = date.MiladiToShamsi();
                objectDic.Add(dateShamsi, values[i]);
                categories.Add(dateShamsi.ToString(CultureInfo.CurrentCulture));
            }
            for (var j = 0; j < values.Count; j++)
            {
                valuesObject[j, 0] = objectDic.Keys.ToList()[j];
                valuesObject[j, 1] = objectDic.Values.ToList()[j];
            }
            var series = new[] {new Series() {Name = indicator.Subject, Data = new Data(valuesObject)}};
            
            switch (chartType)
            {
                case ChartModels.ChartType.BarChart:
                    return ChartDrawer.DrawColumnChart("نمودار ستونی تغییرات مقدار واقعی شاخص بر اساس زمان",
                        indicator.Subject,
                        "مقدار واقعی", "", categories, series);               
                case ChartModels.ChartType.lineChart:
                    return ChartDrawer.DrawLineChart("نمودار خطی تغییرات مقدار واقعی شاخص بر اساس زمان",
                        indicator.Subject, categories.ToArray(),
                        "مقدار واقعی", "", series);
                default:
                    return null;
                
            }
        }
    }
}