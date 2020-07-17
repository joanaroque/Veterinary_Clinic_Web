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
    public class OwnersController : Controller
    {
        private IOwnerRepository _OwnerRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public OwnersController(IOwnerRepository OwnerRepository,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _OwnerRepository = OwnerRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: Owners
        public IActionResult Index()
        {
            return View(_OwnerRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Owners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            var Owner = await _OwnerRepository.GetByIdAsync(id.Value);

            if (Owner == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            return View(Owner);
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
        public async Task<IActionResult> Create(OwnerViewModel OwnerViewModel)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (OwnerViewModel.ImageFile != null && OwnerViewModel.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(OwnerViewModel.ImageFile, "Owners");

                }

                var Owner = _converterHelper.ToOwner(OwnerViewModel, path, true);

                Owner.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                await _OwnerRepository.CreateAsync(Owner);

                return RedirectToAction(nameof(Index));
            }
            return View(OwnerViewModel);
        }

        // GET: Owners/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            var Owner = await _OwnerRepository.GetByIdAsync(id.Value);
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
        public async Task<IActionResult> Edit(OwnerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Owners");
                    }

                    var Owner = _converterHelper.ToOwner(model, path, false);

                    Owner.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    await _OwnerRepository.UpdateAsync(Owner);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _OwnerRepository.ExistAsync(model.Id))
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
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            var Owner = await _OwnerRepository.GetByIdAsync(id.Value);
            await _OwnerRepository.DeleteAsync(Owner);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult OwnerNotFound()
        {
            return View();
        }
    }
}
