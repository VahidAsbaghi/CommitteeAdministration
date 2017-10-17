using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace CommitteeManagement.Model
{
    public class User : IdentityUser
    {
        public User()
        {
            NewRoles=new HashSet<Role>();
            Sessions=new HashSet<Session>();
            SubCriterionModifications=new HashSet<SubCriterionModification>();
            CriterionModifications=new HashSet<CriterionModification>();
            IndicatorModifications=new HashSet<IndicatorModification>();
            Visitors=new HashSet<Visitor>();
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            userIdentity.AddClaim(new Claim("CustomUserName", Name +" " + LastName));
            return userIdentity;
        }
        // Primitive properties
        //public string Id { get; set; }
        public GenderEnum  Gender { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime? CreatedTime { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public int? CommitteeRefId { get; set; }
        [ForeignKey("CommitteeRefId")]
        public virtual Committee Committee { get; set; }
        public virtual ContactInfo ContactInfo { get; set; }
        public virtual ICollection<Role> NewRoles { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<SubCriterionModification> SubCriterionModifications { get; set; }
        public virtual ICollection<CriterionModification> CriterionModifications { get; set; }
        public virtual ICollection<IndicatorModification> IndicatorModifications { get; set; }
        public virtual ICollection<Visitor> Visitors { get; set; }
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
