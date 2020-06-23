using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Helpers;

namespace Vet_Clinic.Web.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUserHelper _userHelper;


        public DoctorsController(IDoctorRepository doctorRepository, IUserHelper userHelper)
        {
            _doctorRepository = doctorRepository;
            _userHelper = userHelper;
        }

        // GET: Doctors
        public IActionResult Index()
        {
            return View(_doctorRepository.GetAll());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _doctorRepository.GetByIdAsync(id.Value);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorID,Name,LastName,Specialty,MedicalLicense,TIN,PhoneNumber,Email,Schedule,ObsRoom,Address,DateOfBirth")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                doctor.User = await _userHelper.GetUserByEmailAsync("joana.ramos.roque@formandos.cinel.pt");

                await _doctorRepository.CreateAsync(doctor);

                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _doctorRepository.GetByIdAsync(id.Value);
            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorID,Name,LastName,Specialty,MedicalLicense,TIN,PhoneNumber,Email,Schedule,ObsRoom,Address,DateOfBirth")] Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    doctor.User = await _userHelper.GetUserByEmailAsync("joana.ramos.roque@formandos.cinel.pt");

                    await _doctorRepository.UpdateAsync(doctor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _doctorRepository.ExistAsync(doctor.Id))
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
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _doctorRepository.GetByIdAsync(id.Value);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            await _doctorRepository.DeleteAsync(doctor);

            return RedirectToAction(nameof(Index));
        }
    }
}
