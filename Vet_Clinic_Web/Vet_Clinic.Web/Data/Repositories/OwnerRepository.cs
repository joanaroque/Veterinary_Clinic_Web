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

        public async Task AddPetAsync(Pet pet)
        {
            var owner = await GetOwnerWithPetsAsync(pet.Owner.Id);
            if (owner == null)
            {
                return;
            }

            owner.Pets.Add(pet);

            _context.Owners.Update(owner);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeletePetAsync(Pet pet)
        {
            var owner = await _context.Owners.
                Where(c => c.Pets.Any(p => p.Id == pet.Id))
                .FirstOrDefaultAsync();

            if (owner == null)
            {
                return 0;
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return owner.Id;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Owners.Include(p => p.CreatedBy);
        }

        public IEnumerable<SelectListItem> GetComboPets(int ownerId)
        {
            var list = _context.Pets.Where(p => p.Owner.Id == ownerId).Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()

            }).OrderBy(p => p.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a Pet...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboOwners()
        {
            var list = _context.Owners.Select(p => new SelectListItem
            {
                Text = p.User.FirstName,
                Value = p.Id.ToString()

            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a Owner...]",
                Value = "0"
            });

            return list;
        }

        public async Task<Owner> GetOwnerWithPetsAsync(int ownerId)
        {
            return await _context.Owners
                .Include(o => o.CreatedBy)
               .Include(o => o.Pets)
               .ThenInclude(p => p.Specie)
               .Include(o => o.Pets)
               .ThenInclude(p => p.Histories)
               .FirstOrDefaultAsync(m => m.Id == ownerId);
        }

        public async Task<Pet> GetPetAsync(int id)
        {
            return await _context.Pets.FindAsync(id);
        }

        public async Task<int> UpdatePetAsync(Pet pet)
        {
            var owner = _context.Owners
                .Where(o => o.Pets.Any(p => p.Id == pet.Id))
                .FirstOrDefault();

            if (owner == null)
            {
                return 0;
            }

            _context.Pets.Update(pet);
            await _context.SaveChangesAsync();

            return pet.Id;
        }
    }

}
