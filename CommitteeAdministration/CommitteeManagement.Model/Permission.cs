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
        }
        public int Id { get; set; }
        public bool IndicatorDeadlineAdjust { get; set; }
        public bool Criterion { get; set; }
        public bool SubCriterion { get; set; }
        public bool Indicator { get; set; }
        public bool AddRealIndicator { get; set; }
        public bool UpdateRealIndicator { get; set; }
        public bool Add { get; set; }
        public bool Delete { get; set; }
        public bool Update { get; set; }
        public virtual ICollection<Role> Roles{ get; set; }
    }
}
