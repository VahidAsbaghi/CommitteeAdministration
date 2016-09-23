using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitteeManagement.Model
{
    public class CriterionModification
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public bool Update { get; set; }
        public bool Add { get; set; }
        public bool Delete { get; set; }
        public int UserId { get; set; }
        public int CriterionId { get; set; }
        [ForeignKey("CriterionId")]
        public virtual Criterion Criterion { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
