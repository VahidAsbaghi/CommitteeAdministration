using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace CommitteeManagement.Model
{
    public class Criterion
    {
        public Criterion()
        {
            SubCriteria=new HashSet<SubCriterion>();
            CriterionModifications=new HashSet<CriterionModification>();
            Permissions=new HashSet<Permission>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا معیار مورد نظر را وارد کنید")]
        public string Subject { get; set; }
        [Required]
        public double? Coefficient { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CommitteeId { get; set; }
        [ForeignKey("CommitteeId")]
        public virtual Committee Committee { get; set; }
        public virtual  ICollection<SubCriterion>  SubCriteria{ get; set; }
        public virtual ICollection<CriterionModification> CriterionModifications { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
