using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Helpers
{
    public interface IConverterHelper
    {
        Doctor ToDoctor(DoctorViewModel model, string path, bool isNew);

        DoctorViewModel ToDoctorViewModel(Doctor doctor);

        Animal ToAnimal(AnimalViewModel model, string path, bool isNew);

        AnimalViewModel ToAnimalViewModel(Animal animal);

        Customer ToCustomer(CustomerViewModel model, string path, bool isNew);

        CustomerViewModel ToCustomerViewModel(Customer customer);

    }
}
