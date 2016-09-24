using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitteeManagement.Model
{
    public class IndicatorRealValue
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public DateTime Time { get; set; }
        public int IndicatorId { get; set; }
        [ForeignKey("IndicatorId")]
        public virtual Indicator Indicator{ get; set; }
    }
}
