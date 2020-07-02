using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Helpers;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUserHelper _userHelper;


        public DoctorsController(IDoctorRepository doctorRepository, IUserHelper userHelper)
        {
            _doctorRepository = doctorRepository;
            _userHelper = userHelper;
        }

        // GET: Doctors
        public IActionResult Index()
        {
            return View(_doctorRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _doctorRepository.GetByIdAsync(id.Value);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorViewModel doctorViewModel)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (doctorViewModel.ImageFile != null && doctorViewModel.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Doctors",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await doctorViewModel.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Doctors/{file}";
                }

                var doctor = this.ToDoctor(doctorViewModel, path);

                //TODO: change to the logged user
                doctor.User = await _userHelper.GetUserByEmailAsync("joana.ramos.roque@formandos.cinel.pt");

                await _doctorRepository.CreateAsync(doctor);

                return RedirectToAction(nameof(Index));
            }
            return View(doctorViewModel);
        }

        private Doctor ToDoctor(DoctorViewModel view, string path)
        {
            return new Doctor
            {
                Id = view.Id,
                ImageUrl = path,
                LastName = view.LastName,
                Specialty = view.Specialty,
                MedicalLicense = view.MedicalLicense,
                Name = view.Name,
                TIN = view.TIN,
                PhoneNumber = view.PhoneNumber,
                Email = view.Email,
                Schedule = view.Schedule,
                ObsRoom = view.ObsRoom,
                Address = view.Address,
                DateOfBirth = view.DateOfBirth,
                User = view.User
            };
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _doctorRepository.GetByIdAsync(id.Value);
            if (doctor == null)
            {
                return NotFound();
            }

            var view = this.ToDoctorViewModel(doctor);

            return View(view);
        }

        private DoctorViewModel ToDoctorViewModel(Doctor doctor)
        {
            return new DoctorViewModel
            {
                Id = doctor.Id,
                ImageUrl = doctor.ImageUrl,
                LastName = doctor.LastName,
                Specialty = doctor.Specialty,
                MedicalLicense = doctor.MedicalLicense,
                Name = doctor.Name,
                TIN = doctor.TIN,
                PhoneNumber = doctor.PhoneNumber,
                Email = doctor.Email,
                Schedule = doctor.Schedule,
                ObsRoom = doctor.ObsRoom,
                Address = doctor.Address,
                DateOfBirth = doctor.DateOfBirth,
                User = doctor.User
            };
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\Doctors",
                            file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/Doctors/{file}";
                    }

                    var doctor = this.ToDoctor(model, path);

                    //TODO: Change to the logged user
                    doctor.User = await _userHelper.GetUserByEmailAsync("rafaasfs@gmail.com");
                    await _doctorRepository.UpdateAsync(doctor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _doctorRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _doctorRepository.GetByIdAsync(id.Value);
            await _doctorRepository.DeleteAsync(doctor);

            return RedirectToAction(nameof(Index));
        }
    }
}
