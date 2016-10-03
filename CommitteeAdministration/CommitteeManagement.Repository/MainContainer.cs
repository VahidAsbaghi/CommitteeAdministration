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
    public class MainContainer: RepositoryContainerBase,IMainContainer
    {
        private readonly IDataContext _dataContext;
        public UserRepository UserRepository { get; set; }
        public CommitteeRepository CommitteeRepository { get; set; }
        public ContactInfoRepository ContactInfoRepository { get; set; }
        public VisitorRepository VisitorRepository { get; set; }
        public SubCriterionModificationRepository SubCriterionModificationRepository { get; set; }
        public SubCriterionRepository SubCriterionRepository { get; set; }
        public SessionRepository SessionRepository { get; set; }
        public RoleRepository RoleRepository { get; set; }
        public PermissionRepository PermissionRepository { get; set; }
        public IndicatorRealValueRepository IndicatorRealValueRepository { get; set; }
        public IndicatorModificationRepository IndicatorModificationRepository { get; set; }
        public IndicatorIdealValueRepository IndicatorIdealValueRepository { get; set; }
        public IndicatorRepository IndicatorRepository { get; set; }
        public CriterionModificationRepository CriterionModificationRepository { get; set; }
        public CriterionRepository CriterionRepository { get; set; }

        public MainContainer(IDataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
            UserRepository=new UserRepository(dataContext);
            CommitteeRepository=new CommitteeRepository(dataContext);
            ContactInfoRepository=new ContactInfoRepository(dataContext);
            CriterionRepository=new CriterionRepository(dataContext);
            CriterionModificationRepository=new CriterionModificationRepository(dataContext);
            IndicatorRepository=new IndicatorRepository(dataContext);
            IndicatorIdealValueRepository=new IndicatorIdealValueRepository(dataContext);
            IndicatorModificationRepository=new IndicatorModificationRepository(dataContext);
            IndicatorRealValueRepository=new IndicatorRealValueRepository(dataContext);
            PermissionRepository=new PermissionRepository(dataContext);
            RoleRepository=new RoleRepository(dataContext);
            SessionRepository=new SessionRepository(dataContext);
            SubCriterionRepository=new SubCriterionRepository(dataContext);
            SubCriterionModificationRepository=new SubCriterionModificationRepository(dataContext);
            VisitorRepository=new VisitorRepository(dataContext);

        }
        public DbEntityEntry<T> Entry<T>(T entity) where T : class
        {
            return _dataContext.GetEntry(entity);
        }
        public new int SaveChanges()
        {
            return  _dataContext.SaveChanges();
        }

        public new void RollBack()
        {
            _dataContext.Rollback();
        }

        public new Task<int> SaveChangesAsync()
        {
            return _dataContext.SaveChangesAsync();
        }

        public new Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _dataContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
