using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Helpers
{
    public interface IConverterHelper
    {
        /// <summary>
        /// receives a model from a doctor and returns an entity
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="path">string</param>
        /// <param name="isNew">bool</param>
        /// <returns>doctor entity</returns>
        Doctor ToDoctor(DoctorViewModel model, string path, bool isNew);



        /// <summary>
        /// receives a doctor entity and returns a model
        /// </summary>
        /// <param name="doctor">entity doctor</param>
        /// <returns>doctor model</returns>
        DoctorViewModel ToDoctorViewModel(Doctor doctor);



        /// <summary>
        /// receives a model from a pet and returns an entity
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="path">string</param>
        /// <param name="isNew">bool</param>
        /// <returns>pet entity</returns>
        Pet ToPet(PetViewModel model, string path, bool isNew);


        /// <summary>
        /// receives a pet entity and returns a model
        /// </summary>
        /// <param name="pet">pet</param>
        /// <returns>pet model</returns>
        PetViewModel ToPetViewModel(Pet pet);


        /// <summary>
        /// receives a model from an assistant and returns an entity
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="path">string</param>
        /// <param name="isNew">bool</param>
        /// <returns>assistant entity</returns>
        Assistant ToAssistant(AssistantViewModel model, string path, bool isNew);


        /// <summary>
        /// receives a assistant entity and returns a model
        /// </summary>
        /// <param name="assistant">assistant</param>
        /// <returns>assistant model</returns>
        AssistantViewModel ToAssistantViewModel(Assistant assistant);



        /// <summary>
        /// receives a model from an Appointment and returns an entity
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="isNew">bool</param>
        /// <returns>Appointment entity</returns>
        Appointment ToAppointment(AppointmentViewModel model, bool isNew);



        /// <summary>
        /// receives a appointment entity and returns a model
        /// </summary>
        /// <param name="appointment">appointment</param>
        /// <returns>appointment model</returns>
        AppointmentViewModel ToAppointmentViewModel(Appointment appointment);


    }
}
