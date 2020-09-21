using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

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


        public ServiceTypesController(IImageHelper imageHelper,
            IServiceTypesRepository serviceTypesRepository,
                        IUserHelper userHelper,
                        IConverterHelper converterHelper)
        {
            _serviceTypesRepository = serviceTypesRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: ServiceTypes
        public IActionResult Index()
        {
            var service = _serviceTypesRepository.GetAll().ToList();

            return View(service);
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
        public async Task<IActionResult> Edit(int? id)
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

        // POST: ServiceTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description")]ServiceType serviceType)
        {
            if (id != serviceType.Id)
            {
                return new NotFoundViewResult("SpecieNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceTypesRepository.UpdateAsync(serviceType);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _serviceTypesRepository.ExistAsync(serviceType.Id))
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ServiceTypeNotFound");
            }

            var serviceType = await _serviceTypesRepository.GetServiceWithHistory(id.Value);

            if (serviceType == null)
            {
                return new NotFoundViewResult("ServiceTypeNotFound");

            }

            if (serviceType.Histories.Count > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            await _serviceTypesRepository.DeleteAsync(serviceType);

            return RedirectToAction(nameof(Index));
        }
    }
}
