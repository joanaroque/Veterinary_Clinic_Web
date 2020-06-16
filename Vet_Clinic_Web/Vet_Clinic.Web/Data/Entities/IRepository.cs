using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vet_Clinic.Web.Data.Entities
{
    public interface IRepository
    {
        void AddDoctor(Doctor doctor);

        Doctor GetDoctor(int id);

        IEnumerable<Doctor> GetDoctors();

        bool DoctorExists(int id);

        void RemoveDoctor(Doctor doctor);

        Task<bool> SaveAllAsync();

        void UpdateDoctor(Doctor doctor);
    }
}