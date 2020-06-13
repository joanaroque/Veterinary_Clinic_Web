using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;
        //private Random _random;


        public SeedDB(DataContext context)
        {
            _context = context;
            //_random = new Random();
        }

        public async Task SeedAsync() 
        {
            await _context.Database.EnsureCreatedAsync();

        }
    }
}
