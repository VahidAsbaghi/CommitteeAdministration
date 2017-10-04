using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using CommitteeAdministration.Services.Contract;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Point = DotNet.Highcharts.Options.Point;

namespace CommitteeAdministration.Services
{
    public class ChartDrawer : IChartDrawer
    {
        /// <summary>
        /// Draws the column chart.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="subtitle">the subtitle of the chart</param>
        /// <param name="yAxisTitle">The y axis title.</param>
        /// <param name="tooltipLastWord"></param>
        /// <param name="categories">The categories.</param>
        /// <param name="seriesArray"></param>
        /// <returns></returns>
        public Highcharts DrawColumnChart(string title,string subtitle,  string yAxisTitle,string tooltipLastWord, List<string> categories, Series[] seriesArray)
        {
            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                .SetTitle(new Title { Text = title })
                .SetSubtitle(new Subtitle { Text = subtitle })
                .SetXAxis(new XAxis
                {
                    //Categories = categories.ToArray(),
                    Type = AxisTypes.Datetime,
                    //TickInterval = 1000 * 3600 * 24*20,
                    
                    DateTimeLabelFormats = new DateTimeLabel
                    {
                        Second = "'%Y-%m-%d<br/>%H:%M:%S'",
                        Minute = "%m/%Y %H:%M",
                        Hour = "'%m/%Y %H:%M'",
                        Day = "'%m/%Y'",
                        Week = "'%m/%Y'",
                        Month = "'%m/%Y'",
                        Year = "'%Y'"
                    },
                    
                })
                .SetYAxis(new YAxis
                {
                    //Min = 0,
                    Title = new YAxisTitle { Text = yAxisTitle},
                    
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
                    Shadow = true,
                    UseHTML = true
                })
                .SetTooltip(new Tooltip
                {
                    //Formatter = @"function() { return ''+ this.x +': '+ this.y +'"+ tooltipLastWord+"'; }"
                    Formatter =
                        "function() { return 'date: '+Highcharts.dateFormat('%d - %m - %Y', this.x) +' value: '+ this.y;}"
                    ,
                    UseHTML = true
                })
                .SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        PointPadding = 0.2,
                        BorderWidth = 0
                    }
                })
                .SetCredits(new Credits { Enabled = false })
                .SetSeries(seriesArray);

            return chart;
            
        }

        /// <summary>
        /// Draws the line chart.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="subTitle">The sub title.</param>
        /// <param name="xAxesCategories">The x axes categories.</param>
        /// <param name="yAxesTitle">The y axes title.</param>
        /// <param name="legendLastWord">The legend last word.</param>
        /// <param name="seriesArray">The series array.</param>
        /// <returns></returns>
        public Highcharts DrawLineChart(string title, string subTitle,string[] xAxesCategories,string yAxesTitle,string legendLastWord,Series[] seriesArray)
        {
            var chart = new Highcharts("chart")
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Line,
                    MarginRight = 130,
                    MarginBottom = 25,
                    ClassName = "chart"
                })
                .SetTitle(new Title
                {
                    Text = title,
                    X = -20,
                    UseHTML = true
                })
                .SetSubtitle(new Subtitle
                {
                    Text = subTitle,
                    X = -20,
                    UseHTML = true
                })
                .SetXAxis(new XAxis
                {
                    //Categories = xAxesCategories,
                    Type = AxisTypes.Datetime,
                    DateTimeLabelFormats = new DateTimeLabel
                    {
                        Second = "'%Y-%m-%d<br/>%H:%M:%S'",
                        Minute = "%m/%Y %H:%M",
                        Hour = "'%m/%Y %H:%M'",
                        Day = "'%m/%Y'",
                        Week = "'%m/%Y'",
                        Month = "'%m/%Y'",
                        Year = "'%Y'"
                    }
                    // Labels = new XAxisLabels { Formatter = "function() { return moment(this.value).format('YYYY - MM - DD');}" },
                })
                .SetYAxis(new YAxis
                {
                    Title = new YAxisTitle {Text = yAxesTitle},
                    PlotLines = new[]
                    {
                        new YAxisPlotLines
                        {
                            Value = 0,
                            Width = 1,
                            Color = ColorTranslator.FromHtml("#808080")
                        }
                    }

                })
                .SetTooltip(new Tooltip
                {
                    Formatter =
                        "function() { return 'date: '+Highcharts.dateFormat('%d - %m - %Y', this.x) +' value: '+ this.y;}"
                    ,
                    UseHTML = true
                })
                .SetLegend(new Legend
                {
                    Layout = Layouts.Vertical,
                    Align = HorizontalAligns.Right,
                    VerticalAlign = VerticalAligns.Top,
                    X = -10,
                    Y = 100,
                    BorderWidth = 0,
                    UseHTML = true
                })
                .SetCredits(new Credits { Enabled = false })
                .SetSeries(seriesArray
                );
            return chart;
        }

        public Highcharts DrawPieChart(string title,string subTitle, Series series)
        {
            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart {Type = ChartTypes.Pie,PlotShadow = false})
                .SetTitle(new Title {Text = title}).SetSubtitle(new Subtitle {Text = subTitle})
                .SetTooltip(new Tooltip
                {
                    Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.point.y; }",
                    UseHTML = true
                })
                .SetPlotOptions(new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        AllowPointSelect = true,
                        Cursor = Cursors.Pointer,
                        DataLabels = new PlotOptionsPieDataLabels
                        {
                            Color = ColorTranslator.FromHtml("#000000"),
                            ConnectorColor = ColorTranslator.FromHtml("#000000"),
                            Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.point.y; }",
                            UseHTML = true
                        }
                    }
                })
                .SetCredits(new Credits { Enabled = false })
                .SetSeries(series);
            //new Series
            //{
            //    Type = ChartTypes.Pie,
            //    Name = "Browser share",
            //    Data = new Data(new object[]
            //    {
            //        new object[] { "Firefox", 45.0 },
            //        new object[] { "IE", 26.8 },
            //        new Point
            //        {
            //            Name = "Chrome",
            //            Y = 12.8,
            //            Sliced = true,
            //            Selected = true
            //        },
            //        new object[] { "Safari", 8.5 },
            //        new object[] { "Opera", 6.2 },
            //        new object[] { "Others", 0.7 }
            //    })
            //});
        

    return chart;
        }
    }
}