﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Models;

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
            return _context.Owners
                .Include(o => o.User)
                .Include(o => o.Pets)
                .Include(o => o.Appointments);
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

        public async Task<Owner> GetCurrentUserOwner(string currentUser)
        {
            var owner = await _context.Owners
                       .Where(a => a.User.Id == currentUser.ToString())
                       .FirstOrDefaultAsync();

            return owner;
        }

        public async Task<Owner> GetFirstOwnerAsync(string identityName)
        {
            var owner = await _context.Owners
                 .FirstOrDefaultAsync(o => o.User.Email.ToLower()
                 .Equals(identityName));

            return owner;
        }

        public async Task<Owner> GetOwnerDetailsAsync(int ownerId)
        {
            var owner = await _context.Owners
               .Include(o => o.User)
               .Include(o => o.Appointments)
               .Include(o => o.Pets)
               .ThenInclude(p => p.Specie)
               .Include(o => o.Pets)
               .FirstOrDefaultAsync(m => m.Id == ownerId);

            return owner;
        }

        public async Task<Owner> GetOwnerWithPetsAsync(int ownerId)
        {
            return await _context.Owners
                .Include(o => o.CreatedBy)
               .Include(o => o.Pets)
               .ThenInclude(p => p.Specie)
               .Include(o => o.Pets)
               .FirstOrDefaultAsync(m => m.Id == ownerId);
        }

        public async Task<Owner> GetOwnerWithUserByIdAsync(int ownerId)
        {
            var owner = await _context.Owners
                    .Include(o => o.User)
                     .FirstOrDefaultAsync(o => o.Id == ownerId);

            return owner;
        }
    }

}
