using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitteeManagement.Model
{
    public class SubCriterion
    {
        public SubCriterion()
        {
            SubCriterionModifications=new HashSet<SubCriterionModification>();
            Permissions=new HashSet<Permission>();
            Indicators=new HashSet<Indicator>();
        }
        public int Id { get; set; }
        public string Subject { get; set; }
        public double Coefficient { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public int CriterionId { get; set; }
        public virtual ICollection<SubCriterionModification> SubCriterionModifications { get; set; }
        [ForeignKey("CriterionId")]
        public virtual Criterion Criterion{ get; set; }
        public int? CommitteeId { get; set; }
        [ForeignKey("CommitteeId")]
        public virtual Committee Committee { get; set; }
        public virtual ICollection<Indicator> Indicators { get; set; }
        public virtual ICollection<Permission> Permissions{ get; set; }
    }
}
