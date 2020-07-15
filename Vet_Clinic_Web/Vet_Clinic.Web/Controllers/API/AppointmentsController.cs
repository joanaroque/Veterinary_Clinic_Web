using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vet_Clinic.Web.Data.Repositories;

namespace Vet_Clinic.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // para entrar na api é preciso ter um token deste tipo
    [ApiController]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentsController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        [HttpGet]
        public IActionResult GetAppointment()
        {
            return Ok(_appointmentRepository.GetAll());
        }


    }
}
