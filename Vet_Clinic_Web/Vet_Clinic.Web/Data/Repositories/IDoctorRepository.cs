using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        /// <summary>
        /// fetch all doctors, including the user who created
        /// </summary>
        /// <returns>all doctors, including the user who created</returns>
        IQueryable GetAllWithUsers();



        /// <summary>
        ///  fetch all doctors, select a new instance from a list of doctors with name and id
        /// </summary>
        /// <returns>list of doctors</returns>
        IEnumerable<SelectListItem> GetComboDoctors();




        Task<Doctor> GetDoctorByIdAsync(int doctorId);

    }
}
