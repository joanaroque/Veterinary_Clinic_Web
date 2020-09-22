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

        public IQueryable GetAllByDate()
        {
            return _context.Appointments
                .Include(a => a.CreatedBy)
                .Include(a => a.Owner)
                .ThenInclude(o => o.User)
                .Include(a => a.Pet)
                .Include(a => a.Doctor)
                .Where(a => a.ScheduledDate >= DateTime.Today.ToUniversalTime());
        }


        public Task<Appointment> GetAllWithUsers(int id)
        {
            return _context.Appointments
                     .Include(p => p.Doctor)
                     .Include(p => p.Pet)
                     .Include(p => p.Owner)
                     .ThenInclude(p => p.User)
                      .FirstOrDefaultAsync(p => p.Id == id);
        }


        public  async Task<List<Doctor>> GetWorkingDoctorsAsync(int appointmentHour)
        {
            var workingDoctors = await _context.Doctors
                .Where(d => d.WorkStart <= appointmentHour && d.WorkEnd > appointmentHour)
                .ToListAsync();

            return workingDoctors;
        }


        public async Task<List<Doctor>> GetScheduledDoctorsAsync(DateTime scheduledDate)
        {
            var doctorsAlreadyScheduled = await _context.Appointments
                    .Where(a => a.ScheduledDate.Equals(scheduledDate))
                    .Select(a => a.Doctor).ToListAsync();

            return doctorsAlreadyScheduled;
        }

        public async Task<List<Appointment>> GetAppointmentFromCurrentOwnerAsync(string currentUser)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Owner)
                .ThenInclude(o => o.User)
                .Include(a => a.Pet)
                .Include(a => a.Doctor)
                .Where(a => a.ScheduledDate >= DateTime.Today.ToUniversalTime())
                .Where(a => a.Owner.User.Id == currentUser.ToString()).ToListAsync();


            return appointments;
        }
    }
}
