using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public interface IPetRepository : IGenericRepository<Pet>
    {

        /// <summary>
        /// fetch all pets, including the user who created
        /// </summary>
        /// <returns>all pets, including the user who created</returns>
        IQueryable GetAllWithUsers();

        /// <summary>
        ///  fetch all pets, from the especific owner, select a new instance from a list of pets with name and id
        /// </summary>
        /// <returns>list of pets from that owner</returns>
        IEnumerable<SelectListItem> GetComboPets(int ownerId);


        /// <summary>
        ///  get the attributes of pets, by the pet ID that is received
        /// </summary>
        /// <param name="petId">pet id</param>
        /// <returns> the details of the current pet</returns>
        Task<Pet> GetDetailsPetAsync(int petId);


        /// <summary>
        ///   pick up the pets of the specific owner that is received
        /// </summary>
        /// <param name="ownerId"> owner id</param>
        /// <returns>pets from that owner</returns>
        Task<List<Pet>> GetPetFromOwnerAsync(int ownerId);


        /// <summary>
        ///  fetch the pets of the owner who is received, including the user who created him
        /// </summary>
        /// <param name="currentUser"> current user</param>
        /// <returns>the pets of the owner who is received, including the user who created him</returns>
        Task<List<Pet>> GetPetFromCurrentOwnerAsync(string currentUser);


        //todo comentar
        Task<List<Pet>> GetPetBySpecieAsync(int specieId);



        Task<Pet> GetPetByAsync(int petId);
    }
}
