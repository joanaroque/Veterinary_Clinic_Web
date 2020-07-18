using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Helpers;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Data.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;


        public AppointmentRepository(DataContext context,
            IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;

        }

        public async Task AddDaysAsync(int days)
        {
            DateTime initialDate;

            if (!_context.Appointments.Any())
            {
                initialDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
            }
            else
            {
                var agenda = _context.Appointments.LastOrDefault();
                initialDate = new DateTime(agenda.AppointmentSchedule.Year, agenda.AppointmentSchedule.Month, agenda.AppointmentSchedule.AddDays(1).Day, 8, 0, 0);
            }

            var finalDate = initialDate.AddDays(days);
            while (initialDate < finalDate)
            {
                if (initialDate.DayOfWeek != DayOfWeek.Saturday && initialDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    var finalDate2 = initialDate.AddHours(10);
                    while (initialDate < finalDate2)
                    {
                        _context.Appointments.Add(new Appointment
                        {
                            AppointmentSchedule = initialDate.ToUniversalTime(),
                        });


                        initialDate = initialDate.AddMinutes(30);
                    }

                    initialDate = initialDate.AddHours(14);
                }
                else
                {
                    initialDate = initialDate.AddDays(1);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddAppointmentAsync(AppointmentViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            var appointment = await _context.Appointments.FindAsync(model.Id);

            if (appointment != null)
            {
                appointment.IsAvailable = false;
                appointment.Owner = await _context.Owners.FindAsync(model.OwnerId);
                appointment.Pet = await _context.Pets.FindAsync(model.PetId);
                appointment.AppointmentObs = model.AppointmentObs;
                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();
            }


            //appointment = new Appointment
            //{
            //    Doctor = doctor,
            //    Pet = Pet,
            //    Owner = Owner,
            //    User = user,
            //};

            //_context.Appointments.Add(appointment);


            

        }

        public async Task<IQueryable<Appointment>> GetAppointmentsAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return _context.Appointments
                    .Include(o => o.Owner)
                    .Include(p => p.Pet)
                    .OrderByDescending(o => o.AppointmentSchedule);
            }

            return _context.Appointments
                    .Include(o => o.Owner)
                    .Include(p => p.Pet)
                    .Where(o => o.User == user)
                    .OrderByDescending(o => o.AppointmentSchedule);
        }

        public async Task<IQueryable<Appointment>> GetDetailTempsAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }

            return _context.Appointments
                    .Include(a => a.Doctor)
                    .Include(a => a.Pet)
                    .Include(a => a.Owner)
                    .Where(a => a.User == user)
                    .OrderByDescending(o => o.Pet.Name);
        }

        public async Task ModifyAppointmentAsync(int id, double quantity)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return;
            }

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }
    }
}
