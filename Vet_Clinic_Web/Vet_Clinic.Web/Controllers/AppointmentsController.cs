using Microsoft.AspNetCore.Authorization;
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
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;

        private readonly IUserHelper _userHelper;

        public AppointmentsController(IAppointmentRepository appointmentRepository, IUserHelper userHelper)
        {
            _appointmentRepository = appointmentRepository;
            _userHelper = userHelper;
        }

        // GET: Appointments
        public IActionResult Index()
        {
            return View(_appointmentRepository.GetAll().OrderBy(p => p.Treatment));
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Appointments/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                await _appointmentRepository.CreateAsync(appointment);
                return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    appointment.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                    await _appointmentRepository.UpdateAsync(appointment);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _appointmentRepository.ExistAsync(appointment.Id))
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
            return View(appointment);
        }

        // GET: Appoitnments/Delete/5
        [Authorize]
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

        // POST: Appointments/Delete/5
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
