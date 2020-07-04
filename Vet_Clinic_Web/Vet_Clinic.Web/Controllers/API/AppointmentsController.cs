using Microsoft.AspNetCore.Mvc;
using Vet_Clinic.Web.Data;

namespace Vet_Clinic.Web.Controllers.API
{
    [Route("api/[controller]")]
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
