using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public class AssistantRepository : GenericRepository<Assistant>, IAssistantRepository
    {
        private readonly DataContext _context;


        public AssistantRepository(DataContext context) : base(context)
        {
            _context = context;

        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Assistants.Include(p => p.User);
        }

        public IEnumerable<SelectListItem> GetComboAssistent()
        {
            var list = _context.Assistants.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a Assistant...]",
                Value = "0"
            });

            return list;
        }
    }
}

