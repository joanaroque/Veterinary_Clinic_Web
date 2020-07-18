using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Data
{
    public class HomeController : Controller
    {

        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPetRepository _petRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly DataContext _context;


        public HomeController(IAppointmentRepository appointmentRepository,
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

        [Authorize(Roles = "Customer")]
        public IActionResult MyPets()
        {
            return View(_context.Pets
                .Include(p => p.Appointments)
                .Where(p => p.Owner.User.Email.ToLower().Equals(User.Identity.Name.ToLower())));
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyAppointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Owner)
                .ThenInclude(o => o.User)
                .Include(a => a.Pet)
                .Where(a => a.AppointmentSchedule >= DateTime.Today.ToUniversalTime()).ToListAsync();

            var list = new List<AppointmentViewModel>(appointments.Select(a => new AppointmentViewModel
            {
                AppointmentSchedule = a.AppointmentSchedule,
                Id = a.Id,
                IsAvailable = a.IsAvailable,
                Owner = a.Owner,
                Pet = a.Pet,
                AppointmentObs = a.AppointmentObs
            }).ToList());

            list.Where(a => a.Owner != null && a.Owner.User.UserName.ToLower().Equals(User.Identity.Name.ToLower()))
                .All(a => { a.IsMine = true; return true; });

            return View(list);
        }


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

            var model = new AppointmentViewModel
            {
                Id = agenda.Id,
                OwnerId = owner.Id,
                Pets = _petRepository.GetComboPets(owner.Id)
            };

            return View(model);
        }
    }
}
