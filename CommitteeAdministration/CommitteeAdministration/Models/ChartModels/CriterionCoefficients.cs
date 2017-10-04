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
    public class CriterionCoefficients : Chart
    {
        private static readonly IMainContainer MainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private static readonly IChartDrawer ChartDrawer = ModelContainer.Instance.Resolve<IChartDrawer>();
        public CriterionCoefficients()
        {
            Id = 12;
            ChartIndicType = ChartIndicType.Criterion;
            ChartNameString = ChartNameString.CriterionCoefficients;
            ChartType = new List<string>
            {
                ChartModels.ChartType.PieChart.ToString(),
            };
            if (AllCharts.Contains(12))
                return;
            AllCharts.Add(12, this);
        }
        public static Highcharts DrawChart(int committeeId, ChartType chartType)
        {
            var committee1 =
                MainContainer.CommitteeRepository.FirstOrDefault(committee => committee.Id == committeeId);
            var Criteria = MainContainer.CriterionRepository.Where(criterion => criterion.Committee.Id == committeeId).ToList();

            var valuesObject = new object[Criteria.Count];
            for (var i = 0; i < Criteria.Count; i++)
            {
                valuesObject[i] = new object[] { Criteria[i].Subject, Criteria[i].Coefficient };
            }
            var series = new Series() { Name = committee1.Name, Data = new Data(valuesObject) };

            switch (chartType)
            {
                case ChartModels.ChartType.PieChart:
                    return ChartDrawer.DrawPieChart("نمودار دایره ای ضرایب زیرمعیارهای یک معیار", committee1.Name, series);
                default:
                    return null;
            }
        }
    }
}