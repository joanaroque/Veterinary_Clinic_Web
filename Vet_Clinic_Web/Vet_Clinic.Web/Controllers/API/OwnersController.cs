using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vet_Clinic.Web.Data.Repositories;

namespace Vet_Clinic.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // para entrar na api é preciso ter um token deste tipo
    [ApiController]
    public class OwnersController : Controller
    {
        private readonly IOwnerRepository _OwnerRepository;

        public OwnersController(IOwnerRepository OwnerRepository)
        {
            _OwnerRepository = OwnerRepository;
        }

        [HttpGet]
        public IActionResult GetOwner()
        {
            return Ok(_OwnerRepository.GetAll());
        }
    }
}
