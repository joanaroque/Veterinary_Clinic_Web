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
    public class OwnersController : Controller
    {
        private readonly IPetRepository _petRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly DataContext _context;
        private readonly IServiceTypesRepository _serviceTypesRepository;
        private readonly ISpecieRepository _specieRepository;

        public OwnersController(IPetRepository petRepository,
            IOwnerRepository OwnerRepository,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper,
            DataContext context,
            IServiceTypesRepository serviceTypesRepository,
            ISpecieRepository specieRepository)
        {
            _petRepository = petRepository;
            _ownerRepository = OwnerRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _context = context;
            _serviceTypesRepository = serviceTypesRepository;
            _specieRepository = specieRepository;
        }

        // GET: Owners
        public IActionResult Index()
        {
            var owner = _ownerRepository.GetAll().OrderBy(p => p.User.FirstName).ToList();

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
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> Create(OwnerViewModel ownerViewModel)
        {
            if (ModelState.IsValid)
            {

                if (ownerViewModel.DateOfBirth > DateTime.Today)
                {
                    ModelState.AddModelError("DateOfBirth", "Invalid date of birth");
                    return View(ownerViewModel);
                }

                var path = string.Empty;

                if (ownerViewModel.ImageFile != null && ownerViewModel.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(ownerViewModel.ImageFile, "Owners");

                }

                var owner = _converterHelper.ToOwner(ownerViewModel, path, true);

                owner.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                var newOwner = new Owner
                {
                    Appointments = new List<Appointment>(),
                    Pets = new List<Pet>()
                };

                await _ownerRepository.CreateAsync(owner);

                return RedirectToAction(nameof(Index));
            }
            return View(ownerViewModel);
        }

        // GET: Owners/Edit/5
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            var Owner = await _ownerRepository.GetByIdAsync(id.Value);
            if (Owner == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }
            var view = _converterHelper.ToOwnerViewModel(Owner);

            return View(view);
        }

        // POST: Owners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> Edit(OwnerViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DateOfBirth > DateTime.Today)
                {
                    ModelState.AddModelError("DateOfBirth", "Invalid date of birth");
                    return View(model);
                }

                try
                {
                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Owners");
                    }

                    var owner = _converterHelper.ToOwner(model, path, false);

                    owner.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    await _ownerRepository.UpdateAsync(owner);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _ownerRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("OwnerNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: Owners/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Agent")]
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
                Date = DateTime.Now,
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

                if (model.Date > DateTime.Today)
                {
                    ModelState.AddModelError("DateOfBirth", "Invalid date of birth");
                    return View(model);
                }


                var history = await _converterHelper.ToHistoryAsync(model, true);
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
                .ThenInclude(o => o.User)
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

            var owner = await _context.Owners.FindAsync(id.Value);
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
        public async Task<IActionResult> AddPet(PetViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DateOfBirth > DateTime.Today)
                {
                    ModelState.AddModelError("DateOfBirth", "Invalid date of birth");
                    return View(model);
                }


                var path = string.Empty;

                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, path);
                }

                var pet =  _converterHelper.ToPetAsync(model, path, true);

                await _petRepository.CreateAsync(model);

                return RedirectToAction(nameof(Index));
            }
            model.Species = _specieRepository.GetComboSpecies();
            return View(model);
        }

        public async Task<IActionResult> EditPet(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _context.Pets
                .Include(p => p.Owner)
                .Include(p => p.Specie)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            return View(_converterHelper.ToPetViewModel(pet));
        }

        [HttpPost]
        public async Task<IActionResult> EditPet(PetViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DateOfBirth > DateTime.Today)
                {
                    ModelState.AddModelError("DateOfBirth", "Invalid date of birth");
                    return View(model);
                }


                var path = model.ImageUrl;

                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Pets");
                }

                var pet = await _converterHelper.ToPetAsync(model, path, false);
                _context.Pets.Update(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction($"Details/{model.OwnerId}");
            }

            model.Species = _specieRepository.GetComboSpecies();

            return View(model);
        }

        public async Task<IActionResult> DeletePet(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _context.Pets
                .Include(p => p.Owner)
                .Include(p => p.Histories)
                .FirstOrDefaultAsync(p => p.Id == id.Value);
            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            if (pet.Histories.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "The pet can't be deleted because it has related records.");
                return RedirectToAction($"{nameof(Details)}/{pet.Owner.Id}");
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return RedirectToAction($"{nameof(Details)}/{pet.Owner.Id}");
        }

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
