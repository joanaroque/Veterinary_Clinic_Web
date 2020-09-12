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
    public class ServiceTypesController : Controller
    {
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IServiceTypesRepository _serviceTypesRepository;
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;


        public ServiceTypesController(IImageHelper imageHelper,
            IServiceTypesRepository serviceTypesRepository,
                        IUserHelper userHelper,
                        IConverterHelper converterHelper,
                        DataContext context)
        {
            _serviceTypesRepository = serviceTypesRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _context = context;

        }

        // GET: ServiceTypes
        public IActionResult Index()
        {
            var service = _serviceTypesRepository.GetAll().ToList();

            return View(service);
        }

        // GET: ServiceTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ServiceTypeNotFound");
            }

            var serviceType = await _serviceTypesRepository.GetByIdAsync(id.Value);

            if (serviceType == null)
            {
                return new NotFoundViewResult("ServiceTypeNotFound");
            }

            return View(serviceType);
        }

        // GET: ServiceTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ServiceType serviceType)
        {
            if (ModelState.IsValid)
            {          
               await _serviceTypesRepository.CreateAsync(serviceType);

                return RedirectToAction(nameof(Index));
            }
            return View(serviceType);
        }

        // GET: ServiceTypes/Edit/5
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ServiceTypeNotFound");
            }

            var serviceType = await _context.ServiceTypes.FindAsync(id);
            if (serviceType == null)
            {
                return new NotFoundViewResult("ServiceTypeNotFound");
            }
            return View(serviceType);
        }

        // POST: ServiceTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ServiceType serviceType)
        {
            if (id != serviceType.Id)
            {
                return new NotFoundViewResult("ServiceTypeNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceTypeExists(serviceType.Id))
                    {
                        return new NotFoundViewResult("ServiceTypeNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(serviceType);
        }

        // POST: ServiceType/Delete/5
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ServiceTypeNotFound");
            }

            var serviceType = await _context.ServiceTypes
                 .Include(pt => pt.Histories)
                .FirstOrDefaultAsync(pt => pt.Id == id);

            if (serviceType == null)
            {
                return new NotFoundViewResult("ServiceTypeNotFound");

            }

            if (serviceType.Histories.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "This service Type can't be removed.");
                return RedirectToAction(nameof(Index));
            }

            await _serviceTypesRepository.DeleteAsync(serviceType);

            return RedirectToAction(nameof(Index));
        }

        private bool ServiceTypeExists(int id)
        {
            return _context.ServiceTypes.Any(e => e.Id == id);
        }
    }
}
