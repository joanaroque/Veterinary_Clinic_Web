using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Data.Repositories
{
    public interface IOwnerRepository : IGenericRepository<Owner>
    {
        IQueryable GetAllWithUsers(); 

        IEnumerable<SelectListItem> GetComboOwners();

        Task<Owner> GetOwnersWithPetsAsync(int id);

    }
}
