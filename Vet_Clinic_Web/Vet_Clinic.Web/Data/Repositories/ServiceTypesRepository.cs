using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Vet_Clinic.Web.Data.Repositories
{
    public class ServiceTypesRepository : IServiceTypesRepository
    {
        private readonly DataContext _context;

        public ServiceTypesRepository(DataContext context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.ServiceTypes.Include(p => p.User);
        }

        public IEnumerable<SelectListItem> GetComboServiceTypes()
        {
            var list = _context.ServiceTypes.Select(pt => new SelectListItem
            {
                Text = pt.Name,
                Value = $"{pt.Id}"
            })
                .OrderBy(pt => pt.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a service type...]",
                Value = "0"
            });

            return list;
        }
    }
}
