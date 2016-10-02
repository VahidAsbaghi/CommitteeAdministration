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

        public MainContainer(IDataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
            UserRepository=new UserRepository(dataContext);
            CommitteeRepository=new CommitteeRepository(dataContext);
            ContactInfoRepository=new ContactInfoRepository(dataContext);
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
