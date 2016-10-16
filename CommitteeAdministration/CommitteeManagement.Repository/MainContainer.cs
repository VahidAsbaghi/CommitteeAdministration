using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommitteeManagement.Repository.Data;
using CommitteeManagement.Repository.Repositories;

namespace CommitteeManagement.Repository
{
    /// <summary>
    /// UnitOfWork main container of all repositories
    /// </summary>
    /// <seealso cref="CommitteeManagement.Repository.Data.RepositoryContainerBase" />
    /// <seealso cref="CommitteeManagement.Repository.IMainContainer" />
    public class MainContainer: RepositoryContainerBase,IMainContainer
    {
        private IDataContext _dataContext;
        /// <summary>
        /// Gets or sets the user repository.
        /// </summary>
        /// <value>
        /// The user repository.
        /// </value>
        public UserRepository UserRepository { get; set; }
        /// <summary>
        /// Gets or sets the committee repository.
        /// </summary>
        /// <value>
        /// The committee repository.
        /// </value>
        public CommitteeRepository CommitteeRepository { get; set; }
        /// <summary>
        /// Gets or sets the contact information repository.
        /// </summary>
        /// <value>
        /// The contact information repository.
        /// </value>
        public ContactInfoRepository ContactInfoRepository { get; set; }
        /// <summary>
        /// Gets or sets the visitor repository.
        /// </summary>
        /// <value>
        /// The visitor repository.
        /// </value>
        public VisitorRepository VisitorRepository { get; set; }
        /// <summary>
        /// Gets or sets the sub criterion modification repository.
        /// </summary>
        /// <value>
        /// The sub criterion modification repository.
        /// </value>
        public SubCriterionModificationRepository SubCriterionModificationRepository { get; set; }
        /// <summary>
        /// Gets or sets the sub criterion repository.
        /// </summary>
        /// <value>
        /// The sub criterion repository.
        /// </value>
        public SubCriterionRepository SubCriterionRepository { get; set; }
        /// <summary>
        /// Gets or sets the session repository.
        /// </summary>
        /// <value>
        /// The session repository.
        /// </value>
        public SessionRepository SessionRepository { get; set; }
        /// <summary>
        /// Gets or sets the role repository.
        /// </summary>
        /// <value>
        /// The role repository.
        /// </value>
        public RoleRepository RoleRepository { get; set; }
        /// <summary>
        /// Gets or sets the permission repository.
        /// </summary>
        /// <value>
        /// The permission repository.
        /// </value>
        public PermissionRepository PermissionRepository { get; set; }
        /// <summary>
        /// Gets or sets the indicator real value repository.
        /// </summary>
        /// <value>
        /// The indicator real value repository.
        /// </value>
        public IndicatorRealValueRepository IndicatorRealValueRepository { get; set; }
        /// <summary>
        /// Gets or sets the indicator modification repository.
        /// </summary>
        /// <value>
        /// The indicator modification repository.
        /// </value>
        public IndicatorModificationRepository IndicatorModificationRepository { get; set; }
        /// <summary>
        /// Gets or sets the indicator ideal value repository.
        /// </summary>
        /// <value>
        /// The indicator ideal value repository.
        /// </value>
        public IndicatorIdealValueRepository IndicatorIdealValueRepository { get; set; }
        /// <summary>
        /// Gets or sets the indicator repository.
        /// </summary>
        /// <value>
        /// The indicator repository.
        /// </value>
        public IndicatorRepository IndicatorRepository { get; set; }
        /// <summary>
        /// Gets or sets the criterion modification repository.
        /// </summary>
        /// <value>
        /// The criterion modification repository.
        /// </value>
        public CriterionModificationRepository CriterionModificationRepository { get; set; }
        /// <summary>
        /// Gets or sets the criterion repository.
        /// </summary>
        /// <value>
        /// The criterion repository.
        /// </value>
        public CriterionRepository CriterionRepository { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="MainContainer"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        public MainContainer(IDataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
            UserRepository=new UserRepository(_dataContext);
            CommitteeRepository=new CommitteeRepository(_dataContext);
            ContactInfoRepository=new ContactInfoRepository(_dataContext);
            CriterionRepository=new CriterionRepository(_dataContext);
            CriterionModificationRepository=new CriterionModificationRepository(_dataContext);
            IndicatorRepository=new IndicatorRepository(_dataContext);
            IndicatorIdealValueRepository=new IndicatorIdealValueRepository(_dataContext);
            IndicatorModificationRepository=new IndicatorModificationRepository(_dataContext);
            IndicatorRealValueRepository=new IndicatorRealValueRepository(_dataContext);
            PermissionRepository=new PermissionRepository(_dataContext);
            RoleRepository=new RoleRepository(_dataContext);
            SessionRepository=new SessionRepository(_dataContext);
            SubCriterionRepository=new SubCriterionRepository(_dataContext);
            SubCriterionModificationRepository=new SubCriterionModificationRepository(_dataContext);
            VisitorRepository=new VisitorRepository(_dataContext);

        }
        /// <summary>
        /// Entries the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public DbEntityEntry<T> Entry<T>(T entity) where T : class
        {
            return _dataContext.GetEntry(entity);
        }
        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        public new int SaveChanges()
        {
            return  _dataContext.SaveChanges();
        }
        /// <summary>
        /// Rolls the back.
        /// </summary>
        public new void RollBack()
        {
            _dataContext.Rollback();
        }
        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns></returns>
        public new Task<int> SaveChangesAsync()
        {
            return _dataContext.SaveChangesAsync();
        }
        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public new Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _dataContext.SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
