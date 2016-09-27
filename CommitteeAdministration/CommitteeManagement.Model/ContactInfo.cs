using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitteeManagement.Model
{
    public class ContactInfo
    {
        [Key, ForeignKey("User"),Required]
        public int UserId { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Url]
        public string PhotoLink { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public virtual  User User{ get; set; }
    }
}
