﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public class PetRepository : GenericRepository<Pet>, IPetRepository
    {
        private readonly DataContext _context;

        public PetRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Pets
                 .Include(p => p.Owner)
                 .ThenInclude(o => o.CreatedBy)
                 .Include(p => p.Specie)
                 .Include(p => p.Histories);
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

        public async Task<Pet> GetDetailsPetAsync(int petId)
        {
            var pet = await _context.Pets
                .Include(p => p.Owner)
                .Include(p => p.Specie)
                .Include(p => p.Histories)
                .FirstOrDefaultAsync(p => p.Id == petId);

            return pet;
        }

        public async Task<List<Pet>> GetPetFromOwnerAsync(int ownerId)
        {
            var ownerPets = await _context.Pets
                .Where(p => p.Owner.Id == ownerId).ToListAsync();

            return ownerPets;
        }

        public async Task<List<Pet>> GetPetFromCurrentOwnerAsync(string currentUser)
        {
            var pet = await _context.Pets
               .Include(p => p.Owner)
               .ThenInclude(p => p.User)
               .Where(p => p.Owner.User.Id == currentUser.ToString()).ToListAsync();

            return pet;
        }

        public async Task<List<Pet>> GetPetBySpecieAsync(int specieId)
        {
            var pets = await _context.Pets
                .Include(p => p.Specie)
                .Where(p => p.Specie.Id == specieId).ToListAsync();

            return pets;
        }
    }
}
