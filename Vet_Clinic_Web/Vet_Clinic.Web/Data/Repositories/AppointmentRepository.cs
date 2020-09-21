using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly DataContext _context;


        public AppointmentRepository(DataContext context) : base(context)
        {
            _context = context;
        }    

        public IQueryable GetAllWithUsers()
        {
            return _context.Appointments.Include(p => p.CreatedBy);
        }

        public IEnumerable<SelectListItem> GetComboAppointment()
        {
            var list = _context.Appointments.Select(p => new SelectListItem
            {
                Text = p.CreatedBy.FirstName,
                Value = p.Id.ToString()

            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select an Appointment...]",
                Value = "0"
            });

            return list;
        }

        public async Task<Doctor> GetDoctorAsync(DateTime scheduledDate)
        {
            int appointmentHour = scheduledDate.Hour;

            var workingDoctors = await _context.Doctors
                .Where(d => d.WorkStart <= appointmentHour && d.WorkEnd > appointmentHour)
                .ToListAsync();

            var doctorsAlreadyScheduled = await _context.Appointments
                    .Where(a => a.ScheduledDate.Equals(scheduledDate))
                    .Select(a => a.Doctor).ToListAsync();

            var doctorsNotScheduled = workingDoctors.Except(doctorsAlreadyScheduled);


            return doctorsNotScheduled.FirstOrDefault();
        }
    }
}
