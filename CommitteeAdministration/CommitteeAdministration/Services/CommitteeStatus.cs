using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using CommitteeAdministration.Controllers;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Models;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Services
{
    public class CommitteeStatus:ICommitteeStatus
    {
        private  readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        public List<IndicatorsConditionModel> IndicatorsPercentage(SubCriterion subCriterion,DateTime timeOfComparison)
        {
            var indicators =
                _mainContainer.IndicatorRepository.All().Where(indicator => indicator.SubCriterionId == subCriterion.Id).ToList();// subCriterion.Indicators.ToList();
            var conditions = new List<IndicatorsConditionModel>();
            for (int i=0;i<indicators.Count;i++)// indicator in indicators)
            {
                var indicator = indicators[i];
                var realValue = FindFitestRealValue(indicator.IndicatorRealValues, timeOfComparison);
                var idealValue = FindFitestIdealValue(indicator.IndicatorIdealValues, timeOfComparison);
                var indicatorCondition = CalculateIndicatorCondition(idealValue, realValue);
                conditions.Add(new IndicatorsConditionModel
                {
                    ConditionPercentage = indicatorCondition,
                    Indicator = indicator
                });
            }
            return conditions;
        }
        private  double CalculateIndicatorCondition(IndicatorIdealValue idealValue, IndicatorRealValue realValue)
        {
            var value = realValue.Value.GetValueOrDefault(0);

            double conditionValue;
            if (idealValue.LowerThan.GetValueOrDefault(false))
            {
                conditionValue = 200 - (value / idealValue.Value.GetValueOrDefault(1)) * 100;
            }
            else
            {
                conditionValue = (value / idealValue.Value.GetValueOrDefault(1)) * 100;
            }
            return conditionValue;
        }
        public  IndicatorRealValue FindFitestRealValue(IEnumerable<IndicatorRealValue> realValues,DateTime time)
        {
            var minTime = TimeSpan.MaxValue;
            IndicatorRealValue realValue=null;
            var indicatorRealValues = realValues as IList<IndicatorRealValue> ?? realValues.ToList();
            foreach (var t in indicatorRealValues)
            {
                if (!((t.Time - time) < minTime) || t.Time>time) continue;
                minTime = t.Time.GetValueOrDefault(DateTime.MaxValue) - time;
                realValue = t;
            }
            return realValue ??
                   indicatorRealValues.OrderByDescending(realValueT => realValueT.Time).FirstOrDefault();
        }
        private  IndicatorIdealValue FindFitestIdealValue(IEnumerable<IndicatorIdealValue> idealValues, DateTime time)
        {
            var minTime = TimeSpan.MaxValue;
            IndicatorIdealValue idealValue = null;
            foreach (var t in idealValues)
            {
                if (!((t.Time - time) < minTime) || t.Time > time) continue;
                minTime = t.Time.GetValueOrDefault(DateTime.MaxValue) - time;
                idealValue = t;
            }
            return idealValue ??
                  idealValues.OrderByDescending(idealValueT => idealValueT.Time).FirstOrDefault();
            
        }

        public  List<SubCriterionConditionModel> GetSubCriterionsCondition(Criterion criterion,DateTime comparisonTime)
        {
            var subCriterions =
                _mainContainer.SubCriterionRepository.All()
                    .Where(subCriterion => subCriterion.CriterionId == criterion.Id).ToList();
            var subCriterionsCondition=new List<SubCriterionConditionModel>();
            foreach (var subCriterion in subCriterions)
            {
                var indicatorsCondition = IndicatorsPercentage(subCriterion, comparisonTime);
                double subCriterionCondition = indicatorsCondition.Sum(condition => condition.Indicator.Coefficient.GetValueOrDefault(0.00001)*condition.ConditionPercentage);
                subCriterionsCondition.Add(new SubCriterionConditionModel()
                {
                    Percentage = subCriterionCondition,
                    SubCriterion = subCriterion
                });
            }
            return subCriterionsCondition;
        }

        public SubCriterionConditionModel GetSubCriterionCondition(SubCriterion subCriterion, DateTime from, DateTime to)
        {
            var indicators =
                _mainContainer.IndicatorRepository.Where(indicator => indicator.SubCriterionId.Value == subCriterion.Id).ToList();
            var realValues =
                _mainContainer.IndicatorRealValueRepository.Where(
                    realValue => realValue.IndicatorId.Value == indicators[0].Id);
            var inBoundRealValues=realValues.Where(realvalue=>realvalue)
            var indicatorsConditions=IndicatorsPercentage(subCriterion,)
        }
        public  List<CriterionConditionModel> GetCriteriaCondition(DateTime conditionTime,Committee committee)
        {
            var criteria = _mainContainer.CriterionRepository.All().Where(criterionT=>criterionT.Committee.Id==committee.Id).ToList();
            var criteriaCondition=new List<CriterionConditionModel>();
            foreach (var criterion in criteria)
            {
                var subCriterions =
                    _mainContainer.SubCriterionRepository.All()
                        .Where(subCriterion => subCriterion.CriterionId == criterion.Id);
                var subCriterionsCondition = GetSubCriterionsCondition(criterion, conditionTime);
                double criterionCondition =
                    subCriterionsCondition.Sum(
                        subCriterionCond => subCriterionCond.Percentage*subCriterionCond.SubCriterion.Coefficient);
                criteriaCondition.Add(new CriterionConditionModel() {Criterion = criterion,Percentage = criterionCondition});
            }
            return criteriaCondition;
        }

        public List<IndicatorConditionRateModel> GetIndicatorConditionRate(Indicator indicator,int maxNumberOfPoints)
        {
            var realValues =
                _mainContainer.IndicatorRealValueRepository.All()
                    .Where(realValueT => realValueT.Indicator.Id == indicator.Id).OrderByDescending(realValueT=>realValueT.Time).ToList();
            
            var indicatorConditionRate=new List<IndicatorConditionRateModel>();
            for (int i = 0; i < maxNumberOfPoints; i++)
            {
                if (i>=realValues.Count)
                {
                    break;
                }
                var realValue = realValues[i];
                var idealValue = FindMatchedIdealValue(realValue);
                var conditionPercentage = CalculateIndicatorCondition(idealValue, realValue);
                indicatorConditionRate.Add(new IndicatorConditionRateModel()
                {
                    ConditionPercentage = conditionPercentage,
                    ConditionTime = realValue.Time.GetValueOrDefault(),
                    Indicator = indicator
                });
            }
            return indicatorConditionRate;
        }

        private IndicatorIdealValue FindMatchedIdealValue(IndicatorRealValue realValue)
        {
            var idealValues =
                _mainContainer.IndicatorIdealValueRepository.All()
                    .Where(
                        idealValueT =>
                            idealValueT.Indicator.Id == realValue.Indicator.Id && idealValueT.Time <= realValue.Time);
            var idealValues1=idealValues.
                    OrderByDescending(idealValueT => idealValueT.Time);
            return idealValues1.First();
        }
    }
}