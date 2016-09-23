using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CommitteeManagement.Model
{
    public class User
    {
        public User()
        {
            Roles=new HashSet<Role>();
            Sessions=new HashSet<Session>();
            SubCriterionModifications=new HashSet<SubCriterionModification>();
            CriterionModifications=new HashSet<CriterionModification>();
        }

        // Primitive properties
        public int Id { get; set; }
        public GenderEnum  Gender { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastModificationDate { get; set; }
        public int CommitteeRefId { get; set; }
        [ForeignKey("CommitteeRefId")]
        public virtual Committee Committee { get; set; }
        public virtual Password Password{ get; set; }
        public virtual ContactInfo ContactInfo { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<SubCriterionModification> SubCriterionModifications { get; set; }
        public virtual ICollection<CriterionModification> CriterionModifications { get; set; }
    }

   
    /// <summary>
    /// 
    /// </summary>
    public enum GenderEnum
    {
        Male = 0,
        Female = 1
    }
}
