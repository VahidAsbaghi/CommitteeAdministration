using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommitteeManagement.Repository.Data;
using CommitteeManagement.Repository.Repositories;

namespace CommitteeManagement.Repository
{
    public interface IMainContainer:IRepositoryContainerBase,IDisposable
    {
        UserRepository UserRepository { get; set; }
        CommitteeRepository CommitteeRepository { get; set; }
        ContactInfoRepository ContactInfoRepository { get; set; }
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
