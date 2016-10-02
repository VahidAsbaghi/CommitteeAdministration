using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommitteeManagement.Model;
using CommitteeManagement.Repository.Data;

namespace CommitteeManagement.Repository.Repositories
{
    public class ContactInfoRepository:Repository<ContactInfo>
    {
        public ContactInfoRepository(IDataContext dataContext) : base(dataContext)
        {
        }
    }
}
