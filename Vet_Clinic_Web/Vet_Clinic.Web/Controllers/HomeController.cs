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
        private readonly DataContext _context;
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
            DataContext context,
            ISpecieRepository specieRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _ownerRepository = OwnerRepository;
            _context = context;
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

        public async Task<JsonResult> GetDoctorsAsync(DateTime scheduledDate)
        {
            var doctor = await _appointmentRepository.GetDoctorAsync(scheduledDate);

            return Json(doctor.Name); // todo dividir em 2 metodos no repositorio
        }

        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> MyPets()
        {
            var currentUser = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

          // todo repositorio var owner = await _ownerRepository.GetByIdAsync(int.Parse(currentUser.Id));

            var pet = _context.Pets
                .Include(p => p.Owner)
                .ThenInclude(p => p.User)
                .Where(p => p.Owner.User.Id == currentUser.Id);

            return View(pet);
        }

        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> MyAppointments()
        {
            var currentUser = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            var appointments = await _context.Appointments
                .Include(a => a.Owner)
                .ThenInclude(o => o.User)
                .Include(a => a.Pet)
                .Include(a => a.Doctor)
                .Where(a => a.ScheduledDate >= DateTime.Today.ToUniversalTime())
                .Where(a => a.Owner.User.Id == currentUser.Id).ToListAsync();

            var list = new List<AppointmentViewModel>(appointments.
                Select(a => _converterHelper.ToAppointmentViewModel(a)).
                ToList());

            return View(list);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Schedule()
        {
            var currentUser = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            var owner = _context.Owners
                         .Where(a => a.User.Id == currentUser.Id).FirstOrDefault();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Schedule(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointment = _converterHelper.ToAppointment(model, true);

                appointment.CreatedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                await _appointmentRepository.CreateAsync(appointment);

                return RedirectToAction(nameof(MyAppointments));
            }

            model.Doctors = _doctorRepository.GetComboDoctors();
            model.Pets = _petRepository.GetComboPets(model.OwnerId);

            return View(model);
        }

        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> UnSchedule(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await _appointmentRepository.GetByIdAsync(id.Value);

            if (agenda == null)
            {
                return NotFound();
            }
            agenda.Pet = null;
            agenda.Owner = null;
            agenda.Doctor = null;

            await _appointmentRepository.UpdateAsync(agenda);
            return RedirectToAction(nameof(MyAppointments));
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Edit(int? id)
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

            var model = _converterHelper.ToPetViewModel(pet);

            return View(model);
        }

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

                var pet = _converterHelper.ToPet(model, path, false);
                pet.ModifiedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                await _petRepository.UpdateAsync(pet);

                return RedirectToAction(nameof(MyPets));
            }

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

            if (pet.Histories.Count > 0)
            {
                return RedirectToAction(nameof(MyPets));
            }

            await _petRepository.DeleteAsync(pet);

            return RedirectToAction(nameof(MyPets));
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Create()
        {
            var owner = await _context.Owners
                .FirstOrDefaultAsync(o => o.User.Email.ToLower().Equals(User.Identity.Name.ToLower()));

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

                var owner = await _context.Owners.FindAsync(model.OwnerId);

                model.Owner = owner;

                var specie = await _context.Species.FindAsync(model.SpecieId);

                model.Specie = specie;

                var pet = _converterHelper.ToPet(model, path, true);
                pet.CreatedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                await _petRepository.CreateAsync(pet);

                return RedirectToAction($"{nameof(MyPets)}");
            }
            return View(model);
        }
    }
}
