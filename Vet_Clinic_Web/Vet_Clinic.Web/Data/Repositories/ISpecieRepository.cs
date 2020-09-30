using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public interface ISpecieRepository : IGenericRepository<Specie>
    {

        /// <summary>
        /// fetch all species, including the user who created
        /// </summary>
        /// <returns>all species, including the user who created</returns>
        IQueryable GetAllWithUsers();


        /// <summary>
        ///  fetch all species, from the especific owner, select a new instance from a list of species with name and id
        /// </summary>
        /// <returns>list of species</returns>
        IEnumerable<SelectListItem> GetComboSpecies();



        /// <summary>
        /// get the species including the user, where the id is the one entered
        /// </summary>
        /// <param name="specieId">specie Id</param>
        /// <returns>the species including the user, where the id is the one entered</returns>
        Task<Specie> GetSpecieByIdAsync(int specieId);


    }
}
