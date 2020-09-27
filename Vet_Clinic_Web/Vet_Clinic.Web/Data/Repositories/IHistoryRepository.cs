using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public interface IHistoryRepository : IGenericRepository<History>
    {


        /// <summary>
        /// fetch all histories, including the user who created
        /// </summary>
        /// <returns>all histories, including the user who created</returns>
        IQueryable GetAllWithUsers();



        /// <summary>
        ///  get the stories where the pet associated with the history is equal to the pet sought
        /// </summary>
        /// <param name="petId">pet id</param>
        /// <returns>the pet associated with the history</returns>
        Task<List<History>> GetHistoriesFromPetIdAsync(int petId);

    }
}
