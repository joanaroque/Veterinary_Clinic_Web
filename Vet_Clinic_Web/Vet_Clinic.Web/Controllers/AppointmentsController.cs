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
        private readonly IOwnerRepository _ownerRepository;
        private readonly DataContext _context;


        public AppointmentsController(IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IOwnerRepository ownerRepository,
            DataContext context)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _ownerRepository = ownerRepository;
            _context = context;
        }

        // GET: Appointments
        public IActionResult Index()
        {
            var appointment = _appointmentRepository.GetAll().OrderBy(a => a.User.FullName);

            return View(appointment);
        }

        public IActionResult Schedule()
        {
            var model = new AppointmentViewModel
            {
                Doctors = _doctorRepository.GetComboDoctors(),
                Owners = _ownerRepository.GetComboOwners(),
                Pets = _ownerRepository.GetComboPets(0),

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
                if (model.Date < DateTime.Today)
                {
                    ModelState.AddModelError("AppointmentSchedule", "Invalid Appointment date");
                    return View(model);
                }


                //por isto nas validaçoes ^^^^^^^^^^

                var appointment = await _appointmentRepository.GetByIdAsync(model.Id);

                if (appointment != null)
                {
                    appointment.Doctor = await _context.Doctors.FindAsync(model.Id);
                    appointment.Owner = await _context.Owners.FindAsync(model.OwnerId);
                    appointment.Pet = await _context.Pets.FindAsync(model.PetId);
                    appointment.AppointmentObs = model.AppointmentObs;


                    await _appointmentRepository.UpdateAsync(appointment);

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            model.Doctors = _doctorRepository.GetComboDoctors();
            model.Owners = _ownerRepository.GetComboOwners();
            model.Pets = _ownerRepository.GetComboPets(model.OwnerId);

            return View(model);
        }

        public async Task<JsonResult> GetPetsAsync(int ownerId)
        {
            var pets = await _ownerRepository.GetOwnersWithPetsAsync(ownerId);

            return Json(pets.Pets.OrderBy(p => p.Name));
        }


        public async Task<IActionResult> UnSchedule(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AppointmentNotFound");
            }

            var appointment = await _appointmentRepository.GetByIdAsync(id.Value);
            await _appointmentRepository.DeleteAsync(appointment);

            return RedirectToAction(nameof(Index));

        }

        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
