using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Common.Models;
using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Repositories;

namespace Vet_Clinic.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // para entrar na api é preciso ter um token deste tipo
    [ApiController]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly DataContext _context;


        public OwnersController(IOwnerRepository OwnerRepository,
            DataContext context)
        {
            _ownerRepository = OwnerRepository;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetOwner()
        {
            return Ok(_ownerRepository.GetAll());
        }

        [HttpPost]
        [Route("GetOwnerByEmail")]
        public async Task<IActionResult> GetOwner(OwnerResponse email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var owner = await _context.Owners
                .Include(o => o.User)
                .Include(o => o.Pets)
                .ThenInclude(p => p.Appointments)
                .Include(o => o.Pets)
                .ThenInclude(p => p.Histories)
                .ThenInclude(h => h.ServiceType)
                .FirstOrDefaultAsync(o => o.User.UserName.ToLower() == email.Email.ToLower());

            var response = new OwnerResponse
            {
                Id = owner.Id,
                Name = owner.User.FirstName,
                LastName = owner.User.LastName,
                Email = owner.User.Email,
                PhoneNumber = owner.User.PhoneNumber,
                Pets = owner.Pets.Select(p => new PetResponse
                {
                    DateOfBirth = p.DateOfBirth,
                    Id = p.Id,
                    ImageUrl = p.ImageFullPath,
                    Name = p.Name,
                    Breed = p.Breed,
                    Histories = p.Histories.Select(h => new HistoryResponse
                    {
                        Date = h.Date,
                        Description = h.Description,
                        Id = h.Id,
                        ServiceType = h.ServiceType.Name
                    }).ToList()
                }).ToList()
            };

            return Ok(response);
        }
    }
}
