using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Data.Repositories;

namespace Vet_Clinic.Web
{
    public class ServiceTypesRepository : GenericRepository<ServiceType>, IServiceTypesRepository
    {
        private readonly DataContext _context;

        public ServiceTypesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.ServiceTypes.Include(p => p.CreatedBy);
        }

        public IEnumerable<SelectListItem> GetComboServiceTypes()
        {
            var list = _context.ServiceTypes.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a Service...]",
                Value = "0"
            });

            return list;
        }

        public async Task<ServiceType> GetServiceWithHistory(int id)
        {
           return await _context.ServiceTypes
                 .Include(pt => pt.Histories)
                .FirstOrDefaultAsync(pt => pt.Id == id);
        }
    }
}