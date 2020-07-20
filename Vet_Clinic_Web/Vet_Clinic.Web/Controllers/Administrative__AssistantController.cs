using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Helpers;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Controllers
{
    public class Administrative__AssistantController : Controller
    {
        private readonly IAssistantRepository _assistantRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        public Administrative__AssistantController(IAssistantRepository assistantRepository,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _assistantRepository = assistantRepository;
            _imageHelper = imageHelper;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
        }

        // GET: Administrative__Assistant
        public IActionResult Index()
        {
            return View(_assistantRepository.GetAll().OrderBy(a => a.User.FirstName));
        }

        // GET: Administrative__Assistant/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AssistantNotFound");
            }

            var assistant = await _assistantRepository.GetByIdAsync(id.Value);

            if (assistant == null)
            {
                return new NotFoundViewResult("AssistantNotFound");
            }

            return View(assistant);
        }

        // GET: Administrative__Assistant/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administrative__Assistant/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AssistantViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Assistants");
                }

                var assistant = _converterHelper.ToAssistant(model, path, true);

                assistant.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                await _assistantRepository.CreateAsync(assistant);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Administrative__Assistant/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AssistantNotFound");
            }

            var assistant = await _assistantRepository.GetByIdAsync(id.Value);
            if (assistant == null)
            {
                return new NotFoundViewResult("AssistantNotFound");
            }

            var view = _converterHelper.ToAssistantViewModel(assistant);

            return View(view);
        }

        // POST: Administrative__Assistant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AssistantViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.User.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Assistants");
                    }

                    var assistant = _converterHelper.ToAssistant(model, path, false);

                    assistant.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    await _assistantRepository.UpdateAsync(assistant);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _assistantRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("AssistantNotFound");
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

        // POST: Administrative__Assistant/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AssistantNotFound");
            }

            var doctor = await _assistantRepository.GetByIdAsync(id.Value);
            await _assistantRepository.DeleteAsync(doctor);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AssistantNotFound()
        {
            return View();
        }
    }
}
