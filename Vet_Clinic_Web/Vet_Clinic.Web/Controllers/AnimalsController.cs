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
    public class AnimalsController : Controller
    {
        private readonly IAnimalRepository _animalRepository;

        private readonly IUserHelper _userHelper;

        public AnimalsController(IAnimalRepository animalRepository, IUserHelper userHelper)
        {
            _animalRepository = animalRepository;
            _userHelper = userHelper;
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
                return NotFound();
            }

            var animal = await _animalRepository.GetByIdAsync(id.Value);

            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // GET: Animals/Create
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
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Animals",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await animalViewModel.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Animals/{file}";
                }

                var animal = this.ToAnimal(animalViewModel, path);

                animal.User = await _userHelper.GetUserByEmailAsync("joana.ramos.roque@formandos.cinel.pt");

                await _animalRepository.CreateAsync(animal);

                return RedirectToAction(nameof(Index));
            }
            return View(animalViewModel);
        }

        private Animal ToAnimal(AnimalViewModel view, string path)
        {
            return new Animal
            {
                Id = view.Id,
                Name = view.Name,
                Breed = view.Breed,
                Gender = view.Gender,
                Weight = view.Weight,
                ImageUrl = path,
                Sterilization = view.Sterilization,
                User = view.User
            };
        }

        // GET: Animals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _animalRepository.GetByIdAsync(id.Value);

            if (animal == null)
            {
                return NotFound();
            }

            var view = this.ToAnimalViewModel(animal);

            return View(view);
        }

        private object ToAnimalViewModel(Animal animal)
        {
            return new AnimalViewModel
            {
                Id = animal.Id,
                Name = animal.Name,
                Breed = animal.Breed,
                Gender = animal.Gender,
                Weight = animal.Weight,
                ImageUrl = animal.ImageUrl,
                Sterilization = animal.Sterilization,
                User = animal.User
            };
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
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\Animals",
                            file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/Animals/{file}";
                    }

                    var product = this.ToAnimal(model, path);

                    //TODO: Change to the logged user
                    product.User = await _userHelper.GetUserByEmailAsync("joana.ramos.roque@formandos.cinel.pt");
                    await _animalRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _animalRepository.ExistAsync(model.Id))
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

        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _animalRepository.GetByIdAsync(id.Value);

            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _animalRepository.GetByIdAsync(id);
            await _animalRepository.DeleteAsync(animal);

            return RedirectToAction(nameof(Index));
        }
    }
}
