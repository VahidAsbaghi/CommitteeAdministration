using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitteeManagement.Model
{
    public class Committee
    {
        public Committee()
        {
            Users=new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; } 
    }
}
