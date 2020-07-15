using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Helpers;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Data.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;


        public AppointmentRepository(DataContext context,
            IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;

        }

        public async Task AddItemToAppointmentAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            var doctor = await _context.Doctors.FindAsync(model.DoctorId);
            if (doctor == null)
            {
                return;
            }

            var animal = await _context.Animals.FindAsync(model.AnimalId);
            if (animal == null)
            {
                return;
            }

            var customer = await _context.Customers.FindAsync(model.CustomerId);
            if (customer == null)
            {
                return;
            }

            var appointmentDetailTemp = await _context.AppointmentDetailsTemp
              .Where(adt => adt.User == user && adt.Doctor == doctor && adt.Animal == animal && adt.Customer == customer)
              .FirstOrDefaultAsync();

            if (appointmentDetailTemp == null)
            {
                appointmentDetailTemp = new AppointmentDetailTemp
                {
                    Doctor = doctor,
                    Animal = animal,
                    Customer = customer,
                    Quantity = model.Quantity,
                    User = user,
                };

                _context.AppointmentDetailsTemp.Add(appointmentDetailTemp);
            }
            else
            {
                appointmentDetailTemp.Quantity += model.Quantity;
                _context.AppointmentDetailsTemp.Update(appointmentDetailTemp);
            }


            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<Appointment>> GetAppointmentsAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return _context.Appointments
                    .Include(o => o.Procedures)
                    .ThenInclude(d => d.Animal)
                    .OrderByDescending(o => o.AppointmentSchedule);
            }

            return _context.Appointments
                    .Include(o => o.Procedures)
                    .ThenInclude(d => d.Animal)
                    .Where(o => o.User == user)
                    .OrderByDescending(o => o.AppointmentSchedule);
        }

        public async Task<IQueryable<AppointmentDetailTemp>> GetDetailTempsAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }

            return _context.AppointmentDetailsTemp
                    .Include(a => a.Doctor)
                    .Include(a => a.Animal)
                    .Include(a => a.Customer)
                    .Where(a => a.User == user)
                    .OrderByDescending(o => o.Animal.Name);
        }

        public async Task ModifyAppointmentDetailTempQuantityAsync(int id, double quantity)
        {
            var appointmentDetailTemp = await _context.AppointmentDetailsTemp.FindAsync(id);

            if (appointmentDetailTemp == null)
            {
                return;
            }

            appointmentDetailTemp.Quantity += quantity;

            if (appointmentDetailTemp.Quantity > 0)
            {
                _context.AppointmentDetailsTemp.Update(appointmentDetailTemp);
                await _context.SaveChangesAsync();
            }
        }
    }
}
