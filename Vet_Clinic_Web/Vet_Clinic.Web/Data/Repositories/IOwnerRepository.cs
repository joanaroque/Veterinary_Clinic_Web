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

        /// <summary>
        /// fetch all owners, including the user who created
        /// </summary>
        /// <returns>all owners, including the user who created</returns>
        IQueryable GetAllWithUsers();



        /// <summary>
        ///  fetch all owners, select a new instance from a list of owners with name and id
        /// </summary>
        /// <returns>list of owners</returns>
        IEnumerable<SelectListItem> GetComboOwners();


        /// <summary>
        /// get the attributes of the owners, including attributes of the pets, by the id that is received
        /// </summary>
        /// <param name="ownerId">owner id</param>
        /// <returns>attributes of the owners, including attributes of the pets, by the id that is received</returns>
        Task<Owner> GetOwnerWithPetsAsync(int ownerId);


        /// <summary>
        /// get the attributes of the owners, including the user who created it, for the user received
        /// </summary>
        /// <param name="model"> model user</param>
        /// <returns>attributes of the owners, including the user who created it, for the user received</returns>
        Task<Owner> GetOwnerWithUserAsync(EditUserViewModel model);


        /// <summary>
        /// get the attributes of the owners, including the user who created it, for the user ID received
        /// </summary>
        /// <param name="userId">Id user</param>
        /// <returns>attributes of the owners, including the user who created it, for the user ID received</returns>
        Task<Owner> GetOwnerWithUserByIdAsync(int userId);


        /// <summary>
        /// get the attributes of owners, by the pet ID that is received
        /// </summary>
        /// <param name="ownerId">owner id</param>
        /// <returns>the details of the current owner</returns>
        Task<Owner> GetOwnerDetailsAsync(int ownerId);

        /// <summary>
        ///  get the attributes of the first owner, where the user received is the current user
        /// </summary>
        /// <param name="currentUser">current user</param>
        /// <returns>the attributes of the first owner, where the user received is the current user</returns>
        Task<Owner> GetCurrentUserOwner(string currentUser);


        /// <summary>
        ///  get the attributes of the first owner, where the name is the same as the one received
        /// </summary>
        /// <param name="identityName">identity Name</param>
        /// <returns>attributes of the first owner, where the name is the same as the one received
        /// </summary></returns>
        Task<Owner> GetFirstOwnerAsync(string identityName);

    }
}
