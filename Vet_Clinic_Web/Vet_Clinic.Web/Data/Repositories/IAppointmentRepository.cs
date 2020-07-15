using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Data.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IQueryable<Appointment>> GetAppointmentsAsync(string userName);


        Task<IQueryable<AppointmentDetailTemp>> GetDetailTempsAsync(string userName);


        Task AddItemToAppointmentAsync(AddItemViewModel model, string userName);


        Task ModifyAppointmentDetailTempQuantityAsync(int id, double quantity);

    }
}
