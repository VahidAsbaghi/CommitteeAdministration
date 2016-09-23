using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitteeManagement.Model
{
    public class Indicator
    {
        public Indicator()
        {
            Permissions=new HashSet<Permission>();
            IndicatorModifications=new HashSet<IndicatorModification>();
            IndicatorIdealValues=new HashSet<IndicatorIdealValue>();
            IndicatorRealValues=new HashSet<IndicatorRealValue>();
        }
        public int Id { get; set; }
        public string Subject { get; set; }
        public double Coefficient { get; set; }
        public int DeadlinePeriod { get; set; }
        public bool IsDeleted { get; set; }
        public int SubCriterionId { get; set; }
        [ForeignKey("SubCriterionId")]
        public virtual SubCriterion SubCriterion{ get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<IndicatorModification> IndicatorModifications { get; set; }
        public virtual ICollection<IndicatorRealValue> IndicatorRealValues { get; set; }
        public virtual ICollection<IndicatorIdealValue> IndicatorIdealValues { get; set; }

    }
}
