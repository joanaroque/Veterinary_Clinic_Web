using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
   public interface IPetRepository : IGenericRepository<Pet>
    {

        IEnumerable<SelectListItem> GetComboPets();

    }
}
