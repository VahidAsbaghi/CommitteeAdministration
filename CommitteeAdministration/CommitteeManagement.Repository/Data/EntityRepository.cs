using System.Threading.Tasks;

namespace CommitteeManagement.Repository.Data
{
	public class EntityRepository<T> : Repository<T> where T : class, IEntity
    {
	    private readonly IDataContext _context;

	    public EntityRepository(IDataContext context) : base(context)
	    {
	        _context = context;
	    }

	    
        public async Task<T> FindByIdAsync(int id)
        {
            return await FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}