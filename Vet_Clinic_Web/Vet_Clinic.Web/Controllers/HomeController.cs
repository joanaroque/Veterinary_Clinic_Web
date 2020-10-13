using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Helpers;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Data
{
    public class HomeController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IImageHelper _imageHelper;
        private readonly ISpecieRepository _specieRepository;
        private readonly IPetRepository _petRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;

        public HomeController(IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IOwnerRepository OwnerRepository,
             IPetRepository petRepository,
            IImageHelper imageHelper,
             IUserHelper userHelper,
              IConverterHelper converterHelper,
            ISpecieRepository specieRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _ownerRepository = OwnerRepository;
            _specieRepository = specieRepository;
            _petRepository = petRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }


        /// <summary>
        /// picks up doctors who work within the desired time 
        /// and doctors who already have an appointment for that date
        /// and removes the busy
        /// </summary>
        /// <param name="scheduledDate">intended date</param>
        /// <returns>available doctors</returns>
        public async Task<JsonResult> GetDoctorsAsync(DateTime scheduledDate)
        {
            int appointmentHour = scheduledDate.Hour;

            var workingDoctors = await _appointmentRepository.GetWorkingDoctorsAsync(appointmentHour);

            var doctorsAlreadyScheduled = await _appointmentRepository.GetScheduledDoctorsAsync(scheduledDate);

            var doctorsNotScheduled = workingDoctors.Except(doctorsAlreadyScheduled);

            return Json(doctorsNotScheduled.OrderBy(d => d.User.FullName));
        }

        /// <summary>
        /// gets the user and fetches the current user's (owner) pets
        /// </summary>
        /// <returns>the pet of current owner</returns>
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> MyPets()
        {
            var currentUser = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            var pet = await _petRepository.GetPetFromCurrentOwnerAsync(currentUser.Id);

            return View(pet);
        }

        /// <summary>
        /// get the user and get the list of appointments for the current user
        /// </summary>
        /// <returns>the current user appointments list</returns>
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> MyAppointments()
        {
            var currentUser = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            var appointments = await _appointmentRepository.GetAppointmentFromCurrentOwnerAsync(currentUser.Id);

            var list = new List<AppointmentViewModel>(appointments.
                Select(a => _converterHelper.ToAppointmentViewModel(a)).
                ToList());

            return View(list);
        }


        /// <summary>
        /// get the user and get the list of past appointments for the current user
        /// </summary>
        /// <returns>the current user past appointments list</returns>
        public async Task<IActionResult> OwnerAppointmentsHistory()
        {
            var currentUser = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            var appointments = await _appointmentRepository.GetPastAppointmentFromCurrentOwnerAsync(currentUser.Id);

            var list = new List<AppointmentViewModel>(appointments.
                Select(a => _converterHelper.ToAppointmentViewModel(a)).
                ToList());

            return View(list);
        }

        /// <summary>
        ///  gets the user, get information about the current user(owner), instantiate a new appointment model
        /// </summary>
        /// <returns>returns the new model</returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Schedule()
        {
            var currentUser = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            var owner = await _ownerRepository.GetCurrentUserOwner(currentUser.Id);

            if (owner == null)
            {
                return NotFound();
            }

            var model = new AppointmentViewModel
            {
                Doctors = _doctorRepository.GetComboDoctors(),
                OwnerId = owner.Id,
                Pets = _petRepository.GetComboPets(owner.Id)
            };

            model.Pets = _petRepository.GetComboPets(owner.Id);
            return View(model);
        }


        /// <summary>
        /// gets the doctor, the owner and the pet to create the appointment
        /// </summary>
        /// <param name="model">model appointment</param>
        /// <returns>model appointment</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Schedule(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Doctor = await _doctorRepository.GetDoctorByIdAsync(model.DoctorId);
                    model.Owner = await _ownerRepository.GetOwnerWithUserByIdAsync(model.OwnerId);
                    model.Pet = await _petRepository.GetByIdWithIncludesAsync(model.PetId);

                    var appointment = _converterHelper.ToAppointment(model, true);

                    appointment.CreatedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                    await _appointmentRepository.CreateAsync(appointment);

                    return RedirectToAction(nameof(MyAppointments));
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

            }

            model.Doctors = _doctorRepository.GetComboDoctors();
            model.Pets = _petRepository.GetComboPets(model.OwnerId);

            return View(model);
        }

        /// <summary>
        /// try to get the appointment by id and delete it,
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>view with appointments list</returns>
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> UnSchedule(int? id)
        {
            if (id == null)
            {
                return NotFound();
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
            return RedirectToAction(nameof(MyAppointments));
        }


        [HttpGet]
        public async Task<IActionResult> EditAppointment(int? id)
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

        /// <summary>
        ///  get the doctor, the owner and the pet, 
        ///  convert from model to entity,
        ///  get the user you edited and update
        /// </summary>
        /// <param name="model">model appointment</param>
        /// <returns>the updated model</returns>
        // POST: Assistant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAppointment(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Doctor = await _doctorRepository.GetDoctorByIdAsync(model.DoctorId);
                    model.Owner = await _ownerRepository.GetOwnerWithUserByIdAsync(model.OwnerId);
                    model.Pet = await _petRepository.GetByIdWithIncludesAsync(model.PetId);

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

        [HttpGet]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _petRepository.GetByIdWithIncludesAsync(id.Value);

            if (pet == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToPetViewModel(pet);

            return View(model);
        }

        /// <summary>
        /// updates the current owner's pet information
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>model updated</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Edit(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Pets");

                }

                try
                {

                    model.Owner = await _ownerRepository.GetOwnerWithUserByIdAsync(model.OwnerId);

                    model.Specie = await _specieRepository.GetSpecieByIdAsync(model.SpecieId);


                    var pet = _converterHelper.ToPet(model, path, false);
                    pet.ModifiedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                    await _petRepository.UpdateAsync(pet);

                    return RedirectToAction(nameof(MyPets));
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

            }
            model.Species = _specieRepository.GetComboSpecies();

            return View(model);
        }


        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _petRepository.GetByIdAsync(id.Value);

            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            return View(pet);
        }

        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _petRepository.GetByIdAsync(id.Value);

            if (pet == null)
            {
                return NotFound();
            }

            if (pet.Appointments.Count > 0)
            {
                return RedirectToAction(nameof(MyPets));
            }

            try
            {
                await _petRepository.DeleteAsync(pet);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }


            return RedirectToAction(nameof(MyPets));
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Create()
        {
            var owner = await _ownerRepository.GetFirstOwnerAsync(User.Identity.Name.ToLower());

            if (owner == null)
            {
                return NotFound();
            }

            var model = new PetViewModel
            {
                DateOfBirth = DateTime.Now,
                Species = _specieRepository.GetComboSpecies(),
                OwnerId = owner.Id
            };

            return View(model);
        }

        /// <summary>
        /// create a new pet model
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>new pet model</returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Create(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Pets");

                }
                try
                {
                    model.Owner = await _ownerRepository.GetOwnerWithUserByIdAsync(model.OwnerId);


                    model.Specie = await _specieRepository.GetSpecieByIdAsync(model.SpecieId);

                    var pet = _converterHelper.ToPet(model, path, true);
                    pet.CreatedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                    await _petRepository.CreateAsync(pet);

                    return RedirectToAction($"{nameof(MyPets)}");
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(model);
        }
    }
}
