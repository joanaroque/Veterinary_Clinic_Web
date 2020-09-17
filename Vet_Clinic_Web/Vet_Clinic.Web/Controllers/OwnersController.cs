using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Helpers;
using Vet_Clinic.Web.Models;
using Vet_Clinic.Web.Data;
using System.Collections.Generic;

namespace Vet_Clinic.Web.Controllers
{
    [Authorize(Roles = "Admin, Agent")]
    public class OwnersController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly DataContext _context;
        private readonly IServiceTypesRepository _serviceTypesRepository;
        private readonly ISpecieRepository _specieRepository;
        private readonly IMailHelper _mailHelper;


        public OwnersController(IOwnerRepository OwnerRepository,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper,
            DataContext context,
            IServiceTypesRepository serviceTypesRepository,
            ISpecieRepository specieRepository,
            IMailHelper mailHelper)
        {
            _ownerRepository = OwnerRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _context = context;
            _serviceTypesRepository = serviceTypesRepository;
            _specieRepository = specieRepository;
            _mailHelper = mailHelper;

        }

        // GET: Owners
        public IActionResult Index()
        {
            var owner = _context.Owners
                .Include(o => o.User)
                .Include(o => o.Pets);

            return View(owner);
        }

        // GET: Owners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            var owner = await _context.Owners
                .Include(o => o.User)
                .Include(o => o.Pets)
                .ThenInclude(p => p.Specie)
                .Include(o => o.Pets)
                .ThenInclude(p => p.Histories)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (owner == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            return View(owner);
        }

        // GET: Owners/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Owners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterNewViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Address = model.Address,
                    Email = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.UserName
                };

                var response = await _userHelper.AddUserAsync(user, model.Password);
                if (response.Succeeded)
                {
                    var userInDB = await _userHelper.GetUserByEmailAsync(model.UserName);
                    await _userHelper.AddUSerToRoleAsync(userInDB, "Customer");

                    var creator = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    var owner = new Owner
                    {
                        Appointments = new List<Appointment>(),
                        Pets = new List<Pet>(),
                        User = userInDB,
                        CreatedBy = creator,
                        ModifiedBy = creator,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now                   
                    };

                    await _ownerRepository.CreateAsync(owner);

                    try
                    {
                        var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(userInDB);
                        var tokenLink = Url.Action("ConfirmEmail", "Account", new
                        {
                            userid = userInDB.Id,
                            token = myToken
                        }, protocol: HttpContext.Request.Scheme);

                        _mailHelper.SendMail(model.UserName, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                            $"To allow the user, " +
                            $"please click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");

                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.ToString());
                        return View(model);
                    }
                }

                ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
            }

            return View(model);
        }

        // GET: Owners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            var owner = await _context.Owners
                    .Include(o => o.User)
                     .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (owner == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }
            var model = new EditUserViewModel
            {
                Address = owner.User.Address,
                FirstName = owner.User.FirstName,
                Id = owner.Id.ToString(),
                LastName = owner.User.LastName,
                PhoneNumber = owner.User.PhoneNumber
            };

            return View(model);
        }

        // POST: Owners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var owner = await _context.Owners
                       .Include(o => o.User)
                       .FirstOrDefaultAsync(o => o.Id.ToString() == model.Id);

                owner.User.FirstName = model.FirstName;
                owner.User.LastName = model.LastName;
                owner.User.Address = model.Address;
                owner.User.PhoneNumber = model.PhoneNumber;

                await _userHelper.UpdateUserAsync(owner.User);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // POST: Owners/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            var owner = await _context.Owners
                .Include(pt => pt.Pets)
               .FirstOrDefaultAsync(pt => pt.Id == id);

            if (owner == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            if (owner.Pets.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "This Owner can't be removed.");
                return RedirectToAction(nameof(Index));
            }

            await _ownerRepository.DeleteAsync(owner);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AddHistory(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            var pet = await _context.Pets.FindAsync(id.Value);
            if (pet == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            var model = new HistoryViewModel
            {
                CreateDate = DateTime.Now,
                PetId = pet.Id,
                ServiceTypes = _serviceTypesRepository.GetComboServiceTypes(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddHistory(HistoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var history = _converterHelper.ToHistory(model, true);
                _context.Histories.Add(history);
                await _context.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsPet)}/{model.PetId}");
            }

            model.ServiceTypes = _serviceTypesRepository.GetComboServiceTypes();
            return View(model);
        }

        public async Task<IActionResult> DetailsPet(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _context.Pets
                .Include(p => p.Owner)
                .ThenInclude(o => o.CreatedBy)
                .Include(p => p.Histories)
                .ThenInclude(h => h.ServiceType)
                .FirstOrDefaultAsync(o => o.Id == id.Value);

            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            return View(pet);
        }

        private bool OwnerExists(int id)
        {
            return _context.Owners.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AddPet(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var owner = await _ownerRepository.GetByIdAsync(id.Value);
            if (owner == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var model = new PetViewModel
            {
                DateOfBirth = DateTime.Today,
                OwnerId = owner.Id,
                Species = _specieRepository.GetComboSpecies()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPet(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, path);
                }

                var owner = await _context.Owners.FindAsync(model.OwnerId);

                model.Owner = owner;

                var specie = await _context.Species.FindAsync(model.SpecieId);

                model.Specie = specie;

                var pet = _converterHelper.ToPet(model, path, true);
                pet.CreatedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                await _ownerRepository.AddPetAsync(pet);

                return RedirectToAction($"Details/{model.OwnerId}");
            }

            model.Species = _specieRepository.GetComboSpecies();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPet(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _context.Pets
                .Include(p => p.Owner)
                .Include(p => p.Specie)
                .Include(p => p.Histories)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var view = _converterHelper.ToPetViewModel(pet);

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPet(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = model.ImageUrl;

                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Pets");
                }

                var pet = _converterHelper.ToPet(model, path, false);

                var ownerId = await _ownerRepository.UpdatePetAsync(pet);
                if (ownerId != 0)
                {
                    return RedirectToAction($"Details/{ownerId}");
                }
            }

            model.Species = _specieRepository.GetComboSpecies();

            return View(model);
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePet(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _ownerRepository.GetPetAsync(id.Value);

            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            if (pet.Histories.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "The pet can't be deleted because it has related records.");
                return RedirectToAction($"{nameof(Details)}/{pet.Owner.Id}");
            }

            var ownerId = await _ownerRepository.DeletePetAsync(pet);
            return RedirectToAction($"Details/{ownerId}");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHistory(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            var history = await _context.Histories
                .Include(h => h.Pet)
                .FirstOrDefaultAsync(h => h.Id == id.Value);

            if (history == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            _context.Histories.Remove(history);
            await _context.SaveChangesAsync();
            return RedirectToAction($"{nameof(DetailsPet)}/{history.Pet.Id}");
        }

    }
}
