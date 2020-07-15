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

namespace Vet_Clinic.Web.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public AnimalsController(IAnimalRepository animalRepository,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _animalRepository = animalRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: Animals
        public IActionResult Index()
        {
            return View(_animalRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Animals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AnimalNotFound");
            }

            var animal = await _animalRepository.GetByIdAsync(id.Value);

            if (animal == null)
            {
                return new NotFoundViewResult("AnimalNotFound");
            }

            return View(animal);
        }

        // GET: Animals/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Animals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnimalViewModel animalViewModel)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (animalViewModel.ImageFile != null && animalViewModel.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(animalViewModel.ImageFile, "Animals");
                }

                var animal = _converterHelper.ToAnimal(animalViewModel, path, true);

                animal.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                await _animalRepository.CreateAsync(animal);

                return RedirectToAction(nameof(Index));
            }
            return View(animalViewModel);
        }

        // GET: Animals/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AnimalNotFound");
            }

            var animal = await _animalRepository.GetByIdAsync(id.Value);

            if (animal == null)
            {
                return new NotFoundViewResult("AnimalNotFound");
            }

            var view = _converterHelper.ToAnimalViewModel(animal);

            return View(view);
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AnimalViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        var doctor = _converterHelper.ToAnimal(model, path, false);
                    }

                    var product = _converterHelper.ToAnimal(model, path, false);

                    product.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    await _animalRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _animalRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("AnimalNotFound");
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

        // POST: Animals/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AnimalNotFound");
            }

            var animal = await _animalRepository.GetByIdAsync(id.Value);
            await _animalRepository.DeleteAsync(animal);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
