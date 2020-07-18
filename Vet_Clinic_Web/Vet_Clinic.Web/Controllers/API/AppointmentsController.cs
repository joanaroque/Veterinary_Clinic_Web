using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Common.Models;
using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Helpers;

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
        public async Task<IActionResult> GetAppointmentForOwner(OwnerResponse email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointments = await _context.Appointments
                .Include(a => a.Owner)
                .ThenInclude(o => o.User)
                .Include(a => a.Pet)
                .Where(a => a.AppointmentSchedule >= DateTime.Today.ToUniversalTime())
                .OrderBy(a => a.AppointmentSchedule)
                .ToListAsync();

            var response = new List<AppointmentResponse>();
            foreach (var appointment in appointments)
            {
                var appointmentResponse = new AppointmentResponse
                {
                    Date = appointment.AppointmentSchedule,
                    Id = appointment.Id,
                    IsAvailable = appointment.IsAvailable
                };

                if (appointment.Owner != null)
                {
                    if (appointment.Owner.User.Email.ToLower().Equals(email.Email.ToLower()))
                    {
                        appointmentResponse.Owner = _converterHelper.ToOwnerResposne(appointment.Owner);
                        appointmentResponse.Pet = _converterHelper.ToPetResponse(appointment.Pet);
                    }
                    else
                    {
                        appointmentResponse.Owner = new OwnerResponse {Name = "Reserved" };
                    }
                }

                response.Add(appointmentResponse);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("ScheduleAppointment")]
        public async Task<IActionResult> ScheduleAppointment(ScheduleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = await _context.Appointments.FindAsync(request.AppointmentId);
            if (appointment == null)
            {
                return BadRequest("Agenda doesn't exists.");
            }

            if (!appointment.IsAvailable)
            {
                return BadRequest("Agenda is not available.");
            }

            var owner = await _context.Owners.FindAsync(request.OwnerId);
            if (owner == null)
            {
                return BadRequest("Owner doesn't exists.");
            }

            var pet = await _context.Pets.FindAsync(request.PetId);
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
        public async Task<IActionResult> UnScheduleAppointment(ScheduleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = await _context.Appointments
                .Include(a => a.Owner)
                .Include(a => a.Pet)
                .FirstOrDefaultAsync(a => a.Id == request.AppointmentId);
            if (appointment == null)
            {
                return BadRequest("Agenda doesn't exists.");
            }

            if (appointment.IsAvailable)
            {
                return BadRequest("Agenda is available.");
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
