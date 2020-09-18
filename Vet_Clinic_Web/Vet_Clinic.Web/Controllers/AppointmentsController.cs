﻿using Microsoft.AspNetCore.Authorization;
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
            var appointment = _context.Appointments
                .Include(a => a.CreatedBy)
                .Include(a => a.Owner)
                .ThenInclude(o => o.User)
                .Include(a => a.Pet)
                .Include(a => a.Doctor)
                .Where(a => a.CreateDate >= DateTime.Today.ToUniversalTime());

            return View(appointment);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AssistantNotFound");
            }

            var appointment = await _context.Appointments
                     .Include(p => p.Doctor)
                      .Include(p => p.Pet)
                       .Include(p => p.Owner)
                       .FirstOrDefaultAsync(p => p.Id == id.Value);

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
                var appointment = _converterHelper.ToAppointment(model, false);

                if (appointment == null)
                {
                    return new NotFoundViewResult("AppointmentNotFound");
                }

                appointment.ModifiedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                await _appointmentRepository.UpdateAsync(appointment);

                return RedirectToAction(nameof(Index));
            }

            model.Doctors = _doctorRepository.GetComboDoctors();
            model.Owners = _ownerRepository.GetComboOwners();
            model.Pets = _ownerRepository.GetComboPets(model.OwnerId);

            return View(model);
        }

        // GET: Appointment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AppointmentNotFound");
            }

            var appointment =  _context.Appointments
                .Include(a => a.CreatedBy)
                .Include(a => a.Owner)
                .ThenInclude(o => o.User)
                .Include(a => a.Pet)
                .Include(a => a.Doctor)
                 .FirstOrDefaultAsync(m => m.Id == id);

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

        public async Task<JsonResult> GetDoctorsAsync(DateTime createDate)
        {
            int appointmentHour = createDate.Hour;

            var workingDoctors = await _context.Doctors
                .Where(d => d.WorkStart < appointmentHour && d.WorkEnd > appointmentHour)
                .ToListAsync();

            var doctorsAlreadyScheduled = await _context.Appointments
                    .Where(a => a.CreateDate.Equals(createDate))
                    .Select(a => a.Doctor).ToListAsync();

            var doctorsNotScheduled = workingDoctors.Except(doctorsAlreadyScheduled);


            return Json(doctorsNotScheduled.OrderBy(d => d.Name));
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
