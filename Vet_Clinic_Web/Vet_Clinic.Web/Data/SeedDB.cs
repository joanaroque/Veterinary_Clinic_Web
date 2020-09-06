﻿using Microsoft.AspNetCore.Identity;

using System;
using System.Linq;
using System.Threading.Tasks;

using Vet_Clinic.Web.Data.Entities;
using Vet_Clinic.Web.Helpers;

namespace Vet_Clinic.Web.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;

        public SeedDB(DataContext context,
            IUserHelper userHelper,
            IImageHelper imageHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
        }

        public UserManager<User> UserManager { get; }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await CheckOrCreateRoles();

            await FillUserAsync();
            await FillSpeciesAsync();
            await FillServiceTypesAsync();
            await FillOwnerAsync();
            await FillDoctorAsync();
            await FillAssistantAsync();
            await FillAppointmentAsync();
            await FillPetsAsync();
            await FillHistoriesAsync();

        }

        private async Task CheckOrCreateRoles()
        {
            await _userHelper.CheckRoleAsync("Admin"); // see and do all
            await _userHelper.CheckRoleAsync("Agent"); // crud agents, appointments, owners and Pets + register other agents and Doctor
            await _userHelper.CheckRoleAsync("Doctor"); // crud Doctor and appointments 
            await _userHelper.CheckRoleAsync("Customer"); // crud Pets and crud appointments (and await confirmation)
        }

        private async Task FillHistoriesAsync()
        {
            if (!_context.Histories.Any())
            {
                var pet = _context.Pets.FirstOrDefault();
                var type = _context.ServiceTypes.FirstOrDefault();

                _context.Histories.Add(new History
                {
                    Pet = pet,
                    ServiceType = type,
                    Description = "Otite e orelhas sujas",
                    Date = DateTime.Now.AddDays(-1)
                });
                await _context.SaveChangesAsync();
            }
        }

        private async Task FillPetsAsync()
        {
            if (!_context.Pets.Any())
            {
                var owner = _context.Owners.FirstOrDefault();
                var specie = _context.Species.FirstOrDefault();

                _context.Pets.Add(new Pet
                {
                    Name = "If",
                    Owner = owner,
                    Specie = specie,
                    Breed = "Mongrel",
                    //  ImageUrl = await _imageHelper.UploadImageAsync(_context.Pets .ImageFile, "Pets"),
                    Gender = "Male",
                    Weight = 8,
                    Sterilization = true,
                    DateOfBirth = DateTime.Now.AddYears(-1)
                });

                _context.Pets.Add(new Pet
                {
                    Name = "Else",
                    Owner = owner,
                    Specie = specie,
                    Breed = "Berner Sennenhund",
                    ImageUrl = "w*****",
                    Gender = "Male",
                    Weight = 8,
                    Sterilization = true,
                    DateOfBirth = DateTime.Now.AddYears(-7)
                });


                await _context.SaveChangesAsync();
            }
        }


        private async Task FillSpeciesAsync()
        {
            if (!_context.Species.Any())
            {
                _context.Species.Add(new Specie { Description = "Dog" });
                _context.Species.Add(new Specie { Description = "Cat" });
                _context.Species.Add(new Specie { Description = "Ferret" });
                _context.Species.Add(new Specie { Description = "Dragon" });
                _context.Species.Add(new Specie { Description = "Mermaid" });
                _context.Species.Add(new Specie { Description = "Rhinopithecus" });
                _context.Species.Add(new Specie { Description = "CGrimpoteuthis" });

                await _context.SaveChangesAsync();
            }
        }

        private async Task FillAssistantAsync()
        {
            if (!_context.Assistants.Any())
            {
                _context.Assistants.Add(new Assistant
                {
                    Name = "Alexandra",
                    LastName = "Ramos",
                    PhoneNumber = "874514747",
                    Email = "alex@gmail.pt",
                    TIN = "87545454",
                    ImageUrl = "POR IMAGEM ************************************************",
                    Address = "Rua Zeca Afonso",
                    DateOfBirth = DateTime.Now.AddYears(-35)
                });
                await _context.SaveChangesAsync();
            }
        }

        private async Task FillUserAsync()
        {
            var user = await _userHelper.GetUserByEmailAsync("joanatpsi@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Joana",
                    LastName = "Roque",
                    Email = "joanatpsi@gmail.com",
                    UserName = "joanatpsi@gmail.com",
                    PhoneNumber = "965214744",
                    Address = "Rua da Programação",
                    EmailConfirmed = true
                };

                await _userHelper.AddUserAsync(user, "123456");

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

            if (!isInRole)
            {
                await _userHelper.AddUSerToRoleAsync(user, "Admin");
            }

            await _context.SaveChangesAsync();
        }

        private async Task FillServiceTypesAsync()
        {
            if (!_context.ServiceTypes.Any())
            {
                _context.ServiceTypes.Add(new ServiceType { Name = "Appointment" });
                _context.ServiceTypes.Add(new ServiceType { Name = "Emergency" });
                _context.ServiceTypes.Add(new ServiceType { Name = "Vaccination" });

                await _context.SaveChangesAsync();
            }
        }

        private async Task FillOwnerAsync()
        {
            if (!_context.Owners.Any())
            {
                var pet = _context.Pets.FirstOrDefault();

                _context.Owners.Add(new Owner
                {
                    Name = "Joana",
                    LastName = "Ramos",
                    Email = "lala@joana.pt",
                    PhoneNumber = "123456789",
                    Address = "Rua Adeus",
                    TIN = "44541",
                    ImageUrl = "imagemmmmmmmmmmmmmmmmm",
                    DateOfBirth = DateTime.Now.AddYears(-31).AddMonths(11).AddDays(27)
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task FillDoctorAsync()
        {
            if (!_context.Doctors.Any())
            {
                _context.Doctors.Add(new Doctor
                {

                    Name = "Antonio",
                    LastName = "Henrriques",
                    PhoneNumber = "9856488",
                    Email = "antoni@gmail.com",
                    Specialty = "Ortopedia",
                    MedicalLicense = "45345",
                    WorkStart = "9 h",
                    WorkEnd = "13 h",
                    ObsRoom = "8",
                    TIN = "4342",
                    ImageUrl = "imagemmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
                    Address = "Rua do médico",
                    DateOfBirth = DateTime.Now.AddYears(-58)

                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task FillAppointmentAsync()
        {
            if (!_context.Appointments.Any())
            {
                var owner = _context.Owners.FirstOrDefault();
                var doctor = _context.Doctors.FirstOrDefault();
                var pet = _context.Pets.FirstOrDefault();
                var user = _context.Users.FirstOrDefault();

                _context.Appointments.Add(new Appointment
                {
                    Date = DateTime.Now.AddDays(2),
                    AppointmentObs = "vacinas vacinas vacinas",
                    Owner = owner,
                    Doctor = doctor,
                    Pet = pet,
                    User = user
                });

                _context.Appointments.Add(new Appointment
                {
                    Date = DateTime.Now.AddDays(4),
                    AppointmentObs = "Otites",
                    Owner = owner,
                    Doctor = doctor,
                    Pet = pet,
                    User = user
                });

                _context.Appointments.Add(new Appointment
                {
                    Date = DateTime.Now.AddDays(5),
                    AppointmentObs = "Nao faz xixi",
                    Owner = owner,
                    Doctor = doctor,
                    Pet = pet,
                    User = user
                });

                _context.Appointments.Add(new Appointment
                {
                    Date = DateTime.Now.AddDays(5),
                    AppointmentObs = "Comichão na cabeça",
                    Owner = owner,
                    Doctor = doctor,
                    Pet = pet,
                    User = user
                });

            }

            await _context.SaveChangesAsync();
        }

    }
}

