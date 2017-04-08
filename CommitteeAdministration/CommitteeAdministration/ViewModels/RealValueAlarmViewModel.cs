using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommitteeManagement.Model;

namespace CommitteeAdministration.ViewModels
{
    /// <summary>
    /// View model to use to send alram of expiration of entering real values to committe users.
    /// </summary>
    public class RealValueAlarmViewModel
    {
        /// <summary>
        /// Gets or sets the indicators expiration days. shows the indicator and number of days of expiration goes
        /// </summary>
        /// <value>
        /// The indicators expiration days.
        /// </value>
        public List<Tuple<CommitteeManagement.Model.Indicator,int>> IndicatorsExpirationDays { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public User User { get; set; } 
    }
}