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
    public class PetsController : Controller
    {
        private readonly IPetRepository _PetRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public PetsController(IPetRepository PetRepository,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _PetRepository = PetRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: Pets
        public IActionResult Index()
        {
            return View(_PetRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Pets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var Pet = await _PetRepository.GetByIdAsync(id.Value);

            if (Pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            return View(Pet);
        }

        // GET: Pets/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PetViewModel PetViewModel)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (PetViewModel.ImageFile != null && PetViewModel.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(PetViewModel.ImageFile, "Pets");
                }

                var Pet = _converterHelper.ToPet(PetViewModel, path, true);

                await _PetRepository.CreateAsync(Pet);

                return RedirectToAction(nameof(Index));
            }
            return View(PetViewModel);
        }

        // GET: Pets/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var Pet = await _PetRepository.GetByIdAsync(id.Value);

            if (Pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var view = _converterHelper.ToPetViewModel(Pet);

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

                    await _PetRepository.UpdateAsync(pet);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _PetRepository.ExistAsync(model.Id))
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
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var Pet = await _PetRepository.GetByIdAsync(id.Value);
            await _PetRepository.DeleteAsync(Pet);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult PetNotFound()
        {
            return View();
        }
    }
}
