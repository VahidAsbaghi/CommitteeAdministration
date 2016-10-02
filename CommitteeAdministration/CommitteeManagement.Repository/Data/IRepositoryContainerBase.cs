
using System.Threading;
using System.Threading.Tasks;

namespace CommitteeManagement.Repository.Data
{
    public interface IRepositoryContainerBase
    {
        int SaveChanges();
        void RollBack();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
