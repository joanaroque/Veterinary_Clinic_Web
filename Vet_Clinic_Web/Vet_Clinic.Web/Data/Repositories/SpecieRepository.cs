﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public class SpecieRepository : GenericRepository<Specie>, ISpecieRepository
    {
        private readonly DataContext _context;

        public SpecieRepository(DataContext context) : base(context)
        {
            _context = context;

        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Species.Include(s => s.CreatedBy);
        }

        public async Task<Specie> GetSpecieByIdAsync(int specieId)
        {
            var specie = await _context.Species
                .Include(s => s.CreatedBy)
                .Where(s => s.Id == specieId).FirstOrDefaultAsync();

            return specie;
        }

        public IEnumerable<SelectListItem> GetComboSpecies()
        {
            var list = _context.Species.Select(s => new SelectListItem
            {
                Text = s.Description,
                Value = $"{s.Id}"

            }).OrderBy(s => s.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a Specie...]",
                Value = "0"
            });

            return list;
        }
    }
}
