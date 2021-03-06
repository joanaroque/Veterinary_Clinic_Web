﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    [Authorize(Roles = "Admin, Agent")]
    public class OwnersController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly ISpecieRepository _specieRepository;
        private readonly IMailHelper _mailHelper;
        private readonly IPetRepository _petRepository;

        public OwnersController(IOwnerRepository OwnerRepository,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper,
            ISpecieRepository specieRepository,
            IMailHelper mailHelper,
             IPetRepository petRepository)
        {
            _ownerRepository = OwnerRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _specieRepository = specieRepository;
            _mailHelper = mailHelper;
            _petRepository = petRepository;
        }

        // GET: Owners
        public IActionResult Index()
        {
            var owner = _ownerRepository.GetAllWithUsers();

            return View(owner); 
        }

        /// <summary>
        /// show owner details
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>view owner details</returns>
        // GET: Owners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            var owner = await _ownerRepository.GetOwnerDetailsAsync(id.Value);

            if (owner == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            return View(owner);
        }

        // GET: Owners/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// creates owner and user associated
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>view model new user</returns>
        // POST: Owners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterNewViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Address = model.Address,
                    Email = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.UserName
                };

                var response = await _userHelper.AddUserAsync(user, model.Password);
                if (response.Succeeded)
                {
                    var userInDB = await _userHelper.GetUserByEmailAsync(model.UserName);
                    await _userHelper.AddUSerToRoleAsync(userInDB, "Customer");

                    var creator = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    var owner = new Owner
                    {
                        Appointments = new List<Appointment>(),
                        Pets = new List<Pet>(),
                        User = userInDB,
                        CreatedBy = creator,
                        ModifiedBy = creator,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };

                    await _ownerRepository.CreateAsync(owner);

                    try
                    {
                        var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(userInDB);
                        var tokenLink = Url.Action("ConfirmEmail", "Account", new
                        {
                            userid = userInDB.Id,
                            token = myToken
                        }, protocol: HttpContext.Request.Scheme);

                        _mailHelper.SendMail(model.UserName, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                            $"To allow the user, " +
                            $"please click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");

                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.ToString());
                        return View(model);
                    }
                }

                ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
            }

            return View(model);
        }

        // GET: Owners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            var owner = await _ownerRepository.GetOwnerWithUserByIdAsync(id.Value);

            if (owner == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }
            var model = new EditUserViewModel
            {
                Address = owner.User.Address,
                FirstName = owner.User.FirstName,
                Id = owner.Id.ToString(),
                LastName = owner.User.LastName,
                PhoneNumber = owner.User.PhoneNumber
            };

            return View(model);
        }

        /// <summary>
        /// updates the owner user
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>view model update user</returns>
        // POST: Owners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByIdAsync(model.Id);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;

                await _userHelper.UpdateUserAsync(user);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        /// <summary>
        /// delete the user owner
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>view index</returns>
        // POST: Owners/Delete/5 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            var owner = await _ownerRepository.GetByIdAsync(id.Value);

            if (owner == null)
            {
                return new NotFoundViewResult("OwnerNotFound");
            }

            if (owner.Pets.Count > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _ownerRepository.DeleteAsync(owner);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }


            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// shows the details of the pet of the respective owner
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>view with details</returns>
        public async Task<IActionResult> DetailsPet(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _petRepository.GetDetailsPetAsync(id.Value);

            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            return View(pet);
        }

        /// <summary>
        /// creates a pet from that owner
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>view create</returns>
        public async Task<IActionResult> AddPet(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var owner = await _ownerRepository.GetByIdAsync(id.Value);
            if (owner == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var model = new PetViewModel
            {
                DateOfBirth = DateTime.Today,
                OwnerId = owner.Id,
                Species = _specieRepository.GetComboSpecies()
            };

            return View(model);
        }

        /// <summary>
        /// creates a pet from that owner
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>view with new pet</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPet(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, path);
                }
                try
                {
                    model.Owner = await _ownerRepository.GetOwnerWithUserByIdAsync(model.OwnerId);

                    model.Specie = await _specieRepository.GetSpecieByIdAsync(model.SpecieId);

                    var pet = _converterHelper.ToPet(model, path, true);
                    pet.CreatedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                    await _petRepository.CreateAsync(pet);

                    return RedirectToAction($"Details/{model.OwnerId}");
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPet(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _petRepository.GetDetailsPetAsync(id.Value);


            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var view = _converterHelper.ToPetViewModel(pet);

            return View(view);
        }

        /// <summary>
        /// update the pet
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>updated pet</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPet(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = model.ImageUrl;

                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Pets");
                }

                try
                {
                    var pet = _converterHelper.ToPet(model, path, false);
                    pet.ModifiedBy = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                    await _petRepository.UpdateAsync(pet);

                    return RedirectToAction($"Details/{model.OwnerId}");

                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            model.Species = _specieRepository.GetComboSpecies();

            return View(model);
        }

        /// <summary>
        /// delete that owner's pet
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>owner details view</returns>
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePet(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            var pet = await _petRepository.GetByIdAsync(id.Value);

            if (pet == null)
            {
                return new NotFoundViewResult("PetNotFound");
            }

            if (pet.Appointments.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "The pet can't be deleted because it has related records.");
                return RedirectToAction($"{nameof(Details)}/{pet.Owner.Id}");
            }

            try
            {
                await _petRepository.DeleteAsync(pet);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return RedirectToAction($"Details/{pet.Owner.Id}");
        }
    }
}
