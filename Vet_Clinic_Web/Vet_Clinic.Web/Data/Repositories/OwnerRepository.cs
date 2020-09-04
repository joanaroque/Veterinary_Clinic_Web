using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Owners.Include(p => p.User);
        }

        public IEnumerable<SelectListItem> GetComboOwners()
        {
            var list = _context.Owners.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()

            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a Owner...]",
                Value = "0"
            });

            return list;
        }

        public async Task<Owner> GetOwnersWithPetsAsync(int id)
        {
            return await _context.Owners
                        .Include(p => p.Pets)
                        .Where(p => p.Id == id)
                        .FirstOrDefaultAsync();
        }
    }

}
