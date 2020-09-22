using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {

        IQueryable GetAllByDate();


        Task<Appointment> GetAllWithUsers(int id);


        Task<List<Doctor>> GetWorkingDoctorsAsync(int appointmentHour);


        Task<List<Doctor>> GetScheduledDoctorsAsync(DateTime scheduledDate);


        Task<List<Appointment>> GetAppointmentFromCurrentOwnerAsync(string currentUser);


    }
}
