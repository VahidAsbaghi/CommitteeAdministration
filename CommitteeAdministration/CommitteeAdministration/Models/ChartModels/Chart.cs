using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using CommitteeAdministration.ViewModels;
using DotNet.Highcharts;

namespace CommitteeAdministration.Models.ChartModels
{
    public class Chart
    {
        public static Hashtable AllCharts;
        private static readonly object Lock=new object();
        public Chart()
        {
            if (AllCharts==null)
            {
                lock (Lock)
                {
                    if (AllCharts == null)
                    {
                        AllCharts = new Hashtable();
                    }
                }
               
            }            
        }
        [Key]
        public int Id { get; set; }
        public ChartNameString ChartNameString { get; set; }
        public ChartIndicType ChartIndicType { get; set; }
        public List<string> ChartType { get; set; }
        public bool IsMultiple { get; set; } = false;
        public static Highcharts Draw(Chart chart,ChartsViewModel chartsViewModel,ChartType chartType)
        {
            switch (chart.ChartNameString)
            {
                case ChartNameString.IndicatorChangesOnePeriod:
                    return IndicatorChangesOnePeriod.DrawChart(chartsViewModel.SelectedIndicatorId[0],
                        chartsViewModel.FromDateTimes[0], chartsViewModel.ToDateTimes[0], chartType);
                case ChartNameString.IndicatorChangesMultiPeriod:
                    return IndicatorChangeMultiPeriod.DrawChart(chartsViewModel.SelectedIndicatorId[0],
                        chartsViewModel.FromDateTimes, chartsViewModel.ToDateTimes, chartType);
                case ChartNameString.IndicatorsChangeOnePeriod:
                    return IndicatorsChangeOnePeriod.DrawChart(chartsViewModel.SelectedIndicatorId,
                        chartsViewModel.FromDateTimes[0], chartsViewModel.ToDateTimes[0], chartType);

                case ChartNameString.SubCriterionChangeOnePeriod:
                    return SubCriterionChangeOnePeriod.DrawChart(chartsViewModel.SelectedSubCriterionId[0],
                        chartsViewModel.FromDateTimes[0], chartsViewModel.ToDateTimes[0], chartType);
                case ChartNameString.SubCriteriaChangeOnePeriod:
                    return SubCriteriaChangeOnePeriod.DrawChart(chartsViewModel.SelectedSubCriterionId,
                        chartsViewModel.FromDateTimes[0], chartsViewModel.ToDateTimes[0], chartType);
                case ChartNameString.SubCriterionChangeMultiPeriod:
                    return SubCriterionChangeMultiPeriod.DrawChart(chartsViewModel.SelectedSubCriterionId[0],
                        chartsViewModel.FromDateTimes, chartsViewModel.ToDateTimes, chartType);

                case ChartNameString.CriteriaChangeOnePeriod:
                    return CriteriaChangeOnePeriod.DrawChart(chartsViewModel.SelectedCriterionId,
                        chartsViewModel.FromDateTimes[0], chartsViewModel.ToDateTimes[0], chartType);
                case ChartNameString.CriterionChangeOnePeriod:
                    return CriterionChangeOnePeriod.DrawChart(chartsViewModel.SelectedCriterionId[0],
                        chartsViewModel.FromDateTimes[0], chartsViewModel.ToDateTimes[0], chartType);
                case ChartNameString.CriterionChangeMultiPeriod:
                    return CriterionChangeMultiPeriod.DrawChart(chartsViewModel.SelectedCriterionId[0],
                        chartsViewModel.FromDateTimes, chartsViewModel.ToDateTimes, chartType);

                case ChartNameString.IndicatorCoefficients:
                    return IndicatorCoefficients.DrawChart(chartsViewModel.SelectedSubCriterionId[0], chartType);
                case ChartNameString.SubCriterionCoefficients:
                    return SubCriterionCoefficients.DrawChart(chartsViewModel.SelectedCriterionId[0], chartType);
                case ChartNameString.CriterionCoefficients:
                    return CriterionCoefficients.DrawChart(chartsViewModel.SelectedCommitteeId, chartType);
                default:
                    return null;
            }
        } 
    }
}