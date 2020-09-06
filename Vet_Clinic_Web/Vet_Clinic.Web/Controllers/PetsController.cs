using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Helpers;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Controllers
{
    public class PetsController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly DataContext _context;
        private readonly IServiceTypesRepository _serviceTypesRepository;

        public PetsController(IOwnerRepository ownerRepository,
            IImageHelper imageHelper,
            IConverterHelper converterHelper,
            DataContext context,
            IServiceTypesRepository serviceTypesRepository)
        {
            _ownerRepository = ownerRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _context = context;
            _serviceTypesRepository = serviceTypesRepository;
        }

        // GET: Pets
        public IActionResult Index()
        {
            var pet = _ownerRepository.GetAll().OrderBy(p => p.Name).ToList();

            return View(pet);
        }

        // GET: Pets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _context.Pets
                .Include(p => p.Owner)
                .Include(p => p.Specie)
                .Include(p => p.Histories)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            return View(pet);
        }

       
        // GET: Pets/Edit/5
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _context.Pets
                 .Include(p => p.Owner)
                 .Include(p => p.Specie)
                 .FirstOrDefaultAsync(p => p.Id == id.Value);

            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var view = _converterHelper.ToPetViewModel(pet);

            return View(view);
        }

        // POST: Pets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> Edit(PetViewModel model)
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
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Pets");
                    }

                    var pet = _converterHelper.ToPet(model, path, false);

                    await _ownerRepository.UpdatePetAsync(pet);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _ownerRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("PetNotFound");
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

        // POST: Pets/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _ownerRepository.GetByIdAsync(id.Value);
            await _ownerRepository.DeleteAsync(pet);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult PetNotFound()
        {
            return View();
        }

        public async Task<IActionResult> DeleteHistory(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var history = await _context.Histories
                .Include(h => h.Pet)
                .FirstOrDefaultAsync(h => h.Id == id.Value);
            if (history == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            _context.Histories.Remove(history);
            await _context.SaveChangesAsync();
            return RedirectToAction($"{nameof(Details)}/{history.Pet.Id}");
        }

        public async Task<IActionResult> EditHistory(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var history = await _context.Histories
                .Include(h => h.Pet)
                .Include(h => h.ServiceType)
                .FirstOrDefaultAsync(p => p.Id == id.Value);
            if (history == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var view = new HistoryViewModel
            {
                Date = history.Date,
                Description = history.Description,
                Id = history.Id,
                PetId = history.Pet.Id,
                ServiceTypeId = history.ServiceType.Id,
                ServiceTypes = _serviceTypesRepository.GetComboServiceTypes()
            };

            return View(view);
        }

        [HttpPost]
        public async Task<IActionResult> EditHistory(HistoryViewModel view)
        {
            if (ModelState.IsValid)
            {
                if (view.Date > DateTime.Today)
                {
                    ModelState.AddModelError("DateOfBirth", "Invalid date of birth");
                    return View(view);
                }

                var history = new History
                {
                    Date = view.Date,
                    Description = view.Description,
                    Id = view.Id,
                    Pet = await _context.Pets.FindAsync(view.PetId),
                    ServiceType = await _context.ServiceTypes.FindAsync(view.ServiceTypeId)
                };

                _context.Histories.Update(history);
                await _context.SaveChangesAsync();
                return RedirectToAction($"{nameof(Details)}/{view.PetId}");
            }

            return View(view);
        }

        public async Task<IActionResult> AddHistory(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _context.Pets.FindAsync(id.Value);
            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var view = new HistoryViewModel
            {
                Date = DateTime.Now,
                PetId = pet.Id,
                ServiceTypes = _serviceTypesRepository.GetComboServiceTypes(),
            };

            return View(view);
        }

        [HttpPost]
        public async Task<IActionResult> AddHistory(HistoryViewModel view)
        {
            if (ModelState.IsValid)
            {
                var history = new History
                {
                    Date = view.Date,
                    Description = view.Description,
                    Pet = await _context.Pets.FindAsync(view.PetId),
                    ServiceType = await _context.ServiceTypes.FindAsync(view.ServiceTypeId)
                };

                _context.Histories.Add(history);
                await _context.SaveChangesAsync();
                return RedirectToAction($"{nameof(Details)}/{view.PetId}");
            }

            return View(view);
        }
    }
}
