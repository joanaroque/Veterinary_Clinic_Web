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
        private readonly IAnimalRepository _animalRepository;
        private readonly ICustomerRepository _customerRepository;


        public AppointmentsController(IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IAnimalRepository animalRepository,
            ICustomerRepository customerRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _animalRepository = animalRepository;
            _customerRepository = customerRepository;
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

        public IActionResult AddDoctorAnimalCustomer()
        {
            var model = new AddItemViewModel
            {
                Quantity = 1,
                Doctors = _doctorRepository.GetComboDoctors(),
                Animals = _animalRepository.GetComboAnimals(),
                Customers = _customerRepository.GetComboCustomers()

            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddDoctorAnimalCustomer(AddItemViewModel model)
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
