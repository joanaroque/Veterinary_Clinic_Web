using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vet_Clinic.Web.Data.Repositories;

namespace Vet_Clinic.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // para entrar na api é preciso ter um token deste tipo
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
