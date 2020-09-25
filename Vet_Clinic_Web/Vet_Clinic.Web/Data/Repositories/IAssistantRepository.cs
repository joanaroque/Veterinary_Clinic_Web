using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;
using System.Linq;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public interface IAssistantRepository : IGenericRepository<Assistant>
    {

        /// <summary>
        ///  fetch all assistants, including the user who created
        /// </summary>
        /// <returns>all assistants, including the user who created</returns>
        IQueryable GetAllWithUsers();


        /// <summary>
        ///  fetch all assistants, select a new instance from a list of assistants with name and id
        /// </summary>
        /// <returns>a list of assistants</returns>
        IEnumerable<SelectListItem> GetComboAssistent();
    }
}
