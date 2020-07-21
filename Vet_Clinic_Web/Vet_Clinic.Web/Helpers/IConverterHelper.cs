using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Helpers
{
    public interface IConverterHelper
    {
        Doctor ToDoctor(DoctorViewModel model, string path, bool isNew);

        DoctorViewModel ToDoctorViewModel(Doctor doctor);

        Task<Pet> ToPetAsync(PetViewModel model, string path, bool isNew);

        PetViewModel ToPetViewModel( Pet pet);

        Owner ToOwner(OwnerViewModel model, string path, bool isNew);

        OwnerViewModel ToOwnerViewModel(Owner Owner);

        Task<History> ToHistoryAsync(HistoryViewModel model, bool isNew);

        HistoryViewModel ToHistoryViewModel(History history);

        Assistant ToAssistant(AssistantViewModel model, string path, bool isNew);

        AssistantViewModel ToAssistantViewModel( Assistant assistant);

    }
}
