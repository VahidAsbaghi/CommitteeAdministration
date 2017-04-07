using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommitteeAdministration.Controllers;
using CommitteeManagement.Model;

namespace CommitteeAdministration.Models
{
    /// <summary>
    /// the return model that shows the real value and its related condition. 
    /// </summary>
    public class CommitteeConditionReturnModel
    {
        /// <summary>
        /// Gets or sets the real value.
        /// </summary>
        /// <value>
        /// The real value.
        /// </value>
        public int RealValueId { get; set; }
        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        public CommitteeCondition  Condition { get; set; }
    }

    public class IndicatorsConditionModel
    {
        public Indicator Indicator { get; set; }
        public double ConditionPercentage { get; set; }
    }

    public class SubCriterionConditionModel
    {
        public SubCriterion SubCriterion { get; set; }
        public double Percentage { get; set; }
    }

    public class CriterionConditionModel
    {
        public Criterion Criterion { get; set; }
        public double Percentage { get; set; }
    }

    public class IndicatorConditionRateModel
    {
        public Indicator Indicator { get; set; }
        public DateTime ConditionTime { get; set; }
        public double ConditionPercentage { get; set; }
    }
}