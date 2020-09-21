using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public interface IHistoryRepository : IGenericRepository<History>
    {
        IQueryable GetAllWithUsers();


        Task<History> GetHistoryWithPets(int petId);

    }
}
