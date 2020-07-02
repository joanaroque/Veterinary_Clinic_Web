using Microsoft.AspNetCore.Mvc;
using Vet_Clinic.Web.Data;

namespace Vet_Clinic.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : Controller
    {
        private readonly IAnimalRepository _animalRepository;

        public AnimalsController(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        [HttpGet]
        public IActionResult GetAnimal()
        {
            return Ok(_animalRepository.GetAll());

        }

    }
}
