using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(DataContext context) : base(context)
        {
        }
    }
}
