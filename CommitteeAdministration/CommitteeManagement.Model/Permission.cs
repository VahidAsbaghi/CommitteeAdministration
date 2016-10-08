using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitteeManagement.Model
{
    public class Permission
    {
        public Permission()
        {
            Roles=new HashSet<Role>();
            Indicators=new HashSet<Indicator>();
            SubCriteria=new HashSet<SubCriterion>();
            Criteria=new HashSet<Criterion>();
        }
        public int Id { get; set; }
        public bool? IndicatorDeadlineAdjust { get; set; }
        public bool? Criterion { get; set; }
        public bool? SubCriterion { get; set; }
        public bool? Indicator { get; set; }
        public bool? RealIndicator { get; set; }       
        public bool? Add { get; set; }
        public bool? Delete { get; set; }
        public bool? Update { get; set; }
        public virtual ICollection<Role> Roles{ get; set; }
        public virtual ICollection<Criterion> Criteria { get; set; }
        public virtual ICollection<SubCriterion> SubCriteria { get; set; }
        public virtual ICollection<Indicator> Indicators { get; set; }
    }
}
