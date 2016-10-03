using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommitteeManagement.Model;
using CommitteeManagement.Repository.Data;

namespace CommitteeManagement.Repository.Repositories
{
    public class PermissionRepository:Repository<Permission>
    {
        public PermissionRepository(IDataContext dataContext) : base(dataContext)
        {
        }
    }
}
