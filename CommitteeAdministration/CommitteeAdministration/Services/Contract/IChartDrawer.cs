using System.Collections.Generic;
using DotNet.Highcharts;

namespace CommitteeAdministration.Services.Contract
{
    public interface IChartDrawer
    {
        Highcharts DrawColumnChart(string title, string yAxisTitle, List<string> categories, List<object> datas);

    }
}