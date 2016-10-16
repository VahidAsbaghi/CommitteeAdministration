using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommitteeManagement.Model;

namespace CommitteeAdministration.ViewModels
{
    /// <summary>
    /// View model that is used to add permission into db
    /// </summary>
    public class AddPermissionViewModel
    {
        /// <summary>
        /// Gets or sets the indicator deadline adjust.
        /// </summary>
        /// <value>
        /// The indicator deadline adjust.
        /// </value>
        public bool? IndicatorDeadlineAdjust { get; set; }
        /// <summary>
        /// Gets or sets the criterion.
        /// </summary>
        /// <value>
        /// The criterion.
        /// </value>
        public bool? Criterion { get; set; }
        /// <summary>
        /// Gets or sets the sub criterion.
        /// </summary>
        /// <value>
        /// The sub criterion.
        /// </value>
        public bool? SubCriterion { get; set; }
        /// <summary>
        /// Gets or sets the indicator.
        /// </summary>
        /// <value>
        /// The indicator.
        /// </value>
        public bool? Indicator { get; set; }
        /// <summary>
        /// Gets or sets the real indicator.
        /// </summary>
        /// <value>
        /// The real indicator.
        /// </value>
        public bool? RealIndicator { get; set; }
        /// <summary>
        /// Gets or sets the add.
        /// </summary>
        /// <value>
        /// The add.
        /// </value>
        public bool? Add { get; set; }
        /// <summary>
        /// Gets or sets the delete.
        /// </summary>
        /// <value>
        /// The delete.
        /// </value>
        public bool? Delete { get; set; }
        /// <summary>
        /// Gets or sets the update.
        /// </summary>
        /// <value>
        /// The update.
        /// </value>
        public bool? Update { get; set; }
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public List<Role> Roles { get; set; }
        /// <summary>
        /// Gets or sets the is contained role. to specify that this permission is about which roles
        /// </summary>
        /// <value>
        /// The is contained role.
        /// </value>
        public List<bool> IsContainedRole { get; set; }
    }
    /// <summary>
    /// Edit Permission View Model
    /// </summary>
    public class EditPermissionViewModel
    {
        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        /// <value>
        /// The permission.
        /// </value>
        public Permission Permission { get; set; }
        /// <summary>
        /// Gets or sets the is contained role. to specify that this permission is about which roles
        /// </summary>
        /// <value>
        /// The is contained role.
        /// </value>
        public List<bool> IsContainedRole { get; set; }
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public List<Role> Roles { get; set; }
    }
}