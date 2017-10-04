using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommitteeAdministration.Models.ChartModels
{

    public enum ChartIndicType
    {
        Criterion=0,
        SubCriterion=1,
        Indicator=2
    }

    public enum ChartNameString
    {
        IndicatorChangesOnePeriod,
        IndicatorChangesMultiPeriod,
        IndicatorsChangeOnePeriod,
        SubCriterionChangeOnePeriod,
        SubCriterionChangeMultiPeriod,
        SubCriteriaChangeOnePeriod,
        CriterionChangeOnePeriod,
        CriterionChangeMultiPeriod,
        CriteriaChangeOnePeriod,
        IndicatorCoefficients,
        SubCriterionCoefficients,
        CriterionCoefficients
    }

    public enum ChartType
    {
        BarChart,
        PieChart,
        lineChart
    }
}