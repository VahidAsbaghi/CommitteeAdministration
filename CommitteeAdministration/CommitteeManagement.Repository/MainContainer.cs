using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommitteeManagement.Repository.Data;
using CommitteeManagement.Repository.Repositories;

namespace CommitteeManagement.Repository
{
    public class MainContainer: RepositoryContainerBase,IMainContainer
    {
        public UserRepository UserRepository { get; set; }
        public MainContainer(IDataContext dataContext) : base(dataContext)
        {
            UserRepository=new UserRepository(dataContext);
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void RollBack()
        {
            throw new NotImplementedException();
        }
    }
}
