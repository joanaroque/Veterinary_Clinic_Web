using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vet_Clinic.Web.Data.Entities;

namespace Vet_Clinic.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Customer> Customers { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //quando o modelo for criado, vou desativar a Cascade delete rule
        //    modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        //}

    }
}
