using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommitteeManagement.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CommitteeManagement.Repository.Data
{
    /// <summary>
    /// the Database initializer is used to provide seed data 
    /// seed data is used for test purposes or for example generate admin user at start and so on
    /// </summary>
    /// <seealso cref="System.Data.Entity.DropCreateDatabaseIfModelChanges{DataContext}" />
    public class DbInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        /// <summary>
        /// Seeds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void Seed(DataContext context)
        {
#region ******** add Role Committee and User of super admin user ********

            var role = new Role() { Name = "SuperAdmin" };
            context.Roles.Add(role); //add role to db to get id
            context.SaveChanges();
            var committee = new Committee() { Name = "SuperAdminCommittee" };
            context.Committees.Add(committee); //add committee to get id
            context.SaveChanges();
            var getCommittee = context.Committees.FirstOrDefault(c=>c.Name==committee.Name);
            if (getCommittee == null) return;

            var user = new User { UserName = "vahid.asbaghi@gmail.com", Email = "vahid.asbaghi@gmail.com", CommitteeRefId = getCommittee.Id, Committee = getCommittee };
            using (var userManager = new UserManager<User>(new UserStore<User>(context)))
            {
                var result = userManager.Create<User,string>(user, "Aa@38282469892"); //add user using userManager to hash password
                if (!result.Succeeded)
                {
                    return;
                }
            }

            var getUser = context.Users.FirstOrDefault(userT=>userT.UserName== "vahid.asbaghi@gmail.com");//get added user
            if (getUser == null) return;
            var identityRole = context.Roles.FirstOrDefault(roleT=>roleT.Name==role.Name);//get added role
            if (identityRole != null)
            {
                var newRole = new IdentityUserRole() { RoleId = identityRole.Id, UserId = getUser.Id  }; //create identityUserRole to add to roles and users tables
                user.Roles.Add(newRole);
                role.Users.Add(newRole);
            }
            context.Users.Attach(user);
            context.Roles.Attach(role);
            context.SaveChanges();
#endregion
        }
    }
}
