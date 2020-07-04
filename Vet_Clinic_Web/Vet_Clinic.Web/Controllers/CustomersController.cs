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
    public class CustomersController : Controller
    {
        private ICustomerRepository _customerRepository;

        private readonly IUserHelper _userHelper;

        public CustomersController(ICustomerRepository customerRepository, IUserHelper userHelper)
        {
            _customerRepository = customerRepository;
            _userHelper = userHelper;
        }

        // GET: Customers
        public IActionResult Index()
        {
            return View(_customerRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerRepository.GetByIdAsync(id.Value);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (customerViewModel.ImageFile != null && customerViewModel.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Customers",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await customerViewModel.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Customers/{file}";
                }

                var doctor = this.ToCustomer(customerViewModel, path);

                //TODO: change to the logged user
                doctor.User = await _userHelper.GetUserByEmailAsync("joana.ramos.roque@formandos.cinel.pt");

                await _customerRepository.CreateAsync(doctor);

                return RedirectToAction(nameof(Index));
            }
            return View(customerViewModel);
        }

        private Customer ToCustomer(CustomerViewModel view, string path)
        {
            return new Customer
            {
                Id = view.Id,
                ImageUrl = path,
                LastName = view.LastName,
                Name = view.Name,
                TIN = view.TIN,
                PhoneNumber = view.PhoneNumber,
                Email = view.Email,
                Address = view.Address,
                DateOfBirth = view.DateOfBirth,
                User = view.User
            };
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerRepository.GetByIdAsync(id.Value);
            if (customer == null)
            {
                return NotFound();
            }
            var view = this.ToCustomerViewModel(customer);

            return View(view);
        }

        private CustomerViewModel ToCustomerViewModel(Customer customer)
        {
            return new CustomerViewModel
            {
                Id = customer.Id,
                ImageUrl = customer.ImageUrl,
                LastName = customer.LastName,
                Name = customer.Name,
                TIN = customer.TIN,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Address = customer.Address,
                DateOfBirth = customer.DateOfBirth,
                User = customer.User
            };
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerViewModel model)
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
                            "wwwroot\\images\\Customers",
                            file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/Customers/{file}";
                    }

                    var doctor = this.ToCustomer(model, path);

                    //TODO: Change to the logged user
                    doctor.User = await _userHelper.GetUserByEmailAsync("rafaasfs@gmail.com");
                    await _customerRepository.UpdateAsync(doctor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _customerRepository.ExistAsync(model.Id))
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

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerRepository.GetByIdAsync(id.Value);
            await _customerRepository.DeleteAsync(customer);

            return RedirectToAction(nameof(Index));
        }


    }
}
