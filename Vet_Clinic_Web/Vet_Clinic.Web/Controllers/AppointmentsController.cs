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
    [Authorize(Roles = "Admin")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPetRepository _petRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly DataContext _context;


        public AppointmentsController(IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IPetRepository PetRepository,
            IOwnerRepository OwnerRepository,
            DataContext context)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _petRepository = PetRepository;
            _ownerRepository = OwnerRepository;
            _context = context;
        }

        // GET: Appointments
        public IActionResult Index()
        {
            return View(_context.Appointments
                 .Include(a => a.Owner)
                 .ThenInclude(o => o.User)
                 .Include(a => a.Pet)
                 .Where(a => a.AppointmentSchedule >= DateTime.Today.ToUniversalTime()));
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
                return NotFound();
            }

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(o => o.Id == id.Value);

            if (appointment == null)
            {
                return NotFound();
            }


            var model =  new AppointmentViewModel
            {
                Id = appointment.Id,
                Doctors = _doctorRepository.GetComboDoctors(),
                Pets = _petRepository.GetComboPets(0),
                Owners = _ownerRepository.GetComboOwners()

            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Schedule(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _appointmentRepository.AddAppointmentAsync(model, User.Identity.Name);
                return RedirectToAction("Index");
            }

            model.Owners = _ownerRepository.GetComboOwners();
            model.Pets = _petRepository.GetComboPets(model.OwnerId);

            return View(model);
        }

        public async Task<JsonResult> GetPetsAsync(int ownerId)
        {
            var pets = await _petRepository.GetByIdAsync(ownerId);
               
            return Json(pets);
        }

        public async Task<IActionResult> UnSchedule(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Owner)
                .Include(a => a.Pet)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.IsAvailable = true;
            appointment.Pet = null;
            appointment.Owner = null;
            appointment.AppointmentObs = null;

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

       
    }
}
