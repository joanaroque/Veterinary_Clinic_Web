using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        /// <summary>
        ///   get the attributes where the appointments are from today onwards
        /// </summary>
        /// <returns>the appointments from today onwards</returns>
        IQueryable GetAllByDate();

        /// <summary>
        ///  get the attributes of Appointments, by the pet ID that is received
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>the details of the current Appointment</returns>
        Task<Appointment> GetAllWithUsers(int id);

        /// <summary>
        ///   picks up doctors 'working hours, where the appointment request time is between doctors' working hours
        /// </summary>
        /// <param name="appointmentHour">request hour</param>
        /// <returns>appointment request time</returns>
        Task<List<Doctor>> GetWorkingDoctorsAsync(int appointmentHour);

        /// <summary>
        ///  fetch queries where the scheduled appointment date is the same as the appointment date request
        /// </summary>
        /// <param name="scheduledDate">appointment date request</param>
        /// <returns>same as the appointment date request</returns>
        Task<List<Doctor>> GetScheduledDoctorsAsync(DateTime scheduledDate);



        /// <summary>
        ///  get the attributes of queries where the dates are future and the user is the current one
        /// </summary>
        /// <param name="currentUser">current user</param>
        /// <returns>appointment from current user </returns>
        Task<List<Appointment>> GetAppointmentFromCurrentOwnerAsync(string currentUser);


    }
}
