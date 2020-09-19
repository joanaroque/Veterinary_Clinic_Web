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


        IEnumerable<SelectListItem> GetComboPets(int ownerId);


        Task<Owner> GetOwnerWithPetsAsync(int ownerId);


        Task AddPetAsync(Pet pet);


        Task<int> UpdatePetAsync(Pet pet);


        Task<Pet> GetPetAsync(int id);


        Task<int> DeletePetAsync(Pet pet);

    }
}
