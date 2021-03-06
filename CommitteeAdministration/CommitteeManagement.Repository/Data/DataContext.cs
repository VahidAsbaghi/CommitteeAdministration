using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommitteeManagement.Model;
using CommitteeManagement.Repository.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;


namespace CommitteeManagement.Repository.Data
{
	/// This class can be replaced if you already have a database context.  This is for example
	/// purposes, but it perfectly usable as your actual data context.
	/// Note, you should at least change "DataContext" to a name that matches your domain.
    public partial class DataContext : IdentityDbContext<User>, IDataContext
    {
	    public DataContext():base("name=DefaultConnection")
	    {
            Configuration.LazyLoadingEnabled = true;
            Database.SetInitializer(new Configuration());

        }
      
        // Example of a table in the form of a data set.  Add your own
        // database/model entities here.
        public DbSet<BogusObject> BogusObjects { get; set; }
       // public DbSet<User> User { get; set; }
	    //public override IDbSet<User> Users { get; set; }
        #region IDataContext Members
           //public DbSet<Role> Roles1 { get; set; } 
        public IDbSet<T> GetDbSet<T>() where T : class
        {
            return Set<T>();
        }

        public DbEntityEntry<T> GetEntry<T>(T entity) where T : class
        {
            return Entry(entity);
        }

        public new int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.Write($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.Write($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }
                throw;
            }

        }

        public new Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public new Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

	    public void Rollback()
	    {
            ChangeTracker.DetectChanges();

            var entries = ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged).ToList();

            foreach (var dbEntityEntry in entries)
            {
                var entity = dbEntityEntry.Entity;

                if (entity == null) continue;

                switch (dbEntityEntry.State)
                {
                    case EntityState.Added:
                        {
                        var set = Set(entity.GetType());
                        set.Remove(entity);
                        }
                        break;

                    case EntityState.Modified:
                        Entry(dbEntityEntry.Entity).CurrentValues.SetValues(Entry(dbEntityEntry.Entity).OriginalValues);
                        Entry(dbEntityEntry.Entity).State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        Entry(dbEntityEntry).CurrentValues.SetValues(Entry(dbEntityEntry).OriginalValues);
                        dbEntityEntry.State = EntityState.Unchanged;
                        break;
                }
            }
	    }

        public System.Data.Entity.DbSet<CommitteeManagement.Model.Committee> Committees { get; set; }

        public System.Data.Entity.DbSet<CommitteeManagement.Model.ContactInfo> ContactInfoes { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public System.Data.Entity.DbSet<CommitteeManagement.Model.Indicator> Indicators { get; set; }

        public System.Data.Entity.DbSet<CommitteeManagement.Model.SubCriterion> SubCriterions { get; set; }

        public System.Data.Entity.DbSet<CommitteeManagement.Model.Criterion> Criteria { get; set; }

        public System.Data.Entity.DbSet<CommitteeManagement.Model.IndicatorRealValue> IndicatorRealValues { get; set; }

        public DbSet<IndicatorIdealValue> IndicatorIdealValues { get; set; }

	    public DbSet<IndicatorModification> IndicatorModifications { get; set; }
	    public DbSet<CriterionModification> CriterionModifications { get; set; }
	    public DbSet<SubCriterionModification> SubCriterionModifications { get; set; }
	    public DbSet<Visitor> Visitors { get; set; }
	    public DbSet<Session> Sessions { get; set; }

      

        // public System.Data.Entity.DbSet<CommitteeManagement.Model.Password> Passwords { get; set; }
        #endregion

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    // Your custom model configurations go here.
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}