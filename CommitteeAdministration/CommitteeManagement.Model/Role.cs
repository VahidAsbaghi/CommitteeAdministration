using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CommitteeManagement.Model
{
    public class Role:IdentityRole
    {
        public Role()
        {
            //Users=new HashSet<User>();
            Permissions=new HashSet<Permission>();
        }
       // public int Id { get; set; }
      //  public string Name { get; set; }
      //[NotMapped]
       // public DateTime? CreationDate { get; set; }
      //  public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }

        
    }
}
