using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitteeManagement.Model
{
    public class SubCriterionModification
    {
        public int Id { get; set; }
        public DateTime? Time { get; set; }
        public bool? Update { get; set; }
        public bool? Add { get; set; }
        public bool? Delete { get; set; }
        public string UserId { get; set; }
        public int? SubCriterionId { get; set; }
        [ForeignKey("UserId")]
        public virtual  User User{ get; set; }
        [ForeignKey("SubCriterionId")]
        public virtual SubCriterion SubCriterion{ get; set; }
    }
}
