using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPetRepository _PetRepository;
        private readonly IOwnerRepository _OwnerRepository;


        public AppointmentsController(IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IPetRepository PetRepository,
            IOwnerRepository OwnerRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _PetRepository = PetRepository;
            _OwnerRepository = OwnerRepository;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var model = await _appointmentRepository.GetAppointmentsAsync(User.Identity.Name);
            return View(model);
        }

        // GET: Appointments/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult AddDoctorPetOwner()
        {
            var model = new AddItemViewModel
            {
                Doctors = _doctorRepository.GetComboDoctors(),
                Pets = _PetRepository.GetComboPets(),
                Owners = _OwnerRepository.GetComboOwners()

            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddDoctorPetOwner(AddItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _appointmentRepository.AddItemToAppointmentAsync(model, User.Identity.Name);
                return RedirectToAction("Create");
            }
            return View(model);
        }

    }
}
