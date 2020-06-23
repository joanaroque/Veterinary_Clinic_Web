using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Doctor> Doctors { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }
}
