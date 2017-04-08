using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;

namespace CommitteeAdministration.Services
{
    public class ChartDrawer : IChartDrawer
    {

        public Highcharts BasicColumn(ColumnChartDataModel chartDataModel)
        {
            var series= chartDataModel.DataSeriesList.Select((dataSeriesItem, i) => new Series() {Name = chartDataModel.SeriesNameList[i], Data = new Data(dataSeriesItem.ToArray())}).ToArray();

            Highcharts chart = new Highcharts(chartDataModel.CahrtName)
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                .SetTitle(new Title { Text = chartDataModel.MainTitle})
                .SetSubtitle(new Subtitle { Text = chartDataModel.SubMainTitle})
                .SetXAxis(new XAxis { Categories =chartDataModel.Categories.ToArray()})// new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" } })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Title = new YAxisTitle { Text = chartDataModel.YAxisTitle}
                })
                .SetLegend(new Legend
                {
                    Layout = Layouts.Vertical,
                    Align = HorizontalAligns.Left,
                    VerticalAlign = VerticalAligns.Top,
                    X = 100,
                    Y = 70,
                    Floating = true,
                    BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFFFFF")),
                    Shadow = true
                })
                .SetTooltip(new Tooltip { Formatter = @"function() { return ''+ this.x +': '+ this.y +' mm'; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        PointPadding = 0.2,
                        BorderWidth = 0
                    }
                })
                .SetSeries(series);

            return chart;


        }
        /// <summary>
        /// Draws the column chart.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="yAxisTitle">The y axis title.</param>
        /// <param name="categories">The categories.</param>
        /// <param name="datas">The datas.</param>
        /// <returns></returns>
        public Highcharts DrawColumnChart(string title, string yAxisTitle, List<string> categories, List<object> datas)
        {

            Highcharts chart = new Highcharts("chart")
               .InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = ChartTypes.Column, Margin = new[] { 50, 50, 100, 80 } })
               .SetTitle(new Title { Text = title })
               .SetXAxis(new XAxis
               {
                   Categories = categories.ToArray(),

                   Labels = new XAxisLabels
                   {
                       Rotation = -45,
                       Align = HorizontalAligns.Right,
                       Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif'",

                   }
               })
               .SetYAxis(new YAxis
               {
                   Min = 0,
                   Title = new YAxisTitle { Text = yAxisTitle }
               })
               .SetLegend(new Legend { Enabled = false })
               .SetPlotOptions(new PlotOptions
               {
                   Column = new PlotOptionsColumn
                   {
                       DataLabels = new PlotOptionsColumnDataLabels
                       {
                           Enabled = true,
                           Rotation = -90,
                           Color = ColorTranslator.FromHtml("#FFFFFF"),
                           Align = HorizontalAligns.Right,
                           X = 4,
                           Y = 10,
                           Formatter = "function() { return this.y; }",
                           Crop = false,
                           Overflow = "Justify",
                           Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif',textShadow: '0 0 3px black'"
                       }
                   }
               })
               .SetSeries(new Series
               {
                   Name = "Percentage",
                   Data = new Data(datas.ToArray()),

               });

            return chart;
        }
    }
}