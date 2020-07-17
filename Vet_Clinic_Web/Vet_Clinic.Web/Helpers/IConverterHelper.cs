using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Helpers
{
    public interface IConverterHelper
    {
        Doctor ToDoctor(DoctorViewModel model, string path, bool isNew);

        DoctorViewModel ToDoctorViewModel(Doctor doctor);

        Pet ToPet(PetViewModel model, string path, bool isNew);

        PetViewModel ToPetViewModel(Pet Pet);

        Owner ToOwner(OwnerViewModel model, string path, bool isNew);

        OwnerViewModel ToOwnerViewModel(Owner Owner);

    }
}
