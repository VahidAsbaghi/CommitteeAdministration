using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitteeManagement.Model
{
    public class Password
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }
        public string PasswordPhrase { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual User User { get; set; }

    }

}
