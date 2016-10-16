using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommitteeAdministration.ActionFilters
{
    /// <summary>
    /// Enums of action filters
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// the type of permission that user wants to grant
        /// </summary>
        public enum PermissionType
        {            
            Create,
            Index,
            Delete,
            Edit,
            None
        }
        /// <summary>
        /// user requests permissions for this objects
        /// </summary>
        public enum PermissionObject
        {
            IndicatorDeadline,
            Criterion,
            SubCriterion,
            Indicator,
            RealIndicator,
            None            
        }
    }
}