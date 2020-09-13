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
    [Authorize(Roles = "Admin, Agent, Doctor")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;


        public AppointmentsController(IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IOwnerRepository ownerRepository,
            DataContext context,
            IConverterHelper converterHelper,
            IUserHelper userHelper)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _ownerRepository = ownerRepository;
            _context = context;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
        }

        // GET: Appointments
        public IActionResult Index()
        { 
            var appointment = _appointmentRepository.GetAll().ToList();

            return View(appointment);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AssistantNotFound");
            }

            var appointment = await _appointmentRepository.GetByIdAsync(id.Value);
            if (appointment == null)
            {
                return new NotFoundViewResult("AppointmentNotFound");
            }

            var view = _converterHelper.ToAppointmentViewModel(appointment);

            return View(view);
        }

        // POST: Assistant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointment = await _appointmentRepository.GetByIdAsync(model.Id);

                if (appointment == null)
                {
                    return new NotFoundViewResult("AppointmentNotFound");
                }

                appointment.Id = model.Id;

                await _appointmentRepository.UpdateAsync(appointment);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Appointment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AssistantNotFound");
            }

            var appointment = await _appointmentRepository.GetByIdAsync(id.Value);

            if (appointment == null)
            {
                return new NotFoundViewResult("AppointmentNotFound");
            }

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
                var appointment = _converterHelper.ToAppointment(model, true);

                appointment.CreatedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
               
                await _appointmentRepository.CreateAsync(appointment);

                return RedirectToAction(nameof(Index));
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
