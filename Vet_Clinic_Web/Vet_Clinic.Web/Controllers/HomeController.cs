using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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


        public HomeController(IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IOwnerRepository OwnerRepository,
            IImageHelper imageHelper,
            DataContext context)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _ownerRepository = OwnerRepository;
            _context = context;
            _imageHelper = imageHelper;
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

        //[Authorize(Roles = "Customer")]
        public IActionResult MyPets()
        {
            return View(_context.Pets
                .Include(p => p.Appointments)
                .Where(p => p.Owner.User.Email.ToLower().Equals(User.Identity.Name.ToLower())));
        }

        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyAppointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Owner)
                .ThenInclude(o => o.User)
                .Include(a => a.Pet)
                .Where(a => a.Date >= DateTime.Today.ToUniversalTime()).ToListAsync();

            var list = new List<AppointmentViewModel>(appointments.Select(a => new AppointmentViewModel
            {
                Date = a.Date,
                Id = a.Id,
                Doctor = a.Doctor,
                Owner = a.Owner,
                Pet = a.Pet,
                AppointmentObs = a.AppointmentObs
            }).ToList());

            list.Where(a => a.Owner != null && a.Owner.User.UserName.ToLower().Equals(User.Identity.Name.ToLower()))
                .All(a => { a.IsMine = true; return true; });

            return View(list);
        }

         //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> Schedule(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await _context.Appointments
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (agenda == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners.FirstOrDefaultAsync(o => o.User.UserName.ToLower().Equals(User.Identity.Name.ToLower()));
            if (owner == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FirstOrDefaultAsync(o => o.User.UserName.ToLower().Equals(User.Identity.Name.ToLower()));
            if (doctor == null)
            {
                return NotFound();
            }

            var model = new AppointmentViewModel
            {
                Id = agenda.Id,
                OwnerId = owner.Id,
                DoctorId = doctor.Id,
                Pets = _ownerRepository.GetComboPets(owner.Id)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Schedule(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointment = await _context.Appointments.FindAsync(model.Id);
                if (appointment != null)
                {
                    appointment.Owner = await _context.Owners.FindAsync(model.OwnerId);
                    appointment.Pet = await _context.Pets.FindAsync(model.PetId);
                    appointment.Doctor = await _context.Doctors.FindAsync(model.DoctorId);

                    _context.Appointments.Update(appointment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MyAppointments));
                }
            }

            model.Pets = _ownerRepository.GetComboPets(model.Id);
            return View(model);
        }

        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> UnSchedule(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Owner)
                .Include(a => a.Pet)
                .FirstOrDefaultAsync(o => o.Id == id.Value);

            if (agenda == null)
            {
                return NotFound();
            }
            agenda.Pet = null;
            agenda.Owner = null;
            agenda.Doctor = null;

            _context.Appointments.Update(agenda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyAppointments));
        }

        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id.Value);

            if (pet == null)
            {
                return NotFound();
            }

            var model = new PetViewModel
            {
                DateOfBirth = pet.DateOfBirth,
                Id = pet.Id,
                ImageUrl = pet.ImageUrl,
                Name = pet.Name,
                OwnerId = pet.Owner.Id,
                Weight = pet.Weight,
                Breed = pet.Breed,
                Gender = pet.Gender
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Pets");

                }

                var pet = new Pet
                {
                    DateOfBirth = model.DateOfBirth,
                    Id = model.Id,
                    ImageUrl = path,
                    Name = model.Name,
                    Owner = await _context.Owners.FindAsync(model.OwnerId),
                    Breed = model.Breed,
                    Weight = model.Weight,
                    Gender = model.Gender
                };

                _context.Pets.Update(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyPets));
            }

            return View(model);
        }

        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Owner)
                .ThenInclude(o => o.User)
                .Include(p => p.Histories)
                .ThenInclude(h => h.ServiceType)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Histories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            if (pet.Histories.Count > 0)
            {
                return RedirectToAction(nameof(MyPets));
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyPets));
        }

       //[Authorize(Roles = "Customer")]
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
                OwnerId = owner.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Pets");

                }

                var pet = new Pet
                {
                    DateOfBirth = model.DateOfBirth,
                    ImageUrl = path,
                    Name = model.Name,
                    Owner = await _context.Owners.FindAsync(model.OwnerId),
                    Breed = model.Breed,
                    Weight = model.Weight,
                    Gender = model.Gender
                };

                _context.Pets.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction($"{nameof(MyPets)}");
            }
            return View(model);
        }
    }
}
