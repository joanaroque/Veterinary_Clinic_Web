using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Controllers
{
    public class SpeciesController : Controller
    {
        private readonly DataContext _context;

        public SpeciesController(DataContext context)
        {
            _context = context;
        }

        // GET: Species
        public async Task<IActionResult> Index()
        {
            return View(await _context.Species.ToListAsync());
        }

        // GET: Species/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specie = await _context.Species
                .FirstOrDefaultAsync(m => m.Id == id);
            if (specie == null)
            {
                return NotFound();
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
        public async Task<IActionResult> Create([Bind("Id,Description")] Specie specie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(specie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specie);
        }

        // GET: Species/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specie = await _context.Species.FindAsync(id);
            if (specie == null)
            {
                return NotFound();
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
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecieExists(specie.Id))
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
            return View(specie);
        }


        // POST: Species/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var species = await _context.Species
                .Include(pt => pt.Pets)
                .FirstOrDefaultAsync(pt => pt.Id == id);
            if (species == null)
            {
                return NotFound();
            }

            if (species.Pets.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "The pet type can't be removed.");
                return RedirectToAction(nameof(Index));
            }

            _context.Species.Remove(species);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecieExists(int id)
        {
            return _context.Species.Any(e => e.Id == id);
        }
    }
}
