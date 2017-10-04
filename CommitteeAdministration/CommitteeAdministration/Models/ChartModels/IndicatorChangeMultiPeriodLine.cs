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
    internal class IndicatorChangeMultiPeriodLine : IndicatorChangeMultiPeriod
    {
        private static readonly IMainContainer MainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private static readonly IChartDrawer ChartDrawer = ModelContainer.Instance.Resolve<IChartDrawer>();
        internal static Highcharts AbstractDrawChart(int indicatorId, List<DateTime> froms, List<DateTime> tos)
        {
            var indicator = MainContainer.IndicatorRepository.FirstOrDefault(indicatorT => indicatorT.Id == indicatorId);
            var realValues = MainContainer.IndicatorRealValueRepository.All()
                .Where(
                    realValue => realValue.Indicator.Id == indicatorId);
            DateTime from;
            DateTime to;
            var fromsCount = froms.Count;
            var categories = new List<string>();
            var series = new List<Series>();
            var lastCount = 0;
            
            for (var i = 0; i < fromsCount; i++)
            {
                
                from = froms[i];
                to = tos[i];
                var selectedRealValues = realValues.Where(realValue => realValue.Time >= @from &&
                                                                       realValue.Time <= to).ToList();
                var values = selectedRealValues.Select(realValueT => realValueT.Value.Value).ToList();
                var counter = values.Count;
                if (counter > lastCount)
                {
                    lastCount = counter;
                }
                var dataObjects = new object[counter,2];
                var  objectDic=new SortedDictionary<DateTime,double>();
                
                
                for (var j = 0; j < counter; j++)
                {
                    
                    var date = selectedRealValues[j].Time.GetValueOrDefault().Date;
                    var dateShamsi =
                        date.MiladiToShamsi();

                    objectDic.Add(dateShamsi, values[j]);
                    
                }
                for (int j = 0; j < counter; j++)
                {
                    dataObjects[j, 0] = objectDic.Keys.ToList()[j];
                    dataObjects[j, 1] = objectDic.Values.ToList()[j];
                }
                objectDic = null;
                series.Add(new Series() { Name = indicator.Subject, Data =new Data(dataObjects) });
            }
           
            
            for (int i = 0; i < lastCount; i++)
            {
                categories.Add(i.ToString());
            }


            var chart = ChartDrawer.DrawLineChart("نمودار ستونی تغییرات مقدار واقعی شاخص بر اساس زمان", indicator.Subject, categories.ToArray(),
                "مقدار واقعی", "", series.ToArray());
            return chart;
        }
    }
}