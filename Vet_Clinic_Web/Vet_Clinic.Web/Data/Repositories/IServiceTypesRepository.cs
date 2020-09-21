using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public interface IServiceTypesRepository : IGenericRepository<ServiceType>
    {
        IQueryable GetAllWithUsers();


        IEnumerable<SelectListItem> GetComboServiceTypes();


        Task<ServiceType> GetServiceWithHistory(int id);
    }
}
