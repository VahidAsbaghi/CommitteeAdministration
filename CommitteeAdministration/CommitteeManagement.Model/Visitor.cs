using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CommitteeManagement.Model
{
    public class Visitor
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }
        public string BrowserVersion { get; set; }
        public DateTime Time { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User{ get; set; }
    }
}
