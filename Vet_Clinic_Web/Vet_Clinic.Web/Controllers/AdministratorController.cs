using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IImageHelper _imageHelper;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public AdministratorController(DataContext context,
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IImageHelper imageHelper,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _context = context;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _imageHelper = imageHelper;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _userHelper.CheckRoleAsync(model.RoleName);

                //if (result.Succeeded)
                //{
                //    return RedirectToAction("ListRoles");
                //}

                //foreach (IdentityError error in result.Errors)
                //{
                //    ModelState.AddModelError("", error.Description);
                //}

            }
            return View(model);
        }


        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles.ToList();
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
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";

                return new NotFoundViewResult("UserNotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };


            foreach (var user in _userManager.Users)
            {

                if (await _userHelper.IsUserInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);

                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";

                    return new NotFoundViewResult("AdminNotFound");
                }

                role.Name = model.RoleName;

                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";

                return new NotFoundViewResult("UserNotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userHelper.IsUserInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _userHelper.GetUserByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";

                return new NotFoundViewResult("UserNotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userHelper.GetUserByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await _userHelper.IsUserInRoleAsync(user, role.FullName)))
                {
                    result = await _userHelper.AddUserAsync(user, role.FullName);
                }
                else if (!model[i].IsSelected && await _userHelper.IsUserInRoleAsync(user, role.FullName))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.FullName);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await _userHelper.GetUserByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";

                return new NotFoundViewResult("UserNotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in _roleManager.Roles.ToList())
            {
                var userRoles = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userHelper.IsUserInRoleAsync(user, role.Name))
                {
                    userRoles.IsSelected = true;
                }

                else
                {
                    userRoles.IsSelected = false;
                }

                model.Add(userRoles);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await _userHelper.GetUserByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";

                return new NotFoundViewResult("UserNotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected)
                      .Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("EditUser", new { Id = userId });
        }


        // GET: Administrator/Edit/5
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userHelper.GetUserByIdAsync(id);

            if (id == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";

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
                        Value = x.Name
                    })
            };

            return View(model);
        }

        // POST: Administrator/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(RegisterNewViewModel editUser)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByIdAsync(editUser.Id);

                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id = {editUser.Id} cannot be found";

                    return new NotFoundViewResult("UserNotFound");
                }

                user.FirstName = editUser.FirstName;
                user.LastName = editUser.LastName;
                user.Address = editUser.Address;
                user.PhoneNumber = editUser.PhoneNumber;
                user.Email = editUser.UserName;
                user.UserName = editUser.UserName;


                var result = await _userHelper.UpdateUserAsync(user);

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
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";

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
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";

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
