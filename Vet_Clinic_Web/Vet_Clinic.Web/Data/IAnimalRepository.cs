using System.Linq;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data
{
   public interface IAnimalRepository : IGenericRepository<Animal>
    {
        IQueryable GetAllWithUsers();
    }
}
