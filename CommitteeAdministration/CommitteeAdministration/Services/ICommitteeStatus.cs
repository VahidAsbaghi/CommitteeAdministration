using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommitteeAdministration.Models;
using CommitteeManagement.Model;

namespace CommitteeAdministration.Services
{
    public interface ICommitteeStatus
    {
        List<IndicatorsConditionModel> IndicatorsPercentage(SubCriterion subCriterion, DateTime timeOfComparison);
        List<SubCriterionConditionModel> GetSubCriterionsCondition(Criterion criterion, DateTime comparisonTime);
        List<CriterionConditionModel> GetCriteriaCondition(DateTime conditionTime, Committee committee);
        List<IndicatorConditionRateModel> GetIndicatorConditionRate(Indicator indicator, int maxNumberOfPoints);
        IndicatorRealValue FindFitestRealValue(IEnumerable<IndicatorRealValue> realValues, DateTime time);
    }
}
