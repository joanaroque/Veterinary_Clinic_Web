using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Helpers;

namespace Vet_Clinic.Web.Controllers
{
    [Authorize(Roles = "Admin, Agent")]
    public class SpeciesController : Controller
    {
        private readonly DataContext _context;
        private readonly ISpecieRepository _specie;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;

        public SpeciesController(DataContext context,
            IImageHelper imageHelper,
            ISpecieRepository specie,
                        IUserHelper userHelper,
                        IConverterHelper converterHelper)
        {
            _context = context;
            _specie = specie;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
        }

        // GET: Species
        public IActionResult Index()
        {
            var specie = _specie.GetAll().ToList();

            return View(specie);
        }

        // GET: Species/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("SpecieNotFound");
            }

            var specie = await _specie.GetByIdAsync(id.Value);

            if (specie == null)
            {
                return new NotFoundViewResult("SpecieNotFound");
            }

            return View(specie);
        }

        // GET: Species/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Species/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Specie specie)
        {
            if (ModelState.IsValid)
            {
                await _specie.CreateAsync(specie);

                return RedirectToAction(nameof(Index));
            }

            return View(specie);
        }

        // GET: Species/Edit/5
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("SpecieNotFound");
            }

            var specie = await _specie.GetByIdAsync(id.Value);
            if (specie == null)
            {
                return new NotFoundViewResult("SpecieNotFound");
            }
            return View(specie);
        }

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
                    await _specie.UpdateAsync(specie);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecieExists(specie.Id))
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


        // POST: Species/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("SpecieNotFound");
            }

            var species = await _context.Species
                .Include(pt => pt.Pets)
                .FirstOrDefaultAsync(pt => pt.Id == id);

            if (species == null)
            {
                return new NotFoundViewResult("SpecieNotFound");
            }

            if (species.Pets.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "This specie can't be removed.");
                return RedirectToAction(nameof(Index));
            }

            await _specie.DeleteAsync(species);

            return RedirectToAction(nameof(Index));
        }

        private bool SpecieExists(int id)
        {
            return _context.Species.Any(e => e.Id == id);
        }
    }
}
