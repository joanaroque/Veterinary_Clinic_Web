using System.Linq;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data
{
    public interface IAppointmentRepository : IGenericRepository<Appoitment>
    {
        IQueryable GetAllWithUsers();
    }
}
