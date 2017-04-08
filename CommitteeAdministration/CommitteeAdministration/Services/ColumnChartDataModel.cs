using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommitteeAdministration.Services
{
    public class ColumnChartDataModel
    {
        public List<string> Categories { get; set; }
        public string CahrtName { get; set; }
        public string MainTitle { get; set; }
        public string SubMainTitle { get; set; }
        public string YAxisTitle { get; set; }
        public List<List<object>> DataSeriesList { get; set; }
        public List<string> SeriesNameList { get; set; }
        
    }
}