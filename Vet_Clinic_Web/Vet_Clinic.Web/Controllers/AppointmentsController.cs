using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly IPetRepository _petRepository;

        public AppointmentsController(IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IOwnerRepository ownerRepository,
            IConverterHelper converterHelper,
            IUserHelper userHelper,
            IPetRepository petRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _ownerRepository = ownerRepository;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _petRepository = petRepository;
        }

        // GET: Appointments
        public IActionResult Index()
        {
            var appointment = _appointmentRepository.GetAllByDate();

            return View(appointment);
        }

        public IActionResult AppointmentsHistory()
        {
            var appointment = _appointmentRepository.GetAllPastAppointments();

            return View(appointment);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AssistantNotFound");
            }

            var appointment = await _appointmentRepository.GetAllWithUsers(id.Value);

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
                try
                {
                    model.Doctor = await _doctorRepository.GetByIdAsync(model.DoctorId);
                    model.Owner = await _ownerRepository.GetByIdAsync(model.OwnerId);
                    model.Pet = await _petRepository.GetByIdAsync(model.PetId);

                    var appointment = _converterHelper.ToAppointment(model, false);

                    if (appointment == null)
                    {
                        return new NotFoundViewResult("AppointmentNotFound");
                    }

                    appointment.ModifiedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                    await _appointmentRepository.UpdateAsync(appointment);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

            }

            return View(model);
        }

        public IActionResult Schedule()
        {
            var model = new AppointmentViewModel
            {
                Doctors = _doctorRepository.GetComboDoctors(),
                Owners = _ownerRepository.GetComboOwners(),
                Pets = _petRepository.GetComboPets(0),

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Schedule(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Doctor = await _doctorRepository.GetByIdAsync(model.DoctorId);
                    model.Owner = await _ownerRepository.GetByIdAsync(model.OwnerId);
                    model.Pet = await _petRepository.GetByIdAsync(model.PetId);
                    var appointment = _converterHelper.ToAppointment(model, true);

                    appointment.CreatedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                    await _appointmentRepository.CreateAsync(appointment);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

            }

            model.Doctors = _doctorRepository.GetComboDoctors();
            model.Owners = _ownerRepository.GetComboOwners();
            model.Pets = _petRepository.GetComboPets(model.OwnerId);

            return View(model);
        }

        public async Task<JsonResult> GetDoctorsAsync(DateTime scheduledDate)
        {
            int appointmentHour = scheduledDate.Hour;

            var workingDoctors = await _appointmentRepository.GetWorkingDoctorsAsync(appointmentHour);

            var doctorsAlreadyScheduled = await _appointmentRepository.GetScheduledDoctorsAsync(scheduledDate);

            var doctorsNotScheduled = workingDoctors.Except(doctorsAlreadyScheduled);


            return Json(doctorsNotScheduled.OrderBy(d => d.Name));
        }

        public async Task<JsonResult> GetPetsAsync(int ownerId)
        {
            var ownerPets = await _petRepository.GetPetFromOwnerAsync(ownerId);

            return Json(ownerPets);
        }


        public async Task<IActionResult> UnSchedule(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AppointmentNotFound");
            }

            try
            {
                var appointment = await _appointmentRepository.GetByIdAsync(id.Value);
                await _appointmentRepository.DeleteAsync(appointment);


            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
