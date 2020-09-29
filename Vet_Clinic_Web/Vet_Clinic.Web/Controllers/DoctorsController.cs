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
    public class DoctorsController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public DoctorsController(IDoctorRepository doctorRepository,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _doctorRepository = doctorRepository;
            _imageHelper = imageHelper;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
        }

        // GET: Doctors
        [Authorize(Roles = "Admin, Agent, Customer, Doctor")]
        public IActionResult Index()
        {
            var doctor = _doctorRepository.GetAll().ToList();

            return View(doctor);
        }

        // GET: Doctors/Details/5
        [Authorize(Roles = "Admin, Agent, Customer, Doctor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("DoctorNotFound");
            }

            var doctor = await _doctorRepository.GetByIdAsync(id.Value);

            if (doctor == null)
            {
                return new NotFoundViewResult("DoctorNotFound");
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        [Authorize(Roles = "Admin, Agent")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> Create(DoctorViewModel doctorViewModel)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (doctorViewModel.ImageFile != null && doctorViewModel.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(doctorViewModel.ImageFile, "Doctors");
                }

                try
                {
                    var doctor = _converterHelper.ToDoctor(doctorViewModel, path, true);

                    doctor.CreatedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                    await _doctorRepository.CreateAsync(doctor);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

            }
            return View(doctorViewModel);
        }

        // GET: Doctors/Edit/5
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("DoctorNotFound");
            }

            var doctor = await _doctorRepository.GetByIdAsync(id.Value);

            if (doctor == null)
            {
                return new NotFoundViewResult("DoctorNotFound");
            }

            var view = _converterHelper.ToDoctorViewModel(doctor);

            return View(view);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> Edit(DoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Doctors");
                    }

                    var doctor = _converterHelper.ToDoctor(model, path, false);

                    doctor.ModifiedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    await _doctorRepository.UpdateAsync(doctor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _doctorRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("DoctorNotFound");
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

        // POST: Doctors/Delete/5
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("DoctorNotFound");
            }

            var doctor = await _doctorRepository.GetByIdAsync(id.Value);

            if (doctor == null)
            {
                return new NotFoundViewResult("DoctorNotFound");
            }

            try
            {
                await _doctorRepository.DeleteAsync(doctor);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
