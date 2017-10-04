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
    public class SubCriterionCoefficients : Chart
    {
        private static readonly IMainContainer MainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private static readonly IChartDrawer ChartDrawer = ModelContainer.Instance.Resolve<IChartDrawer>();
        public SubCriterionCoefficients()
        {
            Id = 11;
            ChartIndicType = ChartIndicType.SubCriterion;
            ChartNameString = ChartNameString.SubCriterionCoefficients;
            ChartType = new List<string>
            {
                ChartModels.ChartType.PieChart.ToString(),
            };
            if (AllCharts.Contains(11))
                return;
            AllCharts.Add(11, this);
        }
        public static Highcharts DrawChart(int criterionId, ChartType chartType)
        {
            var criterion1 =
                MainContainer.CriterionRepository.FirstOrDefault(criterion => criterion.Id == criterionId);
            var subCriterions = MainContainer.SubCriterionRepository.Where(subCriterion => subCriterion.Criterion.Id == criterionId).ToList();
            
            var valuesObject = new object[subCriterions.Count];
            for (var i = 0; i < subCriterions.Count; i++)
            {
                valuesObject[i] = new object[] { subCriterions[i].Subject, subCriterions[i].Coefficient };
            }
            var series = new Series() { Name = criterion1.Subject, Data = new Data(valuesObject) };

            switch (chartType)
            {
                case ChartModels.ChartType.PieChart:
                    return ChartDrawer.DrawPieChart("نمودار دایره ای ضرایب زیرمعیارهای یک معیار", criterion1.Subject, series);
                default:
                    return null;
            }
        }
    }
}