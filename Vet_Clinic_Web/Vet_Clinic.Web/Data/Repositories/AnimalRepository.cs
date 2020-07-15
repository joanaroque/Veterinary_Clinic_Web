using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public class AnimalRepository : GenericRepository<Animal>, IAnimalRepository
    {
        private readonly DataContext _context;

        public AnimalRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Animals.Include(p => p.User);
        }

        public IEnumerable<SelectListItem> GetComboAnimals()
        {
            var list = _context.Animals.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select an animal...)",
                Value = "0"
            });

            return list;
        }
    }
}
