using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Data.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        //Task<IQueryable<Appointment>> GetAppointmentsAsync(string userName);

        //Task AddAppointmentAsync(AppointmentViewModel model, string userName);

        Task GetDoctorAsync(int days);

        IQueryable GetAllWithUsers();

        IEnumerable<SelectListItem> GetComboAppointment();
    }
}
