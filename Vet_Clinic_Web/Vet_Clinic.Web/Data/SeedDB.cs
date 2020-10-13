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
        private readonly UserManager<User> _userManager;

        public SeedDB(DataContext context,
            IUserHelper userHelper,
             UserManager<User> userManager)
        {
            _context = context;
            _userHelper = userHelper;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await CheckOrCreateRoles();

            await FillUserAsync();
            await FillSpeciesAsync();
            await FillOwnerAsync();
            await FillDoctorAsync();
            await CheckAppointmentsAsync();
            await FillPetsAsync();
            await FillAssistantAsync();

        }

        private async Task CheckOrCreateRoles()
        {
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Agent");
            await _userHelper.CheckRoleAsync("Doctor");
            await _userHelper.CheckRoleAsync("Customer");
        }

        /// <summary>
        /// populate pets with information
        /// </summary>
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
                    Appointments = _context.Appointments.ToList()
                });

                await _context.SaveChangesAsync();
            }
        }


        /// <summary>
        /// populate species with information
        /// </summary>
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


        /// <summary>
        /// populate assistants with information
        /// </summary>
        private async Task FillAssistantAsync()
        {
            if (!_context.Assistants.Any())
            {
                var user1 = new User
                {
                    Address = "Rua Zeca Afonso",
                    Email = "alexxxx@yopmail.com",
                    FirstName = "Alexandra",
                    LastName = "Ramos",
                    PhoneNumber = "96595088",
                    UserName = "alexxxx@yopmail.com",
                    EmailConfirmed = true
                };

                _context.Assistants.Add(new Assistant
                {
                    User = user1,
                    ImageUrl = ("~/images/Assistants/v.jpg"),
                    DateOfBirth = DateTime.Now.AddYears(-35)
                });

                var user2 = new User
                {
                    Address = "Rua Pedro Afonso",
                    Email = "afooonsooo@yopmail.com",
                    FirstName = "Mariana",
                    LastName = "Pedro",
                    PhoneNumber = "96590088",
                    UserName = "afooonsooo@yopmail.com",
                    EmailConfirmed = true
                };

                _context.Assistants.Add(new Assistant
                {
                    User = user2,
                    ImageUrl = ("~/images/Assistants/v1.jpg"),
                    DateOfBirth = DateTime.Now.AddYears(-25)
                });

                await _context.SaveChangesAsync();
            }
        }


        /// <summary>
        /// populate user with information,
        /// assigns him a role
        /// </summary>
        private async Task FillUserAsync()
        {
            var user = await _userHelper.GetUserByEmailAsync("joana.ramos.roque@formandos.cinel.pt");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Joana",
                    LastName = "Roque",
                    Email = "joana.ramos.roque@formandos.cinel.pt",
                    UserName = "joana.ramos.roque@formandos.cinel.pt",
                    PhoneNumber = "965214744",
                    Address = "Rua da Programação",
                    EmailConfirmed = true

                };

                await _userHelper.AddUserAsync(user, "gfdGF545++");

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


        /// <summary>
        /// populate owner with information
        /// </summary>
        private async Task FillOwnerAsync()
        {

            if (!_context.Owners.Any())
            {
                var user1 = new User
                {
                    Address = "Rua da programação",
                    Email = "lala@yopmail.com",
                    FirstName = "Joana",
                    LastName = "Ramos",
                    PhoneNumber = "96595888",
                    UserName = "lala@yopmail.com",
                    EmailConfirmed = true
                };

                await _userHelper.AddUserAsync(user1, "gfdGF545++");

                var token1 = await _userHelper.GenerateEmailConfirmationTokenAsync(user1);
                await _userHelper.ConfirmEmailAsync(user1, token1);

                var isInRole1 = await _userHelper.IsUserInRoleAsync(user1, "Customer");

                if (!isInRole1)
                {
                    await _userHelper.AddUSerToRoleAsync(user1, "Customer");
                }

                _context.Owners.Add(new Owner
                {
                    User = user1,
                    Pets = _context.Pets.ToList(),
                });

            }

            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// populate doctor with information
        /// </summary>
        private async Task FillDoctorAsync()
        {
            if (!_context.Doctors.Any())
            {
                var user1 = new User
                {
                    Address = "Rua da medicos",
                    Email = "sfddsf@yopmail.com",
                    FirstName = "Anotnio",
                    LastName = "Henrriques",
                    PhoneNumber = "96592888",
                    UserName = "sfddsf@yopmail.com",
                    EmailConfirmed = true
                };

                _context.Doctors.Add(new Doctor
                {
                    User = user1,
                    Specialty = "Ortopedia",
                    MedicalLicense = "45345",
                    WorkStart = 9,
                    WorkEnd = 13,
                    ObsRoom = "8",
                    ImageUrl = ("~/images/Doctors/41d3c742-fb4d-4124-a975-96fb0ceaafd9.jpg"),
                    DateOfBirth = DateTime.Now.AddYears(-58)

                });

                var user2 = new User
                {
                    Address = "Rua do Manuel",
                    Email = "manu@gmail.com",
                    FirstName = "Manuela",
                    LastName = "Anotonio",
                    PhoneNumber = "96512888",
                    UserName = "manu@gmail.com",
                    EmailConfirmed = true
                };

                _context.Doctors.Add(new Doctor
                {
                    User = user2,
                    Specialty = "Dermatologia",
                    MedicalLicense = "45115",
                    WorkStart = 9,
                    WorkEnd = 13,
                    ObsRoom = "2",
                    ImageUrl = ("~/images/Doctors/d66a4c2a-6016-425c-bca9-ae8d39ec7f5a.jpg"),
                    DateOfBirth = DateTime.Now.AddYears(-40)
                });


                await _context.SaveChangesAsync();
            }
        }


        /// <summary>
        /// get the first owner, the first doctor and the first user
        /// </summary>
        private async Task CheckAppointmentsAsync()
        {
            if (!_context.Appointments.Any())
            {
                var owner = _context.Owners.FirstOrDefault();
                var doctor = _context.Doctors.FirstOrDefault();
                var user = _context.Users.FirstOrDefault();
                FillAppointmentAsync(owner, doctor, user);
                await _context.SaveChangesAsync();
            }

        }


        /// <summary>
        /// populate Appointment with information
        /// </summary>
        private void FillAppointmentAsync(Owner owner, Doctor doctor, User user)
        {
            _context.Appointments.Add(new Appointment
            {
                ScheduledDate = DateTime.Now.AddMonths(2),
                CreateDate = DateTime.Now.AddDays(-2),
                AppointmentObs = "vacinas vacinas vacinas",
                Owner = owner,
                Doctor = doctor,
                Pet = owner.Pets.FirstOrDefault(),
                CreatedBy = user
            });

            _context.Appointments.Add(new Appointment
            {
                ScheduledDate = DateTime.Now.AddMonths(2),
                CreateDate = DateTime.Now.AddDays(-2),
                AppointmentObs = "Otites",
                Owner = owner,
                Doctor = doctor,
                Pet = owner.Pets.FirstOrDefault(),
                CreatedBy = user
            });

            _context.Appointments.Add(new Appointment
            {
                ScheduledDate = DateTime.Now.AddMonths(2),
                CreateDate = DateTime.Now.AddDays(-2),
                AppointmentObs = "Nao faz xixi",
                Owner = owner,
                Doctor = doctor,
                Pet = owner.Pets.FirstOrDefault(),
                CreatedBy = user
            });

            _context.Appointments.Add(new Appointment
            {
                ScheduledDate = DateTime.Now.AddMonths(2),
                CreateDate = DateTime.Now.AddDays(-2),
                AppointmentObs = "Comichão na cabeça",
                Owner = owner,
                Doctor = doctor,
                Pet = owner.Pets.FirstOrDefault(),
                CreatedBy = user
            });

            _context.Appointments.Add(new Appointment
            {
                ScheduledDate = DateTime.Now.AddMonths(-15),
                CreateDate = DateTime.Now.AddDays(2),
                AppointmentObs = "Não come há 1 dia",
                Owner = owner,
                Doctor = doctor,
                Pet = owner.Pets.FirstOrDefault(),
                CreatedBy = user
            });

            _context.Appointments.Add(new Appointment
            {
                ScheduledDate = DateTime.Now.AddMonths(-13),
                CreateDate = DateTime.Now.AddDays(6),
                AppointmentObs = "Não quer andar",
                Owner = owner,
                Doctor = doctor,
                Pet = owner.Pets.FirstOrDefault(),
                CreatedBy = user
            });

            _context.Appointments.Add(new Appointment
            {
                ScheduledDate = DateTime.Now.AddMonths(-20),
                CreateDate = DateTime.Now.AddDays(-2),
                AppointmentObs = "Queixas da perna direita",
                Owner = owner,
                Doctor = doctor,
                Pet = owner.Pets.FirstOrDefault(),
                CreatedBy = user
            });
        }
    }
}


