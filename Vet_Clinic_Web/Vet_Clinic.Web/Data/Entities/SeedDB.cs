using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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


        public SeedDB(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;

        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await CheckRoles();

            var admin = await CheckUserAsync("Joana", "Roque", "joanatpsi@gmail.com", "123456789", "Rua Ola", "Admin");
            var customer = await CheckUserAsync("Joana", "Ramos", "joana.ramos.roque@formandos.cinel.pt", "123456789", "Rua Adeus", "Customer");
            await CheckSpeciesAsync();
            await CheckServiceTypesAsync();
            await CheckOwnerAsync(customer);
            await CheckDoctorAsync(customer);
            await CheckAssistantAsync(customer);
            await CheckAdminAsync(admin);
            await CheckAgendasAsync();
            await CheckPetsAsync();

        }

        private async Task CheckPetsAsync()
        {
            if (!_context.Pets.Any())
            {
                var owner = _context.Owners.FirstOrDefault();
                var petType = _context.Species.FirstOrDefault();
                AddPet("If", owner, petType, "Rafeiro");
                AddPet("Else", owner, petType, "Berner Sennenhund");
                await _context.SaveChangesAsync();
            }
        }


        private void AddPet(string name, Owner owner, Specie specie, string breed)
        {
            _context.Pets.Add(new Pet
            {
                DateOfBirth = DateTime.Now.AddYears(-2),
                Name = name,
                Owner = owner,
                Specie = specie,
                Breed = breed

            });
        }


        private async Task CheckSpeciesAsync()
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

        private async Task CheckAssistantAsync(User user)
        {
            if (!_context.Assistants.Any())
            {
                _context.Assistants.Add(new Assistant { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckRoles()
        {
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Customer");
        }

        private async Task CheckAdminAsync(User user)
        {
            if (!_context.Admins.Any())
            {
                _context.Admins.Add(new Admin { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private async Task<User> CheckUserAsync(
           string firstName,
           string lastName,
           string email,
           string phone,
           string address,
           string role)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUSerToRoleAsync(user, role);

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }

        private async Task CheckServiceTypesAsync()
        {
            if (!_context.ServiceTypes.Any())
            {
                _context.ServiceTypes.Add(new ServiceType { Name = "Appointment" });
                _context.ServiceTypes.Add(new ServiceType { Name = "Emergency" });
                _context.ServiceTypes.Add(new ServiceType { Name = "Vaccination" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckOwnerAsync(User user)
        {
            if (!_context.Owners.Any())
            {
                _context.Owners.Add(new Owner { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckDoctorAsync(User user)
        {
            if (!_context.Doctors.Any())
            {
                _context.Doctors.Add(new Doctor { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckAgendasAsync()
        {
            if (!_context.Appointments.Any())
            {
                var initialDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
                var finalDate = initialDate.AddYears(1);
                while (initialDate < finalDate)
                {
                    if (initialDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        var finalDate2 = initialDate.AddHours(10);
                        while (initialDate < finalDate2)
                        {
                            _context.Appointments.Add(new Appointment
                            {
                                AppointmentSchedule = initialDate,
                                IsAvailable = true
                            });

                            initialDate = initialDate.AddMinutes(30);
                        }

                        initialDate = initialDate.AddHours(14);
                    }
                    else
                    {
                        initialDate = initialDate.AddDays(1);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }

}

