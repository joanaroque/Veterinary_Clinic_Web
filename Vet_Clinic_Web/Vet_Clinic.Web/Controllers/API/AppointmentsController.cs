using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Helpers;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public AppointmentsController(IAppointmentRepository appointmentRepository,
            DataContext context,
            IConverterHelper converterHelper)
        {
            _appointmentRepository = appointmentRepository;
            _context = context;
            _converterHelper = converterHelper;
        }

        [HttpGet]
        public IActionResult GetAppointment()
        {
            return Ok(_appointmentRepository.GetAll());
        }

        [HttpPost]
        [Route("GetAppointmentForOwner")]
        public async Task<IActionResult> GetAppointmentForOwner(OwnerViewModel email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointments = await _context.Appointments
                .Include(a => a.Owner)
                .ThenInclude(o => o.User)
                .Include(a => a.Pet)
                .Where(a => a.Date >= DateTime.Today.ToUniversalTime())
                .OrderBy(a => a.Date)
                .ToListAsync();

            var response = new List<AppointmentViewModel>();
            foreach (var appointment in appointments)
            {
                var agenda = new AppointmentViewModel
                {
                    Date = appointment.Date,
                    Id = appointment.Id,
                    IsAvailable = appointment.IsAvailable
                };

                if (appointment.Owner != null)
                {
                    if (appointment.Owner.User.Email.ToLower().Equals(email.User.Email.ToLower()))
                    {
                        agenda.Owner = _converterHelper.ToOwnerViewModel(agenda.Owner);
                        agenda.Pet = _converterHelper.ToPetViewModel(agenda.Pet);
                        agenda.AppointmentObs = agenda.AppointmentObs;

                    }
                    else
                    {
                        agenda.Owner = new OwnerViewModel { Name = "Reserved" };
                    }
                }

                response.Add(agenda);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("ScheduleAppointment")]
        public async Task<IActionResult> ScheduleAppointment(AppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = await _context.Appointments.FindAsync(model.Id);
            if (appointment == null)
            {
                return BadRequest("Appoint doesn't exists.");
            }

            if (!appointment.IsAvailable)
            {
                return BadRequest("Appoint is not available.");
            }

            var owner = await _context.Owners.FindAsync(model.OwnerId);
            if (owner == null)
            {
                return BadRequest("Owner doesn't exists.");
            }

            var pet = await _context.Pets.FindAsync(model.PetId);
            if (pet == null)
            {
                return BadRequest("Pet doesn't exists.");
            }

            appointment.IsAvailable = false;
            appointment.Owner = owner;
            appointment.Pet = pet;

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPost]
        [Route("UnScheduleAppointment")]
        public async Task<IActionResult> UnScheduleAppointment(AppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = await _context.Appointments
                .Include(a => a.Owner)
                .Include(a => a.Pet)
                .FirstOrDefaultAsync(a => a.Id == model.Id);
            if (appointment == null)
            {
                return BadRequest("Appoint doesn't exists.");
            }

            if (appointment.IsAvailable)
            {
                return BadRequest("Appoint is available.");
            }

            appointment.IsAvailable = true;
            appointment.AppointmentObs = null;
            appointment.Owner = null;
            appointment.Pet = null;

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return Ok(true);
        }

    }
}
