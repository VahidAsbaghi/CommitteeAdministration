using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitteeManagement.Model
{
    public class IndicatorModification
    {
        public int Id { get; set; }
        public DateTime? Time { get; set; }
        public bool? AddIndicator { get; set; }
        public bool? UpdateIndicator { get; set; }
        public bool? DeleteIndicator { get; set; }
        public bool? AddRealValue { get; set; }
        public bool? UpdateRealValue { get; set; }
        public bool? AddIdealValue { get; set; }
        public bool? UpdateIdealValue { get; set; }
        public string UserId { get; set; }
        public int? IndicatorId { get; set; }
        [ForeignKey("UserId")]
        public virtual  User User{ get; set; }
        [ForeignKey("IndicatorId")]
        public virtual Indicator Indicator { get; set; }
    }
}
