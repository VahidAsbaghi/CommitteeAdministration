using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitteeManagement.Model
{
    public class Session
    {
        public int Id { get; set; }
        public string HostAddress { get; set; }
        public string SessionKey { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User{ get; set; }
    }
}
