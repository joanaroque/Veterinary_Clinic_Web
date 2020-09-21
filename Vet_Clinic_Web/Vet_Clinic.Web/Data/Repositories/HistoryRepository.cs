using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public class HistoryRepository : GenericRepository<History>, IHistoryRepository
    {
        private readonly DataContext _context;

        public HistoryRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Histories.Include(p => p.CreatedBy);
        }

        public async Task<History> GetHistoryWithPets(int petId)
        {
            var history = await _context.Histories
                .Include(h => h.Pet)
                .Include(h => h.ServiceType)
                .FirstOrDefaultAsync(p => p.Id == petId);

            return history;
        }
    }
}
