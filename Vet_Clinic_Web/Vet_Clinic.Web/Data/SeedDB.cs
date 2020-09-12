using Microsoft.AspNetCore.Identity;

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

        public SeedDB(DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

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
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Agent");
            await _userHelper.CheckRoleAsync("Doctor");
            await _userHelper.CheckRoleAsync("Customer");
        }

        private async Task FillHistoriesAsync()
        {
            if (!_context.Histories.Any())
            {
                _context.Histories.Add(new History
                {
                    Pet = _context.Pets.FirstOrDefault(),
                    ServiceType = _context.ServiceTypes.FirstOrDefault(),
                    Description = "Otite e orelhas sujas",
                    CreateDate = DateTime.Now.AddDays(-1)
                });
                await _context.SaveChangesAsync();
            }
        }

        private async Task FillPetsAsync()
        {
            if (!_context.Pets.Any())
            {
                _context.Pets.Add(new Pet
                {
                    Name = "If",
                    Owner = _context.Owners.FirstOrDefault(),
                    Specie = _context.Species.FirstOrDefault(),
                    Breed = "Mongrel",
                    ImageUrl = ("~/images/Pets/e8ee3f9c-04a9-4677-b093-844009c5cda2.jpg"),
                    Gender = "Male",
                    Weight = 8,
                    Sterilization = true,
                    DateOfBirth = DateTime.Now.AddYears(-1),
                    Histories = _context.Histories.ToList(),
                    Appointments = _context.Appointments.ToList()
                });

                _context.Pets.Add(new Pet
                {
                    Name = "Else",
                    Owner = _context.Owners.FirstOrDefault(),
                    Specie = _context.Species.FirstOrDefault(),
                    Breed = "Berner Sennenhund",
                    ImageUrl = ("~/images/Pets/bernesse.jpg"),
                    Gender = "Male",
                    Weight = 00,
                    Sterilization = true,
                    DateOfBirth = DateTime.Now.AddYears(-7),
                    Histories = _context.Histories.ToList(),
                    Appointments = _context.Appointments.ToList()
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
                    PhoneNumber = "965214744",
                    Email = "alex@gmail.pt",
                    TIN = "9011401",
                    ImageUrl = ("~/images/Assistants/v.jpg"),
                    Address = "Rua Zeca Afonso",
                    DateOfBirth = DateTime.Now.AddYears(-35)
                });


                _context.Assistants.Add(new Assistant
                {
                    Name = "Mariana",
                    LastName = "Pedro",
                    PhoneNumber = "965474544",
                    Email = "pedro@gmail.pt",
                    TIN = "125555",
                    ImageUrl = ("~/images/Assistants/v1.jpg"),
                    Address = "Rua Pedro Afonso",
                    DateOfBirth = DateTime.Now.AddYears(-25)
                });


                _context.Assistants.Add(new Assistant
                {
                    Name = "Margarida",
                    LastName = "Miguel",
                    PhoneNumber = "874514747",
                    Email = "miguel@gmail.pt",
                    TIN = "21011",
                    ImageUrl = ("~/images/Assistants/v3.jpg"),
                    Address = "Rua Mario Afonso",
                    DateOfBirth = DateTime.Now.AddYears(-30)
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
                var user = new User
                {
                    Address = "Rua da programação",
                    Email = "lala@yopmail.com",
                    FirstName = "Joana",
                    LastName = "Ramos",
                    PhoneNumber = "965258888",
                    UserName = "lala@yopmail.com"
                };

                _context.Owners.Add(new Owner
                {
                    User = user,
                    Pets = _context.Pets.ToList(),
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
                    PhoneNumber = "98564881",
                    Email = "antoni@gmail.com",
                    Specialty = "Ortopedia",
                    MedicalLicense = "45345",
                    WorkStart = "9",
                    WorkEnd = "13",
                    ObsRoom = "8",
                    TIN = "4342",
                    ImageUrl = ("~/images/Doctors/41d3c742-fb4d-4124-a975-96fb0ceaafd9.jpg"),
                    Address = "Rua do médico",
                    DateOfBirth = DateTime.Now.AddYears(-58)
                });

                _context.Doctors.Add(new Doctor
                {
                    Name = "Manuela",
                    LastName = "Anotonio",
                    PhoneNumber = "98564882",
                    Email = "manu@gmail.com",
                    Specialty = "Dermatologia",
                    MedicalLicense = "45115",
                    WorkStart = "9",
                    WorkEnd = "13",
                    ObsRoom = "2",
                    TIN = "125111",
                    ImageUrl = ("~/images/Doctors/d66a4c2a-6016-425c-bca9-ae8d39ec7f5a.jpg"),
                    Address = "Rua do Manuel",
                    DateOfBirth = DateTime.Now.AddYears(-40)
                });

                _context.Doctors.Add(new Doctor
                {
                    Name = "Patricio",
                    LastName = "Pires",
                    PhoneNumber = "98512345",
                    Email = "pati@gmail.com",
                    Specialty = "Clinica Geral",
                    MedicalLicense = "12321",
                    WorkStart = "9",
                    WorkEnd = "13",
                    ObsRoom = "3",
                    TIN = "121",
                    ImageUrl = ("~/images/Doctors/41d3c742-fb4d-4124-a975-96fb0ceaafd9.jpg"),
                    Address = "Rua do Caderno",
                    DateOfBirth = DateTime.Now.AddYears(-45)
                });

                _context.Doctors.Add(new Doctor
                {
                    Name = "Vasco",
                    LastName = "Fernandes",
                    PhoneNumber = "965444444",
                    Email = "vasco@gmail.com",
                    Specialty = "Ortodoncia",
                    MedicalLicense = "125444",
                    WorkStart = "13",
                    WorkEnd = "18",
                    ObsRoom = "4",
                    TIN = "4511",
                    ImageUrl = ("~/images/Doctors/e570f689-fb8b-4e2f-96f4-f114cdb7ee15.jpg"),
                    Address = "Rua do Cão",
                    DateOfBirth = DateTime.Now.AddYears(-58)
                });

                _context.Doctors.Add(new Doctor
                {
                    Name = "Margarida",
                    LastName = "Manuela",
                    PhoneNumber = "961248777",
                    Email = "magio@gmail.com",
                    Specialty = "Clinica Geral",
                    MedicalLicense = "45345",
                    WorkStart = "14",
                    WorkEnd = "19",
                    ObsRoom = "5",
                    TIN = "2144",
                    ImageUrl = ("~/images/Doctors/a47b4f0d-08be-4466-a84f-d17e57e41fda.jpg"),
                    Address = "Rua da Avendia",
                    DateOfBirth = DateTime.Now.AddYears(-58)
                });

                _context.Doctors.Add(new Doctor
                {
                    Name = "Joaquina",
                    LastName = "Fonseca",
                    PhoneNumber = "962025802",
                    Email = "joca@gmail.com",
                    Specialty = "Ortopedia",
                    MedicalLicense = "1210",
                    WorkStart = "9",
                    WorkEnd = "13",
                    ObsRoom = "1",
                    TIN = "201",
                    ImageUrl = ("~/images/Doctors/1d58f58a-5f9a-4c4f-9fc5-c75dd340005c.jpg"),
                    Address = "Rua da Secretária",
                    DateOfBirth = DateTime.Now.AddYears(-58)
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task FillAppointmentAsync()
        {
            if (!_context.Appointments.Any())
            {
                _context.Appointments.Add(new Appointment
                {
                    CreateDate = DateTime.Now.AddDays(2),
                    AppointmentObs = "vacinas vacinas vacinas",
                    Owner = _context.Owners.FirstOrDefault(),
                    Doctor = _context.Doctors.FirstOrDefault(),
                    Pet = _context.Pets.FirstOrDefault(),
                    CreatedBy = _context.Users.FirstOrDefault()
                });

                _context.Appointments.Add(new Appointment
                {
                    CreateDate = DateTime.Now.AddDays(4),
                    AppointmentObs = "Otites",
                    Owner = _context.Owners.FirstOrDefault(),
                    Doctor = _context.Doctors.FirstOrDefault(),
                    Pet = _context.Pets.FirstOrDefault(),
                    CreatedBy = _context.Users.FirstOrDefault()
                });

                _context.Appointments.Add(new Appointment
                {
                    CreateDate = DateTime.Now.AddDays(5),
                    AppointmentObs = "Nao faz xixi",
                    Owner = _context.Owners.FirstOrDefault(),
                    Doctor = _context.Doctors.FirstOrDefault(),
                    Pet = _context.Pets.FirstOrDefault(),
                    CreatedBy = _context.Users.FirstOrDefault()
                });

                _context.Appointments.Add(new Appointment
                {
                    CreateDate = DateTime.Now.AddDays(5),
                    AppointmentObs = "Comichão na cabeça",
                    Owner = _context.Owners.FirstOrDefault(),
                    Doctor = _context.Doctors.FirstOrDefault(),
                    Pet = _context.Pets.FirstOrDefault(),
                    CreatedBy = _context.Users.FirstOrDefault()
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}

