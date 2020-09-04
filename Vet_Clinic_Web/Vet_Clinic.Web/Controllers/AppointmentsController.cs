using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Helpers;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPetRepository _petRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly DataContext _context;


        public AppointmentsController(IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IPetRepository petRepository,
            IOwnerRepository ownerRepository,
            DataContext context)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _petRepository = petRepository;
            _ownerRepository = ownerRepository;
            _context = context;
        }

        // GET: Appointments
        public IActionResult Index()
        {
            var appointment = _context.Appointments
                  .Include(a => a.Owner)
                  .Include(a => a.Doctor)
                  .ThenInclude(o => o.User)
                  .Include(a => a.Pet)
                  .Where(a => a.Date >= DateTime.Today.ToUniversalTime());

            return View(appointment);
        }

        public async Task<IActionResult> AddDays()
        {
            await _appointmentRepository.AddDaysAsync(7);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Schedule(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AppointmentNotFound");
            }

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(o => o.Id == id.Value);

            if (appointment == null)
            {
                return new NotFoundViewResult("AppointmentNotFound");
            }


            var model = new AppointmentViewModel
            {
                Id = appointment.Id,
                Doctors = _doctorRepository.GetComboDoctors(),
                Owners = _ownerRepository.GetComboOwners(),
                Pets = _petRepository.GetComboPets(0),
                Date = appointment.Date
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Schedule(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                //por isto nas validaçoes: 
                if (model.Date > DateTime.Today)
                {
                    ModelState.AddModelError("AppointmentSchedule", "Invalid Appointment date");
                    return View(model);
                }


                //por isto nas validaçoes ^^^^^^^^^^

                var appointment = await _context.Appointments.FindAsync(model.Id);
                if (appointment != null)
                {
                    appointment.IsAvailable = false;
                    appointment.Doctor = await _context.Doctors.FindAsync(model.Id);
                    appointment.Owner = await _context.Owners.FindAsync(model.OwnerId);
                    appointment.Pet = await _context.Pets.FindAsync(model.PetId);
                    appointment.AppointmentObs = model.AppointmentObs;
                   

                    await _appointmentRepository.UpdateAsync(appointment);

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            model.Owners = _ownerRepository.GetComboOwners();
            model.Pets = _petRepository.GetComboPets(model.OwnerId);

            return View(model);
        }

        public async Task<JsonResult> GetPetsAsync(int ownerId)
        {
            var owner = await _ownerRepository.GetOwnersWithPetsAsync(ownerId);

            return Json(owner.Pets.OrderBy(p => p.Name));
        }

        public async Task<IActionResult> UnSchedule(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AppointmentNotFound");
            }

            var appointment = await _context.Appointments
                .Include(a => a.Owner)
                .Include(a => a.Pet)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(o => o.Id == id.Value);

            if (appointment == null)
            {
                return new NotFoundViewResult("AppointmentNotFound");
            }

            appointment.IsAvailable = true;
            appointment.Pet = null;
            appointment.Owner = null;
            appointment.AppointmentObs = null;
            appointment.Doctor = null;

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
