
namespace CommitteeManagement.Repository.Data
{
    public interface IRepositoryContainerBase
    {
        int SaveChanges();
        void RollBack();
    }
}
