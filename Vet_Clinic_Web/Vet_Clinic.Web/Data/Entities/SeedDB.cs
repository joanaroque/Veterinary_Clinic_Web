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
        //private Random _random;
        private readonly IUserHelper _userHelper;


        public SeedDB(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            //_random = new Random();
            _userHelper = userHelper;

        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            var user = await _userHelper.GetUserByEmailAsync("joana.ramos.roque@formandos.cinel.pt");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Joana",
                    LastName = "Roque",
                    Email = "joana.ramos.roque@formandos.cinel.pt",
                    UserName = "joana.ramos.roque@formandos.cinel.pt",
                    PhoneNumber = "156456456"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder.");
                }

            }
        }
    }
}
