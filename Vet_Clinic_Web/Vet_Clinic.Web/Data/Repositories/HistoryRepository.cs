using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
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

 

        public async Task<List<History>> GetHistoriesFromPetId(int petId)
        {

            return await _context.Histories.Where(p => p.Pet.Id == petId).ToListAsync();
        }
    }
}
