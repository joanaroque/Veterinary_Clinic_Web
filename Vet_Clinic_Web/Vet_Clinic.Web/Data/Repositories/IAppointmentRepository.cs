using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Data.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IQueryable<Appointment>> GetAppointmentsAsync(string userName);


        Task AddItemToAppointmentAsync(AddItemViewModel model, string userName);


        Task ModifyAppointmentAsync(int id, double quantity);


        Task AddDaysAsync(int days);

    }
}
