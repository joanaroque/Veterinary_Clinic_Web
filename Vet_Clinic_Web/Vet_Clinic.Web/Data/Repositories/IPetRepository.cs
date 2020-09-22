using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public interface IPetRepository : IGenericRepository<Pet>
    {
        IQueryable GetAllWithUsers();


        IEnumerable<SelectListItem> GetComboPets(int ownerId);



        Task<Pet> GetDetailsPetAsync(int petId);



        Task<List<Pet>> GetPetFromOwnerAsync(int ownerId);



        Task<List<Pet>> GetPetFromCurrentOwnerAsync(string currentUser);



    }
}
