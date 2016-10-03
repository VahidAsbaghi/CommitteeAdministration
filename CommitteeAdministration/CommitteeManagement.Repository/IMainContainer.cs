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
        VisitorRepository VisitorRepository { get; set; }
        SubCriterionModificationRepository  SubCriterionModificationRepository { get; set; }
        SubCriterionRepository SubCriterionRepository { get; set; }
        SessionRepository SessionRepository { get; set; }
        RoleRepository RoleRepository { get; set; }
        PermissionRepository PermissionRepository { get; set; }
        IndicatorRealValueRepository IndicatorRealValueRepository { get; set; }
        IndicatorModificationRepository IndicatorModificationRepository { get; set; }
        IndicatorIdealValueRepository IndicatorIdealValueRepository { get; set; }
        IndicatorRepository IndicatorRepository { get; set; }
        CriterionModificationRepository CriterionModificationRepository { get; set; }
        CriterionRepository CriterionRepository { get; set; }
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
