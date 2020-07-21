using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Models;

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
            DataContext context,
            IServiceTypesRepository serviceTypesRepository)
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
        public async Task<IActionResult> GetOwner(Owner email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var owner = await _context.Owners
                .Include(o => o.User)
                .Include(a => a.Appointments)
                .Include(o => o.Pets)
                .ThenInclude(p => p.Specie)
                .Include(o => o.Pets)
                .ThenInclude(p => p.Histories)
                .ThenInclude(h => h.ServiceType)
                .FirstOrDefaultAsync(o => o.User.UserName.ToLower() == email.User.Email.ToLower());

            //var response = new OwnerViewModel
            //{
            //    Id = owner.Id,
            //    Name = owner.User.FirstName,
            //    LastName = owner.User.LastName,
            //    Email = owner.User.Email,
            //    PhoneNumber = owner.User.PhoneNumber,
            //    TIN = owner.TIN,
            //    ImageUrl = owner.ImageUrl,
            //    Address = owner.Address,
            //    DateOfBirth = owner.DateOfBirth,
            //    User = owner.User,
            //    Appointments = owner.Appointments,
            //    Pets = owner.Pets.Select(p => new PetViewModel
            //    {
            //        DateOfBirth = p.DateOfBirth,
            //        Id = p.Id,
            //        ImageUrl = p.ImageFullPath,
            //        Name = p.Name,
            //        Breed = p.Breed,
            //        Gender = p.Gender,
            //        Weight = p.Weight,
            //        Specie = p.Specie,
            //        Owner = p.Owner,
            //        Appointments = p.Appointments,
            //        Histories = p.Histories.Select(h => new HistoryViewModel
            //        {
            //            Date = h.Date,
            //            Description = h.Description,
            //            Id = h.Id,
            //            User = h.User,
            //            ServiceType = h.ServiceType.Id
            //        }).ToList()
            //    }).ToList()
            //};

            // return Ok(response);

            return Ok(owner);
        }
    }
}
