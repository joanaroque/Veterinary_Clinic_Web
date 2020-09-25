using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Helpers;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Controllers
{
    [Authorize(Roles = "Admin, Agent, Doctor")]
    public class AssistantController : Controller
    {
        private readonly IAssistantRepository _assistantRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public AssistantController(IAssistantRepository assistantRepository,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _assistantRepository = assistantRepository;
            _imageHelper = imageHelper;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
        }

        // GET: Assistant
        public IActionResult Index()
        {
            var assistant = _assistantRepository.GetAll().ToList();

            return View(assistant);
        }

        // GET: Assistant/Details/5
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

        // GET: Assistant/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Assistant/Create
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

                try
                {
                    var assistant = _converterHelper.ToAssistant(model, path, true);

                    assistant.CreatedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                    await _assistantRepository.CreateAsync(assistant);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are an Assistant with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

            }
            return View(model);
        }

        // GET: Assistant/Edit/5
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

        // POST: Assistant/Edit/5
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
                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Assistants");
                    }


                    var assistant = _converterHelper.ToAssistant(model, path, false);

                    assistant.ModifiedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

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

        // POST: Assistant/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            try
            {
                await _assistantRepository.DeleteAsync(assistant);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, "This Doctor already has scheduled appointments.\nFirst unschedule the appointment");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
           
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AssistantNotFound()
        {
            return View();
        }
    }
}
