using System.Collections.Generic;
using System.Linq;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Services.Contract;
using CommitteeManagement.Repository;
using DotNet.Highcharts;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Models.ChartModels
{
    public class IndicatorCoefficients : Chart
    {
        private static readonly IMainContainer MainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private static readonly IChartDrawer ChartDrawer = ModelContainer.Instance.Resolve<IChartDrawer>();
        public IndicatorCoefficients()
        {
            Id = 10;
            ChartIndicType = ChartIndicType.Indicator;
            ChartNameString = ChartNameString.IndicatorCoefficients;
            ChartType = new List<string>
            {
                ChartModels.ChartType.PieChart.ToString(),
            };
            if (AllCharts.Contains(10))
                return;
            AllCharts.Add(10, this);
        }
        public static Highcharts DrawChart(int subCriterionId,ChartType chartType)
        {
            var subCriterion1 =
                MainContainer.SubCriterionRepository.FirstOrDefault(subCriterion => subCriterion.Id == subCriterionId);
            var indicators = MainContainer.IndicatorRepository.Where(indicatorT => indicatorT.SubCriterion.Id == subCriterionId).ToList();
            //var coefficients = indicators.Select(indicator => indicator.Coefficient);
            var valuesObject = new object[indicators.Count];
            for (var i = 0; i < indicators.Count; i++)
            {
                valuesObject[i]=new object[] {indicators[i].Subject,indicators[i].Coefficient};
            }
            var series =  new Series() { Name = subCriterion1.Subject, Data = new Data(valuesObject) } ;

            switch (chartType)
            {
                case ChartModels.ChartType.PieChart:
                    return ChartDrawer.DrawPieChart("نمودار دایره ای ضرایب شاخص های یک زیرمعیار",subCriterion1.Subject, series);
                default:
                    return null;
            }
        }
    }
}