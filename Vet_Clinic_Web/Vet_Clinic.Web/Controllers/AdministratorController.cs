using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Helpers;
using Vet_Clinic.Web.Models;


namespace Vet_Clinic.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministratorController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IImageHelper _imageHelper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public AdministratorController(IUserHelper userHelper,
            IMailHelper mailHelper,
            IImageHelper imageHelper,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _imageHelper = imageHelper;
            _roleManager = roleManager;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult ListRoles() 
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        // GET: Administrator/Details/5
        public async Task<IActionResult> DetailsUser(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            var user = await _userHelper.GetUserByIdAsync(id);


            if (user == null)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            return View(user);
        }



        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users.ToList();

            return View(users);
        }


        // GET: Administrator/Edit/5
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userHelper.GetUserByIdAsync(id);

            if (id == null)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Roles = _roleManager.Roles.ToList().Select(
                    x => new SelectListItem()
                    {
                        Selected = userRoles.Contains(x.Name),
                        Text = x.Name,
                        Value = x.Id
                    })
            };

            return View(model);
        }

        // POST: Administrator/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel editUser)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByIdAsync(editUser.Id);

                if (user == null)
                {
                    return new NotFoundViewResult("UserNotFound");
                }

                user.FirstName = editUser.FirstName;
                user.LastName = editUser.LastName;
                user.Address = editUser.Address;
                user.PhoneNumber = editUser.PhoneNumber;

                var selectedRole = await _roleManager.FindByIdAsync(editUser.SelectedRole);

                // Remover roles já associados ao user, o que não foi o selecionado.
                foreach (var currentRole in _roleManager.Roles.ToList())
                {
                    var isSelectedRole = selectedRole.Name.Equals(currentRole.Name);
                    if (!isSelectedRole && await _userHelper.IsUserInRoleAsync(user, currentRole.Name))
                    {
                        // Remover do role
                       await _userManager.RemoveFromRoleAsync(user, currentRole.Name);
                    }
                }

                // Atribuir novo role
                await _userHelper.AddUSerToRoleAsync(user, selectedRole.Name);

                // Atualizar user após atribuir novo role.
                var result = await _userHelper.UpdateUserAsync(user);

                // Se a operação de atualização for com sucesso, enviar para a pagina de listagem de utilizadores.
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View(editUser);
        }

        // POST: Administrator/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userHelper.GetUserByIdAsync(id);

            if (user == null)
            {
                return new NotFoundViewResult("UserNotFound");
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return new NotFoundViewResult("UserNotFound");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("ListRoles");

        }

        public IActionResult UserNotFound()
        {
            return new NotFoundViewResult("UserNotFound");
        }
    }
}
