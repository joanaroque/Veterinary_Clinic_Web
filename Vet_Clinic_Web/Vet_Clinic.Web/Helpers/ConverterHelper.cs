using System.Threading.Tasks;
using Vet_Clinic.Common.Models;
using Vet_Clinic.Web.Data;
using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Models;

namespace Vet_Clinic.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;

        public ConverterHelper(
           DataContext dataContext)
        {
            _context = dataContext;
        }


        public Pet ToPet(PetViewModel model, string path, bool isNew)
        {
            return new Pet
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Breed = model.Breed,
                Gender = model.Gender,
                Weight = model.Weight,
                ImageUrl = path,
                Sterilization = model.Sterilization,
            };
        }

        public PetViewModel ToPetViewModel(Pet Pet)
        {
            return new PetViewModel
            {
                Id = Pet.Id,
                Name = Pet.Name,
                Breed = Pet.Breed,
                Gender = Pet.Gender,
                Weight = Pet.Weight,
                ImageUrl = Pet.ImageUrl,
                Sterilization = Pet.Sterilization,
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
            };
        }

        public OwnerViewModel ToOwnerViewModel(Owner Owner)
        {
            return new OwnerViewModel
            {
                Id = Owner.Id,
                ImageUrl = Owner.ImageUrl,
                LastName = Owner.LastName,
                Name = Owner.Name,
                TIN = Owner.TIN,
                PhoneNumber = Owner.PhoneNumber,
                Email = Owner.Email,
                Address = Owner.Address,
                DateOfBirth = Owner.DateOfBirth,
                User = Owner.User
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
                PetId = history.Pet.Id,
                ServiceTypeId = history.ServiceType.Id,
              //TODO:  ServiceTypes = _combosHelper.GetComboServiceTypes()
            };
        }

        public PetResponse ToPetResponse(Pet pet)
        {
            if (pet == null)
            {
                return null;
            }

            return new PetResponse
            {
                DateOfBirth = pet.DateOfBirth,
                Id = pet.Id,
                ImageUrl = pet.ImageFullPath,
                Name = pet.Name,
                Breed = pet.Breed,
            };
        }

        public OwnerResponse ToOwnerResposne(Owner owner)
        {
            if (owner == null)
            {
                return null;
            }

            return new OwnerResponse
            {
                Address = owner.Address,
                Email = owner.User.Email,
                Name = owner.User.FirstName,
                LastName = owner.User.LastName,
                PhoneNumber = owner.User.PhoneNumber
            };
        }
    }
}
