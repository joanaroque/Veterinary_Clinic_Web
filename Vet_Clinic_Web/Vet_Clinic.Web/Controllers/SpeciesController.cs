using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Helpers;

namespace Vet_Clinic.Web.Controllers
{
    [Authorize(Roles = "Admin, Agent")]
    public class SpeciesController : Controller
    {
        private readonly ISpecieRepository _specieRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IPetRepository _petRepository;
        private readonly IImageHelper _imageHelper;

        public SpeciesController(IImageHelper imageHelper,
            ISpecieRepository specieRepository,
           IUserHelper userHelper,
             IConverterHelper converterHelper,
             IPetRepository petRepository)
        {
            _specieRepository = specieRepository;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _petRepository = petRepository;
            _imageHelper = imageHelper;
        }

        // GET: Species
        public IActionResult Index()
        {
            var specie = _specieRepository.GetAll().ToList();

            return View(specie);
        }

        // GET: Species/Create
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// create a new specie
        /// </summary>
        /// <param name="specie">specie entity</param>
        /// <returns>new specie</returns>
        // POST: Species/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Specie specie)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _specieRepository.CreateAsync(specie);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a Specie with the same name.");
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

            return View(specie);
        }

        // GET: Species/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("SpecieNotFound");
            }

            var specie = await _specieRepository.GetByIdAsync(id.Value);

            if (specie == null)
            {
                return new NotFoundViewResult("SpecieNotFound");
            }
            return View(specie);
        }

        /// <summary>
        /// updates the specie
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="specie">specie entity</param>
        /// <returns>updated specie</returns>
        // POST: Species/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description")] Specie specie)
        {
            if (id != specie.Id)
            {
                return new NotFoundViewResult("SpecieNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _specieRepository.UpdateAsync(specie);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _specieRepository.ExistAsync(specie.Id))
                    {
                        return new NotFoundViewResult("SpecieNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(specie);
        }


        /// <summary>
        /// delete the specie
        /// </summary>
        /// <param name="id">specie id</param>
        /// <returns>index view</returns>
        // POST: Species/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("SpecieNotFound");
            }

            var specie = await _specieRepository.GetByIdAsync(id.Value);


            if (specie == null)
            {
                return new NotFoundViewResult("SpecieNotFound");
            }

            var pets = await _petRepository.GetPetBySpecieAsync(id.Value);


            if (pets.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "This specie can't be removed.");
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _specieRepository.DeleteAsync(specie);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
