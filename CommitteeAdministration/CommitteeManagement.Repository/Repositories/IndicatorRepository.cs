using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommitteeManagement.Model;
using CommitteeManagement.Repository.Data;

namespace CommitteeManagement.Repository.Repositories
{
    public class IndicatorRepository:Repository<Indicator>
    {
        public IndicatorRepository(IDataContext dataContext) : base(dataContext)
        {
        }
    }
}
