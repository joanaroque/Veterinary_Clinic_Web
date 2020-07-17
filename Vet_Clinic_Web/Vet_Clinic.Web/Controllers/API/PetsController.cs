using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Vet_Clinic.Web.Data.Repositories;

namespace Vet_Clinic.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // para entrar na api é preciso ter um token deste tipo
    [ApiController]
    public class PetsController : Controller
    {
        private readonly IPetRepository _PetRepository;

        public PetsController(IPetRepository PetRepository)
        {
            _PetRepository = PetRepository;
        }

        [HttpGet]
        public IActionResult GetPet()
        {
            return Ok(_PetRepository.GetAll());

        }

    }
}
