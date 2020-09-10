﻿using System;

using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Data.Repositories;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly IServiceTypesRepository _serviceTypesRepository;
        private readonly ISpecieRepository _specieRepository;

        public ConverterHelper(
           DataContext dataContext,
           IServiceTypesRepository serviceTypesRepository,
           ISpecieRepository specieRepository)
        {
            _context = dataContext;
            _serviceTypesRepository = serviceTypesRepository;
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
                Histories = model.Histories,
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
                Histories = pet.Histories,
                OwnerId = pet.Owner.Id,
                SpecieId = pet.Specie.Id,
                Species = _specieRepository.GetComboSpecies(),
                ModifiedBy = pet.ModifiedBy,
                UpdateDate = DateTime.Now,

            };
        }

        public Owner ToOwner(OwnerViewModel model, string path, bool isNew)
        {
            return new Owner
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
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Pets = model.Pets,
                Appointments = model.Appointments
            };
        }

        public OwnerViewModel ToOwnerViewModel(Owner owner)
        {
            return new OwnerViewModel
            {
                Id = owner.Id,
                ImageUrl = owner.ImageUrl,
                LastName = owner.LastName,
                Name = owner.Name,
                TIN = owner.TIN,
                PhoneNumber = owner.PhoneNumber,
                Email = owner.Email,
                Address = owner.Address,
                DateOfBirth = owner.DateOfBirth,
                ModifiedBy = owner.ModifiedBy,
                UpdateDate = DateTime.Now,
                Pets = owner.Pets,
                Appointments = owner.Appointments
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

        public History ToHistory(HistoryViewModel model, bool isNew)
        {
            return new History
            {
                Description = model.Description,
                CreatedBy = model.CreatedBy,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Id = isNew ? 0 : model.Id,
                Pet = model.Pet,
                ServiceType = model.ServiceType
            };
        }

        public HistoryViewModel ToHistoryViewModel(History history)
        {
            return new HistoryViewModel
            {
                Description = history.Description,
                Id = history.Id,
                ModifiedBy = history.ModifiedBy,
                UpdateDate = DateTime.Now,
                PetId = history.Pet.Id,
                ServiceTypeId = history.ServiceType.Id,
                ServiceTypes = _serviceTypesRepository.GetComboServiceTypes()
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
                CreateDate = DateTime.Now,
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
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                AppointmentObs = model.AppointmentObs,
                CreatedBy = model.CreatedBy,
                Doctor = model.Doctor,
                Pet = model.Pet,
                Owner = model.Owner
            };
        }

        public Appointment ToAppointmentViewModel(Appointment appointment)
        {
            return new AppointmentViewModel
            {
                Id = appointment.Id,
                ModifiedBy = appointment.ModifiedBy,
                AppointmentObs = appointment.AppointmentObs,
                UpdateDate = DateTime.Now,
                Doctor = appointment.Doctor,
                Pet = appointment.Pet,
                Owner = appointment.Owner
            };
        }
    }
}


