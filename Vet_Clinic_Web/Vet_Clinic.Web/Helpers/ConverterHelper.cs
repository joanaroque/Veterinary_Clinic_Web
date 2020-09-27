using System;

using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly ISpecieRepository _specieRepository;

        public ConverterHelper(
           ISpecieRepository specieRepository)
        {
            _specieRepository = specieRepository;
        }


        public Pet ToPet(PetViewModel model, string path, bool isNew)
        {
            var pet = new Pet
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Breed = model.Breed,
                Gender = model.Gender,
                Weight = model.Weight,
                ImageUrl = path,
                Sterilization = model.Sterilization,
                DateOfBirth = model.DateOfBirth,
                Appointments = model.Appointments,
                Specie = model.Specie,
                Owner = model.Owner,
                CreateDate = DateTime.Now,
                CreatedBy = model.CreatedBy,
                UpdateDate = DateTime.Now
            };

            return pet;
        }

        public PetViewModel ToPetViewModel(Pet pet)
        {
            return new PetViewModel
            {
                Id = pet.Id,
                Name = pet.Name,
                Breed = pet.Breed,
                Gender = pet.Gender,
                Weight = pet.Weight,
                ImageUrl = pet.ImageUrl,
                Sterilization = pet.Sterilization,
                DateOfBirth = pet.DateOfBirth,
                Appointments = pet.Appointments,
                OwnerId = pet.Owner.Id,
                SpecieId = pet.Specie.Id,
                Species = _specieRepository.GetComboSpecies(),
                ModifiedBy = pet.ModifiedBy,
                CreateDate = pet.CreateDate,
                UpdateDate = DateTime.Now,
                OwnerFullName = pet.Owner.User.FullName,

            };
        }

        public Doctor ToDoctor(DoctorViewModel model, string path, bool isNew)
        {
            return new Doctor
            {
                Id = isNew ? 0 : model.Id,
                ImageUrl = path,
                LastName = model.LastName,
                Specialty = model.Specialty,
                MedicalLicense = model.MedicalLicense,
                Name = model.Name,
                TIN = model.TIN,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                WorkStart = model.WorkStart,
                WorkEnd = model.WorkEnd,
                ObsRoom = model.ObsRoom,
                Address = model.Address,
                DateOfBirth = model.DateOfBirth,
                CreatedBy = model.CreatedBy,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };
        }

        public DoctorViewModel ToDoctorViewModel(Doctor doctor)
        {
            return new DoctorViewModel
            {
                Id = doctor.Id,
                ImageUrl = doctor.ImageUrl,
                LastName = doctor.LastName,
                Specialty = doctor.Specialty,
                MedicalLicense = doctor.MedicalLicense,
                Name = doctor.Name,
                TIN = doctor.TIN,
                PhoneNumber = doctor.PhoneNumber,
                Email = doctor.Email,
                WorkStart = doctor.WorkStart,
                WorkEnd = doctor.WorkEnd,
                ObsRoom = doctor.ObsRoom,
                Address = doctor.Address,
                DateOfBirth = doctor.DateOfBirth,
                ModifiedBy = doctor.ModifiedBy,
                UpdateDate = DateTime.Now
            };
        }

        public Assistant ToAssistant(AssistantViewModel model, string path, bool isNew)
        {
            return new Assistant
            {
                Id = isNew ? 0 : model.Id,
                ImageUrl = path,
                LastName = model.LastName,
                Name = model.Name,
                TIN = model.TIN,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Address = model.Address,
                DateOfBirth = model.DateOfBirth,
                CreatedBy = model.CreatedBy,
                CreateDate = isNew ? DateTime.Now : model.CreateDate,
                UpdateDate = DateTime.Now
            };
        }

        public AssistantViewModel ToAssistantViewModel(Assistant assistant)
        {
            return new AssistantViewModel
            {
                Id = assistant.Id,
                ImageUrl = assistant.ImageUrl,
                LastName = assistant.LastName,
                Name = assistant.Name,
                TIN = assistant.TIN,
                PhoneNumber = assistant.PhoneNumber,
                Email = assistant.Email,
                Address = assistant.Address,
                DateOfBirth = assistant.DateOfBirth,
                ModifiedBy = assistant.ModifiedBy,
                UpdateDate = DateTime.Now

            };
        }

        public Appointment ToAppointment(AppointmentViewModel model, bool isNew)
        {

            return new Appointment
            {
                Id = isNew ? 0 : model.Id,
                ScheduledDate = model.ScheduledDate,
                CreateDate = isNew ? DateTime.Now : model.CreateDate,
                UpdateDate = DateTime.Now,
                AppointmentObs = model.AppointmentObs,
                CreatedBy = model.CreatedBy,
                Doctor = model.Doctor,
                Pet = model.Pet,
                Owner = model.Owner
            };
        }

        public AppointmentViewModel ToAppointmentViewModel(Appointment appointment)
        {
            return new AppointmentViewModel
            {
                Id = appointment.Id,
                ScheduledDate = appointment.ScheduledDate,
                ModifiedBy = appointment.ModifiedBy,
                AppointmentObs = appointment.AppointmentObs,
                UpdateDate = appointment.UpdateDate,
                Doctor = appointment.Doctor,
                Pet = appointment.Pet,
                PetId = appointment.Pet.Id,
                Owner = appointment.Owner,
                OwnerId = appointment.Owner.Id
            };
        }
    }
}


