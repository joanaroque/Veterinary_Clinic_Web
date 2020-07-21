using System.Threading.Tasks;
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

        public ConverterHelper(
           DataContext dataContext,
           IServiceTypesRepository serviceTypesRepository)
        {
            _context = dataContext;
            _serviceTypesRepository = serviceTypesRepository;
        }


        public async Task<Pet> ToPetAsync(PetViewModel model, string path, bool isNew)
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
                Specie = await _context.Species.FindAsync(model.SpecieId),
                Owner = await _context.Owners.FindAsync(model.OwnerId)
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
                Owner = pet.Owner,
                Specie = pet.Specie,
                OwnerId = pet.Owner.Id,
                SpecieId = pet.Specie.Id,
                Species = _serviceTypesRepository.GetComboServiceTypes()

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
                User = model.User,
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
                User = owner.User,
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
                Schedule = model.Schedule,
                ObsRoom = model.ObsRoom,
                Address = model.Address,
                DateOfBirth = model.DateOfBirth,
                User = model.User
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
                Schedule = doctor.Schedule,
                ObsRoom = doctor.ObsRoom,
                Address = doctor.Address,
                DateOfBirth = doctor.DateOfBirth,
                User = doctor.User
            };
        }

        public async Task<History> ToHistoryAsync(HistoryViewModel model, bool isNew)
        {
            return new History
            {
                Date = model.Date.ToUniversalTime(),
                Description = model.Description,
                User = model.User,
                Id = isNew ? 0 : model.Id,
                Pet = await _context.Pets.FindAsync(model.PetId),
                ServiceType = await _context.ServiceTypes.FindAsync(model.ServiceTypeId)
            };
        }

        public HistoryViewModel ToHistoryViewModel(History history)
        {
            return new HistoryViewModel
            {
                Date = history.Date,
                Description = history.Description,
                Id = history.Id,
                User = history.User,
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
                User = model.User
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
                User = assistant.User
            };
        }
    }
}


