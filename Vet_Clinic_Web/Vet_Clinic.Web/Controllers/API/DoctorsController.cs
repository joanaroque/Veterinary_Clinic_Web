using Microsoft.AspNetCore.Mvc;
using Vet_Clinic.Web.Data;

namespace Vet_Clinic.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorsController(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        [HttpGet]
        public IActionResult GetDoctor()
        {
            return Ok(_doctorRepository.GetAll());
        }

    }
}
