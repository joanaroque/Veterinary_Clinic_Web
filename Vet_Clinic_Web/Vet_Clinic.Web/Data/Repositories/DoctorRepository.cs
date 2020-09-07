using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly DataContext _context;

        public DoctorRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable GetAllWithUsers()
        {
            return _context.Doctors.Include(p => p.CreatedBy);
        }

        public IEnumerable<SelectListItem> GetComboDoctors()
        {
            var list = _context.Doctors.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()

            }).OrderBy(p => p.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a doctor...]",
                Value = "0"
            });

            return list;
        }
    }
}
