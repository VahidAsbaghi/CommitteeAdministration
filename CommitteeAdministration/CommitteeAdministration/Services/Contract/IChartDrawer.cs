using System;
using System.Collections.Generic;
using DotNet.Highcharts;
using DotNet.Highcharts.Options;

namespace CommitteeAdministration.Services.Contract
{
    public interface IChartDrawer
    {
        Highcharts DrawColumnChart(string title, string subtitle, string yAxisTitle, string tooltipLastWord, List<string> categories, Series[] seriesArray);

        Highcharts DrawLineChart(string title, string subTitle, string[] xAxesCategories, string yAxesTitle,
            string legendLastWord, Series[] seriesArray);

        Highcharts DrawPieChart(string title, string subTitle, Series series);
    }
}