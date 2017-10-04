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
    public class IndicatorsChangeOnePeriodBar : IndicatorsChangeOnePeriod
    {
        private static readonly IMainContainer MainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private static readonly IChartDrawer ChartDrawer = ModelContainer.Instance.Resolve<IChartDrawer>();
        private static readonly ICommitteeStatus CommitteeStatus = ModelContainer.Instance.Resolve<ICommitteeStatus>();
        internal static Highcharts AbstractDrawChart(int[] indicatorsId, DateTime from, DateTime to)
        {
            var indicators = new List<Indicator>();
            for (var i = 0; i < indicatorsId.Length; i++)
            {
                var indicatorid1 = indicatorsId[i];
                indicators.Add(MainContainer.IndicatorRepository.FirstOrDefault(indicator=>indicator.Id== indicatorid1));
            }
            
            var series = new List<Series>();
            var persianCalendar = new PersianCalendar();
            var categories = new List<string>();
            foreach (var indicator in indicators)
            {
                var objectDic = new SortedDictionary<DateTime, double>();
                var realValues =
                    MainContainer.IndicatorRealValueRepository.Where(realValue => realValue.Indicator.Id == indicator.Id && realValue.Time<=to && realValue.Time>=from);
                var counter = realValues.Count();
                var dataObjects = new object[counter, 2];
                foreach (var indicatorRealValue in realValues)
                {
                    var time = indicatorRealValue.Time.Value;
                    var percentage=CommitteeStatus.GetIndicatorPercentage(indicatorRealValue.Indicator,time );
                    var shamsiDate = time.MiladiToShamsi();
                    objectDic.Add(
                        shamsiDate,
                        percentage);

                }
                for (var j = 0; j < counter; j++)
                {
                    dataObjects[j, 0] = objectDic.Keys.ToList()[j];
                    dataObjects[j, 1] = objectDic.Values.ToList()[j];
                }
                objectDic = null;
                series.Add(new Series() { Name = indicator.Subject, Data = new Data(dataObjects) });
            }
           
            //10 is not good value. if u want to use categories in drawing the chart u should replace categories values with actual values.
            for (var i = 0; i < 10; i++)
            {
                categories.Add(i.ToString());
            }


            var chart = ChartDrawer.DrawColumnChart("نمودار ستونی درصد کارایی شاخصها بر اساس زمان", "",
                "درصد کارایی", "", categories, series.ToArray());
            return chart;

        }
    }
}