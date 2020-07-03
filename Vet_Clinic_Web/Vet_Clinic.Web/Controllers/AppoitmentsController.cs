using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Helpers;

namespace Vet_Clinic.Web.Controllers
{
    public class AppoitmentsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;

        private readonly IUserHelper _userHelper;

        public AppoitmentsController(IAppointmentRepository appointmentRepository, IUserHelper userHelper)
        {
            _appointmentRepository = appointmentRepository;
        }

        // GET: Appoitments
        public IActionResult Index()
        {
            return View(_appointmentRepository.GetAll().OrderBy(p => p.Treatment));
        }

        // GET: Appoitments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appoitment = await _appointmentRepository.GetByIdAsync(id.Value);

            if (appoitment == null)
            {
                return NotFound();
            }

            return View(appoitment);
        }

        // GET: Appoitments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appoitments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appoitment appoitment)
        {
            if (ModelState.IsValid)
            {
                appoitment.User = await _userHelper.GetUserByEmailAsync("joana.ramos.roque@formandos.cinel.pt");

                await _appointmentRepository.CreateAsync(appoitment);
                return RedirectToAction(nameof(Index));
            }
            return View(appoitment);
        }

        // GET: Appoitments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appoitment = await _appointmentRepository.GetByIdAsync(id.Value);
            if (appoitment == null)
            {
                return NotFound();
            }
            return View(appoitment);
        }

        // POST: Appoitments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Appoitment appoitment)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    appoitment.User = await _userHelper.GetUserByEmailAsync("joana.ramos.roque@formandos.cinel.pt");

                    await _appointmentRepository.UpdateAsync(appoitment);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _appointmentRepository.ExistAsync(appoitment.Id))
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
            return View(appoitment);
        }

        // GET: Appoitments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentRepository.GetByIdAsync(id.Value);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appoitments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            await _appointmentRepository.DeleteAsync(appointment);

            return RedirectToAction(nameof(Index));
        }

    }
}
