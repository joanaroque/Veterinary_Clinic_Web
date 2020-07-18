using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading.Tasks;
using Vet_Clinic.Common.Models;
using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Helpers;

namespace Vet_Clinic.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // para entrar na api é preciso ter um token deste tipo
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IPetRepository _petRepository;
        private readonly IConverterHelper _converterHelper;


        public PetsController(IPetRepository PetRepository,
            DataContext context,
            IConverterHelper converterHelper)
        {
            _petRepository = PetRepository;
            _context = context;
            _converterHelper = converterHelper;
        }

        [HttpGet]
        public IActionResult GetPet()
        {
            return Ok(_petRepository.GetAll());

        }
        [HttpPost]
        public async Task<IActionResult> PostPet([FromBody] PetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var owner = await _context.Owners.FindAsync(request.OwnerId);
            if (owner == null)
            {
                return BadRequest("Not valid owner.");
            }


            var pet = new Pet
            {
                DateOfBirth = request.DateOfBirth.ToUniversalTime(),
                Name = request.Name,
                Owner = owner,
                Breed = request.Breed,
            };

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();
            return Ok(_converterHelper.ToPetResponse(pet));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPet([FromRoute] int id, [FromBody] PetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Id)
            {
                return BadRequest();
            }

            var oldPet = await _context.Pets.FindAsync(request.Id);
            if (oldPet == null)
            {
                return BadRequest("Pet doesn't exists.");
            }

            oldPet.DateOfBirth = request.DateOfBirth.ToUniversalTime();
            oldPet.Name = request.Name;
            oldPet.Breed = request.Breed;

            _context.Pets.Update(oldPet);
            await _context.SaveChangesAsync();
            return Ok(_converterHelper.ToPetResponse(oldPet));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var pet = await _context.Pets
                .Include(p => p.Histories)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (pet == null)
            {
                return this.NotFound();
            }

            if (pet.Histories.Count > 0)
            {
                BadRequest("The pet can't be deleted because it has history.");
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return Ok("Pet deleted");
        }
    }
}

