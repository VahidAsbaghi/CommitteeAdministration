using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitteeManagement.Model
{
    public class IndicatorIdealValue
    {
        public int Id { get; set; }
        [Required]
        public double Value { get; set; }
        public DateTime? Time { get; set; }
        public bool LowerThan { get; set; }
        public bool MoreThan { get; set; }
        public int? IndicatorId { get; set; }
        [ForeignKey("IndicatorId")]
        public virtual Indicator Indicator { get; set; }
    }
}
