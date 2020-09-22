using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Helpers;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Controllers
{
    [Authorize(Roles = "Admin, Agent, Doctor")]
    public class PetsController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly IPetRepository _petRepository;
        private readonly IHistoryRepository _historyRepository;

        public PetsController(IOwnerRepository ownerRepository,
            IImageHelper imageHelper,
            IConverterHelper converterHelper,
            IUserHelper userHelper,
             IPetRepository petRepository,
             IHistoryRepository historyRepository)
        {
            _ownerRepository = ownerRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _petRepository = petRepository;
            _historyRepository = historyRepository;
        }

        // GET: Pets
        public IActionResult Index()
        {
            var pet = _petRepository.GetAllWithUsers();
    
            return View(pet);

        }

        // GET: Pets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _petRepository.GetDetailsPetAsync(id.Value);

            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            return View(pet);
        }


        // GET: Pets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _petRepository.GetDetailsPetAsync(id.Value);

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
        public async Task<IActionResult> Edit(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Pets");
                    }

                    var pet = _converterHelper.ToPet(model, path, false);

                    pet.ModifiedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                  
                    await _petRepository.UpdateAsync(pet);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _petRepository.ExistAsync(model.Id))
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _petRepository.GetByIdAsync(id.Value);

            await _petRepository.DeleteAsync(pet);

            return RedirectToAction(nameof(Index));
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHistory(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var history = await _historyRepository.GetByIdAsync(id.Value);

            if (history == null)//TODO ESTE DELETE FUNCIONA :O
            {
                return new NotFoundViewResult("PetNotFound");
            }

            await _historyRepository.DeleteAsync(history);

            return RedirectToAction($"{nameof(Details)}/{history.Pet.Id}");
        }


        public async Task<IActionResult> EditHistory(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var history = await _historyRepository.GetByIdAsync(id.Value);

            if (history == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var view = _converterHelper.ToHistoryViewModel(history);

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHistory(HistoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var history = _converterHelper.ToHistory(model, false);

                history.ModifiedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                await _historyRepository.UpdateAsync(history);

                return RedirectToAction($"{nameof(Details)}/{model.PetId}");
            }

           //todo: model.ServiceType = _serviceTypesRepository.GetComboServiceTypes();

            return View(model);
        }

        public async Task<IActionResult> AddHistory(int? id)//todo 
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _petRepository.GetByIdAsync(id.Value);

            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var view = new HistoryViewModel
            {
                CreateDate = DateTime.Now,
                PetId = pet.Id
            };

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHistory(HistoryViewModel view)
        {
            if (ModelState.IsValid)
            {
                var history = _converterHelper.ToHistory(view, true);

                history.CreatedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                await _historyRepository.CreateAsync(history);

                return RedirectToAction($"{nameof(Details)}/{view.PetId}");
            }

            return View(view);
        }
    }
}
