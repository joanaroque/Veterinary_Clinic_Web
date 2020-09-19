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

        public async Task GetDoctorAsync(int days)
        {
            DateTime initialDate;

            if (!_context.Appointments.Any())
            {
                initialDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
            }
            else
            {
                var agenda = _context.Appointments.LastOrDefault();
                initialDate = new DateTime(agenda.ScheduledDate.Year, agenda.ScheduledDate.Month, agenda.ScheduledDate.AddDays(1).Day, 8, 0, 0);
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
                            ScheduledDate = initialDate.ToUniversalTime(),
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

        //    public async Task AddAppointmentAsync(AppointmentViewModel model, string userName)
        //    {
        //        var user = await _userHelper.GetUserByEmailAsync(userName);
        //        if (user == null)
        //        {
        //            return;
        //        }

        //        var appointment = await _context.Appointments.FindAsync(model.Id);

        //        if (appointment != null)
        //        {
        //            appointment.IsAvailable = false;
        //            appointment.Owner = await _context.Owners.FindAsync(model.OwnerId);
        //            appointment.Pet = await _context.Pets.FindAsync(model.PetId);
        //            appointment.Doctor = await _context.Doctors.FindAsync(model.DoctorId);
        //            appointment.AppointmentObs = model.AppointmentObs;
        //            _context.Appointments.Update(appointment);
        //            await _context.SaveChangesAsync();
        //        }


        //        appointment = new Appointment
        //        {
        //            Doctor = doctor,
        //            Pet = Pet,
        //            Owner = Owner,
        //            User = user,
        //        };

        //        _context.Appointments.Add(appointment);

        //    }

        //    public async Task<IQueryable<Appointment>> GetAppointmentsAsync(string userName)
        //    {
        //        var user = await _userHelper.GetUserByEmailAsync(userName);

        //        if (user == null)
        //        {
        //            return null;
        //        }

        //        if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
        //        {
        //            return _context.Appointments
        //                .Include(o => o.Owner)
        //                .Include(p => p.Pet)
        //                .OrderByDescending(o => o.Date);
        //        }

        //        return _context.Appointments
        //                .Include(o => o.Owner)
        //                .Include(p => p.Pet)
        //                .Where(o => o.User == user)
        //                .OrderByDescending(o => o.Date);
        //    }


        //}
    }
}
